namespace MainGameScene.Model
{
    public interface IInputable<T>
    {
        public void AcceptInput(T input);
    }
}