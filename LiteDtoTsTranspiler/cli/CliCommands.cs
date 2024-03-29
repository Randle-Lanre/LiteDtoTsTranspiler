﻿using System.Reflection;
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
            var asm = FindAssembly(settings.ApplicationName);
            await Transpile(asm, settings.TranspileOutputLocation);


            AnsiConsole.MarkupLine(
                $"Executed the Command successfully, generated interface located at -> [green]{settings.TranspileOutputLocation}[/]");
            return 0;
        }

        public override ValidationResult Validate(CommandContext context, CliSettings.OutputFolderSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.TranspileOutputLocation))
                return ValidationResult.Error("Empty Path entered");
            if (!Directory.Exists(settings.TranspileOutputLocation))
                return ValidationResult.Error("Directory Path does not exist");

            return base.Validate(context, settings);
        }
    }

    //
    static string FindAssembly(string assemblyName)
    {
        #region test_dll_in_directory

        var currentDir = Directory.GetCurrentDirectory();

        // Call the recursive method to search for the dll
        var (found, dllPath) = DllHelper.FindDll(currentDir, $"{assemblyName}.dll");

        if (found)
        {
            AnsiConsole.MarkupLine($" The dll was found at: [green]{dllPath}[/]");
            var rule = new Rule
            {
                Style = Style.Parse("blue")
            };
            AnsiConsole.Write(rule);
        }
        else
        {
            AnsiConsole.MarkupLine(
                $" The dll was not found. current directory [red] {currentDir}, [red] {assemblyName}.dll [/]");
            var rule = new Rule
            {
                Style = Style.Parse("blue")
            };
            AnsiConsole.Write(rule);
            return "";
        }

        #endregion

        return dllPath;
    }

    private static async Task Transpile(string dllPath, string outputLocation)
    {
        var assembly = Assembly.LoadFrom(dllPath);
        var dtoClasses = assembly.GetTypes().Where(type => type.Name.EndsWith("Dto")).ToList();

        foreach (var dto in dtoClasses)
        {
            AnsiConsole.MarkupLine($"DTO name--> [bold yellow on blue]{dto.Name}[/]"); //pass to File create

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

#if DEBUG
                Console.WriteLine("PropertyName --> {0}, PropertyType --> {1}", item.Name, item.PropertyType);

#endif
            }

            await FileHelper.TypeSuffix(typeFileCreated);
        }
    }
}