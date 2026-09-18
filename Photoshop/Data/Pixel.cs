namespace MyPhotoshop;

public struct Pixel
{
    private double _r;
    private double _g;
    private double _b;

    public Pixel(double r, double g, double b)
    {
        R = r;
        G = g;
        B = b;
    }

    public double R
    {
        get => _r;
        set => _r = CheckValue(value);
    }
    
    public double G
    {
        get => _g;
        set => _g = CheckValue(value);
    }
    
    public double B
    {
        get => _b;
        set => _b = CheckValue(value);
    }

    private double CheckValue(double value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentException();
        return value;
    }

    public static double Trim(double value) 
        => value < 0 ? 0 : value > 1 ? 1 : value;
}