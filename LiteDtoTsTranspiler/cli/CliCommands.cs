using System.Reflection;
using LiteDtoTsTranspiler.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LiteDtoTsTranspiler.cli;

public static class CliCommands
{
    public class AddPackageCommand : AsyncCommand<CliSettings.OutputFolderSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, CliSettings.OutputFolderSettings settings)
        {
            var ass = FindAssembly(settings.ApplicationName);
            await Transpile(ass, settings.TranspileOutputLocation);


            AnsiConsole.MarkupLine($"Executed the Command, [green]{settings.TranspileOutputLocation}[/]");
            return 0;
        }

        public override ValidationResult Validate(CommandContext context, CliSettings.OutputFolderSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.TranspileOutputLocation))
                return ValidationResult.Error("Empty Path entered");
            if (!Directory.Exists(settings.TranspileOutputLocation))
                return ValidationResult.Error("Directory Path does not exist");

            var ass = FindAssembly(settings.ApplicationName);
            // if (string.IsNullOrWhiteSpace(ass))
            //     return ValidationResult.Error("cannot locate assembly in this directory" +
            //                                   $" sure you have built the program -> {ass}");

            return base.Validate(context, settings);
        }
    }

    //
    static string FindAssembly(string assemblyName)
    {
        #region test_dll_in_directory

        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();

        // Call the recursive method to search for the dll
        var (found, dllPath) = DllHelper.FindDll(currentDir, $"{assemblyName}.dll");

        // Check if the dll was found and print the result
        if (found)
        {
            Console.WriteLine("The dll was found at: " + dllPath);
        }
        else
        {
            Console.WriteLine($"The dll was not found. current directory {currentDir}, {assemblyName}.dll");
            return "";
        }

        #endregion

        return dllPath;
    }

    private static async Task Transpile(string dllPath, string outputLocation)
    {
        var assembly = Assembly.Load(dllPath);
        var dtoClasses = assembly.GetTypes().Where(type => type.Name.EndsWith("Dto")).ToList();

        foreach (var dto in dtoClasses)
        {
            Console.WriteLine("class Name ---> {0}", dto.Name); //pass to File create

            #region Create_Type_File

            var (_, typeFileCreated) = FileHelper.FilePathHelper(dto.Name, outputLocation);

            await FileHelper.TypePrefix(typeFileCreated, dto.Name);

            #endregion

            var properties = dto.GetProperties().ToList();

            foreach (var item in properties)
            {
                #region Write_To_Type_File

                if (!string.IsNullOrWhiteSpace(typeFileCreated))
                {
                    await FileHelper.FileWriter(item.Name, item.PropertyType.ToString(), typeFileCreated);
                }

                #endregion

                Console.WriteLine("PropertyName --> {0}, PropertyType --> {1}", item.Name, item.PropertyType);
            }

            await FileHelper.TypeSuffix(typeFileCreated);
        }
    }
}