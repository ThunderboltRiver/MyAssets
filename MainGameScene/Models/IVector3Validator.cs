
namespace MainGameScene.Model
{
    public interface IVectorValidator<TVector>
    {
        bool IsValidVector(TVector input);
    }
}