@Echo off

IF [%1] neq [] chdir %1

call DbDeploy.exe deploy -p SqlServer
if %errorlevel% neq 0 exit /b %errorlevel%

