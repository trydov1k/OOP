using MyPhotoshop.Parameters;

namespace MyPhotoshop;

public abstract class ParametrizedFilter<TParameters>(TParameters parameters) : IFilter
where TParameters : IParameters
{
    public readonly TParameters Parameters = parameters;

    public ParameterInfo[] GetParameters() => Parameters.GetDescription();

    public abstract Photo Process(Photo photo, TParameters parameters);

    public Photo Process(Photo photo, double[] parameters)
    {
        Parameters.Parse(parameters);
        return Process(photo, Parameters);
    }
}