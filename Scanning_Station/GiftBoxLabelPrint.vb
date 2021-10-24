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
            If InStr(item.ToString(), "ZDesigner") Or InStr(item.ToString(), "Zebra ZT410") Then
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
                    Print(GetLabelContent(dataToPrint(5), 0, 0, 2))
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
            PrintLabel(Controllabel, $"Номер {SerialTextBox.Text} не был зарегистрирован на ТНТ!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            For i = 0 To 7
                prestep.Add(False)
            Next
            BT_Pause.Focus()
            Return prestep
        ElseIf (prestep(0) = PCInfo(6) Or prestep(0) = 37) And prestep(1) = 2 Then 'Плата имеет статус ("текущий шаг"/2)
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
        ElseIf prestep(0) = PreStepID And prestep(1) = 2 Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)
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
        If w = 2 Then
            Return $"
^XA
^RS,,,3,N,,,2
^RR3
^XZ
^XA
^SZ2^JMA
^MCY^PMN
^PW1182
~JSN
^JZY
^LH0,0^LRN
^XZ
~DGR:SSGFX000.GRF,60912,141,:Z64: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:747A
^XA
^FT872,257
^CI28
^A@N,21,21,TT0003M_^FD{Format(Date.Now, "MM.yyyy")}^FS
^FO66,339
^BY2^BCN,72,N,N^FD>:{Mid(sn, 1, 10)}>5{Mid(sn, 11)}^FS
^FT111,438
^CI0
^AAN,27,15^FD{Mid(sn, 1)}^FS
^FO0,35
^XGR:SSGFX000.GRF,1,1^FS
^PQ1,0,1,Y
^XZ
^XA
^IDR:SSGFX000.GRF^XZ
"
        End If
    End Function
End Class