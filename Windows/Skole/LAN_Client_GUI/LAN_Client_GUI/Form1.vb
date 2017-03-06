Imports System.Net.Sockets
Imports System.Text
Public Class Form1
    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim tcpClient As New System.Net.Sockets.TcpClient()
        tcpClient.Connect(TextBox2.Text, 8000)
        Dim networkStream As NetworkStream = tcpClient.GetStream()
        If networkStream.CanWrite And networkStream.CanRead Then
            TextBox4.Text = "Connected"
            Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(TextBox5.Text + ": " + TextBox1.Text + ";;")
            networkStream.Flush()
            networkStream.Write(sendBytes, 0, sendBytes.Length)
            ' Read the NetworkStream into a byte buffer.
            Dim bytes(tcpClient.ReceiveBufferSize) As Byte
            networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
            ' Output the data received from the host to the console.
            Dim returndata As String = Encoding.ASCII.GetString(bytes)
            RichTextBox1.Text = returndata
            tcpClient.Close()
        Else
            If Not networkStream.CanRead Then
                TextBox4.Text = "cannot not write data to this stream"
                tcpClient.Close()
            Else
                If Not networkStream.CanWrite Then
                    TextBox4.Text = "cannot read data from this stream"
                    tcpClient.Close()
                End If
            End If
        End If
    End Sub
End Class
