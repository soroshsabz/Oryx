#!/bin/bash
# --------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.
# --------------------------------------------------------------------------------------------

set -ex

CURRENT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
source $CURRENT_DIR/../__common.sh

echo
echo "Installing .NET Core SDK $DOTNET_SDK_VER ..."
echo

fileName="dotnet.tar.gz"
downloadFileAndVerifyChecksum dotnet $DOTNET_SDK_VER $fileName

DOTNET_DIR="/opt/dotnet"
mkdir -p $DOTNET_DIR
tar -xzf $fileName -C $DOTNET_DIR
rm $fileName
dotnet="$DOTNET_DIR/dotnet"

# Install MVC template based packages
if [ "$INSTALL_PACKAGES" != "false" ]
then
    echo
    echo Installing MVC template based packages...
    sampleAppDir="/tmp/warmup"
    mkdir "$sampleAppDir"
    cd "$sampleAppDir"
    globalJsonContent="{\"sdk\":{\"version\":\"$DOTNET_SDK_VER\"}}"
    echo "$globalJsonContent" > global.json
    $dotnet new mvc
    $dotnet restore
    cd ..
    rm -rf "$sampleAppDir"
fi
