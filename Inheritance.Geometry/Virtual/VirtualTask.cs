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
		double minX = double.MaxValue, maxX = double.MinValue, 
			minY = double.MaxValue, maxY = double.MinValue, 
			minZ = double.MaxValue, maxZ = double.MinValue;
		foreach (var part in Parts)
		{
			var box = part.GetBoundingBox();
			minX = MinCoordinate(minX, box.Position.X , box.SizeX);
			minY = MinCoordinate(minY, box.Position.Y , box.SizeY);
			minZ = MinCoordinate(minZ, box.Position.Z , box.SizeZ);
			maxX = MaxCoordinate(maxX, box.Position.X , box.SizeX);
			maxY = MaxCoordinate(maxY, box.Position.Y , box.SizeY);
			maxZ = MaxCoordinate(maxZ, box.Position.Z , box.SizeZ);
			// Что сделать, чтобы код соответствовал Dont Repeat Yourself?
			// Я не знаю что можно сделать, чтобы не писать 6 одинаковых строк
		}
		
		var center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
		var sizeX = maxX - minX;
		var sizeY = maxY - minY;
		var sizeZ = maxZ - minZ;
    
		return new RectangularCuboid(center, sizeX, sizeY, sizeZ);
	}

	private double MinCoordinate(double oldMin, double coordinate, double size)
	{
		return Math.Min(oldMin, coordinate - size / 2);
	}

	private double MaxCoordinate(double oldMax, double coordinate, double size)
	{
		return Math.Max(oldMax, coordinate + size / 2);
	}
}