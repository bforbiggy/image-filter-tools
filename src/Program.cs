namespace image_filter_tools;

using System.Drawing;
using System.CommandLine;

public class Program
{
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
            description: "File location of image to process."
        ){ IsRequired = true };

        Option<FileInfo?> output = new Option<FileInfo?>(
            name: "--output",
            description: "Output destination of processed image."
        );
        #endregion

        #region Specific image algorithm commands
        // Convert image to ascii art
        Command ascii = new Command("ascii", "Converts image to an ascii representation.");
        ascii.SetHandler((input, output) => {
            Bitmap img = new Bitmap(input.FullName);
            FileStream fs = output is null ? File.Create(replaceExtension(input.FullName, "_output.txt")) : File.Create(output.FullName);
            AsciiScale.writeConverted(img, fs);
        }, input, output);

        // Convert image to grayscale version
        Command grayscale = new Command("grayscale", "Converts image to grayscale.");
        grayscale.SetHandler((input, output) =>{
            Bitmap img = new Bitmap(input.FullName);
            FileStream fs = output is null ? File.Create(replaceExtension(input.FullName, "_output.jpg")) : File.Create(output.FullName);
            GrayScale.writeConverted(img, fs);
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
