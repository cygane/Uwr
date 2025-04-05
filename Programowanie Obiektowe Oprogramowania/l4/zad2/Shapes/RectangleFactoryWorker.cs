namespace Shapes
{
    public class RectangleFactoryWorker : IShapeFactoryWorker
    {
        public bool CanCreate(string shapeName)
        {
            return shapeName.Equals("Rectangle", StringComparison.OrdinalIgnoreCase);
        }

        public IShape CreateShape(params object[] parameters)
        {
            if (parameters.Length != 2 || parameters[0] is not double width || parameters[1] is not double height)
            {
                throw new ArgumentException("Invalid parameters for creating a Rectangle.");
            }

            return new Rectangle(width, height);
        }
    }
}
