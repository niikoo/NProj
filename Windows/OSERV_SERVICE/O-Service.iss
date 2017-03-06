#define MyAppName "IRIS-Service"
#define MyAppPublisher "International Research Institute of Stavanger AS"
#define MyAppURL "http://www.iris.no"
#define IRISServicePath "PPKG_SERVICE\bin\x64\Release\Confused"
#define MyAppVersion GetFileVersion('PPKG_SERVICE\bin\x64\Release\Confused\IRIS-Service.exe')
;Release\Confused Release\Confused\IRIS-Service.exe -> for obfuscated
[Setup]
AppID={{E3003D49-F24D-4F3F-ACB5-BB51B39F79E6}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
CreateAppDir=false
OutputBaseFilename={#MyAppName}_Setup
Compression=lzma2/ultra64
SolidCompression=True
RestartApplications=false
CloseApplications=false
;UninstallLogMode=overwrite
;UninstallDisplayIcon={sys}\IRIS-Service.exe
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppName} Setup
VersionInfoCopyright={#MyAppPublisher}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}
Uninstallable=false
UsePreviousGroup=false
AppendDefaultGroupName=false
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
InternalCompressLevel=ultra
ShowLanguageDialog=auto
DisableReadyPage=true
AppCopyright={#MyAppPublisher}
AllowCancelDuringInstall=False
AppContact=Nikolai Ommundsen - nikolai@lyse.net
MinVersion=0,6.1

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
[Code]
procedure DoBeforeInstall();
var
  ResultCode: Integer;
begin
  // Launch Uninstall and wait for it to terminate
  if Exec(ExpandConstant('{cmd}'), ExpandConstant('/C {sys}\IRIS-Service.exe --uninstall'), ExpandConstant('{sys}'), SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    // handle success if necessary; ResultCode contains the exit code
  end
  else begin
    // handle failure if necessary; ResultCode contains the error code
  end;
  // Proceed Setup
end;

[Dirs]
Name: {win}\GLOW; Permissions: users-readexec; 
[Files]
Source: C:\Windows\GLOW\ddi.igs; DestDir: {win}\GLOW; Permissions: users-readexec; Flags: comparetimestamp overwritereadonly;
Source: C:\Windows\GLOW\appLimiter.conf; DestDir: {win}\GLOW; Permissions: users-readexec; Flags: comparetimestamp overwritereadonly;
Source: C:\Windows\GLOW\noIRISProxy.conf; DestDir: {win}\GLOW; Permissions: users-readexec; Flags: comparetimestamp overwritereadonly;
Source: "{#IRISServicePath}\IRIS-Service.exe"; DestDir: {sys}; Permissions: users-readexec; Flags: 64bit ignoreversion overwritereadonly; BeforeInstall: DoBeforeInstall; 
;Source: "P:\SVG\821\Visual Basic\IRIS SERVICE\invisible_cmd.vbs"; DestDir: {win}; Permissions: users-readexec; Flags: deleteafterinstall; 
[Run]
;Filename: "wscript"; Parameters: "//B //Nologo {win}\invisible_cmd.vbs ""{sys}\IRIS-Service.exe --uninstall > nul"""; Flags: 64bit
;Filename: "wscript"; Parameters: "//B //Nologo {win}\invisible_cmd.vbs ""{sys}\IRIS-Service.exe --install > nul&&NET START IRIS-Service > nul"""; Flags: 64bit
;Filename: "{cmd}"; Parameters: "/C @ECHO OFF&&ECHO Uninstalling IRIS-Service&&{sys}\IRIS-Service.exe --uninstall"; Flags: 64bit runhidden
Filename: "{cmd}"; Parameters: "/C @ECHO OFF&&ECHO Installing IRIS-Service&&{sys}\IRIS-Service.exe --install&&timeout 5&&{sys}\IRIS-Service.exe --sendCommand event::logon"; Flags: 64bit runhidden
;Filename: "{cmd}"; Parameters: "/C @ECHO OFF&&ECHO Starting IRIS-Service&&NET START IRIS-Service"; Flags: 64bit runhidden