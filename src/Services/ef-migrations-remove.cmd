@echo off
echo [93mChoose db context to migrate.[0m
echo [32m1)[0m FixedDiscountLoyaltyContext
REM echo [32m2)[0m TokensContext
echo.
:CONTEXTINPUT
set /p ContextId="[93mEnter context id (or press ENTER to exit):[0m " || goto:EOF
if %ContextId%==1 (
    set ContextName=FixedDiscountLoyalty
) else (
REM if %ContextId%==2 (
REM     set ContextName=Tokens
REM ) else (
     echo Invalid context specified.
     goto CONTEXTINPUT
REM 	)
)
echo.
dotnet ef migrations remove -c %ContextName%Context
echo.
pause