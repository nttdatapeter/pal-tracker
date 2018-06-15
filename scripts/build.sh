#!/usr/bin/env bash
set -ex

DISABLE_AUTH=true dotnet test ./test/PalTrackerTests
artifacts_path=/tmp/artifacts

build_output="/tmp/build-output"
artifacts_path="./artifacts"
version=$1

mkdir -p $build_output
mkdir -p $artifacts_path

cp manifest-*.yml $build_output

dotnet publish src/PalTracker --configuration Release \
    --output $build_output/src/PalTracker/bin/Release/netcoreapp2.1/publish

tar -C $build_output/ -cvzf $artifacts_path/pal-tracker-$version.tgz .
