// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

using System;
using Xunit.Abstractions;

namespace Microsoft.Oryx.Tests.Common
{
    /// <summary>
    /// Helper class for operations involving images in Oryx test projects.
    /// </summary>
    public class ImageTestHelper
    {
        public const string _defaultRepoPrefix = "oryxdevmcr.azurecr.io/public/oryx";
        
        private const string _repoPrefixEnvironmentVariable = "ORYX_TEST_IMAGE_BASE";
        private const string _releaseNameEnvironmentVariable = "ORYX_TEST_TAG_SUFFIX";

        private ITestOutputHelper _output;
        private string _repoPrefix;
        private string _releaseName;

        public ImageTestHelper() : this(outputHelper: null)
        {
        }

        public ImageTestHelper(ITestOutputHelper outputHelper)
        {
            var repoPrefix = Environment.GetEnvironmentVariable(_repoPrefixEnvironmentVariable);
            var releaseName = Environment.GetEnvironmentVariable(_releaseNameEnvironmentVariable);

            Initialize(outputHelper, repoPrefix, releaseName);
        }

        /// <summary>
        /// NOTE: This constructor should only be used for ImageTestHelper unit tests.
        /// </summary>
        /// <param name="outputHelper">XUnit output helper for logging.</param>
        /// <param name="repoPrefix">The image base used to mimic the ORYX_TEST_IMAGE_BASE environment variable.</param>
        /// <param name="releaseName">The tag suffix used to mimic the ORYX_TEST_TAG_SUFFIX environment variable.</param>
        public ImageTestHelper(ITestOutputHelper outputHelper, string repoPrefix, string releaseName)
        {
            Initialize(outputHelper, repoPrefix, releaseName);
        }

        /// <summary>
        /// Constructs a runtime image from the given parameters that follows the format
        /// '{image}/{platformName}:{platformVersion}{tagSuffix}'. The base image can be set with the environment
        /// variable ORYX_TEST_IMAGE_BASE, otherwise the default base 'oryxdevmcr.azurecr.io/public/oryx' will be used.
        /// If a tag suffix was set with the environment variable ORYX_TEST_TAG_SUFFIX, it will be appended to the tag.
        /// </summary>
        /// <param name="platformName">The platform to pull the runtime image from.</param>
        /// <param name="platformVersion">The version of the platform to pull the runtime image from.</param>
        /// <returns>A runtime image that can be pulled for testing.</returns>
        public string GetRuntimeImage(string platformName, string platformVersion)
        {
            return ResolveImageName($"oryx/{platformName}:{platformVersion}");
        }

        /// <summary>
        /// Constructs a 'build' image using either the default image base (oryxdevmcr.azurecr.io/public/oryx), or the
        /// base set by the ORYX_TEST_IMAGE_BASE environment variable. If a tag suffix was set with the environment
        /// variable ORYX_TEST_TAG_SUFFIX, it will be used as the tag, otherwise, the 'latest' tag will be used.
        /// </summary>
        /// <returns>A 'build' image that can be pulled for testing.</returns>
        public string GetBuildImage()
        {
            return ResolveImageName("oryx/build");
        }

        /// <summary>
        /// Constructs a 'build' or 'build:slim' image based on the provided tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetBuildImage(string tag)
        {
            return ResolveImageName($"oryx/build:{tag}");
        }

        /// <summary>
        /// Constructs a 'build:slim' image using either the default image base (oryxdevmcr.azurecr.io/public/oryx), or the
        /// base set by the ORYX_TEST_IMAGE_BASE environment variable. If a tag suffix was set with the environment
        /// variable ORYX_TEST_TAG_SUFFIX, it will be used as the tag, otherwise, the 'latest' tag will be used.
        /// </summary>
        /// <returns>A 'build:slim' image that can be pulled for testing.</returns>
        public string GetSlimBuildImage()
        {
            return ResolveImageName("oryx/build:slim");
        }

        /// <summary>
        /// Constructs a 'build:slim' image using either the default image base (oryxdevmcr.azurecr.io/public/oryx), or the
        /// base set by the ORYX_TEST_IMAGE_BASE environment variable. If a tag suffix was set with the environment
        /// variable ORYX_TEST_TAG_SUFFIX, it will be used as the tag, otherwise, the 'latest' tag will be used.
        /// </summary>
        /// <returns>A 'build:slim' image that can be pulled for testing.</returns>
        public string GetAzureFunctionsJamStackBuildImage()
        {
            return ResolveImageName("oryx/build:azfunc-jamstack");
        }

