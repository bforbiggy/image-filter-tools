namespace image_filter_tools;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Edging {
	/// <summary>
	/// A function that measures the difference (aka intensity) between two colors.
	/// A positive result means an increase in intensity and vice versa.
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <returns></returns>
	private static int deltaColor(Rgba32 from, Rgba32 to) {
		int fromIntensity = from.R + from.G + from.B;
		int toIntensity = to.R + to.G + to.B;
		return (toIntensity - fromIntensity) / 3;
	}

	public static void convertColors(ref Image<Rgba32> img) {
		for (int y = 0; y < img.Height - 1; y++) {
			for (int x = 0; x < img.Width - 1; x++) {
				int deltaX = deltaColor(img[y, x], img[y, x + 1]);
				int deltaY = deltaColor(img[y, x], img[y + 1, x]);
				int c = Math.Clamp(Math.Abs(deltaX) + Math.Abs(deltaY), 0, 255);
				Rgba32 newPixel = new Rgba32((byte)c, (byte)c, (byte)c);
				img[y, x] = newPixel;
			}
		}
	}
}