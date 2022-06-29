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

	//TODO: Prevent darkening
	private static double convolve(ref Image<Rgba32> img, int xOrg, int yOrg) {
		// Used to store the sum of the kernel
		double val = 0;

		//TODO: WHAT IN TARNATION!?!?! 

		// xOrg -= KERNEL.GetLength(1) / 2;
		// yOrg -= KERNEL.GetLength(0) / 2;

		// // Iterate over the kernel
		// for (int ky = 0; ky < KERNEL.GetLength(0); ky++) {
		// 	for (int kx = 0; kx < KERNEL.GetLength(1); kx++) {
		// 		// Match relative kernel pos to pixel pos
		// 		int xPos = Math.Clamp(kx + xOrg, 0, 2);
		// 		int yPos = Math.Clamp(ky + yOrg, 0, 2);

		// 		// Calculate individual pixel value after convlution
		// 		//GrayScale.convertColor(img[yPos, xPos]);
		// 		Rgba32 pixel = img[yPos, xPos];
		// 		val += pixel.R * KERNEL[ky, kx];
		// 	}
		// }

		return val;
	}

	public static void convertColors(ref Image<Rgba32> img, int passCount = 1) {
		for (int i = 0; i < passCount; i++) {
			//Image<Rgba32> imgCopy = img.Clone();
			for (int y = 0; y < img.Height; y++) {
				for (int x = 0; x < img.Width; x++) {
					double intensity = convolve(ref img, x, y);
					int r = (int)(img[y, x].R * intensity);
					int g = (int)(img[y, x].G * intensity);
					int b = (int)(img[y, x].B * intensity);
					img[y, x] = new Rgba32((byte)r, (byte)g, (byte)b);
				}
			}
		}
	}
}