#!/bin/bash
set -ex

# https://medium.com/@Drew_Stokes/bash-argument-parsing-54f3b81a6a8f
PARAMS=""
while (( "$#" )); do
  case "$1" in
    -d|--dir)
      targetDir=$2
      shift 2
      ;;
    -p|--platform)
      platform=$2
      shift 2
      ;;
    -v|--sdk-version)
      sdkVersion=$2
      shift 2
      ;;
    --runtime-version)
      runtimeVersion=$2
      shift 2
      ;;
    -u|--base-url)
      storageBaseUrl=$2
      shift 2
      ;;
    --) # end argument parsing
      shift
      break
      ;;
    -*|--*=) # unsupported flags
      echo "Error: Unsupported flag $1" >&2
      exit 1
      ;;
    *) # preserve positional arguments
      PARAMS="$PARAMS $1"
      shift
      ;;
  esac
done
# set positional arguments in their proper place
eval set -- "$PARAMS"

function printUsage() {
    echo "Usage: installPlatform.sh -p <platform-name> -v <platform-version> -d <directory-to-install> -u <storage-url>"
    exit 1
}

defaultInstallationRootDir="/tmp/oryx/platforms"
if [ -z "$platform" ]; then
    echo "Platform name is required."
    printUsage
fi

if [ -z "$sdkVersion" ]; then
    echo "Platform version is required."
    printUsage
fi

if [ -z "$storageBaseUrl" ]; then
    storageBaseUrl="https://oryx-cdn.microsoft.io"
fi

fileName="$platform-$sdkVersion.tar.gz"

# File to indicate that a download completed successfully
sentinelFileName=".oryx-sdkdownload-sentinel"

# Name of the file which gets created in the runtime version directory
# of .NET Core. This file would contain the sdk version which is used
# by benv to find the sdk version.
dotnetRuntimeVersionToSdkVersionInfoFileName="sdkVersion.txt"

function getInstallDir() {
  if [ ! -z "$targetDir" ]; then
    echo "$targetDir"
  elif [ "$platform" == "dotnet" ]; then
    echo "$defaultInstallationRootDir/$platform/sdks/$sdkVersion"
  else
    echo "$defaultInstallationRootDir/$platform/$sdkVersion"
  fi
}

function downloadSdk() {
    local platformName="$1"
    local version="$2"
    local downloadedFileName="$3"
    local headersFile="/tmp/headers.txt"

    local PLATFORM_BINARY_DOWNLOAD_START=$SECONDS
    echo "Downloading $platformName version '$sdkVersion'..."

    if [ "$platformName" == "hugo" ]; then
        curl \
          -fsSLO \
          --compressed \
          "https://github.com/gohugoio/hugo/releases/download/v$sdkVersion/hugo_extended_${version}_Linux-64bit.tar.gz" \
          --output "$sdkVersion.tar.gz" \
          >/dev/null 2>&1
    else
        curl \
            -D $headersFile \
            -SL "$storageBaseUrl/$platformName/$platformName-$sdkVersion.tar.gz" \
            --output $downloadedFileName

        # Use all lowercase letters to find the header and it's value
        headerName="x-ms-meta-checksum"
        # Search the header ignoring case
        local checksumHeader=$(cat $headersFile | grep -i $headerName: | tr -d '\r')
        # Change the found header and value to lowercase
        checksumHeader=$(echo $checksumHeader | tr '[A-Z]' '[a-z]')
        local checksumValue=${checksumHeader#"$headerName: "}
        rm -f $headersFile
        echo
        echo "Verifying checksum..."
        echo "$checksumValue $downloadedFileName" | sha512sum -c -
    fi
    local PLATFORM_BINARY_DOWNLOAD_ELAPSED_TIME=$(($SECONDS - $PLATFORM_BINARY_DOWNLOAD_START))
    echo "Downloaded in $PLATFORM_BINARY_DOWNLOAD_ELAPSED_TIME sec(s)."
}

PLATFORM_SETUP_START=$SECONDS
echo
echo "Downloading and extracting '$platform' version '$sdkVersion' to '$targetDir'..."
installDir=$(getInstallDir)
rm -rf "$installDir"
mkdir -p "$installDir"
cd "$installDir"

downloadSdk $platform $sdkVersion $sdkVersion.tar.gz

echo Extracting contents...
tar -xzf $sdkVersion.tar.gz -C .
rm -f $sdkVersion.tar.gz
PLATFORM_SETUP_ELAPSED_TIME=$(($SECONDS - $PLATFORM_SETUP_START))
echo "Done in $PLATFORM_SETUP_ELAPSED_TIME sec(s)."
echo
``
# Write out a sentinel file to indicate downlaod and extraction was successful
echo > "$installDir/$sentinelFileName"

if [ "$platform" == "dotnet" ] && [ "$installDir" != "$targetDir" ]; then
  runtimeVersionDir="$defaultInstallationRootDir/$platform/runtimes/$runtimeVersion"
  mkdir -p "$runtimeVersionDir"
  echo "$sdkVersion" > "$runtimeVersionDir/"
  echo > "$runtimeVersionDir/$sentinelFileName"
fi