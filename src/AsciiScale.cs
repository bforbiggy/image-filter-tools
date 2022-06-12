namespace image_to_ascii;

using System.Drawing;
using System.Text;

public class AsciiScale {
    public const String ASCII_SIMPLE = " .:-=+*#%@";
    public const String ASCII_DETAILED = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B @$";

    public static char convertColor(Color color) {
        int gray = color.R;
        int index = (int) (gray / 255.0 * (ASCII_SIMPLE.Length-1));
        return ASCII_SIMPLE[index];
    }

    public static char[][] convertColors(Color[][] colors) {
        char[][] text = new char[colors.Length][];
        for (int y = 0; y < colors.Length; y++) {
            text[y] = new char[colors[y].Length];
            for (int x = 0; x < colors[0].Length; x++) {
                text[y][x] = convertColor(colors[y][x]);
            }
        }
        return text;
    }

    public static void writeConverted(Bitmap img, Stream output)
    {
        StreamWriter sw = new StreamWriter(output);

        double xInc = 1;
        double yInc = 2.5;
        for (double y = 0; y < img.Height; y += yInc)
        {
            for (double x = 0; x < img.Width; x += xInc)
            {
                Color color = img.GetPixel((int)x, (int)y);
                sw.Write(AsciiScale.convertColor(color));
            }
            sw.Write("\n");
        }

        sw.Close();
    }
}
