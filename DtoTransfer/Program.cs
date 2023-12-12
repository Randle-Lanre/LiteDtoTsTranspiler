// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Runtime.CompilerServices;

namespace DtoTransfer;

internal class Program
{
   static string basePath = @"C:\Users\Kehinde\RiderProjects\DtoTransfer\DtoTransfer\TestDtoOutput";

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var assembly = Assembly.GetExecutingAssembly();
        var dtoClasses = assembly.GetTypes().Where(type => type.Name.EndsWith("Dto")).ToList();

        foreach (var dto in dtoClasses)
        {
            Console.WriteLine("class Name ---> {0}", dto.Name); //pass to File create

            #region Create_Type_File

            var ( _, typeFileCreated )= FileHelper(dto.Name);

            #endregion

            var propties = dto.GetProperties().ToList();

            foreach (PropertyInfo item in propties)
            {
                #region Write_To_Type_File

                if (!string.IsNullOrWhiteSpace(typeFileCreated))
                {
                    //
                    FileWriter(item.Name.ToString(), item.PropertyType.ToString(), typeFileCreated);
                }

                #endregion

                Console.WriteLine("PropertyName --> {0}, PropertyType --> {1}", item.Name, item.PropertyType);
            }
        }
    }


    static (bool, string) FileHelper(string dtoName)
    {
        if (string.IsNullOrWhiteSpace(dtoName)) return (false, "");
        File.Create($"{basePath}\\{dtoName}.ts");
        return (true, $"{basePath}\\{dtoName}.ts");

    }

    static bool TypePrefix(string filePath, string dtoName)
    {
        //export Interface name {

        return true;
    }

    static bool TypeSuffix(string filePath, string dtoName)
    {
        // }
        return true;
    }

    static void FileWriter(string propName, string propType, string typeFile)
    {
        if (File.Exists(typeFile))
        {
            
        }
    
        //
        
    }

    static string TypeConverter(string propType) => propType switch
    {
        "System.Int32" => "number",
        "System.String" => "string",
        "System.DateTime" => "Datetime",
        "System.Double" => "float"
    };

}