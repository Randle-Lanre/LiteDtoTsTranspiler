﻿using LiteDtoTsTranspiler.Helpers;

namespace LiteDtoTsTranspiler.Helpers;

public class FileHelper
{
    const string basePath = @"C:\Users\Kehinde\RiderProjects\DtoTransfer\LiteDtoTsTranspiler\TestDtoOutput";

    public static (bool, string) FilePathHelper(string dtoName)
    {
        if (string.IsNullOrWhiteSpace(dtoName)) return (false, "");
        using (File.Create($"{basePath}\\{dtoName}.ts"))
            return (true, $"{basePath}\\{dtoName}.ts");
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
        // id : number
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