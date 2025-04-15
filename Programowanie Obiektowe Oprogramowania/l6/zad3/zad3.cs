using System;
namespace zad3;

//czy dla implementacji z accept byloby wiecej pracy?

public abstract class Tree {}

public class TreeNode : Tree
{
    public Tree Left { get; set; }
    public Tree Right { get; set; }
}

public class TreeLeaf : Tree
{
    public int Value { get; set; }
}

public abstract class TreeVisitor
{
    // Ta metoda nie jest potrzebna, ale ułatwia korzystanie z Visitora
    public void Visit(Tree tree)
    {
        if (tree is TreeNode)
            this.VisitNode((TreeNode)tree);
        if (tree is TreeLeaf)
            this.VisitLeaf((TreeLeaf)tree);
    }

    public virtual void VisitNode(TreeNode node)
    {
        // Tu wiedza o odwiedzaniu struktury
        if (node != null)
        {
            this.Visit(node.Left);
            this.Visit(node.Right);
        }
    }

    public virtual void VisitLeaf(TreeLeaf leaf) {}
}

public class DepthTreeVisitor : TreeVisitor
{
    public int MaxDepth { get; private set; } = 0;

    private int currentDepth = 0;

    public override void VisitNode(TreeNode node)
    {
        if(node != null){
            currentDepth++;
            if (currentDepth > MaxDepth){
                MaxDepth = currentDepth;
            }

            base.VisitNode(node); 
            currentDepth--;
        }
    }

    public override void VisitLeaf(TreeLeaf leaf)
    {
        currentDepth++; 
        if (currentDepth > MaxDepth){
            MaxDepth = currentDepth;
        }

        base.VisitLeaf(leaf); 
        currentDepth--;
    }
}
