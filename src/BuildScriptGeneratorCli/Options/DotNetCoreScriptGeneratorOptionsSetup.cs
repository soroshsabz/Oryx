// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Oryx.BuildScriptGenerator.DotNetCore;

namespace Microsoft.Oryx.BuildScriptGeneratorCli.Options
{
    public class DotNetCoreScriptGeneratorOptionsSetup
        : OptionsSetupBase, IConfigureOptions<DotNetCoreScriptGeneratorOptions>
    {
        public DotNetCoreScriptGeneratorOptionsSetup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public void Configure(DotNetCoreScriptGeneratorOptions options)
        {
            options.DotNetVersion = GetStringValue(SettingsKeys.DotNetVersion);
            options.Project = GetStringValue(SettingsKeys.Project);
            options.MSBuildConfiguration = GetStringValue(SettingsKeys.MSBuildConfiguration);
        }
    }
}
