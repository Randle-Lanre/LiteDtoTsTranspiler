using LiteDtoTsTranspiler.cli;
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


   
}