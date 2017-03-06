Imports System.Runtime.InteropServices
Imports System.DirectoryServices
Imports System.ComponentModel
Imports System.Threading
Imports System.Net
Public Class Form1
    Inherits System.Windows.Forms.Form
    Private mThread As Thread = Nothing
    Public newtext As String
    Dim WithEvents Srv As UNOLibs.Net.ServerClass
    Dim Cli As New UNOLibs.Net.ClientClass 'we use new here cause client need no more initialization
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Private Sub initform(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Get local IP
        Srv = New UNOLibs.Net.ServerClass(28663, False)
        RichTextBox1.Text = "Local IP : " + Srv.LocalIP + vbCrLf
        'Search for Network Computers
        Dim childEntry As DirectoryEntry
        Dim ParentEntry As New DirectoryEntry()
        Try
            ParentEntry.Path = "WinNT:"
            For Each childEntry In ParentEntry.Children
                Dim newNode As New TreeNode(childEntry.Name)
                Select Case childEntry.SchemaClassName
                    Case "Domain"
                        Dim ParentDomain As New TreeNode(childEntry.Name)
                        TreeView1.Nodes.AddRange(New TreeNode() {ParentDomain})

                        Dim SubChildEntry As DirectoryEntry
                        Dim SubParentEntry As New DirectoryEntry()
                        SubParentEntry.Path = "WinNT://" & childEntry.Name
                        For Each SubChildEntry In SubParentEntry.Children
                            Dim newNode1 As New TreeNode(SubChildEntry.Name)
                            Select Case SubChildEntry.SchemaClassName
                                Case "Computer"
                                    ParentDomain.Nodes.Add(newNode1)
                            End Select
                        Next
                End Select
            Next
        Catch Excep As Exception
            MsgBox("Error While Reading Directories")
        Finally
            ParentEntry = Nothing
        End Try
        SplashScreen1.Close()
        StartStop()
    End Sub
    Private Sub AddText(ByVal [text] As String)
        Me.RichTextBox1.Text += [text] & vbCrLf
    End Sub
    Private Sub ThreadProcSafe()
        ' Check if this method is running on a different thread
        ' than the thread that created the control.
        If Me.RichTextBox1.InvokeRequired Then
            ' It's on a different thread, so use Invoke.
            Dim d As New SetTextCallback(AddressOf AddText)
            Me.Invoke(d, New Object() {[newtext]}) ' Invoke
        Else
            ' It's on the same thread, no need for Invoke.
            Me.RichTextBox1.Text = [newtext] & vbCrLf ' No invoke
        End If
    End Sub
    'Start the server on a specified port / stop function.
    Public Sub StartStop()
        If Not Srv.isRunning Then
            'Clear messages from previous session(don't care to this)
            'Start the server
            Srv = Nothing
            Srv = New UNOLibs.Net.ServerClass(28662, True)
            'It's already Started!
            RichTextBox1.Text += "Server Started" & vbCrLf & "Looking for connections" & vbCrLf & vbCrLf
        Else
            'Stop and terminate the server
            Srv.StopServer()
            RichTextBox1.Text += "Server terminated" & vbCrLf & vbCrLf
        End If
    End Sub
    Public Sub totextbox()
        ' Create a background thread and start it.
        Me.mThread = New Thread( _
            New ThreadStart(AddressOf Me.ThreadProcSafe))
        Me.mThread.Start()
    End Sub
    Private Sub OnIncomingMessage(ByVal Argus As UNOLibs.Net.ServerClass.InMessEvArgs) Handles Srv.IncomingMessage
        'Example of Reading and display Message
        newtext = Argus.message & " (" & Argus.senderIP & ")"
        totextbox()
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        txtcomputer.Text = TreeView1.SelectedNode.Text
        TextBox1.Text = GetIPAddress(txtcomputer.Text)
    End Sub

    Function GetIPAddress(ByVal CompName As String) As String
        Dim oAddr As System.Net.IPAddress
        Dim sAddr As String
        Try
            With System.Net.Dns.GetHostByName(CompName)
                oAddr = New System.Net.IPAddress(.AddressList(0).Address)
                sAddr = oAddr.ToString
            End With
            GetIPAddress = sAddr
        Catch Excep As Exception
            MsgBox(Excep.Message, MsgBoxStyle.OkOnly, "Lan Messenger")
            TextBox1.Focus()

        Finally

        End Try
    End Function

    Private Sub Send_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Read DATA from the form
        Dim IP As String = TextBox1.Text
        Dim Port As Integer = 28662
        Dim DATA As String = TextBox2.Text
        Dim NICK As String = TextBox3.Text
        If NICK = "" Then
            MsgBox("Please enter a nickname", MsgBoxStyle.Critical, "Error")
        Else
            'Now send it.
            Try
                Cli.SendMessage(IP, Port, NICK & ": " & DATA)
                RichTextBox1.Text += "You -> " & txtcomputer.Text & " (" & IP & "): " + DATA & vbCrLf
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error sending message")
            End Try
        End If
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Send_Click(sender, e)
        End If
    End Sub
End Class
