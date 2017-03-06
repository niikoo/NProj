Imports System.IO
Imports System
Imports System.Windows.Forms
Public Class Form1
    Dim openpath As String = ""
    Dim globala1, globala2 As Integer
    Dim lagret = 1

    Public Function GetFileContents(ByVal FullPath As String, _
       Optional ByRef ErrInfo As String = "") As String

        Dim strContents As String
        Dim objReader As StreamReader
        Try
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return False
    End Function

    Public Function FilePutContent(ByVal strData As String, _
     ByVal FullPath As String, _
       Optional ByVal ErrInfo As String = "") As Boolean
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function

    Private Sub AvsluttToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AvsluttToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Function ÅpneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ÅpneToolStripMenuItem.Click
        Dim openfile, tmpopath As String
        Dim msgresult As Integer
        If lagret <> 1 Then
            msgresult = MsgBox("Du har ikke lagret prosjektet, vil du lagre det nå?", MsgBoxStyle.YesNoCancel) 'ja = 6, nei=7, cancel=2
            If msgresult = 2 Then
                Return False
            ElseIf msgresult = 6 Then
                LagreToolStripMenuItem_Click(sender, e)
                Return False
            End If
        End If
        OpenFileDialog1.ShowDialog()
        tmpopath = OpenFileDialog1.FileName
        If tmpopath <> "" Then
            openpath = tmpopath
            openfile = GetFileContents(openpath)
            RichTextBox1.Text = openfile
            TextBox1.Text = "Fil: " & openpath
            Return True
        End If
        Return False
    End Function

    Private Sub LagreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LagreToolStripMenuItem.Click
        Dim errorx As String
        Dim skip As Integer
        If openpath = "" Then
            SaveFileDialog1.ShowDialog()
            openpath = SaveFileDialog1.FileName
            If openpath = "" Then
                skip = 1
            End If
        End If
        If skip <> 1 Then
            errorx = FilePutContent(RichTextBox1.Text, openpath)
            If errorx = True Then
                MsgBox("Filen er lagret")
            Else
                MsgBox("Feil under lagring, prøv igjen", MsgBoxStyle.Information, "Nikolai's Notepad")
                openpath = ""
            End If
            lagret = 1
        End If
        TextBox1.Text = "Fil: " & openpath
    End Sub

    Private Sub RichTextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox1.TextChanged
        lagret = 0
    End Sub

    Private Sub LagreSomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LagreSomToolStripMenuItem.Click
        Dim errorx As String
        Dim skip As Integer = 1
        openpath = ""
        SaveFileDialog1.ShowDialog()
        openpath = SaveFileDialog1.FileName
        If openpath <> "" Then
            skip = 0
        End If
        If skip = 0 Then
            errorx = FilePutContent(RichTextBox1.Text, openpath)
            If errorx = True Then
                MsgBox("Filen er lagret")
            Else
                MsgBox("Feil under lagring, prøv igjen")
                openpath = ""
            End If
        End If
        TextBox1.Text = "Fil: " & openpath
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If System.Environment.GetCommandLineArgs.Length = 2 Then
            openpath = System.Environment.GetCommandLineArgs(1)
        End If
        If openpath <> "" Then
            RichTextBox1.Text = GetFileContents(openpath)
            TextBox1.Text = "Fil: " & openpath
        End If
    End Sub

    Private Sub OmToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OmToolStripMenuItem.Click
        MsgBox("Nikolai's Notepad av niikoo", MsgBoxStyle.OkOnly, "Om")
    End Sub
End Class