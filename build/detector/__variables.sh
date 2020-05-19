#!/bin/bash
set -e

# Since this file is expected to be 'sourced' from any script under any folder in this repo, 
# we cannot get the repo directory from this file itself and instead rely on the parent script
# which does the sourcing.
if [ -z "$REPO_DIR" ]; then
    "The variable 'REPO_DIR' cannot be empty. It should point to repo root."
    exit 1
fi

declare -r BUILD_CONFIGURATION="${BUILDCONFIGURATION:-Debug}"
declare -r ARTIFACTS_DIR="$REPO_DIR/artifacts"
declare -r ARTIFACTS_NUGET_PACKAGES_DIR="$ARTIFACTS_DIR/packages"
declare -r TESTS_SRC_DIR="$REPO_DIR/tests"
