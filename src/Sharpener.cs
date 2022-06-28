namespace image_filter_tools;

using System.Drawing;
using System.Drawing.Imaging;

public class Sharpener {
	public static Color[,] convertColors(Color[,] colors) {
		// Grab edges to enhance
		Color[,] edges = (Color[,])colors.Clone();
		edges = Edging.convertColors(edges);

		// Apply edge enhancement to image
		for (int y = 0; y < colors.GetLength(0) - 1; y++) {
			for (int x = 0; x < colors.GetLength(1) - 1; x++) {
				// Subtract edges from original image to make blacks blacker
				int r = Math.Clamp(colors[y, x].R - edges[y, x].R / 2, 0, 255);
				int g = Math.Clamp(colors[y, x].G - edges[y, x].G / 2, 0, 255);
				int b = Math.Clamp(colors[y, x].B - edges[y, x].B / 2, 0, 255);
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
		convertColors(colors);

		for (int y = 0; y < img.Height; y++) {
			for (int x = 0; x < img.Width; x++) {
				img.SetPixel(x, y, colors[y, x]);
			}
		}
		img.Save(output, ImageFormat.Jpeg);
	}
}