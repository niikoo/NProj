Imports System.IO
Public Class Form1
    Public Sub SelfDestruct()
        Dim path As String = "destructor.bat"
        If Not File.Exists(path) Then
            ' Create a file to write to.
            Using sw As StreamWriter = File.CreateText(path)
                sw.WriteLine("@echo off")
                sw.WriteLine(":loop")
                sw.WriteLine("DEL SelfDestruct.exe")
                sw.WriteLine("CLS")
                sw.WriteLine("IF NOT EXIST SelfDestruct.exe GOTO end")
                sw.WriteLine("GOTO loop")
                sw.WriteLine(":end")
                sw.WriteLine("del destructor.bat")
            End Using
            System.Diagnostics.Process.Start(path)
        End If
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Close()
        SelfDestruct()
        Application.Exit()
    End Sub
End Class
