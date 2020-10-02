Imports Library3


Public Class WF_Calibration
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
    Private Sub WF_Calibration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'получение данных о станции
        GB_ErrorCode.Location = New Point(1950, 333)
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
        L_LOT.Text = LOTInfo(1)
        L_Model.Text = LOTInfo(0)
        'загружаем список кодов ошибок в грид SQL запрос "ErrorCodeList" 
        LoadGridFromDB(DG_ErrorCodes, "use FAS select [ErrorCodeID],[ErrorCode],[Description]  FROM [FAS].[dbo].[FAS_ErrorCode] where ErrGroup = 4")
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
        Timer_Calibration.Start()
        ShiftCounterInfo = ShiftCounterStart(PCInfo(4), IDApp, LOTID)
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        YieldCounter()
    End Sub 'Загрузка рабочей формы
    'Счетчик Yield
    Private Sub YieldCounter()
        GetAquaLog()
        If LB_PassLotRes.Text <> 0 Then
            Yield = (CInt(Int(LB_PassLotRes.Text)) / (CInt(Int(LB_PassLotRes.Text)) + CInt(Int(LB_FailLotRes.Text)))) * 100
            LB_Yield.Text = Yield.ToString("00.00")
        Else
            LB_Yield.Text = ""
            LB_Procent.Visible = False
        End If
    End Sub
    Private Sub GetAquaLog()
        Select Case Date.Now.Hour * 3600 + Date.Now.Minute * 60 + Date.Now.Second
            Case 28801 To 72000
                LB_PassLotRes.Text = SelectString("SELECT count(*) FROM [FAS].[dbo].[CT_Aq_Calibration] where Pass = 1 
                                and format (date,'dd.MM.yyyy HH:mm:ss') between ('" & Date.Today & " 08:00:00') and ('" & Date.Today & " 20:00:00')")
                LB_FailLotRes.Text = SelectString("SELECT count(*) FROM [FAS].[dbo].[CT_Aq_Calibration] where Pass = 0 
                                and format (date,'dd.MM.yyyy HH:mm:ss') between ('" & Date.Today & " 08:00:00') and ('" & Date.Today & " 20:00:00')")
            Case 0 To 28800
                LB_PassLotRes.Text = SelectString("SELECT count(*) FROM [FAS].[dbo].[CT_Aq_Calibration] where Pass = 1 
                                and format (date,'dd.MM.yyyy HH:mm:ss') between ('" & Date.Today.AddDays(-1) & " 20:00:00') and ('" & Date.Today & " 08:00:00')")
                LB_FailLotRes.Text = SelectString("SELECT count(*) FROM [FAS].[dbo].[CT_Aq_Calibration] where Pass = 0 
            and format (date,'dd.MM.yyyy HH:mm:ss') between ('" & Date.Today.AddDays(-1) & " 20:00:00') and ('" & Date.Today & " 08:00:00')")
            Case 72000 To 86399
                LB_PassLotRes.Text = SelectString("SELECT count(*) FROM [FAS].[dbo].[CT_Aq_Calibration] where Pass = 1 
                                and format (date,'dd.MM.yyyy HH:mm:ss') between ('" & Date.Today & " 20:00:00') and ('" & Date.Today & " 23:59:59')")
                LB_FailLotRes.Text = SelectString("SELECT count(*) FROM [FAS].[dbo].[CT_Aq_Calibration] where Pass = 0 
                                and format (date,'dd.MM.yyyy HH:mm:ss') between ('" & Date.Today & " 20:00:00') and ('" & Date.Today & " 23:59:59')")
        End Select
    End Sub
    'Часы в программе
    Private Sub CurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles CurrentTimeTimer.Tick
        CurrrentTimeLabel.Text = TimeString
    End Sub 'Часы в программе
    Private Sub Timer_Calibration_Tick(sender As Object, e As EventArgs) Handles Timer_Calibration.Tick
        YieldCounter()
    End Sub



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
            PCBCheckRes = CheckPCB(SerialTextBox.Text, Controllabel, SerialTextBox, BT_Pause, DG_THT_Start)
        If PCBCheckRes(0) = True Then
            PrintLabel(Controllabel, "Введите код ошибки!", 12, 193, Color.OrangeRed)
            'Если плата прошла этапы АОИ и ТНТ Старт
            GetError()
        End If

    End Sub

    'Кнопка Fail 
    Private Sub GetError()
        SerialTextBox.Enabled = False
        DG_UpLog.Visible = False
        GB_ErrorCode.Location = New Point(195, 333)
        GB_ErrorCode.Visible = True
        CB_ErrorCode.Focus()
    End Sub

    'функция обноления результата тестирования для Pass/Fail
    Private Sub UpdateStepRes(StepID As Integer, StepRes As Integer, PcbID As Integer)
        Dim Message As String
        Dim MesColor As Color
        Dim ErrCode As New ArrayList()
        Select Case StepRes
            Case 3
                ErrCode = GetErrorCode()
                Message = "Плата " & PCBCheckRes(2) & " не прошла этап " & PCInfo(7) & "!" &
                   vbCrLf & "Передайте плату в ремонт!"
                MesColor = Color.Red
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Карантин", ErrCode(1), "Плата не прошла этап " & PCInfo(7) & "!" &
                  vbCrLf & "Передайте плату в ремонт!")
        End Select
        Select Case (If(SelectString("select count(*) FROM [FAS].[dbo].[Ct_StepResult] where PCBID =" & PcbID) = 0, 1, 2))
            Case 1
                RunCommand("USE FAS insert into [FAS].[dbo].[Ct_StepResult] (PCBID,StepID,TestResult, ScanDate)
                    values (" & PcbID & "," & StepID & "," & StepRes & ",CURRENT_TIMESTAMP)")
            Case 2
                RunCommand("USE FAS Update [FAS].[dbo].[Ct_StepResult] 
                    set StepID = " & StepID & ", TestResult = " & StepRes & ", ScanDate = CURRENT_TIMESTAMP
                    where PCBID = " & PcbID)
        End Select

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

    'Кнопка Сохранения кода ошибки
    Private Sub BT_SeveErCode_Click(sender As Object, e As EventArgs) Handles BT_SeveErCode.Click
        If CB_ErrorCode.Text = "" Then
            MsgBox("Укажите код ошибки")
        Else
            ShiftCounter(3)
            If CB_ErrorCode.Text = "V5" Then
                UpdateStepRes(PCInfo(6), 6, PCBCheckRes(1))
            Else
                UpdateStepRes(PCInfo(6), 3, PCBCheckRes(1))
            End If

            CB_ErrorCode.Text = ""
            BT_CleareSN_Click(sender, e)
        End If
    End Sub

    'Кнопка закрытия формы записи ошибок
    Private Sub BT_CloseErrMode_Click(sender As Object, e As EventArgs)
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
    Private Sub ShiftCounter(StepRes As Integer)
        ShiftCounterInfo(1) += 1
        ShiftCounterInfo(2) += 1
        If StepRes = 2 Then
            ShiftCounterInfo(3) += 1
        Else
            ShiftCounterInfo(4) += 1
        End If
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_Procent.Visible = True
        ShiftCounterUpdateCT(PCInfo(4), PCInfo(0), ShiftCounterInfo(0), ShiftCounterInfo(1), ShiftCounterInfo(2),
                         ShiftCounterInfo(3), ShiftCounterInfo(4))
    End Sub
    'Кнопка вызова PCB Info Mode
    Private Sub BT_PCBInfo_Click(sender As Object, e As EventArgs) Handles BT_PCBInfo.Click
            Controllabel.Text = ""
            If GB_PCBInfoMode.Visible = False Then
            GB_PCBInfoMode.Visible = True
            'GB_PCBInfoMode.Location = New Point(3, 312)
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

                LoadGridFromDB(DG_THTStartFromDB,
               "SELECT [PCBserial],[PCBResult],format([PCBScanTime],'dd.MM.yyyy HH:mm:ss')  FROM [SMDCOMPONETS].[dbo].[THTStart] 
                where PCBserial = '" & TB_GetPCPInfo.Text & "' 
                order by PCBScanTime")

                For i = 0 To DG_THTStartFromDB.RowCount - 1
                    DG_PCB_Steps.Rows.Add(i + 1, DG_THTStartFromDB.Item(0, i).Value, "THT Start",
                                      If(DG_THTStartFromDB.Item(1, i).Value = True, "Pass", "Fail"), "---",
                                      "---", "---", "---", "---", DG_THTStartFromDB.Item(2, i).Value)
                Next
                Dim Count As Integer = DG_PCB_Steps.RowCount + 1
                For i = 0 To DG_PCBInfoFromDB.RowCount - 1
                    DG_PCB_Steps.Rows.Add(i + Count, DG_PCBInfoFromDB.Item(0, i).Value, DG_PCBInfoFromDB.Item(1, i).Value,
                                      DG_PCBInfoFromDB.Item(2, i).Value, DG_PCBInfoFromDB.Item(3, i).Value,
                                      DG_PCBInfoFromDB.Item(4, i).Value, DG_PCBInfoFromDB.Item(5, i).Value,
                                      DG_PCBInfoFromDB.Item(6, i).Value, DG_PCBInfoFromDB.Item(7, i).Value,
                                      DG_PCBInfoFromDB.Item(8, i).Value)
                Next
                DG_PCB_Steps.Sort(DG_PCB_Steps.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
            End If
        End Sub



    End Class



