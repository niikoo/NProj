Public Class Form1
    Dim kort() As String = {"2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "Th", "Jh", "Qh", "Kh", "Ah", "2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "Td", "Jd", "Qd", "Kd", "Ad", "2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "Tc", "Jc", "Qc", "Kc", "Ac", "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "Ts", "Js", "Qs", "Ks", "As"}
    Dim scorep1 As Integer
    Dim scorep2 As Integer

    Public Function score(ByVal player)
        If player = 1 Then
            scorep1 += 1
        ElseIf player = 2 Then
            scorep2 += 1
        End If
        Label3.Text = "Score: " & scorep1 & " - " & scorep2
        Return True
    End Function

    Public Function getresult(ByVal value(), ByVal type())
        '15 "As, Kh, Qh, 3d, 6h" - høyt kort
        '16 "4s, 4d, As, Kh, Qh" - et par
        '17 "4s, 4d, As, Ah, Qh" - to par
        '18 "4s, 4d, 4h, Ah, Qh" - tre like
        '19 "4s, 5d, 6h, 7h, 8h" - straight (etterfølgende verdi)
        '20 "4s, 5s, Ts, As, Qs" - flush (Alle i en farge)
        '21 "4s, 4d, 4h, 4d, Qh" - fire like
        '22 "4s, 5s, 6s, 7s, 8s" - straight flush (både etterfølgende verdi og samme farge)
        'Dim value() As String = {"10", "10", "3", "3", "2"}
        'Dim type() As String = {"1", "2", "1", "1", "1"}
        Dim s1, s3 As Integer
        s1 = 0
        If type(s1) = type(s1 + 1) Then
            If type(s1 + 1) = type(s1 + 2) Then
                If type(s1 + 2) = type(s1 + 3) Then
                    If type(s1 + 3) = type(s1 + 4) Then
                        'Alle samme farge
                        If value(0) + 1 = value(1) Then
                            s3 += 1
                        ElseIf value(0) = 14 Then
                            If 1 + 1 = value(1) Then
                                s3 += 1
                            End If
                        End If
                        If value(1) + 1 = value(2) Then
                            s3 += 1
                        ElseIf value(1) = 14 Then
                            If 1 + 1 = value(2) Then
                                s3 += 1
                            End If
                        End If
                        If value(2) + 1 = value(3) Then
                            s3 += 1
                        ElseIf value(2) = 14 Then
                            If 1 + 1 = value(3) Then
                                s3 += 1
                            End If
                        End If
                        If value(3) + 1 = value(4) Then
                            s3 += 1
                        ElseIf value(3) = 14 Then
                            If 1 + 1 = value(4) Then
                                s3 += 1
                            End If
                        End If
                    End If
                End If
            End If
        End If
        If s3 = 4 Then
            Return 22 & value(4) 'Straight Flush
        End If

        Dim z1, z2, z3(4), z4 As Integer
        For z1 = 0 To 4
            For z2 = 0 To 4
                If value(z1) = value(z2) And z1 <> z2 Then
                    z3(z1) += 1
                    z4 = value(z1)
                End If
            Next
            If z3(z1) = 3 Then
                Return 21 & z4 'Fire like
            End If
        Next z1

        If type(s1) = type(s1 + 1) Then
            If type(s1 + 1) = type(s1 + 2) Then
                If type(s1 + 2) = type(s1 + 3) Then
                    If type(s1 + 3) = type(s1 + 4) Or 1 + 1 = type(0) Then
                        Return 20 'Flush
                    End If
                End If
            End If
        End If


        Dim n1 As Integer
        If value(0) + 1 = value(1) Then
            n1 += 1
        ElseIf value(0) = 14 Then
            If 1 + 1 = value(1) Then
                n1 += 1
            End If
        End If
        If value(1) + 1 = value(2) Then
            n1 += 1
        ElseIf value(1) = 14 Then
            If 1 + 1 = value(2) Then
                n1 += 1
            End If
        End If
        If value(2) + 1 = value(3) Then
            n1 += 1
        ElseIf value(2) = 14 Then
            If 1 + 1 = value(3) Then
                n1 += 1
            End If
        End If
        If value(3) + 1 = value(4) Then
            n1 += 1
        ElseIf value(3) = 14 Then
            If 1 + 1 = value(4) Then
                n1 += 1
            End If
        End If
        If n1 = 4 Then
            Return 19 & value(4) 'Straight
        End If

        Dim c1, c2, c3(4), c4 As Integer
        For c1 = 0 To 4
            For c2 = 0 To 4
                If value(c1) = value(c2) And c1 <> c2 Then
                    c3(c1) += 1
                    c4 = value(c1)
                End If
            Next
            If c3(c1) = 2 Then
                Return 18 & c4 'Tre like
            End If
        Next c1



        Dim g1, g2, g3, g4, g5 As Integer
        For g1 = 0 To 4
            For g2 = 0 To 4
                If value(g1) = value(g2) And g1 <> g2 Then
                    g5 = value(g1)
                End If
            Next
        Next
        If g5 <> False Then
            For g3 = 0 To 4
                For g4 = 0 To 4
                    If value(g3) = value(g4) And g3 <> g4 And value(g3) <> g5 Then
                        Return 17 & value(g3) 'To par
                    End If
                Next
            Next
        End If

        Dim b1, b2 As Integer
        For b1 = 0 To 4
            For b2 = 0 To 4
                If value(b1) = value(b2) And b1 <> b2 Then
                    Return 16 & value(b2) 'Et par
                End If
            Next
        Next

        Return value(4) 'Høyt kort
    End Function

    Public Function master(ByVal value(), ByVal type())
        Dim a1, a2, a3, a4 As Integer
        Dim sortarray(4), hsortarray(4) As String
        For a1 = 0 To 4
            sortarray(a1) = value(a1)
        Next
        Array.Sort(sortarray)
        For a4 = 0 To 4
            sortarray(a4) = sortarray(a4).Replace("A", "10")
            sortarray(a4) = sortarray(a4).Replace("B", "11")
            sortarray(a4) = sortarray(a4).Replace("C", "12")
            sortarray(a4) = sortarray(a4).Replace("D", "13")
            sortarray(a4) = sortarray(a4).Replace("E", "14")
        Next
        For a2 = 0 To 4
            a3 = 0
            While value(a2) = sortarray(a3)
                a3 += 1
            End While
            hsortarray(a3) = type(a2)
        Next a2
        Return getresult(sortarray, hsortarray)
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim t, random_resultat As Integer
        RichTextBox1.Text = "" 'clear textbox
        Dim hand(4), f1, f2, fu1(4), fu2(4) As String
        For t = 0 To 4
            f1 = 0
            f2 = 0
            random_resultat = Int(Rnd() * 52)
            'RichTextBox1.Text += " " & kort(random_resultat) 'spiller 1 output
            f1 = kort(random_resultat).Substring(0, 1)
            f2 = kort(random_resultat).Substring(1, 1)
            Select Case t
                Case 0
                    PictureBox1.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f1 + f2 + ".png"
                Case 1
                    PictureBox2.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f1 + f2 + ".png"
                Case 2
                    PictureBox3.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f1 + f2 + ".png"
                Case 3
                    PictureBox4.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f1 + f2 + ".png"
                Case 4
                    PictureBox5.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f1 + f2 + ".png"
            End Select
            fu1(t) = f1
            fu1(t) = fu1(t).Replace("T", "A")
            fu1(t) = fu1(t).Replace("J", "B")
            fu1(t) = fu1(t).Replace("Q", "C")
            fu1(t) = fu1(t).Replace("K", "D")
            fu1(t) = fu1(t).Replace("A", "E")
            Select Case f2
                Case "h"
                    fu2(t) = 1
                Case "d"
                    fu2(t) = 2
                Case "c"
                    fu2(t) = 3
                Case "s"
                    fu2(t) = 4
            End Select
        Next t
        'RichTextBox1.Text += vbCrLf 'enter
        Dim t2, random_resultat2 As Integer
        Dim hand2(4), f12, f22, fu12(4), fu22(4) As String
        For t2 = 0 To 4
            f12 = 0
            f22 = 0
            random_resultat2 = Int(Rnd() * 52)
            'RichTextBox1.Text += " " & kort(random_resultat2) 'Kort spiller 2 output
            f12 = kort(random_resultat2).Substring(0, 1)
            f22 = kort(random_resultat2).Substring(1, 1)
            Select Case t2
                Case 0
                    PictureBox6.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f12 + f22 + ".png"
                Case 1
                    PictureBox7.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f12 + f22 + ".png"
                Case 2
                    PictureBox8.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f12 + f22 + ".png"
                Case 3
                    PictureBox9.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f12 + f22 + ".png"
                Case 4
                    PictureBox10.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\" + f12 + f22 + ".png"
            End Select
            fu12(t2) = f12
            fu12(t2) = fu12(t2).Replace("T", "A")
            fu12(t2) = fu12(t2).Replace("J", "B")
            fu12(t2) = fu12(t2).Replace("Q", "C")
            fu12(t2) = fu12(t2).Replace("K", "D")
            fu12(t2) = fu12(t2).Replace("A", "E")
            Select Case f22
                Case "h"
                    fu22(t2) = 1
                Case "d"
                    fu22(t2) = 2
                Case "c"
                    fu22(t2) = 3
                Case "s"
                    fu22(t2) = 4
            End Select
        Next t2
        Dim xyv1, xyv2, skipcase, yyv1, yyv2 As Integer
        Dim xxv1, xxv2 As String
        Dim xyvr, grv As String
        grv = False
        xyv1 = master(fu1, fu2)
        xyv2 = master(fu12, fu22)
        xxv1 = xyv1.ToString
        xxv2 = xyv2.ToString
        If xxv1.Length = 3 Then
            yyv1 = xxv1.Substring(2, 1)
            xyv1 = xxv1.Substring(0, 2)
        ElseIf xxv1.Length = 4 Then
            yyv1 = xxv1.Substring(2, 2)
            xyv1 = xxv1.Substring(0, 2)
        End If
        If xxv2.Length = 3 Then
            yyv2 = xxv2.Substring(2, 1)
            xyv2 = xxv2.Substring(0, 2)
        ElseIf xxv2.Length = 4 Then
            yyv2 = xxv2.Substring(2, 2)
            xyv2 = xxv2.Substring(0, 2)
        End If
        If xyv1 > xyv2 Then
            RichTextBox1.Text += "Spiller 1 vant"
            score(1)
            xyvr = xyv1
        ElseIf xyv1 = xyv2 Then
            If yyv1 > yyv2 Then
                RichTextBox1.Text += "Spiller 1 vant"
                score(1)
                grv = yyv1
            ElseIf yyv1 < yyv2 Then
                RichTextBox1.Text += "Spiller 2 vant"
                score(2)
                grv = yyv2
            Else
                xyvr = 15
                Button1_Click(sender, e)
                skipcase = 1
            End If
            xyvr = xyv1
        Else
            RichTextBox1.Text += "Spiller 2 vant"
            score(2)
            xyvr = xyv2
        End If
        If skipcase <> 1 Then
            RichTextBox1.Text += " med "
        End If
        If skipcase <> 1 Then
            Select Case xyvr
                '16 "4s, 4d, As, Kh, Qh" - et par
                '17 "4s, 4d, As, Ah, Qh" - to par
                '18 "4s, 4d, 4h, Ah, Qh" - tre like
                '19 "4s, 5d, 6h, 7h, 8h" - straight (etterfølgende verdi)
                '20 "4s, 5s, Ts, As, Qs" - flush (Alle i en farge)
                '21 "4s, 4d, 4h, 4d, Qh" - fire like
                '22 "4s, 5s, 6s, 7s, 8s" - straight flush (både etterfølgende verdi og samme farge)
                Case Is <= 14
                    xyvr = Replace(xyvr, "10", "T")
                    xyvr = Replace(xyvr, "11", "J")
                    xyvr = Replace(xyvr, "12", "Q")
                    xyvr = Replace(xyvr, "13", "K")
                    xyvr = Replace(xyvr, "14", "A")
                    RichTextBox1.Text += "høyt kort: " & xyvr
                Case Is = 15
                    RichTextBox1.Text += "uavgjort"
                Case Is = 16
                    RichTextBox1.Text += "et par"
                Case Is = 17
                    RichTextBox1.Text += "to par"
                Case Is = 18
                    RichTextBox1.Text += "tre like"
                Case Is = 19
                    RichTextBox1.Text += "straight"
                Case Is = 20
                    RichTextBox1.Text += "flush"
                Case Is = 21
                    RichTextBox1.Text += "fire like"
                Case Is = 22
                    RichTextBox1.Text += "straight flush"
                Case Else
                    RichTextBox1.Text += "feil!"
            End Select
            If grv <> False Then
                grv = Replace(grv, "10", "T")
                grv = Replace(grv, "11", "J")
                grv = Replace(grv, "12", "Q")
                grv = Replace(grv, "13", "K")
                grv = Replace(grv, "14", "A")
                RichTextBox1.Text += " i " & grv
            End If
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PictureBox1.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox2.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox3.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox4.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox5.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox6.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox7.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox8.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox9.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
        PictureBox10.ImageLocation = "C:\Documents and Settings\Nikolai Ommundsen\Mine dokumenter\Visual Studio 2008\Projects\Kortspill\Kortstokk\greencard.png"
    End Sub
End Class