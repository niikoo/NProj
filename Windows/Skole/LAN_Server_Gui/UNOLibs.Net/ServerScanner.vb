Public Class ServerScanner
    Public Event PerformPBStep()
    Public Event ScanFinished()
    Public Event IPfound(ByVal IP As String)
    Dim b As Integer = 0
    Public ReadOnly P As ScannerParameters
    Private ActiveThreads() As Threading.Thread
    Private ActiveThreadsState() As Integer

    Public Sub New(ByVal ScanParameters As ScannerParameters)
        P = ScanParameters
        Try
            Dim S As String = P.SubnetToScanIP
            Dim b() As String = S.Split(".")
            Dim i As Integer
            Dim bb(2) As Byte
            For i = 0 To 2
                bb(i) = Byte.Parse(b(i))
            Next
            P.SubnetToScanIP = bb(0).ToString + "." + bb(1).ToString + "." + bb(2).ToString + "."
        Catch ex As Exception
            Throw New Exception("Parameters are incorrect")
        End Try
    End Sub

    Public Sub StartScan()
        Dim s1 As Integer = 0
        Dim s2 As Integer = 1
        Dim pb As Boolean = P.useProgressBarEvent
        If P.progressbarsteps > 1 Then
            If P.progressbarsteps > 255 Then P.progressbarsteps = 255
            s2 = 255 \ P.progressbarsteps
        End If
        Dim LocalHost As System.Net.IPHostEntry
        Dim LocalEndPoint As System.Net.IPEndPoint
        LocalHost = System.Net.Dns.Resolve(System.Net.Dns.GetHostName())
        Dim localIP = LocalHost.AddressList(0).ToString
        Dim i As Integer
        Dim ClientCount As Integer
        Dim ActiveThreadStart As Threading.ThreadStart
        ClientCount = Int32.Parse("255")
        ReDim ActiveThreads(ClientCount - 1)
        ReDim ActiveThreadsState(ClientCount - 1)
        For i = 0 To 254
            System.Threading.Thread.CurrentThread.Sleep(15)
            ActiveThreadStart = New Threading.ThreadStart(AddressOf FindSpot)
            'clear any existing threads (if the start button was clicked more than once)
            ActiveThreads(i) = Nothing
            'Create a Thread object 
            ActiveThreads(i) = New Threading.Thread(ActiveThreadStart)
            'Starting the thread invokes the ThreadStart delegate
            ActiveThreads(i).Name = i.ToString
            ActiveThreadsState(i) = System.Threading.ThreadState.Running
            ActiveThreads(i).Start()
            If pb Then
                s1 += 1
                If s1 >= s2 Then
                    s1 = 0
                    RaiseEvent PerformPBStep()
                End If
            End If
        Next i
        ActiveThreads(0).Join()
        RaiseEvent ScanFinished()
    End Sub
    Private Sub FindSpot()
        b += 1
        Dim a As Integer = b
        Dim Client As New System.Net.Sockets.TcpClient
        Dim request As String = "scan"
        Dim CurrentIP As String
        CurrentIP = P.SubnetToScanIP + a.ToString
        Try
            Dim cl As New ClientClass
            cl.SendMessage(CurrentIP, P.TCPPort, request)
        Catch e As Exception
            System.Threading.Thread.CurrentThread.Sleep(3000)
            GoTo Errout
        End Try

        RaiseEvent IPfound(CurrentIP)
Errout:
        Client.Close()
    End Sub

    Public Class ScannerParameters
        Public SubnetToScanIP As String = "127.0.0.1"
        Public TCPPort As Int16 = 6666
        Public useProgressBarEvent As Boolean = False
        Public progressbarsteps As Integer = 1
    End Class
End Class