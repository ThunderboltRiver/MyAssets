using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


namespace ItemSearchSystem
{
    public class Taker
    {
        public IReadOnlyReactiveCollection<GameObject> CurrentSelections => _currentSelections;
        private ReactiveCollection<GameObject> _currentSelections = new();
        /// <summary>
        /// 渡されたゲームオブジェクトの配列からTakableなもののみをCurrentSelectionsとして扱う.以前のSelections内のゲームオブジェクトで入力配列にないものは選択解除する.
        /// </summary>
        /// <param name="gameObjects">入力されるゲームオブジェクトの配列</param>
        /// <returns></returns>
        public IEnumerable<GameObject> Select(IEnumerable<GameObject> gameObjects)
        {

            var nextSelections = gameObjects.Where(SelectionFilter).ToArray();
            Deselect(gameObject => !nextSelections.Contains(gameObject));
            Array.ForEach(nextSelections, (GameObject gameObject) =>
            {
                if (gameObject.TryGetComponent(out ITakable takable))
                {
                    _currentSelections.Add(gameObject);
                    takable.OnSelected();
                };
            });
            return CurrentSelections;

        }
        /// <summary>
        /// 条件に当てはまるものを選択解除し,CurrentSelectionsから削除する
        /// </summary>
        /// <param name="condition">GameObjectに対する条件式</param>
        public void Deselect(Func<GameObject, bool> condition)
        {
            if (_currentSelections.Count > 0)
            {
                Array.ForEach(_currentSelections.Where(condition).ToArray(), (GameObject gameObject) =>
                    {
                        if (gameObject.TryGetComponent(out ITakable takable))
                        {
                            _currentSelections.Remove(gameObject);
                            takable.OnDeselected();
                        };
                    }
                );
            }

        }
        /// <summary>
        /// すべての選択状態を解除する
        /// </summary>
        public void Deselect()
        {
            Deselect((GameObject gameObject) => true);
        }
        /// <summary>
        /// 選択する対象のフィルター。デフォルトでは常にtrueを返す
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected virtual bool SelectionFilter(GameObject gameObject)
        {
            return true;
        }

        /// <summary>
        /// Takeを実行する方向を計算するための仮想メソッド.デフォルトではゼロベクトルを常に返す
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected virtual Vector3 TakeDirection(GameObject gameObject)
        {
            return Vector3.zero;
        }
        public bool Take(int index, out object obj)
        {
            try
            {
                GameObject gameObject = CurrentSelections.ElementAtOrDefault(index);
                if (gameObject.TryGetComponent(out ITakable takable))
                {
                    Deselect((GameObject selected) => selected == gameObject);
                    takable.OnTaken(TakeDirection(gameObject));
                    obj = takable;
                    return true;
                };
                obj = null;
                return false;
            }
            catch
            {
                obj = null;
                return false;
            }

        }

        public bool Take(int index, out GameObject gameObject)
        {
            gameObject = new();
            return true;
        }

        public bool Take(out object obj)
        {
            return Take(0, out obj);
        }
    }

    public class RayCastingTaker : Taker
    {
        private Transform _transform;
        private float _maxDistance;

        private Dictionary<GameObject, Vector3> _takeDirections = new();
        protected override bool SelectionFilter(GameObject gameObject)
        {
            bool result = Physics.Raycast(_transform.position, gameObject.transform.position - _transform.position, out RaycastHit hitInfo, _maxDistance);
            _takeDirections[gameObject] = hitInfo.normal;
            return result;
        }
        protected override Vector3 TakeDirection(GameObject gameObject)
        {
            return _takeDirections.Remove(gameObject, out Vector3 result) ? result : Vector3.zero;
        }

        public RayCastingTaker(Transform transform, float maxDistance)
        {
            _transform = transform;
            _maxDistance = maxDistance;
        }
    }
}