namespace image_filter_tools;

using System.Drawing;
using System.CommandLine;
using System.IO;
using System.CommandLine.Parsing;

public class Program
{
    /// <summary>
    /// Given a path to an image, attemp to load said image.
    /// The loaded image can also be scaled if necessary.
    /// </summary>
    /// <param name="path">Path to image</param>
    /// <param name="scale">Scale factor for image</param>
    /// <returns></returns>
    private static Bitmap imgFromPath(string path, double scale){
        Bitmap image = new Bitmap(path);
        if(scale != 1.0){
            Size size = new Size((int)(image.Width * scale), (int)(image.Height * scale));
            image = new Bitmap(image, size);
        }
        return image;
    }

    /// <summary>
    /// When given the path for the original source image, this method generates a new path.
    /// This is useful for generating output paths that do not override original images.
    /// </summary>
    /// <param name="inputPath">The original source image path.</param>
    /// <param name="ext">The extension of output, which is .jpg by default.</param>
    /// <returns>Output path.</returns>
    private static string genOutputPath(string inputPath, string ext = ".jpg"){
        if (Path.HasExtension(inputPath))
            inputPath = inputPath.Substring(0, inputPath.LastIndexOf("."));
        return inputPath + ext;
    }

    public static void Main(string[] args)
    {
        // Because some goofy type people might run the executable with no arguments, let's ask for some.
        if(Console.GetCursorPosition() == (0, 0)){
            
        }

        #region Global options/arguments for program
        Option<FileInfo> input = new Option<FileInfo>(
            name: "--input",
            description: "File location of image to process."
        ){ IsRequired = true };

        Option<string> output = new Option<string>(
            name: "--output",
            description: "Output destination of processed image."
        );

        Option<double> resize = new Option<double>(
            name: "--resize",
            description: "Scale the output image by this factor.",
            getDefaultValue: () => 1.0
        );
        #endregion

        #region Specific algorithms
        // Convert image to ascii art
        Command ascii = new Command("ascii", "Converts image to an ascii representation.");
        ascii.SetHandler((inputPath, outputPath, scaleFactor) => {
            Bitmap img = imgFromPath(inputPath.FullName, scaleFactor);
            FileStream fs = File.Create(outputPath ?? genOutputPath(inputPath.FullName, ".txt"));
            AsciiScale.writeConverted(img, fs);
        }, input, output, resize);

        // Convert image to grayscale version
        Command grayscale = new Command("grayscale", "Converts image to grayscale.");
        grayscale.SetHandler((inputPath, outputPath, scaleFactor) =>{
            Bitmap img = imgFromPath(inputPath.FullName, scaleFactor);
            FileStream fs = File.Create(outputPath ?? genOutputPath(inputPath.FullName, ".jpg"));
            AsciiScale.writeConverted(img, fs);
        }, input, output, resize);
        #endregion

        // Create root command and add global options
        RootCommand rootCommand = new RootCommand("A command line tool for applying filters to images.");
        rootCommand.AddGlobalOption(input);
        rootCommand.AddGlobalOption(output);
        rootCommand.AddGlobalOption(resize);

        // Enable all available image operations
        rootCommand.AddCommand(ascii);
        rootCommand.AddCommand(grayscale);

        rootCommand.Invoke(args);
    }
}
