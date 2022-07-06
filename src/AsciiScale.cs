namespace image_filter_tools;

using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class AsciiScale {
	public const double xInc = 1;
	public const double yInc = 2.5;

	public const string ASCII_SIMPLE = " .:-=+*#%@";
	public const string ASCII_DETAILED = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,""^`'.";

	public static char convertPixel(ref Rgba32 color, bool detailed = false) {
		string ASCII = detailed ? ASCII_DETAILED : ASCII_SIMPLE;
		GrayScale.convertPixel(ref color);
		int index = (int)(color.R / 255.0 * (ASCII.Length - 1));
		return ASCII[index];
	}

	public static string convertImage(ref Image<Rgba32> img, bool detailed = false) {
		StringBuilder sb = new StringBuilder();

		img.ProcessPixelRows((accessor) => {
			for (double y = 0; y < accessor.Height; y += yInc) {
				Span<Rgba32> pixelRow = accessor.GetRowSpan((int)y);
				for (double x = 0; x < pixelRow.Length; x += xInc) {
					sb.Append(convertPixel(ref pixelRow[(int)x], detailed));
				}
				sb.AppendLine();
			}
		});

		return sb.ToString();
	}
}
