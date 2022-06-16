namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class BlurFilter {
    public static Color avgAroundPoint(Color[,] colors, int x, int y) {
        int red = 0;
        int green = 0;
        int blue = 0;
        for (int col = x - 1; col <= x + 1; col++) {
            for (int row = y - 1; row <= y + 1; row++) {
                red += colors[row, col].R;
                green += colors[row, col].G;
                blue += colors[row, col].B;
            }
        }
        return Color.FromArgb(red / 9, green / 9, blue / 9);
    }

    private static Color avgAroundPoint(Bitmap img, int x, int y) {
        int red = 0;
        int green = 0;
        int blue = 0;
        for (int col = x - 1; col <= x + 1; col++) {
            for (int row = y - 1; row <= y + 1; row++) {
                Color color = img.GetPixel(col, row);
                red += color.R;
                green += color.G;
                blue += color.B;
            }
        }
        return Color.FromArgb(red / 9, green / 9, blue / 9);
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
        for (int x = 0; x < img.Width; x++) {
            for (int y = 0; y < img.Height; y++) {
                Color color;
                if (x != 0 && x != img.Width - 1 && y != 0 && y != img.Height - 1)
                    color = BlurFilter.avgAroundPoint(img, x, y);
                else
                    color = img.GetPixel(x, y);
                img.SetPixel(x, y, color);
            }
        }

        img.Save(output, ImageFormat.Jpeg);
    }
}