Public Class Form1
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim peng As Integer
        Dim rentefot As Double
        Dim years As Integer
        Dim dobbel As Integer
        Dim dobbelsaldo As Integer
        Dim rentey As Double
        peng = Val(TextBox1.Text)
        rentefot = Val(TextBox2.Text)
        rentefot = rentefot / 100
        dobbelsaldo = peng * 2
        For years = 1 To 100
            rentey = (1 + rentefot) ^ years
            dobbel = peng * rentey
            If (dobbel > dobbelsaldo) Then
                Label4.Text = years & " år"
                Label6.Text = dobbel & " kr"
                Exit For
            End If
        Next years
        dobbel = 0
        rentefot = 0
        years = 0
        rentey = 0
    End Sub
End Class
