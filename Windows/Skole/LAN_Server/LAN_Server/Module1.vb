Imports System.Net.Sockets
Imports System.Text
Class TCPSrv
    Public Shared Function ConcatBytes(ByVal a As Byte(), ByVal b As Byte()) As Byte()
        Dim bytes As Byte() = New Byte(a.Length + (b.Length - 1)) {}
        Array.Copy(a, bytes, a.Length)
        Array.Copy(b, 0, bytes, a.Length, b.Length)
        Return bytes
    End Function
    Public Shared Function StrToByteArray(ByVal str As String) As Byte()
        Dim encoding As New System.Text.ASCIIEncoding()
        Return encoding.GetBytes(str)
    End Function 'StrToByteArray
    Shared Sub Main()
        ' Must listen on correct port- must be same as port client wants to connect on.
        Const portNumber As Integer = 8000
        Dim saveddata As String = ""
        Dim spacer As [Byte]() = Encoding.ASCII.GetBytes(";;")
        Dim tcpListener As New TcpListener(portNumber)
start:
        tcpListener.Start()
        Console.WriteLine("Waiting for connection...")
        Try
            'Accept the pending client connection and return 
            'a TcpClient initialized for communication. 
            Dim tcpClient As TcpClient = tcpListener.AcceptTcpClient()
            Console.WriteLine("Connection accepted.")
            ' Get the stream
            Dim networkStream As NetworkStream = tcpClient.GetStream()
            ' Read the stream into a byte array
            Dim bytes(tcpClient.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
            ' Return the data received from the client to the console.
            Dim clientdata As String = Encoding.ASCII.GetString(bytes)
            Console.WriteLine(("Client sent: " + clientdata))
            saveddata += clientdata & ";;"
            Console.WriteLine("DETTE ER SAVEDDATA: " + saveddata + "  - CLIENTDATA: " + clientdata + " - SLUTT AV CLIENTDATA")
            Dim sendBytes As [Byte]() = StrToByteArray(saveddata)
            networkStream.Flush()
            networkStream.Write(sendBytes, 0, sendBytes.Length)
            'Console.WriteLine(("Message Sent /> : " + saveddata))
            'Console.WriteLine(saveddata)
            'Any communication with the remote client using the TcpClient can go here.
            'Close TcpListener and TcpClient.
            tcpClient.Close()
            tcpListener.Stop()

            Dim index As Integer
            For index = 0 To s.length() - 1
                Dim c As Char = s.charAt(index)
                If c = ControlChars.NullChar Then
                    Exit For
                End If
            Next
            s = s.substring(0, index) ' SPACE SKIPPER

            ' index is exclusive so this will cut off the String right before the '\0'

            GoTo start
            Console.WriteLine("exit")
            Console.ReadLine()
        Catch e As Exception
            Console.WriteLine(e.ToString())
            Console.ReadLine()
        End Try
    End Sub
End Class