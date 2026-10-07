namespace Inheritance.DataStructure;

public class Category(string productName, MessageType messageType, MessageTopic messageTopic) : IComparable
{
    private readonly string _productName = productName;
    private readonly MessageType _messageType = messageType;
    private readonly MessageTopic _messageTopic = messageTopic;

    public override bool Equals(object? obj)
    {
        if (obj is not Category category)
            return false;
        
        return category._productName == _productName
            && category._messageType == _messageType
            && category._messageTopic == _messageTopic;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_productName, _messageType, _messageTopic);
    }

    public override string ToString()
    {
        return $"{_productName}.{_messageType}.{_messageTopic}";
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return -1;  // Почему мы обязаны кидать -1, а не ошибку?
        
        if (obj is not Category category)
            throw new ArgumentException($"Object {nameof(obj)} is not {nameof(Category)}");

        var compareWithProduct = String.Compare(_productName, category._productName, StringComparison.Ordinal);

        if (compareWithProduct != 0)
            return compareWithProduct;
        
        var compareWithType = _messageType.CompareTo(category._messageType);
        
        if (compareWithType != 0)
            return compareWithType;
        
        return _messageTopic.CompareTo(category._messageTopic);
    }
    
    public static bool operator >=(Category c1, Category c2)
    {
        return c1.CompareTo(c2) >= 0;
    }
    
    public static bool operator <=(Category c1, Category c2)
    {
        return c1.CompareTo(c2) <= 0;
    }
    
    public static bool operator >(Category c1, Category c2)
    {
        return c1.CompareTo(c2) > 0;
    }
    
    public static bool operator <(Category c1, Category c2)
    {
        return c1.CompareTo(c2) < 0;
    }
}