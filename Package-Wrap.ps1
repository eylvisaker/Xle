#
# Builds the files that will be used for the package-wrap step.
#
param (
	[string] $version,
  [string] $sourceDir = "Package",
  [string] $destDir = "Output"
)

function WrapPackage {
  param([string] $ProjectName)

  if ($version -eq "") {
    echo "Pass -version to supply a version number."
  }

  if ($version -ne "") { $version = "-$version" }

  echo "Packaging $ProjectName v$version"
  echo "Using source directory $sourceDir"
  echo "and destination directory $destDir"

  New-Item -ItemType Directory -Force -Path "$destDir"

  Copy-Item "$sourceDir\$($ProjectName)_Windows$version.zip" -Destination "$destDir\$($ProjectName)_Windows$version.zip"
}

WrapPackage "LegacyOfTheAncients"
WrapPackage "LegendOfBlacksilver"

write-output "Package wrapping on Windows completed."
