Public Class Form2
    Inherits System.Windows.Forms.Form
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

    Public Sub enable_disable()
        If Label1.Enabled = False Then
            Label1.Enabled = True
            Label2.Enabled = True
            Label3.Enabled = True
            Label4.Enabled = True
            Label5.Enabled = True
            Label6.Enabled = True
            Label7.Enabled = True
            Label8.Enabled = True
            Label9.Enabled = True
        Else
            Label1.Enabled = False
            Label2.Enabled = False
            Label3.Enabled = False
            Label4.Enabled = False
            Label5.Enabled = False
            Label6.Enabled = False
            Label7.Enabled = False
            Label8.Enabled = False
            Label9.Enabled = False
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
        Return True
    End Function


    Public Function old_ai(ByVal player)
        ' AI '
        If tur = 1 Then
            tur = 2
        Else
            tur = 1
        End If
        If player = 1 Then
            player = 2
        Else
            player = 1
        End If
        Dim antallchecked As Integer = 0
        For counter1 = 1 To 9
            If xo1 = 3 Then
                antallchecked += 1
            End If
            If xo2 = 4 Then
                antallchecked += 1
            End If
            If xo3 = 5 Then
                antallchecked += 1
            End If
            If xo4 = 6 Then
                antallchecked += 1
            End If
            If xo5 = 7 Then
                antallchecked += 1
            End If
            If xo6 = 8 Then
                antallchecked += 1
            End If
            If xo7 = 9 Then
                antallchecked += 1
            End If
            If xo8 = 10 Then
                antallchecked += 1
            End If
            If xo9 = 11 Then
                antallchecked += 1
            End If
        Next
        If antallchecked = 9 Then
            Return False
        End If
        If antallchecked > 7 Then
            If xo1 = 3 Then
                Label1.Text = "O"
                Label1.ForeColor = Color.Red
                Return True
            End If
            If xo2 = 4 Then
                Label2.Text = "O"
                Label2.ForeColor = Color.Red
                Return True
            End If
            If xo3 = 5 Then
                Label3.Text = "O"
                Label3.ForeColor = Color.Red
                Return True
            End If
            If xo4 = 6 Then
                Label4.Text = "O"
                Label4.ForeColor = Color.Red
                Return True
            End If
            If xo5 = 7 Then
                Label5.Text = "O"
                Label5.ForeColor = Color.Red
                Return True
            End If
            If xo6 = 8 Then
                Label6.Text = "O"
                Label6.ForeColor = Color.Red
                Return True
            End If
            If xo7 = 9 Then
                Label7.Text = "O"
                Label7.ForeColor = Color.Red
                Return True
            End If
            If xo8 = 10 Then
                Label8.Text = "O"
                Label8.ForeColor = Color.Red
                Return True
            End If
            If xo9 = 11 Then
                Label9.Text = "O"
                Label9.ForeColor = Color.Red
                Return True
            End If
            Return False
        End If

        If xo1 = xo3 And xo2 = 4 Then
            Label2.Text = "O"
            Label2.ForeColor = Color.Red
            Return True
        ElseIf xo4 = xo6 And xo5 = 7 Then
            Label3.Text = "O"
            Label3.ForeColor = Color.Red
            Return True
        ElseIf xo7 = xo9 And xo8 = 10 Then
            Label8.Text = "O"
            Label8.ForeColor = Color.Red '
            Return True
        ElseIf xo1 = xo7 And xo4 = 6 Then
            Label4.Text = "O"
            Label4.ForeColor = Color.Red
            Return True
        Else
            Dim rand As New Random
            Dim randnum, ender As Integer
            ender = 0
            While ender < 1
                randnum = rand.Next(1, 10)
                Select Case randnum
                    Case Is = 1
                        If (Label1.Text = "") Then
                            Label1.Text = "O"
                            Label1.ForeColor = Color.Red
                            xo1 = player
                            Return True
                        End If
                    Case Is = 2
                        If (Label2.Text = "") Then
                            Label2.Text = "O"
                            Label2.ForeColor = Color.Red
                            xo2 = player
                            Return True
                        End If
                    Case Is = 3
                        If (Label3.Text = "") Then
                            Label3.Text = "O"
                            Label3.ForeColor = Color.Red
                            xo3 = player
                            Return True
                        End If
                    Case Is = 4
                        If (Label4.Text = "") Then
                            Label4.Text = "O"
                            Label4.ForeColor = Color.Red
                            xo4 = player
                            Return True
                        End If
                    Case Is = 5
                        If (Label5.Text = "") Then
                            Label5.Text = "O"
                            Label5.ForeColor = Color.Red
                            xo5 = player
                            Return True
                        End If
                    Case Is = 6
                        If (Label6.Text = "") Then
                            Label6.Text = "O"
                            Label6.ForeColor = Color.Red
                            xo6 = player
                            Return True
                        End If
                    Case Is = 7
                        If (Label7.Text = "") Then
                            Label7.Text = "O"
                            Label7.ForeColor = Color.Red
                            xo7 = player
                            Return True
                        End If
                    Case Is = 8
                        If (Label8.Text = "") Then
                            Label8.Text = "O"
                            Label8.ForeColor = Color.Red
                            xo8 = player
                            Return True
                        End If
                    Case Is = 9
                        If (Label9.Text = "") Then
                            Label9.Text = "O"
                            Label9.ForeColor = Color.Red
                            xo9 = player
                            Return True
                        End If
                End Select
            End While
        End If
        Return False
    End Function

    Function CatsGame() As Boolean
        Dim i As Integer
        For i = 1 To 9
            If labelgruppe.Controls("Label" & CStr(i)).Text = " " Then Exit For
        Next
        If i > 9 Then Return True
        Return False
    End Function

    Function ai(ByVal player)
        Dim i As Integer
        Dim Found As Boolean = False
        Randomize()
        While (Found = False)
            If CatsGame() = False Then
                Return True
            Else
                i = findgoodspot(Rnd() * 9)
                If (labelgruppe.Controls("Label" & CStr(i)).Text <> "O") And (labelgruppe.Controls("Label" & CStr(i)).Text <> "X") Then
                    'If MoveNum = 1 Then CompsFirstMove = i
                    labelgruppe.Controls("Label" & CStr(i)).Text = "O"
                    labelgruppe.Controls("Label" & CStr(i)).ForeColor = Color.Red
                    Found = True
                    Select Case i
                        Case Is = 1
                            xo1 = 2
                        Case Is = 2
                            xo2 = 2
                        Case Is = 3
                            xo3 = 2
                        Case Is = 4
                            xo4 = 2
                        Case Is = 5
                            xo5 = 2
                        Case Is = 6
                            xo6 = 2
                        Case Is = 7
                            xo7 = 2
                        Case Is = 8
                            xo8 = 2
                        Case Is = 9
                            xo9 = 2
                    End Select
                End If
            End If
        End While
        Return True
    End Function

    Function findgoodspot(ByVal i As Integer)
        Select Case i
            Case 0
                Return Rnd() * 8 + 1
            Case 1
                If labelgruppe.Controls("Label" & CStr(1)).Text = " " Then _
                          If (((labelgruppe.Controls("Label" & CStr(2)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(2)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(3)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(3)).Text = "X"))) _
                          Or (((labelgruppe.Controls("Label" & CStr(4)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(4)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(7)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(7)).Text = "X"))) _
                          Or (((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(9)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(9)).Text = " "))) Then findgoodspot = 1
            Case 2
                If labelgruppe.Controls("Label" & CStr(2)).Text = " " Then _
                      If (((labelgruppe.Controls("Label" & CStr(1)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(1)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(3)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(3)).Text = "X"))) _
                      Or (((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(8)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(8)).Text = "X"))) Then findgoodspot = 2
            Case 3
                If labelgruppe.Controls("Label" & CStr(3)).Text = " " Then _
                      If (((labelgruppe.Controls("Label" & CStr(1)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(1)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(2)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(2)).Text = "X"))) _
                      Or (((labelgruppe.Controls("Label" & CStr(6)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(6)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(9)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(9)).Text = "X"))) _
                      Or (((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(7)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(7)).Text = "X"))) Then findgoodspot = 3
            Case 4
                If labelgruppe.Controls("Label" & CStr(4)).Text = " " Then _
                  If (((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(6)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(6)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(1)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(1)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(7)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(7)).Text = "X"))) Then findgoodspot = 4
            Case 5
                If labelgruppe.Controls("Label" & CStr(5)).Text = " " Then _
                  If (((labelgruppe.Controls("Label" & CStr(4)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(4)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(6)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(6)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(2)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(2)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(8)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(8)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(1)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(1)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(9)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(9)).Text = " "))) _
                  Or (((labelgruppe.Controls("Label" & CStr(3)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(3)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(7)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(7)).Text = " "))) Then findgoodspot = 5
            Case 6
                If labelgruppe.Controls("Label" & CStr(6)).Text = " " Then _
                  If (((labelgruppe.Controls("Label" & CStr(4)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(4)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(3)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(3)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(9)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(9)).Text = "X"))) Then findgoodspot = 6
            Case 7
                If labelgruppe.Controls("Label" & CStr(7)).Text = " " Then _
                  If (((labelgruppe.Controls("Label" & CStr(8)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(8)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(9)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(9)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(1)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(1)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(4)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(4)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(3)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(3)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "X"))) Then findgoodspot = 7
            Case 8
                If labelgruppe.Controls("Label" & CStr(8)).Text = " " Then _
                  If (((labelgruppe.Controls("Label" & CStr(7)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(7)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(9)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(9)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(2)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(2)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "X"))) Then findgoodspot = 8
            Case 9
                If labelgruppe.Controls("Label" & CStr(9)).Text = " " Then _
                  If (((labelgruppe.Controls("Label" & CStr(7)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(7)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(8)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(8)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(3)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(3)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(6)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(6)).Text = "X"))) _
                  Or (((labelgruppe.Controls("Label" & CStr(1)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(1)).Text = "O")) And ((labelgruppe.Controls("Label" & CStr(5)).Text = " ") Or (labelgruppe.Controls("Label" & CStr(5)).Text = "X"))) Then findgoodspot = 9
        End Select
        Return i
    End Function

    Public Function xof(ByVal rute, ByVal player)
        enable_disable()
        If tur = 1 Then
            tur = 2
        Else
            tur = 1
        End If
        If (rute = 1) Then
            If Label1.Text = "" Then
                If (player = 1) Then
                    Label1.Text = "X"
                    Label1.ForeColor = Color.Blue
                End If
            End If
            xo1 = player
        ElseIf (rute = 2) Then
            If Label2.Text = "" Then
                If (player = 1) Then
                    Label2.Text = "X"
                    Label2.ForeColor = Color.Blue
                End If
            End If
            xo2 = player
        ElseIf (rute = 3) Then
            If Label3.Text = "" Then
                If (player = 1) Then
                    Label3.Text = "X"
                    Label3.ForeColor = Color.Blue
                End If
            End If
            xo3 = player
        ElseIf (rute = 4) Then
            If Label4.Text = "" Then
                If (player = 1) Then
                    Label4.Text = "X"
                    Label4.ForeColor = Color.Blue
                End If
            End If
            xo4 = player
        ElseIf (rute = 5) Then
            If Label5.Text = "" Then
                If (player = 1) Then
                    Label5.Text = "X"
                    Label5.ForeColor = Color.Blue
                End If
            End If
            xo5 = player
        ElseIf (rute = 6) Then
            If Label6.Text = "" Then
                If (player = 1) Then
                    Label6.Text = "X"
                    Label6.ForeColor = Color.Blue
                End If
            End If
            xo6 = player
        ElseIf (rute = 7) Then
            If Label7.Text = "" Then
                If (player = 1) Then
                    Label7.Text = "X"
                    Label7.ForeColor = Color.Blue
                End If
            End If
            xo7 = player
        ElseIf (rute = 8) Then
            If Label8.Text = "" Then
                If (player = 1) Then
                    Label8.Text = "X"
                    Label8.ForeColor = Color.Blue
                End If
            End If
            xo8 = player
        ElseIf (rute = 9) Then
            If Label9.Text = "" Then
                If (player = 1) Then
                    Label9.Text = "X"
                    Label9.ForeColor = Color.Blue
                End If
            End If
            xo9 = player
        End If
        If ai(player) = False Then
            MsgBox("Det har oppstått en feil", MsgBoxStyle.Critical, "TicTacToe")
        Else
            enable_disable()
        End If
        If (xo1 = xo2) Then
            If (xo2 = xo3) Then
                duvant(xo1)
                Return True
            End If
        End If
        If (xo4 = xo5) Then
            If (xo5 = xo6) Then
                duvant(xo4)
                Return True
            End If
        End If
        If (xo7 = xo8) Then
            If (xo8 = xo9) Then
                duvant(xo7)
                Return True
            End If
        End If
        If (xo1 = xo4) Then
            If (xo4 = xo7) Then
                duvant(xo1)
                Return True
            End If
        End If
        If (xo2 = xo5) Then
            If (xo5 = xo8) Then
                duvant(xo2)
                Return True
            End If
        End If
        If (xo3 = xo6) Then
            If (xo6 = xo9) Then
                duvant(xo3)
                Return True
            End If
        End If
        If (xo1 = xo5) Then
            If (xo5 = xo9) Then
                duvant(xo1)
                Return True
            End If
        End If
        If (xo3 = xo5) Then
            If (xo5 = xo7) Then
                duvant(xo3)
                Return True
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
        MsgBox("Du har startet en ny runde", MsgBoxStyle.Information, "Ny runde")
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
            MsgBox("Du har startet et nytt spill", MsgBoxStyle.Information, "Nytt spill")
        End If
    End Sub

    ' LABEL CLICKS

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        If Label1.Text = "" Then
            xof(1, 1)
        End If
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        If Label2.Text = "" Then
            xof(2, 1)
        End If
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        If Label3.Text = "" Then
            xof(3, 1)
        End If
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
        If Label4.Text = "" Then
            xof(4, 1)
        End If
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click
        If Label5.Text = "" Then
            xof(5, 1)
        End If
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click
        If Label6.Text = "" Then
            xof(6, 1)
        End If
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
        If Label7.Text = "" Then
            xof(7, 1)
        End If
    End Sub

    Private Sub Label8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label8.Click
        If Label8.Text = "" Then
            xof(8, 1)
        End If
    End Sub

    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click
        If Label9.Text = "" Then
            xof(9, 1)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Menu1.Show()
        Me.Hide()
    End Sub
End Class


