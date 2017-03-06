Public Class ClientClass
    Public Sub SendMessage(ByVal Ip As String, ByVal Port As Integer, ByVal Message As String)
        If Not checkip(Ip) Then
            Throw New Exception("Ip is not in the right format")
            Exit Sub
        End If
        Try
            Dim client As New System.Net.Sockets.TcpClient
            Dim Buffer() As Byte = System.Text.Encoding.Default.GetBytes(Message.ToCharArray)
            client.Connect(Ip, Port)
            client.GetStream.Write(Buffer, 0, Buffer.Length)
            client.Close()
        Catch ex As Exception
            Throw New Exception("Remote IP is not reachable")
        End Try
    End Sub
    Private Function checkip(ByVal ip As String) As Boolean
        Try
            Dim ss() As String = ip.Split(".")
            Dim bb(3) As Byte
            Dim i As Integer
            For i = 0 To 3
                bb(i) = Byte.Parse(ss(i))
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class