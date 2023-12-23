using System.Reflection;
using LiteDtoTsTranspiler.cli;
using LiteDtoTsTranspiler.Helpers;
using Spectre.Console.Cli;

namespace LiteDtoTsTranspiler;

internal static class Program
{

    static int Main(string[] args)
    {
        var app = new CommandApp();

        app.Configure(
            config =>
            {
                config.AddBranch<CliSettings.AddSettings>("generate", add =>
                {
                    add.AddCommand<CliCommands.AddPackageCommand>("dto")
                        .WithDescription("Converts all Dto class to TS interfaces, Dto classes must" +
                                         " end with Dto e.g AnimalDto.cs")
                        .WithExample("generate", "dto", "appclication_name", "c:\\users\\exampleuser\\outputfolder");
                });
               
            }
        );
        return app.Run(args);
    }


    static async Task TranspileDto()
    {
      

        #region test_dll_in_directory

        // Get the current directory
        string currentDir = Directory.GetCurrentDirectory();

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
            return;
        }

        #endregion

        await Transpile(dllPath);
    }

    private static async Task Transpile(string dllPath)
    {
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