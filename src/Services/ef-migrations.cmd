@echo off
echo [93mChoose db context to migrate.[0m
echo [32m1)[0m IdentityConfigurationDbContext
echo [32m2)[0m IdentityPersistedGrantDbContext
echo.
:CONTEXTINPUT
set /p ContextId="[93mEnter context id (or press ENTER to exit):[0m " || goto:EOF
if %ContextId%==1 (
    cd ../Identity
    set ContextName=IdentityConfigurationDb
) else (
if %ContextId%==2 (
	cd ../Identity
    set ContextName=IdentityPersistedGrantDb
) else (
     echo Invalid context specified.
     goto CONTEXTINPUT
	)
)
echo.
set /p MigrationName="[93mEnter migration name (or press ENTER to exit):[0m " || goto:EOF
echo.
dotnet ef migrations add %MigrationName% -o Data/Migrations/%ContextName% -c %ContextName%Context
echo.
cd ../Services
pause