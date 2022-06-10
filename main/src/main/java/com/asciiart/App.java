package com.asciiart;

import java.awt.Color;
import java.awt.image.BufferedImage;
import java.awt.image.PixelGrabber;
import java.io.File;
import java.io.IOError;
import java.io.IOException;
import javax.imageio.ImageIO;


public class App {
    private static Color intToColor(int rgba){
        int red = (rgba >> 16) & 0xFF;
        int green = (rgba >> 8) & 0xFF;
        int blue = rgba & 0xFF;
        return new Color(red, green, blue);
    }

    public static void main(String[] args) {
        try {
            // Read source image
            File file = new File("main/src/main/resources/morbius_poster.jpg");
            BufferedImage img = ImageIO.read(file);

            // Create grayscaled version of image
            BufferedImage grayImg = new BufferedImage(img.getWidth(), img.getHeight(), BufferedImage.TYPE_INT_RGB);
            for(int x = 0; x < img.getWidth(); x++){
                for(int y = 0; y < img.getHeight(); y++){
                    int rgba = img.getRGB(x, y);
                    Color color = GrayScale.toGrayColor(intToColor(rgba));
                    grayImg.setRGB(x, y, color.getRGB());
                }
            }

            // Output grayscaled image
            String outputPath = file.getCanonicalPath().substring(0, file.getCanonicalPath().indexOf(".")) + "_grayscaled.jpg";
            File outputFile = new File(outputPath);
            ImageIO.write(grayImg, "jpg", outputFile);
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}