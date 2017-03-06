Public Class Form1
    Dim xo1 As Integer = 3
    Dim xo2 As Integer = 4
    Dim xo3 As Integer = 5
    Dim xo4 As Integer = 6
    Dim xo5 As Integer = 7
    Dim xo6 As Integer = 8
    Dim xo7 As Integer = 9
    Dim xo8 As Integer = 10
    Dim xo9 As Integer = 11
    Dim tur As Integer = 2
    Dim vunnet1 As Integer = 0
    Dim vunnet2 As Integer = 0
    Dim newPoint As New System.Drawing.Point()
    Dim a, b As Integer

    Private Sub Form1_Down(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseDown
        a = MousePosition.X - Me.Location.X
        b = MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Form1_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseMove
        If MouseButtons = Windows.Forms.MouseButtons.Left Then
            newPoint = MousePosition
            newPoint.X = newPoint.X - (a)
            newPoint.Y = newPoint.Y - (b)
            Me.Location = newPoint
        End If
    End Sub


    Public Function duvant(ByVal player)
        MsgBox("Spiller " & player & " vant!", MsgBoxStyle.Information, "Gratulerer!")
        If (player = 1) Then
            vunnet1 += 1
        ElseIf (player = 2) Then
            vunnet2 += 1
        End If
        Label11.Text = vunnet1 & " - " & vunnet2
        MsgBox("Klikk 'Ny Runde' for å starte en ny runde", MsgBoxStyle.Information, "Ny Runde?")
        Return True
    End Function

    Public Function xof(ByVal rute, ByVal player)
        If (tur = player) Then
            MsgBox("Det er ikke din tur")
            Beep()
            Return False
        Else
            turtext.Text = "Spiller " & tur & " sin tur"
            If (tur = 1) Then
                tur = 2
            ElseIf (tur = 2) Then
                tur = 1
            End If
        End If
        If (rute = 1) Then
            If (player = 1) Then
                Label1.Text = "X"
                Label1.ForeColor = Color.Blue
            Else
                Label1.Text = "O"
                Label1.ForeColor = Color.Red
            End If
            Button1.Hide()
            Button10.Hide()
            xo1 = player
        ElseIf (rute = 2) Then
            If (player = 1) Then
                Label2.Text = "X"
                Label2.ForeColor = Color.Blue
            Else
                Label2.Text = "O"
                Label2.ForeColor = Color.Red
            End If
            Button2.Hide()
            Button11.Hide()
            xo2 = player
        ElseIf (rute = 3) Then
            If (player = 1) Then
                Label3.Text = "X"
                Label3.ForeColor = Color.Blue
            Else
                Label3.Text = "O"
                Label3.ForeColor = Color.Red
            End If
            Button3.Hide()
            Button12.Hide()
            xo3 = player
        ElseIf (rute = 4) Then
            If (player = 1) Then
                Label4.Text = "X"
                Label4.ForeColor = Color.Blue
            Else
                Label4.Text = "O"
                Label4.ForeColor = Color.Red
            End If
            Button4.Hide()
            Button13.Hide()
            xo4 = player
        ElseIf (rute = 5) Then
            If (player = 1) Then
                Label5.Text = "X"
                Label5.ForeColor = Color.Blue
            Else
                Label5.Text = "O"
                Label5.ForeColor = Color.Red
            End If
            Button5.Hide()
            Button14.Hide()
            xo5 = player
        ElseIf (rute = 6) Then
            If (player = 1) Then
                Label6.Text = "X"
                Label6.ForeColor = Color.Blue
            Else
                Label6.Text = "O"
                Label6.ForeColor = Color.Red
            End If
            Button6.Hide()
            Button15.Hide()
            xo6 = player
        ElseIf (rute = 7) Then
            If (player = 1) Then
                Label7.Text = "X"
                Label7.ForeColor = Color.Blue
            Else
                Label7.Text = "O"
                Label7.ForeColor = Color.Red
            End If
            Button7.Hide()
            Button16.Hide()
            xo7 = player
        ElseIf (rute = 8) Then
            If (player = 1) Then
                Label8.Text = "X"
                Label8.ForeColor = Color.Blue
            Else
                Label8.Text = "O"
                Label8.ForeColor = Color.Red
            End If
            Button8.Hide()
            Button17.Hide()
            xo8 = player
        ElseIf (rute = 9) Then
            If (player = 1) Then
                Label9.Text = "X"
                Label9.ForeColor = Color.Blue
            Else
                Label9.Text = "O"
                Label9.ForeColor = Color.Red
            End If
            Button9.Hide()
            Button18.Hide()
            xo9 = player
        End If
        If (xo1 = xo2) Then
            If (xo2 = xo3) Then
                duvant(xo1)
            End If
        End If
        If (xo4 = xo5) Then
            If (xo5 = xo6) Then
                duvant(xo4)
            End If
        End If
        If (xo7 = xo8) Then
            If (xo8 = xo9) Then
                duvant(xo7)
            End If
        End If
        If (xo1 = xo4) Then
            If (xo4 = xo7) Then
                duvant(xo1)
            End If
        End If
        If (xo2 = xo5) Then
            If (xo5 = xo8) Then
                duvant(xo2)
            End If
        End If
        If (xo3 = xo6) Then
            If (xo6 = xo9) Then
                duvant(xo3)
            End If
        End If
        If (xo1 = xo5) Then
            If (xo5 = xo9) Then
                duvant(xo1)
            End If
        End If
        If (xo3 = xo5) Then
            If (xo5 = xo7) Then
                duvant(xo3)
            End If
        End If
        Return True
    End Function

    ' PLAYER 1 BUTTONS

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        xof(1, 1)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        xof(2, 1)
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        xof(3, 1)
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        xof(4, 1)
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        xof(5, 1)
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        xof(6, 1)
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        xof(7, 1)
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        xof(8, 1)
    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        xof(9, 1)
    End Sub

    ' PLAYER 2 BUTTONS
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        xof(1, 2)
    End Sub
    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        xof(2, 2)
    End Sub
    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        xof(3, 2)
    End Sub
    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        xof(4, 2)
    End Sub
    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        xof(5, 2)
    End Sub
    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        xof(6, 2)
    End Sub
    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        xof(7, 2)
    End Sub
    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        xof(8, 2)
    End Sub
    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        xof(9, 2)
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        Button1.Show()
        Button2.Show()
        Button3.Show()
        Button4.Show()
        Button5.Show()
        Button6.Show()
        Button7.Show()
        Button8.Show()
        Button9.Show()
        Button10.Show()
        Button11.Show()
        Button12.Show()
        Button13.Show()
        Button14.Show()
        Button15.Show()
        Button16.Show()
        Button17.Show()
        Button18.Show()
        Label1.Text = ""
        Label2.Text = ""
        Label3.Text = ""
        Label4.Text = ""
        Label5.Text = ""
        Label6.Text = ""
        Label7.Text = ""
        Label8.Text = ""
        Label9.Text = ""
        xo1 = 10
        xo2 = 11
        xo3 = 12
        xo4 = 13
        xo5 = 14
        xo6 = 15
        xo6 = 16
        xo7 = 17
        xo8 = 18
        xo9 = 19
        Dim turn As Integer = tur
        If (tur = 1) Then
            turn = 2
        ElseIf (tur = 2) Then
            turn = 1
        End If
        MsgBox("Du har startet en ny runde, det er spiller " & turn & " sin tur", MsgBoxStyle.Information, "Ny runde")
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        Application.Exit()
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Dim resultat As Integer
        resultat = MsgBox("Er du sikker på at du vil starte et nytt spill? Dette vil slette scoren.", MsgBoxStyle.YesNo, "Nytt spill")
        If (resultat = 6) Then
            Dim turn As Integer = tur
            If (tur = 1) Then
                turn = 2
            ElseIf (tur = 2) Then
                turn = 1
            End If
            vunnet1 = 0
            vunnet2 = 0
            Label11.Text = vunnet1 & " - " & vunnet2
            MsgBox("Du har startet et nytt spill, det er spiller " & turn & " sin tur", MsgBoxStyle.Information, "Nytt spill")
        End If
    End Sub
End Class
