using MyPhotoshop.Parameters;

namespace MyPhotoshop;

public abstract class ParametrizedFilter<TParameters> : IFilter
where TParameters : IParameters, new()
{
    public ParameterInfo[] GetParameters() => new TParameters().GetDescription();

    public abstract Photo Process(Photo photo, TParameters parameters);

    public Photo Process(Photo photo, double[] parameters)
    {
        var _parameters = new TParameters();
        _parameters.Parse(parameters);
        return Process(photo, _parameters);
    }
}