using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

public class Program
{
    public static void writeAscii(Bitmap img, String outputPath)
    {
        StringBuilder sb = new StringBuilder();
        
        double xInc = 1;
        double yInc = 2.5;
        for (double y = 0; y < img.Height; y += yInc)
        {
            for (double x = 0; x < img.Width; x += xInc)
            {
                Color color = img.GetPixel((int)x, (int)y);
                sb.Append(AsciiScale.toAsciiChar(color));
            }
            sb.Append("\n");
        }

        File.WriteAllText(outputPath, sb.ToString());
    }

    public static void writeGrayScale(Bitmap img, String outputPath)
    {
        // Create grayscaled version of image
        Bitmap grayImg = new Bitmap(img.Width, img.Height);
        for(int x = 0; x < img.Width; x++)
        {
            for(int y = 0; y < img.Height; y++)
            {
                Color color = GrayScale.toGrayColor(img.GetPixel(x, y));
                grayImg.SetPixel(x, y, color);
            }
        }

        // Output grayscaled image
        grayImg.Save(outputPath, ImageFormat.Jpeg);
    }

    public static void Main(String[] args)
    {
        // Read source image
        Bitmap img = new Bitmap(args[0]);

        // Determine operation based on next arg
        args[0] = args[0].Substring(0, args[0].LastIndexOf("."));
        String option = args[1].ToLower();
        if(option == "ascii"){
            writeAscii(img, $"{Directory.GetCurrentDirectory()}/{args[0]}_output.txt");
        }
        else if(option == "grayscale"){
            writeGrayScale(img, $"{Directory.GetCurrentDirectory()}/{args[0]}_output.jpg");
        }
    }
}