        public string GetGitHubActionsBuildImage()
        {
            return ResolveImageName("oryx/build:github-actions");
        }

        /// <summary>
        /// Constructs a 'pack' image using either the default image base (oryxdevmcr.azurecr.io/public/oryx), or the
        /// base set by the ORYX_TEST_IMAGE_BASE environment variable. If a tag suffix was set with the environment
        /// variable ORYX_TEST_TAG_SUFFIX, it will be used as the tag, otherwise, the 'latest' tag will be used.
        /// </summary>
        /// <returns>A 'pack' image that can be pulled for testing.</returns>
        public string GetPackImage()
        {
            return ResolveImageName("oryx/pack");
        }

        /// <summary>
        /// Constructs a 'cli' image using either the default image base (oryxdevmcr.azurecr.io/public/oryx), or the
        /// base set by the ORYX_TEST_IMAGE_BASE environment variable. If a tag suffix was set with the environment
        /// variable ORYX_TEST_TAG_SUFFIX, it will be used as the tag, otherwise, the 'latest' tag will be used.
        /// </summary>
        /// <returns>A 'cli' image that can be pulled for testing.</returns>
        public string GetCliImage()
        {
            return ResolveImageName("oryx/cli");
        }

        public string ResolveImageName(string imageName)
        {
            // Examples:
            // oryx/build
            // oryx/build:slim
            var imageNameParts = imageName.Split(':');

            string repo = null;
            string tag = null;
            if (imageNameParts.Length == 1)
            {
                repo = imageNameParts[0];
            }
            else if (imageNameParts.Length == 2)
            {
                repo = imageNameParts[0];
                tag = imageNameParts[1];
            }

            // Ex: oryx/build       => oryxdevmcr.azurecr.io/public/oryx/build
            // Ex: oryx/build:slim  => oryxdevmcr.azurecr.io/public/oryx/build
            var resolvedRepo = repo;
            if (!repo.StartsWith(_repoPrefix))
            {
                resolvedRepo = $"{_repoPrefix}/{repo}";
            }

            // Resolved tag names in different scenarios
            // Ex: oryx/build           => latest
            // Ex: oryx/build:latest    => latest
            // Ex: oryx/build           => 20191003.1
            // Ex: oryx/build:latest    => 20191003.1
            // Ex: oryx/build:latest    => 20191003.1-patch1
            // Ex: oryx/build:slim      => slim
            // Ex: oryx/build:slim      => slim-20191003.1
            // Ex: oryx/build:slim      => slim-20191003.1-patch1
            string resolvedTag = null;
            if (string.IsNullOrEmpty(tag) || tag == "latest")
            {
                resolvedTag = string.IsNullOrEmpty(_releaseName) ? "latest" : _releaseName;
            }
            else
            {
                resolvedTag = string.IsNullOrEmpty(_releaseName) ? tag : $"{tag}-{_releaseName}";
            }

            var finalImageName = $"{resolvedRepo}:{resolvedTag}";
            return finalImageName;
        }

        private void Initialize(ITestOutputHelper ouputHelper, string repoPrefix, string releaseName)
        {
            _output = ouputHelper;
            if (string.IsNullOrEmpty(repoPrefix))
            {
                _output?.WriteLine(
                    $"No value provided for '{nameof(repoPrefix)}', " +
                    $"using default repo prefix '{_defaultRepoPrefix}'.");
                repoPrefix = _defaultRepoPrefix;
            }

            _repoPrefix = repoPrefix;
            _repoPrefix = _repoPrefix.Trim('/');

            var lastIndex = _repoPrefix.LastIndexOf("/oryx");
            if (lastIndex >= 0)
            {
                _repoPrefix = _repoPrefix.Remove(lastIndex);
            }

            if (string.IsNullOrEmpty(releaseName))
            {
                _output?.WriteLine("No value provided for release name, no suffix will be added to image tags.");
            }

            _releaseName = releaseName;
        }
    }
}
