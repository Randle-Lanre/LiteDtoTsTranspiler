using System.Reflection;
using LiteDtoTsTranspiler.Helpers;

namespace LiteDtoTsTranspiler;

//TODO: deal with allocations

/**
 * when program is run in a directory
 * enumerate all files inside to find the required dll
 * then load that and find the DTOs
 */
internal static class Program
{
    private static async Task Main()
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