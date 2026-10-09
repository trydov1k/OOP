namespace Generics.Robots;

public interface IRobotAI<out TRobotCommand>
{
	public TRobotCommand GetCommand();
}

public interface IDevice<in TRobotCommand>
{
	public string ExecuteCommand(TRobotCommand command);
}

public class ShooterAI : IRobotAI<ShooterCommand>
{
	int _counter = 1;

	public ShooterCommand GetCommand()
	{
		return ShooterCommand.ForCounter(_counter++);
	}
}

public class BuilderAI : IRobotAI<BuilderCommand>
{
	int _counter = 1;

	public BuilderCommand GetCommand()
	{
		return BuilderCommand.ForCounter(_counter++);
	}
}

public class Mover : IDevice<IMoveCommand>
{
	public string ExecuteCommand(IMoveCommand command)
	{
		if (command == null)
			throw new ArgumentException();
		return $"MOV {command.Destination.X}, {command.Destination.Y}";
	}
}

public class ShooterMover : IDevice<IShooterMoveCommand>
{
	public string ExecuteCommand(IShooterMoveCommand command)
	{
		if (command == null)
			throw new ArgumentException();
		var hide = command.ShouldHide ? "YES" : "NO";
		return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
	}
}

public class Robot<TRobotCommand>
{
	private readonly IRobotAI<TRobotCommand> _ai;
	private readonly IDevice<TRobotCommand> _device;

	public Robot(IRobotAI<TRobotCommand> ai, IDevice<TRobotCommand> executor)
	{
		_ai = ai;
		_device = executor;
	}

	public IEnumerable<string> Start(int steps)
	{
		for (var i = 0; i < steps; i++)
		{
			var command = _ai.GetCommand();
			if (command == null)
				break;
			yield return _device.ExecuteCommand(command);
		}
	}
}

public static class Robot
{
	public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
	{
		return new Robot<TCommand>(ai, executor);
	}
}