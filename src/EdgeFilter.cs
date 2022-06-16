namespace image_filter_tools;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class EdgeFilter {
    /// <summary>
    /// A function that measures the difference (aka intensity) between two colors.
    /// A positive result means an increase in intensity and vice versa.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private static int deltaColor(Color from, Color to) {
        from = GrayScale.convertColor(from);
        to = GrayScale.convertColor(to);
        return to.R - from.R;
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

    public static void writeConverted(Bitmap img, Stream output) {
        Color[,] colors = new Color[img.Height, img.Width];
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                colors[y, x] = img.GetPixel(x, y);
            }
        }
        colors = convertColors(colors);

        for (int x = 0; x < img.Width; x++) {
            for (int y = 0; y < img.Height; y++) {
                img.SetPixel(x, y, colors[y, x]);
            }
        }
        img.Save(output, ImageFormat.Jpeg);
    }
}