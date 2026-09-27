using MyPhotoshop.Parameters;

namespace MyPhotoshop;

public abstract class ParametrizedFilter(IParameters parameters) : IFilter
{
    public readonly IParameters Parameters = parameters;

    public ParameterInfo[] GetParameters() => Parameters.GetDescription();

    public abstract Photo Process(Photo photo, IParameters parameters);

    public Photo Process(Photo photo, double[] parameters)
    {
        Parameters.Parse(parameters);
        return Process(photo, Parameters);
    }
}