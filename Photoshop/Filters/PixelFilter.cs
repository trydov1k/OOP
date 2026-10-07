using MyPhotoshop.Parameters;

namespace MyPhotoshop;

public abstract class PixelFilter<TParameters>(TParameters parameters) : ParametrizedFilter<TParameters>(parameters)
where TParameters : IParameters
{
    public abstract Pixel ProcessPixel(Pixel original, TParameters parameters);

    public override Photo Process(Photo original, TParameters parameters)
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