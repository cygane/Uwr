// interfejs dla fabryki
namespace Shapes
{
    public interface IShapeFactoryWorker
    {
        bool CanCreate(string shapeName);
        IShape CreateShape(params object[] parameters);
    }
}
