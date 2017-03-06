Imports System.Net

Public Class ServerClass
    Public Event IncomingMessage(ByVal eventargs As InMessEvArgs)
    Public Listener As Sockets.TcpListener
    Private started As Boolean = False
    Private Lipthread As Threading.Thread
    Private NetPresent As Boolean = False
    Private LIP As String = "127.0.0.1"
    Private WithEvents Timer As New Timers.Timer(100)
    Public Function LocalIP() As String
        Return LIP
    End Function
    Public Function isRunning() As Boolean
        Return Me.started
    End Function
    Public Sub New(ByVal port As Integer, ByVal autostart As Boolean)
        InitServer(port)
        If autostart Then StartServer()
    End Sub
    Private Sub InitServer(ByVal Port As Integer)
        GetLocalIP()
        If Me.NetPresent Then
            Listener = New Sockets.TcpListener(IPAddress.Parse(LIP), Port)
        Else
            Throw New Exception("No network detected")
        End If
    End Sub
    Private Sub GetLocalIP()
        Lipthread = New Threading.Thread(AddressOf LocalIPThreadSub)
        Lipthread.Priority = Threading.ThreadPriority.Highest
        Lipthread.Start()
        System.Threading.Thread.CurrentThread.Sleep(1200)
        Lipthread.Abort()
    End Sub
    Public Sub StartServer()
        If Not started Then
            Listener.Start()
            Timer.Start()
            Timer.Enabled = True
            started = True
        End If
    End Sub
    Private Sub LocalIPThreadSub()
        Try
            Dim Dns As System.Net.Dns
            LIP = Dns.Resolve(System.Net.Dns.GetHostName).AddressList(0).ToString()
            NetPresent = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AcceptClients(ByVal o As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer.Elapsed
        Try
            If Listener.Pending Then
                Timer.Enabled = False
                Dim Thread As New Threading.Thread(AddressOf OnConnect)
                Thread.Start()
                Timer.Enabled = True
            End If
        Catch
        End Try
    End Sub
    Private Sub OnConnect()
        Dim Client As System.Net.Sockets.Socket = Listener.AcceptSocket
        Dim Buffer() As Byte
        Try
            Dim bi As Integer = 0
            While Client.Available > 0
                ReDim Preserve Buffer(bi)
                Client.Receive(Buffer, bi, 1, Sockets.SocketFlags.None)
                bi += 1
            End While
        Catch e As IndexOutOfRangeException

        End Try
        Dim message As String = System.Text.Encoding.Default.GetString(Buffer)
        Dim REP As System.Net.IPEndPoint = Client.RemoteEndPoint
        Client.Close()
        Dim args As New InMessEvArgs(message, REP.Address.ToString())
        RaiseEvent IncomingMessage(args)
    End Sub
    Public Sub StopServer()
        If started Then
            Timer.Enabled = False
            Timer.Stop()
            Listener.Stop()
            started = False
        End If
    End Sub
    Public Structure InMessEvArgs
        Dim message As String
        Dim senderIP As String
        Public Sub New(ByVal Message As String, ByVal SenderIP As String)
            Me.message = Message
            Me.senderIP = SenderIP
        End Sub
    End Structure
End Class