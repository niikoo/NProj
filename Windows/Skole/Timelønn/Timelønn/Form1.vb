Public Class Form1
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Definering av variabler

        Dim timelønn As Integer
        Dim skatteprosent As Integer
        Dim timer As Integer
        Dim skattkr As Integer
        Dim brutto As Integer
        Dim netto As Integer

        'Variabelverdier

        timelønn = Val(TextBox1.Text)
        timer = Val(TextBox2.Text)
        skatteprosent = Val(TextBox3.Text)

        'Kalkulering

        brutto = timelønn * timer
        skattkr = brutto * skatteprosent
        skattkr = skattkr / 100
        netto = brutto - skattkr

        'Variabler ut til tekstbokser

        TextBox4.Text = brutto
        TextBox5.Text = skattkr
        TextBox6.Text = netto
    End Sub
End Class
