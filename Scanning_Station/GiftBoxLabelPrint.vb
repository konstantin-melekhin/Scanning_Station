Imports Library3
Imports System.Deployment.Application
Imports System.Drawing.Printing
Imports System.IO


Public Class GiftBoxLabelPrint
    Dim LOTID, IDApp As Integer
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim Coordinats, LOTInfo As ArrayList 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim SNFormat As ArrayList
#Region "Загрузка рабочей формы"
    Public Sub New(LOTIDWF As Integer, IDApp As Integer)
        InitializeComponent()
        Me.LOTID = LOTIDWF
        Me.IDApp = IDApp
    End Sub
    Private Sub GiftBoxLabelPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myVersion As Version
        If ApplicationDeployment.IsNetworkDeployed Then
            myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion
        End If
        LB_SW_Wers.Text = String.Concat("v", myVersion)
#Region "Обнаружение принтеров и установка дефолтного принтера"
        For Each item In PrinterSettings.InstalledPrinters
            If InStr(item.ToString(), "ZDesigner") Or InStr(item.ToString(), "TSC TX600") Then
                CB_DefaultPrinter.Items.Add(item.ToString())
            End If
        Next
        If CB_DefaultPrinter.Items.Count <> 0 Then
            CB_DefaultPrinter.Text = CB_DefaultPrinter.Items(0)
        Else
            PrintLabel(Controllabel, "Ни один принтер не подключен!", 32, 564, Color.Red)
        End If
        GetCoordinats()
        PrintLabel(Controllabel, "", 32, 564, Color.Red)
#End Region
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
                        "LenSN = " & LenSN & vbCrLf &
                        "SNBot_SNTop_SNFas = " & LOTInfo(19) & vbCrLf        'LOTInfo
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
        'Запуск программы
        '___________________________________________________________
        GB_UserData.Location = New Point(10, 12)
        L_UserName.Text = ""
        GB_UserData.Visible = True
        GB_WorkAria.Visible = False
        SerialTextBox.Focus()
        'запуск счетчика продукции за день
        CurrentTimeTimer.Start()
        ShiftCounterInfo = ShiftCounterStart(PCInfo(4), IDApp, LOTID)
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_LOTCounter.Text = ShiftCounterInfo(2)
    End Sub
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
                SerialTextBox.Focus()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            TB_RFIDIn.Clear()
        End If
    End Sub 'регистрация пользователя
#End Region
#Region "Часы в программе"
    Private Sub CurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles CurrentTimeTimer.Tick
        CurrrentTimeLabel.Text = TimeString
    End Sub 'Часы в программе
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
        If e.KeyCode = Keys.Enter Then
            GetFTSN()
            If SNFormat(0) = True Then
                Dim dataToPrint As New ArrayList(CheckstepResult(GetPreStep(SNFormat(4))))
                If dataToPrint(7) = True Then
                    Print(GetLabelContent(dataToPrint(5), 0, 0, 1))
                    WriteToDB(dataToPrint)
                    SerialTextBox.Clear()
                End If
            End If
        End If
    End Sub
#End Region
#Region "очистка Серийного номера при ошибке"
    Private Sub BT_ClearSN_Click(sender As Object, e As EventArgs) Handles BT_ClearSN.Click
        SerialTextBox.Clear()
        Controllabel.Text = ""
        SerialTextBox.Enabled = True
        SerialTextBox.Focus()
    End Sub
