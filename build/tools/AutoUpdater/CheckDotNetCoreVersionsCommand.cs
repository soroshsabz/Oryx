using System;
using System.IO;
using System.Net.Http;
using McMaster.Extensions.CommandLineUtils;

namespace AutoUpdater
{
    [Command("get-new-dotnet-core-versions", Description = "")]
    public class CheckDotNetCoreVersionsCommand
    {
        public int OnExecute(CommandLineApplication app, IConsole console)
        {
            
            return 0;
        }
    }
}
