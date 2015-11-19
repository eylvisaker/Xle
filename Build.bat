@ECHO OFF

nuget restore Xle.sln
if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

msbuild.exe Xle.sln /T:rebuild /P:Configuration=%1
