namespace Incapsulation.Weights;

public class Indexer
{
    private readonly double[] storage;
    private readonly int start;
    public readonly int Length;
    public Indexer(double[] original, int start, int length)
    {
        if (start < 0 || length < 0 || start + length > original.Length) 
            throw new ArgumentException();
        storage = original;
        this.start = start;
        Length = length;
    }
    
    public double this[int i]
    {
        get
        {
            CheckIndex(i);
            return storage[i + start];
        }
        set
        {
            CheckIndex(i);
            storage[i + start] = value;
        }
    }

    private void CheckIndex(int i)
    {
        if (i < 0 || i + start > Length)
            throw new IndexOutOfRangeException();
    }
}