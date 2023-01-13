namespace MainGameScene.Model
{
    public interface IInputHandler<T>
    {
        public void HandleInput(T input);
    }
}