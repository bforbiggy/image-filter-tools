namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class BlurFilter {
    public static Color avgAroundPoint(Color[,] colors, int x, int y) {
        int r = 0, g = 0, b = 0, count = 0;
        int rowmax = Math.Min(x + 1, colors.GetLength(0) - 1);
        int colmax = Math.Min(y + 1, colors.GetLength(1) - 1);
        for (int row = Math.Max(0, x - 1); row <= rowmax; row++) {
            for (int col = Math.Max(0, y - 1); col <= colmax; col++) {
                Color color = colors[row, col];
                r += color.R;
                g += color.G;
                b += color.B;
                count++;
            }
        }
        return Color.FromArgb(r / count, g / count, b / count);
    }

    public static Color[,] convertColors(Color[,] colors) {
        Color[,] grayColors = new Color[colors.GetLength(0), colors.GetLength(1)];
        for (int y = 0; y < colors.Length; y++) {
            for (int x = 0; x < colors.GetLength(1); x++) {
                if (x != 0 && x != colors.GetLength(1) && y >= 0 && y < colors.GetLength(0))
                    grayColors[y, x] = avgAroundPoint(colors, x, y);
                else
                    grayColors[y, x] = colors[y, x];
            }
        }
        return grayColors;
    }

    public static void writeConverted(Bitmap img, Stream output) {
        // Gets blurred version of image
        Color[,] colors = new Color[img.Height, img.Width];
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                colors[y, x] = img.GetPixel(x, y);
            }
        }
        convertColors(colors);

        // Writes blurred version of image to bitmap
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Height; x++) {
                img.SetPixel(x, y, colors[y, x]);
            }
        }
        img.Save(output, ImageFormat.Jpeg);
    }
}