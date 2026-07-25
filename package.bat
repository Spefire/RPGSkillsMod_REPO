@echo off
setlocal

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0package.ps1" %*
set "EXITCODE=%ERRORLEVEL%"

if not "%EXITCODE%"=="0" (
    echo.
    echo Le packaging a echoue.
)

pause
exit /b %EXITCODE%
