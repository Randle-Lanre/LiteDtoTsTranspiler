namespace LiteDtoTsTranspiler.Helpers;

public static class FileHelper
{
    public static (bool, string) FilePathHelper(string dtoName, string outputLocation)
    {
        if (string.IsNullOrWhiteSpace(dtoName)) return (false, "");
        try
        {
            // using (File.Create($"{outputLocation}\\{dtoName}.ts"))
            using (File.Create(Path.Combine(outputLocation, dtoName)+ ".ts"))
                return (true, $"{Path.Combine(outputLocation, dtoName)}.ts");
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            return (false, "");
        }
    }


    public static async Task TypePrefix(string filePath, string dtoName)
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

    public static async Task TypeSuffix(string filePath)
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

    public static async Task FileWriter(string propName, string propType, string typeFile)
    {
        if (!File.Exists(typeFile)) return;

        var utility = new DtoTypeConverter();

        var line = $"{propName} : {utility.ConvertCsTypeToTsType(propType)}, " + Environment.NewLine;

        try
        {
            await File.AppendAllTextAsync(typeFile, line);
        }
        catch (IOException e)
        {
            Console.WriteLine("exception writing to file, {0}", e);
        }
    }
}