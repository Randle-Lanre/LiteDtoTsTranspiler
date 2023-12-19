using System.Reflection;
using LiteDtoTsTranspiler.Helpers;
using Spectre.Console.Cli;

namespace LiteDtoTsTranspiler;

using Spectre.Console;

//TODO: deal with allocations

/**
 * when program is run in a directory
 * enumerate all files inside to find the required dll
 * then load that and find the DTOs
 */
internal static class Program
{
    #region Settings
    public class AddSettings : CommandSettings
    {
        [CommandArgument(0, "[PROJECT]")] 
        public string Project { get; set; }
    }

    public class AddPackageSettings  : AddSettings
    {
        [CommandArgument(0, "<PACKAGE_NAME>")]
        public string PackageName { get; set; }
        
        [CommandOption("-v|--version <VERSION>")]
        public string Version { get; set; }
    }
    
    public class AddReferenceSettings : AddSettings
    {
        [CommandArgument(0, "<PROJECT_REFERENCE>")]
        public string ProjectReference { get; set; }
    }
    #endregion
    
    #region Commands

    public class AddPackageCommand : Command<AddPackageSettings>
    {
        public override int Execute(CommandContext context, AddPackageSettings settings)
        {
            // Omitted
            return 0;
        }
    }

    public class AddReferenceCommand : Command<AddReferenceSettings>
    {
        public override int Execute(CommandContext context, AddReferenceSettings settings)
        {
            // Omitted
            return 0;
        }
    }
    #endregion

    static int Main(string[] args)
    {
        var app = new CommandApp();

        app.Configure(
            config =>
            {
                // config.AddBranch<AddSettings>("add", add =>
                // {
                //     add.AddCommand<AddPackageCommand>("package");
                //     add.AddCommand<AddReferenceCommand>("reference");
                // });
                config.AddCommand<AddReferenceCommand>("reference");
                config.AddCommand<AddPackageCommand>("package")
                    .IsHidden().WithAlias("packager")
                    .WithDescription("gets the package for you")
                    .WithExample("size", "c:\\windows", "--pattern", "*.dll");
            }
        );
        return app.Run(args);
    }


    static async Task TranspileDto()
    {
        Console.WriteLine("Hello, World!");

        #region test_dll_in_directory

        // Get the current directory
        string currentDir = Directory.GetCurrentDirectory();

        // Call the recursive method to search for the dll
        string dllPath = DllHelper.FindDll(currentDir, "LiteDtoTsTranspiler.dll");

        // Check if the dll was found and print the result
        if (dllPath != null)
        {
            Console.WriteLine("The dll was found at: " + dllPath);
        }
        else
        {
            Console.WriteLine("The dll was not found.");
            return;
        }

        #endregion

        // var assembly = Assembly.GetExecutingAssembly();
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