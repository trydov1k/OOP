namespace MyPhotoshop;

public static class Convertors
{
    public static Photo Bitmap2Photo(Bitmap bmp)
    {
        var photo = new Photo(bmp.Width, bmp.Height);

        for (var x = 0; x < bmp.Width; x++)
        for (var y = 0; y < bmp.Height; y++)
        {
            var pixel = bmp.GetPixel(x, y);
            photo.Data[x, y] = new Pixel();
            photo.Data[x, y].R = (double)pixel.R / 255;
            photo.Data[x, y].G = (double)pixel.G / 255;
            photo.Data[x, y].B = (double)pixel.B / 255;
        }
        return photo;
    }

    static int ToChannel(double val)
    {
        if (val is < 0 or > 1)
            throw new Exception($"Wrong channel value {val} (the value must be between 0 and 1");
        return (int)(val * 255);
    }

    public static Bitmap Photo2Bitmap(Photo photo)
    {
        var bmp = new Bitmap(photo.Width, photo.Height);
        for (var x = 0; x < bmp.Width; x++)
        for (var y = 0; y < bmp.Height; y++)
            bmp.SetPixel(x, y, Color.FromArgb(
                ToChannel(photo.Data[x, y].R),
                ToChannel(photo.Data[x, y].G),
                ToChannel(photo.Data[x, y].B)));

        return bmp;
    }
}