Param(
  [ValidateSet("Debug", "Release")]
  [string]$config = "Debug",

  [ValidateSet("AnyCPU", "x86", "x64", "ARM")]
  [string]$cpu = "AnyCPU",
  
  [string]$version
)

$solutionFile = "Xle.sln"

$ErrorActionPreference = "Stop"

$buildArgs = @()
$buildArgs += $solutionFile
$buildArgs += "/T:Build"
$buildArgs += "/P:Configuration=$config"

if ($cpu -ne "AnyCPU") {
	$buildArgs += "/P:Platform=$cpu"
}

if (![string]::IsNullOrEmpty($version)) {
    $versions = $version.Split(".");
    
    if ($versions.Length -ne 3) {
        echo "Version string must be Major.Minor.Build format."
        exit 1
    }
    
    $date = Get-Date
    $revision = ($date.Year % 100).ToString("00") + $date.DayOfYear.ToString("000")
    
    $buildArgs += "/P:VersionMajor=$($versions[0])"
    $buildArgs += "/P:VersionMinor=$($versions[1])"
    $buildArgs += "/P:VersionBuild=$($versions[2])"
    $buildArgs += "/P:VersionRevision=$($revision)"
    
    echo "Setting version to $($versions[0]).$($versions[1]).$($versions[2]).$($revision)"
}

function Initialize
{
	$cd = $MyInvocation.MyCommand.Path
	
	$env:Path = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin;$($env:PATH)"
}

function RestoreDependencies
{
	.\.nuget\nuget.exe restore $solutionFile
	if ($LastExitCode -ne 0) { exit $LastExitCode }
}

function Build
{
	& msbuild.exe $buildArgs
	if ($LastExitCode -ne 0) { exit $LastExitCode }
}

#----------------------------------------- End functions -------------------------------------------


Initialize
RestoreDependencies
Build
