namespace image_filter_tools;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Sharpener {
	public static void convertImage(ref Image<Rgba32> img) {
		// Grab edges to enhance
		Image<Rgba32> edges = img.Clone();
		Edging.convertImage(ref edges);

		// Apply edge enhancement to image
		for (int y = 0; y < img.Height - 1; y++) {
			for (int x = 0; x < img.Width - 1; x++) {
				// Subtract edges from original image to make blacks blacker
				int r = Math.Clamp(img[y, x].R - edges[y, x].R / 2, 0, 255);
				int g = Math.Clamp(img[y, x].G - edges[y, x].G / 2, 0, 255);
				int b = Math.Clamp(img[y, x].B - edges[y, x].B / 2, 0, 255);
				img[y, x] = new Rgba32((byte)r, (byte)g, (byte)b);
			}
		}
	}
}