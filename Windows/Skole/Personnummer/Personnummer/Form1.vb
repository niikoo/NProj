Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, input, output As String
        input = TextBox1.Text
        p1 = input.Substring(0, 1)
        p2 = input.Substring(1, 1)
        p3 = input.Substring(2, 1)
        p4 = input.Substring(3, 1)
        p5 = input.Substring(4, 1)
        p6 = input.Substring(5, 1)
        p7 = input.Substring(6, 1)
        p8 = input.Substring(7, 1)
        p9 = input.Substring(8, 1)
        p10 = input.Substring(9, 1)
        p11 = input.Substring(10, 1)
        If (3 * p1 + 7 * p2 + 6 * p3 + 1 * p4 + 8 * p5 + 9 * p6 + 4 * p7 + 5 * p8 + 2 * p9 + 1 * p10) Mod 11 = 0 Then
            If (5 * p1 + 4 * p2 + 3 * p3 + 2 * p4 + 7 * p5 + 6 * p6 + 5 * p7 + 4 * p8 + 3 * p9 + 2 * p10 + 1 * p11) Mod 11 = 0 Then
                MsgBox("Personnummeret er gyldig")
                If p9 Mod 2 = 0 Then
                    MsgBox("Kvinne")
                Else
                    MsgBox("Mann")
                End If
            Else
                MsgBox("Personnummeret er ugyldig")
            End If
        Else
            MsgBox("Personnummeret er ugyldig")
        End If
    End Sub
End Class
