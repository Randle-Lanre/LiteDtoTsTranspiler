// See https://aka.ms/new-console-template for more information

using System.Reflection;


namespace DtoTransfer;

//TODO: deal with allocations
internal class Program
{
    static string basePath = @"C:\Users\Kehinde\RiderProjects\DtoTransfer\DtoTransfer\TestDtoOutput";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var assembly = Assembly.GetExecutingAssembly();
        var dtoClasses = assembly.GetTypes().Where(type => type.Name.EndsWith("Dto")).ToList();

        foreach (var dto in dtoClasses)
        {
            Console.WriteLine("class Name ---> {0}", dto.Name); //pass to File create

            #region Create_Type_File

            var (_, typeFileCreated) = FileHelper(dto.Name);

            await TypePrefix(typeFileCreated, dto.Name);

            #endregion

            var properties = dto.GetProperties().ToList();

            foreach (PropertyInfo item in properties)
            {
                #region Write_To_Type_File

                if (!string.IsNullOrWhiteSpace(typeFileCreated))
                {
                    //
                    await FileWriter(item.Name, item.PropertyType.ToString(), typeFileCreated);
                }

                #endregion

                Console.WriteLine("PropertyName --> {0}, PropertyType --> {1}", item.Name, item.PropertyType);
            }

            await TypeSuffix(typeFileCreated);
        }
    }


    static (bool, string) FileHelper(string dtoName)
    {
        if (string.IsNullOrWhiteSpace(dtoName)) return (false, "");
        using (File.Create($"{basePath}\\{dtoName}.ts"))
            return (true, $"{basePath}\\{dtoName}.ts");
    }

    static async Task TypePrefix(string filePath, string dtoName)
    {
        var pfx = $"export interface {dtoName}  " + "{" + Environment.NewLine;
        try
        {
            await File.WriteAllTextAsync(filePath, pfx);
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
        }
    }

    static async Task TypeSuffix(string filePath)
    {
        var sfx = Environment.NewLine + "}";
        try
        {
            await File.AppendAllTextAsync(filePath, sfx);
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static async Task FileWriter(string propName, string propType, string typeFile)
    {
        if (!File.Exists(typeFile)) return;

        var utility = new Utility();
        // id : number
        var line = $"{propName} : {utility.TypeConverter(propType)}, " + Environment.NewLine;
        try
        {
            await File.AppendAllTextAsync(typeFile, line);
        }
        catch (IOException e)
        {
            Console.WriteLine("exception writing to file, {0}", e);
        }

    }

    class Utility
    {
        public string TypeConverter(string propType) => propType switch
        {
            "System.Int32" => "number",
            "System.String" => "string",
            "System.DateTime" => "Date",
            "System.Double" => "number",
            "System.Int64" => "bigint",
            "System.Boolean" => "boolean",
            _ => "any"
        };
    }
}