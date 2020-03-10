// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// --------------------------------------------------------------------------------------------

using Microsoft.Oryx.Tests.Common;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Oryx.Common.Tests
{
    public class ImageTestHelperTest
    {
        private const string _defaultRepoPrefix = "oryxdevmcr.azurecr.io/public/oryx";

        private const string _buildRepository = "build";
        private const string _packRepository = "pack";
        private const string _latestTag = "latest";
        private const string _slimTag = "slim";

        private readonly ITestOutputHelper _output;

        public ImageTestHelperTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetRuntimeImage_Validate_RepoPrefixSet()
        {
            // Arrange
            var platformName = "test";
            var platformVersion = "1.0";
            var repoPrefix = "oryxtest";
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Act
            var runtimeImage = imageHelper.GetRuntimeImage(platformName, platformVersion);

            // Assert
            var expectedImage = $"{repoPrefix}/oryx/{platformName}:{platformVersion}";
            Assert.Equal(expectedImage, runtimeImage);
        }

        [Fact]
        public void GetRuntimeImage_Validate_TagSuffixSet()
        {
            // Arrange
            var platformName = "test";
            var platformVersion = "1.0";
            var repoPrefix = string.Empty;
            var tagSuffixValue = "buildNumber";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);
            var expectedImage = $"{_defaultRepoPrefix}/{platformName}:{platformVersion}-{tagSuffixValue}";

            // Act
            var runtimeImage = imageHelper.GetRuntimeImage(platformName, platformVersion);

            // Assert
            Assert.Equal(expectedImage, runtimeImage);
        }

        [Fact]
        public void GetRuntimeImage_Validate_NoRepoPrefixOrTagSuffixSet()
        {
            // Arrange
            var platformName = "test";
            var platformVersion = "1.0";
            var repoPrefix = string.Empty;
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Act
            var runtimeImage = imageHelper.GetRuntimeImage(platformName, platformVersion);

            // Assert
            var expectedImage = $"{_defaultRepoPrefix}/{platformName}:{platformVersion}";
            Assert.Equal(expectedImage, runtimeImage);
        }

        [Fact]
        public void GetRuntimeImage_Validate_BothRepoPrefixAndTagSuffixSet()
        {
            // Arrange
            var platformName = "test";
            var platformVersion = "1.0";
            var repoPrefix = "oryxtest";
            var tagSuffixValue = "buildNumber";
            var expectedImage = $"{repoPrefix}/oryx/{platformName}:{platformVersion}-{tagSuffixValue}";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Act
            var runtimeImage = imageHelper.GetRuntimeImage(platformName, platformVersion);

            // Assert
            Assert.Equal(expectedImage, runtimeImage);
        }

        [Fact]
        public void GetBuildImage_Validate_RepoPrefixSet()
        {
            // Arrange
            var repoPrefix = "oryxtest";
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Act
            var buildImage = imageHelper.GetBuildImage();

            // Assert
            var expectedImage = $"{repoPrefix}/oryx/{_buildRepository}:{_latestTag}";
            Assert.Equal(expectedImage, buildImage);
        }

        [Fact]
        public void GetBuildImage_Validate_TagSuffixSet()
        {
            // Arrange
            var repoPrefix = string.Empty;
            var tagSuffixValue = "buildNumber";
            var expectedImage = $"{_defaultRepoPrefix}/{_buildRepository}:{tagSuffixValue}";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Act
            var buildImage = imageHelper.GetBuildImage();

            // Assert
            Assert.Equal(expectedImage, buildImage);
        }

        [Fact]
        public void GetSlimBuildImage_Validate_RepoPrefixSet()
        {
            // Arrange
            var repoPrefix = "oryxtest";
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Act
            var buildImage = imageHelper.GetSlimBuildImage();

            // Assert
            var expectedImage = $"{repoPrefix}/oryx/{_buildRepository}:{_slimTag}";
            Assert.Equal(expectedImage, buildImage);
        }

        [Fact]
        public void GetSlimBuildImage_Validate_TagSuffixSet()
        {
            // Arrange
            var repoPrefix = string.Empty;
            var tagSuffixValue = "buildNumber";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);
            var expectedImage = $"{_defaultRepoPrefix}/{_buildRepository}:{_slimTag}-{tagSuffixValue}";

            // Act
            var buildImage = imageHelper.GetSlimBuildImage();

            // Assert
            Assert.Equal(expectedImage, buildImage);
        }

        [Fact]
        public void GetPackImage_Validate_RepoPrefixSet()
        {
            // Arrange
            var repoPrefix = "oryxtest";
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);
            var expectedImage = $"{repoPrefix}/oryx/{_packRepository}:{_latestTag}";

            // Act
            var packImage = imageHelper.GetPackImage();

            // Assert
            Assert.Equal(expectedImage, packImage);
        }

        [Fact]
        public void GetPackImage_Validate_TagSuffixSet()
        {
            // Arrange
            var repoPrefix = string.Empty;
            var tagSuffixValue = "buildNumber";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);
            var expectedImage = $"{_defaultRepoPrefix}/{_packRepository}:{tagSuffixValue}";

            // Act
            var packImage = imageHelper.GetPackImage();

            // Assert
            Assert.Equal(expectedImage, packImage);
        }

        [Fact]
        public void GetBuildImage_Validate_LatestTag()
        {
            // Arrange
            var repoPrefix = string.Empty;
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);
            var expectedImage = $"{_defaultRepoPrefix}/{_buildRepository}:{_latestTag}";

            // Act
            var buildImage = imageHelper.GetBuildImage(_latestTag);

            // Assert
            Assert.Equal(expectedImage, buildImage);
        }

        [Fact]
        public void GetBuildImage_Validate_SlimTag()
        {
            // Arrange
            var repoPrefix = string.Empty;
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);
            var expectedImage = $"{_defaultRepoPrefix}/{_buildRepository}:{_slimTag}";

            // Act
            var buildImage = imageHelper.GetBuildImage(_slimTag);

            // Assert
            Assert.Equal(expectedImage, buildImage);
        }

        [Fact]
        public void GetBuildImage_Validate_InvalidTag()
        {
            // Arrange
            var repoPrefix = string.Empty;
            var tagSuffixValue = string.Empty;
            var imageHelper = new ImageTestHelper(_output, repoPrefix, tagSuffixValue);

            // Assert
            Assert.Throws<NotSupportedException>(() => { imageHelper.GetBuildImage("invalidTag"); });
        }

        // When running locally(dev boxes), release name is not set by default. It is only set in CI agents.
        [Theory]
        [InlineData("oryx/build", "registryPrefix/oryx/build:latest")]
        [InlineData("oryx/build:latest", "registryPrefix/oryx/build:latest")]
        [InlineData("registryPrefix/oryx/build", "registryPrefix/oryx/build:latest")]
        [InlineData("registryPrefix/oryx/build:latest", "registryPrefix/oryx/build:latest")]
        [InlineData("oryx/build:slim", "registryPrefix/oryx/build:slim")]
        [InlineData("registryPrefix/oryx/build:slim", "registryPrefix/oryx/build:slim")]
        public void ResolvedImageName_HasLatestTag_WhenThereIs_NoRelease(
            string originalImageName,
            string expectedImageName)
        {
            // Arrange
            var repoPrefix = "registryPrefix";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, releaseName: null);

            // Act
            var actualImageName = imageHelper.ResolveImageName(originalImageName);

            // Assert
            Assert.Equal(expectedImageName, actualImageName);
        }

        // When running in CI agents, there is a release name.
        [Theory]
        [InlineData("oryx/build:latest", "registryPrefix/oryx/build:20200101.1")]
        [InlineData("oryx/build", "registryPrefix/oryx/build:20200101.1")]
        [InlineData("registryPrefix/oryx/build", "registryPrefix/oryx/build:20200101.1")]
        [InlineData("oryx/build:slim", "registryPrefix/oryx/build:slim-20200101.1")]
        [InlineData("registryPrefix/oryx/build:slim", "registryPrefix/oryx/build:slim-20200101.1")]
        public void ResolvedImageName_HasReleaseSpecificTag_WhenThereIsRelease(
            string originalImageName,
            string expectedImageName)
        {
            // Arrange
            var repoPrefix = "registryPrefix";
            var releaseName = "20200101.1";
            var imageHelper = new ImageTestHelper(_output, repoPrefix, releaseName);

            // Act
            var actualImageName = imageHelper.ResolveImageName(originalImageName);

            // Assert
            Assert.Equal(expectedImageName, actualImageName);
        }
    }
}
