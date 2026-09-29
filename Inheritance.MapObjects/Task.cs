namespace Inheritance.MapObjects;

public class Dwelling : IAppropriatingObject
{
	public int Owner { get; set; }
}

public class Mine : IBeatingWithArmyObject,  IConsumingTreasureObject, IAppropriatingObject
{
	public int Owner { get; set; }
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Creeps : IBeatingWithArmyObject, IConsumingTreasureObject
{
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Wolves : IBeatingWithArmyObject
{
	public Army Army { get; set; }
}

public class ResourcePile : IConsumingTreasureObject
{
	public Treasure Treasure { get; set; }
}

public interface IBeatingWithArmyObject
{
	public Army Army { get; set; }
}

public interface IConsumingTreasureObject
{
	public Treasure Treasure { get; set; }
}

public interface IAppropriatingObject
{
	public int Owner { get; set; }
}

public static class Interaction
{
	public static void Make(Player player, object mapObject)
	{
		if (mapObject is IBeatingWithArmyObject beatingWithArmyObject)
		{
			if (!player.CanBeat(beatingWithArmyObject.Army))
			{
				player.Die();
				return;
			}
		}

		if (mapObject is IConsumingTreasureObject consumingTreasureObject)
		{
			player.Consume(consumingTreasureObject.Treasure);
		}
		
		if (mapObject is IAppropriatingObject appropriatingObject)
		{
			appropriatingObject.Owner = player.Id;
		}
	}
}