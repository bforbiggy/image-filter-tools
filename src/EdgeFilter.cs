namespace image_filter_tools;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class EdgeFilter {
    private static int deltaColor(Color from, Color to) {
        from = GrayScale.convertColor(from);
        to = GrayScale.convertColor(to);
        return Math.Abs(from.R - to.R);
    }

    public static Color[][] convertColors(Color[][] colors) {
        for (int y = 0; y < colors.Length - 1; y++) {
            for (int x = 0; x < colors[0].Length - 1; x++) {
                int c = (deltaColor(colors[y][x], colors[y][x + 1]) + deltaColor(colors[y][x], colors[y + 1][x])) / 2;
                colors[y][x] = Color.FromArgb(c, c, c);
            }
        }
        return colors;
    }

    public static void writeConverted(Bitmap img, Stream output) {
        Color[][] colors = new Color[img.Height][];
        for (int y = 0; y < img.Height; y++) {
            colors[y] = new Color[img.Width];
            for (int x = 0; x < img.Height; x++) {
                colors[y][x] = img.GetPixel(x, y);
            }
        }
        colors = convertColors(colors);

        for (int x = 0; x < img.Width; x++) {
            for (int y = 0; y < img.Height; y++) {
                img.SetPixel(x, y, colors[y][x]);
            }
        }
        img.Save(output, ImageFormat.Jpeg);
    }
}