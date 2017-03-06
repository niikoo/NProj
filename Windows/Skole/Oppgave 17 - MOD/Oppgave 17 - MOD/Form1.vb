Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim tid, timer, minutter, sekunder, cnt As Integer
        tid = Val(TextBox4.Text)

        minutter = tid Mod 3600
        timer = (tid - minutter) / 3600
        sekunder = minutter Mod 60
        minutter = (minutter - sekunder) / 60

        TextBox1.Text = timer
        TextBox2.Text = minutter
        TextBox3.Text = sekunder
    End Sub
End Class
