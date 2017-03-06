Imports System.Net.Sockets
Imports System.Text
Class TCPCli
    Shared Sub Main()
start:
        Dim tcpClient As New System.Net.Sockets.TcpClient()
        tcpClient.Connect("127.0.0.1", 8000)
        Dim networkStream As NetworkStream = tcpClient.GetStream()
        If networkStream.CanWrite And networkStream.CanRead Then
            'Write something
            Console.WriteLine("Tekst å sende til server")
            Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(Console.ReadLine)
            networkStream.Flush()
            networkStream.Write(sendBytes, 0, sendBytes.Length)
            ' Read the NetworkStream into a byte buffer.
            Dim bytes(tcpClient.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
            ' Output the data received from the host to the console.
            Dim returndata As String = Encoding.ASCII.GetString(bytes)
            Console.WriteLine(("Host returned: " + returndata))
            GoTo start
        Else
            If Not networkStream.CanRead Then
                Console.WriteLine("cannot not write data to this stream")
                tcpClient.Close()
            Else
                If Not networkStream.CanWrite Then
                    Console.WriteLine("cannot read data from this stream")
                    tcpClient.Close()
                End If
            End If
        End If
        ' pause so user can view the console output
        Console.ReadLine()
    End Sub
End Class
