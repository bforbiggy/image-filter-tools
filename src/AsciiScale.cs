namespace image_filter_tools;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class AsciiScale {
	public const double xInc = 1;
	public const double yInc = 2.5;

	public const string ASCII_SIMPLE = " .:-=+*#%@";
	public const string ASCII_DETAILED = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,""^`'.";

	public static char convertColor(ref Rgba32 color, bool detailed = false) {
		string ASCII = detailed ? ASCII_DETAILED : ASCII_SIMPLE;
		GrayScale.convertColor(ref color);
		int index = (int)(color.R / 255.0 * (ASCII.Length - 1));
		return ASCII[index];
	}

	public static char[,] convertColors(ref Image<Rgba32> img, bool detailed = false) {
		char[,] text = new char[img.Height, img.Width];

		img.ProcessPixelRows((accessor) => {
			for (double y = 0; y < accessor.Height; y += yInc) {
				Span<Rgba32> pixelRow = accessor.GetRowSpan((int)y);
				for (double x = 0; x < pixelRow.Length; x += xInc) {
					text[(int)y, (int)x] = convertColor(ref pixelRow[(int)x], detailed);
				}
			}
		});

		return text;
	}

	public static void writeConverted(ref Image<Rgba32> img, Stream output, bool detailed = false) {
		StreamWriter sw = new StreamWriter(output);

		img.ProcessPixelRows((accessor) => {
			for (double y = 0; y < accessor.Height; y += yInc) {
				Span<Rgba32> pixelRow = accessor.GetRowSpan((int)y);
				for (double x = 0; x < pixelRow.Length; x += xInc) {
					sw.Write(convertColor(ref pixelRow[(int)x], detailed));
				}
				sw.Write("\n");
			}
		});

		sw.Close();
	}
}
