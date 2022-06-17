namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class GrayScale {
    public static Color convertColor(Color color) {
        int avg = (color.R + color.G + color.B) / 3;
        return Color.FromArgb(avg, avg, avg);
    }

    public static Color[,] convertColors(Color[,] colors) {
        for (int y = 0; y < colors.GetLength(0); y++) {
            for (int x = 0; x < colors.GetLength(1); x++) {
                colors[y, x] = convertColor(colors[y, x]);
            }
        }
        return colors;
    }

    public static void writeConverted(Bitmap img, Stream output) {
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                img.SetPixel(x, y, GrayScale.convertColor(img.GetPixel(x, y)));
            }
        }
        img.Save(output, ImageFormat.Jpeg);
    }
}