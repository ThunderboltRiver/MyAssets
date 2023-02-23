using System;
using UniRx;
using UniRx.Triggers;
using UniRx.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;


namespace General.Views
{
    /// <summary>
    /// ボタンが押される -> 押されている時間timeを計測 
    ///                  While time < 一定時間  
    ///                      if 指がボタンの領域内にない -> ストリーム終了
    ///                      elseif PointerUp -> ストリーム終了
    ///                  -> ストリーム終了
    /// </summary>
    public class PointerUpUI : MonoBehaviour
    {
        protected ObservableEventTrigger eventTrigger;
        private Subject<PointerEventData> _IsPressed = new();
        private Subject<long> _WhilePressingIsNotTimeOut = new();
        private Subject<PointerEventData> _IsPointerOutOfArea = new();
        private Subject<PointerEventData> _IsPointerUpInAreaNotTimeOut = new();
        private Subject<PointerEventData> _IsPointerUpAfterTimeOut = new();

        [SerializeField] int waiteSeconts;
        public IObservable<PointerEventData> IsPressed => _IsPressed;
        public IObservable<long> WhilePressingIsNotTimeOut => _WhilePressingIsNotTimeOut;
        public IObservable<PointerEventData> IsPointerOutOfArea => _IsPointerOutOfArea;
        public IObservable<PointerEventData> IsPointerUpInAreaNotTimeOut => _IsPointerUpInAreaNotTimeOut;
        public IObservable<PointerEventData> IsPointerUpAfterTimeOut => _IsPointerUpAfterTimeOut;


        void Awake()
        {
            eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();
        }
        void Start()
        {
            //SubScribeしないとDo(OnNext)は実行されない
            //SelectManyは発生したイベントごとにストリームを作成する
            //TakeをするとOnNext()でTakeする前の値が次のObservableに渡される.
            // OnCompleteは次のObservableに電番する.



            eventTrigger.OnPointerDownAsObservable()
            .Do((eventData) =>
            {
                Debug.Log($"Pressed");
                _IsPressed.OnNext(eventData);
            })
            .SelectMany(
                eventData => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .TakeWhile(time => time < waiteSeconts)
                .Do((time) =>
                {
                    _WhilePressingIsNotTimeOut.OnNext(time);
                })
                .DoOnCompleted(() =>
                {
                    Debug.Log($"TimeOut");
                })
                .TakeUntil(eventTrigger
                    .OnDragAsObservable()
                    .Where(eventData => !eventData.hovered.Contains(gameObject))
                    .Do((e) =>
                    {
                        Debug.Log($"Pointer Exit: {e}");
                        _IsPointerOutOfArea.OnNext(e);
                    }))
                .TakeUntil(eventTrigger.OnPointerUpAsObservable()
                    .Where(eventData => eventData.hovered.Contains(gameObject))
                    .Do((e) =>
                    {
                        Debug.Log($"Pointer Up: {e}");
                        _IsPointerUpInAreaNotTimeOut.OnNext(e);
                    }))
            )
            .Subscribe((time) =>
            {
                eventTrigger.OnPointerUpAsObservable()
                .Where(eventData => waiteSeconts <= time + 1)
                .Take(1)
                .TakeUntil(eventTrigger.OnDragAsObservable()
                    .Where(eventData => !eventData.hovered.Contains(gameObject))
                )
                .Subscribe((e) =>
                {
                    _IsPointerUpAfterTimeOut.OnNext(e);
                }).AddTo(this);
                Debug.Log($"Pressing......{time}");
            })
            .AddTo(this);
        }



        protected virtual void OnStart() { }
    }
}