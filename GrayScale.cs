using System.Drawing;

public class GrayScale{
    public static Color toGrayColor(Color color){
        int avg = (color.R + color.G + color.B)/3;
        return Color.FromArgb(avg, avg, avg);
    }

    public static Color[][] toGrayColors(Color[][] colors){
        Color[][] grayColors = new Color[colors.Length][];
        for(int y = 0; y < colors.Length; y++){
            grayColors[y] = new Color[colors[y].Length];
            for(int x = 0; x < colors.Length; x++){
                grayColors[y][x] = toGrayColor(colors[y][x]);
            }
        }
        return grayColors;
    }
}
