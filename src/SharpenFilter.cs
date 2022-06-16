namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class SharpenFilter {
    public static double[,] GUASSIAN_KERNEL = generateKernel(5, 5);

    private static double[,] generateKernel(int width, int height, double stdev = 1.0) {
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        double[,] kernel = new double[width, height];
        double sum = 0.0;

        // Equation: e^-[(x^2+y^2)/(2*stdev^2)] / (2*pi*stdev^2)
        double a = 2.0 * stdev * stdev;
        for (int x = -halfWidth; x <= halfWidth; x++) {
            for (int y = -halfHeight; y <= halfHeight; y++) {
                double r = x * x + y * y;
                kernel[x + halfWidth, y + halfHeight] = Math.Exp(-r / a) / (Math.PI * a);
                sum += kernel[x + halfWidth, y + halfHeight];
            }
        }

        // Kernel normalization!?!?!?
        for (int y = 0; y < kernel.GetLength(0); y++)
            for (int x = 0; x < kernel.GetLength(1); x++)
                kernel[y, x] /= sum;

        return kernel;
    }

    public static Color[,] convertColors(Color[,] colors) {
        // Grab edges to enhance
        Color[,] edges = (Color[,])colors.Clone();
        edges = EdgeFilter.convertColors(edges);

        // Apply edge enhancement to image
        for (int y = 0; y < colors.GetLength(0); y++) {
            for (int x = 0; x < colors.GetLength(1); x++) {
                int r = Math.Clamp((int)(colors[y, x].R * (1 + edges[y, x].R / 255.0)), 0, 255);
                int g = Math.Clamp((int)(colors[y, x].G * (1 + edges[y, x].G / 255.0)), 0, 255);
                int b = Math.Clamp((int)(colors[y, x].B * (1 + edges[y, x].B / 255.0)), 0, 255);
                colors[y, x] = Color.FromArgb(r, g, b);
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