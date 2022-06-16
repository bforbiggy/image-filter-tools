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
        Color[][] edgeColors = new Color[colors.Length][];
        for (int y = 0; y < colors.Length; y++) {
            edgeColors[y] = new Color[colors[y].Length];
            for (int x = 0; x < colors.Length; x++) {
                if (x != 0 && y != 0 && x != colors.Length - 1 && y != colors.Length - 1) {
                    int c = (deltaColor(colors[y][x], colors[y][x + 1]) + deltaColor(colors[y][x], colors[y + 1][x])) / 2;
                    edgeColors[y][x] = Color.FromArgb(c, c, c);
                }
                else
                    edgeColors[y][x] = colors[y][x];
            }
        }
        return edgeColors;
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

    // public static void Main(String[] args) {
    //     Bitmap img = new Bitmap("pog.png");
    //     Stream output = new FileStream("output.jpg", FileMode.Create);
    //     writeConverted(img, output);
    // }
}