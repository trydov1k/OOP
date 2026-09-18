namespace MyPhotoshop;

public class Pixel
{
    private double r;
    private double g;
    private double b;

    public double R
    {
        get => r;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentException();
            r = value;
        }
    }
    
    public double G
    {
        get => g;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentException();
            g = value;
        }
    }
    
    public double B
    {
        get => b;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentException();
            b = value;
        }
    }
}