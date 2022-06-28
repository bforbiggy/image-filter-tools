namespace image_filter_tools;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class BlurFilter {
	[Obsolete("This is incredibly inefficient, do not use.")]
	public static Rgba32 avgAroundPoint(ref Image<Rgba32> img, int xOrg, int yOrg) {
		int r = 0, g = 0, b = 0, count = 0;
		int yMax = Math.Min(yOrg + 1, img.Height - 1);
		int xMax = Math.Min(xOrg + 1, img.Width - 1);
		for (int y = Math.Max(0, yOrg - 1); y <= yMax; y++) {
			for (int x = Math.Max(0, xOrg - 1); x <= xMax; x++) {
				Rgba32 color = img[y, x];
				r += color.R;
				g += color.G;
				b += color.B;
				count++;
			}
		}

		return new Rgba32((byte)(r / count), (byte)(g / count), (byte)(b / count));
	}

	public static void convertImage(ref Image<Rgba32> img, int pass = 5) {
		Rgba32 horizontalAvg(ref Image<Rgba32> img, int xOrg, int y){
			int r = 0, g = 0, b = 0, count = 0;
			int xMax = Math.Min(xOrg + 1, img.Width - 1);
			for(int x = Math.Max(0, xOrg-1); x <= xMax; x++){
				Rgba32 color = img[y, x];
				r += color.R;
				g += color.G;
				b += color.B;
				count++;
			}
			return new Rgba32((byte)(r / count), (byte)(g / count), (byte)(b / count));
		}

		// Perform horizontal blur
		for (int y = 0; y < img.Height; y++) {
			for (int x = 0; x < img.Width; x++) {
				img[y,x] = horizontalAvg(ref img, x, y);
			}
		}

		Rgba32 verticalAvg(ref Image<Rgba32> img, int x, int yOrg){
			int r = 0, g = 0, b = 0, count = 0;
			int yMax = Math.Min(yOrg + 1, img.Height - 1);
			for(int y = Math.Max(0, yOrg-1); y <= yMax; y++){
				Rgba32 color = img[y, x];
				r += color.R;
				g += color.G;
				b += color.B;
				count++;
			}
			return new Rgba32((byte)(r / count), (byte)(g / count), (byte)(b / count));
		}

		// Perform vertical blur
		for (int x = 0; x < img.Width; x++) {
			for (int y = 0; y < img.Height; y++) {
				img[y, x] = verticalAvg(ref img, x, y);
			}
		}
	}
}