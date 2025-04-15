using Xunit;
using zad3;

public class zad3Tests
{

    [Fact]
    public void DepthTreeVisitor_CalculatesCorrectDepth()
    {
        // Given
        Tree root = new TreeNode()
        {
            Left = new TreeNode()
            {
                Left = new TreeLeaf() { Value = 1 },
                Right = new TreeLeaf() { Value = 2 },
            },
            Right = new TreeLeaf() { Value = 3 }
        };

        var depthVisitor = new DepthTreeVisitor();

        // When
        depthVisitor.Visit(root);

        // Then
        Assert.Equal(3, depthVisitor.MaxDepth); // Głębokość drzewa to 3
    }

    [Fact]
    public void DepthTreeVisitor_ReturnsZeroForEmptyTree()
    {
        // Given
        Tree root = null; // Puste drzewo

        var depthVisitor = new DepthTreeVisitor();

        // When
        depthVisitor.Visit(root);

        // Then
        Assert.Equal(0, depthVisitor.MaxDepth); // Brak węzłów, głębokość powinna wynosić 0
    }


    [Fact]
    public void DepthTreeVisitor_CalculatesCorrectDepthWithSingleLeaf()
    {
        // Given
        Tree root = new TreeLeaf() { Value = 5 };

        var depthVisitor = new DepthTreeVisitor();

        // When
        depthVisitor.Visit(root);

        // Then
        Assert.Equal(1, depthVisitor.MaxDepth); // Głębokość powinna wynosić 1 (tylko liść)
    }
}

