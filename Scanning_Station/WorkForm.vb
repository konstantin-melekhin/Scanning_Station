Imports Library3
Imports System.Deployment.Application
Public Class WorkForm
    Dim LOTID, IDApp As Integer
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim LOTInfo As New ArrayList() 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim Yield As Double, RepeatStep As Boolean, PassOrFail As Boolean
    Dim PCBCheckRes As New ArrayList()
#Region "Загрузка рабочей формы"
    Public Sub New(LOTIDWF As Integer, IDApp As Integer)
        InitializeComponent()
        Me.LOTID = LOTIDWF
        Me.IDApp = IDApp
    End Sub
    Private Sub WorkForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myVersion As Version
        If ApplicationDeployment.IsNetworkDeployed Then
            myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion
        End If
        LB_SW_Wers.Text = String.Concat("v", myVersion)

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
        LenSN = If(LOTInfo(2) = True, GetLenSN(LOTInfo(3)), GetLenSN(LOTInfo(8)))
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
        LoadGridFromDB(DG_ErrorCodes, "use FAS select [ErrorCodeID],[ErrorCode],[Description]  FROM [FAS].[dbo].[FAS_ErrorCode] where [ErrGroup] = 4")
        'Записываем коды ошибок в рабочий комбобокс
        If DG_ErrorCodes.Rows.Count <> 0 Then
            For J = 0 To DG_ErrorCodes.Rows.Count - 1
                CB_ErrorCode.Items.Add(DG_ErrorCodes.Rows(J).Cells(1).Value)
            Next
        End If
        'Запуск программы
        '___________________________________________________________
        GB_UserData.Location = New Point(10, 12)
        If PCInfo(6) = 29 Then
            CB_User_Input.Checked = True
            CB_User_Input.Enabled = False
            Label14.Visible = False
            LB_PassLotRes.Visible = False
            Label15.Visible = False
            LB_FailLotRes.Visible = False
            Label17.Visible = False
            LB_Yield.Visible = False
            LB_Procent.Visible = False
            CB_GoldSample.Visible = False
        ElseIf PCInfo(6) = 1 Then
            CB_GoldSample.Visible = True
        End If

        L_UserName.Text = ""
        SerialTextBox.Focus()
        'запуск счетчика продукции за день
        CurrentTimeTimer.Start()
        ShiftCounterInfo = ShiftCounterStart(PCInfo(4), IDApp, LOTID)
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_LOTCounter.Text = ShiftCounterInfo(2)
        LB_PassLotRes.Text = ShiftCounterInfo(3)
        LB_FailLotRes.Text = ShiftCounterInfo(4)
        YieldCounter()
    End Sub 'Загрузка рабочей формы
#End Region
#Region "Yield counter"
    Private Sub YieldCounter()
        If LB_PassLotRes.Text <> 0 Then
            Yield = (CInt(Int(LB_PassLotRes.Text)) / (CInt(Int(LB_PassLotRes.Text)) + CInt(Int(LB_FailLotRes.Text)))) * 100
            LB_Yield.Text = Yield.ToString("00.00")
        Else
            LB_Yield.Text = "100.00"
        End If
    End Sub
#End Region
#Region "Часы в программе"
    Private Sub CurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles CurrentTimeTimer.Tick
        CurrrentTimeLabel.Text = TimeString
    End Sub 'Часы в программе
