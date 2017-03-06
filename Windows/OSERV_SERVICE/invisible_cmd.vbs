'nom1@IRIS - SERVICE INSTALL HELPER
Dim WSHShell,objFileSystem,outFile,strCMD,strBatchFileLocation,strTempDir,objFolder
Set objFileSystem = Wscript.CreateObject("Scripting.FileSystemObject")
strTempDir = "C:\temp\"
If objFileSystem.FolderExists(strTempDir) Then
   Set objFolder = objFileSystem.GetFolder(strTempDir)
Else
   Set objFolder = objFileSystem.CreateFolder(strTempDir)
End If
strBatchFileLocation = strTempDir & "is.bat"
Set WSHShell = WScript.CreateObject("WScript.Shell")
objFileSystem.CreateTextFile(strBatchFileLocation)
set outFile = objFileSystem.OpenTextFile(strBatchFileLocation, 2, true)
strCMD=wscript.arguments(0)
outFile.WriteLine strCMD
outFile.Close
WSHShell.Run(strBatchFileLocation), 0, true
objFileSystem.DeleteFile strBatchFileLocation, True