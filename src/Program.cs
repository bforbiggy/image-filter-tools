namespace image_filter_tools;

using System.Drawing;
using System.CommandLine;
using System.IO;

public class Program
{
    static string inputPath = "";
    static Bitmap img;
    
    static string? outputPath = null;
    static FileStream? fsOut = null;

    private static string replaceExtension(String path, String newExt){
        if(Path.HasExtension(path))
            path = path.Substring(0, path.LastIndexOf("."));
        return path + newExt;
    }

    public static async Task<int> Main(string[] args)
    {
        #region Global options/arguments for program
        Option<FileInfo> input = new Option<FileInfo>(
            name: "--input",
            description: "File location of image to process.",
            parseArgument: (result) => {
                inputPath = result.Tokens.Single().Value;
                img = new Bitmap(inputPath);
                return new FileInfo(inputPath);
            }
        ){ IsRequired = true };

        Option<FileInfo?> output = new Option<FileInfo?>(
            name: "--output",
            description: "Output destination of processed image.",
            parseArgument: (result) => {
                string output = result.Tokens.Single().Value;

                fsOut = File.Create(output);
                outputPath = output;
                
                return new FileInfo(output);
            }
        );
        #endregion

        #region Specific image algorithm commands
        // Convert image to ascii art
        Command ascii = new Command("ascii", "Converts image to an ascii representation.");
        ascii.SetHandler((input, output) => {
            outputPath = outputPath ?? replaceExtension(inputPath, "_output.txt");
            AsciiScale.writeConverted(img, fsOut ?? File.Create(outputPath));
        }, input, output);

        // Convert image to grayscale version
        Command grayscale = new Command("grayscale", "Converts image to grayscale.");
        grayscale.SetHandler((input, output) =>{

            outputPath = outputPath ?? replaceExtension(inputPath, "_output.jpg");
            GrayScale.writeConverted(img, fsOut ?? File.Create(outputPath));
        }, input, output);
        #endregion

        // Create root command, including global options
        RootCommand rootCommand = new RootCommand("A command line tool for applying filters to images.");
        rootCommand.AddGlobalOption(input);
        rootCommand.AddGlobalOption(output);

        // Enable all available image operations
        rootCommand.AddCommand(ascii);
        rootCommand.AddCommand(grayscale);

        return await rootCommand.InvokeAsync(args);
    }
}
