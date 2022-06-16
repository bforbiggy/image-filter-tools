namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class GrayScale {
    public static Color convertColor(Color color) {
        int avg = (color.R + color.G + color.B) / 3;
        return Color.FromArgb(avg, avg, avg);
    }

    public static Color[,] convertColors(Color[,] colors) {
        Color[,] grayColors = new Color[colors.GetLength(0), colors.GetLength(1)];
        for (int y = 0; y < colors.Length; y++) {
            for (int x = 0; x < colors.Length; x++) {
                grayColors[y, x] = convertColor(colors[y, x]);
            }
        }
        return grayColors;
    }

    public static void writeConverted(Bitmap img, Stream output) {
        for (int x = 0; x < img.Width; x++) {
            for (int y = 0; y < img.Height; y++) {
                Color color = GrayScale.convertColor(img.GetPixel(x, y));
                img.SetPixel(x, y, color);
            }
        }

        img.Save(output, ImageFormat.Jpeg);
    }
}