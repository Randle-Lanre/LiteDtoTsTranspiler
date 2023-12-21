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
            if (settings.PackageName != "cow")
            {
                return ValidationResult.Error($"you requested for -{settings.PackageName} - instead of cow");
            }

            return base.Validate(context, settings);
        }
    }
}