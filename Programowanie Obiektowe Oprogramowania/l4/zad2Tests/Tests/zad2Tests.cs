using Shapes;
using Xunit;

namespace zad2Tests
{
    public class zad2Tests
    {
        [Fact]
        public void ShouldCreateSquareWhenSquareIsRegistered()
        {
            var factory = new ShapeFactory();
            factory.RegisterWorker(new SquareFactoryWorker());

            var square = factory.CreateShape("Square", 5);

            Assert.IsType<Square>(square);
            Assert.Equal(25, square.Area());
        }

        [Fact]
        public void ShouldCreateRectangleWhenRectangleIsRegistered()
        {
            var factory = new ShapeFactory();
            factory.RegisterWorker(new RectangleFactoryWorker());

            var rectangle = factory.CreateShape("Rectangle", 3, 5);

            Assert.IsType<Rectangle>(rectangle);
            Assert.Equal(15, rectangle.Area());
        }

        [Fact]
        public void ShouldThrowExceptionWhenShapeIsNotRegistered()
        {
            var factory = new ShapeFactory();

            Assert.Throws<InvalidOperationException>(() => factory.CreateShape("Triangle", 3, 5, 8));
        }

        [Fact]
        public void ShouldThrowExceptionWhenInvalidParametersForSquare()
        {
            var factory = new ShapeFactory();
            factory.RegisterWorker(new SquareFactoryWorker());

            Assert.Throws<ArgumentException>(() => factory.CreateShape("Square", 3, 4)); 
        }

        [Fact]
        public void ShouldThrowExceptionWhenInvalidParametersForRectangle()
        {
            var factory = new ShapeFactory();
            factory.RegisterWorker(new RectangleFactoryWorker());

            Assert.Throws<ArgumentException>(() => factory.CreateShape("Rectangle", 3)); 
        }
    }
}
