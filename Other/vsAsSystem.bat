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
ECHO Running visual studio as system...
C:\Windows\pstools\PsExec.exe -i -s "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"
EXIT
