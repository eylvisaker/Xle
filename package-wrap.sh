#!/bin/bash

source ./project-vars.sh

version=$1
sourceDir=$2
destDir=$3

projectRoot=`pwd`
tmpRoot=tmp
tmpDir="$tmpRoot/$ProjectName"

echo "Packaging $ProjectName v$version"
echo "Using source directory $sourceDir"
echo "and destination directory $destDir"

if [ -n "$version" ]; then
  version="-$version"
fi

mkdir -p $destDir
mkdir -p $tmpDir/lib

unzip -o "$sourceDir/${ProjectName}_Desktop${version}.zip" -d "$tmpDir/lib"

cp Linux/* $tmpDir

cd $tmpRoot

tar zcvf "$projectRoot/$destDir/${ProjectName}_Linux${version}.tar.gz" ./$ProjectName

