namespace Incapsulation.Failures.Models;

public class Device
{
    public int DeviceId { get; set; }
    public string Name { get; set; }

    public Device(int deviceId, string name)
    {
        DeviceId = deviceId;
        Name = name;
    }
}