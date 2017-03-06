@echo off
:: BatchGotAdmin - https://sites.google.com/site/eneerge/home/BatchGotAdmin - modified by niikoo
:-------------------------------------
REM  --> Check for permissions
AT >NUL
REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    echo UAC.ShellExecute "C:\WINDOWS\SYSTEM32\CMD.EXE", "/C %~s0", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    if exist "%temp%\getadmin.vbs" ( del "%temp%\getadmin.vbs" )
    pushd "%CD%"      
    CD /D "%~dp0"
:--------------------------------------
ECHO Fixing x2utilGH.dll missing problems
mklink /H C:\windows\system32\x2utilGH.dll C:\windows\system32\spool\drivers\x64\3\x2utilGH.dll
EXIT
