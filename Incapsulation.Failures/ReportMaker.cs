namespace Incapsulation.Failures;

public class ReportMaker
{
	public static List<string> FindDevicesFailedBeforeDate(
		DateTime date,
		Failure[] failures,
		Device[] devices,
		DateTime[] times)
	{
		if (failures.Length != devices.Length || devices.Length != times.Length)
			throw new ArgumentException();

		var result = new List<string>();
		
		for (var i = 0; i < failures.Length; i++)
		{
			var failure = failures[i];
			var device = devices[i];
			var time = times[i];
			
			if (failure.IsSerious && time.CompareTo(date) == -1)
				result.Add(device.Name);
		}
		
		return result;
	}
	
	public static List<string> FindDevicesFailedBeforeDateObsolete(
		int day, int month, int year,
		int[] failureTypes, 
		int[] deviceId, 
		object[][] times,
		List<Dictionary<string, object>> devices)
	{
		var _date = new DateTime(year, month, day);
		
		var _failures = ParseFailures(failureTypes);

		var _devices = ParseDevices(devices);

		var _times = ParseTimes(times);
		
		return FindDevicesFailedBeforeDate(_date, _failures, _devices, _times);
	}

	private static Failure[] ParseFailures(int[] failureTypes)
	{
		var failures = new Failure[failureTypes.Length];
		for (var i = 0; i < failureTypes.Length; i++)
			failures[i] = new Failure(failureTypes[i]);
		return failures;
	}

	private static Device[] ParseDevices(List<Dictionary<string, object>> devices)
	{
		var _devices = new Device[devices.Count];
		for (var i = 0; i < devices.Count; i++)
		{
			var device = devices[i];
			_devices[i] = new Device(
				(int)device["DeviceId"], 
				(string)device["Name"]);
		}
		return _devices;
	}

	private static DateTime[] ParseTimes(object[][] times)
	{
		var _times = new DateTime[times.Length];
		for (var i = 0; i < times.Length; i++)
		{
			var time = times[i];
			_times[i] = new DateTime(
				(int)time[2], 
				(int)time[1], 
				(int)time[0]);
		}
		return _times;
	}
}

public enum FailureType
{
	UnexpectedShutdown,
	ShortNonResponding,
	HardwareFailures,
	ConnectionProblems
}

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