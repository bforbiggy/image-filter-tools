namespace image_filter_tools;

using System.Drawing;

public class AsciiScale {
    public const string ASCII_SIMPLE = " .:-=+*#%@";
    public const string ASCII_DETAILED = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,""^`'.";

    public static char convertColor(Color color, bool detailed = false) {
        string ASCII = detailed ? ASCII_DETAILED : ASCII_SIMPLE;
        int index = (int)(GrayScale.convertColor(color).R / 255.0 * (ASCII.Length - 1));
        return ASCII[index];
    }

    public static char[][] convertColors(Color[][] colors, bool detailed = false) {
        char[][] text = new char[colors.Length][];
        for (int y = 0; y < colors.Length; y++) {
            text[y] = new char[colors[y].Length];
            for (int x = 0; x < colors[0].Length; x++) {
                text[y][x] = convertColor(colors[y][x], detailed);
            }
        }
        return text;
    }

    public static void writeConverted(Bitmap img, Stream output, bool detailed = false) {
        StreamWriter sw = new StreamWriter(output);

        double xInc = 1;
        double yInc = 2.5;
        for (double y = 0; y < img.Height; y += yInc) {
            for (double x = 0; x < img.Width; x += xInc) {
                Color color = img.GetPixel((int)x, (int)y);
                sw.Write(AsciiScale.convertColor(color, detailed));
            }
            sw.Write("\n");
        }

        sw.Close();
    }
}
