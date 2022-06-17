namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class Denoiser {
    public static double[,] GAUSSIAN_KERNEL = generateKernel(5, 5);

    private static double[,] generateKernel(int width, int height, double stdev = 1.0) {
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        double[,] kernel = new double[height, width];
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

    private static Color convolve(Color[,] colors, int x, int y, double[,] kernel) {
        // Used to store the sum of the kernel
        double val = 0, percent = 0;

        y -= kernel.GetLength(0) / 2;
        x -= kernel.GetLength(1) / 2;

        // Iterate over the kernel
        for (int ky = 0; ky < kernel.GetLength(0); ky++) {
            for (int kx = 0; kx < kernel.GetLength(1); kx++) {
                // Match kernel to pixel pos
                int xPos = kx + x;
                int yPos = ky + y;

                // Calculate kernel value output
                if (0 <= xPos && xPos < colors.GetLength(1) && 0 <= yPos && yPos < colors.GetLength(0)) {
                    val += colors[yPos, xPos].R * kernel[ky, kx];
                    percent += kernel[ky, kx];
                }

            }
        }

        int output = (int)(val * (1 / percent));
        return Color.FromArgb(output, output, output);
    }

    public static Color[,] convertColors(Color[,] colors) {
        Color[,] temp = (Color[,])colors.Clone();
        for (int y = 0; y < colors.GetLength(0); y++) {
            for (int x = 0; x < colors.GetLength(1); x++) {
                colors[y, x] = convolve(temp, x, y, GAUSSIAN_KERNEL);
            }
        }
        return colors;
    }

    public static void writeConverted(Bitmap img, Stream output) {
        Color[,] colors = new Color[img.Height, img.Width];
        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                colors[x, y] = img.GetPixel(x, y);
            }
        }
        GrayScale.convertColors(colors);
        convertColors(colors);

        for (int y = 0; y < img.Height; y++) {
            for (int x = 0; x < img.Width; x++) {
                img.SetPixel(x, y, colors[x, y]);
            }
        }
        img.Save(output, ImageFormat.Jpeg);
    }
}