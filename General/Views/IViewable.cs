using UniRx;
using UnityEngine;
namespace General.Views
{
    public interface IViewable<TValue>
    {
        IReadOnlyReactiveProperty<TValue> View { get; }
    }
}