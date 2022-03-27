Imports Library3
Imports System.Deployment.Application
Imports System.Drawing.Printing
Imports System.IO


Public Class TabletLabelPrint
    Dim LOTID, IDApp As Integer
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim Coordinats, LOTInfo As ArrayList 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim SNFormat As ArrayList
    Dim SnArr As New ArrayList
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
                CB_Printer1.Items.Add(item.ToString())
            End If
        Next
        If CB_Printer1.Items.Count <> 0 Then
            CB_Printer1.Text = CB_Printer1.Items(0)
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
    'начало работы приложения FAS Scanning Station
#Region "Окно ввода серийного номера платы"
    Private Sub SerialTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SerialTextBox.KeyDown
        LB_CurrentErrCode.Text = ""
        Controllabel.Text = ""
        If e.KeyCode = Keys.Enter Then
            GetFTSN()
            If SNFormat(0) = True And CheckstepResult(GetPreStep(SNFormat(3)))(7) = True Then
                Print(GetLabelContent(GetContentToPrint(SNFormat(3)), 0, 0, PCInfo(6)), PCInfo(6))
                WriteToDB(SNFormat(3), SerialTextBox.Text, SnArr)
                SerialTextBox.Clear()
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
        SNFormat = New ArrayList()
        SNFormat = GetScanSNFormat(If(PCInfo(6) = 25, LOTInfo(8), LOTInfo(3)), SerialTextBox.Text, PCInfo(6))
        If SNFormat(0) = False Then
            PrintLabel(Controllabel, "Формат номера не определен!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
        End If
    End Sub
#End Region
#Region "2. Проверка предыдущего шага и загрузка данных о плате"
    Private Function GetPreStep(_snid As Integer) As ArrayList
        Dim newArr As New ArrayList
        If PCInfo(6) = 25 Then
            newArr = (SelectListString($"Use FAS select 
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num 
            FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  SNID  = {_snid}) tt
            where  tt.num = 1 "))
        ElseIf PCInfo(6) = 45 Then
            newArr = (SelectListString($"Use FAS select 
            tt.StepID,tt.TestResultID, 
            tt.PCBID,(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num 
            FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  PCBID  = {_snid}) tt
            where  tt.num = 1 "))
        End If

        Return newArr
    End Function
#End Region
#Region "3. Запись в базу"
    Private Sub WriteToDB(pcbid As Integer, pcbSN As String, snarr As ArrayList)
        RunCommand($"use fas         
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID], SNID)values
          ({pcbid},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},(SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{snarr(1)}'))")
        CurrentLogUpdate(ShiftCounter(), pcbSN, snarr(1), snarr(2), snarr(3), snarr(4))
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
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN1 As String, SN2 As String, SN3 As String, SN4 As String, SN5 As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN1, SN2, SN5, SN3, SN4, Date.Now)
        DG_UpLog.Sort(DG_UpLog.Columns(3), System.ComponentModel.ListSortDirection.Descending)
    End Sub
#End Region
#Region "9. Функция печати на этикетке"
    Private Sub Print(ByVal content As String, stepid As Integer)
        If CB_Printer1.Text <> "" And stepid = 45 Then
            RawPrinterHelper.SendStringToPrinter(CB_Printer1.Text, content)
        ElseIf CB_Printer1.Text <> "" And stepid = 25 Then
            RawPrinterHelper.SendStringToPrinter(CB_Printer1.Text, content)
        Else
            MsgBox("Принтер не выбран или не подключен")
        End If
    End Sub
#End Region
#Region "10. Определение и сохранение координат"
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
#Region "11. Функция определения результата этапа"
    Private Function CheckstepResult(prestep As ArrayList) As ArrayList
        If prestep.Count = 0 And StartStepID <> PCInfo(6) Then ' шаг не первый, но предыдущего результата нет
            PrintLabel(Controllabel, $"Номер {SerialTextBox.Text} не был зарегистрирован на ТНТ!", 12, 193, Color.Red)
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
            'Если плата в таблице OperLog имеет шаг совпадающий со станцией ОТК, результат равен 2 
        ElseIf prestep(0) = 3 And prestep(1) = 2 And PCInfo(6) = 45 Then 'And PCInfo(6) = 1 Плата имеет статус Prestep/2 (проверка предыдущего шага)
            PrintLabel(Controllabel, $"Номер {prestep(3)}  отправлен на печать!", 12, 193, Color.Green)
            prestep.Add(True)
            Return prestep
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
#Region "12. Функция определния данных для печати из базы данных"
    Private Function GetContentToPrint(pcbid As Integer)
        SnArr = SelectListString($"declare @pcbid as int = {pcbid} 
        if (SELECT pcbid FROM [FAS].[dbo].[CT_Aquarius] where LOTID = {LOTID} and pcbid = @pcbid) = @pcbid
             SELECT [ID],[SN],[IMEI],[MAC_BT],[MAC_WF],[LOTID],[IsPrinted],[RePrintCount] 
             FROM [FAS].[dbo].[CT_Aquarius] where LOTID = {LOTID} and pcbid = @pcbid;
        else 
             SELECT top(1) [ID],[SN],[IMEI],[MAC_BT],[MAC_WF],[LOTID],[IsPrinted],[RePrintCount] 
             FROM [FAS].[dbo].[CT_Aquarius] where LOTID = {LOTID} and IsPrinted = 0;")

        Dim sql As String = (If(SnArr(6) = False, $"Use FAS Update [FAS].[dbo].[CT_Aquarius] set IsPrinted = 1,PrintByID = {UserInfo(0)}, 
                  PrintDate = CURRENT_TIMESTAMP,pcbid = {pcbid} where id = {SnArr(0)}
                  insert into Ct_FASSN_reg ([SN],[LOTID],[UserID],[AppID],[LineID],[RegDate])values
                  ('{SnArr(1)}',{SnArr(5)},{UserInfo(0)},{IDApp},{PCInfo(2)},CURRENT_TIMESTAMP)",
                  $"Use FAS Update [FAS].[dbo].[CT_Aquarius] set IsRePrinted = 1, RePrintByID = {UserInfo(0)},RePrintDate = CURRENT_TIMESTAMP,
                  RePrintCount = {SnArr(7) + 1} where id = {SnArr(0)}"))
        RunCommand(sql)
        Return SnArr
    End Function
#End Region
    Private Function GetLabelContent(sn As ArrayList, x As Integer, y As Integer, StepID As Integer) As String
        If StepID = 45 Then
            PrintLabel(Controllabel, "Серийный номер " & sn(1) & vbCrLf &
                        "MAC WiFi " & sn(4) & vbCrLf &
                        "IMEI " & sn(2) & vbCrLf &
                        "MAC BT " & sn(3), 12, 192, Color.Green)
            Return $"
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
^FD>:" & Mid(sn(1), 1, 11) & ">5" & Mid(sn(1), 12) & "^FS
^BY2,3,47^FT255,213^BCN,,Y,N
^FD>;" & Mid(sn(2), 1, 14) & ">6" & Mid(sn(2), 15) & "^FS
^BY2,3,47^FT255,137^BCN,,Y,N
^FD>;" & Mid(sn(3), 1, 4) & ">6" & Mid(sn(3), 5) & "^FS
^FO44,234^GB597,78,3^FS
^FO44,158^GB597,78,3^FS
^FO44,82^GB597,78,3^FS
^FO44,7^GB597,78,3^FS
^BY2,3,47^FT255,288^BCN,,Y,N
^FD>;" & Mid(sn(4), 1, 4) & ">6" & Mid(sn(4), 5) & "^FS
^PQ1,0,1,Y^XZ"
        End If
    End Function
End Class
