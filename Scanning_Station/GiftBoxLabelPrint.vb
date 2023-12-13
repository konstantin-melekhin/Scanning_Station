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
            If InStr(item.ToString(), "ZDesigner") Or InStr(item.ToString(), "Zebra ZT410") Or InStr(item.ToString(), "TSC") Then
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
        RB_NewBox.Checked = True
#End Region
        LB_CurrentErrCode.Text = ""
        CB_CheckID.Checked = True
        CB_Deception.Checked = False
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
        'загружаем список кодов ошибок в грид SQL запрос "ErrorCodeList" 
        LoadGridFromDB(DG_ErrorCodes, "use FAS select [ErrorCodeID],[ErrorCode],[Description]  FROM [FAS].[dbo].[FAS_ErrorCode] where [ErrGroup] = 5")
        'Записываем коды ошибок в рабочий комбобокс
        If DG_ErrorCodes.Rows.Count <> 0 Then
            For J = 0 To DG_ErrorCodes.Rows.Count - 1
                CB_ErrorCode.Items.Add(DG_ErrorCodes.Rows(J).Cells(1).Value)
            Next
        End If
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
    'начало работы приложения FAS Scanning Station SBB16W05AH037958
#Region "Окно ввода серийного номера платы"
    Private Sub SerialTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SerialTextBox.KeyDown
        LB_CurrentErrCode.Text = ""
        Controllabel.Text = ""
        If e.KeyCode = Keys.Enter Then
            GetFTSN()
            If SNFormat(0) = True Then
                If CB_CheckID.Checked = True And LOTInfo(20) = 34 Then
                    If CheckSberID(SerialTextBox.Text) <> SerialTextBox.Text Then
                        PrintLabel(Controllabel, "Номер не прошит в устройство! Вернуть на перепрошивку", 12, 193, Color.Red)
                        SerialTextBox.Enabled = False
                        Exit Sub
                    End If
                End If
                Dim dataToPrint As New ArrayList(CheckstepResult(GetPreStep(SNFormat(3))))
                If dataToPrint(7) = True Then
                    If LOTInfo(20) = 34 Then
                        Print(GetLabelContent(dataToPrint(5), 0, 0, If(RB_NewBox.Checked = True, 3, 2)))
                    ElseIf LOTInfo(20) = 44 Then
                        If LOTInfo(0) = "T1100" Then
                            Print(GetLabelContent_TabletT1100(dataToPrint(5), 0, 0))
                            'Print(GetLabelContent_SmartPhone(dataToPrint(5), 0, 0))
                        ElseIf LOTInfo(0) = "T800" Then
                            Print(GetLabelContent_TabletT800(dataToPrint(5), 0, 0))
                        ElseIf LOTInfo(0) = "f+ R570E" Then
                            Print(GetLabelContent_SmartPhone(dataToPrint(5), 0, 0))
                        End If
                    ElseIf LOTInfo(20) = 50 Then
                        Print(GetLabelContent_SmartPhone_Y(dataToPrint(5), 0, 0))
                    End If

                    WriteToDB(dataToPrint)
                    SerialTextBox.Clear()
                    End If
                End If
        End If
    End Sub
