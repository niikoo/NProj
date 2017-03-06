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
    Dim tur As Integer = 1
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
        Return True
    End Function

    Public Function xof(ByVal rute, ByVal player)
        If (tur = 1) Then
            tur = 2
        ElseIf (tur = 2) Then
            tur = 1
        End If
        Label13.Text = tur
        If (tur = 1) Then
            Label13.ForeColor = Color.White
        Else
            Label13.ForeColor = Color.Red
        End If
        If (rute = 1) Then
            If Label1.Text = "" Then
                If (player = 1) Then
                    Label1.Text = "X"
                    Label1.ForeColor = Color.Blue
                Else
                    Label1.Text = "O"
                    Label1.ForeColor = Color.Red
                End If
            End If
            xo1 = player
        ElseIf (rute = 2) Then
            If Label2.Text = "" Then
                If (player = 1) Then
                    Label2.Text = "X"
                    Label2.ForeColor = Color.Blue
                Else
                    Label2.Text = "O"
                    Label2.ForeColor = Color.Red
                End If
            End If
            xo2 = player
        ElseIf (rute = 3) Then
            If Label3.Text = "" Then
                If (player = 1) Then
                    Label3.Text = "X"
                    Label3.ForeColor = Color.Blue
                Else
                    Label3.Text = "O"
                    Label3.ForeColor = Color.Red
                End If
            End If
            xo3 = player
        ElseIf (rute = 4) Then
            If Label4.Text = "" Then
                If (player = 1) Then
                    Label4.Text = "X"
                    Label4.ForeColor = Color.Blue
                Else
                    Label4.Text = "O"
                    Label4.ForeColor = Color.Red
                End If
            End If
            xo4 = player
        ElseIf (rute = 5) Then
            If Label5.Text = "" Then
                If (player = 1) Then
                    Label5.Text = "X"
                    Label5.ForeColor = Color.Blue
                Else
                    Label5.Text = "O"
                    Label5.ForeColor = Color.Red
                End If
            End If
            xo5 = player
        ElseIf (rute = 6) Then
            If Label6.Text = "" Then
                If (player = 1) Then
                    Label6.Text = "X"
                    Label6.ForeColor = Color.Blue
                Else
                    Label6.Text = "O"
                    Label6.ForeColor = Color.Red
                End If
            End If
            xo6 = player
        ElseIf (rute = 7) Then
            If Label7.Text = "" Then
                If (player = 1) Then
                    Label7.Text = "X"
                    Label7.ForeColor = Color.Blue
                Else
                    Label7.Text = "O"
                    Label7.ForeColor = Color.Red
                End If
            End If
            xo7 = player
        ElseIf (rute = 8) Then
            If Label8.Text = "" Then
                If (player = 1) Then
                    Label8.Text = "X"
                    Label8.ForeColor = Color.Blue
                Else
                    Label8.Text = "O"
                    Label8.ForeColor = Color.Red
                End If
            End If
            xo8 = player
        ElseIf (rute = 9) Then
            If Label9.Text = "" Then
                If (player = 1) Then
                    Label9.Text = "X"
                    Label9.ForeColor = Color.Blue
                Else
                    Label9.Text = "O"
                    Label9.ForeColor = Color.Red
                End If
            End If
            xo9 = player
        End If
        If (xo1 = xo2) Then
            If (xo2 = xo3) Then
                duvant(xo1)
                MsgBox("X")
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

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
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
        MsgBox("Du har startet en ny runde, det er spiller " & tur & " sin tur", MsgBoxStyle.Information, "Ny runde")
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        Application.Exit()
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Dim resultat As Integer
        resultat = MsgBox("Er du sikker på at du vil starte et nytt spill? Dette vil slette scoren.", MsgBoxStyle.YesNo, "Nytt spill")
        If (resultat = 6) Then
            vunnet1 = 0
            vunnet2 = 0
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
            Label11.Text = vunnet1 & " - " & vunnet2
            MsgBox("Du har startet et nytt spill, det er spiller " & tur & " sin tur", MsgBoxStyle.Information, "Nytt spill")
        End If
    End Sub

    ' LABEL CLICKS

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        If Label1.Text = "" Then
            xof(1, tur)
        End If
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        If Label2.Text = "" Then
            xof(2, tur)
        End If
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        If Label3.Text = "" Then
            xof(3, tur)
        End If
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
        If Label4.Text = "" Then
            xof(4, tur)
        End If
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        If Label5.Text = "" Then
            xof(5, tur)
        End If
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click
        If Label6.Text = "" Then
            xof(6, tur)
        End If
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        If Label7.Text = "" Then
            xof(7, tur)
        End If
    End Sub

    Private Sub Label8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label8.Click
        If Label8.Text = "" Then
            xof(8, tur)
        End If
    End Sub

    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click
        If Label9.Text = "" Then
            xof(9, tur)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Menu1.Show()
        Me.Hide()
    End Sub
End Class


