@Echo off

IF [%1] neq [] chdir %1

call DbDeploy.exe deploy -p PostgreSQL
if %errorlevel% neq 0 exit /b %errorlevel%

