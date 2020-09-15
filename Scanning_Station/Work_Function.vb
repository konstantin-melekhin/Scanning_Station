Imports Library3


Module Work_Function
    Public Function CheckPCB(PCBSN As String, Controllabel As Label, SerialTextBox As TextBox, BT_Pause As Button, DG_THT_Start As DataGridView) As ArrayList
        Dim PCBRes As New ArrayList()
        'прерка таблицы лазер
        Dim PCBID As Integer = SelectInt("use SMDCOMPONETS SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] 
                                            where Content = '" & PCBSN & "'")
        If PCBID = 0 Then
            PrintLabel(Controllabel, "Плата " & PCBSN & " не зарегистрирована в базе!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            PCBRes.Add(False)
            PCBRes.Add("Плата не зарегистрирована в базе!")
            'CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Error", "", "Плата не зарегистрирована в базе!")
        Else
            'Проверка ТНТ старт
            LoadGridFromDB(DG_THT_Start, "use SMDCOMPONETS SELECT [PCBserial],[AOIpass],[AOIverify],[PCBResult] 
            FROM [SMDCOMPONETS].[dbo].[THTStart]  where PCBserial = '" & PCBSN & "'")
            Dim Lab As String
            If DG_THT_Start.RowCount <> 0 Then
                For i = 0 To DG_THT_Start.RowCount - 1
                    If DG_THT_Start.Item(1, i).Value = True Then
                        PCBRes = New ArrayList()
                        PCBRes.Add(True)
                        PCBRes.Add(PCBID)
                        PCBRes.Add(PCBSN)
                        SerialTextBox.Enabled = True
                        SerialTextBox.Focus()
                        Exit For
                    ElseIf DG_THT_Start.Item(1, i).Value = False Then
                        PrintLabel(Controllabel, "Плата " & PCBSN & " не прошла AOI!", 12, 193, Color.Red)
                        SerialTextBox.Enabled = False
                        BT_Pause.Focus()
                        PCBRes.Add(False)
                        PCBRes.Add("Плата не прошла AOI!")
                    End If
                Next
            Else
                PrintLabel(Controllabel, "Плата " & PCBSN & " не прошла THT Start!", 12, 193, Color.Red)
                SerialTextBox.Enabled = False
                BT_Pause.Focus()
                PCBRes.Add(False)
                PCBRes.Add("Плата не прошла THT Start!")
            End If
            'If PCBSN <> UCase("a") Then 'and PCBResult = 1

            'Else

            'End If
        End If
        Return PCBRes
    End Function
End Module
