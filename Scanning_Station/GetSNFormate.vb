﻿Public Module GetSNFormate
    'функция определения длины серийного номера
    Public Function GetLenSN(Format As String) As Integer
        Dim Coordinats As Integer() = New Integer(2) {}
        For i = 0 To 5 Step 2
            Dim J As Integer
            Coordinats(J) = Convert.ToInt32(Mid(Mid(Format, Len(Format) - 5), i + 1, 2), 16)
            J += 1
        Next
        Return (Coordinats(0) + Coordinats(1) + Coordinats(2))
    End Function
    'функция определения координат серийного номера
    Public Function GetCoordinats(Format As String) As Array
        Dim Coordinats As Integer() = New Integer(2) {}
        For i = 0 To 5 Step 2
            Dim J As Integer
            Coordinats(J) = Convert.ToInt32(Mid(Mid(Format, Len(Format) - 5), i + 1, 2), 16)
            J += 1
        Next
        Return Coordinats
    End Function
#Region " 'функция определения формата серийного номера для трех номеров "
    Public Function GetLOTSNFormat(FormatSMT As String) As String
        Dim Coordinats() As Integer = GetCoordinats(FormatSMT)
        Dim MascBase As String = Mid(FormatSMT, 1, Coordinats(0)) + Mid(FormatSMT, Coordinats(0) + Coordinats(1) + 1, Coordinats(2))
        Return MascBase
    End Function

    Public Function GetScanSNFormat(FormatBot As String, FormatTop As String, FormatFAS As String, SN As String, StepID As Integer) As ArrayList
        Dim Coordinats() As Integer
        Dim Res As ArrayList = New ArrayList()
        Dim VarSN As Integer
        ' i = 1 --Номер BOT, i = 2 --Номер TOP, i = 3 --Номер FAS, i = 4 --Номер не определен
        For i = 1 To 4
            If i <> 4 Then
                Dim SNBase As String
                Coordinats = GetCoordinats(If(i = 1, FormatBot, If(i = 2, FormatTop, FormatFAS)))
                SNBase = If(i = 1, FormatBot, If(i = 2, FormatTop, FormatFAS))
                If Coordinats Is Nothing Then
                Else
                    Dim MascBase As String = Mid(SNBase, 1, Coordinats(0)) + Mid(SNBase, Coordinats(0) + Coordinats(1) + 1, Coordinats(2))
                    Dim MascSN As String = Mid(SN, 1, Coordinats(0)) + Mid(SN, Coordinats(0) + Coordinats(1) + 1, Coordinats(2))
                    If (MascBase = MascSN) = True Then
                        Res.Add(True) 'Res(0)
                        Res.Add(i) 'Res(1)
                        VarSN = Convert.ToInt32(Mid(SN, Coordinats(0) + 1, Coordinats(1)))
                        Res.Add(VarSN) 'Res(2)
                        Exit For
                    End If
                End If
            Else
                Res.Add(False) 'Res(0)
                Res.Add(i) 'Res(1)
                Res.Add(0) 'Res(2)
            End If
        Next
        Select Case Res(1)
            Case 1
                Res.Add($"Формат номера {SN & vbCrLf }соответствует BOT! Отсканируйте ТОР")'Res(3) ' Текст сообщения
            Case 2
                Res.Add(If(StepID = 43, $"Формат номера {SN & vbCrLf }соответствует TOP! Отсканируйте BOT номер",
                                        $"Формат номера {SN & vbCrLf }соответствует TOP! Отсканируйте FAS номер"))
            Case 3
                Res.Add($"Формат номера {SN & vbCrLf }соответствует FAS! Отсканируйте ТОР номер")
            Case 4
                Res.Add($"Формат номера {SN & vbCrLf }не соответствует выбранному лоту!")
        End Select
        Return Res
    End Function
#End Region



End Module