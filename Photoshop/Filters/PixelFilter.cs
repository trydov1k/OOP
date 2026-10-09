using MyPhotoshop.Parameters;

namespace MyPhotoshop;

public class PixelFilter<TParameters>: ParametrizedFilter<TParameters>
where TParameters : IParameters, new()
{
    private readonly string _name;
    private readonly Func<Pixel, TParameters, Pixel> _processPixel;

    public PixelFilter(string name, Func<Pixel, TParameters, Pixel> processPixel)
    {
        _name = name;
        _processPixel = processPixel;
    }
    
    public override Photo Process(Photo original, TParameters parameters)
    {
        var result = new Photo(original.Width, original.Height);
        
        for (var x = 0; x < result.Width; x++)
        for (var y = 0; y < result.Height; y++)
        {
            result[x, y] = _processPixel(original[x,y], parameters);
        }
        return result;
    }

    public override string ToString()
    {
        return _name;
    }
}