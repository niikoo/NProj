Public Class Form1
    Public Function romertall(ByVal tall)
        'I= 1
        'V = 5
        'X = 10
        'L = 50
        'C = 100
        'D = 500
        'M = 1000

        TextBox2.Text = ""
        Dim ii, iv, ix, il, ic, id, im As Integer
        id = tall Mod 1000
        im = (tall - id) / 1000

        ic = id Mod 500
        id = (id - ic) / 500

        il = ic Mod 100
        ic = (ic - il) / 100

        ix = il Mod 50
        il = (il - ix) / 50

        iv = ix Mod 10
        ix = (ix - iv) / 10

        ii = iv Mod 5
        iv = (iv - ii) / 5

        For imf = 1 To im
            TextBox2.Text = TextBox2.Text & "M"
        Next imf

        For idf = 1 To id
            TextBox2.Text = TextBox2.Text & "D"
        Next idf

        For icf = 1 To ic
            TextBox2.Text = TextBox2.Text & "C"
        Next icf

        For ilf = 1 To il
            TextBox2.Text = TextBox2.Text & "L"
        Next ilf

        For ixf = 1 To ix
            TextBox2.Text = TextBox2.Text & "X"
        Next ixf

        For ivf = 1 To iv
            TextBox2.Text = TextBox2.Text & "V"
        Next ivf

        For nif = 1 To ii
            TextBox2.Text = TextBox2.Text & "I"
        Next nif
        Return True
    End Function
    Public Function vanlig(ByVal text)
        Dim strx, stry, skip As Integer
        Dim total As Long
        Dim rev As String
        skip = False
        rev = text
        rev = rev.ToUpper
        rev = Replace(rev, "M", 7)
        rev = Replace(rev, "D", 6)
        rev = Replace(rev, "C", 5)
        rev = Replace(rev, "L", 4)
        rev = Replace(rev, "X", 3)
        rev = Replace(rev, "V", 2)
        rev = Replace(rev, "I", 1)
        For strx = 1 To rev.Length
            stry = strx - 1
            skip = False
            If strx <> rev.Length Then
                'MsgBox("allinfo: strx: " & strx & " and substr1: " & rev.Substring((strx - 1), 1) & " < " & rev.Substring((strx), 1))
                If rev.Substring((strx - 1), 1) < rev.Substring((strx), 1) Then
                    If (strx >= 1) Then
                        If rev.Substring((strx - 1), 1) = "6" Then
                            total -= 500
                        ElseIf rev.Substring((strx - 1), 1) = "5" Then
                            total -= 100
                        ElseIf rev.Substring((strx - 1), 1) = "4" Then
                            total -= 50
                        ElseIf rev.Substring((strx - 1), 1) = "3" Then
                            total -= 10
                        ElseIf rev.Substring((strx - 1), 1) = "2" Then
                            total -= 5
                        ElseIf rev.Substring((strx - 1), 1) = "1" Then
                            total -= 1
                        End If
                    Else
                    End If
                    skip = True
                End If
            End If
                If (skip = False) Then
                If rev.Substring(stry, 1) = "7" Then
                    total += 1000
                ElseIf rev.Substring(stry, 1) = "6" Then
                    total += 500
                ElseIf rev.Substring(stry, 1) = "5" Then
                    total += 100
                ElseIf rev.Substring(stry, 1) = "4" Then
                    total += 50
                ElseIf rev.Substring(stry, 1) = "3" Then
                    total += 10
                ElseIf rev.Substring(stry, 1) = "2" Then
                    total += 5
                ElseIf rev.Substring(stry, 1) = "1" Then
                    total += 1
                End If
            End If
        Next strx
        TextBox1.Text = total
        Return True
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TIL ROMERTALL
        romertall(TextBox1.Text)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'TIL VANLIGE TALL
        vanlig(TextBox2.Text)
    End Sub
End Class
