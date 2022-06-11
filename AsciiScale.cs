using System.Drawing;

public class AsciiScale {
    public const String ASCII_SIMPLE = " .:-=+*#%@";
    public const String ASCII_DETAILED = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B @$";

    public static char toAsciiChar(Color color) {
        int gray = color.R;
        int index = (int) (gray / 255.0 * (ASCII_SIMPLE.Length-1));
        return ASCII_SIMPLE[index];
    }

    public static char[][] toAsciiText(Color[][] colors) {
        char[][] text = new char[colors.Length][];
        for (int y = 0; y < colors.Length; y++) {
            text[y] = new char[colors[y].Length];
            for (int x = 0; x < colors[0].Length; x++) {
                text[y][x] = toAsciiChar(colors[y][x]);
            }
        }
        return text;
    }
}
