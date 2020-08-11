using System;
using McMaster.Extensions.CommandLineUtils;

namespace AutoUpdater
{
    [Command("autoupdater", Description = "Tool to run checks and send out PRs if necessary.")]
    [Subcommand(typeof(GitHubRunnersCachedImagesCheckCommand))]
    class Program
    {
        internal int OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp();

            return 0;
        }

        private static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);
    }
}
