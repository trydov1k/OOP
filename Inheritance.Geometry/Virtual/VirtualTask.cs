using System.Drawing;

namespace Inheritance.Geometry.Virtual;

public abstract class Body
{
	public Vector3 Position { get; }

	protected Body(Vector3 position)
	{
		Position = position;
	}

	public abstract bool ContainsPoint(Vector3 point);

	public abstract RectangularCuboid GetBoundingBox();
}

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
	}

	public override bool ContainsPoint(Vector3 point)
	{
		var vector = point - Position;
		var length2 = vector.GetLength2();
		return length2 <= Radius * Radius;
	}

	public override RectangularCuboid GetBoundingBox()
	{
		var size = Radius * 2;
		return new RectangularCuboid(Position, size, size, size);
	}
}

public class RectangularCuboid : Body
{
	public double SizeX { get; }
	public double SizeY { get; }
	public double SizeZ { get; }

	public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
	{
		SizeX = sizeX;
		SizeY = sizeY;
		SizeZ = sizeZ;
	}

	public override bool ContainsPoint(Vector3 point)
	{
		var minPoint = new Vector3(
			Position.X - SizeX / 2,
			Position.Y - SizeY / 2,
			Position.Z - SizeZ / 2);
		var maxPoint = new Vector3(
			Position.X + SizeX / 2,
			Position.Y + SizeY / 2,
			Position.Z + SizeZ / 2);

		return point >= minPoint && point <= maxPoint;
	}

	public override RectangularCuboid GetBoundingBox()
	{
		return this;
	}
}

public class Cylinder : Body
{
	public double SizeZ { get; }

	public double Radius { get; }

	public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
	{
		SizeZ = sizeZ;
		Radius = radius;
	}

	public override bool ContainsPoint(Vector3 point)
	{
		var vectorX = point.X - Position.X;
		var vectorY = point.Y - Position.Y;
		var length2 = vectorX * vectorX + vectorY * vectorY;
		var minZ = Position.Z - SizeZ / 2;
		var maxZ = minZ + SizeZ;

		return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
	}

	public override RectangularCuboid GetBoundingBox()
	{
		var size = Radius * 2;
		return new  RectangularCuboid(Position, size, size, SizeZ);
	}
}

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}

	public override bool ContainsPoint(Vector3 point)
	{
		return Parts.Any(body => body.ContainsPoint(point));
	}

	public override RectangularCuboid GetBoundingBox()
	{
		Vector3 minPoint = new(double.MaxValue, double.MaxValue, double.MaxValue), 
			maxPoint = new(double.MinValue, double.MinValue, double.MinValue);
		foreach (var box in Parts.Select(body => body.GetBoundingBox()))
		{
			minPoint = MinPoint(minPoint, box);
			maxPoint = MaxPoint(maxPoint, box);
			// Вынес методы нахождения точек минимума и максимума в отдельные методы
			// По другому вижу только если использовать LINQ, а мне не хочется
			// его использовать, так как тогда мы пройдемся по коллекции 6 раз 
		}
		
		var center = new Vector3(
			(minPoint.X + maxPoint.X) / 2, 
			(minPoint.Y + maxPoint.Y) / 2, 
			(minPoint.Z + maxPoint.Z) / 2);
		
		var sizeX = maxPoint.X - minPoint.X;
		var sizeY = maxPoint.Y - minPoint.Y;
		var sizeZ = maxPoint.Z - minPoint.Z;
    
		return new RectangularCuboid(center, sizeX, sizeY, sizeZ);
	}

	private static Vector3 MinPoint(Vector3 minPoint, RectangularCuboid rectangularCuboid)
	{
		var minX = Math.Min(minPoint.X, rectangularCuboid.Position.X - rectangularCuboid.SizeX / 2);
		var minY = Math.Min(minPoint.Y, rectangularCuboid.Position.Y - rectangularCuboid.SizeY / 2);
		var minZ = Math.Min(minPoint.Z, rectangularCuboid.Position.Z - rectangularCuboid.SizeZ / 2);
		return new Vector3(minX, minY, minZ);
	}
	
	private static Vector3 MaxPoint(Vector3 maxPoint, RectangularCuboid rectangularCuboid)
	{
		var maxX = Math.Max(maxPoint.X, rectangularCuboid.Position.X + rectangularCuboid.SizeX / 2);
		var maxY = Math.Max(maxPoint.Y, rectangularCuboid.Position.Y + rectangularCuboid.SizeY / 2);
		var maxZ = Math.Max(maxPoint.Z, rectangularCuboid.Position.Z + rectangularCuboid.SizeZ / 2);
		return new Vector3(maxX, maxY, maxZ);
	}
}