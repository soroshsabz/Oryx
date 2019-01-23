﻿// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

namespace Microsoft.Oryx.BuildScriptGenerator.Python
{
    /// <summary>
    /// Build script template for Python in Bash.
    /// </summary>
    public partial class PythonBashBuildSnippet
    {
        public PythonBashBuildSnippet(
            string virtualEnvironmentName,
            string virtualEnvironmentModule,
            string virtualEnvironmentParameters,
            string packagesDirectory)
        {
            VirtualEnvironmentName = virtualEnvironmentName;
            VirtualEnvironmentModule = virtualEnvironmentModule;
            VirtualEnvironmentParameters = virtualEnvironmentParameters;
            PackagesDirectory = packagesDirectory;
        }

        public string VirtualEnvironmentName { get; set; }

        public string VirtualEnvironmentModule { get; set; }

        public string VirtualEnvironmentParameters { get; set; }

        /// <summary>
        /// Gets or sets the directory where the packages will be downloaded to.
        /// </summary>
        public string PackagesDirectory { get; set; }
    }
}