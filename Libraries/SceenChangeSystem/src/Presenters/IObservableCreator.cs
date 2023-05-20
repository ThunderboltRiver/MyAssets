using System;
namespace SceenChangeSystem.Presenters
{
    public interface IDisposableCreator<T>
    {
        IDisposable Create(T data);
    }

}