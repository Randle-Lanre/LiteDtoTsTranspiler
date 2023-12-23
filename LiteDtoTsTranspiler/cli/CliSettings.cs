using Spectre.Console.Cli;

namespace LiteDtoTsTranspiler.cli;

public static class CliSettings
{
    public class AddSettings : CommandSettings
    {
        [CommandArgument(0, "[PROJECT]")] public string Project { get; set; }
    }

    public class OutputFolderSettings : AddSettings
    {
        [CommandArgument(0, "<ApplicationName>")]
        public required string ApplicationName { get; set; }

        [CommandArgument(1, "<OUTPUT_FILE_PATH>")]
        public required string TranspileOutputLocation { get; set; }

        [CommandOption("-v|--version <VERSION>")]
        public string Version { get; set; }
    }
}