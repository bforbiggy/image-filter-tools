using System.Runtime.CompilerServices;

namespace image_filter_tools.src;

public class GuassianFilter {
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
}