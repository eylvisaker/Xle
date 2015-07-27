@ECHO OFF

nuget restore Xle.sln
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

cd Agate
nuget restore AgateLib-Windows.sln
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

cd ..

msbuild.exe Xle.sln /T:rebuild /P:Configuration=%1
