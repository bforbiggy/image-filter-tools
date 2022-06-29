namespace image_filter_tools;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Denoiser {
	public static double[,] KERNEL = generateKernel(5, 5);

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

	private static Rgba32 convolve(ref Image<Rgba32> img, int xOrg, int yOrg) {
		// Used to store the sum of the kernel
		double r = 0, g = 0, b = 0;

		xOrg -= KERNEL.GetLength(1) / 2;
		yOrg -= KERNEL.GetLength(0) / 2;

		// Iterate over the kernel
		for (int ky = 0; ky < KERNEL.GetLength(0); ky++) {
			for (int kx = 0; kx < KERNEL.GetLength(1); kx++) {
				// Match relative kernel pos to pixel pos
				int xPos = Math.Clamp(xOrg + kx, 0, img.Width - 1);
				int yPos = Math.Clamp(yOrg + ky, 0, img.Height - 1);

				// Calculate individual pixel value after convolution
				Rgba32 pixel = img[xPos, yPos];
				r += (pixel.R * KERNEL[ky, kx]);
				g += (pixel.G * KERNEL[ky, kx]);
				b += (pixel.B * KERNEL[ky, kx]);
			}
		}

		return new Rgba32((byte)r, (byte)g, (byte)b);
	}

	public static void convertColors(ref Image<Rgba32> img, int passCount = 1) {
		for (int i = 0; i < passCount; i++) {
			//Image<Rgba32> imgCopy = img.Clone();
			for (int y = 0; y < img.Height; y++) {
				for (int x = 0; x < img.Width; x++) {
					img[x, y] = convolve(ref img, x, y);
				}
			}
		}
	}
}