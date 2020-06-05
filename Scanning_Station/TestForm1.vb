Public Class TestForm1
    Private Sub TestForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim objlist As New ArrayList()
        'objlist.AddRange(New String() {"Hello", "world"})  'добавляем в список массив строк
        'For Each item As Object In objlist
        '    MsgBox(item)
        'Next
        'MsgBox(objlist(0))
        'MsgBox(objlist(1))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label1.Text = GetSNFormat(TextBox1.Text, TextBox2.Text, TextBox3.Text)
    End Sub

    'FormatSMT As String, FormatFAS As String,SN As String



    Public Function GetCoordinats(Format As String) As Array
        Dim Coordinats As Integer() = New Integer(3) {}
        For i = 0 To 5 Step 2
            Dim J As Integer
            Coordinats(J) = Convert.ToInt32(Mid(Mid(Format, Len(Format) - 5), i + 1, 2), 16)
            J += 1
        Next
        Coordinats(3) = (Coordinats(0) + Coordinats(1) + Coordinats(2))
        Return Coordinats
    End Function



    Public Function GetSNFormat(FormatSMT As String, FormatFAS As String, SN As String)
        Dim Coordinats As Integer() = New Integer(2) {}
        Coordinats = GetCoordinats(FormatSMT)
        Dim Res As Integer, Bool As Boolean
        ' i = 1 --Номер SMT, i = 2 --Номер FAS, i = 3 --Номер не определен
        For i = 1 To 3
            If i <> 3 Then
                Dim SNBase As String
                SNBase = If(i = 1, FormatSMT, FormatFAS)
                Dim MascBase As String = Mid(SNBase, 1, Coordinats(0)) + Mid(SNBase, Coordinats(0) + Coordinats(1) + 1, Coordinats(2))
                Dim MascSN As String = Mid(SN, 1, Coordinats(0)) + Mid(SN, Coordinats(0) + Coordinats(1) + 1, Coordinats(2))
                Bool = If(MascBase = MascSN, True, False)
                If Bool = True Then
                    Res = i
                    Exit For
                End If
            Else
                Res = i
            End If
        Next
        Return Res
    End Function

    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    TextBox2.Text = CInt("&H" & TextBox1.Text)
    '    TextBox3.Text = Convert.ToInt32(TextBox1.Text, 16)
    'End Sub
    '    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    '        Dim Format As String = "00001DPH110Sffffff"
    '        Dim Coordinats As Integer() = New Integer(2) {}
    '        For i = 0 To 5 Step 2
    '            Dim J As Integer
    '            Coordinats(J) = Convert.ToInt32(Mid(Mid(Format, Len(Format) - 5), i + 1, 2), 16)
    '            J += 1
    '        Next
    '        TextBox1.Text = ("X = " & Coordinats(0) & "; Y = " & Coordinats(1) & "; Z = " & Coordinats(2))
    '        TextBox3.Text = (Coordinats(0) + Coordinats(1) + Coordinats(2))
    '    End Sub
End Class