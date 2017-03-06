Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim mynt, kron As Integer
        mynt = 0
        kron = 0
        For runde = 1 To 40
            If Rnd() > 0.5 Then
                kron += 1
            Else
                mynt += 1
            End If
        Next
        TextBox1.Text = kron
        TextBox2.Text = mynt
    End Sub
End Class
