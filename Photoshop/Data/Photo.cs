namespace MyPhotoshop;

public class Photo
{
	public int Width { get; set; }
	public int Height { get; set; }
	public Pixel[,] Data { get; private set; }
	
	public Photo(int width, int height)
	{
		Width = width;
		Height = height;
		Data = new Pixel[width, height];
	}
}