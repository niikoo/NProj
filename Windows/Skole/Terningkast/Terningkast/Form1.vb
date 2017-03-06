Public Class Form1
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim random As New Random
        Dim rand As Integer
        Dim data(5) As Integer
        For runde = 1 To 1000
            rand = random.Next(1, 7)
            If rand = 6 Then
                data(5) += 1
            ElseIf rand = 5 Then
                data(4) += 1
            ElseIf rand = 4 Then
                data(3) += 1
            ElseIf rand = 3 Then
                data(2) += 1
            ElseIf rand = 2 Then
                data(1) += 1
            ElseIf rand = 1 Then
                data(0) += 1
            End If
        Next
        RichTextBox1.Text = "Antall terningkast: 100 " & vbLf & " 1: " & data(0) & "" & vbLf & " 2: " & data(1) & "" & vbLf & " 3: " & data(2) & "" & vbLf & " 4: " & data(3) & "" & vbLf & " 5: " & data(4) & "" & vbLf & " 6: " & data(5)
    End Sub
End Class
