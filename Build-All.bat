
powershell .\Build.ps1 -cpu AnyCPU %*
if %ERRORLEVEL% NEQ 0 exit /b 1
