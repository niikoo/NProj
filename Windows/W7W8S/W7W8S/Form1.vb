Public Class Form1

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim proc As Process
        Dim ProcessProperties As New ProcessStartInfo
        ProcessProperties.FileName = "C:\\Windows\\W7E\\explorer.exe"
        ProcessProperties.Arguments = ""
        ProcessProperties.Verb = "runas"
        Dim myProcess As Process = Process.Start(ProcessProperties)
        Application.Exit()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim proc As Process
        Dim ProcessProperties As New ProcessStartInfo
        ProcessProperties.FileName = "C:\\Windows\\explorer.exe"
        ProcessProperties.Arguments = ""
        'ProcessProperties.Verb = "runas"
        ProcessProperties.LoadUserProfile = True
        ProcessProperties.UseShellExecute = False
        Dim myProcess As Process = Process.Start(ProcessProperties)
        Application.Exit()
    End Sub
End Class
