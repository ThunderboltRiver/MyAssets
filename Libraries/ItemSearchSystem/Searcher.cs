using System;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;
namespace ItemSearchSystem
{
    public abstract class Searcher
    {
        private readonly ReactiveCollection<GameObject> _currentRecognition = new();//Searchの結果の格納場所

        /// <summary>
        /// ゲームワールドからゲームオブジェクトを認識する方法.Search内部で呼び出す.
        /// </summary>
        /// <returns>認識したいゲームオブジェクトの配列</returns>
        protected abstract GameObject[] Recognize();
        public IReadOnlyReactiveCollection<GameObject> CurrentRecognition => _currentRecognition;//認識結果を公開

        /// <summary>
        /// ゲームワールド内のオブジェクトを検索し,その結果を_currentRecognitionとする.
        /// 以前の検索結果はすべてDesearchする.
        /// </summary>
        public void Search()
        {
            Search(gameObject => false);
        }

        /// <summary>
        /// ゲームワールド内のオブジェクトを検索し,その結果を_currentRecognitionとする.
        /// 以前の検索結果のうちpreservingConditionを満たすものは保持しておき,それ以外はDesearchする
        /// </summary>
        /// <param name="preservingCondition">以前の検索結果の内で今回も保持するゲームオブジェクトの条件</param>
        public void Search(Func<GameObject, bool> preservingCondition)
        {
            GameObject[] recognition = Recognize().Where(gameObject => gameObject.TryGetComponent<ISearchable>(out _)).ToArray();
            Desearch(gameObject => !recognition.Contains(gameObject) && !preservingCondition(gameObject));
            Array.ForEach(recognition.Where(gameObject => !_currentRecognition.Contains(gameObject)).ToArray(), (GameObject gameObject) =>
            {
                _currentRecognition.Add(gameObject);
                gameObject.GetComponent<ISearchable>().OnSearch();
            });
        }
        /// <summary>
        /// CurrentRecognitionからゲームオブジェクトを消してOnDesearchを呼び出す
        /// </summary>
        /// <param name="condition">削除するゲームオブジェクトの条件</param>
        public void Desearch(Func<GameObject, bool> condition)
        {
            if (_currentRecognition != null)
            {
                Array.ForEach(_currentRecognition.Where(gameObject => gameObject.TryGetComponent<ISearchable>(out _) && condition(gameObject)).ToArray()
                , gameObject =>
                {
                    _currentRecognition.Remove(gameObject);
                    gameObject.GetComponent<ISearchable>().OnDesearch();
                });
            }
        }
    }

    /// <summary>
    /// Transformの位置を中心とする球体内に入るゲームオブジェクトを認識するSearcher
    /// </summary>
    public class SphereSearcher : Searcher
    {
        public Transform Transform { get; }
        public float Radius { get; }

        protected override GameObject[] Recognize()
        {
            return Physics.OverlapSphere(Transform.position, Radius).Select(collider => collider.gameObject).ToArray();
        }
        public SphereSearcher(Transform transform, float radius)
        {
            Transform = transform;
            Radius = radius;
        }

    }
}