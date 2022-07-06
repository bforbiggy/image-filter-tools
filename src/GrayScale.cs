using image_filter_tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class GrayScale {
	public static void convertPixel(ref Rgba32 color) {
		int avg = (color.R + color.G + color.B) / 3;
		color.R = (byte)avg;
		color.G = (byte)avg;
		color.B = (byte)avg;
	}

	public static void convertImage(ref Image<Rgba32> img) {
		img.ProcessPixelRows(accessor => {
			for (int y = 0; y < accessor.Height; y++) {
				Span<Rgba32> row = accessor.GetRowSpan(y);

				foreach (ref Rgba32 pixel in row) {
					convertPixel(ref pixel);
				}
			}
		});
	}
}