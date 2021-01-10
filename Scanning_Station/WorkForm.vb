Imports Library3
Public Class WorkForm
    Dim LOTID, IDApp As Integer
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim LOTInfo As New ArrayList() 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim Yield As Double
    Public Sub New(LOTIDWF As Integer, IDApp As Integer)
        InitializeComponent()
        Me.LOTID = LOTIDWF
        Me.IDApp = IDApp
    End Sub
    'Загрузка рабочей формы


    Private Sub WorkForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LB_CurrentErrCode.Text = ""
        'получение данных о станции
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
        'получение данных о текущем лоте
        LOTInfo = GetCurrentContractLot(LOTID)
        LenSN = GetLenSN(LOTInfo(3))
        TextBox2.Text = "Model = " & LOTInfo(0) & vbCrLf &
                        "LOT = " & LOTInfo(1) & vbCrLf &
                        "CheckFormatSN_SMT = " & LOTInfo(2) & vbCrLf &
                        "SMTNumberFormat = " & LOTInfo(3) & vbCrLf &
                        "SMTRangeChecked = " & LOTInfo(4) & vbCrLf &
                        "SMTStartRange = " & LOTInfo(5) & vbCrLf &
                        "SMTEndRange = " & LOTInfo(6) & vbCrLf &
                        "CheckFormatSN_FAS = " & LOTInfo(7) & vbCrLf &
                        "FASNumberFormat = " & LOTInfo(8) & vbCrLf &
                        "FASRangeChecked = " & LOTInfo(9) & vbCrLf &
                        "FASStartRange = " & LOTInfo(10) & vbCrLf &
                        "FASEndRange = " & LOTInfo(11) & vbCrLf &
                        "SingleSN = " & LOTInfo(12) & vbCrLf &
                        "ParseLog = " & LOTInfo(13) & vbCrLf &
                        "StepSequence = " & LOTInfo(14) & vbCrLf &
                        "BoxCapacity = " & LOTInfo(15) & vbCrLf &
                        "PalletCapacity = " & LOTInfo(16) & vbCrLf &
                        "LiterIndex = " & LOTInfo(17) & vbCrLf &
                        "PreRackStage = " & LOTInfo(18) &
                        "LenSN = " & LenSN & vbCrLf 'LOTInfo
        'Определить стартовый шаг, текущий и последующий
        StepSequence = New String(Len(LOTInfo(14)) / 2 - 1) {}
        For i = 0 To Len(LOTInfo(14)) - 1 Step 2
            Dim J As Integer
            StepSequence(J) = Mid(LOTInfo(14), i + 1, 2)
            J += 1
        Next
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
        LB_PassLotRes.Text = ShiftCounterInfo(3)
        LB_FailLotRes.Text = ShiftCounterInfo(4)
        YieldCounter()
    End Sub 'Загрузка рабочей формы

    Private Sub YieldCounter()
        If LB_PassLotRes.Text <> 0 Then
            Yield = (CInt(Int(LB_PassLotRes.Text)) / (CInt(Int(LB_PassLotRes.Text)) + CInt(Int(LB_FailLotRes.Text)))) * 100
            LB_Yield.Text = Yield.ToString("00.00")
        Else
            LB_Yield.Text = "100.00"
        End If
    End Sub

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
    End Sub ' условия для возврата в окно настроек
    '_________________________________________________________________________________________________________________
    'начало работы приложения FAS Scanning Station
    'окно ввода серийного номера платы
    Dim PCBCheckRes As New ArrayList()
    Private Sub SerialTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SerialTextBox.KeyDown
        LB_CurrentErrCode.Text = ""
        Dim Mess As New ArrayList()
        If e.KeyCode = Keys.Enter And SerialTextBox.TextLength = LenSN Then
            OperatinWithPCB(sender, e)
            'если введен не верный номер
        ElseIf e.KeyCode = Keys.Enter Then
            PrintLabel(Controllabel, SerialTextBox.Text & " не верный номер", 12, 193, Color.Red)
            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "", "Плата имеет не верный номер")
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        End If
    End Sub
    'ПОСЛЕДОВАТЕЛЬНОСТЬ ОБРАБОТКИ СЕРИЙНОГО НОМЕРА
    Private Sub OperatinWithPCB(sender As Object, e As KeyEventArgs)
        Dim Mess As New ArrayList()
        'проверка регистрации платы на THT Start и на гравировщике
        PCBCheckRes = CheckPCB(SerialTextBox.Text)
        If PCBCheckRes(0) = True Then
            'Если плата прошла этапы АОИ и ТНТ Старт
            Mess = GetStepResult()
            'If BT_Pass.Visible = False Then
            '    CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, Mess(0), "", Mess(1))
            '    PrintLabel(Controllabel, Mess(1), 12, 193, Mess(2))
            '    If Mess(3) = True Then
            '        BT_CleareSN_Click(sender, e)
            '    End If
            'End If
            'Если плата не прошла этапы АОИ и ТНТ Старт
        Else
            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "", PCBCheckRes(1))
        End If
        SerialTextBox.Focus()
    End Sub
    Private Function CheckPCB(PCBSN As String) As ArrayList
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
            If PCBSN <> SelectString("use SMDCOMPONETS SELECT top 1 [PCBserial] FROM [SMDCOMPONETS].[dbo].[THTStart] as THT
                                        where PCBserial = '" & PCBSN & "' and PCBResult = 1") Then
                PrintLabel(Controllabel, "Плата " & PCBSN & " не прошла THT Start!", 12, 193, Color.Red)
                SerialTextBox.Enabled = False
                BT_Pause.Focus()
                PCBRes.Add(False)
                PCBRes.Add("Плата не прошла THT Start!")
            Else
                PCBRes.Add(True)
                PCBRes.Add(PCBID)
                PCBRes.Add(PCBSN)
            End If
        End If
        Return PCBRes
    End Function
    'функция определения результата этапа
    'Private Function GetStepResult() As ArrayList
    '    'продолжить сдесь, добавить arraylist для месседж
    '    Dim Mess As New ArrayList()
    '    ' В аргументах PCBID = PCBCheckRes(1) и PCBSN = PCBCheckRes(2), CurrentStepID = PCInfo(6) и CurrentStep = PCInfo(7)
    '    Dim PCBStepRes As New ArrayList(SelectListString("USE FAS SELECT [StepID],[TestResult],[ScanDate],[SNID]
    '                        FROM [FAS].[dbo].[Ct_StepResult] where [PCBID] = " & PCBCheckRes(1)))
    '    'Если плата не зарегистрирована в таблице StepResult и номер текущей станции совпадает со стартовым этапом
    '    If PCBStepRes.Count = 0 And StartStepID = PCInfo(6) Then
    '        'Создаем запись о прохождении первого шага в таблице StepResult и OperLog
    '        RunCommand("USE FAS insert into [FAS].[dbo].[Ct_StepResult] ([PCBID],[StepID],[TestResult],[ScanDate])
    '                    values (" & PCBCheckRes(1) & "," & PCInfo(6) & ",2,CURRENT_TIMESTAMP)")
    '        RunCommand("Use Fas insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
    '                [StepByID],[LineID])values
    '                (" & PCBCheckRes(1) & "," & LOTID & "," & PCInfo(6) & ",2,CURRENT_TIMESTAMP,
    '                " & UserInfo(0) & "," & PCInfo(2) & ")")
    '        Mess.AddRange(New ArrayList() From {"В процессе", "Плата " & PCBCheckRes(2) & " проходит этап " & PCInfo(7) & "!", Color.Orange, True})
    '        'Если плата не зарегистрирована в таблице StepResult и но номер текущей станции совпадает со стартовым этапом

    '    ElseIf PCBStepRes.Count = 0 And StartStepID <> PCInfo(6) Then ' шаг не первый, но предыдущего результата нет
    '        Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " не прошла этап " & PreStep & "!" &
    '                      vbCrLf & "Передайте плату на этап " & StartStep & "!", Color.Red, False})
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '        'Если плата в таблице StepResult имеет шаг совпадающий с текущей станцией и результат равен 1
    '    ElseIf PCBStepRes(0) = PCInfo(6) And PCInfo(6) = 1 And PCBStepRes(1) = 1 Then 'Плата имеет статус 1/1
    '        BT_Pass.Visible = True
    '        BT_Fail.Visible = True
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '        PrintLabel(Controllabel, "Подтвердите результат теста!", 12, 193, Color.OrangeRed)
    '        CurrrentTimeLabel.Focus()
    '        'Если плата в таблице StepResult имеет шаг совпадающий с текущей станцией и результат равен 2
    '    ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 2 Then 'Плата имеет статус 1/2
    '        Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " уже прошла этап " & PCInfo(7) & "!" &
    '                       vbCrLf & "Передайте плату на следующий этап " & NextStep & "!", Color.DarkGreen, False})
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '        'Если плата в таблице StepResult имеет  результат равен 3
    '    ElseIf PCInfo(6) <> 4 And PCBStepRes(1) = 3 Then 'Плата имеет статус x/3, то проверить опер лог и определить откуда плата
    '        Mess.AddRange(New ArrayList() From {"Карантин", "Плата " & PCBCheckRes(2) & " находится в карантине!" &
    '                       vbCrLf & "Проверьте информацию о плате передайте ее в ремонт!", Color.Red, False})
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '        'Если плата в таблице StepResult имеет шаг совпадающий с предыдущей станцией и результат равен 2
    '    ElseIf PCBStepRes(0) = PreStepID And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2 (проверка предыдущего шага)
    '        Mess.AddRange(New ArrayList() From {"В процессе", "Плата " & PCBCheckRes(2) & " проходит этап " & PCInfo(7) & "!", Color.Orange, True})
    '        UpdateStepRes(PCInfo(6), 1, PCBCheckRes(1))
    '        'Если плата в таблице StepResult имеет шаг совпадающий со станцией ремонта, результат равен 2 и 
    '        'номер текущей станции совпадает со стартовой станцией
    '    ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 2 And PCInfo(6) = StartStepID Then 'Плата вернулась из ремонта на первый этап
    '        Mess.AddRange(New ArrayList() From {"В процессе", "Плата " & PCBCheckRes(2) & " проходит этап " & PCInfo(7) & "!", Color.Orange, True})
    '        UpdateStepRes(PCInfo(6), 1, PCBCheckRes(1))
    '        'Если плата в таблице StepResult имеет шаг совпадающий со станцией ремонта, результат равен 2 и 
    '        'номер текущей станции не совпадает со стартовой станцией
    '    ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 2 And PCInfo(6) <> StartStepID Then 'Плата вернулась из ремонта не на первый этап
    '        Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " пришла из ремонта." & vbCrLf &
    '                       "передайте плату на операцию " & StartStep & "!", Color.Red, False})
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '        'Если плата в таблице StepResult имеет шаг не совпадающий с предыдущей станцией и результат равен 2
    '    ElseIf PCBStepRes(0) <> PreStepID And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2
    '        'Проверить опер лог и изменить коментарий
    '        Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! " & vbCrLf &
    '                       "Перейдите во вкладку ИНФО и опредилите принадлежность платы!", Color.Red, False})
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '        'Если плата в таблице StepResult емеет результат 1 и текущий шаг = 4 (станция ремонта)
    '    ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 1 Then
    '        Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " зарегистрирована в ремонте!", Color.Red, False})
    '        SerialTextBox.Enabled = False
    '        BT_Pause.Focus()
    '    End If
    '    'функция возвращает ArrayList со значениями для запись в лог
    '    Return Mess
    'End Function

    'функция определения результата этапа
    Dim FirstStep, RepeatStep As Boolean
    Private Function GetStepResult() As ArrayList
        'продолжить сдесь, добавить arraylist для месседж
        RepeatStep = New Boolean
        FirstStep = New Boolean
        Dim Mess As New ArrayList()
        ' В аргументах PCBID = PCBCheckRes(1) и PCBSN = PCBCheckRes(2), CurrentStepID = PCInfo(6) и CurrentStep = PCInfo(7)
        Dim PCBStepRes As New ArrayList(SelectListString("USE FAS SELECT [StepID],[TestResult],[ScanDate],[SNID]
                            FROM [FAS].[dbo].[Ct_StepResult] where [PCBID] = " & PCBCheckRes(1)))
        'Если плата не зарегистрирована в таблице StepResult и номер текущей станции совпадает со стартовым этапом
        If PCBStepRes.Count = 0 And StartStepID = PCInfo(6) Then
            FirstStep = True
            SelectAction()
        ElseIf PCBStepRes.Count = 0 And StartStepID <> PCInfo(6) Then ' шаг не первый, но предыдущего результата нет
            Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " не прошла этап THT END!" &
                          vbCrLf & "Передайте плату на этап THT END!", Color.Red, False})
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        ElseIf PCBStepRes(0) = PCInfo(6) And PCInfo(6) = 1 And PCBStepRes(1) = 1 Then 'Плата имеет статус 1/1
            SelectAction()
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 2 Then 'Плата имеет статус 1/2
            'Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " уже прошла этап " & PCInfo(7) & "!" &
            '               vbCrLf & "Передайте плату на следующий этап " & NextStep & " или измените результат!", Color.Red, False})

            PrintLabel(LB_CurrentErrCode, "Плата " & PCBCheckRes(2) & " уже прошла этап " & PCInfo(7) & "!" &
                           vbCrLf & "Передайте плату на следующий этап " & NextStep & " или измените результат!", 12, 270, Color.Red)
            RepeatStep = True
            SelectAction()
            'SerialTextBox.Enabled = False
            'BT_Pause.Focus()
            'Если плата в таблице StepResult имеет  результат равен 3
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 3 Then 'Плата имеет статус x/3, то проверить опер лог и определить откуда плата
            PrintLabel(LB_CurrentErrCode, "Плата уже в карантине. Передайте плату в ремонт или обновите статус!", 12, 270, Color.Red)
            RepeatStep = True
            SelectAction()
            'Если плата в таблице StepResult имеет шаг совпадающий с предыдущей станцией и результат равен 2
        ElseIf PCBStepRes(0) = PreStepID And PCInfo(6) = 1 And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2 (проверка предыдущего шага)
            UpdateStepRes(1, 1, PCBCheckRes(1))
            SerialTextBox.Text = ""
            'Если плата в таблице StepResult имеет шаг совпадающий со станцией ремонта, результат равен 2 и 
            'номер текущей станции совпадает со стартовой станцией
        ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 2 Then 'Плата вернулась из ремонта на первый этап  And PCInfo(6) = StartStepID
            UpdateStepRes(1, 1, PCBCheckRes(1))
            SerialTextBox.Text = ""
            'Если плата в таблице StepResult имеет шаг совпадающий со станцией ремонта, результат равен 2 и 
            'номер текущей станции не совпадает со стартовой станцией
        ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 3 Then 'Плата вернулась из ремонта не на первый этап And PCInfo(6) <> StartStepID
            'Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " пришла из ремонта." & vbCrLf &
            '               "Плата не отремонтирована! Поместите плату в карантин!", Color.Red, False})
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " пришла из ремонта." & vbCrLf &
                           "Плата не отремонтирована! Поместите плату в карантин!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            'Если плата в таблице StepResult имеет шаг не совпадающий с предыдущей станцией и результат равен 2
        ElseIf PCBStepRes(0) <> PreStepID And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2
            'Проверить опер лог и изменить коментарий
            Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! " & vbCrLf &
                           "Перейдите во вкладку ИНФО и опредилите принадлежность платы!", Color.Red, False})
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        Else
            Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! " & vbCrLf &
                           "Перейдите во вкладку ИНФО и опредилите принадлежность платы!", Color.Red, False})
            UpdateStepRes(PCInfo(6), 5, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        End If
        'функция возвращает ArrayList со значениями для запись в лог
        Return Mess
    End Function
    'выбрать действие Pass\Fail
    Private Sub SelectAction()
        PrintLabel(Controllabel, "Подтвердите результат теста!", 12, 193, Color.OrangeRed)
        BT_Pass.Visible = True
        BT_Fail.Visible = True
        SerialTextBox.Enabled = False
        CurrrentTimeLabel.Focus()
    End Sub
    'функция обноления результата тестирования для Pass/Fail
    'Private Sub UpdateStepRes(StepID As Integer, StepRes As Integer, PcbID As Integer)
    '    Dim Message As String
    '    Dim MesColor As Color
    '    Dim ErrCode As New ArrayList()
    '    Select Case StepRes
    '        Case 2
    '            Message = "Плата " & PCBCheckRes(2) & " прошла этап " & PCInfo(7) & "!" &
    '               vbCrLf & "Передайте плату на следующий этап " & NextStep & "!"
    '            MesColor = Color.Green
    '            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "", "Плата прошла этап " & PCInfo(7) & "!" &
    '               vbCrLf & "Передайте плату на следующий этап " & NextStep & "!")
    '        Case 3
    '            ErrCode = GetErrorCode()
    '            Message = "Плата " & PCBCheckRes(2) & " не прошла этап " & PCInfo(7) & "!" &
    '               vbCrLf & "Передайте плату в ремонт!"
    '            MesColor = Color.Red
    '            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Карантин", ErrCode(1), "Плата не прошла этап " & PCInfo(7) & "!" &
    '              vbCrLf & "Передайте плату в ремонт!")
    '    End Select
    '    RunCommand("USE FAS Update [FAS].[dbo].[Ct_StepResult] 
    '                set StepID = " & StepID & ", TestResult = " & StepRes & ", ScanDate = CURRENT_TIMESTAMP
    '                where PCBID = " & PcbID)
    '    RunCommand("insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
    '                [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
    '                (" & PcbID & "," & LOTID & "," & StepID & "," & StepRes & ",CURRENT_TIMESTAMP,
    '                " & UserInfo(0) & "," & PCInfo(2) & "," &
    '                If(StepRes = 3, ErrCode(0), "Null") & "," &
    '                If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") & ")")
    '    PrintLabel(Controllabel, Message, 12, 193, MesColor)
    'End Sub
    'функция обноления результата тестирования для Pass/Fail
    Private Sub UpdateStepRes(StepID As Integer, StepRes As Integer, PcbID As Integer)
        Dim Message As String
        Dim MesColor As Color
        Dim ErrCode As New ArrayList()
        Select Case StepRes
            Case 1
                Message = "Плата " & PCBCheckRes(2) & " проходит этап " & PCInfo(7) & "!"
                MesColor = Color.Orange
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "В процессе", "", "Плата проходит этап " & PCInfo(7) & "!")
            Case 2
                Message = "Плата " & PCBCheckRes(2) & " прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату на следующий этап " & NextStep & "!"
                MesColor = Color.Green
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "", "Плата прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату на следующий этап " & NextStep & "!")
            Case 3
                ErrCode = GetErrorCode()
                Message = "Плата " & PCBCheckRes(2) & " не прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату в ремонт!"
                MesColor = Color.Red
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Карантин", ErrCode(1), "Плата не прошла этап " & PCInfo(7) & "!" &
                  vbCrLf & "Передайте плату в ремонт!")
            Case 5
                Message = "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! " & vbCrLf &
                           "Перейдите во вкладку ИНФО и опредилите принадлежность платы!"
                MesColor = Color.Red
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "", "Плата имеет не верный предыдыдущий шаг!")
                PrintLabel(Controllabel, Message, 12, 193, MesColor)
                Exit Sub
            Case 6
                StepRes = 2
                ErrCode = GetErrorCode()
                Message = "Плата " & PCBCheckRes(2) & " прошла этап " & PCInfo(7) & " с ошибкой V5!" &
                   vbCrLf & "Передайте плату на следующий этап " & NextStep & "!"
                MesColor = Color.Green
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "", "Плата прошла этап " & PCInfo(7) & " с ошибкой V5!" &
                   vbCrLf & "Передайте плату на следующий этап " & NextStep & "!")
        End Select


        If FirstStep = True Then
            ''Создаем запись о прохождении первого шага в таблице StepResult и OperLog
            RunCommand("USE FAS insert into [FAS].[dbo].[Ct_StepResult] ([PCBID],[StepID],[TestResult],[ScanDate])
                        values (" & PCBCheckRes(1) & "," & PCInfo(6) & "," & StepRes & ",CURRENT_TIMESTAMP)")
        Else
            RunCommand("USE FAS Update [FAS].[dbo].[Ct_StepResult] 
                    set StepID = " & StepID & ", TestResult = " & StepRes & ", ScanDate = CURRENT_TIMESTAMP
                    where PCBID = " & PcbID)
        End If

        If If(ErrCode.Count <> 0, ErrCode(0), 0) = 514 Then
            RunCommand("insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    (" & PcbID & "," & LOTID & "," & StepID & "," & StepRes & ",CURRENT_TIMESTAMP,
                    " & UserInfo(0) & "," & PCInfo(2) & "," &
                                If(StepRes = 2, ErrCode(0), "Null") & "," &
                                If(StepRes = 2, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") & ")")
        Else
            RunCommand("insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    (" & PcbID & "," & LOTID & "," & StepID & "," & StepRes & ",CURRENT_TIMESTAMP,
                    " & UserInfo(0) & "," & PCInfo(2) & "," &
                    If(StepRes = 3, ErrCode(0), "Null") & "," &
                    If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") & ")")
        End If
        PrintLabel(Controllabel, Message, 12, 193, MesColor)
    End Sub
    'Функция для автоматизации регистрации результата (Пробел - Pass/ F - вызов окна ввода ошибки
    Private Sub CurrrentTimeLabel_KeyDown(sender As Object, e As KeyEventArgs) Handles CurrrentTimeLabel.KeyDown
        If e.KeyCode = Keys.Space Then
            BT_Pass_Click(sender, e)
        ElseIf e.KeyCode = Keys.F Then
            BT_Fail_Click(sender, e)
        End If
    End Sub
    'Кнопка Pass
    Private Sub BT_Pass_Click(sender As Object, e As EventArgs) Handles BT_Pass.Click
        ShiftCounter(2, RepeatStep)
        UpdateStepRes(PCInfo(6), 2, PCBCheckRes(1))
        BT_CleareSN_Click(sender, e)
        LB_CurrentErrCode.Text = ""
    End Sub
    'Кнопка Сохранения кода ошибки

    Private Sub BT_SeveErCode_Click(sender As Object, e As EventArgs) Handles BT_SeveErCode.Click
        If CB_ErrorCode.Text = "" Then
            MsgBox("Укажите код ошибки")
        Else
            ShiftCounter(3, RepeatStep)
            UpdateStepRes(PCInfo(6), 3, PCBCheckRes(1))
            CB_ErrorCode.Text = ""
            BT_CleareSN_Click(sender, e)
        End If
        LB_CurrentErrCode.Text = ""
    End Sub


    'Кнопка Fail 
    Private Sub BT_Fail_Click(sender As Object, e As EventArgs) Handles BT_Fail.Click
        GB_ErrorCode.Visible = True
        GB_ErrorCode.Location = New Point(180, 333)
        DG_UpLog.Visible = False
        CB_ErrorCode.Focus()
    End Sub
    'Кнопка закрытия формы записи ошибок
    Private Sub BT_CloseErrMode_Click(sender As Object, e As EventArgs) Handles BT_CloseErrMode.Click
        GB_ErrorCode.Visible = False
        DG_UpLog.Visible = True
        CurrrentTimeLabel.Focus()
    End Sub

    Private Function GetErrorCode() As ArrayList
        Dim ErrorCode As New ArrayList()
        'определяем errorcodID
        For J = 0 To DG_ErrorCodes.Rows.Count - 1
            If CB_ErrorCode.Text = DG_ErrorCodes.Rows(J).Cells(1).Value Then
                ErrorCode.Add(DG_ErrorCodes.Rows(J).Cells(0).Value)
                ErrorCode.Add(DG_ErrorCodes.Rows(J).Cells(1).Value)
                Exit For
            End If
        Next
        Return ErrorCode
    End Function
    'Поиск введенного кода ошибки в гриде
    Private Sub CB_ErrorCode_TextChanged(sender As Object, e As EventArgs) Handles CB_ErrorCode.TextChanged
        CB_ErrorCode.MaxLength = 2
        If Len(CB_ErrorCode.Text) = 2 Then
            BT_SeveErCode.Focus()
        ElseIf Len(CB_ErrorCode.Text) <> 2 Then
            Exit Sub
        End If
        BT_SeveErCode.Focus()
    End Sub
    'Кнопка очистки поля ввода номера
    Private Sub BT_CleareSN_Click(sender As Object, e As EventArgs) Handles BT_CleareSN.Click
        If GB_PCBInfoMode.Visible = False Then
            SerialTextBox.Clear()
            SerialTextBox.Enabled = True
            GB_ErrorCode.Visible = False
            BT_Pass.Visible = False
            BT_Fail.Visible = False
            DG_UpLog.Visible = True
            TB_Description.Clear()
            SerialTextBox.Focus()
        Else
            TB_GetPCPInfo.Clear()
            TB_GetPCPInfo.Enabled = True
            TB_GetPCPInfo.Focus()
        End If
    End Sub
    'Функция запролнения LogGrid 
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN As String, ScanRes As String, ErrCode As String, Descr As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN, ScanRes, Date.Now, ErrCode, Descr)
        DG_UpLog.Sort(DG_UpLog.Columns(3), System.ComponentModel.ListSortDirection.Descending)
    End Sub
    'Счетчик продукции
    Private Sub ShiftCounter(StepRes As Integer, Repeat As Boolean)
        If Repeat = False Then
            ShiftCounterInfo(1) += 1
            ShiftCounterInfo(2) += 1
            If StepRes = 2 Then
                ShiftCounterInfo(3) += 1
            ElseIf StepRes = 3 And Repeat = False Then
                ShiftCounterInfo(4) += 1
            End If
            Label_ShiftCounter.Text = ShiftCounterInfo(1)
            LB_LOTCounter.Text = ShiftCounterInfo(2)
            LB_PassLotRes.Text = ShiftCounterInfo(3)
            LB_FailLotRes.Text = ShiftCounterInfo(4)
            YieldCounter()
            ShiftCounterUpdateCT(PCInfo(4), PCInfo(0), ShiftCounterInfo(0), ShiftCounterInfo(1), ShiftCounterInfo(2),
                            ShiftCounterInfo(3), ShiftCounterInfo(4))
        End If
    End Sub
    'Кнопка вызова PCB Info Mode
    Private Sub BT_PCBInfo_Click(sender As Object, e As EventArgs) Handles BT_PCBInfo.Click
        Controllabel.Text = ""
        If GB_PCBInfoMode.Visible = False Then
            GB_PCBInfoMode.Visible = True
            TB_GetPCPInfo.Focus()
        Else
            GB_PCBInfoMode.Visible = False
        End If
        BT_CleareSN_Click(sender, e)
    End Sub
    'Проверка шагов сканирования требуемой платы
    Private Sub TB_GetPCPInfo_KeyDown(sender As Object, e As KeyEventArgs) Handles TB_GetPCPInfo.KeyDown
        DG_PCB_Steps.Rows.Clear()
        If e.KeyCode = Keys.Enter Then
            LoadGridFromDB(DG_PCBInfoFromDB,
                    "use FAS SELECT ls.Content,Ss.StepName,Sr.Result,Er.ErrorCode,Er.Description,[Descriptions],Ln.LineName,Us.UserName
        ,format([StepDate],'dd.MM.yyyy HH:mm:ss') as Date
        FROM [FAS].[dbo].[Ct_OperLog] as Lg
        left join SMDCOMPONETS.dbo.LazerBase as Ls On Ls.IDLaser = lg.PCBID
        left join [FAS].[dbo].[Ct_StepScan] as Ss On Ss.ID = StepID
        left join [FAS].[dbo].[Ct_TestResult] as Sr On Sr.ID = TestResultID
        left join [FAS].[dbo].[FAS_Users] as Us On Us.UserID = [StepByID]
        left join [FAS].[dbo].[FAS_Lines] as Ln On Ln.LineID = Lg.LineID
        left join [FAS].[dbo].[FAS_ErrorCode] as Er On Er.ErrorCodeID = Lg.ErrorCodeID
        where Ls.Content = '" & TB_GetPCPInfo.Text & "'
        order by StepDate")
            TB_GetPCPInfo.Enabled = False

            For i = 0 To DG_PCBInfoFromDB.RowCount - 1
                DG_PCB_Steps.Rows.Add(i + 1, DG_PCBInfoFromDB.Item(0, i).Value, DG_PCBInfoFromDB.Item(1, i).Value,
                                      DG_PCBInfoFromDB.Item(2, i).Value, DG_PCBInfoFromDB.Item(3, i).Value,
                                      DG_PCBInfoFromDB.Item(4, i).Value, DG_PCBInfoFromDB.Item(5, i).Value,
                                      DG_PCBInfoFromDB.Item(6, i).Value, DG_PCBInfoFromDB.Item(7, i).Value,
                                      DG_PCBInfoFromDB.Item(8, i).Value)
            Next
            DG_PCB_Steps.Sort(DG_PCB_Steps.Columns(9), System.ComponentModel.ListSortDirection.Ascending)
        End If
    End Sub
End Class