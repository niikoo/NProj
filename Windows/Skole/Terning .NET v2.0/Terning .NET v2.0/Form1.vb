Public Class Form1
    Private LooperVerdi As Integer = 1
    Private OPLoop As Integer = 0
    Private count() As Integer = {0, 0, 0, 0, 0, 0}
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
            count(0) += 1
        ElseIf (a = 2) Then
            hideall()
            PictureBox3.Show()
            PictureBox7.Show()
            count(1) += 1
        ElseIf (a = 3) Then
            hideall()
            PictureBox3.Show()
            PictureBox5.Show()
            PictureBox7.Show()
            count(2) += 1
        ElseIf (a = 4) Then
            hideall()
            PictureBox1.Show()
            PictureBox3.Show()
            PictureBox7.Show()
            PictureBox9.Show()
            count(3) += 1
        ElseIf (a = 5) Then
            hideall()
            PictureBox1.Show()
            PictureBox3.Show()
            PictureBox5.Show()
            PictureBox7.Show()
            PictureBox9.Show()
            count(4) += 1
        ElseIf (a = 6) Then
            hideall()
            PictureBox1.Show()
            PictureBox3.Show()
            PictureBox4.Show()
            PictureBox6.Show()
            PictureBox7.Show()
            PictureBox9.Show()
            count(5) += 1
        End If
        RichTextBox1.Text = "Resultater:" & vbCrLf & vbCrLf
        For g = 0 To 5
            RichTextBox1.Text += "Antall " & (g + 1) & ": " & count(g) & vbCrLf
        Next
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ElektroniskTerning(100)
    End Sub

    Private Sub K_Down(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button1.KeyDown
        If (e.KeyCode = Keys.T) Then
            If PictureBox10.Visible = False Then
                hideall()
                PictureBox10.Show()
            End If
            Timer1.Start()
        End If
    End Sub
    Private Sub K_Up(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.KeyUp
        If Timer1.Enabled = True Then
            Timer1.Stop()
            ElektroniskTerning(LooperVerdi)
            LooperVerdi = 1
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If LooperVerdi = 6 Then
            LooperVerdi = 1
        Else
            LooperVerdi += 1
        End If
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
        PictureBox10.Hide()
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        OPLoop += 1
        Me.Opacity = (Me.Opacity + OPLoop) / 100
        If Me.Opacity = 1 Then
            Timer2.Stop()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer2.Start()
        RichTextBox1.Text = "Resultater:" & vbCrLf & vbCrLf & "Ingen Resultater"
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Me.Width = 125 Then
            Button2.Hide()
            For a = 0 To 150
                Me.Width += 1
            Next
            Button3.Show()
        End If
        Button1.Focus()
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Me.Width = 276 Then
            Button3.Hide()
            For a = 0 To 150
                Me.Width -= 1
            Next
            Button2.Show()
        End If
        Button1.Focus()
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        For gh = 0 To 5
            count(gh) = 0
        Next
        RichTextBox1.Text = "Resultater:" & vbCrLf & vbCrLf & "Ingen Resultater"
        hideall()
        Button1.Focus()
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If Me.Width = 276 Then
            Button3.Hide()
            Button5.Hide()
            For a = 0 To 160
                Me.Width += 1
            Next
            Button6.Show()
        End If
        'MsgBox(Me.Width)
        Button1.Focus()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If Me.Width = 437 Then
            Button6.Hide()
            For a = 0 To 160
                Me.Width -= 1
            Next
            Button3.Show()
            Button5.Show()
        End If
        Button1.Focus()
    End Sub
    Private Sub kastflere(ByVal antall As Integer)
        Dim randomnum As New Random
        Dim axd As Integer
        For xd = 1 To antall
            axd = randomnum.Next(1, 7)
            'count(axd) += 1
        Next
    End Sub

    Private Sub p1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles p1.Click
        kastflere(10)
    End Sub

    Private Sub bt2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt2.Click
        kastflere(100)
    End Sub

    Private Sub bt3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt3.Click
        kastflere(1000)
    End Sub

    Private Sub bt4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt4.Click
        Dim egnedefinert As Integer
        Try
            egnedefinert = Val(TextBox1.Text)
            If (egnedefinert <> 0) Then
                kastflere(egnedefinert)
            Else
                MsgBox("Feil! Du har ikke skrevet inn et gyldig antall")
            End If
        Catch ex As Exception
            MsgBox("Feil! Du har ikke skrevet inn et gyldig antall")
        End Try
    End Sub
End Class
