#!/bin/bash
# --------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.
# --------------------------------------------------------------------------------------------

set -e

declare -r REPO_DIR=$( cd $( dirname "$0" ) && cd .. && cd .. && pwd )

# Load all variables
source $REPO_DIR/build/detector/__variables.sh

solutionFileName="Detector.sln"

echo
echo "Building $solutionFileName..."
echo
cd $REPO_DIR/
dotnet build -c $BUILD_CONFIGURATION $solutionFileName