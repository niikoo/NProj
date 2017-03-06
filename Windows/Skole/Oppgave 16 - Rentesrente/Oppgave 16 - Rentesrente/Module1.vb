Module Module1

    Sub Main()
        Dim peng As Integer
        Dim rentefot As Double
        Dim years As Integer
        Dim dobbel As Integer
        Dim dobbelsaldo As Integer
        Dim rentey As Double
        Console.Write("Saldo: ")
        peng = Val(Console.ReadLine())
        Console.Write("Rentefot i %: ")
        rentefot = Val(Console.ReadLine())
        Console.WriteLine()
        rentefot = rentefot / 100:
        dobbelsaldo = peng * 2
        For years = 1 To 10
            rentey = (1 + rentefot) ^ years
            dobbel = peng * rentey
            If (dobbel > dobbelsaldo) Then
                Console.WriteLine("År til beløpet fordobles: {0}", years)
                Console.WriteLine("Ny saldo: {0}", dobbel)
                Exit For
            End If
        Next years
        Stop
    End Sub
End Module
