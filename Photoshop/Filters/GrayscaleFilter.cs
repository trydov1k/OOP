using MyPhotoshop.Parameters;

namespace MyPhotoshop;

public class GrayscaleFilter : PixelFilter<GrayscaleParameters>
{
    public override string ToString()
    {
        return "Оттенки серого";
    }

    public override Pixel ProcessPixel(Pixel original, GrayscaleParameters parameters)
    {
        var brightness = (original.R + original.B + original.G) / 3;
        return new Pixel(brightness, brightness, brightness);
    }
}