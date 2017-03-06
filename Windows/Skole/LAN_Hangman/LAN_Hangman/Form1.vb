Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Threading
Imports System.Net
Public Class Form1
    'PORTS: 28664 (main) , 28665 (ip check)
    Inherits System.Windows.Forms.Form
    Private mThread As Thread = Nothing
    Public newtext As String
    Public recive As Boolean = True
    Dim WithEvents Srv As UNOLibs.Net.ServerClass
    Dim Cli As New UNOLibs.Net.ClientClass 'we use new here cause client need no more initialization
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Private Sub initform(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Get local IP
        Srv = New UNOLibs.Net.ServerClass(28665, False)
        ' RichTextBox1.Text = "Local IP : " + Srv.LocalIP + vbCrLf
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
            Srv = New UNOLibs.Net.ServerClass(28664, True)
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
        'Recive message
        If recive = True Then
            newtext = "Nytt ord motatt (fra IP: " & Argus.senderIP & ")"
            totextbox()
            newword(Argus.message)
        End If
    End Sub '

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
        Dim Port As Integer = 28664
        Dim DATA As String = TextBox2.Text
        'Now send it.
        Try
            Cli.SendMessage(IP, Port, DATA)
            RichTextBox1.Text += "You ->" & IP & ": " + DATA & vbCrLf
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error sending message")
        End Try
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Send_Click(sender, e)
        End If
    End Sub
    Private Sub TextBox3_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        TextBox3.Focus()
        If e.KeyCode = Keys.E Then
            foreslaa_Click()
        End If
    End Sub

    Private Sub foreslaa_Click() Handles foreslaa.Click
        TextBox3.Focus()
    End Sub

    Private Sub newword(ByVal text As String)
        gue1.Text = ""
        gue2.Text = ""
        gue3.Text = ""
        gue4.Text = ""
        gue5.Text = ""
        recive = False
        Dim wlenght As Integer = text.Length
        Select Case wlenght
            Case 1
                gue1.Text = "_"
            Case 2
                gue1.Text = "_"
                gue2.Text = "_"
            Case 3
                gue1.Text = "_"
                gue2.Text = "_"
                gue3.Text = "_"
            Case 4
                gue1.Text = "_"
                gue2.Text = "_"
                gue3.Text = "_"
                gue4.Text = "_"
            Case 5
                gue1.Text = "_"
                gue2.Text = "_"
                gue3.Text = "_"
                gue4.Text = "_"
                gue5.Text = "_"
        End Select
        'feil.Text = 8
        'recive = True
    End Sub
End Class
