#!/bin/sh

#usage: nuget-pack <version>

if [[ "$1" == "-h" ]]; then print-file-comments "$0"; exit; fi

version="$1"
if [[ -z "$version" ]]; then
	echo 1>&2 "fatal: version required"
	exec print-file-comments "$0"
	exit 1
fi

tag="nuget-$version"
commit_hash=$(git rev-parse @)
metadata="$commit_hash"

dotnet pack src/CSharpCollections/CSharpCollections.csproj \
	-c Release \
	--include-symbols //p:SymbolPackageFormat=snupkg \
	-o .nupkg/ \
	&& git tag "$tag" -m "Create nuget tag $tag"