#End Region
#Region "1. Определение формата номера"
    Public Sub GetFTSN()
        Dim SNID As Integer
        SNFormat = New ArrayList()
        SNFormat = GetScanSNFormat(LOTInfo(19).Split(";")(0), LOTInfo(19).Split(";")(1), LOTInfo(19).Split(";")(2), SerialTextBox.Text, PCInfo(6))
        SNID = If(SNFormat(1) = 1 Or SNFormat(1) = 2,
                SelectInt($"use SMDCOMPONETS select IDLaser  FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = '{SerialTextBox.Text}'"),
                SelectInt($"USE FAS Select [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{SerialTextBox.Text}'"))
        If SNID > 0 Then
            SNFormat.Add(SNID)

        Else
            SNFormat(0) = False
            PrintLabel(Controllabel, "Формат номера не определен!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
        End If
    End Sub
#End Region
#Region "2. Проверка предыдущего шага и загрузка данных о плате"
    Private Function GetPreStep(_snid As Integer) As ArrayList
        Dim newArr As ArrayList
        Select Case SNFormat(1)
            Case 1
                newArr = New ArrayList(SelectListString($"Use FAS select
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  PCBID  = {_snid}) tt
            where  tt.num = 1 "))
            Case 2
                newArr = New ArrayList(SelectListString($"Use FAS select 
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  PCBID  = {_snid}) tt
            where  tt.num = 1 "))
            Case 3
                newArr = New ArrayList(SelectListString($"Use FAS select
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by snid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  SNID  = {_snid}) tt
            where  tt.num = 1 "))
        End Select
        Return newArr
    End Function
#End Region
#Region "3. Запись в базу"
    Private Sub WriteToDB(snBufer As ArrayList)
        RunCommand($"use fas         
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID], SNID)values
          ({snBufer(2)},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{snBufer(4)})")
        PrintLabel(Controllabel, $"Номер {snBufer(5)} был отправлен на печать!", 12, 193, Color.Green)
        CurrentLogUpdate(ShiftCounter(), snBufer(5), snBufer(3))
    End Sub
#End Region
#Region "4. Счетчик продукции"
    Private Function ShiftCounter() As Integer
        ShiftCounterInfo(1) += 1
        ShiftCounterInfo(2) += 1
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_LOTCounter.Text = ShiftCounterInfo(2)
        ShiftCounterUpdateCT(PCInfo(4), PCInfo(0), ShiftCounterInfo(0), ShiftCounterInfo(1), ShiftCounterInfo(2))
        Return ShiftCounterInfo(1)
    End Function
#End Region
#Region "8. Функция запролнения LogGrid "
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN1 As String, SN2 As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN1, SN2, Date.Now)
        DG_UpLog.Sort(DG_UpLog.Columns(3), System.ComponentModel.ListSortDirection.Descending)
    End Sub
#End Region
#Region "Функция печати на этикетке"
    Private Sub Print(ByVal content As String)
        If CB_DefaultPrinter.Text <> "" Then
            RawPrinterHelper.SendStringToPrinter(CB_DefaultPrinter.Text, content)
        Else
            MsgBox("Принтер не выбран или не подключен")
        End If
    End Sub
#End Region
#Region "Определение и сохранение координат"
    Private Sub GetCoordinats()
        Coordinats = New ArrayList
        Try
            For Each item In File.ReadAllLines("C:\Conract_LabelSet\Coordinats.csv")
                Coordinats.Add(item.Split(";")(0))
                Coordinats.Add(item.Split(";")(1))
            Next
            Num_X.Value = Coordinats(0)
            Num_Y.Value = Coordinats(1)
        Catch ex As Exception
            Dim PrinterInfo() As String = New String(0) {$"0;0"}
            IO.Directory.CreateDirectory("C:\Conract_LabelSet\")
            File.Create("C:\Conract_LabelSet\Coordinats.csv").Close()
            File.WriteAllLines("C:\Conract_LabelSet\Coordinats.csv", PrinterInfo)
            GetCoordinats()
        End Try
    End Sub
    Private Sub BT_Save_Coordinats_Click(sender As Object, e As EventArgs) Handles BT_Save_Coordinats.Click
        File.WriteAllText("C:\Conract_LabelSet\Coordinats.csv", $"{Num_X.Value};{Num_Y.Value}")
        GetCoordinats()
    End Sub
#End Region
#Region "Функция определения результата этапа"
    Private Function CheckstepResult(prestep As ArrayList) As ArrayList
        If prestep.Count = 0 And StartStepID <> PCInfo(6) Then ' шаг не первый, но предыдущего результата нет
            'Mess.AddRange(New ArrayList() From {"Ошибка", $"Плата {PCBCheckRes(2)} не прошла этап {StartStep}!
            '              {vbCrLf} Передайте плату на этап {StartStep}!", Color.Red, False})
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        ElseIf prestep(0) = PCInfo(6) And prestep(1) = 2 Then 'Плата имеет статус ("текущий шаг"/2)
            PrintLabel(LB_CurrentErrCode, "Плата " & prestep(2) & " уже прошла этап " & PCInfo(7) & "!" &
                           vbCrLf & "Передайте плату на следующий этап " & NextStep & " или измените результат!", 12, 270, Color.Red)
            'RepeatStep = True
            'SelectAction()
            'Если плата в таблице StepResult имеет  результат равен 3
        ElseIf prestep(0) = PCInfo(6) And prestep(1) = 3 Then 'Плата имеет статус ("текущий шаг"/3)
            PrintLabel(LB_CurrentErrCode, "Плата уже в карантине. Передайте плату в ремонт или обновите статус!", 12, 270, Color.Red)
            'RepeatStep = True
            'SelectAction()
            'Если плата в таблице OperLog имеет шаг совпадающий с предыдущей станцией и результат равен 2
        ElseIf prestep(0) = PreStepID And prestep(1) = 2 Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)
            prestep.Add(True)
            Return prestep
            'Если плата в таблице OperLog имеет шаг совпадающий со станцией ОТК, результат равен 2 
        ElseIf prestep(0) = 40 And prestep(1) = 2 Then 'Плата вернулась из ремонта 
            'RepeatStep = True
            'SelectAction()
        ElseIf prestep(0) = 41 And prestep(1) = 2 Then 'Повторная проверка эталона
            'RepeatStep = True
            'SelectAction()
            'Если плата в таблице OperLog имеет шаг совпадающий со станцией ОТК, результат равен 3
        ElseIf prestep(0) = 40 And prestep(1) = 3 Then
            PrintLabel(Controllabel, "Плата " & prestep(2) & " пришла из ремонта." & vbCrLf &
                           "Плата не отремонтирована! Поместите плату в карантин!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            'Если плата в таблице StepResult имеет шаг не совпадающий с предыдущей станцией и результат равен 2
        Else
            Dim sender As Object, e As EventArgs
            'BT_PCBInfo_Click(sender, e)
            TB_GetPCPInfo.Text = prestep(2)
            'GetLogInfo()
            'Mess.AddRange(New ArrayList() From {"Ошибка", "Плата " & PCBCheckRes(2) & " имеет не верный предыдыдущий шаг! ", Color.Red, False})
            'UpdateStepRes(PCInfo(6), 5, PCBCheckRes(1))
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
        End If

    End Function
#End Region

    Private Function GetLabelContent(sn As String, x As Integer, y As Integer,w As Integer) As String
        If w = 1 Then
            Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW1110
^LL0461
^LS0
^FO736,288^GFA,01152,01152,00012,:Z64:
eJy9k7Fq3EAQhmdPbGWwVEiQIhfyBLHTifiQFEh6GU6dAnmEC+Qg5QY36fIKVwY3aY8Y7A0kvV/gQODG4MIpRQy3+Wd3tKnTZHRaDZ/+mZ2Z1RH9B/tkoqu6ZfSvnNtMeN2tV+Kn+y9f9xIxb4jeNMG/xH0wBL/lGF62lHg0EtklKct+j2dFJw8XsKtNagoqkB5XU1BqaudtqKBSP17BekTMxsRvWc6QqVV+k0xh58uzwFM85iF/NucqhVdcyUtf4vF7LLM7m7o9lSOTzurdjk59VG0PLr5R7TsuvL4L7fr6Q8Na6meLffEGq6lflnLQbfB3HCT8HikqmQ8XtBR+A+0o3LboWuaJCSQb4ZiMprfCU1PRxLXtadInwzjxlk7byMkNfzlmY4P/i2oz8RV1jRL9SA+3Ua+uWyX642R4EbmmPOork36ceE/6u+gx4uRG+PMVqXeiPwFzouGTmovPJ8WnyJbj1rGeadikrqcFyPPf3tcZ89z7z7bMP/tE/WPm4dTGkvkZJ1ItS5U/sGRY8Go4m7asL4jX2rDemSPD1eT8F2i0rwZv07sBXzoKga95SFzgIeE34tK+/pwy3GgI/ElZ0pGBmH+LR9gIyQ/Pz7dViUiUobv1cpGRap4iuXP7Bd5zB/rn657kO0k41YfgY/Y6csv1CLekdjbqZ07mqWSm/2x/AHlCscE=:A30D
^FO608,288^GFA,01152,01152,00012,:Z64:
eJy1k7FLw0AYxd+ZhAZbkg4RFTo4OzgHKW0KgmuGBpf+MYeO/gsOHUv9B5zkHNwdXYSgg6UF56LQ+uXuvksGlw6+cumP1+uXdy8E+H+FDfYbPJQ1X2U1r0qHQinHnmz4yB3HMnUcoOc4wrnjhD6sPv3C6tE/WCla18w5hGBewLvlpBnEM/uU5pPjUPpLFxP4cnHQndT+8LH2D8aW94H2BrUmDW42x5pCsi1egowLHa6H6wFMziLv5uMqE+lnHaZfVVbS6BXHHRjfe0AaWb+VoOvbRuO7yjeN+j3kHdtoRJsjDDQnvqD7mkb7YVtGttEjOljHNpoinkW6jKrNw9yHsK2dzkLrK3OK6kuYJDqQTa6D6o10WZjW9CVnP4YO2tKZqGr2KeWaB+d2/3I+l9/zez2/KIpq6dtst1tZLa6XH4s5Q62kwZGsOWj4XsMX2EFjGl9Y3soBOM6TSjBShsvyjJbhLLugZViVS+erbOT8j+mNfLd+gZGbv5ErubHzlXhTdjtKb+5esEy4MVD03rGvgtqXeziZ7nLIP/ULmr1ttQ==:04F1
^FO448,288^GFA,01536,01536,00016,:Z64:
eJztlDEOgzAMRS1loBtjR67SS3TuFbqxICUj5+jIJcoReoN07sSAlA5V3BgnJQi1BQmJDrVA1s/Xk2XZCcDasSlL+ktwH0WiNRy0vlCmEIggEe+UO591E2mF7KvYT7w/4o2B3JhGuEyRuvMRX2dDvs56Xu5B7rKjcLnTVgGSbxXrBzD/4P6EBeYt66QFyOttI1rWqTvH2P/Ge194P5nCXxesXwD3X8yov2b/aHn+wQ/zD/XD/IMf9ufF45CXTkvs+w/79463J5C383/+P9t/5fmqn9/H+4/+/kfvD/tqEk/7R/Xj92cGv2w8Aemmr6s=:249B
^FO32,64^GFA,25344,25344,00132,:Z64:
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:944B
^BY2,3,84^FT36,379^BCN,,Y,N
^FD>:{Mid(sn, 1, 10)}>5{Mid(sn, 11)}^FS
^BY2,2,103^FT872,387^BEN,,Y,N
^FD4640076240212^FS
^FT268,204^A0N,25,21^FH\^FD10.2021.^FS
^PQ1,0,1,Y^XZ"
        Else
            Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW1110
^LL0461
^LS0
^FO736,288^GFA,01152,01152,00012,:Z64:
eJy9k7Fq3EAQhmdPbGWwVEiQIhfyBLHTifiQFEh6GU6dAnmEC+Qg5QY36fIKVwY3aY8Y7A0kvV/gQODG4MIpRQy3+Wd3tKnTZHRaDZ/+mZ2Z1RH9B/tkoqu6ZfSvnNtMeN2tV+Kn+y9f9xIxb4jeNMG/xH0wBL/lGF62lHg0EtklKct+j2dFJw8XsKtNagoqkB5XU1BqaudtqKBSP17BekTMxsRvWc6QqVV+k0xh58uzwFM85iF/NucqhVdcyUtf4vF7LLM7m7o9lSOTzurdjk59VG0PLr5R7TsuvL4L7fr6Q8Na6meLffEGq6lflnLQbfB3HCT8HikqmQ8XtBR+A+0o3LboWuaJCSQb4ZiMprfCU1PRxLXtadInwzjxlk7byMkNfzlmY4P/i2oz8RV1jRL9SA+3Ua+uWyX642R4EbmmPOork36ceE/6u+gx4uRG+PMVqXeiPwFzouGTmovPJ8WnyJbj1rGeadikrqcFyPPf3tcZ89z7z7bMP/tE/WPm4dTGkvkZJ1ItS5U/sGRY8Go4m7asL4jX2rDemSPD1eT8F2i0rwZv07sBXzoKga95SFzgIeE34tK+/pwy3GgI/ElZ0pGBmH+LR9gIyQ/Pz7dViUiUobv1cpGRap4iuXP7Bd5zB/rn657kO0k41YfgY/Y6csv1CLekdjbqZ07mqWSm/2x/AHlCscE=:A30D
^FO608,288^GFA,01152,01152,00012,:Z64:
eJy1k7FLw0AYxd+ZhAZbkg4RFTo4OzgHKW0KgmuGBpf+MYeO/gsOHUv9B5zkHNwdXYSgg6UF56LQ+uXuvksGlw6+cumP1+uXdy8E+H+FDfYbPJQ1X2U1r0qHQinHnmz4yB3HMnUcoOc4wrnjhD6sPv3C6tE/WCla18w5hGBewLvlpBnEM/uU5pPjUPpLFxP4cnHQndT+8LH2D8aW94H2BrUmDW42x5pCsi1egowLHa6H6wFMziLv5uMqE+lnHaZfVVbS6BXHHRjfe0AaWb+VoOvbRuO7yjeN+j3kHdtoRJsjDDQnvqD7mkb7YVtGttEjOljHNpoinkW6jKrNw9yHsK2dzkLrK3OK6kuYJDqQTa6D6o10WZjW9CVnP4YO2tKZqGr2KeWaB+d2/3I+l9/zez2/KIpq6dtst1tZLa6XH4s5Q62kwZGsOWj4XsMX2EFjGl9Y3soBOM6TSjBShsvyjJbhLLugZViVS+erbOT8j+mNfLd+gZGbv5ErubHzlXhTdjtKb+5esEy4MVD03rGvgtqXeziZ7nLIP/ULmr1ttQ==:04F1
^FO448,288^GFA,01536,01536,00016,:Z64:
eJztlDEOgzAMRS1loBtjR67SS3TuFbqxICUj5+jIJcoReoN07sSAlA5V3BgnJQi1BQmJDrVA1s/Xk2XZCcDasSlL+ktwH0WiNRy0vlCmEIggEe+UO591E2mF7KvYT7w/4o2B3JhGuEyRuvMRX2dDvs56Xu5B7rKjcLnTVgGSbxXrBzD/4P6EBeYt66QFyOttI1rWqTvH2P/Ge194P5nCXxesXwD3X8yov2b/aHn+wQ/zD/XD/IMf9ufF45CXTkvs+w/79463J5C383/+P9t/5fmqn9/H+4/+/kfvD/tqEk/7R/Xj92cGv2w8Aemmr6s=:249B
^FO32,64^GFA,25344,25344,00132,:Z64:
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:944B
^BY2,3,80^FT36,375^BCN,,N,N
^FD>:{Mid(sn, 1, 10)}>5{Mid(sn, 11)}^FS
^BY2,2,103^FT872,387^BEN,,Y,N
^FD4640076240212^FS
^FT268,204^A0N,25,21^FH\^FD10.2021.^FS
^FT147,394^A0N,19,19^FH\^FD{sn}^FS
^PQ1,0,1,Y^XZ"
        End If
    End Function
End Class