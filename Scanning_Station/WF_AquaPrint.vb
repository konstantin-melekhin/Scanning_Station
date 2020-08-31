Imports Library3

Public Class WF_AquaPrint
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
    Private Sub WF_AquaPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        LoadGridFromDB(DG_ErrorCodes, "use FAS select [ErrorCodeID],[ErrorCode],[Description]  FROM [FAS].[dbo].[FAS_ErrorCode] where ErrGroup = 4")
        'Записываем коды ошибок в рабочий комбобокс
        If DG_ErrorCodes.Rows.Count <> 0 Then
            For J = 0 To DG_ErrorCodes.Rows.Count - 1
                CB_ErrorCode.Items.Add(DG_ErrorCodes.Rows(J).Cells(1).Value)
            Next
        End If
        'Устанавливаем дефолты при загоузке формы 
        'Настройка COM порта
        PrintSerialPort.PortName = "com3"
        PrintSerialPort.BaudRate = 115200
        'Требуется печать или нет
        Try
            PrintSerialPort.Open()
            PrintSerialPort.Close()
        Catch ex As Exception
            PrintLabel(Controllabel, "Проверьте подключение ком порта!", 12, 193, Color.Red) ' если не настроен ком порт для печати
            SerialTextBox.Enabled = False
        End Try
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
            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "Плата имеет не верный номер")
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
            'Если плата не прошла этапы АОИ и ТНТ Старт
        Else
            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", PCBCheckRes(1))
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
            CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Error", "Плата не зарегистрирована в базе!")
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
    Private Function GetStepResult() As ArrayList
        'продолжить сдесь, добавить arraylist для месседж
        Dim Mess As New ArrayList()
        ' В аргументах PCBID = PCBCheckRes(1) и PCBSN = PCBCheckRes(2), CurrentStepID = PCInfo(6) и CurrentStep = PCInfo(7)
        Dim PCBStepRes As New ArrayList(SelectListString("USE FAS SELECT [StepID],[TestResult],[ScanDate],[SNID]
                            FROM [FAS].[dbo].[Ct_StepResult] where [PCBID] = " & PCBCheckRes(1)))

        If PCBStepRes.Count = 0 Then
            UpdateStepRes(PCInfo(6), 4, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            'Если плата в таблице StepResult имеет шаг совпадающий с текущей станцией и результат равен 2
        ElseIf PCBStepRes(0) = PCInfo(6) And PCBStepRes(1) = 2 Then 'Плата имеет статус 1/2
            PassAction(2)
            'Если плата в таблице StepResult имеет  результат равен 3
        ElseIf PCBStepRes(1) = 3 Then 'Плата имеет статус x/3, то проверить опер лог и определить откуда плата
            'Mess.AddRange(New ArrayList() From {"Карантин", "Плата " & PCBCheckRes(2) & " находится в карантине!" &
            '               vbCrLf & "Проверьте информацию о плате передайте ее в ремонт!", Color.Red, False})
            UpdateStepRes(PCInfo(6), 6, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            'Если плата в таблице StepResult имеет шаг совпадающий с предыдущей станцией и результат равен 2
        ElseIf PCBStepRes(0) = PreStepID And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2 (проверка предыдущего шага)
            PassAction(1)
            'Если плата в таблице StepResult имеет шаг совпадающий со станцией ремонта, результат равен 2 и 
            'номер текущей станции совпадает со стартовой станцией
        ElseIf PCBStepRes(0) = 4 And PCBStepRes(1) = 2 And PCInfo(6) = 25 Then 'Плата вернулась из ремонта на первый этап
            PassAction(1)
        ElseIf PCBStepRes(0) <> PreStepID And PCBStepRes(1) = 2 Then 'Плата имеет статус Prestep/2
            'Проверить опер лог и изменить коментарий
            UpdateStepRes(PCInfo(6), 5, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        Else
            UpdateStepRes(PCInfo(6), 5, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        End If
        'функция возвращает ArrayList со значениями для запись в лог
        Return Mess
    End Function
    'функция печати

    Private Sub PassAction(i As Integer)
        If PrintSN(i) = True Then
            UpdateStepRes(PCInfo(6), 2, PCBCheckRes(1))
            SerialTextBox.Clear()
        Else
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        End If
    End Sub

    Dim SNArray As New ArrayList
    Private Function PrintSN(i As Integer)
        Dim res As Boolean
        SNArray = SelectListString("use fas
        SELECT [ID],[SN],[IMEI],[MAC_BT],[MAC_WF],[IsPrinted],[PrintByID],[PrintDate]
        ,[IsRePrinted],[RePrintByID],[RePrintDate],[RePrintCount]
        FROM [FAS].[dbo].[CT_Aquarius] where [LOTID] = 20063 and [SN] = '" & SerialTextBox.Text & "'") 'INS220020101
        If SNArray.Count <> 0 Then
            AquaSN(SNArray)
            PrintLabel(Controllabel, "Серийный номер " & SNArray(1) & vbCrLf &
                        "MAC WiFi " & SNArray(4) & vbCrLf &
                        "IMEI " & SNArray(2) & vbCrLf &
                        "MAC BT " & SNArray(3), 12, 192, Color.Green)
            Dim sql = If(SNArray(5) = False, "Update [FAS].[dbo].[CT_Aquarius] set IsPrinted = 1,PrintByID = " & UserInfo(0) & ", 
                 PrintDate = CURRENT_TIMESTAMP where id = " & SNArray(0),
                "Update [FAS].[dbo].[CT_Aquarius] set IsRePrinted = 1, RePrintByID = " & UserInfo(0) & ",RePrintDate = CURRENT_TIMESTAMP,
                 RePrintCount = " & SNArray(11) + 1 & " where id = " & SNArray(0))
            RunCommand(sql)
            res = True
        Else
            PrintLabel(Controllabel, "Номер не найден в базе!", 12, 192, Color.Red)
            res = False
        End If
        Return Res
    End Function

    'функция обноления результата тестирования для Pass/Fail
    Private Sub UpdateStepRes(StepID As Integer, StepRes As Integer, PcbID As Integer)
        Dim Message As String
        Dim MesColor As Color
        Dim ErrCode As New ArrayList()
        Select Case StepRes
            Case 2
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Успех", "Плата прошла этап " & PCInfo(7) & "!")
                ShiftCounter(2)
            Case 4
                Message = "Плата " & PCBCheckRes(2) & " не прошла этап тестирования!" & vbCrLf & "Передайте плату на предыдущий этап!"
                MesColor = Color.Red
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "не прошла этап тестирования!" &
               vbCrLf & "Передайте плату на предыдущий этап!")
                PrintLabel(Controllabel, Message, 12, 193, MesColor)
                Exit Sub
            Case 5
                Message = "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! " & vbCrLf &
                           "Перейдите во вкладку ИНФО и опредилите принадлежность платы!"
                MesColor = Color.Red
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Ошибка", "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг!")
                PrintLabel(Controllabel, Message, 12, 193, MesColor)
                Exit Sub
            Case 6
                Message = "Плата " & PCBCheckRes(2) & " находится в карантине!" &
                           vbCrLf & "Проверьте информацию о плате передайте ее в ремонт!"
                MesColor = Color.Red
                CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Карантин", "Плата " & PCBCheckRes(2) & " находится в карантине!")
                PrintLabel(Controllabel, Message, 12, 193, MesColor)
                Exit Sub
        End Select
        RunCommand("USE FAS Update [FAS].[dbo].[Ct_StepResult] 
                    set StepID = " & StepID & ", TestResult = " & StepRes & ", ScanDate = CURRENT_TIMESTAMP
                    where PCBID = " & PcbID)
        RunCommand("insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[ErrorCodeID],[Descriptions])values
                    (" & PcbID & "," & LOTID & "," & StepID & "," & StepRes & ",CURRENT_TIMESTAMP,
                    " & UserInfo(0) & "," & PCInfo(2) & "," &
                    If(StepRes = 3, ErrCode(0), "Null") & "," &
                    If(StepRes = 3, If(TB_Description.Text = "", "Null", "'" & TB_Description.Text & "'"), "Null") & ")")
    End Sub
    'Кнопка очистки поля ввода номера
    Private Sub BT_CleareSN_Click(sender As Object, e As EventArgs) Handles BT_CleareSN.Click
        If GB_PCBInfoMode.Visible = False Then
            SerialTextBox.Clear()
            SerialTextBox.Enabled = True
            DG_UpLog.Visible = True
            TB_Description.Clear()
            SerialTextBox.Focus()
        Else
            TB_GetPCPInfo.Clear()
            TB_GetPCPInfo.Enabled = True
            TB_GetPCPInfo.Focus()
        End If
    End Sub
    'Функция заполнения LogGrid 
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN As String, ScanRes As String, Descr As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN, ScanRes, Date.Now, Descr)
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
            LB_LOTCounter.Text = ShiftCounterInfo(2)
        ShiftCounterUpdateCT(PCInfo(4), PCInfo(0), ShiftCounterInfo(0), ShiftCounterInfo(1), ShiftCounterInfo(2),
                             ShiftCounterInfo(3), ShiftCounterInfo(4))
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


    Public Function AquaSN(SN As ArrayList)
        'INS2> 520020330
        '35639911074697>67
        '0058>63F>5048752
        '0058>63F048D51
        Dim Content As String = "
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,15^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW685
^LL0354
^LS0
^FT183,64^A0N,42,40^FH\^FDSN^FS
^FT154,215^A0N,42,40^FH\^FDIMEI^FS
^FT95,139^A0N,42,40^FH\^FDBT Addr^FS
^FT62,290^A0N,42,40^FH\^FDWiFi Addr^FS

^BY2,3,47^FT255,62^BCN,,Y,N
^FD>:" & Mid(SN(1), 1, 4) & ">5" & Mid(SN(1), 5) & "^FS
^BY2,3,47^FT255,213^BCN,,Y,N
^FD>;" & Mid(SN(2), 1, 14) & ">6" & Mid(SN(2), 15) & "^FS
^BY2,3,47^FT255,137^BCN,,Y,N
^FD>;" & Mid(SN(3), 1, 4) & ">6" & Mid(SN(3), 5) & "^FS
^FO44,234^GB597,78,3^FS
^FO44,158^GB597,78,3^FS
^FO44,82^GB597,78,3^FS
^FO44,7^GB597,78,3^FS
^BY2,3,47^FT255,288^BCN,,Y,N
^FD>;" & Mid(SN(4), 1, 4) & ">6" & Mid(SN(4), 5) & "^FS
^PQ1,0,1,Y^XZ
"
        PrintSerialPort.Open()
        PrintSerialPort.Write(Content) 'ответ в COM порт
        PrintSerialPort.Close()
    End Function


End Class
