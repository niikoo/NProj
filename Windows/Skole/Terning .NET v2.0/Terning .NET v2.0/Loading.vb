Public Class Loading
    Private LooperVerdi As Integer = 0
    Private LoadingLoop As Integer = 0
    Private OPLoop As Integer = 100
    Private Sub ElektroniskTerning(ByVal val As Integer)
        Dim a As Integer
        If val = 100 Then
            Dim rand As New Random
            a = rand.Next(1, 7)
        Else
            a = val
        End If
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
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If LooperVerdi = 7 Then
            Timer1.Stop()
            Timer3.Start()
        Else
            LooperVerdi += 1
        End If
        ElektroniskTerning(LooperVerdi)
    End Sub
    Private Sub hideall()
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

    Private Sub Loading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Start()
        Timer2.Start()
        Me.Opacity = 1
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        If LoadingLoop = 0 Then
            LoadingLoop = 1
            Label3.Text = "Laster"
        ElseIf LoadingLoop = 4 Then
            LoadingLoop = 1
            Label3.Text = "Laster"
        ElseIf LoadingLoop = 1 Then
            Label3.Text = "Laster."
            LoadingLoop += 1
        ElseIf LoadingLoop = 2 Then
            Label3.Text = "Laster.."
            LoadingLoop += 1
        ElseIf LoadingLoop = 3 Then
            Label3.Text = "Laster..."
            LoadingLoop += 1
        End If
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        OPLoop -= 1
        Me.Opacity = (Me.Opacity + OPLoop) / 100
        If Me.Opacity = 0 Then
            Form1.Show()
            Timer3.Stop()
            Me.Close()
        End If
    End Sub
End Class
