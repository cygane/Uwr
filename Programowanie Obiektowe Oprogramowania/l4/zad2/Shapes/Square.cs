namespace Shapes
{
    public class Square : IShape
    {
        public double SideLength { get; }

        public Square(double sideLength)
        {
            SideLength = sideLength;
        }

        public double Area() => SideLength * SideLength;
    }
}
