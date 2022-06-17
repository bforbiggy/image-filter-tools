namespace image_filter_tools;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class Edging {
    /// <summary>
    /// A function that measures the difference (aka intensity) between two colors.
    /// A positive result means an increase in intensity and vice versa.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private static int deltaColor(Color from, Color to) {
        int fromIntensity = from.R + from.G + from.B;
        int toIntensity = to.R + to.G + to.B;
        return (toIntensity - fromIntensity) / 3;
    }

    public static Color[,] convertColors(Color[,] colors) {
        for (int y = 0; y < colors.GetLength(0) - 1; y++) {
            for (int x = 0; x < colors.GetLength(1) - 1; x++) {
                int deltaX = deltaColor(colors[y, x], colors[y, x + 1]);
                int deltaY = deltaColor(colors[y, x], colors[y + 1, x]);
                int c = Math.Clamp(Math.Abs(deltaX) + Math.Abs(deltaY), 0, 255);
                colors[y, x] = Color.FromArgb(c, c, c);
            }
        }
        return colors;
    }

    public static void convertImg(Bitmap img) {
        Color[,] colors = new Color[img.Height, img.Width];
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                int deltaX = deltaColor(colors[y, x], colors[y, x + 1]);
                int deltaY = deltaColor(colors[y, x], colors[y + 1, x]);
                int c = Math.Clamp(Math.Abs(deltaX) + Math.Abs(deltaY), 0, 255);
                img.SetPixel(y, x, Color.FromArgb(c, c, c));
            }
        }
    }

    public static void writeConverted(Bitmap img, Stream output) {
        convertImg(img);
        img.Save(output, ImageFormat.Jpeg);
    }
}