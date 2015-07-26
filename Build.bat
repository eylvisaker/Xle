@ECHO OFF

msbuild.exe Xle.sln /T:rebuild /P:Configuration=%1
