param (
	[string] $version
)

$destDir = "Output"

if ($version -ne "") { $version = "-$version" }

function CreatePackage($sourceDir, $packageName)
{
	$destination = "$($destDir)\$($packageName).zip"
	
	if (Test-Path $destination) { Remove-Item $destination }
	
	"Creating $destination"
	[IO.Compression.ZipFile]::CreateFromDirectory($sourceDir, $destination)
}

Add-Type -assembly "System.IO.Compression.FileSystem"

$dummy = New-Item -ItemType Directory -Force -Path $destDir

CreatePackage LegacyOfTheAncients\bin\Release "LegacyOfTheAncients_Windows$version"
CreatePackage LegendOfBlacksilver\bin\Release "LegendOfBlacksilver_Windows$version"

"Packaging complete."
