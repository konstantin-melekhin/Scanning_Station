Imports Library3


Public Class WorkForm
    ReadOnly IDApp As Integer = 26
    Dim LOTID As Integer, LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim LOTInfo As New ArrayList() 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim CloseForm As Boolean
    'Загрузка рабочей формы
    Private Sub WorkForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: данная строка кода позволяет загрузить данные в таблицу "FASDataSet.FAS_ErrorCode". При необходимости она может быть перемещена или удалена.
        Me.FAS_ErrorCodeTableAdapter.Fill(Me.FASDataSet.FAS_ErrorCode)
        LOTID = SettingsForm.LOTID
        If LOTID = 0 Then
            CloseForm = True
            SettingsForm.Show()
            SettingsForm.L_Result.Visible = True
            Me.Close()
            MsgBox("Выберите ЛОТ повторно и снова запустите программу!")
            Exit Sub
        End If

        Controllabel.Text = ""
        LoadGridFromDB(DG_StepList, "USE FAS SELECT [ID],[StepName],[Description] FROM [FAS].[dbo].[Ct_StepScan]")
        PCInfo = GetPCInfo(IDApp)
        LabelAppName.Text = PCInfo(1)
        Label_StationName.Text = PCInfo(5)
        LB_CurrentStep.Text = PCInfo(7)
        Lebel_StationLine.Text = PCInfo(3)
        TextBox1.Text = "App_ID = " & PCInfo(0) & vbCrLf &
                        "App_Caption = " & PCInfo(1) & vbCrLf &
                        "lineID = " & PCInfo(2) & vbCrLf &
                        "LineName = " & PCInfo(3) & vbCrLf &
                        "StationID = " & PCInfo(4) & vbCrLf &
                        "StationName = " & PCInfo(5) & vbCrLf &
                        "CT_ScanStepID = " & PCInfo(6) & vbCrLf &
                        "CT_ScanStep = " & PCInfo(7) & vbCrLf 'PCInfo

        LOTInfo = GetCurrentContractLot(LOTID)
        LenSN = GetLenSN(LOTInfo(2))
        TextBox2.Text = "Model = " & LOTInfo(0) & vbCrLf &
                        "LOT = " & LOTInfo(1) & vbCrLf &
                        "SMTNumberFormat = " & LOTInfo(2) & vbCrLf &
                        "SMTRangeChecked = " & LOTInfo(3) & vbCrLf &
                        "SMTStartRange = " & LOTInfo(4) & vbCrLf &
                        "SMTEndRange = " & LOTInfo(5) & vbCrLf &
                        "ParseLog = " & LOTInfo(6) & vbCrLf &
                        "StepSequence = " & LOTInfo(7) & vbCrLf &
                        "LenSN = " & LenSN & vbCrLf 'LOTInfo
        StepSequence = New String(Len(LOTInfo(7)) / 2 - 1) {}
        For i = 0 To Len(LOTInfo(7)) - 1 Step 2
            Dim J As Integer
            StepSequence(J) = Mid(LOTInfo(7), i + 1, 2)
            J += 1
        Next
        'Определить текущий шаг
        For i = 0 To StepSequence.Count - 1
            If Convert.ToInt32(StepSequence(i), 16) = PCInfo(6) Then
                StartStepID = Convert.ToInt32(StepSequence(0), 16)
                PreStepID = If(i <> 0, Convert.ToInt32(StepSequence(i - 1), 16), 0)
                NextStepID = If(i <> StepSequence.Count - 1, Convert.ToInt32(StepSequence(i + 1), 16), 0)
                For Each row As DataGridViewRow In DG_StepList.Rows
                    Dim j As Integer
                    If StartStepID = DG_StepList.Item(0, j).Value Then
                        StartStep = DG_StepList.Item(1, j).Value
                    ElseIf PreStepID = DG_StepList.Item(0, j).Value Then
                        PreStep = DG_StepList.Item(1, j).Value
                    ElseIf NextStepID = DG_StepList.Item(0, j).Value Then
                        NextStep = DG_StepList.Item(1, j).Value
                    End If
                    j += 1
                Next
                If PreStepID = StartStepID Then
                    PreStep = StartStep
                End If
                Exit For
            End If
        Next
        L_LOT.Text = LOTInfo(1)
        L_Model.Text = LOTInfo(0)
        'загружаем список кодов ошибок в грид SQL запрос "ErrorCodeList" 
        LoadGridFromDB(DG_ErrorCodes, "use FAS select [ErrorCodeID],[ErrorCode],[Description]  FROM [FAS].[dbo].[FAS_ErrorCode]")
        'Записываем коды ошибок в рабочий комбобокс
        If DG_ErrorCodes.Rows.Count <> 0 Then
            For J = 0 To DG_ErrorCodes.Rows.Count - 1
                CB_ErrorCode.Items.Add(DG_ErrorCodes.Rows(J).Cells(1).Value)
            Next
        End If

        'Запуск программы
        '___________________________________________________________
        GB_UserData.Location = New Point(10, 12)
        TB_RFIDIn.Focus()
        'запуск счетчика продукции за день
        CurrentTimeTimer.Start()
        ShiftCounterInfo = ShiftCounterStart(PCInfo(4), IDApp, LOTID)
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_LOTCounter.Text = ShiftCounterInfo(2)
    End Sub 'Загрузка рабочей формы
    'Часы в программе
    Private Sub CurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles CurrentTimeTimer.Tick
        CurrrentTimeLabel.Text = TimeString
    End Sub 'Часы в программе
    'регистрация пользователя
    Dim UserInfo As New ArrayList()
    Private Sub TB_RFIDIn_KeyDown(sender As Object, e As KeyEventArgs) Handles TB_RFIDIn.KeyDown
        TB_RFIDIn.MaxLength = 10
        If e.KeyCode = Keys.Enter And TB_RFIDIn.TextLength = 10 Then ' если длина номера равна 10, то запускаем процесс
            UserInfo = GetUserData(TB_RFIDIn.Text, GB_UserData, GB_WorkAria, L_UserName, TB_RFIDIn)
            TextBox3.Text = "UserID = " & UserInfo(0) & vbCrLf &
                        "Name = " & UserInfo(1) & vbCrLf &
                        "User Group = " & UserInfo(2) & vbCrLf  'UserInfo
            SerialTextBox.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            TB_RFIDIn.Clear()
        End If
    End Sub 'регистрация пользователя
    ' условия для возврата в окно настроек
    Dim OpenSettings As Boolean
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles BT_OpenSettings.Click, BT_LOGInClose.Click
        OpenSettings = True
        Me.Close()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If CloseForm = False Then
            Dim Question As String
            Question = If(OpenSettings = True, "Вы подтверждаете возврат в окно настроек?", "Вы подтверждаете выход из программы?")
            Select Case MsgBox(Question, MsgBoxStyle.YesNo, "")
                Case MsgBoxResult.Yes
                    e.Cancel = False
                    If OpenSettings = True Then
                        SettingsForm.Show()
                    End If
                Case MsgBoxResult.No
                    e.Cancel = True
            End Select
            OpenSettings = False
        End If
    End Sub ' условия для возврата в окно настроек
    '_________________________________________________________________________________________________________________
    'начало работы приложения FAS Scanning Station
    '________________________________________________________________________________________________________________
    'окно ввода серийного номера платы
    Dim PCBCheckRes As New ArrayList()
    Private Sub SerialTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SerialTextBox.KeyDown
        If e.KeyCode = Keys.Enter And SerialTextBox.TextLength = LenSN Then
            PCBCheckRes = CheckPCB(SerialTextBox.Text)
            If PCBCheckRes(0) = True Then
                GetStepResult()
                'PrintLabel(Controllabel, SerialTextBox.Text & " номер принят", 26, 155, Color.Green)
            End If
            SerialTextBox.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            PrintLabel(Controllabel, SerialTextBox.Text & " не верный номер", 26, 155, Color.Red)
            SerialTextBox.Clear()
            SerialTextBox.Focus()
        End If
    End Sub

    Private Function CheckPCB(PCBSN As String) As ArrayList
        Dim PCBRes As New ArrayList()
        Dim res As Boolean
        Dim PCBID As Integer = SelectInt("use SMDCOMPONETS SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] 
                                            where Content = '" & PCBSN & "'")
        If PCBID = 0 Then
            PrintLabel(Controllabel, "Плата " & PCBSN & " не зарегистрирована в базе!", 26, 155, Color.Red)
            PCBRes.Add(False)
        Else
            If PCBSN <> SelectString("use SMDCOMPONETS SELECT top 1 [PCBserial] FROM [SMDCOMPONETS].[dbo].[THTStart] as THT
                                        where PCBserial = '" & PCBSN & "' and PCBResult = 1") Then
                PrintLabel(Controllabel, "Плата " & PCBSN & " не прошла THT Start!", 26, 155, Color.Red)
                PCBRes.Add(False)
            Else
                PCBRes.Add(True)
                PCBRes.Add(PCBID)
                PCBRes.Add(PCBSN)
            End If
        End If
        Return PCBRes
    End Function

    Private Sub GetStepResult()
        ' В аргументах PCBID = PCBCheckRes(1) и PCBSN = PCBCheckRes(2), CurrentStepID = PCInfo(6) и CurrentStep = PCInfo(7)
        Dim PCBStepRes As New ArrayList(SelectListString("USE FAS SELECT [StepID],[TestResult],[ScanDate],[SNID]
                            FROM [FAS].[dbo].[ATestTable_Ct_StepResult] where [PCBID] = " & PCBCheckRes(1)))
        If PCBStepRes.Count = 0 And StartStepID = PCInfo(6) Then
            RunCommand("USE FAS insert into [FAS].[dbo].[ATestTable_Ct_StepResult] ([PCBID],[StepID],[TestResult],[ScanDate])
                        values (" & PCBCheckRes(1) & "," & PCInfo(6) & ",1,CURRENT_TIMESTAMP)")
            RunCommand("Use Fas insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID])values
                    (" & PCBCheckRes(1) & "," & LOTID & "," & PCInfo(6) & ",1,CURRENT_TIMESTAMP,
                    " & UserInfo(0) & "," & PCInfo(2) & ")")
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " проходит этап " & PCInfo(7) & "!", 26, 155, Color.Orange)
            CleareSn()
        ElseIf PCBStepRes.Count = 0 And StartStepID <> PCInfo(6) Then ' шаг не первый, но предыдущего результата нет
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " не прошла этап " & PreStep & "!" &
                      vbCrLf & "Передайте плату на этап " & StartStep & "!", 26, 155, Color.Red)
            CleareSn()
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 1 Then 'Плата имеет статус 1/1
            BT_Pass.Enabled = True
            BT_Fail.Enabled = True
            SerialTextBox.Enabled = False
            Controllabel.Text = "Подтвердите результат теста!"
            CurrrentTimeLabel.Focus()
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 2 Then 'Плата имеет статус 1/2
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " уже прошла этап " & PCInfo(7) & "!" &
                       vbCrLf & "Передайте плату на следующий этап " & NextStep & "!", 26, 155, Color.DarkGreen)
            CleareSn()
        ElseIf PCBStepRes(1) = 3 Then 'Плата имеет статус x/3, то проверить опер лог и определить откуда плата
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " находится в карантине!" &
                       vbCrLf & "Передайте плату в ремонт!", 26, 155, Color.Red)
            CleareSn()
        ElseIf PCBStepRes(0) = PreStepID And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2 (проверка предыдущего шага)
            UpdateStepRes(PCInfo(6), 1, PCBCheckRes(1))
        ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 2 And PCInfo(6) = StartStepID Then 'Плата вернулась из ремонта на первый этап
            UpdateStepRes(PCInfo(6), 1, PCBCheckRes(1))
        ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 2 And PCInfo(6) <> StartStepID Then 'Плата вернулась из ремонта не на первый этап
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " пришла из ремонта." & vbCrLf &
                       "передайте плату на операцию " & StartStep & "!", 26, 155, Color.Red)
            CleareSn()
        ElseIf PCBStepRes(0) <> PCInfo(6) And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " пришла из ремонта." & vbCrLf &
                       "передайте плату на операцию " & StartStep & "!", 26, 155, Color.Red)
            CleareSn()
        End If


    End Sub
    Private Sub UpdateStepRes(StepID As Integer, StepRes As Integer, PcbID As Integer)
        Dim Message As String
        Dim MesColor As Color
        Select Case StepRes
            Case 1
                Message = "Плата " & PCBCheckRes(2) & " проходит этап " & PCInfo(7) & "!"
                MesColor = Color.Orange
            Case 2
                Message = "Плата " & PCBCheckRes(2) & " прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату на следующий этап " & NextStep & "!"
                MesColor = Color.Green
            Case 3
                Message = "Плата " & PCBCheckRes(2) & " не прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату в ремонт!"
                MesColor = Color.Red
        End Select
        RunCommand("USE FAS Update [FAS].[dbo].[ATestTable_Ct_StepResult] 
                    set StepID = " & StepID & ", TestResult = " & StepRes & ", ScanDate = CURRENT_TIMESTAMP
                    where PCBID = " & PcbID)
        RunCommand("insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    (" & PcbID & "," & LOTID & "," & StepID & "," & StepRes & ",CURRENT_TIMESTAMP,
                    " & UserInfo(0) & "," & PCInfo(2) & "," &
                    If(StepRes = 3, ErrorCodeID, "Null") & "," &
                    If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") & ")")
        PrintLabel(Controllabel, Message, 26, 155, MesColor)
        CleareSn()
    End Sub

    Private Sub CurrrentTimeLabel_KeyDown(sender As Object, e As KeyEventArgs) Handles CurrrentTimeLabel.KeyDown
        If e.KeyCode = Keys.Space Then
            BT_Pass_Click(sender, e)
        ElseIf e.KeyCode = Keys.F Then
            BT_Fail_Click(sender, e)
        End If
    End Sub

    Private Sub BT_Pass_Click(sender As Object, e As EventArgs) Handles BT_Pass.Click
        UpdateStepRes(PCInfo(6), 2, PCBCheckRes(1))
    End Sub
    Private Sub BT_SeveErCode_Click(sender As Object, e As EventArgs) Handles BT_SeveErCode.Click
        GetErrorCode()
        UpdateStepRes(PCInfo(6), 3, PCBCheckRes(1))
        CB_ErrorCode.Text = ""

    End Sub

    Private Sub CleareSn()
        SerialTextBox.Clear()
        SerialTextBox.Enabled = True
        GB_ErrorCode.Visible = False
        BT_Pass.Enabled = False
        BT_Fail.Enabled = False
        ErrorCodeID = 0
        TB_Description.Clear()
        SerialTextBox.Focus()
    End Sub

    Private Sub BT_Fail_Click(sender As Object, e As EventArgs) Handles BT_Fail.Click
        GB_ErrorCode.Visible = True
        GB_ErrorCode.Location = New Point(64, 460)
        CB_ErrorCode.Focus()
    End Sub

    Dim ErrorCodeID As Integer
    Dim LastErrCodeText As String
    Private Function GetErrorCode()
        ErrorCodeID = 0
        'определяем errorcodID
        For J = 0 To DG_ErrorCodes.Rows.Count - 1
            If CB_ErrorCode.Text = DG_ErrorCodes.Rows(J).Cells(1).Value Then
                ErrorCodeID = DG_ErrorCodes.Rows(J).Cells(0).Value
                LastErrCodeText = CB_ErrorCode.Text
                Exit For
            End If
        Next
        Return ErrorCodeID
    End Function
    Private Sub CB_ErrorCode_TextChanged(sender As Object, e As EventArgs) Handles CB_ErrorCode.TextChanged
        CB_ErrorCode.MaxLength = 2
        If Len(CB_ErrorCode.Text) = 2 Then

            BT_SeveErCode.Focus()
        ElseIf Len(CB_ErrorCode.Text) <> 2 Then
            Exit Sub
        End If
        BT_SeveErCode.Focus()
    End Sub







End Class