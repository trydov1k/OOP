namespace MyPhotoshop.Parameters;

public class EmptyParameters : IParameters
{
    public ParameterInfo[] GetDescription()
    {
        return [];
    }

    public void Parse(double[] parameters)
    {
        
    }
}