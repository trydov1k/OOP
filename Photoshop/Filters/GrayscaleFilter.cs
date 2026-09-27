namespace MyPhotoshop;

public class GrayscaleFilter : PixelFilter
{
    public override ParameterInfo[] GetParameters()
    {
        return [];
    }
    
    public override string ToString()
    {
        return "Оттенки серого";
    }

    public override Pixel ProcessPixel(Pixel original, double[] parameters)
    {
        var brightness = (original.R + original.B + original.G) / 3;
        return new Pixel(brightness, brightness, brightness);
    }
}