
$env:path = $env:path + ";C:\Program Files (x86)\MSBuild\14.0\bin;C:\sonar-runner\bin"
$pwd = Convert-Path .
$coverageReportPath = "$pwd\Coverage.xml"
"Using coverage report path = $coverageReportPath"

if (! $?) { throw "Sonar analysis initialization failed." }

.\Build.bat Debug
if (! $?) { throw "Build failed." }

.\Coverage.bat
if (! $?) { throw "Coverage execution failed." }

sonar-runner
if (! $?) { throw "Sonar analysis failed." }