#End Region
    '#Region "Обределение формата этикетки "
    '    Dim LabelScenario As Integer
    '    Private Sub RB_NewBox_CheckedChanged(sender As Object, e As EventArgs) Handles RB_NewBox.CheckedChanged
    '        If RB_NewBox.Checked = True Then
    '            LabelScenario = 3
    '        End If
    '    End Sub
    '    Private Sub RB_OldBox_CheckedChanged(sender As Object, e As EventArgs) Handles RB_OldBox.CheckedChanged
    '        If RB_OldBox.Checked = True Then
    '            LabelScenario = 2
    '        End If
    '    End Sub
    '#End Region
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
        SNFormat = New ArrayList()
        SNFormat = GetSTBSNFormat(LOTInfo(19).Split(";")(2), SerialTextBox.Text)
        If SNFormat(0) = False Then
            SNFormat = GetSTBSNFormat(LOTInfo(8), SerialTextBox.Text)
        End If
        If SNFormat(0) = False Then
            PrintLabel(Controllabel, "Формат номера не определен!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
        End If
        If LOTID = 20469 Then
            SNFormat(3) = SelectInt($"select id FROM [FAS].[dbo].[Ct_FASSN_reg] 
                        where sn = (select top (1) sn  FROM [FAS].[dbo].[CT_Aquarius] 
                        where IMEI = '{SerialTextBox.Text}' or IMEI2 = '{SerialTextBox.Text}')")
        End If
    End Sub
#End Region
#Region "2. Проверка предыдущего шага и загрузка данных о плате"
    Private Function GetPreStep(_snid As Integer) As ArrayList
        Dim newArr As New ArrayList(SelectListString($"Use FAS select 
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by SNid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  SNID  = {_snid}) tt
            where  tt.num = 1 "))
        If newArr.Count = 0 And CB_Deception.Checked = True Then
            RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],Descriptions, SNID)values
        (8830503,{LOTID},30,2,CURRENT_TIMESTAMP,11,19,'Исправление ошибки в программе(пропущен пр.шаг)',{_snid})")
            newArr = SelectListString($"Use FAS select 
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  SNID  = {_snid}) tt
            where  tt.num = 1 ")
        End If
        Return newArr
    End Function
#End Region
#Region "3. Запись в базу"
    Private Sub WriteToDB(snBufer As ArrayList)
        RunCommand($"use fas         
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID], SNID)values
          ({snBufer(2)},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{snBufer(4)})")
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
            PrintLabel(Controllabel, $"Номер {SerialTextBox.Text} не был на связке серийных номеров (PCB_STB)!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            For i = 0 To 7
                prestep.Add(False)
            Next
            BT_Pause.Focus()
            Return prestep
        ElseIf (prestep(0) = PCInfo(6) Or prestep(0) = 37 Or prestep(0) = 6) And prestep(1) = 2 Then 'Плата имеет статус ("текущий шаг"/2)
            If MsgBox("Повторить печать этого номера?", MessageBoxButtons.YesNo) = 6 Then
                PrintLabel(Controllabel, $"Номер {prestep(5)} повторно отправлен на печать!", 12, 193, Color.Green)
                prestep.Add(True)
                Return prestep
            Else
                prestep.Add(False)
                PrintLabel(Controllabel, $"Печать номера {prestep(5)} отменена!", 12, 193, Color.DarkOrange)
                SerialTextBox.Clear()
                Return prestep
            End If
            'Если плата в таблице OperLog имеет шаг совпадающий с предыдущей станцией и результат равен 2
        ElseIf prestep(0) = 30 And prestep(1) = 2 Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)
            PrintLabel(Controllabel, $"Номер {prestep(5)}  отправлен на печать!", 12, 193, Color.Green)
            prestep.Add(True)
            Return prestep
        ElseIf prestep(0) = prestepid And prestep(1) = 2 And (LOTInfo(20) = 44 Or LOTInfo(20) = 50) Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)

            PrintLabel(Controllabel, $"Номер {prestep(5)}  отправлен на печать!", 12, 193, Color.Green)
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

        ElseIf prestep(0) = 44 And prestep(1) = 3 Then
            PrintLabel(Controllabel, "Сбербокс " & prestep(5) & " добавлен в карантин!" & vbCrLf &
                       "Переместите этот сбербокс в карантин!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            prestep.Add(False)
            Return prestep
            'Если плата в таблице StepResult имеет шаг не совпадающий с предыдущей станцией и результат равен 2



        Else
            Dim sender As Object, e As EventArgs
            'BT_PCBInfo_Click(sender, e)
            'TB_GetPCPInfo.Text = prestep(2)
            PrintLabel(Controllabel, "Устройство " & prestep(5) & " имеет не верный шаг!" & vbCrLf &
                       "Проверьте шаг в программе PCB_Information!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            prestep.Add(False)
            Return prestep
        End If
    End Function
#End Region
#Region "GetLabelContent"
    Private Function GetLabelContent(sn As String, x As Integer, y As Integer, w As Integer) As String
        If w = 2 Then
            Return $"
^XA
^RS,,,3,N,,,2
^RR3
^XZ
<xpml><page quantity='0' pitch='39.0 mm'></xpml>^XA
^SZ2^JMA
^MCY^PMN
^PW1110
~JSN
^JZY
^LH0,0^LRN
^XZ
<xpml></page></xpml><xpml><page quantity='1' pitch='39.0 mm'></xpml>~DGR:SSGFX000.GRF,54270,134,:Z64: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:CB53
^XA
^FT313,272
^CI28

^A@N,21,21,TT0003M_^FD{Format(Date.Now, "MM.yyyy")}^FS
^FO66,317
^BY2^BCN,77,N,N^FD>:{Mid(sn, 1, 10)}>5{Mid(sn, 11)}^FS
^FT111,422
^CI0
^AAN,27,15^FD{Mid(sn, 1)}^FS
^FO1,16
^XGR:SSGFX000.GRF,1,1^FS
^PQ1,0,1,Y
^XZ
<xpml></page></xpml>^XA
^IDR:SSGFX000.GRF^XZ
"
        ElseIf w = 3 Then
            Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW370
^LL0189
^LS0
^FO64,128^GFA,00896,00896,00028,:Z64:
eJzl0DFqwzAUBmAbQ72lFxDNNWIITaFH6QUUPHaQHhqUKR67iPQqBQ0edQAPfqDBq0CLCgJVlduxS6BQ6L9+8PO/V1X/N3Xi15m+1uQY7+tR6rA/oGNNvCPdKMJDsWGJrDaDD+lgI9ORkc7IgMVOFB/B8J4vbaOOWvXZzrvVhMMtzPwJtGw3R6Gs6ubzrnSKwSGDSdhiTKv0+j7J8LmlEReKHibpV7PFxJcpihY874tRq5B0XpbObA4X8Kd1C10IzbZuuU2zwwR28Dy1TWSG5BvstxmKz2BfNN+3lWM3ZJvtUjo39duPP/sN+0v5AF+VkL8=:3EC3
^BY1,3,77^FT112,101^BCN,,N,N
^FD>:{Mid(sn, 1, 10)}>5{Mid(sn, 11)}^FS
^FT103,128^A0N,25,24^FH\^FD{Mid(sn, 1)}^FS
^FT266,154^A0N,21,21^FH\^FD{Format(Date.Now, "MM.yyyy")}^FS
^PQ1,0,1,Y^XZ
"            '^A@N,21,21,TT0003M_^FD{Format(Date.Now, "MM.yyyy")}^FS
            '^A@N,21,21,TT0003M_^FD05.2022^FS
        End If
    End Function
#Region "Смартфон F+ R570"
    Private Function GetLabelContent_SmartPhone(sn As String, x As Integer, y As Integer) As String
        Dim snarr As ArrayList = SelectListString($"select top (1) sn,IMEI,IMEI2,format(printdate, 'ddMMyy') FROM [FAS].[dbo].[CT_Aquarius] where SN = '{sn}'")
        Return $"
        SIZE 69.5 mm, 30 mm
        GAP 2 mm, 0 mm
        DIRECTION 40,0
        REFERENCE 0,0
        OFFSET 0 mm
        SET PEEL OFF
        SET CUTTER OFF
        SET PARTIAL_CUTTER OFF
        SET TEAR ON
        CLS
        BITMAP 1137,501,58,176,1,яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБЃЃЃЃЃЃЃЃЃЃЃѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяябЃЃЃЃЃЃЃЃЃЃЃЃЃѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓЂяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяшяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяи8888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя88888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяуѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя8888888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю8888888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяыѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяуѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяясяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяаяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш888888888888888889яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяШ88888888888888<<8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяР?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяЃ?яЃ яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яЃ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яђ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэ?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь888888888888888?яё888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьp?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэ с?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓЃуяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓуяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэс?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьр?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888ш?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888?ш?яё888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьр?яђяяяяяяяяяяяа ?яяяяяр яяяяАяяяяяяяэяб?яЃЂ яяА яяяЂЂяяяяяЂЂЂяяяя ЂяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓяГяяѓѓѓѓЏЂЂџяяАЂ‡яяш ЂЂяяяяр ЂЂ‡яяьЂЂ‡яяяяяящѓѓѓѓѓѓѓѓѓѓѓѓ‡яѓяяѓѓѓѓЏЂ яяА яяа   яяяА   яяь  яяяяяяэя?яЃ  яяА яяА   яяя     яяь  яяяяяяь?ь?яђяяШяяяяь?яьяяяяяь888888888888ш8?яё888?яяИяю	яяш?яьяяяяяь888888888889яш8?яё888/  яяА яь     яяр     яю   ?яяяяяьяР?яђЂ яяА яш     яа     яю   ?яяяяяэяЃ?яЃЂЂџяяАЂ‡ярЂЂЂЂЂ?ярЂЂЂЂЂѓяюЂЂЂџяяяяящѓѓѓѓѓѓѓѓѓѓѓюѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яаЂЂЂЂЂџярЂЂЂЂЂЃяюЂЂЂЏяяяяящѓѓѓѓѓѓѓѓѓѓЃясѓѓяяѓѓѓѓЏЂ яяА яА     яр     яю   яяяяяэяб?яЃ  яяА яА     яш      яю   яяяяяья€?яђяяШя яь  ю яяяяяь8888888888?ю888?яё888?яяИя€ ьяью?ю<яяяяяь8888888889яь888?яё888/  яяА я  я  яь яр  ?яя  яяяяяьяр?яђЂ яяА я  яА яюяяш  яяЂ яяяяяэяяЃ?яЃЂЂџяяАЂ‡я ЂїяаЂЃяяяяьЂЂџяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡я ЂярЂЃяяяяюЂЂЏяяАЂ‡яяяяящѓѓѓѓѓѓѓЃяясѓѓѓѓяяѓѓѓѓЏЂ яяА я  яш  яяяяяя  яяА яяяяяэ яяБ?яЃ  яяА ю  яш  яяяяяяЂ яяа яяяяяьь    яяю?яђяяШюяяьяяяяяяяияяяяяь8?ю<<?яяяш88888?яё888?яяИюяяьяяяяя€яяияяяяяь88яяяяяю88888?яё888/  яяА ю  яяь  яяяяяА яяа яяяяяьяяяяяр?яђЂ яяА яѓѓяяю  яяяяяА яяа яяяяяэ яяяяы?яЃЂЂџяяАЂ‡яяяяяюЂЂяяяяяаЂѓяяаЂ‡яяяяящѓѓѓЃГягЃѓѓѓѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяюЂЂ?яяяяяаЂѓяяАЂ‡яяяяящѓѓѓѓЃЂѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏЂ яяА яяяяяя  ?яяяяяа яяа яяяяяэ 1?яЃ  яяА яяяяяю  ?яяяяяа яяа яяяяяьш?яђяяШяяяяяю?яяяяяряяияяяяяь888888888?ш8888?яё888?яяИяяяяяя?Ияяияяяяяь888888888ш8888?яё888/  яяА яяяяяя  ?А      яяа яяяяяьяр?яђЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃЂЂџяяАЂ‡яяяяяя ЂџАЂЂЂЂЂЂѓяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяя ЂџАЂЂЂЂЂЂѓяяАЂ‡яяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃ  яяА яяяяяя  ?А      яяа яяяяяьяш?яђяяШяяяяяя?Ияяияяяяяь888888888яш8888?яё888?яяИяяяяяя?Ияяияяяяяь888888888яш8888?яё888/  яяА яяяяяя  ?А      яяа яяяяяьяр?яђЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃЂЂџяяАЂ‡яяяяяя ЂїАЂЂ   ЂѓяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяя ЂїАЂѓГГАЂѓяяАЂ‡яяяяящѓѓѓѓѓѓѓЂЂярЂѓЃЃїяЃѓѓѓЏЂ яяА яяяяяя  ?А яяа яяа яяяяяэяяяяЃчяячЃ  яяА ю<<яяю  ?А яяа яяа яяяяяьяяяяђяяяяяяШю  яяю?ияяияяияяяяяь8888888яяяяё?яяяя888?яяИюяяю?ияяияяияяяяяь8888888яяяяё?яяяя888/Ђ яяА ю  яяь  а яяА яяа яяяяяьяяяяђяяяяЂ яяА ю  яяь  а яяА яяа яяяяяэяяяяЃяяяяЃЂЂЏяяАЂ‡ю ЂяяшЂЂяаЂ‡яяАЂ‡яяаЂ‡яяяяящѓѓѓѓѓѓѓџяяяяГѓяяяяѓѓѓЏЂЂЏяяАЂ‡я ЂяшЂЂяаЂѓяяЂЂЏяяАЂ‡яяяяящѓѓѓѓѓѓѓџяяяяѓѓяяяяѓѓѓЏЂ яяА я  яр  яа яяЂ яяа яяяяяэяяяяЃяяяяЃЂ яяЂ я  ?яр  яр яя  яяа яяяяяьяяяяђяяяяяяяяаяшяюяяияяяяяь8888888яяяяё?яяяя888?И	яьяяИ	яшяь?яяияяяяяь8888888яяяяё?яяяя888/А  яш  я  я  яш  ш  ?яяа яяяяяьяш?яњА  Ђ  яЂ ь  яш  А  яяа яяяяяэ  яр   ?яЂаЂЂЂ ЂЂ‡яАЂЂ ЂЂ‡яьЂЂЂ ЂЂяю  ЂЂяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏаЂЂЂЂЂЂ‡яАЂЂЂЂЂ‡яюЂЂЂЂЂЂяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏр      яа     яю     яю    яяяяэ яс?яЃр      яр     яя     яю    яяяяьяш?яђшяр?яяяюяяяяь888888888яш8888?яё888?ьHяшяя€яю	яяяяь888888888яш8888?яё888/ь    А яь     яяяА    яю    яяяяьяр?яђю   А яю    яяяа    яю    яяяяэ яс?яЃя ЂЂѓАЂ‡яяЂЂЂЂѓяяярЂЂЂЂяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏяЂЂЂ‡АЂ‡яяАЂЂЂ‡яяяшЂЂЂЂяяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏяа  А яяа   яяяя   яяю    яяяяэ яс?яЃяш  ?А яяш   яяяяЂ  яяю    яяяяь  ?яђяь яШяяю яяяяяш ?яяюяяяяь888888888888888?яё888?яя яИяяяш ?яяяяяю яяяяяияяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяШяяяяяяяяяяяяяяяяяяяияяяяяь888888888888888?яё888?яяяяяИяяяяяяяяяяяяяяяяяяяияяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяШяяяяяяяяяяяяяяяяяяяа яяяяяь888888888888888?яё888?яяяяяИяяяяяяяяяяяяяяяяяяяш8?яяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяьяђяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяэяЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяэѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‚яяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяэѓѓѓѓѓѓѓѓѓѓѓѓѓЃЃяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяэяяЃяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяШяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888?яяя8888яяяяяИяяяяяяяяяяяяяяяяяяяяяяяяяяяю8888888888888?яяя8888яяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяѓѓѓѓїяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяюѓѓѓѓїяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяё888888888888?яяь8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяё888888888888?яяш8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяРяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБяябяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓ‡яяГѓѓѓЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяясяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888888888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяГѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‚яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяябяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш88888888888888888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь88888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБ яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя????????????яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя
        CODEPAGE 1251
        TEXT 1598,410,""ROMAN.TTF"",180,1,6,""Торговая марка F+ tech ""
        TEXT 1598,333,""ROMAN.TTF"",180,1,6,""Цвет: черный""
        TEXT 752,669,""ROMAN.TTF"",180,1,8,""R570E ""
        TEXT 1062,543,""ROMAN.TTF"",180,1,6,""SN: ""
        TEXT 974,377,""ROMAN.TTF"",180,1,6,""EAN: ""
        TEXT 1618,208,""ROMAN.TTF"",180,1,6,""IMEI: ""
        BARCODE 964,596,""128M"",104,0,180,2,4,""!104{Mid(snarr(0), 1, 12)}!099{Mid(snarr(0), 13)}""
        TEXT 956,487,""ROMAN.TTF"",180,1,5,""{snarr(0)}""
        BARCODE 827,422,""EAN13"",98,0,180,3,6,""4630251760277""
        TEXT 815,319,""ROMAN.TTF"",180,1,4,""6""
        TEXT 794,319,""ROMAN.TTF"",180,1,4,""3""
        TEXT 772,319,""ROMAN.TTF"",180,1,4,""0""
        TEXT 751,319,""ROMAN.TTF"",180,1,4,""2""
        TEXT 730,319,""ROMAN.TTF"",180,1,4,""5""
        TEXT 709,319,""ROMAN.TTF"",180,1,4,""1""
        TEXT 677,319,""ROMAN.TTF"",180,1,4,""7""
        TEXT 656,319,""ROMAN.TTF"",180,1,4,""6""
        TEXT 634,319,""ROMAN.TTF"",180,1,4,""0""
        TEXT 613,319,""ROMAN.TTF"",180,1,4,""2""
        TEXT 592,319,""ROMAN.TTF"",180,1,4,""7""
        TEXT 571,319,""ROMAN.TTF"",180,1,4,""7""
        TEXT 854,319,""ROMAN.TTF"",180,1,4,""4""
        BARCODE 1475,239,""128M"",90,0,180,3,6,""!105{Mid(snarr(1), 1, 14)}!100{Mid(snarr(1), 15)}""
        TEXT 1388,145,""ROMAN.TTF"",180,1,5,""{snarr(1)}""
        BARCODE 944,251,""128M"",102,0,180,3,6,""!105{Mid(snarr(2), 1, 14)}!100{Mid(snarr(2), 15)}""
        TEXT 857,145,""ROMAN.TTF"",180,1,5,""{snarr(2)}""
        TEXT 1501,147,""ROMAN.TTF"",180,1,5,""IMEI1:""
        TEXT 969,147,""ROMAN.TTF"",180,1,5,""IMEI2:""
        BARCODE 338,237,""128M"",59,0,90,2,4,""!104{Mid(snarr(0), 1, 12)}!099{Mid(snarr(0), 13)}""
        TEXT 275,287,""ROMAN.TTF"",90,1,4,""{snarr(0)}""
        TEXT 380,241,""ROMAN.TTF"",90,1,5,""R570E ""
        BARCODE 235,257,""128M"",59,0,90,3,6,""!105{Mid(snarr(1), 1, 14)}!100{Mid(snarr(1), 15)}""
        TEXT 171,331,""ROMAN.TTF"",90,1,4,""{snarr(1)}""
        BARCODE 131,257,""128M"",59,0,90,3,6,""!105{Mid(snarr(2), 1, 14)}!100{Mid(snarr(2), 15)}""
        TEXT 67,331,""ROMAN.TTF"",90,1,4,""{snarr(2)}""
        TEXT 334,103,""ROMAN.TTF"",90,1,6,""SN: ""
        TEXT 230,103,""ROMAN.TTF"",90,1,6,""IMEI1: ""
        TEXT 126,103,""ROMAN.TTF"",90,1,6,""IMEI2: ""
        TEXT 845,93,""ROMAN.TTF"",180,1,5,""Сделано в России""
        PRINT 1,1
        "
        'для добавления даты на этикетку заменить строку
        'TEXT 1051,93,""ROMAN.TTF"",180,1,5,""Сделано в России    {snarr(3)}""
    End Function
#End Region
#Region "планшет Т1100"
    Private Function GetLabelContent_TabletT1100(sn As String, x As Integer, y As Integer) As String
        Dim snarr As ArrayList = SelectListString($"select top (1) sn,IMEI,IMEI2,format(printdate, 'ddMMyy') FROM [FAS].[dbo].[CT_Aquarius] where SN = '{sn}'")
        Return $"
    SIZE 69.10 mm, 29.9 mm
    GAP 2 mm, 0 mm
    DIRECTION 0,0
    REFERENCE 0,0
    OFFSET 0 mm
    SET PEEL OFF
    SET CUTTER OFF
    SET PARTIAL_CUTTER OFF
    SET TEAR ON
    CLS
    BITMAP 1139,494,58,176,1,яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБЃЃЃЃЃЃЃЃЃЃЃѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяябЃЃЃЃЃЃЃЃЃЃЃЃЃѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓЂяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяшяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяи8888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя88888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяуѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя8888888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю8888888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяыѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяуѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяясяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяаяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш888888888888888889яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяШ88888888888888<<8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяР?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяЃ?яЃ яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яЃ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яђ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэ?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь888888888888888?яё888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьp?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэ с?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓЃуяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓуяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэс?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьр?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888ш?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888?ш?яё888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьр?яђяяяяяяяяяяяа ?яяяяяр яяяяАяяяяяяяэяб?яЃЂ яяА яяяЂЂяяяяяЂЂЂяяяя ЂяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓяГяяѓѓѓѓЏЂЂџяяАЂ‡яяш ЂЂяяяяр ЂЂ‡яяьЂЂ‡яяяяяящѓѓѓѓѓѓѓѓѓѓѓѓ‡яѓяяѓѓѓѓЏЂ яяА яяа   яяяА   яяь  яяяяяяэя?яЃ  яяА яяА   яяя     яяь  яяяяяяь?ь?яђяяШяяяяь?яьяяяяяь888888888888ш8?яё888?яяИяю	яяш?яьяяяяяь888888888889яш8?яё888/  яяА яь     яяр     яю   ?яяяяяьяР?яђЂ яяА яш     яа     яю   ?яяяяяэяЃ?яЃЂЂџяяАЂ‡ярЂЂЂЂЂ?ярЂЂЂЂЂѓяюЂЂЂџяяяяящѓѓѓѓѓѓѓѓѓѓѓюѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яаЂЂЂЂЂџярЂЂЂЂЂЃяюЂЂЂЏяяяяящѓѓѓѓѓѓѓѓѓѓЃясѓѓяяѓѓѓѓЏЂ яяА яА     яр     яю   яяяяяэяб?яЃ  яяА яА     яш      яю   яяяяяья€?яђяяШя яь  ю яяяяяь8888888888?ю888?яё888?яяИя€ ьяью?ю<яяяяяь8888888889яь888?яё888/  яяА я  я  яь яр  ?яя  яяяяяьяр?яђЂ яяА я  яА яюяяш  яяЂ яяяяяэяяЃ?яЃЂЂџяяАЂ‡я ЂїяаЂЃяяяяьЂЂџяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡я ЂярЂЃяяяяюЂЂЏяяАЂ‡яяяяящѓѓѓѓѓѓѓЃяясѓѓѓѓяяѓѓѓѓЏЂ яяА я  яш  яяяяяя  яяА яяяяяэ яяБ?яЃ  яяА ю  яш  яяяяяяЂ яяа яяяяяьь    яяю?яђяяШюяяьяяяяяяяияяяяяь8?ю<<?яяяш88888?яё888?яяИюяяьяяяяя€яяияяяяяь88яяяяяю88888?яё888/  яяА ю  яяь  яяяяяА яяа яяяяяьяяяяяр?яђЂ яяА яѓѓяяю  яяяяяА яяа яяяяяэ яяяяы?яЃЂЂџяяАЂ‡яяяяяюЂЂяяяяяаЂѓяяаЂ‡яяяяящѓѓѓЃГягЃѓѓѓѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяюЂЂ?яяяяяаЂѓяяАЂ‡яяяяящѓѓѓѓЃЂѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏЂ яяА яяяяяя  ?яяяяяа яяа яяяяяэ 1?яЃ  яяА яяяяяю  ?яяяяяа яяа яяяяяьш?яђяяШяяяяяю?яяяяяряяияяяяяь888888888?ш8888?яё888?яяИяяяяяя?Ияяияяяяяь888888888ш8888?яё888/  яяА яяяяяя  ?А      яяа яяяяяьяр?яђЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃЂЂџяяАЂ‡яяяяяя ЂџАЂЂЂЂЂЂѓяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяя ЂџАЂЂЂЂЂЂѓяяАЂ‡яяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃ  яяА яяяяяя  ?А      яяа яяяяяьяш?яђяяШяяяяяя?Ияяияяяяяь888888888яш8888?яё888?яяИяяяяяя?Ияяияяяяяь888888888яш8888?яё888/  яяА яяяяяя  ?А      яяа яяяяяьяр?яђЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃЂЂџяяАЂ‡яяяяяя ЂїАЂЂ   ЂѓяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяя ЂїАЂѓГГАЂѓяяАЂ‡яяяяящѓѓѓѓѓѓѓЂЂярЂѓЃЃїяЃѓѓѓЏЂ яяА яяяяяя  ?А яяа яяа яяяяяэяяяяЃчяячЃ  яяА ю<<яяю  ?А яяа яяа яяяяяьяяяяђяяяяяяШю  яяю?ияяияяияяяяяь8888888яяяяё?яяяя888?яяИюяяю?ияяияяияяяяяь8888888яяяяё?яяяя888/Ђ яяА ю  яяь  а яяА яяа яяяяяьяяяяђяяяяЂ яяА ю  яяь  а яяА яяа яяяяяэяяяяЃяяяяЃЂЂЏяяАЂ‡ю ЂяяшЂЂяаЂ‡яяАЂ‡яяаЂ‡яяяяящѓѓѓѓѓѓѓџяяяяГѓяяяяѓѓѓЏЂЂЏяяАЂ‡я ЂяшЂЂяаЂѓяяЂЂЏяяАЂ‡яяяяящѓѓѓѓѓѓѓџяяяяѓѓяяяяѓѓѓЏЂ яяА я  яр  яа яяЂ яяа яяяяяэяяяяЃяяяяЃЂ яяЂ я  ?яр  яр яя  яяа яяяяяьяяяяђяяяяяяяяаяшяюяяияяяяяь8888888яяяяё?яяяя888?И	яьяяИ	яшяь?яяияяяяяь8888888яяяяё?яяяя888/А  яш  я  я  яш  ш  ?яяа яяяяяьяш?яњА  Ђ  яЂ ь  яш  А  яяа яяяяяэ  яр   ?яЂаЂЂЂ ЂЂ‡яАЂЂ ЂЂ‡яьЂЂЂ ЂЂяю  ЂЂяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏаЂЂЂЂЂЂ‡яАЂЂЂЂЂ‡яюЂЂЂЂЂЂяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏр      яа     яю     яю    яяяяэ яс?яЃр      яр     яя     яю    яяяяьяш?яђшяр?яяяюяяяяь888888888яш8888?яё888?ьHяшяя€яю	яяяяь888888888яш8888?яё888/ь    А яь     яяяА    яю    яяяяьяр?яђю   А яю    яяяа    яю    яяяяэ яс?яЃя ЂЂѓАЂ‡яяЂЂЂЂѓяяярЂЂЂЂяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏяЂЂЂ‡АЂ‡яяАЂЂЂ‡яяяшЂЂЂЂяяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏяа  А яяа   яяяя   яяю    яяяяэ яс?яЃяш  ?А яяш   яяяяЂ  яяю    яяяяь  ?яђяь яШяяю яяяяяш ?яяюяяяяь888888888888888?яё888?яя яИяяяш ?яяяяяю яяяяяияяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяШяяяяяяяяяяяяяяяяяяяияяяяяь888888888888888?яё888?яяяяяИяяяяяяяяяяяяяяяяяяяияяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяШяяяяяяяяяяяяяяяяяяяа яяяяяь888888888888888?яё888?яяяяяИяяяяяяяяяяяяяяяяяяяш8?яяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяьяђяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяэяЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяэѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‚яяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяэѓѓѓѓѓѓѓѓѓѓѓѓѓЃЃяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяэяяЃяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяШяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888?яяя8888яяяяяИяяяяяяяяяяяяяяяяяяяяяяяяяяяю8888888888888?яяя8888яяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяѓѓѓѓїяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяюѓѓѓѓїяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяё888888888888?яяь8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяё888888888888?яяш8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяРяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБяябяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓ‡яяГѓѓѓЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяясяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888888888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяГѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‚яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяябяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш88888888888888888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь88888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБ яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя????????????яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя
    BARCODE 326,222,""128M"",59,0,90,2,4,""!104{Mid(snarr(0), 1, 4)}!099{Mid(snarr(0), 5, 4)}!100{Mid(snarr(0), 9, 3)}!099{Mid(snarr(0), 12)}""
    CODEPAGE 1251
    TEXT 262,279,""ROMAN.TTF"",90,1,4,""{snarr(0)}""
    TEXT 378,224,""ROMAN.TTF"",90,1,5,""T1100-RUS""
    BARCODE 222,222,""128M"",59,0,90,3,6,""!105{Mid(snarr(1), 1, 14)}!100{Mid(snarr(1), 15)}""
    TEXT 158,296,""ROMAN.TTF"",90,1,4,""{snarr(1)}""
    BARCODE 118,222,""128M"",59,0,90,3,6,""!105{Mid(snarr(2), 1, 14)}!100{Mid(snarr(2), 15)}""
    TEXT 54,296,""ROMAN.TTF"",90,1,4,""{snarr(2)}""
    TEXT 321,68,""ROMAN.TTF"",90,1,6,""SN: ""
    TEXT 217,68,""ROMAN.TTF"",90,1,6,""IMEI1: ""
    TEXT 114,68,""ROMAN.TTF"",90,1,6,""IMEI2: ""
    TEXT 1445,387,""ROMAN.TTF"",180,1,6,""T1100-RUS""
    TEXT 1043,537,""ROMAN.TTF"",180,1,6,""SN: ""
    TEXT 956,371,""ROMAN.TTF"",180,1,6,""EAN: ""
    TEXT 1599,202,""ROMAN.TTF"",180,1,6,""IMEI: ""
    BARCODE 924,590,""128M"",104,0,180,2,4,""!104{Mid(snarr(0), 1, 4)}!099{Mid(snarr(0), 5, 4)}!100{Mid(snarr(0), 9, 3)}!099{Mid(snarr(0), 12)}""
    TEXT 904,481,""ROMAN.TTF"",180,1,5,""{snarr(0)}""
    BARCODE 809,416,""EAN13"",98,0,180,3,6,""4680059766513""
    TEXT 797,313,""ROMAN.TTF"",180,1,4,""6""
    TEXT 776,313,""ROMAN.TTF"",180,1,4,""8""
    TEXT 754,313,""ROMAN.TTF"",180,1,4,""0""
    TEXT 733,313,""ROMAN.TTF"",180,1,4,""0""
    TEXT 711,313,""ROMAN.TTF"",180,1,4,""5""
    TEXT 690,313,""ROMAN.TTF"",180,1,4,""9""
    TEXT 659,313,""ROMAN.TTF"",180,1,4,""7""
    TEXT 638,313,""ROMAN.TTF"",180,1,4,""6""
    TEXT 616,313,""ROMAN.TTF"",180,1,4,""6""
    TEXT 595,313,""ROMAN.TTF"",180,1,4,""5""
    TEXT 573,313,""ROMAN.TTF"",180,1,4,""1""
    TEXT 552,313,""ROMAN.TTF"",180,1,4,""3""
    TEXT 836,313,""ROMAN.TTF"",180,1,4,""4""
    BARCODE 1457,233,""128M"",90,0,180,3,6,""!105{Mid(snarr(1), 1, 14)}!100{Mid(snarr(1), 15)}""
    TEXT 1370,138,""ROMAN.TTF"",180,1,5,""{snarr(1)}""
    BARCODE 926,245,""128M"",102,0,180,3,6,""!105{Mid(snarr(2), 1, 14)}!100{Mid(snarr(2), 15)}""
    TEXT 839,138,""ROMAN.TTF"",180,1,5,""{snarr(2)}""
    TEXT 1482,141,""ROMAN.TTF"",180,1,5,""IMEI1:""
    TEXT 950,141,""ROMAN.TTF"",180,1,5,""IMEI2:""
    TEXT 866,87,""ROMAN.TTF"",180,1,5,""Сделано в России""
    TEXT 1599,330,""ROMAN.TTF"",180,1,6,""Торговая марка F+ tech ""
    PRINT 1,1
    CLS
    "
    End Function
#End Region
#Region "Планшет Т800"
    Private Function GetLabelContent_TabletT800(sn As String, x As Integer, y As Integer) As String
        Dim snarr As ArrayList = SelectListString($"select top (1) sn,IMEI,IMEI2,format(printdate, 'ddMMyy') FROM [FAS].[dbo].[CT_Aquarius] where SN = '{sn}'")
        Return $"
SIZE 69.10 mm, 29.9 mm
GAP 2 mm, 0 mm
DIRECTION 0,0
REFERENCE 0,0
OFFSET 0 mm
SET PEEL OFF
SET CUTTER OFF
SET PARTIAL_CUTTER OFF
SET TEAR ON
CLS
BITMAP 1139,494,58,176,1,яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБЃЃЃЃЃЃЃЃЃЃЃѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяябЃЃЃЃЃЃЃЃЃЃЃЃЃѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓЂяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяшяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяи8888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя88888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяуѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя8888888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю8888888888888888888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяыѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяуѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяясяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяаяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш888888888888888889яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяШ88888888888888<<8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяР?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяЃ?яЃ яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яЃ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яђ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяю?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэ?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь888888888888888?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь888888888888888?яё888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьp?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэ с?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓЃуяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓуяяѓѓѓѓЏяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэс?яЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьр?яђяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888ш?яё888?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888?ш?яё888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьр?яђяяяяяяяяяяяа ?яяяяяр яяяяАяяяяяяяэяб?яЃЂ яяА яяяЂЂяяяяяЂЂЂяяяя ЂяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓяГяяѓѓѓѓЏЂЂџяяАЂ‡яяш ЂЂяяяяр ЂЂ‡яяьЂЂ‡яяяяяящѓѓѓѓѓѓѓѓѓѓѓѓ‡яѓяяѓѓѓѓЏЂ яяА яяа   яяяА   яяь  яяяяяяэя?яЃ  яяА яяА   яяя     яяь  яяяяяяь?ь?яђяяШяяяяь?яьяяяяяь888888888888ш8?яё888?яяИяю	яяш?яьяяяяяь888888888889яш8?яё888/  яяА яь     яяр     яю   ?яяяяяьяР?яђЂ яяА яш     яа     яю   ?яяяяяэяЃ?яЃЂЂџяяАЂ‡ярЂЂЂЂЂ?ярЂЂЂЂЂѓяюЂЂЂџяяяяящѓѓѓѓѓѓѓѓѓѓѓюѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яаЂЂЂЂЂџярЂЂЂЂЂЃяюЂЂЂЏяяяяящѓѓѓѓѓѓѓѓѓѓЃясѓѓяяѓѓѓѓЏЂ яяА яА     яр     яю   яяяяяэяб?яЃ  яяА яА     яш      яю   яяяяяья€?яђяяШя яь  ю яяяяяь8888888888?ю888?яё888?яяИя€ ьяью?ю<яяяяяь8888888889яь888?яё888/  яяА я  я  яь яр  ?яя  яяяяяьяр?яђЂ яяА я  яА яюяяш  яяЂ яяяяяэяяЃ?яЃЂЂџяяАЂ‡я ЂїяаЂЃяяяяьЂЂџяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓяяѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡я ЂярЂЃяяяяюЂЂЏяяАЂ‡яяяяящѓѓѓѓѓѓѓЃяясѓѓѓѓяяѓѓѓѓЏЂ яяА я  яш  яяяяяя  яяА яяяяяэ яяБ?яЃ  яяА ю  яш  яяяяяяЂ яяа яяяяяьь    яяю?яђяяШюяяьяяяяяяяияяяяяь8?ю<<?яяяш88888?яё888?яяИюяяьяяяяя€яяияяяяяь88яяяяяю88888?яё888/  яяА ю  яяь  яяяяяА яяа яяяяяьяяяяяр?яђЂ яяА яѓѓяяю  яяяяяА яяа яяяяяэ яяяяы?яЃЂЂџяяАЂ‡яяяяяюЂЂяяяяяаЂѓяяаЂ‡яяяяящѓѓѓЃГягЃѓѓѓѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяюЂЂ?яяяяяаЂѓяяАЂ‡яяяяящѓѓѓѓЃЂѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏЂ яяА яяяяяя  ?яяяяяа яяа яяяяяэ 1?яЃ  яяА яяяяяю  ?яяяяяа яяа яяяяяьш?яђяяШяяяяяю?яяяяяряяияяяяяь888888888?ш8888?яё888?яяИяяяяяя?Ияяияяяяяь888888888ш8888?яё888/  яяА яяяяяя  ?А      яяа яяяяяьяр?яђЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃЂЂџяяАЂ‡яяяяяя ЂџАЂЂЂЂЂЂѓяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяя ЂџАЂЂЂЂЂЂѓяяАЂ‡яяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃ  яяА яяяяяя  ?А      яяа яяяяяьяш?яђяяШяяяяяя?Ияяияяяяяь888888888яш8888?яё888?яяИяяяяяя?Ияяияяяяяь888888888яш8888?яё888/  яяА яяяяяя  ?А      яяа яяяяяьяр?яђЂ яяА яяяяяя  ?А      яяа яяяяяэ яс?яЃЂЂџяяАЂ‡яяяяяя ЂїАЂЂ   ЂѓяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏЂЂџяяАЂ‡яяяяяя ЂїАЂѓГГАЂѓяяАЂ‡яяяяящѓѓѓѓѓѓѓЂЂярЂѓЃЃїяЃѓѓѓЏЂ яяА яяяяяя  ?А яяа яяа яяяяяэяяяяЃчяячЃ  яяА ю<<яяю  ?А яяа яяа яяяяяьяяяяђяяяяяяШю  яяю?ияяияяияяяяяь8888888яяяяё?яяяя888?яяИюяяю?ияяияяияяяяяь8888888яяяяё?яяяя888/Ђ яяА ю  яяь  а яяА яяа яяяяяьяяяяђяяяяЂ яяА ю  яяь  а яяА яяа яяяяяэяяяяЃяяяяЃЂЂЏяяАЂ‡ю ЂяяшЂЂяаЂ‡яяАЂ‡яяаЂ‡яяяяящѓѓѓѓѓѓѓџяяяяГѓяяяяѓѓѓЏЂЂЏяяАЂ‡я ЂяшЂЂяаЂѓяяЂЂЏяяАЂ‡яяяяящѓѓѓѓѓѓѓџяяяяѓѓяяяяѓѓѓЏЂ яяА я  яр  яа яяЂ яяа яяяяяэяяяяЃяяяяЃЂ яяЂ я  ?яр  яр яя  яяа яяяяяьяяяяђяяяяяяяяаяшяюяяияяяяяь8888888яяяяё?яяяя888?И	яьяяИ	яшяь?яяияяяяяь8888888яяяяё?яяяя888/А  яш  я  я  яш  ш  ?яяа яяяяяьяш?яњА  Ђ  яЂ ь  яш  А  яяа яяяяяэ  яр   ?яЂаЂЂЂ ЂЂ‡яАЂЂ ЂЂ‡яьЂЂЂ ЂЂяю  ЂЂяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏаЂЂЂЂЂЂ‡яАЂЂЂЂЂ‡яюЂЂЂЂЂЂяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏр      яа     яю     яю    яяяяэ яс?яЃр      яр     яя     яю    яяяяьяш?яђшяр?яяяюяяяяь888888888яш8888?яё888?ьHяшяя€яю	яяяяь888888888яш8888?яё888/ь    А яь     яяяА    яю    яяяяьяр?яђю   А яю    яяяа    яю    яяяяэ яс?яЃя ЂЂѓАЂ‡яяЂЂЂЂѓяяярЂЂЂЂяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓѓяуѓѓѓѓяяѓѓѓѓЏяЂЂЂ‡АЂ‡яяАЂЂЂ‡яяяшЂЂЂЂяяюЂЂЂЂЃяяяящѓѓѓѓѓѓѓѓ‚яуѓѓѓѓяяѓѓѓѓЏяа  А яяа   яяяя   яяю    яяяяэ яс?яЃяш  ?А яяш   яяяяЂ  яяю    яяяяь  ?яђяь яШяяю яяяяяш ?яяюяяяяь888888888888888?яё888?яя яИяяяш ?яяяяяю яяяяяияяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяШяяяяяяяяяяяяяяяяяяяияяяяяь888888888888888?яё888?яяяяяИяяяяяяяяяяяяяяяяяяяияяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяаЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяАЂ‡яяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяэ?яЃяяяяяА яяяяяяяяяяяяяяяяяяяа яяяяяь?яђяяяяяШяяяяяяяяяяяяяяяяяяяа яяяяяь888888888888888?яё888?яяяяяИяяяяяяяяяяяяяяяяяяяш8?яяяяяь888888888888888?яё888/яяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяьяђяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяэяЃяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяэѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‚яяѓѓѓѓЏяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяэѓѓѓѓѓѓѓѓѓѓѓѓѓЃЃяяѓѓѓѓЏяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяэяяЃяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяШяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888?яяя8888яяяяяИяяяяяяяяяяяяяяяяяяяяяяяяяяяю8888888888888?яяя8888яяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяѓѓѓѓїяяяяяАЂ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяюѓѓѓѓїяяяяяА яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяё888888888888?яяь8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяё888888888888?яяш8888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяРяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБяябяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓ‡яяГѓѓѓЃяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓяяѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяясяьяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888888888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь8888888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяГѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‚яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяябяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяш88888888888888888/яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь88888888888888888яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяБ яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяягѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяящѓѓѓѓѓѓѓѓѓѓѓѓѓѓѓ‡яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя ?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяряяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяь?яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя????????????яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя
BARCODE 326,222,""128M"",59,0,90,2,4,""!104{Mid(snarr(0), 1, 11)}!099{Mid(snarr(0), 12)}""
TEXT 262,279,""ROMAN.TTF"",90,1,4,""{snarr(0)}""
TEXT 378,224,""ROMAN.TTF"",90,1,5,""T800-RUS""
BARCODE 222,222,""128M"",59,0,90,3,6,""!105{Mid(snarr(1), 1, 14)}!100{Mid(snarr(1), 15)}""
TEXT 158,296,""ROMAN.TTF"",90,1,4,""{snarr(1)}""
BARCODE 118,222,""128M"",59,0,90,3,6,""!105{Mid(snarr(2), 1, 14)}!100{Mid(snarr(2), 15)}""
TEXT 54,296,""ROMAN.TTF"",90,1,4,""{snarr(2)}""
TEXT 321,68,""ROMAN.TTF"",90,1,6,""SN: ""
TEXT 217,68,""ROMAN.TTF"",90,1,6,""IMEI1: ""
TEXT 114,68,""ROMAN.TTF"",90,1,6,""IMEI2: ""
TEXT 1445,387,""ROMAN.TTF"",180,1,6,""T800-RUS""
TEXT 1043,537,""ROMAN.TTF"",180,1,6,""SN: ""
TEXT 956,371,""ROMAN.TTF"",180,1,6,""EAN: ""
TEXT 1599,202,""ROMAN.TTF"",180,1,6,""IMEI: ""
BARCODE 924,590,""128M"",104,0,180,2,4,""!104{Mid(snarr(0), 1, 11)}!099{Mid(snarr(0), 12)}""
TEXT 904,481,""ROMAN.TTF"",180,1,5,""{snarr(0)}""
BARCODE 809,416,""EAN13"",98,0,180,3,6,""4680059766575""
TEXT 797,313,""ROMAN.TTF"",180,1,4,""6""
TEXT 776,313,""ROMAN.TTF"",180,1,4,""8""
TEXT 754,313,""ROMAN.TTF"",180,1,4,""0""
TEXT 733,313,""ROMAN.TTF"",180,1,4,""0""
TEXT 711,313,""ROMAN.TTF"",180,1,4,""5""
TEXT 690,313,""ROMAN.TTF"",180,1,4,""9""
TEXT 659,313,""ROMAN.TTF"",180,1,4,""7""
TEXT 638,313,""ROMAN.TTF"",180,1,4,""6""
TEXT 616,313,""ROMAN.TTF"",180,1,4,""6""
TEXT 595,313,""ROMAN.TTF"",180,1,4,""5""
TEXT 573,313,""ROMAN.TTF"",180,1,4,""7""
TEXT 552,313,""ROMAN.TTF"",180,1,4,""5""
TEXT 836,313,""ROMAN.TTF"",180,1,4,""4""
BARCODE 1457,233,""128M"",90,0,180,3,6,""!105{Mid(snarr(1), 1, 14)}!100{Mid(snarr(1), 15)}""
TEXT 1370,138,""ROMAN.TTF"",180,1,5,""{snarr(1)}""
BARCODE 926,245,""128M"",102,0,180,3,6,""!105{Mid(snarr(2), 1, 14)}!100{Mid(snarr(2), 15)}""
TEXT 839,138,""ROMAN.TTF"",180,1,5,""{snarr(2)}""
TEXT 1482,141,""ROMAN.TTF"",180,1,5,""IMEI1:""
TEXT 950,141,""ROMAN.TTF"",180,1,5,""IMEI2:""
TEXT 866,87,""ROMAN.TTF"",180,1,5,""Сделано в России""
TEXT 1599,330,""ROMAN.TTF"",180,1,6,""Торговая марка F+ tech ""
PRINT 1,1
CLS
"
    End Function
#End Region
#Region "Смартфон AYYA"
    Private Function GetLabelContent_SmartPhone_Y(sn As String, x As Integer, y As Integer) As String
        Dim snarr As ArrayList = SelectListString($"select top (1) sn,IMEI,IMEI2,format(printdate, 'ddMMyy') FROM [FAS].[dbo].[CT_Aquarius] where SN = '{sn}'")
        Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
~DG000.GRF,10368,108,
,::::::::::::::::::::::::::::::lG07FC,hR03FC0iK07FC,P07FE0gX03FC0gH01FLF80gK03FHF80H07FC0gT01FFE0H07FE0H07FEFFC0H0HFC001FFE0K07FKFE,O07FHFE0gW03FC0gH01FLF80gK0KF8007FC0gT01FFE0H07FE0H0HFCFFC001FF8001FFE0K07FKFE,N01FJFgX03FC0gH01FLF80gK0KFE007FC0gT03FFE0H03FF0H0HF87FE001FF0H03FFE0K07FKFE0H07FC,N07FIFE0gW03FC0gH01FLF80gH0800FKFH07FC0gT03FHFI01FF001FF83FE003FF0H03FHFL07FKFE001FFC,N0KFE0gW03FC0gH01FLF80gG07800FKF807FC0gT03FHFI01FF801FF03FF003FE0H03FHFL07FKFE007FFC,M01FJFE0gW03FC0gH01FLF80g07F800FKFC07FC0gT07FHFJ0HF801FF01FF003FE0H07FHFL07FKFE01FHFC,M03FJFC0gW03FC0gH01FLF80g0HF800FF81FFE07FC0gT07F7F80H0HFC03FE01FF807FC0H07F7F80J07FKFE07FHFC,M07FFC01C0gW03FC0gK07FC0gI0HF800FF807FE07FC0gT07F7F80H07FC03FE00FF807FC0H07F7F80L01FF0I0JFC,M0IFhH03FC0gK07FC0gI0HF800FF803FE07FC0gT07F7F80H07FC07FC00FF80FF80H07F7F80L01FF0I07FHFC,M0HFC0hG03FC0gK07FC0gI0HF800FF803FF07FC0gT0HF7FC0H03FE07FC007FC0FF80H0HF7FC0L01FF0I07FHFC,L01FF80hG03FC0gK07FC0gI0HF800FF801FF07FC0gT0HF3FC0H03FE07F8007FC0FF0I0HF3FC0L01FF0I07F7FC,L01FF0K03FFC00FFE0H03FE0H03FC0FE001FKFI01FHF80K0HFI01FF007FC0K07FC0H07F03C1FF007FC0H0HF80FIFE0FF801FF07FC0FC0J07F80H0HF03F80J0FE0M0FE3FC0H01FF0FF0H03FE1FE0I0FE3FC0L01FF0I07C7FC,L03FF0K03FFC01FFE001FHF8003FC7FFC01FKFI0KFK07FFC001FF007FC0K07FC0H07F0FC1FF007FC007FHF8FIFE0FF801FF07FC7FF80H03FFE0H0HF1FFE0I07FF80K01FE3FC0H01FF0FF0H03FE1FE0H01FE3FC0L01FF0I0307FC,L03FE0K03FFC01FFE00FIFE003FCFHFE01FKFH03FJFC0H01FIFH01FF007FC0K07FC0H07F9FC1FF007FC00FIF0FIFE0FF801FF07FDFHFC0H0JF800FF3FHF8001FHFE0K01FE3FE0I0HF1FE0H01FE3FC0H01FE3FE0L01FF0K07FC,L03FE0K03FFC01FFE00FJFH03FDFIF01FKFH07FJFE0H03FIFC01FF007FC0K07FC0H07FBFC1FF007FC01FIF0FIFE0FF801FF07FJFE001FIFE00FKFC003FIFL01FE1FE0I0HF9FE0H01FF3FC0H01FE1FE0L01FF0K07FC,L03FE0K03FFE01FFE007FIF803FKF81FKF01FLF8007FIFE01FF007FC0K07FC0H07FBFC1FF007FC03FIF0FIFE0FF801FF07FJFE003FJFH0LFC007FIF80J03FE1FE0I07F9FC0I0HF3F80H03FE1FE0L01FF0K07FC,L07FC0K03FFE03FFE007FIF803FKFC1FKF03FLFC00FJFE01FF007FC0K07FC0H07FHFC1FF007FC07FIF0FIFE0FF803FE07FKFH07FJFH0LFE00FJFC0J03FC1FF0I07FHFC0I0JF80H03FC1FF0L01FF0K07FC,L07FC0K03FFE03FFE007C0FFC03FFC1FFC001FF0H03FF3FCFFC01FF83FF01FF007FC0K07FC0H07FHFC1FF007FC07FC0E00FF800FF803FE07FF87FF00FFC1FF80FHF07FE01FF07FC0J03FC1FF0I03FHF80I07FHFJ03FC1FF0L01FF0K07FC,L07FC0K03FHF03EFF002007FC03FF80FFE001FF0H07FE3FC7FE01FF01FF01FF007FC0K07FC0H07FHFC1FF007FC0FF80100FF800FF80FFE07FE01FF00FF80FF80FFE03FE01FE03FE0J03FC0FF0I03FHF80I07FHFJ03FC0FF0L01FF0K07FC,L07FC0K03FHF07EFF0J03FC03FF003FE001FF0H0HFC3FC3FF03FE00FF81FF007FC0K07FC0H07FF801FF007FC0FF80I0HF800FF81FFC07FE01FF81FF007FC0FFC03FF03FC01FE0J07FC0FF80H01FHFK03FFE0I07FC0FF80K01FF0K07FC,L07FC0K07FBF07EFF0J03FE03FF003FE001FF0H0HF83FC1FF03FE00FF81FF007FC0K07FC0H07FF001FF007FC0FFC0I0HF800FKFC07FC00FF81FF007FC0FF801FF03FC00FE0J07F80FF80H01FFE0J03FFC0I07F80FF80K01FF0K07FC,L07FC0K07FBF07EFF0I03FFE03FE003FF001FF0H0HF83FC1FF03FE00FF81FF007FC0K07FC0H07FE001FF007FC0FFE0I0HF800FKF807FC00FF81FF007FC0FF801FF03F800FF0J07F80FF80I0HFE0J01FFC0I07F80FF80K01FF0K07FC,L07FC0K07FBF8FCFF0H07FHFE03FE001FF001FF001FF03FC0FF87FC007FC1FKFC0K07FC0H07FC001FF007FC07FFC0H0HF800FKFH07FC00FF83FE003FE0FF801FF03F800FF0J0HF807FC0I07FC0K0HF80I0HF807FC0K01FF0K07FC,L07FC0K07FBF8FCFF001FIFE03FE001FF001FF001FF03FC0FF87FC007FC1FKFC0K07FC0H07FC001FF007FC07FHF800FF800FJFC007FC00FF83FE003FE0FF801FF07FKFK0HFH07FC0I07FC0K0HF80I0HFH07FC0K01FF0K07FC,L07FE0K07F9F8FCFF003FIFE03FE001FF001FF001FF03FC0FF87FC007FC1FKFC0K07FC0H07FC001FF007FC03FHFC00FF800FJFI07FC00FF83FE003FE0FF801FF07FKFK0LFC0I07FC0K0HF80I0LFC0K01FF0K07FC,L03FE0K07F9FDF8FF007FIFE03FE001FF001FF001FF03FC0FF87FC007FC1FKFC0K07FC0H07FC001FF007FC01FHFE00FF800FIF80H07FC00FF83FE003FE0FF801FF07FKFJ01FKFC0I07FC0K0HF80H01FKFC0K01FF0K07FC,L03FE0K07F9FDF8FF00FJFE03FE001FF001FF001FF03FC0FF87FC007FC1FKFC0K07FC0H07FC001FF007FC00FIFH0HF800FF80J07FC00FF83FE003FE0FF801FF07FKFJ01FKFE0I07FC0K0HF80H01FKFE0K01FF0K07FC,L03FF0K07F9FDF8FF01FFC3FE03FE001FF001FF001FF03FC0FF87FC007FC1FKFC0K07FC0H07FC001FF007FC003FHF80FF800FF80J07FC00FF83FE003FE0FF801FF07FKFJ01FKFE0I07FC0K0HF80H01FKFE0K01FF0K07FC,L03FF0K07F8FDF8FF01FF03FE03FE001FF001FF001FF03FC0FF87FC007FC1FF007FC0K07FC0H07FC001FF007FC0H0IF80FF800FF80J07FC00FF83FE003FE0FF801FF07FKFJ01FKFE0I07FC0K0HF80H01FKFE0K01FF0K07FC,L01FF80J07F0FHF0FF03FE03FE03FE003FE001FF0H0HF83FC1FF03FE00FF81FF007FC0K07FC0H07FC001FF007FC0H01FFC0FF800FF80J07FC00FF81FF007FC0FF801FF07F80M03FLFJ07FC0K0HF80H03FLFL01FF0K07FC,L01FFC0J07F0FHF0FF03FE03FE03FE003FE001FF0H0HF83FC1FF03FE00FF81FF007FC0K07FC0H07FC001FF007FC0I07FC0FF800FF80J07FC00FF81FF007FC0FF801FF03F80M03FE003FF0I07FC0K0HF80H03FE003FF0K01FF0K07FC,M0IFK07F0FHF0FF03FE03FE03FF007FE001FF0H0HFC3FC3FF03FE00FF81FF007FC0K07FC0H07FC001FF80FFC0I07FC0FFC00FF80J07FC00FF81FF007FC0FF801FF03FC0M03FE001FF0I07FC0K0HF80H03FE001FF0K01FF0K07FC,M0IFC01C07F07FE0FF03FE07FE03FF80FFE001FF0H07FE3FC7FE01FF01FF01FF007FC0K07FC0H07FC0H0HF80FFC04007FC0FFC00FF80J07FC00FF80FF80FF80FF801FF03FE0M07FC001FF80H07FC0K0HF80H07FC001FF80J01FF0K07FC,M07FJFC07F07FE0FF03FF0FFE03FFC1FFC001FF0H03FF3FCFFC01FF83FF01FF007FC0K07FC0H07FC0H0HFC3FFC0780FF807FFE0FF80J07FC00FF80FFC1FF80FF801FF01FFC0180I07FC001FF80H07FC0K0HF80H07FC001FF80J01FF0K07FC,M03FJFC07F07FE0FF81FJFE03FKFC001FF0H03FLFC00FJFE01FF007FC0K07FC0H07FC0H0LFC0FJF807FFE0FF80J07FC00FF807FJFH0HF801FF01FJFC0I07FC0H0HF80H07FC0K0HF80H07FC0H0HF80J01FF0K07FC,M01FJFE0FF03FC0FF81FJFE03FKF8001FF0H01FLF8007FIFC01FF007FC0K07FC0H07FC0H07FJFC0FJFH07FFE0FF80J07FC00FF803FIFE00FF801FF00FJFC0I0HFC0H0HF80H07FC0K0HF80H0HFC0H0HF80J01FF0K07FC,N0KFE0FF03FC07F80FJFE03FKFI01FF0I0MFI07FIF801FF007FC0K07FC0H07FC0H07FJFC0FJFH03FFE0FF80J07FC00FF803FIFC00FF801FF007FIFC0I0HF80H0HFC0H07FC0K0HF80H0HF80H0HFC0J01FF0K07FC,N03FIFE0FF03FC07F807FFDFE03FJFE0H01FF0I03FJFC0H01FIFH01FF007FC0K07FC0H07FC0H03FHFBFC1FIFE003FFE0FF80J07FC00FF800FIF800FF801FF003FIFC0I0HF80H07FC0H07FC0K0HF80H0HF80H07FC0J01FF0K07FC,O0JFE0FF03FC07F803FF1FE03FE7FF80H01FF0J0KFK07FFC001FF007FC0K07FC0H07FC0H01FFE3FC07FHF80H0HFE0FF80J07FC00FF8003FFE0H0HF801FF0H0JFC0I0HF80H07FC0H07FC0K0HF80H0HF80H07FC0J01FF0K07FC,P0HFE00FF0J07F800FC1FE03FE1FC0I01FF0J01FHF80K0FE0H01FF007FC0K07FC0H07FC0I03F83FC007F80I03FC0FF80J07FC00FF80H07F0I0HF801FF0I0HFC0I01FF0I07FE0H07FC0K0HF8001FF0I07FE0J01FF0K07FC,gS03FE0U03FC0,::::::::::,:::::::::::~DG001.GRF,19968,104,
,::::::::::::::gQ03FOF80T07FKFC0gN0MFE001FLF80gM03FLF80T07FOF8,gQ07FOFC0T03FKFE0gM01FLFE0H0MF80gM07FLF80T07FOFC,gQ07FOFE0T03FLFgN01FLFC0H0MFC0gM07FLFV0QFC,gQ0QFE0T01FLFgN03FLF80H07FKFC0gM0NFV0QFE,gP01FQFV0MF80gL07FLF80H03FKFE0gM0MFE0T01FPFE,gP01FQF80T0MFC0gL07FLFJ03FLFgM01FLFC0T03FQF,gP03FQF80T07FKFC0gL0NFJ01FLFgM03FLFC0T07FQF80,gP03FQFC0T03FKFE0gL0MFE0J0MF80gK03FLF80T07FQF80,gP07FQFE0T03FKFE0gK01FLFC0J0MFC0gK07FLFV0SFC0,gP0SFE0T01FLFgL01FLFC0J07FKFC0gK07FLFV0SFE0,gP0TFV0MF80gJ03FLF80J03FKFE0gK0MFE0T01FRFE0,gO01FSFV0MF80gJ07FLFL03FLFgK01FLFC0T03FSF0,gO03FSF80T07FKFC0gJ07FLFL01FLFgK01FLFC0T03FSF8,gO03FSFC0T07FKFE0gJ0MFE0L0MF80gI03FLF80T07FSF8,gO07FSFC0T03FKFE0gI01FLFC0L0MF80gI03FLF80T07FSFC,gO07FSFE0T01FLFgJ01FLFC0L07FKFC0gI07FLFV0UFC,gO0VFU01FLF80gH03FLF80L07FKFE0gI0MFE0T01FTFE,gN01FUFV0MF80gH03FLF80L03FKFE0gI0MFE0T01FUF,gN01FLFEFMF80T07FKFC0gH07FLFN01FLFgI01FLFC0T03FLFCFMF,gN03FLFC7FLFC0T07FKFE0gH0MFE0M01FLF80gG03FLF80T07FLFC7FLF80,gN07FLF83FLFC0T03FKFE0gH0MFE0N0MF80gG03FLF80T07FLF87FLFC0,gN07FLF81FLFE0T01FLFgH01FLFC0N07FKFC0gG07FLFV0NF03FLFC0,gN0NF01FLFE0T01FLFgH03FLF80N07FKFE0gG07FKFE0T01FLFE01FLFE0,gM01FMFH0NFV0MF80g03FLF80N03FKFE0gG0MFE0T01FLFE01FLFE0,gM01FLFE007FLF80T0MFC0g07FLFP01FLFgG01FLFC0T03FLFC00FMF0,gM03FLFC007FLF80T07FKFC0g07FKFE0O01FLFgG01FLF80T03FLFC007FLF8,gM03FLFC003FLFC0T03FKFE0g0MFE0P0MF80Y03FLF80T07FLF8007FLF8,gM07FLF8003FLFE0T03FLFg01FLFC0P0MFC0Y03FLFV0NFI03FLFC,gM0NF8001FLFE0T01FLFg01FLF80P07FKFC0Y07FLFV0NFI01FLFE,gL01FMFJ0NFV0MF80X03FLF80P03FKFE0Y0MFE0T01FLFE0H01FLFE,gL01FLFE0I0NF80T0MF80X03FLFR03FLFg0MFC0T03FLFE0I0NF,gL03FLFC0I07FLF80T07FKFC0X07FLFR01FLFY01FLFC0T03FLFC0I0NF80,gL03FLFC0I03FLFC0T03FKFE0X0MFE0R0MF80W03FLF80T07FLF80I07FLF80,gL07FLF80I03FLFC0T03FKFE0X0MFC0R0MFC0W03FLFV07FLF80I03FLFC0,gL0NF80I01FLFE0T01FLFX01FLF80R07FKFC0W07FLFV0NFK03FLFC0,gL0NFL0NFU01FLF80V03FLF80R03FKFE0W07FKFE0T01FLFE0J01FLFE0,gK01FLFE0K0NFV0MF80V03FLFT03FKFE0W0MFC0T01FLFE0J01FMF0,gK01FLFE0K07FLF80T07FKFC0V07FLFT01FLFW01FLFC0T03FLFC0K0NF0,gK03FLFC0K07FLFC0T07FKFE0V07FKFE0S01FLF80U01FLF80T07FLF80K07FLF8,gK07FLFC0K03FLFC0T03FKFE0V0MFC0T0MF80U03FLFV07FLF80K07FLFC,gK07FLF80K01FLFE0T01FLFV01FLFC0T07FKFC0U03FLFV0NFM03FLFC,gK0NFM01FLFE0T01FLF80T01FLF80T07FKFE0U07FKFE0T01FMFM01FLFE,gJ01FMFN0NFV0MF80T03FLFV03FKFE0U0MFC0T01FLFE0L01FMF,gJ01FLFE0M0NF80T0MFC0T03FLFV01FLFV0MFC0T03FLFC0M0NF,gJ03FLFC0M07FLF80T07FKFC0T07FKFE0U01FLF80S01FLF80T03FLFC0M07FLF80,gJ07FLFC0M03FLFC0T03FKFE0T0MFE0V0MF80S01FLF80T07FLF80M07FLF80,gJ07FLF80M01FLFE0T01FLFU0MFC0V07FKFC0S03FLFV0NFO03FLFC0,gJ0NFO01FLFE0T01FLFT01FLF80V07FKFC0S07FKFE0U0NFO03FLFE0,gJ0NFP0NFV0MF80R01FLF80V03FKFE0S07FKFE0T01FLFE0N01FLFE0,gI01FLFE0O0NF80T0MFC0R03FLFX01FLFT0MFC0T03FLFE0O0NF0,gI03FLFC0O07FLF80T07FKFC0R07FKFE0W01FLFS01FLF80T03FLFC0O0NF0,gI03FLFC0O03FLFC0T03FKFE0R07FKFE0X0MF80Q01FLF80T07FLF80O07FLF8,gI07FLF80O03FLFC0T03FKFE0R0MFC0X0MFC0Q03FLFV0NF80O03FLFC,gI0NF80O01FLFE0T01FLFR01FLF80X07FKFC0Q03FLFV0NFQ03FLFE,gI0NFQ01FMFV0MF80P01FLF80X03FKFE0Q07FKFE0T01FLFE0P01FLFE,gH01FLFE0Q0NFV0MFC0P03FLFg03FLFR0MFC0T01FLFE0Q0NF,gH01FLFE0Q07FLF80T07FKFC0P03FKFE0Y01FLFR0MFC0T03FLFC0Q0NF,gH03FLFC0Q07FLF80T07FKFE0P07FKFE0g0MF80O01FLF80T07FLF80Q07FLF80,gH07FLF80Q03FLFC0T03FKFE0P0MFC0g0MFC0O03FLFV07FLF80Q03FLFC0,gH07FLF80Q01FLFE0T01FLFQ0MFC0g07FKFC0O03FKFE0U0NFS03FLFC0,gH0NFS01FMFV0MF80N01FLF80g07FKFE0O07FKFE0T01FMFS01FLFE0,gG01FLFE0S0NFV0MF80N01FLFgH03FKFE0O07FKFC0T01FLFE0R01FLFE0,gG01FLFE0S07FLF80T07FKFC0N03FKFE0gG01FLFP0MFC0T03FLFC0S0NF0,gG03FLFC0S07FLF80T07FKFE0N07FKFE0gH0MF80N0MF80T07FLFC0S07FLF8,gG03FLFC0S03FLFC0T03FKFE0N07FKFC0gH0MFC0M01FLFV07FLF80S07FLF8,gG07FLF80S03FLFE0T01FLFO0MFC0gH07FKFC0M03FLFV0NFU03FLFC,gG0NFU01FLFE0T01FLF80L01FLF80gH07FKFE0M03FKFE0U0NFU03FLFE,gG0NFV0NFV0MF80L01FLFgJ03FKFE0M07FKFC0T01FLFE0T01FLFE,g01FLFE0U0NFV07FKFC0L03FLFgJ01FLFN0MFC0T03FLFC0U0NF,g03FLFE0U07FLF80T07FKFE0L03FKFE0gI01FLF80L0MF80T03FLFC0U07FLF80,g03FLFC0U03FLFC0T03FKFE0L07FKFE0gJ0MF80K01FLF80T07FLF80U07FLF80,g07FLF80U03FLFE0T01FLFM0MFC0gJ0MFC0K03FLFV0NF80U03FLFC0,g0NF80U01FLFE0T01FLFM0MF80gJ07FKFC0K03FKFE0U0NFW03FLFC0,g0NFW01FMFV0MF80J01FLF80gJ03FKFE0K07FKFC0T01FLFE0V01FLFE0,Y01FLFE0W0NFV0MFC0J01FLFgL01FLFL07FKFC0T01FLFE0W0NF0,Y01FLFE0W07FLF80T07FKFC0J03FKFE0gK01FLFL0MF80T03FLFC0W0NF0,Y03FLFC0W03FLFC0T03FKFE0J07FKFC0gL0MF80I01FLF80T07FLF80W07FLF8,Y07FLF80W03FLFC0T03FLFK07FKFC0gL0MFC0I01FLFV07FLF80W07FLFC,Y07FLF80W01FLFE0T01FLFK0MF80gL07FKFC0I03FKFE0U0NFY03FLFC,Y0NFY01FLFE0U0MF80I0MF80gL03FKFE0I07FKFE0T01FLFE0X01FLFE,X01FMFg0NFV0MFC0H01FLFgN03FLFJ07FKFC0T01FLFE0X01FLFE,X01FLFE0Y07FLF80T07FKFC0H03FKFE0gM01FLFJ0MF80T03FLFC0Y0NF,X03FLFC0Y07FLF80T03FKFE0H03FKFE0gN0MF80H0MF80T03FLFC0Y0NF80,X07FLFC0Y03FLFC0T03FKFE0H07FKFC0gN0MFC001FLFV07FLF80Y07FLF80,X07FLF80Y03FLFE0T01FLFI0MF80gN07FKFC003FLFV0NFgG03FLFC0,X0NFgG01FLFE0T01FLF800FLF80gN07FKFE003FKFE0U0NFgG01FLFE0,X0NFgH0NFV0MF801FLFgP03FKFE007FKFC0T01FLFE0g01FLFE0,W01FLFE0gG07FLFV07FKFC01FKFE0gO01FLFH07FKFC0T03FLFC0gG0NF0,W03FLFC0gG07FLF80T07FKFE03FKFE0gP0MF80FLF80T03FLFC0gG0NF8,W03FLFC0gG03FLFC0T03FKFE07FKFC0gP0MF81FLFV07FLF80gG07FLF8,W07FLF80gG03FLFE0T01FLF07FKFC0gP07FKFC1FLFV0NF80gG03FLFC,W0NF80gG01FLFE0T01FLF8FLF80gP07FKFE3FKFE0U0NFgI03FLFC,W0NFgJ0NFV0UFgR03FSFC0T01FLFE0gH01FLFE,V01FLFE0gI0NFV07FSFgR01FSFC0T03FLFE0gH01FMF,V03FLFE0gI07FLF80T07FRFE0gQ01FSF80T03FLFC0gI0NF80,V03FLFC0gI07FLFC0T03FRFC0gR0TF80T07FLF80gI07FLF80,V07FLFC0gI07FLFC0T03FRFC0gR07FRFV07FLFC0gI07FLFC0,V07FgYFE0T01FRF80gR07FQFE0U0hGFC0,V0hGFE0U0SFgT03FQFE0T01FhFE0,U01FhGFV0SFgT03FQFC0T01FhGF0,U01FhGF80T07FPFE0gS01FQF80T03FhGF0,U03FhGFC0T03FPFC0gT0RF80T07FhGF8,U07FhGFC0T03FPFC0gT0RFV07FhGF8,U07FhGFE0T01FPF80gT07FOFE0U0hIFC,U0hIFE0U0QF80gT03FOFE0U0hIFE,U0hJFV0QFgV03FOFC0T01FhHFE,T01FhIF80T07FNFE0gU01FOF80T03FhIF,T03FhIF80T07FNFE0gV0PF80T03FhIF,T03FhIFC0T03FNFC0gV0PFV07FhIF80,T07FhIFC0T01FNF80gV07FMFE0U0hKFC0,T0hKFE0T01FNF80gV03FMFE0U0hKFE0,T0hLFV0OFgX03FMFC0T01FhJFE0,S01FhKFV07FMFgX01FMF80T03FhKF0,S03FhKF80T07FLFE0gW01FMF80T03FhKF0,S03FhKFC0T03FLFC0gX0NFV07FhKF8,S07FhKFC0T01FLF80gX07FKFE0U07FhKFC,S07FhKFE0T01FLF80gX07FKFE0U0hMFC,S0hNFU01FLF80gX07FKFE0T01FhLFE,R01FhMFU01FLF80gX07FKFE0T01FhLFE,R01FLFE0gQ0NF80S01FLF80gX07FKFE0T03FLFC0gQ0NF,R03FLFC0gQ07FLF80S01FLF80gX07FKFE0T07FLFC0gQ07FLF80,R07FLFC0gQ03FLFC0S01FLF80gX07FKFE0T07FLF80gQ07FLF80,R07FLF80gQ01FLFE0S01FLF80gX07FKFE0T0NFgS03FLFC0,R0NFgS01FLFE0S01FLF80gX07FKFE0S01FMFgS03FLFE0,Q01FMFgT0NFT01FLF80gX07FKFE0S01FLFE0gR01FLFE0,Q01FLFE0gS0NF80R01FLF80gX07FKFE0S03FLFC0gS0NF0,Q03FLFC0gS07FLF80R01FLF80gX07FKFE0S03FLFC0gS07FLF8,Q03FLFC0gS03FLFC0R01FLF80gX07FKFE0S07FLF80gS07FLF8,Q07FLF80gS03FLFC0R01FLF80gX07FKFE0S0NFgU03FLFC,Q0NFgU01FLFE0R01FLF80gX07FKFE0S0NFgU03FLFC,Q0NFgV0NFS01FLF80gX07FKFE0R01FLFE0gT01FLFE,P01FLFE0gU0NFS01FLF80gX07FKFE0R03FLFE0gU0NF,P03FLFE0gU07FLF80Q01FLF80gX07FKFE0R03FLFC0gU0NF,P03FLFC0gU03FLFC0Q01FLF80gX07FKFE0R07FLF80gU07FLF80,P07FLF80gU03FLFC0Q01FLF80gX07FKFE0R07FLF80gU07FLFC0,P0NF80gU01FLFE0Q01FLF80gX07FKFE0R0NFgW03FLFC0,P0NFgW01FLFE0Q01FLF80gX07FKFE0Q01FLFE0gV01FLFE0,O01FLFE0gW0NFR01FLF80gX07FKFE0Q01FLFE0gV01FMF0,O01FLFE0gW07FLF80P01FLF80gX07FKFE0Q03FLFC0gW0NF0,O03FLFC0gW07FLF80P01FLF80gX07FKFE0Q07FLFC0gW07FLF8,O07FLFC0gW03FLFC0P01FLF80gX07FKFE0Q07FLF80gW07FLFC,O07FLF80gW03FLFE0P01FLF80gX07FKFE0Q0NFgY03FLFC,O0NFgY01FLFE0P01FLF80gX07FKFE0P01FMFgY01FLFE,N01FMFh0NFQ01FLF80gX07FKFE0P01FLFE0gX01FLFE,N01FLFE0gY07FLF80O01FLF80gX07FKFE0P03FLFC0gY0NF,N03FLFC0gY07FLF80O01FLF80gX07FKFE0P03FLFC0gY0NF80,N07FLFC0gY03FLFC0O01FLF80gX07FKFE0P07FLF80gY07FLF80,N07FLF80gY03FLFC0O01FLF80gX07FKFE0P0NFhG03FLFC0,N0NFhG01FLFE0O01FLF80gX07FKFE0P0NFhG03FLFE0,N0NFhH0NFP01FLF80gX07FKFE0O01FLFE0h01FLFE0,M01FLFE0hG07FLFP01FLF80gX07FKFE0O03FLFE0hG0NF0,,:::::::::::::::::::::::::~DG002.GRF,04224,044,
,:::::::::::::::::::::M07FF0H01FFE0hM03FLF0,M0IF8001FFE0hM03FLF0,M0IF8001FFE0hM03FLFI03FE0,M0IF8003FFE0hM03FLFI0HFE0,M0IFC003FFE0hM03FLFH03FFE0,M0IFC003FFE0hM03FLFH0IFE0,M0IFC007FFE0hM03FLF03FHFE0,M0IFC007FFE0hP0HF80H07FHFE0,M0IFE007FFE0hP0HF80H03FHFE0,M0IFE00FHFE0hP0HF80H03FHFE0,M0FEFE00FHFE0hP0HF80H03FBFE0,M0FEFF00FDFE0I07F80I0KFE0I0FE0H01FJFC07FC0K0F0L0HF80H03E3FE0,M0FE7F01FDFF0H03FFE0I0KFE0H07FF8001FJFC07FC0J01F80K0HF80H0183FE0,M0FE7F01FDFF0H0JF80H0KFE001FHFE001FJFC07FC0J03FC0K0HF80J03FE0,M0FE7F01FCFF001FIFE0H0KFE003FIFH01FJFC07FC0J07FE0K0HF80J03FE0,M0FE7F81F8FF003FJFI0KFE007FIF801FJFC07FC0J07FE0K0HF80J03FE0,M0FE7F83F8FF007FJFI0KFE00FJFC01FJFC07FC0J07FE0K0HF80J03FE0,L01FE3F83F8FF00FFC1FF800FF03FE01FF07FC01FE07FC07FC0J07FE0K0HF80J03FE0,L01FE3F83F0FF00FF80FF800FF03FE01FE03FE01FE07FC07FC0J07FE0K0HF80J03FE0,L01FE3FC7F0FF01FF007FC00FF03FE03FC01FE01FE07FC07FC0J03FC0K0HF80J03FE0,L01FE3FC7F0FF01FF007FC00FF03FE03FC00FE01FE07FC07FHFE0H01F80K0HF80J03FE0,L01FE1FC7F0FF01FF007FC00FF03FE03F800FF01FE07FC07FIFC0H0F0L0HF80J03FE0,L01FE1FC7E0FF03FE003FE00FF03FE03F800FF01FE07FC07FJFQ0HF80J03FE0,L01FE1FE7E0FF03FE003FE00FF03FE07FKF01FE07FC07FJF80O0HF80J03FE0,L01FE1FEFE0FF03FE003FE00FF03FE07FKF01FE07FC07FJFC0O0HF80J03FE0,L01FE0FEFC0FF03FE003FE01FE03FE07FKF01FE07FC07FJFE0O0HF80J03FE0,L01FE0FEFC0FF03FE003FE01FE03FE07FKF01FE07FC07FC0FFE0O0HF80J03FE0,L01FE0FHFC0FF03FE003FE01FE03FE07FKF01FE07FC07FC03FF0O0HF80J03FE0,L01FE0FHF80FF03FE003FE01FE03FE07FKF01FE07FC07FC01FF0O0HF80J03FE0,L01FE07FF80FF01FF007FC03FE03FE07F80J03FC07FC07FC01FF00F0L0HF80J03FE0,L01FE07FF80FF01FF007FC03FC03FE03F80J03FC07FC07FC01FF01F80K0HF80J03FE0,L01FE07FF80FF01FF007FC03FC03FE03FC0J07FC07FC07FC01FF03FC0K0HF80J03FE0,L01FE03FF00FF80FF80FF807FC03FE03FE0J0HFC07FC07FC03FE07FE0K0HF80J03FE0,L03FE03FF00FF80FFC1FF807F803FE01FFC0183FF807FC07FC0FFE07FE0K0HF80J03FE0,L03FC03FF00FF807FJF03FLFC1FJFC3FF807FC07FJFC07FE0K0HF80J03FE0,L03FC03FE00FF803FIFE03FLFC0FJFC3FF007FC07FJFC07FE0K0HF80J03FE0,L03FC01FE00FF803FIFC03FLFC07FIFC3FF007FC07FJFH07FE0K0HF80J03FE0,L03FC01FE00FF800FIF803FLFC03FIFC1FE007FC07FIFE003FC0K0HF80J03FE0,L03FC01FC00FF8003FFE003FLFC00FIFC1F8007FC07FIF8003FC0K0HF80J03FE0,L03FC0K0HF80H07F0H03FLFC0H0HFC01E0H07FC03FHFK0F0L0HF80J03FE0,gL03F80H01FC0,::::::::,::::::::::::::::::::::::~DG003.GRF,02560,016,
,:::::::::H07FHFV07FHF,H07FFE0U07FHF,:H07FHFV07FHF,::::::H07FHFL07FHFL07FHF,:H07FHFL07FHFL0JF,H03FHF80J07FHFL0IFE,::H03FHFC0J07FHFK01FHFE,H01FHFC0J07FHFK01FHFC,H01FHFC0J07FHFK03FHFC,H01FHFE0J07FHFK03FHFC,I0IFE0J07FHFK07FHFC,I0JFK07FHFK07FHF8,I0JF80I07FHFK0JF8,I07FHF80I07FHFK0JF0,I07FHFC0I07FHFJ01FIF0,I07FHFE0I07FHFJ03FIF0,I03FIFJ07FHFJ07FHFE0,I01FIF80H07FHFJ0JFC0,I01FIFC0H07FHFI01FIFC0,J0JFE0H07FHFI03FIF80,J0KFI07FHFI07FIF80,J07FIF8007FHFI0KF,J03FIFE007FHFH03FIFE,J01FJF807FHFH0KFC,J01FJFE07FHF03FJFC,K0LF87FHF0FKF8,K07FKF7FHF7FKF0,K03FUFE0,K01FUFC0,L0VF80,L07FTF,L03FSFE,M0TF8,M07FRF0,M03FQFE0,N0RF80,N03FOFE,O0PF8,O03FMFE0,P07FLF,P01FKFC,R0JF80,,:::::::::::::::::::::::::::H07FHFV07FHF,:H07FFE0U07FHF,H07FHFV07FHF,:::::::::H07FHFV0JF,H03FHF80T0IFE,:H03FHF80S01FHFE,H01FHFC0S01FHFC,:H01FHFE0S03FHFC,:I0JFT07FHF8,I0JFT0JF8,I0JF80R0JF8,I07FHFC0Q01FIF0,I07FHFC0Q03FIF0,I03FHFE0Q07FHFE0,I01FIFR07FHFC0,I01FIF80P0JFC0,J0JFC0O01FIF80,J0JFE0O07FIF80,J07FIF80N0KF,J07FIFC0M01FJF,J03FJFN07FIFE,J01FJF80L0KFC,K0LFL07FJF8,K07FJFC0I01FKF0,K03FKFE003FKFE0,K03FUFE0,K01FUFC0,L07FTF,L03FSFE,L01FSFC,M0TF8,M03FQFE0,M01FQFC0,N07FPF,N03FOFE,O07FNF0,O01FMFC0,P03FKFE,Q03FIFE0,R01FFC,,::::::::::::::::~DG004.GRF,01536,012,
,:::::::::::::::::::::::::::::::H0E0,H0F0,H038,:I0E,I070U010,I0380T030,I01C0T070,J0C0T0F0,J070S01C0,J0380R0380,J01C0R07,K0E0R0E,I01C7FF80N01C,I07E3FF80N038,I07F1C380N0F0,I07F1E380N0FE,I07F1F380M03FF80,I03FHFB80M03FFC0,I07FKFM07C7C0,I0H61FLFC001F81E0,I0E61FF9FLF3F3CE0,I0C600380H03FJF3EF0,H01C6001C0J01FHF3EF0,H01860H0E0K01C73CE0,H03860H0780J038799E0,H03860H0380J0707C1E0,H03060H07C0J0E03FFC0,H03060H07E0I01C03FFC0,H03060H05B0I03800FF,H0FC60H059C0H0F0H01F,H0FC60H058E001E0I07,H0EC60H058700380I07,H0EC60H058380380I07,H0EC60H0581C0E0J07,H0EC60H0580E1C0J07,H0EC60H058073C0J07,H0EC60H05803F0K07,H0EC60H05801E0K07,:H0EC60H05803F0K07,H0FC60H07807380J07,H0FC60H0781E1C0J07,H07060J01C0E0J07,H07060J038070J07,H07060J0700380I07,H03060I01C001C0I07,H03060I0380H0E0I07,H03060I070I070I07E0,H03860I0E0I0380H07E0,H03FE0H01C0I01C0H07E0,H03FE0H0380J0E00FFE0,H03FE0H070J01FJF,H03FE0H0E0H01FKF0,I0E6001C3FKFE0,I0E6003FJFE00E0,I07FKFC0J078,I01FHFC0M01C,J0F81C0M01C,J06070O07,:J061E0O03C0,K0380P0E0,K070Q0E0,K0E0Q038,J01C0Q01C,J0380R0E,J070S07,J0E0S0380,I01C0S01C0,I0380T0E0,I070U070,H01C0U030,H0380U010,H070,H0F0,,:::::::::::::::::::~DG005.GRF,02048,016,
,::::::::::::::::::::::::::::::T07F80,T0HFC0,S03FFE4,S0JF3,R03FIF9C0,R0KF9F0,Q03FJFCFC,Q0LFCFE,P03FKFCFF80,P03FKFE7FE0,P03FKFE7FF8,P01FKFE7FFE,K030I01FKFE7FHF80,K0180I0LFE7FHFE0,L0E0I07FJFE7FIF0,L0F80H07FJFE7FIF8,L07C0H03FJFE7FIFC,L07F0H03FJFE7FIFE,L0HFC001FJFE7FJF,K03FFE0H0KFE7FJF,K07FHF800FJF87FJF,J01FIFE007FHFE07FJF80,J07FJFH07FHF807FJF80,I01FKFH03FFE007FJF80,I03FKFH01FF8007FJF80,I07FKFH01FF0H07FJF80,I0MFI0FC0H07FJF80,I0MFI070I07FJF80,H01FLFI040I07FJF80,H01FLFN07FJF80,:::H01FLFL01FMFC,H01FLFM0NFE,H01FLFM03FLF8,H01FLFN0LFE0,H01FLFN07FJF80,H01FKF70M01FIFE,H01FJFC30N07FHF8,H01FJF030N01FHF0,H01FIFC010O07FC0,H01FIFH010O03F,H01FHFC0S0C,H01FHF80,H01FFE,H01FF8,H01FE1E0,H01FC7F0,H01F8FFC,H01E3FFE,H01C7FHF80N07FJF80,H01CFIFE0N07FJF80,H019FJFO07FJF80,H013FJFC0M07FJF80,H017FKFN07FJF80,I07FKF80L07FJF80,I0MFC0L07FJF80,:I0MF80L07FJF80,I0MFN07FJF80,I0MFJ018007FJF80,I0LFE0I070H07FJF80,I0LFE0H01E0H07FJF80,I0LFC0H07E0H07FJF80,I07FJF8001FC0H07FJF80,I03FJF8007FE0H07FJF80,I01FJFH01FHFI07FJF80,J07FIFH03FHFC007FJF80,J03FHFE00FIFE007FJF80,K0IFC03FJF807FJF80,K07FFC07FJFE07FJF,K01FF807FKF07FJF,L0HF807FKFC7FJF,L03F007FLF7FJF,M0F007FLF7FIFE,M06007FLF7FIFC,P07FLF7FIFC,P07FLF7FIF0,P07FLF7FHFE0,P07FKFE7FHF80,P07FKFE7FFE,P07FKFE7FF8,P07FKFE7FE0,P07FKFEFFC0,P07FKFCFF,P07FKFCFC,P07CFJFDF0,P0783FIF9C0,P0700FIF3,P02003FFE4,P020H0HFC8,T03F80,,:::~DG006.GRF,02048,016,
,::::::::H07FF0W03FF,::::::::::H07FgHF,:::::::::,::::::::::H07FgHF,::::::::::H07FF0L01FFC,:::::::::H07FgHF,::::::::::,:::::::::U04,H07FF0L01FFC0L03FF,:::::::::H07FgHF,:::::::::,:::::::::::::::::::::::~DG007.GRF,09216,036,
,::::::::::::K01FgNFE,:::::K01FgNFE0T03FLF0,K01FgNFE0T0NF0,K01FgNFE0S03FMF0,K01FgNFE0S0OF0,K01FgNFE0R03FNF0,K01FgNFE0Q01FOF0,K01FgNFE0Q07FOF0,K01FgNFE0P01FPF0,K01FgNFE0P07FPF0,K01FgNFE0O01FQF0,K01FgNFE0O07FQF0,K01FgNFE0N03FRF0,K01FgNFE0N0TF0,K01FgNFE0M03FSF0,K01FgNFE0M0UF0,K01FgNFE0L03FTF0,K01FgNFE0K01FUF0,K01FgNFE0K07FUF0,K01FgNFE0J01FVF0,::K01FgNFE0K0WF0,::V07FMFW0WF0,:V07FMFW07FUF0,::::V07FMFW03FLFBFMF0,V07FMFW03FKFE3FMF0,V07FMFW03FKF83FMF0,V07FMFW03FJFE03FMF0,V07FMFW01FJF803FMF0,V07FMFW01FIFE003FMF0,V07FMFW01FIF8003FMF0,V07FMFW01FHFE0H03FMF0,V07FMFW01FHF80H03FMF0,V07FMFX0HFC0I03FMF0,V07FMFX0HFK03FMF0,V07FMFX0FC0J03FMF0,V07FMFX0F0K03FMF0,V07FMFX040K03FMF0,V07FMFgK03FMF0,:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::,:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::~DG008.GRF,02304,012,
,:::::::::::::::::::::::::::::::::::::R01F0H07C0,R03F800FF0,R07FC01FF0,R0HFE03FF8,:::R07FC01FF0,R03F800FF0,R01F0H07C0,,:::R0FC,:::::::R0MF8,::::::::R0FC,:::::::,::T03FC0,S01FFC0,S07FFC1F0,S0IFC3F0,R01FHFC3F0,R03FHFC1F0,R03FHFC1F8,R07FHFC1F8,R07F8FC1F8,R0HF0FC1F8,R0FE0FC1F8,R0FC0FC1F8,::R0FC0FC3F8,:R07E0FC3F0,R07F0FC7F0,R07FCFCFF0,R03FKF0,R03FJFE0,R01FJFE0,S0KFC0,S07FIF80,S03FIF,T0IFC,U0FE0,,:::V01E,S07807F80,R01FE0FFC0,R01FF1FFC0,R03FF9FFE0,R07FF9FFE0,R07FKF0,::R07FHFE3F0,R0FE3FC1F8,R0FC1FC1F8,R0F80F80F8,:::::R0MF8,::::::R07FKF8,R07FKF0,,:::W01FHFE,W03FHFE,::O07FPFE,::O07FNF8,:::::W03F8,::::::::::::O07FNF8,::::::::,::::::::::::::~DG009.GRF,56832,148,
,::::::::::::::::::::::::::::::::::::J03FJFE0jJ07F0K03F80J01FC0R01FF0gN0HFY03FE,J03FJFE0jI03FFE0I01FHFK0IF80Q0IFC0gL0IFE0V01FHF80,J03FJFE0jI0JF80H07FHFC0H03FHFE0P03FHFE0gL0JF80U07FHFC0hN01FE0H01FHFE0H01FF0H07F80I01FF0H01F8,J03FJFE0jH01FIFC0H0JFE0H07FIFQ0JFE0gL0JFC0T01FIFC0hM01FHF8003FHFE0H03FF007FFE0I03FF0H07FE,J03FJFE0jH03FIFE001FJFI0KF80N01FIFC0gL07FHFE0T03FIF80hM03FHFE003FHFE0H03FF00FIF80H03FF001FHF80,J03F800FE0jH07FC1FF003FE0FF801FF07FC0N03FF00C0gL0703FF0T07FE0180hM03FIFH03FHFE0H07FF00FIFC0H07FF003FHFC0,J03F800FE0jH07F80FF003FC07F801FE03FC0N03FC0gO0400FF80S07F80hP01FIFH03FHFE0H0IFH07FHFC0H0IFH03FHFC0,J03F800FE0jH0HFH07F807F803FC03FC01FE0N07F80gR07F80S0HFhR01E0FF803E0K0IFH0783FE0H0IFH07F0FE0,J03F800FE0jH0FE003FC07F001FE03F800FF0N07F0gS03FC0S0FE0hQ01007F803E0J01F7F00401FE001F7F007F0FE0,J03F800FE07E0780I07C0H0FE07FC00FE003FFE0I03E0I0JFC0FE07FC7FIFH01F8007FHFE07F0I01E0H01FC001FC0FE0H0FE07F0H07F0N0HFK0HF807FC007F0H0FC0F003FIF80H01FC0FE07FC00F8001FE0I03F81FF0H03E0FIFE003F0H0HF807FC007F0U03F803E0J01F7F0J0FE001F7F00FE07F0,J03F800FE07E3FE0H03FF800FE07FC0FHF807FHFC001FFC0H0JFC0FE07FC7FIFH0HFE007FHFE07F0I03F0H01FC001FC0FE0H0FE07F0H07F0N0FE0J0HF80FFC07FFE00FC7FC03FIF80H01FC0FE0FF007FF001FC0I03F81FF003FFCFIFE01FFC00FF80FFC07FFE0T03F807E0J03F7F0J0FE003F7F00FE07F0,J03F800FE07EFHF800FHFE00FE0FFC3FHFC07FIFH07FHFI0JFC0FE0FFC7FIF01FHFH07FHFE07F0I07F8001FC001FC0FE0H0FE07F0H07F0I03E1F0FE0I01FF80FFC0FIFH0FDFHF03FIF80I0FC0FE0FE01FHFC01FC0I03F83FF007FFCFIFE03FFE01FF80FFC0FIF03E1F0O03F807E0J07E7F0J0FE007E7F00FE03F0,J03F800FE07FIFC01FIFH0FE0FFC1FHFE07FIFH0JF800FIFC0FE0FFC7FIF03FHFC07FHFE07F0I07F8003F80H0FC1FC0H07E0FE0H03F0I03C3E1FC0I01FFC0FFC0FIF80FJF83FIF80I0FC0FE1FE03FHFE03F80I03F83FF01FHF8FIFE07FHF81FFC0FFC0FIF81F0F0O07F007FF0I07E7F0I01FC007E7F00FC03F0,J03F800FE07FIFC03FIF80FE1FFC1FHFE07F07F81FIFC00FIFC0FE1FFC7FIF07FHFC07FHFE07F0I07F8003F80H0FE1FC0H07F0FE0H03F80H07C3E1FC0I01FFC1FFC0FIFC0FJF83FIF80I0FE0FE1FC07FIF03F80I03F87FF01FHF8FIFE0FIF81FFC1FFC0FIFC1F0F80N0HFH07FFE0H0FC7F0I03FC00FC7F01FC03F8,J03F800FE07FC3FE03FC3F80FE1FFC1C0FF07F03F81FE1FC00FC1FC0FE1FFC01FC00FE0FE07E0FE07F0I07F8003F80H0FE1FC0H07F0FE0H03F80H0F87C1FC0I01FFC1FFC0703FC0FF87FC00FE001FIFE0FE3F807F87F03F80I03F87FF03FC0803F801FC1FC1FFC1FFC0703FC0F87C0L03FFE007FHF800FC7F0H0IF800FC7F01FC03F8,J03F800FE07F80FE07F01FC0FE1EFC0H07F07F03F83F80FE00FC1FC0FE1EFC01FC01FC07E07E0FE07F0I03F0H03F80H0FE1FC0H07F0FE0H03F80H0F0781FC0I01FFC1EFC0401FC0FF01FC00FE001FIFE0FE7F00FE03F83F80I03F87BF07F80H03F803F80FC1FFC1EFC0401FC0787C0L03FF8007FHFC01F87F0H0HFE001F87F01FC03F8,J03F800FE07F80FE07F01FC0FE3FFC0H07F07F03F83F80FE00FC1FC0FE3FFC01FC01F803E07E0FE07FFE001E0H03F80H0FE1FC0H07F0FE0H03F8001F0F81FC0I01FBE3EFC0I0FE0FF01FC00FE001FIFE0FE7E00FE03F83F80I03F8FHF07F0I03F803F007C1FBE3EFC0I0FE07C3E0L03FF0H07FHFE03F07F0H0HFC003F07F01FC03F8,J03F800FE07F007F07E00FC0FE3DFC0H0FE07F07F03F007E00FC1FC0FE3DFC01FC01F803F07E0FE07FHFC0K03F80H0FE1FC0H07F0FE0H03F8001E1F01FC0I01FBE3EFC001FFE0FE00FE00FE001FIFE0FEFC00FC01F83F80I03F8F7F07F0I03F803F007E1FBE3EFC001FFE03E1E0L03FFE0I07FE03F07F0H0IF803F07F01FC03F8,J03F800FE07F007F0FE00FE0FE7DFC07FFC07FHFE07F007F00FC1FC0FE7DFC01FC03FJF07E0FE07FHFE0K03F80H0FE1FC0H07F0FE0H03F8003E1E01FC0I01FBE3EFC01FHFE0FE00FE00FE001FIFE0FHFC01FC01FC3F80I03F9F7F0FE0I03F807FIFE1FBE3EFC01FHFE01E1F0L03FHFJ01FE07E07F0H0IFC07E07F01FC03F8,J03F800FE07F007F0FE00FE0FE7DFC07FF007FHFC07F007F00FC1FC0FE7DFC01FC03FJF07E0FE07FIFL03F80H0FE1FC0H07F0FE0H03F8007C3E01FE0I01FBF7CFC07FHFE0FE00FE00FE0K0FE0FHFE01FC01FC3FC0I03F9F7F0FE0I03F807FIFE1FBF7CFC07FHFE01F0F80M0HF80I0HF07E07F0I03FE07E07F01FC03F8,J03F800FE07F007F0FE00FE0FE79FC07FF807FHFC07F007F01F81FC0FE79FC01FC03FJF07E0FE07FIF80J01F8001FC0FC0H0FE07E0H07F0H07C3E00FE0I01F9F7CFC0FIFE0FE00FE00FE0K0FE0FIF01FC01FC1FC0I03F9E7F0FE0I03F807FIFE1F9F7CFC0FIFE01F0F80M03F80I07F0FJFE0I0FE0FJFE1FC03F8,J03F800FE07F007F0FE00FE0FEF9FC07FFE07FIF07F007F01F81FC0FEF9FC01FC03FJF07E0FE07F07FC0J01FC001FC0FE0H0FE07F0H07F0H03E1E00FF0I01F9F7CFC1FF0FE0FE00FE00FE0J01FC0FIF81FC01FC1FE0I03FBE7F0FE0I03F807FIFE1F9F7CFC1FF0FE01E1F0N01FC0I07F0FJFE0I07F0FJFE0FC03F0,J03F800FE07F007F0FE00FE0FEF9FC0H0HF07F07F87F007F01F81FC0FEF9FC01FC03FJF07E0FE07F01FC0J01FC001FC0FE0H0FE07F0H07F0H01E1F00FF0I01F9F78FC1FC0FE0FE00FE00FE0J01FC0FIFC1FC01FC1FE0I03FBE7F0FE0I03F807FIFE1F9F78FC1FC0FE03E1E0N01FC0I07F0FJFE0I07F0FJFE0FC03F0,J03F800FE07F007E07E00FE0FEF1FC0H07F07F01FC3F007F03F81FC0FEF1FC01FC01F80I0FE0FE07F01FC0J01FE003FC0FF001FE07F800FF0H01F0F807F80H01F9FF8FC3F80FE0FE00FC00FE0J03FC0FE3FC0FC01FC0FF0I03FBC7F07F0I03F803F0I01F9FF8FC3F80FE07C3E0010K01FC0I07F0FJFE0I07F0FJFE0FE07F0H080,J03F800FE07F80FE07F01FC0FHF1FC0H07F87F01FC3F80FE03F81FC0FHF1FC01FC01FC0I0FC0FE07F01FC1E0I0FE007F807F003FC03F801FE0I0F07807FC0H01F8FF8FC3F80FE0FF01FC00FE0J07F80FE1FE0FE03F80FF80H03FFC7F07F0I03F803F80H01F8FF8FC3F80FE0783C07F0K01FC0I0FE0FJFE0I07F0FJFE0FE07F03F80,J03F800FE07F81FE07F01FC0FDE1FC2007F87F01FC3F80FE07F01FC0FDE1FC01FC01FE0I0FC0FE07F01F83F0I07F80FF803FC07FC01FE03FE0I0F87C03FF80C1F8FF0FE3F81FE0FF03FC00FE0J0HF80FE0FE0FE03F807FF0183F787F07F80H03F803FC0H01F8FF0FE3F81FE0F87C07E0H02003F80800FE0I07F00800FE0I07F007F0FE03F,J03F800FE07FC3FC03FC3F80FFE1FC380FF87F01FC1FE1FC07F01FC0FFE1FC01FC00FF0043FC0FE07F07F87F80H07FC1FF003FE0FF801FF07FC0I07C3C03FIFC3F0FF0FE3FC3FE0FF87F800FE00603FF00FE0FE07F87F007FIF83FF87F07FC0803F801FE0083F0FF0FE3FC3FE0F0F807E0H03C07F80E03FE0I07F00F01FE0I07F007F0FE03F,J03F800FE07FIFC03FIF80FFC1FC3FIF07F07FC1FIFC3FKF0FFC1FC01FC00FIFC7FC0FE07FIF87F80H03FIFE001FJFI0KF80I03C3E01FIFC3F0FF0FE1FIFE0FJF800FE00FIFE00FE0FF07FIFH03FIF83FF07F03FHF803F801FIF83F0FF0FE1FIFE1F0F007C0H03FIF80FIFC0I07F00FIFE0I07F003FHFC03E,J03F800FE07FIF801FIFH0HFC1FC7FIF07FIF80FIF83FKF0FFC1FC01FC007FHFC7F80FE07FIF07F80H01FIFC0H0JFE0H07FIFK03E1F00FIFC3F07F0FE1FIFE0FJFI0FE00FIFC00FE07F03FHFE001FIF83FF07F01FHF803F800FIF83F07F0FE1FIFE3E1F00FC0H03FIFH0JF80I07F00FIFC0I07F003FHFC07E,J03F800FE07FIFI0IFE00FFC1FC7FHFC07FIFH07FHF03FKF0FFC1FC01FC003FHFE7F00FE07FHFE07F80I0JF80H07FHFC0H03FHFE0P03FHFE3F07E0FE0FHF7E0FIFE0H0FE00FIF800FE07F81FHFC0H07FHFC3FF07F00FHFC03F8007FHFC3F07E0FE0FHF7E0K0FC0H07FHFE01FIFK07F01FIF80I07F001FHF807E,J03F800FE07F7FE0H03FF800FF81FC1FHF807FHFC001FFC03FKF0FF81FC01FC0H0IFE3E00FE07FHF803F0J03FFE0I01FHFK0IF80P01FHFE3F07E0FE07FC7E0FEFFC0H0FE00FHFE0H0FE03F807FF0I03FHFC3FE07F003FFC03F8001FHFC3F07E0FE07FC7E0K0F80H03FHF801FHFC0J07F00FHFE0J07F0H07FE007C,J03F800FE07F0F0J07C0H0HF81FC03FC007FFC0I03E003FKF0FF81FC01FC0I0HF03800FE03FF0H01E0K0HFL07F80J03FC0R01FE03F03E0FE01F07E0FE1E0I0FE001FE0I0FE03FC00F80J03FC03FE07F0H07F003F80H01FE03F03E0FE01F07E0K0F80I03FC0H01FC0K07F0H0HFL07F0H01F8007C,R07F0gU03F0H03F0jO0FE0iW01F0hH0F8,:R07F0gU03F0H03F0jO0FE0iW01E0hH0F0,R07F0gU03F0H03F0jO0FE0iW03E0hG01F0,R07F0gU03F0H03F0jO0FE0iW03C0hG01E0,R07F0gU03F0H03F0jO0FE0,:R07F0lR0FE0,:,::::::::::::::lJ0FE,J03F803FE0iP0F0780gI0F0780O0FE0K03FJF80M07FC0iG01FF0,J03F807FC0iP0F0780gI0F0780O0FE0K03FJF80L03FHFiH0IFC,J03F807F80iP0F8F0gJ0F8F0O03FF80J03FJF80L0JF80hY03FHFE,J03F80FF0iQ07FF0gJ07FF0N01FIF80I03FJF80K03FIF80hY0JFE,J03F81FE0iQ03FE0gJ03FE0N07FIFC0I03FJF80K07FIFi01FIFC,J03F81FE0iQ01F80gJ01F80N0LFJ03FJF80K0HFC030hY03FF00C,J03F83FC0kP01FKF80J03F80M0HFiI03FC0,J03F83F80kP03FCFE7FC0J03F80L01FE0iH07F80,J03F87F0kQ07F8FE3FC0J03F80L01FC0iH07F,J03F8FF003F03C0H01FC0I01F01FC07F0H03E0I0JFC007F0H0FC0F0J07C07F03FE7F03FE0FE07FC0H0FE07FCFC0F0I07F0H0FE07FC0K07F0FE1FE0J03F80L03FC0I07F03FE0FC0F0H0FE07FCFF00FF0H0F80K07FIFC1FHFEFIFE0K0HFJ01FC0FF83F03C003F81FF3FC03FC003E0K01FJF07E0780I07C0I01F80H07F0H07FHFE0,J03F8FE003F1FF001FHF8001FFE1FC07F001FFC0H0JFC07FFE00FC7FC0H07FF87F07F87F03FE0FE07FC0H0FE0FF0FC7FC007FFE00FE07FC0K0FE0FE0FE0J03F80L03F80I07F03FE0FC7FC00FE07FC7F00FE00FHFL07FIFC1FHFEFIFE0K0FE0I01FC0FF83F1FF003F81FF1FC03F803FFC0J01FJF07E3FE0H03FF80H0HFE007FFC007FHFE0,J03F9FC003F7FFC03FHFC003FFE1FC07F007FHFI0JFC0FIFH0FDFHFI0IF87F07F07F07FE0FE0FFC0H0FE0FE0FDFHFH0JFH0FE0FFC0K0FE0FE0FE0J03F80L03F80I07F07FE0FDFHFH0FE0FFC7F80FE01FHFL07FIFC1FHFEFIFE0K0FE0I01FC1FF83F7FFC03F83FF1FE03F807FFC0J01FJF07EFHF800FHFE001FHF01FHFE007FHFE0,J03F9FC003FIFE03FHFE00FHFC1FC07F00FIF800FIFC0FIF80FJF803FHF07F0FF07F07FE0FE0FFC0H0FE1FE0FJF80FIF80FE0FFC0J01FC0FE07F0J03F80L07F0J07F07FE0FJF80FE0FFC3F80FE07FFE0K07FIFC1FHFEFIFE0J01FC0I01FC1FF83FIFE03F83FF0FE03F81FHF80J01FJF07FIFC01FIFH03FHFC0FIFH07FHFE0,J03FBF8003FIFE03FIFH0IFC1FC07F01FIFC00FIFC0FIFC0FJF803FHF07F0FE07F0FFE0FE1FFC0H0FE1FC0FJF80FIFC0FE1FFC0J01FC0FE07F0J03F80L07F0J07F0FFE0FJF80FE1FFC3F81FC07FFE0K07FIFC1FHFEFIFE0J01FC0I01FC3FF83FIFE03F87FF0FE07F01FHF80J01FJF07FIFC03FIF807FHFC0FIFH07FHFE0,J03FHFI03FE1FF01C0FF01FE041FC07F01FE1FC00FC1FC0703FC0FF87FC07F8107F1FC07F0FFE0FE1FFC0H0FE3F80FF87FC0703FC0FE1FFC0J01FC0FE07F0J03F80L07F0J07F0FFE0FF87FC0FE1FFC3FC1FC0FF020K07F01FC1FC0H03F80K01FC0I01FC3FF83FE1FF03F87FF0FF07F03FC080J01FC07F07FC3FE03FC3F80FE0FE0E07F807E0FE0,J03FHF8003FC07F01007F03FC001FC07F03F80FE00FC1FC0401FC0FF01FC0FF0H07F3F807F0F7E0FE1EFC0H0FE7F00FF01FC0401FC0FE1EFC0J01FC0FE07F0J03F80L07F0J07F0F7E0FF01FC0FE1EFC1FC1FC1FE0M07F01FC1FC0H03F80K01FC0I01FC3DF83FC07F03F87BF07F07F07F80L01FC07F07F80FE07F01FC1FC07E0H03F807E0FE0,J03FHFE003FC07F0I03F83F8001FC07F03F80FE00FC1FC0I0FE0FF01FC0FE0H07F3F007F1FFE0FE3FFC0H0FE7E00FF01FC0I0FE0FE3FFC0J01FC0FE07F0J03F80L07F0J07F1FFE0FF01FC0FE3FFC1FC3F81FC0M07F01FC1FC0H03F80K01FC0I01FC7FF83FC07F03F8FHF07F0FE07F0M01FC07F07F80FE07F01FC1F803E0H03F807E0FE0,J03FIFH03F803F8007FF83F8001FC07F03F007E00FC1FC001FFE0FE00FE0FE0H07F7E007F1EFE0FE3DFC0H0FEFC00FE00FE001FFE0FE3DFC0J01FC0FE07F0J03F80L07F0J07F1EFE0FE00FE0FE3DFC0FE3F81FC0M07F01FC1FC0H03F80K01FC0I01FC7BF83F803F83F8F7F03F8FE07F0M01FC07F07F007F07E00FC1F803F0H07F007E0FE0,J03FIF803F803F807FHF87F0H01FJF07F007F00FC1FC01FHFE0FE00FE1FC0H07FFE007F3EFE0FE7DFC0H0IFC00FE00FE01FHFE0FE7DFC0J01FC0FE07F0J03F80L07F0J07F3EFE0FE00FE0FE7DFC0FE3F03F80M07F01FC1FC0H03F80K01FC0I01FCFBF83F803F83F9F7F03F8FC0FE0M01FC07F07F007F0FE00FE3FJF03FFE007E0FE0,J03F87FC03F803F81FIF87F0H01FJF07F007F00FC1FC07FHFE0FE00FE1FC0H07FHFH07F3EFE0FE7DFC0H0IFE00FE00FE07FHFE0FE7DFC0J01FC0FE07F0J03F80L07F80I07F3EFE0FE00FE0FE7DFC0FE3F03F80M07F01FC1FC0H03F80K01FE0I01FCFBF83F803F83F9F7F03F8FC0FE0M01FC07F07F007F0FE00FE3FJF03FF8007E0FE0,J03F83FC03F803F83FIF87F0H01FJF07F007F01F81FC0FIFE0FE00FE1FC0H07FHF807F3CFE0FE79FC0H0JFH0FE00FE0FIFE0FE79FC0K0FE0FE0FE0J03F80L03F80I07F3CFE0FE00FE0FE79FC07E7F03F80M07F01FC1FC0H03F80L0FE0I01FCF3F83F803F83F9E7F01F9FC0FE0M01FC07F07F007F0FE00FE3FJF03FFC00FC0FE0,J03F81FE03F803F87FC3F87F0H01FJF07F007F01F81FC1FF0FE0FE00FE1FC0H07FHFC07F7CFE0FEF9FC0H0JF80FE00FE1FF0FE0FEF9FC0K0FE0FE0FE0J03F80L03FC0I07F7CFE0FE00FE0FEF9FC07F7E03F80M07F01FC1FC0H03F80L0HFJ01FDF3F83F803F83FBE7F01FDF80FE0M01FC07F07F007F0FE00FE3FJF03FHFH0FC0FE0,J03F80FE03F803F87F03F87F0H01FJF07F007F01F81FC1FC0FE0FE00FE1FC0H07FHFE07F7CFE0FEF9FC0H0JFC0FE00FE1FC0FE0FEF9FC0K0HF0FE1FE0J03F80L03FC0I07F7CFE0FE00FE0FEF9FC03F7E03F80M07F01FC1FC0H03F80L0HFJ01FDF3F83F803F83FBE7F00FDF80FE0M01FC07F07F007F0FE00FE3FJFI07F80FC0FE0,J03F80FE03F803F0FE03F83F8001FC07F03F007F03F81FC3F80FE0FE00FC0FE0H07F1FE07F78FE0FEF1FC0H0FE3FC0FE00FC3F80FE0FEF1FC0040H07F8FE3FC078003F8003C0H01FE0I07F78FE0FE00FC0FEF1FC03F7E01FC0I020H07F01FC1FC0H03F800F0I07F80H01FDE3F83F803F03FBC7F00FDF807F0J08001FC07F07F007E07E00FE1F80K03F81FC0FE0,J03F80FF03FC07F0FE03F83F8001FC07F03F80FE03F81FC3F80FE0FF01FC0FE0H07F0FF07FF8FE0FHF1FC0H0FE1FE0FF01FC3F80FE0FHF1FC1FC0H03FCFE7F80FC003F8007E0H01FF0I07FF8FE0FF01FC0FHF1FC03FFC01FC0H0FE0H07F01FC1FC0H03F801F80H07FC0H01FFE3F83FC07F03FFC7F00FHFH07F0H03F8001FC07F07F80FE07F01FC1FC0K03FC1FC0FE0,J03F807F03FC0FF0FE07F83FC001FC07F03F80FE07F01FC3F81FE0FF03FC0FF0H07F07F07EF0FE0FDE1FC0H0FE0FE0FF03FC3F81FE0FDE1FC1F80H03FKF80FC003F8007E0I0HFE0307EF0FE0FF03FC0FDE1FC01FFC01FE0H0FC0H07F01FC1FC0H03F801F80H03FF80C1FBC3F83FC0FF03F787F007FF007F8003F0H01FC07F07F81FE07F01FC1FE0H01003FC3F80FE0,J03F807F03FE1FE0FF0FF83FE041FC07F01FE1FC07F01FC3FC3FE0FF87F80FF8107F07F07FF0FE0FFE1FC0H0FE0FE0FF87F83FC3FE0FFE1FC1F80H01FKF01FE003F800FF0I0KF07FF0FE0FF87F80FFE1FC01FFC01FF020FC0H07F01FC1FC0H03F803FC0H03FIFC1FFC3F83FE1FE03FF87F007FF007FC083F0H01FC07F07FC3FC03FC3F80FF0041C07FC3F80FE0,J03F807F83FIFE07FIF81FHFC1FC07F01FIFC3FKF1FIFE0FJF807FHF07F07F87FE0FE0FFC1FC0H0FE0FF0FJF81FIFE0FFC1FC1F0J07FIFC01FE003F800FF0I07FIF07FE0FE0FJF80FFC1FC00FF800FHFE0F80H07F01FC1FC0H03F803FC0H01FIFC1FF83F83FIFE03FF07F003FE003FHF83E0H01FC07F07FIFC03FIF80FIFC1FIF9FKF8,J03F803F83FIFC07FIF80FHFC1FC07F00FIF83FKF1FIFE0FJFH03FHF07F03F87FE0FE0FFC1FC0H0FE07F0FJF01FIFE0FFC1FC3F0J03FIFH01FE003F800FF0I03FIF07FE0FE0FJFH0HFC1FC00FF8007FFE1F80H07F01FC1FC0H03F803FC0I0JFC1FF83F83FIFC03FF07F003FE001FHF87E0H01FC07F07FIF801FIFH07FHFC3FIF9FKF8,J03F803FC3FIF803FFDF807FFE1FC07F007FHF03FKF0FHF7E0FIFE001FHF87F03FC7FE0FE0FFC1FC0H0FE07F8FIFE00FHF7E0FFC1FC3F0K07FF8001FE003F800FF0J0JF87FE0FE0FIFE00FFC1FC00FF0H03FHF1F80H07F01FC1FC0H03F803FC0I03FHFE1FF83F83FIF803FF07F003FC0H0IFC7E0H01FC07F07FIFI0IFE003FHFE3FHFE1FKF8,J03F803FC3FBFF001FF1F801FFE1FC07F001FFC03FKF07FC7E0FEFFC0H07FF87F01FC7FC0FE0FF81FC0H0FE03F8FEFFC007FC7E0FF81FC3E0L0FE0I0FC003F8007E0J07FHF87FC0FE0FEFFC00FF81FC007F0I0IF1F0I07F01FC1FC0H03F801F80I01FHFE1FF03F83FBFF003FE07F001FC0H03FFC7C0H01FC07F07F7FE0H03FF80H0IFE0FHFC1FKF8,J03F801FC3F8780H07C1F8003F81FC07F0H03E003FKF01F07E0FE1E0J0FE07F01FE7FC0FE0FF81FC0H0FE03FCFE1E0H01F07E0FF81FC3E0L0FE0I078003F8003C0K07F807FC0FE0FE1E0H0HF81FC00FE0I01FC1F0I07F01FC1FC0H03F800F0K01FE01FF03F83F878003FE07F003F80I07F07C0H01FC07F07F0F0J07C0J0HFH01FE01FKF8,Q03F80gM03F0H03F0L0FE0gW0FE0V07C0L0FE0gQ0FE0R0FE0L03E0gX03F80Q03F80L0F80O07F0gG01F8001F8,Q03F80gM03F0H03F0L0FE0gW0FE0V07C0h0FE0Q01FC0L03E0gX03F80Q07F0M0F80O07F0gG01F8001F8,Q03F80gM03F0H03F0L0FE0gW0FE0V0780h0FE0Q03FC0L03C0gX03F80Q0HFN0F0P07F0gG01F8001F8,Q03F80gM03F0H03F0L0FE0gW0FE0V0F80h0FE0P03FF80L07C0gX03F80P0HFE0L01F0P07F0gG01F8001F8,Q03F80gM03F0H03F0L0FE0gW0FE0V0F0hG0FE0P01FF80L0780gX03F80P07FE0L01E0P07F0gG01F8001F8,Q03F80gM03F0H03F0L0FE0gW0FE0hY0FE0P01FF0hN03F80P07FC0Y07F0gG01F8001F8,Q03F80gM03F0H03F0L0FE0gW0FE0hY0FE0P01FE0hN03F80P07F80Y07F0gG01F8001F8,Q03F80hG0FE0gW0FE0hY0FE0P01F80hN03F80P07E0g07F,Q03F80hG0FE0gW0FE0hY0FE0Q0F0hO03F80P03C0g07F,nR080iI02,,:::::::::::::hG03F0jT03F,J0KFE0gP03F0gX0783C0hQ03F,:J0KFE0gP03F0gX07C780gR07F0V03F0Y07F0M07E0I03F8007FIFC0,J0KFE0gP03F0gX03FF80gQ01FF0V03F0X01FF0L01FF8001FHFH07FIFC0,J0KFE0gP03F0gX01FF0gR07FF0V03F0X07FF0L07FFE00FIF807FIFC0,J0KFE0gP03F0gY0FC0gQ01FHFW03F0W01FHFM0JFH0JFC07FIFC0,L0FE0gR03F0iS03FHFW03F0W03FHFM0JFH07FHFE07FIFC0,L0FE0gR03F0iS01FHFW03F0W01FHFL01FC3F807C1FE0I03F80,L0FE0gR03F0iS01FHFW03F0W01FHFL01FC3F80600FE0I03F80,L0FE001F81E001FC0FF9FE01FE1FF00FF80H0HFC0I07F0H07FHFE07F0I03F80FE07F0I0FE07F03FE0L0JFC0H0F80H0HF807FC0H01C7F0O0F80J0HFC0H0FE07FC0H0F80H01C7F0K03F81FC04007F0I07F,L0FE001F8FF801FC0FF8FE01FC1FF01FF800FIF8007FFE007FHFE07F0I03F80FE07F0I0FE07F03FE0L0JFC007FF0H0HF80FFC0H0107F0N07FF0I0JF800FE07FC00FHFI0107F0K03F81FC0I07F0I07F,L0FE001FBFFE01FC1FF8FF01FC3FF01FF803FIFE00FIFH07FHFE07F0I03F80FE07F0I0FE07F07FE0L0JFC01FHFC01FF80FFC0J07F0M01FHFC003FIFE00FE0FFC01FHFK07F0K03F80FC0I07F0I0FE,L0FE001FJF01FC1FF87F01FC3FF81FF807FJFH0JF807FHFE07F0I03F80FE07F0I0FE07F07FE0L0JFC03FHFE01FFC0FFC0J07F0M03FHFE007FJFH0FE0FFC07FFE0J07F0K03F00FC0I07F0I0FE,L0FE001FJF01FC3FF87F03F83FF83FF80FKF80FIFC07FHFE07F0I03F80FE07F0I0FE07F0FFE0L0JFC07FIF01FFC1FFC0J07F0M07FIFH0LF80FE1FFC07FFE0J07F0K07F00FE0I07F0H01FC,L0FE001FF0FF81FC3FF87F83F83FF83FF81FE3F3FC0703FC07E0FE07F0I03F80FE07F0I0FE07F0FFE0L0FC1FC07F87F01FFC1FFC0J07F0M07F87F01FE3F3FC0FE1FFC0FF020J07F0K07F00FE0I0FE0H01FC,L0FE001FE03F81FC3DF83F83F83FF83DF83FC3F1FE0401FC07E0FE07F0I03F80FE07F0I0FE07F0F7E0L0FC1FC0FE03F81FFC1EFC0J07F0M0FE03F83FC3F1FE0FE1EFC1FE0L07F0K07F00FE0I0FE0H03F8,L0FE001FE03F81FC7FF83F87F03F7C7DF83F83F0FE0I0FE07E0FE07FFE003F80FE07FFE00FE07F1FFE0L0FC1FC0FE03F81FBE3EFC0J07F0M0FE03F83F83F0FE0FE3FFC1FC0L07F0K07F00FE0H01FC0H03F8,L0FE001FC01FC1FC7BF81FC7F03F7C7DF87F83F0FF001FFE07E0FE07FHFC03F80FE07FHFC0FE07F1EFE0L0FC1FC0FC01F81FBE3EFC0J07F0M0FC01F87F83F0FF0FE3DFC1FC0L07F007FF87F00FE0H03FC0H07F0,L0FE001FC01FC1FCFBF81FC7E03F7C7DF87F03F07F01FHFE07E0FE07FHFE03FIFE07FHFE0FE07F3EFE0L0FC1FC1FC01FC1FBE3EFC0J07F0L01FC01FC7F03F07F0FE7DFC3F80L07F007FF87F00FE0H03F80H07F0,L0FE001FC01FC1FCFBF81FC7E03F7EF9F87F03F07F07FHFE07E0FE07FIF03FIFE07FIF0FE07F3EFE0L0FC1FC1FC01FC1FBF7CFC0J07F0L01FC01FC7F03F07F0FE7DFC3F80L07F007FF87F00FE0H07F80H07F0,L0FE001FC01FC1FCF3F80FCFE03F3EF9F87F03F07F0FIFE07E0FE07FIF83FIFE07FIF8FE07F3CFE0K01F81FC1FC01FC1F9F7CFC0J07F0L01FC01FC7F03F07F0FE79FC3F80L07F007FF87F00FE0H0HFJ0FE0,L0FE001FC01FC1FDF3F80FEFC03F3EF9F87F03F07F1FF0FE07E0FE07F07FC3FIFE07F07F8FE07F7CFE0K01F81FC1FC01FC1F9F7CFC0J07F0L01FC01FC7F03F07F0FEF9FC3F80L07F007FF83F00FC001FE0I0FE0,L0FE001FC01FC1FDF3F807EFC03F3EF1F87F03F07F1FC0FE07E0FE07F01FC3FIFE07F01FCFE07F7CFE0K01F81FC1FC01FC1F9F78FC0J07F0L01FC01FC7F03F07F0FEF9FC3F80L07F0K03F00FC003FC0H01FC0,L0FE001FC01F81FDE3F807EFC03F3FF1F87F83F0FF3F80FE0FE0FE07F01FC3F80FE07F01FCFE07F78FE0020H03F81FC0FC01FC1F9FF8FC0J07F0I040H0FC01FC7F83F0FF0FEF1FC1FC0L07F0K03F81FC007F80H01FC0,L0FE001FE03F81FFE3F807FF803F1FF1F83F83F0FE3F80FE0FC0FE07F01FC3F80FE07F01FCFE07FF8FE0FE0H03F81FC0FE03F81F8FF8FC0J07F001FC0H0FE03F83F83F0FE0FHF1FC1FC0L07F0K03F81FC00FF0I03F80,L0FE001FE07F81FBC3F803FF803F1FE1FC3FC3F1FE3F81FE0FC0FE07F01F83F80FE07F01F8FE07EF0FE0FC0H07F01FC0FE03F81F8FF0FE0J07F001F80H0FE03F83FC3F1FE0FDE1FC1FE0L07F0K01FC3F801FE0I03F80,L0FE001FF0FF01FFC3F803FF807E1FE1FC1FE3F3FC3FC3FE3FC0FE07F07F83F80FE07F07F8FE07FF0FE0FC0H07F01FC07F87F03F0FF0FE0J07F001F80H07F87F01FE3F3FC0FFE1FC1FF020J07F0K01FC3F803FC0I07F,L0FE001FJF01FF83F801FF007E1FE1FC0FKF81FIFE7FC0FE07FIF83F80FE07FIF8FE07FE0FE0F8003FKF07FIF03F0FF0FE0J07F001F0I07FIFH0LF80FFC1FC0FHFE0J07F0L0JFH0KF807F,L0FE001FIFE01FF83F801FF007E0FE1FC07FJF81FIFE7F80FE07FIF03F80FE07FIF0FE07FE0FE1F8003FKF03FHFE03F07F0FE0J07F003F0I03FHFE007FJF80FFC1FC07FFE0J07F0L0JF01FJF80FE,L0FE001FIFC01FF83F801FE007E0FC1FC03FIFE00FHF7E7F00FE07FHFE03F80FE07FHFE0FE07FE0FE1F8003FKF01FHFC03F07E0FE0J07F003F0I01FHFC003FIFE00FFC1FC03FHFK07F0L07FFE01FJF80FE,L0FE001FDFF801FF03F800FE007E0FC1FC00FIF8007FC7E3E00FE07FHF803F80FE07FHF80FE07FC0FE1F0H03FKFH07FF003F07E0FE0J07F003E0J07FF0I0JF800FF81FC00FHFK07F0L01FF801FJF81FC,L0FE001FC3C001FF03F801FC007E07C1FC0H0HFC0H01F07E3800FE03FF0H03F80FE03FF800FE07FC0FE1F0H03FKFI0F8003F03E0FE0J07F003E0K0F80J0HFC0H0HF81FC001FC0J07F0M07E001FJF83FC,P01FC0Q01FC0R03F0hI03E0H03F0H03F0gI07C0R03F,P01FC0Q03F80R03F0hI03E0H03F0H03F0gI07C0R03F,P01FC0Q07F80R03F0hI03C0H03F0H03F0gI0780R03F,P01FC0P07FF0S03F0hI07C0H03F0H03F0gI0F80R03F,P01FC0P03FF0S03F0hI0780H03F0H03F0gI0F0S03F,P01FC0P03FE0S03F0hN03F0H03F0gX03F,P01FC0P03FC0S03F0hN03F0H03F0gX03F,P01FC0P03F0T03F0jT03F,P01FC0P01E0T03F0jT03F,gJ010,,::::::::::::::J03F0H0FE0iS07FE0J03F80O01FC003F81FKFJ0FE0P07F001FC1FJFC0H07FC,J03F0H0FE0iS07FE0I01FHFP01FC003F81FKFI07FFC0O07F001FC1FJFC003FHF,J03F001FE0iS07FE0I07FHFC0N01FC003F81FKFH01FIFP07F001FC1FJFC00FIF80Q07F0I03FC0I03E0J0FC0H03FHFC0H0FC,J03F003FE0iS0HFE0I0JFE0N01FC003F81FKFH03FIF80N07F001FC1FJFC03FIF80P03FFE003FHFI01FFE0H03FF0H07FHFC003FF,J03F003FE0iS0IFI01FJFO01FC003F81FKFH07FIFC0N07F001FC1FJFC07FIFQ01FIFH07FHFC007FHFI0IFC007FHFC00FHFC0,J03F007FE0iS0FBF0H03FE0FF80M01FC003F81FC007F00FF83FE0N07F001FC1FJFC0FFC030P01FIF807FHFE00FIF801FHFE007FHFC01FHFE0,J03F007FE0iR01FBF0H03FC07F80M01FC003F81FC007F00FF01FE0N07F001FC001FC0H0HFU0JFC03FHFE01FIFC01FHFE007FHFC01FHFE0,J03F00FFE0iR01FBF8007F803FC0M01FC003F81FC007F01FE00FF0N07F001FC001FC001FE0T0F83FC03C1FF01FC1FC03F87F007C0I03F87F0,J03F00FFE0iR01FBF8007F001FE0M01FC003F81FC007F01FC007F80M07F001FC001FC001FC0T0C01FC0200FF01F80FE03F87F007C0I03F87F0,J03F01FBE001FC00FIFH01F0H0JFE001F0H01FHFH01FC0FF8FIFE003F0H0JFC0FE0I03C0I01F9F800FE0H0FE0M01FC003F81FC007F03F8003F80M07F001FC001FC003FC0T0800FE0I07F03F80FE07F03F807C0I07F03F8,J03F01FFE01FHFH0JFH0HFE00FIFE00FFE003FHFE01FC0FF8FIFE01FFC00FIFC0FE0I07E0I03F1FC00FE0H0FE0M01FC003F81FC007F03F8003F80M07F001FC001FC003F80W0FE0I07F03F80FE07F03F80FC0I07F03F8,J03F03F7E07FHF80FIF03FHF80FIFE03FHF803FIF81FC1FF8FIFE03FFE00FIFC0FE0I0HFJ03F1FC00FE0H0FE0I07C3E1FC003F81FC007F03F8003F80H01F0F87F001FC001FC003F80H01F0F80O0FE0I07F03F80FE07F01F80FC0I07F01F8,J03F07F7E03FHFC0FIF07FHFC0FIFE07FHFC03FIF81FC1FF8FIFE07FHF80FIFC0FE0I0HFJ03F1FC01FC0H07E0I0787C1FC003F81FC007F07F0H01F80H01E1F07F001FC001FC007F0J0F8780O0FE0I0FE01FC1FC07E01F80FFE0H07E01F8,J03F07E7E03FHFC0FIF0FIFE0FIFE0FIFE03F83FC1FC3FF8FIFE0FIF80FIFC0FE0I0HFJ07F0FC01FC0H07F0I0F87C1FKF81FC007F07F0H01FC0H03E1F07F001FC001FC007F0J0F87C0O0FE0H01FE01FF3F80FE01FC0FHFC00FE01FC,J03F0FE7E0381FE0FE0H0HF0FE003F800FF0FE03F81FC1FC3FF803F801FC1FC0FC1FC0FE0I0HFJ07F0FE01FC0H07F0H01F0F81FKF81FC007F07F0H01FC0H07C3E07F001FC001FC007F0J07C3E0N01FC007FFC00FIF80FE01FC0FIFH0FE01FC,J03F0FE7E0I0FE0FE001FC07F003F801FC07F03F81FC1FC3DF803F803F80FC0FC1FC0FE0I07E0I07E0FE01FC0H07F0H01E0F01FKF81FC007F07F0H01FC0H0783C07F001FC001FC007F0J03C3E0N01FC007FF0H07FFE00FE01FC0FIF80FE01FC,J03F1FC7E0I0FE0FE001FC07F003F801FC07F03F81FC1FC7FF803F803F007C0FC1FC0FHFC003C0I07E0FE01FC0H07F0H03E1F01FKF81FC007F07F0H01FC0H0F87C07F001FC001FC007F0J03E1F0N03F8007FE0H03FHFH0FE01FC0FIFC0FE01FC,J03F1FC7E0H01FC0FE001F803F003F801F803F03F83F81FC7BF803F803F007E0FC1FC0FIF80L0FE07F01FC0H07F0H03C3E01FKF81FC007F07F0H01FC0H0F0F807F001FC001FC007F0J01F0F0N07F8007FFC003FHF80FE01FC0H0HFC0FE01FC,J03F3F87E00FHF80FE003F803F803F803F803F83FIF01FCFBF803F807FIFE0FC1FC0FIFC0L0FC07F01FC0H07F0H07C3C01FKF81FC007F07F0H01FC001F0F007F001FC001FC007F0K0F0F80M07F0H07FFE00FIFC0FE01FC0H03FC0FE01FC,J03F3F07E00FFE00FE003F803F803F803F803F83FHFE01FCFBF803F807FIFE0FC1FC0FIFE0L0KF01FC0H07F0H0F87C01FC003F81FC007F07F0H01FC003E1F007F001FC001FC007F80J0F87C0M0HFJ01FF01FE3FE0FE01FC0H01FE0FE01FC,J03F7F07E00FHFH0FE003F803F803F803F803F83FHFE01FCF3F803F807FIFE0FC1FC0FJFL01FJF80FC0H0FE0H0F87C01FC003F81FC007F03F0H03F8003E1F007F001FC001FC003F80J0F87C0L01FE0J07F01FC0FE0FE01FC0I0FE0FE01FC,J03F7E07E00FHFC0FE003F803F803F803F803F83FIF81FDF3F803F807FIFE0FC1FC0FE0FF80J01FJF80FE0H0FE0H07C3C01FC003F81FC007F03F8003F8001F0F007F001FC001FC003FC0J0F0F80L03FC0J03F83F80FF07E01F80I0FE07E01F8,J03FFE07E0H01FE0FE003F803F803F803F803F83F83FC1FDF3F803F807FIFE0FC1FC0FE03F80J01FJF80FE0H0FE0H03C3E01FC003F81FC007F03F8003F80H0F0F807F001FC001FC003FC0I01F0F0M07F80J03F83F807F07E01F80I0FE07E01F8,J03FFC07E0I0FE0FE001F803F803F801F803F83F80FE1FDE3F803F803F0I01FC1FC0FE03F80J01FJF80FF001FE0H03E1F01FC003F81FC007F03FC007F80H0F87C07F001FC001FC001FE0I03E1F0H080I0HFL03F83F807F07F03F80I0FE07F03F80040,J03FFC07E0I0HF0FE001FC07F003F801FC07F03F80FE1FFE3F803F803F80H01F81FC0FE03F83C0H03F803FC07F003FC0H01E0F01FC003F81FC007F01FC00FF0I0783C07F001FC001FC001FF0I03C1E03F80H01FE0K03F83F807F07F03F80H01FC07F03F81FC0,J03FF807E0400FF0FE001FC07F003F801FC07F03F80FE1FBC3F803F803FC0H01F81FC0FE03F07E0H03F801FC03FC07FC0H01F0F81FC003F81FC007F00FF01FF0I07C3E07F001FC001FC0H0HFE0307C3E03F0I03FC0H04007F03FC0FE03F87F01001FC03F87F01F80,J03FF807E0701FF0FE0H0HF0FE003F800FF0FE03F80FE1FFC3F803F801FE0087F81FC0FE0FF0FF0H03F801FC03FE0FF80I0F8781FC003F81FC007F00FF83FE0I03E1E07F001FC001FC0H0KF0787C03F0I07F80H0780FF01FE1FE03F87F01C07FC03F87F01F80,J03FF007E07FHFE0FE0H0JFE003F800FIFE03F83FE1FF83F803F801FIF8FF81FC0FJF0FF0H07F001FE01FJFK0787C1FC003F81FC007F007FIFC0I01E1F07FKF801FC0H07FIF0F87803E0H01FJF07FIF01FIFC01FHFE01FIF801FHFE01F,J03FE007E0FIFE0FE0H07FHFC003F8007FHFC03FIFC1FF83F803F800FIF8FF01FC0FIFE0FF0H07F001FE00FIFE0J07C3E1FC003F81FC007F003FIF80I01F0F87FKF801FC0H03FIF1F0F807E0H03FJF07FHFE00FIFC01FHFE01FIFH01FHFE03F,J03FE007E0FIF80FE0H03FHF8003F8003FHF803FIF81FF83F803F8007FHFCFE01FC0FIFC0FF0H07F0H0FE007FHFC0N01FC003F81FC007F001FIFP07FKF801FC0I0JF80J07E0H03FJF0FIFC007FHF800FHFC03FHFE0H0IFC03F,J03FC007E03FHFH0FE0I0HFE0H03F80H0HFE003FHFE01FF03F803F8001FHFC7C01FC0FIFH07E0H0HFI0HFH01FHFP01FC003F81FC007F0H07FFC0O07FKF801FC0I07FHF80J07C0H03FJF07FHFI01FFE0H03FF003FHF80H03FF003E,J03FC007E007F800FE0I01F0I03F80H01F0H03FFE001FF03F803F80H01FE07001FC07FE0H03C0H0FE0H0HFI07F80O01FC003F81FC007F0H01FE0P07FKF801FC0J07F80K07C0H03FJFH07F80I03F0J0FC0H03F80J0FC003E,mI01F80W0F80hG07C,:mI01F80W0F0hH078,mI01F80V01F0hH0F8,mI01F80V01E0hH0F0,mI01F8,::,:::::::::::::::kR0E0,J03F803FE0jX03FE0gK0JFE0hM0FE,J03F807FC0jW03FFC0gK0JFE0hL0IFC0,J03F807F80jW07FFC0gK0JFE0hK03FIFiW0F8,J03F80FF0jW01FHFC0gK0JFE0hK03FIFiU01FF8,J03F81FE0jW03FHF80gK0JFE0hK01FIF80iS07FF8,J03F81FE0jW07FE0gM0FE0hN01F07F80iR01FHF8,J03F83FC0jW07F0gN0FE0hN01803FC0iR03FHF8,J03F83F80jW0FC0gN0FE0hQ01FC0iR07FF,J03F87F0jX0FC0gN0FE0hQ01FC0iR0HF8,J03F8FF0H01FC001FIF81FC0FF83F80FE07F03FE0FE03F81FHFE3F03C0H01FC0H07FHFE0H03E03F81FF01FC0I0IFC0J0F8001F878001FIF80N0JFL0FE001FE01FE001F0H03F0H0IF80K03FC03FC3FIFO01FC07FFC0I07C0H0FC0F0H0FE0H01FC0FE07FCFE07FC1FC07F001FC0M03FIFM0FE0,J03F8FE001FHF801FIF81FC0FF83F80FE07F03FE0FE03F81FHFE3F1FF001FHF8007FHFE003FFC3F83FC1FHF800FIFC0I07FF001F3FF001FIF80N0JFL0FE0H0FE01FC01FFE01FFC01FIFL01FC03F83FIFO01FC0FIF8003FF800FC7FC00FE0H01FC0FE0FF0FE07FC1FC07F01FHF80L03FIFL01FC0,J03F9FC003FHFC01FIF81FC1FF83F80FE07F07FE0FE03F81FHFE3F7FFC03FHFC007FHFE007FFC3F83F83FHFC01FIFC0H01FHFC01F7FF801FIF80N0JFL0FE0H0HF01FC03FFE03FFE01FIFC0J01FE03F83FIFO03F80FIFE00FHFE00FDFHFH0FE0H01FC0FE0FE0FE0FFC1FC07F03FHFC0L03FIFL01F80,J03F9FC003FHFE01FIF81FC1FF83F80FE07F07FE0FE03F81FHFE3FIFE03FHFE007FHFE01FHF83F87F83FHFE03FIFC0H03FHFE03FIFC01FIF80N0JFL0FE0H07F01FC0FHFC07FHF81FIFC0K0FE03F83FIFO03F80FIFE01FIFH0KF80FE0H01FC0FE1FE0FE0FFC1FC07F03FHFE0L03FIFL03F8F80,J03FBF8003FIF01FIF81FC3FF83F80FE07F0FFE0FE03F81FHFE3FIFE03FIFH07FHFE01FHF83F87F03FIF07FC1FC0H07FIF03FIFE01FIF80N0JFL0FE0H07F03F80FHFC0FIF81FC1FE0K0FE07F03FIFO0HFH0FE0FF03FIF80FJF80FE0H01FC0FE1FC0FE1FFC1FC07F03FIFM03FIFL03FBFF0,J03FHFI01C0FF01F83F81FC3FF83F80FE07F0FFE0FE03F81FC003FE1FF01C0FF007E0FE03FC083F8FE01C0FF07F01FC0H07F87F03FE0FF01F83F80N0FE0M0FE0H07F83F81FE041FC1FC1FC0FE0K0HF07F03F07F0L01FFE00FE07F03FC3F80FF87FC0FE0H01FC0FE3F80FE1FFC1FC07F01C0FF0L03F07F0K03FIF8,J03FHF8001007F01F83F81FC3DF83F80FE07F0F7E0FE03F81FC003FC07F01007F007E0FE07F8003F9FC01007F07F01FC0H0FE03F83FC07F01F83F80N0FE0M0FE0H03F83F83FC003F80FC1FC0FE0K07F07F03F07F0L01FFC00FE07F07F01FC0FF01FC0FE0H01FC0FE7F00FE1EFC1FC07F01007F0L03F07F0K07FIFC,J03FHFE0J03F81F83F81FC7FF83F80FE07F1FFE0FE03F81FC003FC07F0I03F807E0FE07F0H03F9F80I03F87F01FC0H0FE03F83FC07F01F83F80N0FE0M0FE0H03F87F03F8003F007C1FC0FE0K07F0FE03F07F0L01FF800FE07F07F01FC0FF01FC0FHFC01FC0FE7E00FE3FFC1FC07F0I03F80K03F07F0K07FIFE,J03FIFJ07FF81F83F81FC7BF83F80FE07F1EFE0FE03F81FC003F803F8007FF807E0FE07F0H03FBF0I07FF87F81FC0H0FC01F83F803F81F83F80N0FE0M0FE0H01FC7F03F8003F007E1FC1FC0K03F8FE03F07F0L01FFE00FE0FE07E00FC0FE00FE0FIF81FC0FEFC00FE3DFC1FC07F0H07FF80K03F07F0K07FE1FE,J03FIF8007FHF81F83F81FCFBF83FIFE07F3EFE0FJF81FC003F803F807FHF807E0FE0FE0H03FHFI07FHF83FC1FC001FC01FC3F803F81F83F80N0FE0M0FE0H01FC7E07F0H07FIFE1FIF80K03F8FC03F07F0L01FHF80FIFC0FE00FE0FE00FE0FIFC1FC0FHFC00FE7DFC1FJFH07FHF80K03F07F0K07F80FE,J03F87FC01FIF81F83F81FCFBF83FIFE07F3EFE0FJF81FC003F803F81FIF807E0FE0FE0H03FHF801FIF81FIFC001FC01FC3F803F81F83F80N0FE0M0FE0H01FC7E07F0H07FIFE1FIFM03F8FC03F07F0N07FC0FIF80FE00FE0FE00FE0FIFE1FC0FHFE00FE7DFC1FJF01FIF80K03F07F0K07F00FF,J03F83FC03FIF81F83F81FCF3F83FIFE07F3CFE0FJF81FC003F803F83FIF80FC0FE0FE0H03FHFC03FIF80FIFC001FC01FC3F803F81F83F80N0FE0M0FE0I0FCFE07F0H07FIFE1FIFM01F9FC03F07F0N01FC0FIF80FE00FE0FE00FE0FJF1FC0FIFH0FE79FC1FJF03FIF80K07E07F0K07F007F,J03F81FE07FC3F81F83F81FDF3F83FIFE07F7CFE0FJF81FC003F803F87FC3F80FC0FE0FE0H03FHFE07FC3F803FHFC001FC01FC1F803F81F83F80N0FE0M0FE0I0FEFC07F0H07FIFE1FIFC0K01FDF803F07F0O0FE0FIFE0FE00FE0FE00FE0FE0FF1FC0FIF80FEF9FC1FJF07FC3F80K07E07F0K07F007F,J03F80FE07F03F81F83F81FDF3F83FIFE07F7CFE0FJF81FC003F803F87F03F80FC0FE0FE0H03FIF07F03F80FIFC001FC01FC1F803F81F83F80N0FE0M0FE0I07EFC07F0H07FIFE1FC1FE0L0FDF803F07F0O0FE0FE0FF0FE00FE0FE00FE0FE03F9FC0FIFC0FEF9FC1FJF07F03F80K07E07F0K03F007F,J03F80FE0FE03F83F83F81FDE3F83F80FE07F78FE0FE03F81FC003F803F0FE03F81FC0FE07F0H03F8FF0FE03F81FE1FC0H0FC01FC1FC03F03F83F80F0H040H0FE0H0F0I0FE0I07EFC03F8003F0I01FC07F0020I0FDF807F07F01E0L0FE0FE03F87E00FE0FE00FC0FE03F9FC0FE3FC0FEF1FC1FC07F0FE03F80080H0FE07F01E0H03F007F,J03F80FF0FE03F83F03F81FFE3F83F80FE07FF8FE0FE03F81FC003FC07F0FE03F81FC0FE07F0H03F87F8FE03F81FC1FC0H0FE03F81FC07F03F03F81F81FC0H0FE001F80H0FE0I07FF803F8003F80H01FC07F0FE0I0IFH07E07F03F0L0FE0FE03F87F01FC0FF01FC0FE03F9FC0FE1FE0FHF1FC1FC07F0FE03F83F80H0FE07F03F0H03F807E,J03F807F0FE07F83F03F81FBC3F83F80FE07EF0FE0FE03F81FC003FC0FF0FE07F83F80FE07F8003F83F8FE07F83F81FC0H0FE03F80FE07F03F03F81F81F80H0FE001F80H0FE0I03FF803FC003FC0H01FC07F0FC0I07FF007E07F03F0H01001FE0FE03F87F01FC0FF03FC0FE03F1FC0FE0FE0FDE1FC1FC07F0FE07F83F0H01FC07F03F0H03F80FE,J03F807F0FF0FF8FF03F81FFC3F83F80FE07FF0FE0FE03F81FC003FE1FE0FF0FF83F80FE07FC083F83F8FF0FF83F81FC0H07F87F00FF0FE0FF03F83FC1F80H0FE003FC0H0FE0I03FF803FE041FE0081FC07F0FC0I07FF01FE07F07F8003E07FC0FE03F83FC3F80FF87F80FE0FF1FC0FE0FE0FFE1FC1FC07F0FF0FF83F0H01FC07F07F8001FE1FE,J03F807F87FIF9FF03F81FF83F83F80FE07FE0FE0FE03F81FC003FIFE07FIF9FKF83FHF83F83FC7FIF83F01FC0H07FIFH07FHFE1FF03F83FC1F0I0FE003FC0H0FE0I01FF001FHFC1FIF81FC1FF0F80I03FE03FE07F07F8003FIFC0FE0FF83FIF80FJF80FJF1FC0FE0FF0FFC1FC1FC07F07FIF83E0H0LFC7F80H0JFC,J03F803F87FIF9FE03F81FF83F83F80FE07FE0FE0FE03F81FC003FIFC07FIF9FKF81FHF83F81FC7FIF87F01FC0H03FHFE003FHFC1FE03F83FC3F0I0FE003FC0H0FE0I01FF0H0IFC0FIF81FIFE1F80I03FE03FC07F07F8003FIF80FJF01FIFH0KFH0JFE1FC0FE07F0FFC1FC1FC07F07FIF87E0H0LFC7F80H0JF8,J03F803FC3FFDF9FC03F81FF83F83F80FE07FE0FE0FE03F81FC003FIF803FFDF9FKF80FHFC3F81FE3FFDF87F01FC0H01FHFC001FHF81FC03F83FC3F0I0FE003FC0H0FE0I01FE0H07FFE07FHFC1FIFC1F80I03FC03F807F07F8007FIFH0JFE00FHFE00FIFE00FIFC1FC0FE07F8FFC1FC1FC07F03FFDF87E0H0LFC7F80H03FHF0,J03F803FC1FF1F8F803F81FF03F83F80FE07FC0FE0FE03F81FC003FBFF001FF1F9FKF803FFC3F80FE1FF1F8FE01FC0I07FF0I0HFE00F803F81F83E0I0FE001F80H0FE0J0FE0H01FFE01FHFC1FIF01F0J01FC01F007F03F0H01FHFC00FIF8003FF800FEFFC00FIF01FC0FE03F8FF81FC1FC07F01FF1F87C0H0LFC3F0I01FFC0,J03F801FC07C1F8E003F81FF03F83F80FE07FC0FE0FE03F81FC003F8780H07C1F9FKF8007F03F80FF07C1F9FE01FC0J0F80I03F0H0E003F80F03E0I0FE0H0F0I0FE0I01FC0I03F8001FE01FHFH01F0J03F801C007F01E0I03FE0H0IF80I07C0H0FE1E0H07FF001FC0FE03FCFF81FC1FC07F007C1F87C0H0LFC1E0J03E,hU03F80N01F8001F80hM07C0Y01FC0X03E0J03F80gR0FE0gX0F80H0FC0H0FC0,hU03F80N01F8001F80hM07C0Y03F80X03E0J07F0gS0FE0gX0F80H0FC0H0FC0,hU03F80N01F8001F80hM0780Y07F80X03C0J0HFgT0FE0gX0F0I0FC0H0FC0,hU03F80N01F8001F80hM0F80X07FF0Y07C0I0HFE0gS0FE0gW01F0I0FC0H0FC0,hU03F80N01F8001F80hM0F0Y03FF0Y0780I07FE0gS0FE0gW01E0I0FC0H0FC0,hU03F80N01F8001F80iN03FE0gK07FC0gS0FE0hI0FC0H0FC0,hU03F80N01F8001F80iN03FC0gK07F80gS0FE0hI0FC0H0FC0,hU03F80jK03F0gL07E0gT0FE,hU03F80jK01E0gL03C0gT0FE,mJ010gM02,,::::::::::::::M07FC0hO01FHFhU03FIFC0,L03FHFhP03FHFE0hS03FIFC0,L0JF80hN03FIF80hR03FIFC0,K03FIF80hN03FIFC0hR03FIFC0,K07FIFhP03FIFC0hR03FIFC0,K0HFC030hO03F83FE0hR03F01FC0,K0HFhS03F80FE0hR03F01FC0,J01FE0hR03F80FE0hR03F01FC0,J01FC0hR03F807F0hR03F01FC0,J03FC0J0JFC001F8007FHFE003F8007F01FC0H0F80J07FFC0I03F807F0H07C0J07C0H03E03F81FF07F03FE0gG03F01FC003F801FIFC01FC0I01FJF07E0780I07C0H0FE07FC00FE003FFE0I03E0I0JFC0H07C1FIFC1FHFJ0FE0H0F,J03F80J0JFC00FFE007FHFE03FHFH07F01FC007FF0J0JF80H03F807F003FF80H07FF803FFC3F81FF07F03FE0gG03F01FC03FHF01FIFC1FHF80H01FJF07E3FE0H03FF800FE07FC0FHF807FHFC001FFC0H0JFC007FF9FIFC3FHFE00FHFC01F80,J03F80J0JFC01FHFH07FHFE07FHF807F01FC01FHFC0I0JFE0H03F807F00FHFE0H0IF807FFC3F83FF07F07FE0gG03F01FC07FHF81FIFC3FHFC0H01FJF07EFHF800FHFE00FE0FFC3FHFC07FIFH07FHFI0JFC00FHF9FIFC3FIF81FHFE03FC0,J07F0K0JFC03FHFC07FHFE07FHFC07F01FC03FHFE0I0JFE0H03F807F01FIFH03FHF01FHF83F83FF07F07FE0gG03F01FC07FHFC1FIFC3FHFE0H01FJF07FIFC01FIFH0FE0FFC1FHFE07FIFH0JF800FIFC03FHF1FIFC3FIF81FIF03FC0,J07F0K0JFC07FHFC07FHFE07FHFE07F01FC07FIFJ0FE0FF0H03F80FE03FIF803FHF01FHF83F87FF07F0FFE0gG03F01FC07FHFE1FIFC3FIFI01FJF07FIFC03FIF80FE1FFC1FHFE07F07F81FIFC00FIFC03FHF1FIFC3F83FC1FIF83FC0,J07F0K0FC1FC0FE0FE07E0FE0381FE07F01FC07F87F0I0FE07F0H03F80FE03FC3F807F8103FC083F87FF07F0FFE0gG03F01FC0381FE007F001C0FF0H01FC07F07FC3FE03FC3F80FE1FFC1C0FF07F03F81FE1FC00FC1FC07F81007F003F81FC0E07F83FC0,J07F0K0FC1FC1FC07E07E0FE0200FE07F01FC0FE03F80H0FE07F0H03F83FE07F01FC0FF0H07F8003F87BF07F0F7E0gG03F01FC0200FE007F001007F0H01FC07F07F80FE07F01FC0FE1EFC0H07F07F03F83F80FE00FC1FC0FF0I07F003F81FC0803F81F80,J07F0K0FC1FC1F803E07E0FE0I07F07F01FC0FE03F80H0FE07F0H03FIFC07F01FC0FE0H07F0H03F8FHF07F1FFE0gG07E01FC0I07F007F0J03F8001FC07F07F80FE07F01FC0FE3FFC0H07F07F03F83F80FE00FC1FC0FE0I07F003F81FC0H01FC0F,J07F0K0FC1FC1F803F07E0FE0H0IF07F01FC0FC01F80H0FE0FE0H03FIFC07E00FC0FE0H07F0H03F8F7F07F1EFE0gG07E01FC0H0IFH07F0I07FF8001FC07F07F007F07E00FC0FE3DFC0H0FE07F07F03F007E00FC1FC0FE0I07F003F83F8003FFC,J07F0K0FC1FC3FJF07E0FE00FIF07FIFC1FC01FC0H0JFC0H03FIFH0FE00FE1FC0H0FE0H03F9F7F07F3EFE0gG07E01FC00FIFH07F0H07FHF8001FC07F07F007F0FE00FE0FE7DFC07FFC07FHFE07F007F00FC1FC1FC0I07F003FIFH03FHFC,J07F80J0FC1FC3FJF07E0FE03FIF07FIFC1FC01FC0H0JF80H03FHFC00FE00FE1FC0H0FE0H03F9F7F07F3EFE0gG07E01FC03FIFH07F001FIF8001FC07F07F007F0FE00FE0FE7DFC07FF007FHFC07F007F00FC1FC1FC0I07F003FHFE00FIFC,J03F80I01F81FC3FJF07E0FE07FIF07FIFC1FC01FC0H0JF80H03FFE0H0FE00FE1FC0H0FE0H03F9E7F07F3CFE0gG0FE01FC07FIFH07F003FIF8001FC07F07F007F0FE00FE0FE79FC07FF807FHFC07F007F01F81FC1FC0I07F003FHFE01FIFC,J03FC0I01F81FC3FJF07E0FE0FF87F07FIFC1FC01FC0H0JFE0H03F80I0FE00FE1FC0H0FE0H03FBE7F07F7CFE0gG0FE01FC0FF87F007F007FC3F8001FC07F07F007F0FE00FE0FEF9FC07FFE07FIF07F007F01F81FC1FC0I07F003FIF83FE1FC,J03FC0I01F81FC3FJF07E0FE0FE07F07FIFC1FC01FC0H0FE0FF0H03F80I0FE00FE1FC0H0FE0H03FBE7F07F7CFE0gG0FC01FC0FE07F007F007F03F8001FC07F07F007F0FE00FE0FEF9FC0H0HF07F07F87F007F01F81FC1FC0I07F003F83FC3F81FC,J01FE0I03F81FC1F80I0FE0FE1FC07F07F01FC0FC01FC0H0FE03F8003F80I07E00FE0FE0H07F0H03FBC7F07F78FE0g01FC01FC1FC07F007F00FE03F8001FC07F07F007E07E00FE0FEF1FC0H07F07F01FC3F007F03F81FC0FE0I07F003F80FE7F01FC,J01FF0I03F81FC1FC0I0FC0FE1FC07F07F01FC0FE03F80H0FE03F8003F80I07F01FC0FE0H07F0H03FFC7F07FF8FE0g01FC01FC1FC07F007F00FE03F8001FC07F07F80FE07F01FC0FHF1FC0H07F87F01FC3F80FE03F81FC0FE0I07F003F80FE7F01FC0F,K0HFE0307F01FC1FE0I0FC0FE1FC0FF07F01FC0FE03F80H0FE03F8003F80I07F01FC0FF0H07F8003F787F07EF0FE0g03F801FC1FC0FF007F00FE07F8001FC07F07F81FE07F01FC0FDE1FC2007F87F01FC3F80FE07F01FC0FF0I07F003F80FE7F03FC1F80,K0KF07F01FC0FF0043FC0FE1FE1FF07F01FC07F87F0I0FE03F8003F80I03FC3F80FF8107FC083FF87F07FF0FE0g03F801FC1FE1FF007F00FF0FF8001FC07F07FC3FC03FC3F80FFE1FC380FF87F01FC1FE1FC07F01FC0FF81007F003F80FE7F87FC3FC0,K07FIF3FKF0FIFC7FC0FE0FJF07F01FC07FIFJ0FE0FF8003F80I03FIF807FHF03FHF83FF07F07FE0FE0Y01FLF8FJFH07F007FIF8001FC07F07FIFC03FIF80FFC1FC3FIF07F07FC1FIFC3FKF07FHFH07F003F83FE3FIFC3FC0,K03FIF3FKF07FHFC7F80FE0FJF07F01FC03FHFE0I0KFI03F80I01FIFH03FHF01FHF83FF07F07FE0FE0Y01FLF8FJFH07F007FIF8001FC07F07FIF801FIFH0HFC1FC7FIF07FIF80FIF83FKF03FHFH07F003FIFC3FIFC3FC0,L0JFBFKF03FHFE7F00FE07FFBF07F01FC01FHFC0I0JFE0H03F80J0IFE001FHF80FHFC3FF07F07FE0FE0Y01FLF87FFBF007F003FFDF8001FC07F07FIFI0IFE00FFC1FC7FHFC07FIFH07FHF03FKF01FHF807F003FIF81FFEFC3FC0,L07FHFBFKFH0IFE3E00FE03FE3F07F01FC007FF0J0JF80H03F80J03FF80H07FF803FFC3FE07F07FC0FE0Y01FLF83FE3F007F001FF1F8001FC07F07F7FE0H03FF800FF81FC1FHF807FHFC001FFC03FKFH07FF807F003FHFE00FF8FC1F80,M07F83FKFI0HF03800FE00F83F07F01FC0H0F80J0IF80I03F80K07C0J0FE0H07F03FE07F07FC0FE0Y01FLF80F83F007F0H07C1F8001FC07F07F0F0J07C0H0HF81FC03FC007FFC0I03E003FKFI0FE007F003FFE0H03E0FC0F,P03F0H03F0jY01F80H01F80gK07F0gU03F0H03F0,::::::kW01F80H01F80gK07F,mR07F,,::::::::::::::::::::~DG010.GRF,02048,016,
,:::::::::::::::::R07FF8,Q07FIFE0,:O0603FIFE0,N01E03FIFE060,N07F03FIFE078,M01FF038007C07E,M07FF80L0HF80,M0IF80L0HFC0,M0HFC0M0IF0,M07F0N03FF0,M03E0O0HF0,M0380O07E0,K020R01C0,K070S080,K0F80T040,J01FC0T0E0,J03FC0S03F0,J07F80S03F8,J0HFU03FC,I01FE0T01FE,I01FC0U0FE,I01FC0U0HF,J0F80U07F80,J030V03F80,gH01F,gH01C,,H018,H01E,H03F,H03F0Y070,H07F0X01F8,H07E0X01F8,:H07E0X01FC,H0FE0Y0FC,H0FC0Y0FC,::H03C0Y0FE,gJ07E,:,:::01F8,:01F80Y07E,:::H0FC0Y0FE,H0FC0Y0FC,:::H070Y01FC,gI01F8,gJ0F8,gJ018,,I0180,I0780,H01FC0W04,H01FC0W0F,I0FE0V01FC0,:I07F0V03F80,I07F80U07F80,I03FC0U07F,I01FC0U0FE,I01F80T01FE,J0F0U01FC,J040V0F8,gH070,L060T020,L0F0R020,K01FC0Q0E0,K03FE0P01F0,K01FF0P03F8,L0HFC0O0HFC,L07FE0N03FF8,L03FC0N07FF0,M0FC0N03FE0,M0780F0J0303F80,M0181FF0H03F03E,O01FFE07FF018,O01FFE07FF8,O03FFE07FF8,P0HFE07FF8,P01FE07FE0,R0C07C,,:::::::::::::::^XA
^MMT
^PW1748
^LL1252
^LS0
^FT32,256^XG000.GRF,1,1^FS
^FT32,192^XG001.GRF,1,1^FS
^FT32,352^XG002.GRF,1,1^FS
^FT1536,800^XG003.GRF,1,1^FS
^FT1536,896^XG004.GRF,1,1^FS
^FT1536,992^XG005.GRF,1,1^FS
^FT1536,1120^XG006.GRF,1,1^FS
^FT1408,256^XG007.GRF,1,1^FS
^FT1536,576^XG008.GRF,1,1^FS
^FT32,704^XG009.GRF,1,1^FS
^FT1536,416^XG010.GRF,1,1^FS
^BY6,3,58^FT47,832^BCN,,N,N
^FD>;{Mid(snarr(1), 1, 14)}>6{Mid(snarr(1), 15)}^FS
^BY6,3,59^FT47,983^BCN,,N,N
^FD>;{Mid(snarr(2), 1, 14)}>6{Mid(snarr(2), 15)}^FS
^BY6,3,58^FT47,1123^BCN,,N,N
^FD>:{Mid(snarr(0), 1, 4)}>5{Mid(snarr(0), 5)}^FS
^FO1626,798^GB0,77,14^FS
^FT966,676^A0N,50,48^FH\^FD12/2023^FS
^FT47,759^A0N,67,67^FH\^FDIMEI 1: {snarr(1)}^FS
^FT47,910^A0N,67,67^FH\^FDIMEI 2: {snarr(2)}^FS
^FT47,1051^A0N,67,67^FH\^FDSN: {snarr(0)}^FS
^PQ1,0,1,Y^XZ
^XA^ID000.GRF^FS^XZ
^XA^ID001.GRF^FS^XZ
^XA^ID002.GRF^FS^XZ
^XA^ID003.GRF^FS^XZ
^XA^ID004.GRF^FS^XZ
^XA^ID005.GRF^FS^XZ
^XA^ID006.GRF^FS^XZ
^XA^ID007.GRF^FS^XZ
^XA^ID008.GRF^FS^XZ
^XA^ID009.GRF^FS^XZ
^XA^ID010.GRF^FS^XZ
"
        '        Return $"
        '^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
        '^XA
        '^MMT
        '^PW874
        '^LL0626
        '^LS0
        '^FO32,96^GFA,03328,03328,00052,:Z64:
        'eJzt0zFu2zAUBuAncOCWd4GU7BW6E1KP1KBDWFSo6GrwqBuk1+jIwoPHXIFBBm8NhQ7hIIh9lFTGdmAgUyf9gkiB0EeB5BPAmjVr1qxZs+bN0RfGq/zEzdTJOPcQbTbcSfa08RWw6AGKaC8af2SwPfyaDVJjLpiPMWSDHrf7nWsMmYpmODfgp5ZZf2z4tt3ZxoCACJjJa1Nnc+X5vn20t4uBy0ZlIzy7Lw7mOZknMvwhoL3xTZAOAm8t+KsPzlcYvPrimLvWNInQk/ljyXR0k6ncbWhCM7BxNr3zsSPTBAzvRjaQKX7DM0wGF+OboQlx5PHE1PTtQY58TGakhbyYzz/f61rxzeEHdttksHe9a2nf6rospUQBqoYBvhWP7sWwoEsynaTdSWZDhlk6H1VLgQJBqck8HBkZtODtfnvH9jGZwvUDGa+VJiFjMhrUialeGR/J2GyaMZlUbMlI4I1S+pVpaD1wwdBVEcmmXYzcZSPg3JjZLHuw/WfYbGoxmSCT4TZMhtuuVDqc7nXR94spS4H4/dzETqhUA9zkM0WIfWxDMnIQSGfqZ2NmUyRTUeVwk2uHzE3cTAaDQDamocnoyUB1f62k/uq4ka4Ii1GfBpgMdwKLofBIrys09Wzm6KN/4a1ZzX/IX+EYcHM=:602C
        '^FO0,0^GFA,07168,07168,00056,:Z64:
        'eJztl03OnDAMhkEsWHKEHIWjQdUD9Ai9ClUvgtRFt+zKAuGS+O9lEpiv0rcrXsxMwE9snDcmU1WPPfbYY4899j9bT/vxWdMm44EaWuW33LIx2kCUXJQjqtWPJ2vKHBGNOnXy3S12o9x2zQ3kMXQOTrglHaMd2dOUHtNjDOLX3nBHFjQnbjRfjd3Rkj6JrrmQokZusTk6uUMyRjuykFknixHEL7zhVpubvzV2kCcnGaN19EeefjFfjd1LpUtcoF+ySov4ThZ7EO67jMvcajE0NteVqJMxWk8/RIWrxdDYKs8Sd/il5VFxxRgSuxZubwsCVU7FFb9kDpX11hQEekDy+LvHYL/mLTdZETgGx1ZZr3Uu0Hipl+UdzZfnaEVIS4GLKaicnOPNobJeqlzYyE0Wg2OrrGepAFossc47g+9YuTynAhcZ3Z4z0NOJ6zOBIreYbwdbOcYKmUDjlQaEzb4cW+V5xXGZtZKjxVZOK4sWM6+h87Ivz6Hy1AaFlioFnVfVtupEH+B2/WDEZK1KRUvIYJ1Xd1PETZ4Fjh+tt85ru3DXArec8n7HjdYtYmyTtaYFxgm4QFvrTiPIM+da6Z3aeb0bnrlXgZ652TpgjO3yrDKBdv7sSVze7SfZFPfcVnEdgnEzyLrKBMrjxjqvv80WkGfOcfzaOq+/PReRHEHLuOI2e2PG2C7rXKBSXxOovqHTW97kmXOyntZ5TU+08QujwRZ8xZGffmgHWWcC1aEu0hdLJ56aWAx3nAr0q3E9oaxfBappK/fTyt3TN5Andyc3LZMK9Lctbzi4CbjhhmvlKMMcnYR5Fqhmbwcqu9vZsW0UbgYuGCcHP8umlcPVYPHOnNoVZw4ngfZ2VU4OVu3GLxW4wS/z5La6NUnXNVv/hWvcAQXqV6V4G9xaP8RN6XH95mBLChXwZF649YXrwMM5yELE6EXr5YX9Ea7LuPBSATbIHo4UbH7WzTnMYn3LTcBpXu0F59snnO7pb97AOUdFzkXODaPE5bnhZrzgoB21p7XVLHjiT+Og3TYn7Z6nyDgIAhw20/Sod1yNe+wdh0XEXrCB21zgcNGcwxKlmW+5AXrWOw67bQ+91bNPc3yQCyduK3D4VvCcMfumyOEm9ygBskhrcsuV/sw99thjj32e/QUpxdC/:07D0
        '^FO32,160^GFA,01280,01280,00020,:Z64:
        'eJzt0bEOgjAQgOFrMOlm30Cew4nR13HUxARIB0ZeqeQGRl+hTR/AsjkYzxOLEnDTyfhv/YZrmwP490nKZONjSgWbzSe2m1t5nhoEyRbeGU2M521PcACyK0FohIGw6E0QW16yJRYCsO1PCRVkqRnbUbamc77xL1sflbadb5zH631eNMFGbPd/9ZaNzAxW14OhfViasjnnjfDYRFuxab7AJGzORFNVi86FDSJ6O5imigK1iITRlCpJUshbjaSjLWVxYcuq5PnmPgnb2aK+bf9+ohuuaKcC:35BE
        '^FO768,352^GFA,00768,00768,00008,:Z64:
        'eJzN0LENwyAQheGzKK70CIzCZoHRGIURXFJYvLw7osSWkiJFIiPhr0GY+0XmWrf5WdvD7aM4qm4T7TS+DGaiu1klDJqrLKAotIBbBBUBvDs36DA7opkA7DTSbv+m9oZAK11osZdnDJ8g+jERRZP/ry/6nVS3TWe/k8EazH7i/UzvN7Xt/Ti1ekfe7f2G5Hf9lNobn/2oT5A896Hf7Ye5LrPu/gCO4g==:0814
        '^FO768,416^GFA,00512,00512,00008,:Z64:
        'eJy9kD0OwjAMhZ0fFYQq6NgF0SP0CD1Kz8HkLkxIbMyAGDpyA3IBDsDWUzBVCn51egSw5HyJ82L5hei3kSdmiTbRsCyNZJDEvpUa6k/VuDeRB19EG9Qj00509hTHvbzJOnYfoedgzhMHt2XwkUG/5j5HvxXfS+WtVl4b5YXNpNNzzrZS+gL0XC6VtTfSf8GtVQYX4aOLY5jmGjA60YGoBo9EJVgk71XyLl4dqVd4R294n+/mf/p3fAF1OSzF:94D6
        '^FO768,448^GFA,00768,00768,00008,:Z64:
        'eJzd0S8OwjAUBvBuXSiCMG4wjoEg2bVwnZvlCJyENEHMIDgCDkPCc5tY9vHa9wSZJUFQ80v/vbZfjfn7VjZifRPxTFi8kg59kH5PzJId2XWGforTLNgqiDX5DlwR5B2bgfZRC9qoZm6Ne7JSZfxoMq5v4xkHdmC3LMkl7VU9i7lv0r4cwei5WmeYSbm6knX3nUv3DWPSX5CsWnTxPWWLR9Q6eV/MY1JHlT5zcprbQnMsNNeiPH39Vb9vb2L+fzs=:5AB1
        '^FO768,512^GFA,00512,00512,00008,:Z64:
        'eJxjYBgcgP8AkGD8QJj+DwKEaZLtJ9JcoDvsoO7FTxNrHonuBZvP+IEwTaS9AMEtlXo=:530B
        '^FO704,0^GFA,03200,03200,00020,:Z64:
        'eJztzzEKgDAQRNENKbbMEXLUXE3wIh5BsBU3EMsZJGCjML981YyZUj+oxN1biwvMiVViDS0FmhOrxBpaDrRCzJ9tbN3GsznL03YQW4llsHQS24ktxAzNZDKZjJpS6vt1WWa1qQ==:4BB0
        '^FO768,224^GFA,00768,00768,00008,:Z64:
        'eJxjYBhwYCEDoWvssNMweXRQQICu/4efJqQfHbA3QGj5ORDavg+VrobSxej0PAhtDaXt/0DN+QGh+T9AzX+Aw2Kog2R+oOqH0fV3cNiLRhMKDwwAcxgqLf//P5SG6EOnGRj4yKJxmYcwlxYAAEWDPNE=:246C
        '^FO32,192^GFA,17920,17920,00080,:Z64:
        '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:6EF8
        '^FO768,160^GFA,00768,00768,00008,:Z64:
        'eJxjYBgy4D+EYvx/AEwb/D8Gpn8wHAcL/2DgB0kwHmBgfgDVAeJLQNk2QMwDxBVALAfEH4CYH4hBatkbIGqYoTQTGKEIsYPJBxg0P5gugKqG0QlwR/NhpQvgNEw/xLwPUFUwDyDMgbmD+QCEwwijYe5/APWPA9QSFijmg4rB/QoNH6BB7CAzGPsYnMFm8x9/xAChIbYzHm9gGP4AAMVvJws=:C4B5
        '^FO813,427^GB0,39,7^FS
        '^FT34,431^A0N,33,33^FH\^FDIMEI 1: {snarr(1)}^FS
        '^BY3,3,29^FT34,460^BCN,,N,N
        '^FD>;{Mid(snarr(1), 1, 14)}>6{Mid(snarr(1), 15)}^FS
        '^FT30,504^A0N,33,33^FH\^FDIMEI 2: {snarr(2)}^FS
        '^BY3,3,29^FT34,533^BCN,,N,N
        '^FD>;{Mid(snarr(2), 1, 14)}>6{Mid(snarr(2), 15)}^FS
        '^FT34,576^A0N,33,33^FH\^FDSN: {snarr(0)}^FS
        '^BY3,3,29^FT34,605^BCN,,N,N
        '^FD>:{Mid(snarr(0), 1, 4)}>5{Mid(snarr(0), 5)}^FS
        '^FT519,393^A0N,29,28^FH\^FD12/2024^FS
        '^PQ1,0,1,Y^XZ"
    End Function
#End Region
#End Region

#Region "Проверка SberID"
    Private Sub Label17_DoubleClick(sender As Object, e As EventArgs) Handles Label17.DoubleClick
        If CB_CheckID.Visible = True Then
            CB_CheckID.Visible = False
        Else
            CB_CheckID.Visible = True
        End If
    End Sub
    Private Function CheckSberID(SN)
        Return SelectString($"SELECT SN  FROM [FAS].[dbo].[SberCheckID] where SN='{SN}'")
    End Function
#End Region
#Region "Без проверки передыдущего шага"
    Private Sub BT_PCBInfo_Click(sender As Object, e As EventArgs) Handles BT_PCBInfo.Click
        If CB_Deception.Visible = False Then
            CB_Deception.Visible = True
        ElseIf CB_Deception.Visible = True Then
            CB_Deception.Visible = False
            CB_Deception.Checked = False
        End If
    End Sub
#End Region


End Class

