using System.Collections.ObjectModel;
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
    ///                      elseif PointerUp -> Subscribe()
    ///                  -> ストリーム終了
    /// </summary>
    public class PointerUpUI : MonoBehaviour
    {
        protected ObservableEventTrigger eventTrigger;

        [SerializeField] int waiteSeconts;

        public IObservable<PointerEventData> GetEventData()
        {
            return eventTrigger.OnPointerUpAsObservable();
        }


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
            })
            .SelectMany(
                eventData => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .TakeWhile(time => time < waiteSeconts).DoOnCompleted(() =>
                {
                    Debug.Log($"TimeOut");
                })
                .TakeUntil(eventTrigger
                    .OnDragAsObservable()
                    .Where(eventData => !eventData.hovered.Contains(gameObject))
                    .Do((e) =>
                    {
                        Debug.Log($"Pointer Drag: {e}");
                    }))
                .TakeUntil(eventTrigger.OnPointerUpAsObservable()
                    .Do((e) =>
                    {
                        Debug.Log($"Pointer Up: {e}");
                    }))
                .DoOnCompleted(() =>
                    {
                        Debug.Log($"Completed");
                    })

            )
            .Subscribe((e) =>
            {
                if (e < waiteSeconts)
                {
                    Debug.Log(e);
                }

            })
            .AddTo(this);
        }



        protected virtual void OnStart() { }
    }
}