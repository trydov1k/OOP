namespace MyPhotoshop;

public abstract class PixelFilter : IFilter
{
    public abstract ParameterInfo[] GetParameters();
    public abstract Pixel ProcessPixel(Pixel original, double[] parameters);

    public Photo Process(Photo original, double[] parameters)
    {
        var result = new Photo(original.Width, original.Height);
        
        for (var x = 0; x < result.Width; x++)
        for (var y = 0; y < result.Height; y++)
        {
            result[x, y] = ProcessPixel(original[x,y], parameters);
        }
        return result;
    }
}