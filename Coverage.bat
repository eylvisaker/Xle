
packages\OpenCover.4.6.519\tools\OpenCover.Console.exe "-target:Test.bat" -excludebyfile:*\*.Designer.cs -output:Coverage.xml -register:user
@if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe -reports:Coverage.xml -targetdir:TestReport -reporttypes:html;htmlchart;htmlsummary -assemblyfilters:-MoonSharp.Interpreter;-Moq;
@if %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

