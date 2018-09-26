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

CreatePackage SmokedGB\bin\Release "SmokedGB_Windows$version"
CreatePackage Thornbridge.WindowsUniversal\bin\x64\Release "ThornbridgeSaga_XBoxOne$version"
CreatePackage Thornbridge.Android\bin\Android\AnyCPU\Release "ThornbridgeSaga_Android$version"

"Packaging complete."
