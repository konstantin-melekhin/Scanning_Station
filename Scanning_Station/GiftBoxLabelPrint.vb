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
                            Print(GetLabelContent_Tablet(dataToPrint(5), 0, 0))
                            'Print(GetLabelContent_SmartPhone(dataToPrint(5), 0, 0))
                        ElseIf LOTInfo(0) = "f+ R570E" Then
                            Print(GetLabelContent_SmartPhone(dataToPrint(5), 0, 0))
                        End If

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
        SNFormat = GetSTBSNFormat(LOTInfo(8), SerialTextBox.Text)
        If SNFormat(0) = False Then
            PrintLabel(Controllabel, "Формат номера не определен!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
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
        ElseIf prestep(0) = prestepid And prestep(1) = 2 And LOTInfo(20) = 44 Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)

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

    Private Function GetLabelContent_Tablet(sn As String, x As Integer, y As Integer) As String
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
BARCODE 809,416,""EAN13"",98,0,180,3,6,""468005976651""
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

