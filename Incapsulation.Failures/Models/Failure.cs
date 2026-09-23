namespace Incapsulation.Failures.Models;

public class Failure
{
    public FailureType Type { get; private set; }

    public bool IsSerious => Type == FailureType.UnexpectedShutdown
                             || Type == FailureType.HardwareFailures;
    
    public Failure(int typeNumber)
    {
        Type = (FailureType)typeNumber;
    }
}