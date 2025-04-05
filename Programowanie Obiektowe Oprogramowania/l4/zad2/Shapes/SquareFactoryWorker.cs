namespace Shapes
{
    public class SquareFactoryWorker : IShapeFactoryWorker
    {
        public bool CanCreate(string shapeName)
        {
            return shapeName.Equals("Square", StringComparison.OrdinalIgnoreCase);
        }

        public IShape CreateShape(params object[] parameters)
        {
            if (parameters.Length != 1 || parameters[0] is not double sideLength)
            {
                throw new ArgumentException("Invalid parameters for creating a Square.");
            }

            return new Square(sideLength);
        }
    }
}
