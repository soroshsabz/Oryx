echo
echo "Using Node version:"
node --version
echo
{{ PackageInstallerVersionCommand }}

{{ if PackageRegistryUrl | IsNotBlank }}
echo
echo "Adding package registry to .npmrc: {{ PackageRegistryUrl }}"
echo "registry={{ PackageRegistryUrl }}" >> ~/.npmrc
echo
{{ end }}

{{ if ConfigureYarnCache }}
# Yarn config is per user, and since the build might run with a non-root account, we make sure
# the yarn cache is set on every build.
YARN_CACHE_DIR=/usr/local/share/yarn-cache
if [ -d $YARN_CACHE_DIR ]
then
	echo
    echo "Configuring Yarn cache folder..."
    yarn config set cache-folder $YARN_CACHE_DIR
fi
{{ end }}

zippedModulesFileName={{ CompressedNodeModulesFileName }}
PruneDevDependencies={{ PruneDevDependencies }}

cd "$SOURCE_DIR"

echo
echo "Running '{{ PackageInstallCommand }}'..."
echo
{{ PackageInstallCommand }}

{{ if CustomRunBuildCommand | IsNotBlank }}
	echo
	{{ CustomRunBuildCommand }}
	echo
{{ else }}
	{{ if NpmRunBuildCommand | IsNotBlank }}
	echo
	echo "Running '{{ NpmRunBuildCommand }}'..."
	echo
	{{ NpmRunBuildCommand }}
	{{ end }}

	{{ if NpmRunBuildAzureCommand | IsNotBlank }}
	echo
	echo "Running '{{ NpmRunBuildAzureCommand }}'..."
	echo
	{{ NpmRunBuildAzureCommand }}
	{{ end }}
{{ end }}

{{ if RunNpmPack }}
echo
echo "Running custom packaging scripts that might exist..."
echo
npm run package || true
echo
echo "Running 'npm pack'..."
echo
npm pack
{{ end }}

if [ "$PruneDevDependencies" == "true" ]
then
	echo
	echo "Pruning dev dependencies..."
	npm prune --production
fi

{{ if CompressNodeModulesCommand | IsNotBlank }}
if [ "$SOURCE_DIR" != "$DESTINATION_DIR" ]
then
	if [ -f $zippedModulesFileName ]; then
		echo
		echo "File '$zippedModulesFileName' already exists under '$SOURCE_DIR'. Deleting it..."
		rm -f $zippedModulesFileName
	fi

	if [ -d node_modules ]
	then
		echo
		echo Zipping existing 'node_modules' folder...
		START_TIME=$SECONDS
		# Make the contents of the node_modules folder appear in the zip file, not the folder itself
		cd node_modules
		{{ CompressNodeModulesCommand }} ../$zippedModulesFileName .
		ELAPSED_TIME=$(($SECONDS - $START_TIME))
		echo "Done in $ELAPSED_TIME sec(s)."
	fi
fi
{{ end }}
