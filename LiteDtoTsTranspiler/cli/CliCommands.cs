using System.Reflection;
using LiteDtoTsTranspiler.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LiteDtoTsTranspiler.cli;

public static class CliCommands
{
    public class AddPackageCommand : Command<CliSettings.OutputFolderSettings>
    {
        //TODO: check output folder exist 
        //TODO: check if DLL can be found, otherwise warm user
        //TODO: check check if DTOs can be found otherwise prompt user

        public override int Execute(CommandContext context, CliSettings.OutputFolderSettings settings)
        {
            
            AnsiConsole.MarkupLine($"Executed the Command, [green]{settings.PackageName}[/]");
            return 0;
        }

        public override ValidationResult Validate(CommandContext context, CliSettings.OutputFolderSettings settings)
        {
            var path =  AssemblyPath();
            if (string.IsNullOrWhiteSpace(path))  return ValidationResult.Error("Cannot find path");
            
            //
            if (settings.PackageName != "cow")
            {
                return ValidationResult.Error($"you requested for -{settings.PackageName} - instead of cow");
            }

            return base.Validate(context, settings);
        }
        
    }
    
    //
    static string AssemblyPath()
    {
      

        #region test_dll_in_directory

        // Get the current directory
        var currentDir = Directory.GetCurrentDirectory();

        // Call the recursive method to search for the dll
        var (found, dllPath) = DllHelper.FindDll(currentDir, "LiteDtoTsTranspiler.dll");

        // Check if the dll was found and print the result
        if (found)
        {
            Console.WriteLine("The dll was found at: " + dllPath);
        }
        else
        {
            Console.WriteLine("The dll was not found.");
            return "";
        }

        #endregion

        // await Transpile(dllPath);
        return dllPath;
    }

    private static async Task Transpile(string dllPath)
    {
        var assembly = Assembly.Load(dllPath);
        var dtoClasses = assembly.GetTypes().Where(type => type.Name.EndsWith("Dto")).ToList();

        foreach (var dto in dtoClasses)
        {
            Console.WriteLine("class Name ---> {0}", dto.Name); //pass to File create

            #region Create_Type_File

            var (_, typeFileCreated) = FileHelper.FilePathHelper(dto.Name);

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