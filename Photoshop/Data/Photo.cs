namespace MyPhotoshop;

public class Photo
{
	public readonly int Width;
	public readonly int Height;
	public readonly Pixel[,] Data;
	
	public Photo(int width, int height)
	{
		Width = width;
		Height = height;
		Data = new Pixel[width, height];
		
		for (var x = 0; x < width; x++)
		for (var y = 0; y < height; y++)
			Data[x, y] = new Pixel();
	}
}