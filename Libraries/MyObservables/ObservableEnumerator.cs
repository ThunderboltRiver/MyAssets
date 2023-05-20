using System;
using UniRx;
namespace MyObservables
{
    public class ObservableEnumerator<T> where T : struct
    {

        /// <summary>
        /// このクラスの本体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private readonly Subject<T> _selfCore = new Subject<T>();

        /// <summary>
        /// 現在保持する値
        /// </summary>
        /// <value></value>
        public T? CurrentValue { get; private set; } = null;

        public bool HasValue => CurrentValue != null;

        /// <summary>
        /// 自身の値が変更されたときに通知するObservable
        /// </summary>
        public IObservable<T> ValueChangeAsObservable => _selfCore;

        /// <summary>
        /// 次のObservableEnumerator
        /// </summary>
        ObservableEnumerator<T> _next;

        /// <summary>
        /// 次のObservableEnumeratorを設定する.nextがnullの場合のみ設定できる
        /// </summary>
        /// <value></value>
        public ObservableEnumerator<T> Next
        {
            get => _next;
            set => _next ??= value;
        }
        public void NextValue(T value)
        {
            if (HasValue)
            {
                //自身の古い値を次のObservableEnumeratorに渡す
                _next?.NextValue((T)CurrentValue);
            }
            //自身の値を更新する
            CurrentValue = value;
            //更新を通知する
            _selfCore.OnNext(value);
        }

    }

}
