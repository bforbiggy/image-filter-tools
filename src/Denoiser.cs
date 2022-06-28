namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

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
	private static double convolve(Color[,] colors, int x, int y) {
		// Used to store the sum of the kernel
		double val = 0, percent = 0;

		y -= KERNEL.GetLength(0) / 2;
		x -= KERNEL.GetLength(1) / 2;

		// Iterate over the kernel
		for (int ky = 0; ky < KERNEL.GetLength(0); ky++) {
			for (int kx = 0; kx < KERNEL.GetLength(1); kx++) {
				// Match kernel to pixel pos
				int xPos = kx + x;
				int yPos = ky + y;

				// Calculate kernel value intensity output
				if (0 <= xPos && xPos < colors.GetLength(1) && 0 <= yPos && yPos < colors.GetLength(0)) {
					Color gray = colors[yPos, xPos];//GrayScale.convertColor(colors[yPos, xPos]);
					val += gray.R / 255.0 * KERNEL[ky, kx];
					percent += KERNEL[ky, kx];
				}

			}
		}

		return val / percent;
	}

	public static Color[,] convertColors(Color[,] colors, int passCount = 1) {
		for (int i = 0; i < passCount; i++) {
			Color[,] temp = (Color[,])colors.Clone();
			for (int y = 0; y < colors.GetLength(0); y++) {
				for (int x = 0; x < colors.GetLength(1); x++) {
					double intensity = convolve(colors, x, y);
					int r = (int)(colors[y, x].R * intensity);
					int g = (int)(colors[y, x].G * intensity);
					int b = (int)(colors[y, x].B * intensity);
					colors[y, x] = Color.FromArgb(r, g, b);
				}
			}
		}
		return colors;
	}

	public static void writeConverted(Bitmap img, Stream output, int passCount = 1) {
		Color[,] colors = new Color[img.Height, img.Width];
		for (int y = 0; y < img.Height; y++) {
			for (int x = 0; x < img.Width; x++) {
				colors[x, y] = img.GetPixel(x, y);
			}
		}
		colors = convertColors(colors, passCount);

		for (int y = 0; y < img.Height; y++) {
			for (int x = 0; x < img.Width; x++) {
				img.SetPixel(x, y, colors[x, y]);
			}
		}
		img.Save(output, ImageFormat.Jpeg);
	}
}