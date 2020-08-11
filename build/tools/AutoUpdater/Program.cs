using System;
using McMaster.Extensions.CommandLineUtils;

namespace AutoUpdater
{
    [Command("autoupdater", Description = "Tool to run checks and send out PRs if necessary.")]
    [Subcommand(typeof(GitHubRunnersCachedImagesCheckCommand))]
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
