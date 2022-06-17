namespace image_filter_tools;

using System.Drawing;
using System.CommandLine;
using System.IO;
using System.CommandLine.Parsing;

public class Program {
    /// <summary>
    /// Given a path to an image, attemp to load said image.
    /// The loaded image can also be scaled if necessary.
    /// </summary>
    /// <param name="path">Path to image</param>
    /// <param name="scale">Scale factor for image</param>
    /// <returns></returns>
    private static Bitmap scaleImg(Bitmap img, double scale = 1.0) {
        if (scale != 1.0) {
            Size size = new Size((int)(img.Width * scale), (int)(img.Height * scale));
            img = new Bitmap(img, size);
        }
        return img;
    }



    /// <summary>
    /// This method verifies that the user inputted a valid path.
    /// If the path is invalid, generate a path based on input file path.
    /// This is useful for generating output paths that do not override original images.
    /// </summary>
    /// <param name="userOutput">The user specified</param>
    /// <param name="ext">The extension of output, which is .jpg by default.</param>
    /// <returns>Output path.</returns>
    private static string getValidPath(string userOutput, FileInfo inputPath, string ext = ".jpg") {
        if (userOutput != null && userOutput.Length == 0)
            return userOutput;

        return inputPath.DirectoryName + Path.GetFileNameWithoutExtension(inputPath.FullName) + "_output" + ext;
    }

    public static void Main(string[] args) {
        // Because some goofy type people might run the executable with no arguments, let's provide basic functionality.
        if (Console.GetCursorPosition() == (0, 0)) {
            Console.WriteLine(@"ARE YOU INSANE??!??!? Don't run this program by double-clicking it. Run this in the terminal");
            Console.WriteLine(@"Shift-right click and ""Open in terminal"" then run the program. (ex. ""./image-filter-tools ascii --input pog.png --scale 0.3"")");
            Console.WriteLine(@"Press any key to exit...");
            Console.ReadKey();
        }

        #region Global options/arguments for program
        Bitmap? img = null;
        Option<FileInfo> input = new Option<FileInfo>(
            aliases: new string[] { "-i", "--input" },
            description: "File location of image to process.",
            parseArgument: (result) => {
                string path = result.Tokens.Single().Value;

                if (!File.Exists(path))
                    result.ErrorMessage = "File path does not point to a file.";

                try {
                    img = new Bitmap(path);
                }
                catch (ArgumentException) {
                    result.ErrorMessage = "File could not be processed as an image.";
                }

                return new FileInfo(path);
            }
        ) { IsRequired = true };

        Option<string> output = new Option<string>(
            aliases: new string[] { "-o", "--output" },
            description: "Output destination of processed image."
        );

        Option<double> resize = new Option<double>(
            aliases: new string[] { "-s", "--resize", "--scale" },
            description: "Scale the output image by this factor.",
            getDefaultValue: () => 1.0
        );
        #endregion

        #region Specific algorithms and their related options
        // Which ascii art set to use
        Option<bool> detailed = new Option<bool>(
            aliases: new string[] { "-d", "--detailed" },
            description: "Whether or not to use detailed ascii art set.",
            getDefaultValue: () => false
        );

        // ASCII-ify image
        Command ascii = new Command("ascii", "Converts image to an ascii representation.");
        ascii.AddOption(detailed);
        ascii.SetHandler((inputPath, outputPath, scaleFactor, isDetailed) => {
            img = scaleImg(img!, scaleFactor);
            FileStream fs = File.Create(getValidPath(outputPath, inputPath, ".txt"));
            AsciiScale.writeConverted(img, fs, isDetailed);
        }, input, output, resize, detailed);

        // Grayscale image
        Command grayscale = new Command("grayscale", "Converts image to grayscale.");
        grayscale.SetHandler((inputPath, outputPath, scaleFactor) => {
            img = scaleImg(img!, scaleFactor);
            FileStream fs = File.Create(getValidPath(outputPath, inputPath));
            GrayScale.writeConverted(img, fs);
        }, input, output, resize);

        // Blur image
        Command blur = new Command("blur", "Performs a moving average blur.");
        blur.SetHandler((inputPath, outputPath, scaleFactor) => {
            img = scaleImg(img!, scaleFactor);
            FileStream fs = File.Create(getValidPath(outputPath, inputPath));
            BlurFilter.writeConverted(img, fs);
        }, input, output, resize);

        // Edge detect image
        Command edge = new Command("edge", "Filter image to get image edges.");
        edge.SetHandler((inputPath, outputPath, scaleFactor) => {
            img = scaleImg(img!, scaleFactor);
            FileStream fs = File.Create(getValidPath(outputPath, inputPath));
            Edging.writeConverted(img, fs);
        }, input, output, resize);

        // Sharpen image
        Command sharpen = new Command("sharpen", "Sharpen image.");
        sharpen.SetHandler((inputPath, outputPath, scaleFactor) => {
            img = scaleImg(img!, scaleFactor);
            FileStream fs = File.Create(getValidPath(outputPath, inputPath));
            Sharpener.writeConverted(img, fs);
        }, input, output, resize);

        // Denoise image
        Command denoise = new Command("denoise", "Denoise image.");
        denoise.SetHandler((inputPath, outputPath, scaleFactor) => {
            img = scaleImg(img!, scaleFactor);
            FileStream fs = File.Create(getValidPath(outputPath, inputPath));
            Denoiser.writeConverted(img, fs);
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
        rootCommand.AddCommand(blur);
        rootCommand.AddCommand(edge);
        rootCommand.AddCommand(sharpen);
        rootCommand.AddCommand(denoise);

        rootCommand.Invoke(args);
    }
}
