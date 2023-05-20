using System;
using System.Linq;

namespace MyObservables
{
    public class ObservableLinkedQueue<T> where T : struct
    {
        //中心となるObservableEnumeratorの配列
        private readonly ObservableEnumerator<T>[] _coreArray;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maximumCapacity">最大容量</param>
        public ObservableLinkedQueue(int maximumCapacity)
        {
            //配列の初期化
            _coreArray = Enumerable.Range(0, maximumCapacity).Select(_ => new ObservableEnumerator<T>()).ToArray();

            //配列の要素をつなげる
            int i = 0;
            while (i < maximumCapacity - 1)
            {
                _coreArray[i].Next = _coreArray[i + 1];
                i++;
            }
        }

        /// <summary>
        /// このキューの最大容量
        /// </summary>
        public int Length => _coreArray.Length;

        /// <summary>
        /// このキューに値を追加する
        /// </summary>
        /// <param name="value">追加したい値</param>
        public void Enqueue(T value)
        {
            _coreArray.First().NextValue(value);
        }

        /// <summary>
        /// このキューから値を取り出す.Enqueueした順に取り出される
        /// Enqueueされていない場合は例外を投げる
        /// </summary>
        /// <returns>キューの一番先頭の値</returns>
        public T Dequeue()
        {
            if (_coreArray.All(x => !x.HasValue))
            {
                throw new InvalidOperationException("キューに値が入っていません");
            }
            return (T)_coreArray.Where(x => x.HasValue).Last().CurrentValue;

        }

        /// <summary>
        /// 指定したインデックスに対応するObservableEnumeratorが保持する値を取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T? GetCurrentValue(int index)
        {
            IndexValidation(index);
            return _coreArray[index].CurrentValue;
        }

        public IObservable<T> ValueChangeAsObservable(int index)
        {
            IndexValidation(index);
            return _coreArray[index].ValueChangeAsObservable;
        }


        /// <summary>
        /// 指定したインデックスのObservableEnumeratorが値を保持しているかどうかを取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasValue(int index)
        {
            IndexValidation(index);
            return _coreArray[index].HasValue;
        }

        /// <summary>
        /// 指定したインデックスが有効なインデックスかどうかを検証する
        /// </summary>
        /// <param name="index"></param>
        private void IndexValidation(int index)
        {
            if (index < 0 || index >= _coreArray.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

    }
}