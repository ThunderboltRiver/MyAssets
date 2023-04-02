using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


namespace ItemSearchSystem
{
    public class Taker
    {
        private ReactiveCollection<GameObject> _currentSelections = new();
        public IReadOnlyReactiveCollection<GameObject> CurrentSelections => _currentSelections;
        public readonly int maxSelection;

        public Taker()
        {
            maxSelection = 1;
        }
        public Taker(int maxSelection)
        {
            this.maxSelection = maxSelection;
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

        /// <summary>
        /// 渡されたゲームオブジェクトの配列からTakableなもののみをCurrentSelectionsとして扱う.以前のSelections内のゲームオブジェクトで入力配列にないものは選択解除する.
        /// </summary>
        /// <param name="gameObjects">入力されるゲームオブジェクトの配列</param>
        /// <returns></returns>
        public IEnumerable<GameObject> Select(IEnumerable<GameObject> gameObjects)
        {

            var nextSelections = gameObjects.Where(SelectionFilter).Take(maxSelection).ToArray();
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
            if (_currentSelections != null)
            {
                Array.ForEach(_currentSelections.Where(gameObject => gameObject.TryGetComponent<ITakable>(out _) && condition(gameObject)).ToArray()
                , gameObject =>
                    {
                        _currentSelections.Remove(gameObject);
                        gameObject.GetComponent<ITakable>().OnDeselected();
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
        /// CurrentSelectionsの中からインデックスで指定されたゲームオブジェクト内のコンポーネントからITakableを探してOnTakenを実行する
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool Take(int index, out object obj, out GameObject go)
        {
            try
            {
                GameObject gameObject = CurrentSelections.ElementAtOrDefault(index);
                if (gameObject.TryGetComponent(out ITakable takable))
                {
                    Deselect((GameObject selected) => selected == gameObject);
                    takable.OnTaken(TakeDirection(gameObject));
                    go = gameObject;
                    obj = takable;
                    return true;
                };
                go = null;
                obj = null;
                return false;
            }
            catch
            {
                go = null;
                obj = null;
                return false;
            }

        }


        public bool Take(int index, out object obj)
        {
            return Take(index, out obj, out _);
        }

        public bool Take(int index, out GameObject go)
        {
            return Take(index, out _, out go);
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