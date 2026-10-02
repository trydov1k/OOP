namespace Inheritance.Geometry.Visitor;

public interface IVisitor<out T>
{
	T Visit(Ball ball);
	T Visit(RectangularCuboid rectangularCuboid);
	T Visit(Cylinder cylinder);
	T Visit(CompoundBody compoundBody);
}

public abstract class Body
{
	public Vector3 Position { get; }

	protected Body(Vector3 position)
	{
		Position = position;
	}
	
	public abstract T Accept<T>(IVisitor<T> visitor);
}

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
	}

	public override T Accept<T>(IVisitor<T> visitor)
	{
		return visitor.Visit(this);
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
	
	public override T Accept<T>(IVisitor<T> visitor)
	{
		return visitor.Visit(this);
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
	
	public override T Accept<T>(IVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}
	
	public override T Accept<T>(IVisitor<T> visitor)
	{
		return visitor.Visit(this);
	}
}

public class BoundingBoxVisitor : IVisitor<RectangularCuboid>
{
	public RectangularCuboid Visit(Ball ball)
	{
		var sideSize = ball.Radius * 2;
		return new RectangularCuboid(ball.Position, sideSize, sideSize, sideSize);
	}

	public RectangularCuboid Visit(RectangularCuboid rectangularCuboid)
	{
		return rectangularCuboid;
	}

	public RectangularCuboid Visit(Cylinder cylinder)
	{
		var sideSize = cylinder.Radius * 2;
		return new  RectangularCuboid(cylinder.Position, sideSize, sideSize, cylinder.SizeZ);
	}

	public RectangularCuboid Visit(CompoundBody compoundBody)
	{
		double minX = double.MaxValue, maxX = double.MinValue, 
			minY = double.MaxValue, maxY = double.MinValue, 
			minZ = double.MaxValue, maxZ = double.MinValue;
		foreach (var part in compoundBody.Parts)
		{
			var visitor = new BoundingBoxVisitor();
			var box = part.Accept(visitor);
			minX = MinCoordinate(minX, box.Position.X , box.SizeX);
			minY = MinCoordinate(minY, box.Position.Y , box.SizeY);
			minZ = MinCoordinate(minZ, box.Position.Z , box.SizeZ);
			maxX = MaxCoordinate(maxX, box.Position.X , box.SizeX);
			maxY = MaxCoordinate(maxY, box.Position.Y , box.SizeY);
			maxZ = MaxCoordinate(maxZ, box.Position.Z , box.SizeZ);
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

public class BoxifyVisitor: IVisitor<Body>
{
	public Body Visit(Ball ball)
	{
		return ball.Accept(new BoundingBoxVisitor());
	}

	public Body Visit(RectangularCuboid rectangularCuboid)
	{
		return rectangularCuboid.Accept(new BoundingBoxVisitor());
	}

	public Body Visit(Cylinder cylinder)
	{
		return cylinder.Accept(new BoundingBoxVisitor());
	}

	public Body Visit(CompoundBody compoundBody)
	{
		var newParts = compoundBody.Parts
			.Select(body => body.Accept(this))
			.ToList();
		
		return new CompoundBody(newParts);
	}
}