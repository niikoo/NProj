Public Class Menu1
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
    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Application.Exit()
    End Sub

    Private Sub Avlsutt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Avslutt.Click
        Application.Exit()
    End Sub

    Private Sub Player2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles player2.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Player1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles player1.Click
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub Om_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Om.Click
        MsgBox("TicTacToe - Version 2.0" & vbLf & "made by niikoo", MsgBoxStyle.Information, "TicTacToe")
    End Sub
End Class