Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Variabeldefinering

        Dim forbruk As Double
        Dim gjennomsnitt As Double
        Dim km As Integer
        Dim liter As Integer
        Dim min As Integer
        Dim timer As Double

        'Inndata

        km = Val(TextBox1.Text)
        liter = Val(TextBox2.Text)
        min = Val(TextBox3.Text)

        'Kalkulasjon
        timer = min / 60
        gjennomsnitt = (km / timer)
        forbruk = (liter / km) * 10

        'Utdata

        TextBox4.Text = forbruk
        TextBox5.Text = gjennomsnitt

    End Sub
End Class
