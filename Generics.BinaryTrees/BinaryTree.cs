using System.Collections;

namespace Generics.BinaryTrees;

public class Node<TValue>(TValue value)
{
    public readonly TValue Value = value;
    public Node<TValue>? Left { get; set; }
    public Node<TValue>? Right { get; set; }
}

public class BinaryTree<TValue> : IEnumerable<TValue>
    where TValue : IComparable<TValue>
{
    private Node<TValue>? _root;  // не делал readonly так как теоретически метод Remove (который мы не обязаны пока делать) может делать _root = null
    
    public TValue Value => _root.Value;
    public Node<TValue>? Left => _root?.Left;
    public Node<TValue>? Right => _root?.Right;

    public void Add(TValue value)
    {
        _root = AddRecursive(_root, value);
    }

    private static Node<TValue> AddRecursive(Node<TValue>? node, TValue value)
    {
        if (node is null)
            return new Node<TValue>(value);
        
        if (IsLess(value, node.Value))
            node.Left = AddRecursive(node.Left, value);
        if (IsGrater(value, node.Value))
            node.Right = AddRecursive(node.Right, value);
        
        if (IsEqual(value, node.Value))
            node.Left = AddRecursive(node.Left, value);
        
        return node;
    }

    private static bool IsLess(TValue value1, TValue value2)
        => value1.CompareTo(value2) < 0;

    private static bool IsGrater(TValue value1, TValue value2)
        => value1.CompareTo(value2) > 0;
    
    private static bool IsEqual(TValue value1, TValue value2)
        => value1.CompareTo(value2) == 0;
    
    public IEnumerator<TValue> GetEnumerator()
    {
        return ReturnInOrder(_root).GetEnumerator();
    }

    private static IEnumerable<TValue> ReturnInOrder(Node<TValue>? node)
    {
        if (node is null)
            yield break;
        foreach (var value in ReturnInOrder(node.Left))
            yield return value;
        yield return node.Value;
        foreach (var value in ReturnInOrder(node.Right))
            yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}

public static class BinaryTree
{
    public static BinaryTree<T> Create<T>(params T[] values) where T : IComparable<T>
    {
        var tree = new BinaryTree<T>();
        foreach (var value in values)
            tree.Add(value);
        return tree;
    }
}
