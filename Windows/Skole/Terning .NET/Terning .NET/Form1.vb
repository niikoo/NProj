Public Class Form1
    Public timerresult As Integer = 1
    Public Sub hideall()
        PictureBox1.Hide()
        PictureBox2.Hide()
        PictureBox3.Hide()
        PictureBox4.Hide()
        PictureBox5.Hide()
        PictureBox6.Hide()
        PictureBox7.Hide()
        PictureBox8.Hide()
        PictureBox9.Hide()
    End Sub
    Public Sub display(ByVal a As Integer)
        If (a = 1) Then
            hideall()
            PictureBox5.Show()
        ElseIf (a = 2) Then
            hideall()
            PictureBox3.Show()
            PictureBox7.Show()
        ElseIf (a = 3) Then
            hideall()
            PictureBox3.Show()
            PictureBox5.Show()
            PictureBox7.Show()
        ElseIf (a = 4) Then
            hideall()
            PictureBox1.Show()
            PictureBox3.Show()
            PictureBox7.Show()
            PictureBox9.Show()
        ElseIf (a = 5) Then
            hideall()
            PictureBox1.Show()
            PictureBox3.Show()
            PictureBox5.Show()
            PictureBox7.Show()
            PictureBox9.Show()
        ElseIf (a = 6) Then
            hideall()
            PictureBox1.Show()
            PictureBox3.Show()
            PictureBox4.Show()
            PictureBox6.Show()
            PictureBox7.Show()
            PictureBox9.Show()
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim a As Integer
        Dim rand As New System.Random()
        a = rand.Next(1, 7)
        display(a)
    End Sub

    Public Sub K_Down(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If (e.KeyCode = Keys.T) Then
            Timer1.Start()
        End If
    End Sub
    Public Sub Space_Down(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button1.KeyDown
        If (e.KeyCode = Keys.T) Then
            Timer1.Start()
        End If
    End Sub
    Public Sub K_Up(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.KeyUp
        If Timer1.Enabled = True Then
            Timer1.Stop()
            display(timerresult)
            timerresult = 0
        End If
    End Sub
    Public Sub Space_Up(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.KeyUp
        If Timer1.Enabled = True Then
            Timer1.Stop()
            display(timerresult)
            timerresult = 0
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If timerresult = 7 Then
            timerresult = 1
        Else
            timerresult += 1
        End If
    End Sub
End Class