#End Region
#Region "Регистрация пользователя"
    Dim UserInfo As New ArrayList()
    Private Sub TB_RFIDIn_KeyDown(sender As Object, e As KeyEventArgs) Handles TB_RFIDIn.KeyDown
        TB_RFIDIn.MaxLength = 10
        If e.KeyCode = Keys.Enter And TB_RFIDIn.TextLength = 10 Then ' если длина номера равна 10, то запускаем процесс
            UserInfo = GetUserData(TB_RFIDIn.Text, GB_UserData, GB_WorkAria, L_UserName, TB_RFIDIn)
            If UserInfo.Count <> 0 Then
                TextBox3.Text = "UserID = " & UserInfo(0) & vbCrLf &
                                "Name = " & UserInfo(1) & vbCrLf &
                                "User Group = " & UserInfo(2) & vbCrLf  'UserInfo
                TB_RFIDIn.Clear()
                If SerialTextBox.Text = "" Then
                    If (UserInfo(2) = 5 Or UserInfo(2) = 1) And PCInfo(6) = 1 Then
                        'CB_GoldSample.Checked = True
                        CB_User_Input.Checked = True
                        Controllabel.Text = ""
                    ElseIf (UserInfo(2) <> 5 Or UserInfo(2) <> 1) And PCInfo(6) = 1 Then
                        PrintLabel(Controllabel,
                           $"Пользователь {UserInfo(1)} не является технологом!{vbCrLf}Опция регистрации эталона отклонена!",
                           12, 193, Color.Red)
                        CB_GoldSample.Checked = False
                    End If
                    SerialTextBox.Focus()
                Else
                    ResultAction(sender, e, PassOrFail)
                End If
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            TB_RFIDIn.Clear()
        End If
    End Sub 'регистрация пользователя
    'Опция запроса пользователя / золотого образца / Quality
    Private Sub CB_User_Input_CheckedChanged(sender As Object, e As EventArgs) Handles CB_User_Input.CheckedChanged
        If CB_User_Input.Checked = True Then
            GB_UserData.Visible = True
            GB_WorkAria.Visible = False
            TB_RFIDIn.Focus()
        End If
    End Sub
    Private Sub CB_GoldSample_CheckedChanged(sender As Object, e As EventArgs) Handles CB_GoldSample.CheckedChanged
        If CB_GoldSample.Checked = True Then
            GB_UserData.Visible = True
            GB_WorkAria.Visible = False
            CB_User_Input.Enabled = False
            TB_RFIDIn.Focus()
        ElseIf CB_GoldSample.Checked = False Then
            CB_User_Input.Enabled = True
            CB_User_Input.Checked = False
        End If
    End Sub
    Private Sub CB_Quality_CheckedChanged(sender As Object, e As EventArgs) Handles CB_Quality.CheckedChanged

    End Sub

#End Region
#Region "Условия для возврата в окно настроек"
    Dim OpenSettings As Boolean
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles BT_OpenSettings.Click
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
#End Region
    '_________________________________________________________________________________________________________________
    'начало работы приложения FAS Scanning Station
#Region "Окно ввода серийного номера платы"
    Private Sub SerialTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SerialTextBox.KeyDown
        LB_CurrentErrCode.Text = ""
        Controllabel.Text = ""
        Dim Mess As New ArrayList() 'RDW238120040012'
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
#End Region
#Region "ПОСЛЕДОВАТЕЛЬНОСТЬ ОБРАБОТКИ СЕРИЙНОГО НОМЕРА"
    Private Sub OperatinWithPCB(sender As Object, e As KeyEventArgs)
        Dim Mess As New ArrayList()
        'проверка регистрации платы на THT Start и на гравировщике
        PCBCheckRes = CheckPCB(SerialTextBox.Text)
        If PCBCheckRes(0) = True Then
            'Если плата прошла этапы АОИ и ТНТ Старт
            Mess = GetStepResult()
            If Mess.Count <> 0 Then
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, Mess(0), "", Mess(1))
                PrintLabel(Controllabel, Mess(1), 12, 193, Mess(2))
            End If
            'Если плата не прошла этапы АОИ и ТНТ Старт
        Else
            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "", PCBCheckRes(1))
        End If
        SerialTextBox.Focus()
    End Sub
