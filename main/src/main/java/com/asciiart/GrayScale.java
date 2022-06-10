package com.asciiart;

import java.awt.*;

public class GrayScale{
    public static Color toGrayColor(Color color){
        int red = color.getRed();
        int green = color.getGreen();
        int blue = color.getBlue();
        int avg = (int)(red + green + blue)/3;
        return new Color(avg, avg, avg);
    }

    public static Color[][] toGrayColors(Color[][] colors){
        Color[][] grayColors = new Color[colors.length][];
        for(int y = 0; y < colors.length; y++){
            grayColors[y] = new Color[colors[y].length];
            for(int x = 0; x < colors.length; x++){
                grayColors[y][x] = toGrayColor(colors[y][x]);
            }
        }
        return grayColors;
    }
}
