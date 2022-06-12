namespace image_to_ascii;

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
        Option<FileInfo> input = new Option<FileInfo>(
            name: "--input",
            description: "File location of image to process."
        ){ IsRequired = true };

        Option<FileInfo?> output = new Option<FileInfo?>(
            name: "--output",
            description: "Output destination of processed image."
        );

        Command ascii = new Command("ascii", "Converts image to an ascii representation.");
        ascii.SetHandler((input) => {
            Bitmap img = new Bitmap(input.FullName);
            FileStream output = File.Create(replaceExtension(input.FullName, "_output.txt"));
            AsciiScale.writeConverted(img, output);
        }, input);

        Command grayscale = new Command("grayscale", "Converts image to grayscale.");
        grayscale.SetHandler((input) =>{
            Bitmap img = new Bitmap(input.FullName);
            FileStream output = File.Create(replaceExtension(input.FullName, "_output.jpg"));
            GrayScale.writeConverted(img, output);
        }, input);

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
