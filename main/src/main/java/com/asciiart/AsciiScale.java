package com.asciiart;

import java.awt.Color;
public class AsciiScale {
    public static final String ASCII_SIMPLE = " .:-=+*#%@";
    public static final String ASCII_DETAILED = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B @$";

    public static char toAsciiChar(Color color){
        int gray = color.getRed();
        int index = (int)(gray/255.0 * ASCII_SIMPLE.length());
        return ASCII_SIMPLE.charAt(index);
    }

    public static char[][] toAsciiText(Color[][] colors) {
        char[][] text = new char[colors.length][];
        for (int y = 0; y < colors.length; y++) {
            text[y] = new char[colors[y].length];
            for (int x = 0; x < colors[0].length; x++) {
                text[y][x] = toAsciiChar(colors[y][x]);
            }
        }
        return text;
    }
}