#End Region
#Region "Проверка регистрации платы на THT Start и на гравировщике"
    Private Function CheckPCB(PCBSN As String) As ArrayList
        Dim PCBRes As New ArrayList()
        'прерка таблицы лазер
        Dim PCBID As Integer
        If LOTInfo(2) = True Then
            PCBID = SelectInt($"use SMDCOMPONETS SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = '{PCBSN}'")
        ElseIf LOTInfo(2) = False And LOTInfo(7) = True Then
            PCBID = SelectInt($"use FAS SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{PCBSN}'")
            If PCBID = 0 Then
                PCBID = SelectInt($"use FAS insert into [FAS].[dbo].[Ct_FASSN_reg] 
                ([SN],[LOTID],[UserID],[AppID],[LineID],[RegDate])values
                ('{PCBSN}',{LOTID}, {1},26,{PCInfo(2)},CURRENT_TIMESTAMP)
                SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{PCBSN}'")
            End If
        End If
        If PCBID = 0 Then
            PrintLabel(Controllabel, "Плата " & PCBSN & " не зарегистрирована в базе!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            PCBRes.Add(False)
            PCBRes.Add("Плата не зарегистрирована в базе!")
            'CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Error", "", "Плата не зарегистрирована в базе!")
        ElseIf LOTInfo(2) = True Then
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
        ElseIf LOTInfo(2) = False And LOTInfo(7) = True Then
            PCBRes.Add(True)
            PCBRes.Add(PCBID)
            PCBRes.Add(PCBSN)
        End If
        Return PCBRes
    End Function
#End Region
#Region "Функция определения результата этапа"
    Private Function GetStepResult() As ArrayList
        'продолжить сдесь, добавить arraylist для месседж
        RepeatStep = New Boolean
        Dim Mess As New ArrayList()
        ' В аргументах PCBID = PCBCheckRes(1) и PCBSN = PCBCheckRes(2), CurrentStepID = PCInfo(6) и CurrentStep = PCInfo(7)
        Dim PCBStepRes As New ArrayList(SelectListString($"Use FAS
                select 
                tt.StepID,tt.TestResultID, tt.StepDate ,tt.SNID
                from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num 
                FROM [FAS].[dbo].[Ct_OperLog] 
                where LOTID = {LOTID} and  PCBID  ={PCBCheckRes(1)}) tt
                where  tt.num = 1"))
        'Если плата не зарегистрирована в таблице StepResult и номер текущей станции совпадает со стартовым этапом
        If PCBStepRes.Count = 0 And StartStepID = PCInfo(6) Then
            SelectAction()
        ElseIf PCBStepRes.Count = 0 And StartStepID <> PCInfo(6) Then ' шаг не первый, но предыдущего результата нет
            Mess.AddRange(New ArrayList() From {"Ошибка", $"Плата {PCBCheckRes(2)} не прошла этап {StartStep}!
                          {vbCrLf} Передайте плату на этап {StartStep}!", Color.Red, False})
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 2 Then 'Плата имеет статус ("текущий шаг"/2)
            PrintLabel(LB_CurrentErrCode, "Плата " & PCBCheckRes(2) & " уже прошла этап " & PCInfo(7) & "!" &
                           vbCrLf & "Передайте плату на следующий этап " & NextStep & " или измените результат!", 12, 270, Color.Red)
            RepeatStep = True
            SelectAction()
            'Если плата в таблице StepResult имеет  результат равен 3
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 3 Then 'Плата имеет статус ("текущий шаг"/3)
            PrintLabel(LB_CurrentErrCode, "Плата уже в карантине. Передайте плату в ремонт или обновите статус!", 12, 270, Color.Red)
            RepeatStep = True
            SelectAction()
            'Если плата в таблице OperLog имеет шаг совпадающий с предыдущей станцией и результат равен 2
        ElseIf PCBStepRes(0) = PreStepID And PCBStepRes(1) = 2 Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)
            SelectAction()
            'Если плата в таблице OperLog имеет шаг совпадающий со станцией ОТК, результат равен 2 
        ElseIf PCBStepRes(0) = 40 And PCBStepRes(1) = 2 Then 'Плата вернулась из ремонта 
            If PCInfo(6) <> 29 Then

            End If

            RepeatStep = True
            SelectAction()
        ElseIf PCBStepRes(0) = 41 And PCBStepRes(1) = 2 Then 'Повторная проверка эталона
            RepeatStep = True
            SelectAction()
            'Если плата в таблице OperLog имеет шаг совпадающий со станцией ОТК, результат равен 3
        ElseIf PCBStepRes(0) = 40 And PCBStepRes(1) = 3 Then
            PrintLabel(Controllabel, "Плата " & PCBCheckRes(2) & " пришла из ремонта." & vbCrLf &
                           "Плата не отремонтирована! Поместите плату в карантин!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            'Если плата в таблице StepResult имеет шаг не совпадающий с предыдущей станцией и результат равен 2
        Else
            Dim sender As Object, e As EventArgs
            BT_PCBInfo_Click(sender, e)
            TB_GetPCPInfo.Text = PCBCheckRes(2)
            GetLogInfo()
            Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! ", Color.Red, False})
            UpdateStepRes(PCInfo(6), 5, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        End If
        'функция возвращает ArrayList со значениями для запись в лог
        Return Mess
    End Function
#End Region
#Region "Выбор действия Pass\Fail"
    Private Sub SelectAction()
        If (CB_User_Input.Checked = True And PCInfo(6) = 29) Or (PCInfo(6) = 1 And CB_Quality.Checked = True) Then
            Dim sender As Object, e As EventArgs
            ResultAction(sender, e, If(CB_Quality.Checked = False, True, False))
        Else
            PrintLabel(Controllabel, "Подтвердите результат теста!", 12, 193, Color.OrangeRed)
            BT_Pass.Visible = True
            BT_Fail.Visible = True
            SerialTextBox.Enabled = False
            CurrrentTimeLabel.Focus()
        End If
    End Sub
#End Region
#Region "Функция обноления результата тестирования для Pass/Fail"
    Private Sub UpdateStepRes(StepID As Integer, StepRes As Integer, PcbID As Integer)
        Dim Message As String
        Dim MesColor As Color
        Dim ErrCode As New ArrayList()
        Select Case StepRes
            Case 2
                If CB_GoldSample.Checked = False And CB_Quality.Checked = False Then
                    Message = "Плата " & PCBCheckRes(2) & " прошла этап " & PCInfo(7) & "!" &
                                       vbCrLf & "Передайте плату на следующий этап " & NextStep & "!"
                    MesColor = Color.Green
                    CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "", "Плата прошла этап " & PCInfo(7) & "!" &
                       vbCrLf & "Передайте плату на следующий этап " & NextStep & "!")
                ElseIf CB_GoldSample.Checked = True Then
                    Message = $"Плата {PCBCheckRes(2)} прошла этап {PCInfo(7) } со статусом ЭТАЛОН!"
                    MesColor = Color.Green
                    CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "",
                                     $"Плата {PCBCheckRes(2)} прошла этап {PCInfo(7) } со статусом ЭТАЛОН!")

                ElseIf CB_Quality.Checked = True Then
                    Message = $"Плата {PCBCheckRes(2)} отправлена на согласование в ОТК!"
                    MesColor = Color.Green
                    CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "",
                                     $"Плата {PCBCheckRes(2)} отправлена на согласование в ОТК!")
                End If
            Case 3
                ErrCode = GetErrorCode()
                If CB_Quality.Checked = False Then
                    Message = "Плата " & PCBCheckRes(2) & " не прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату в ремонт!"
                    MesColor = Color.Red
                    CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Карантин", ErrCode(1), "Плата не прошла этап " & PCInfo(7) & "!" &
                  vbCrLf & "Передайте плату в ремонт!")
                ElseIf CB_Quality.Checked = True Then
                    Message = $"Плата {PCBCheckRes(2)} отправлена на согласование в ОТК!"
                    MesColor = Color.Red
                    CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Согласование", "",
                                     $"Плата {PCBCheckRes(2)} отправлена на согласование в ОТК!")
                End If
            Case 5
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
        If If(ErrCode.Count <> 0, ErrCode(0), 0) = 514 Then
            RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([PCBID], [LOTID], [StepID], [TestResultID], [StepDate],
                    [StepByID], [LineID], [ErrorCodeID], [Descriptions])values
                    ({PcbID},{LOTID},{StepID},{StepRes},CURRENT_TIMESTAMP,
                    {UserInfo(0)},{PCInfo(2)},
                    {If(StepRes = 2, ErrCode(0), "Null")},
                    {If(StepRes = 2, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") })")
        ElseIf CB_Quality.Checked = False And LOTInfo(2) = True Then
            RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    ({PcbID},{LOTID},{If(CB_GoldSample.Checked = True, 41, StepID)},{StepRes},CURRENT_TIMESTAMP,
                    {UserInfo(0)},{PCInfo(2)},
                    {If(StepRes = 3, ErrCode(0), "Null")},
                    {If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") })")
            CB_GoldSample.Checked = False
        ElseIf CB_Quality.Checked = False And LOTInfo(2) = False And LOTInfo(7) = True Then
            RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([SNID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    ({PcbID},{LOTID},{If(CB_GoldSample.Checked = True, 41, StepID)},{StepRes},CURRENT_TIMESTAMP,
                    {UserInfo(0)},{PCInfo(2)},
                    {If(StepRes = 3, ErrCode(0), "Null")},
                    {If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") })")
            CB_GoldSample.Checked = False
        ElseIf CB_Quality.Checked = True Then
            RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    ({PcbID},{LOTID},{42},{StepRes},CURRENT_TIMESTAMP,
                    {UserInfo(0)},{PCInfo(2)},
                    {If(StepRes = 3, ErrCode(0), "Null")},
                    {If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") })")
            CB_Quality.Checked = False
        End If
        PrintLabel(Controllabel, Message, 12, 193, MesColor)
    End Sub
#End Region
#Region "Функция для автоматизации регистрации результата (Пробел - Pass/ F - вызов окна ввода ошибки) и кнопки Pass/Fail"
    Private Sub CurrrentTimeLabel_KeyDown(sender As Object, e As KeyEventArgs) Handles CurrrentTimeLabel.KeyDown
        If e.KeyCode = Keys.Space Then
            BT_Pass_Click(sender, e)
        ElseIf e.KeyCode = Keys.F Then
            BT_Fail_Click(sender, e)
        End If
    End Sub
    'Кнопка Pass
    Private Sub BT_Pass_Click(sender As Object, e As EventArgs) Handles BT_Pass.Click
        If CB_User_Input.Checked = True Then
            ResultAction(sender, e, True)
        Else
            GB_UserData.Visible = True
            GB_WorkAria.Visible = False
            TB_RFIDIn.Focus()
            PassOrFail = True
        End If
    End Sub
    'Кнопка Fail 
    Private Sub BT_Fail_Click(sender As Object, e As EventArgs) Handles BT_Fail.Click
        If CB_User_Input.Checked = True Then
            ResultAction(sender, e, False)
        Else
            GB_UserData.Visible = True
            GB_WorkAria.Visible = False
            TB_RFIDIn.Focus()
            PassOrFail = False
        End If
    End Sub
#End Region
#Region "Закрытие окна ввода пароля"
    Private Sub BT_LOGInClose_Click(sender As Object, e As EventArgs) Handles BT_LOGInClose.Click
        GB_UserData.Visible = False
        GB_WorkAria.Visible = True
        SerialTextBox.Enabled = False
        CB_User_Input.Checked = False
        MsgBox("Окно определения пользователя было закрыто.")
    End Sub
#End Region
#Region "Событие при нажатии Pass/Fail"
    Private Sub ResultAction(sender As Object, e As EventArgs, res As Boolean)
        If UserInfo.Count <> 0 And res = True Then
            ShiftCounter(2, RepeatStep)
            UpdateStepRes(PCInfo(6), 2, PCBCheckRes(1))
            BT_CleareSN_Click(sender, e)
            LB_CurrentErrCode.Text = ""
        ElseIf UserInfo.Count <> 0 And res = False Then
            GB_ErrorCode.Visible = True
            GB_ErrorCode.Location = New Point(180, 333)
            DG_UpLog.Visible = False
            CB_ErrorCode.Text = If(CB_Quality.Checked = True, "QT", "")
            CB_ErrorCode.Focus()
        End If
    End Sub
#End Region
#Region "Обработка ввода кода ошибки"
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
#End Region
#Region "Кнопка очистки поля ввода номера"
    Private Sub BT_CleareSN_Click(sender As Object, e As EventArgs) Handles BT_CleareSN.Click
        If SerialTextBox.Text = "" Then
            LB_CurrentErrCode.Text = ""
            Controllabel.Text = ""
        End If
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
#End Region
#Region "Функция запролнения LogGrid "
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN As String, ScanRes As String, ErrCode As String, Descr As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN, ScanRes, Date.Now, ErrCode, Descr)
        DG_UpLog.Sort(DG_UpLog.Columns(3), System.ComponentModel.ListSortDirection.Descending)
    End Sub
#End Region
#Region "Счетчик продукции"
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
#End Region
#Region "Кнопка вызова PCB Info Mode / 'Проверка шагов сканирования требуемой платы"
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
        If e.KeyCode = Keys.Enter Then
            GetLogInfo()
        End If
    End Sub

    Private Sub GetLogInfo()
        LoadGridFromDB(DG_PCB_Steps,
                    $"Use FAS
select 
num '№',
(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) 'Номер платы',
(select StepName from  [FAS].[dbo].[Ct_StepScan] where id = tt.StepID  ) 'Название станции',
(select Result from [FAS].[dbo].[Ct_TestResult] where id = tt.TestResultID ) 'Результат',
(select E.ErrorCode from [FAS].[dbo].[FAS_ErrorCode] E where ErrorCodeID = tt.ErrorCodeID ) 'Код ошибки',
(select E.Description from [FAS].[dbo].[FAS_ErrorCode] E where ErrorCodeID = tt.ErrorCodeID ) 'Описание ошибки',
tt.Descriptions 'Примечание',
(select L.LineName from [FAS].[dbo].[FAS_Lines] L where L.LineID = tt.LineID) 'Линия',
(select U.UserName from [FAS].[dbo].[FAS_Users] U where U.UserID = tt.StepByID) 'Пользователь',
format([StepDate],'dd.MM.yyyy HH:mm:ss') 'Дата'
from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate ) num 
FROM [FAS].[dbo].[Ct_OperLog] 
where PCBID  = (SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = '{TB_GetPCPInfo.Text}')) tt
order by num")
        TB_GetPCPInfo.Enabled = False
    End Sub
#End Region
End Class