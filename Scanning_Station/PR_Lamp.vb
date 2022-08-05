Imports Library3
Imports System.Deployment.Application
Imports System.Drawing.Printing
Imports System.IO

Public Class PR_Lamp
    Dim LOTID, IDApp As Integer
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim Coordinats, LOTInfo As New ArrayList() 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim PCBCheckRes As New ArrayList()
    Dim SNFormat As ArrayList, SNID As Integer
    Dim SNBufer As New ArrayList From {0, 0, 0, "", "", ""} 'SNBufer = (ДрайверID,LEDId,FAS,SNДрайвер,SN_LED,SNFAs )
#Region "Загрузка рабочей формы"
    Public Sub New(LOTIDWF As Integer, IDApp As Integer)
        InitializeComponent()
        Me.LOTID = LOTIDWF
        Me.IDApp = IDApp
    End Sub
    Private Sub PR_Lamp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        SNID = 0
        LB_CurrentErrCode.Text = ""
        Controllabel.Text = ""
        Dim Mess As New ArrayList()
        If e.KeyCode = Keys.Enter Then
            Dim _stepArr As ArrayList
            If GetFTSN() = True Then
                If PCInfo(6) = 51 Then
                    If (SNBufer(0) <> 0 Or SNBufer(2) <> 0) And SNBufer(1) <> 0 Then
                        Dim ResStep As Boolean = GetPreStep(SNBufer, PCInfo(6))
                        If ResStep = True Then
                            If getLabelSN().Count > 0 Then
                                Print(GetLabelContent(SNBufer(5), 0, 0))
                                WriteToDB()
                            Else
                                PrintLabel(Controllabel, $"Номер для печати не найден в базе!", 12, 193, Color.Red)
                            End If
                        ElseIf ResStep = False Then
                            PrintLabel(Controllabel, $"Светильник с номерами: Драйвер {SNBufer(4)} и LED {SNBufer(3)} {vbCrLf}имеет не верный предыдущий шаг!", 12, 193, Color.Red)
                        End If
                    End If
                Else
                    Dim ResStep As Boolean = GetPreStep(SNBufer, PCInfo(6))
                    If ResStep = True Then
                        If PCInfo(6) = 51 Then
                            If getLabelSN().Count > 0 Then
                                Print(GetLabelContent(SNBufer(5), 0, 0))
                                WriteToDB()
                            Else
                                PrintLabel(Controllabel, $"Номер для печати не найден в базе!", 12, 193, Color.Red)
                            End If
                        Else
                            WriteToDB()
                        End If
                    ElseIf ResStep = False Then
                        PrintLabel(Controllabel, $"Светильник с номерами: Драйвер {SNBufer(4)} и LED {SNBufer(3)} {vbCrLf}имеет не верный предыдущий шаг!", 12, 193, Color.Red)
                    End If
                End If
            End If
        End If
    End Sub
#End Region
#Region "Кнопка очистки поля ввода номера"
    Private Sub BT_ClearSN_Click(sender As Object, e As EventArgs) Handles BT_ClearSN.Click
        If SerialTextBox.Text = "" Then
            LB_CurrentErrCode.Text = ""
            Controllabel.Text = ""
        End If
        If GB_PCBInfoMode.Visible = False Then
            SerialTextBox.Clear()
            Controllabel.Text = ""
            SerialTextBox.Enabled = True
            SNBufer = New ArrayList From {0, 0, 0, "", "", ""}
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
#Region "1. Определение формата номера"
    Public Function GetFTSN() As Boolean
        Dim col As Color, Mess As String, Res As Boolean
        SNFormat = New ArrayList()
        SNFormat = GetPrScanSNFormat(LOTInfo(19).Split(";")(0), LOTInfo(19).Split(";")(1), LOTInfo(19).Split(";")(2), SerialTextBox.Text)
        If SNFormat.Count > 4 Then
            If SNFormat(1) = 2 And SNFormat(3) = "" Then
                SNFormat(3) = SelectString($"use SMDCOMPONETS
                declare @Cont as nvarchar(50) = '{SerialTextBox.Text}'
                declare @IdLaz as int = (select IDLaser from SMDCOMPONETS.dbo.LazerBase where Content = @Cont)
                if  @IdLaz  is null
                insert into [SMDCOMPONETS].[dbo].[LazerBase](ID,LogDate,ProductName,BoardID,Content,Marked,Result,InsertionDateTime,PCID) values  
                (1,CURRENT_TIMESTAMP,'Manual',1,@Cont,1,'Manual',CURRENT_TIMESTAMP,1)
                select IDLaser from SMDCOMPONETS.dbo.LazerBase where Content = @Cont
                ")
            End If
            SNID = SNFormat(3)
        Else
            If CB_Reprint.Checked = True Then
                Print(GetLabelContent(SerialTextBox.Text, 0, 0))
                PrintLabel(Controllabel, $"Номер {SerialTextBox.Text} повторно отправлен на печать!", 12, 193, Color.Green)
                SerialTextBox.Clear()
                CB_Reprint.Checked = False
                SerialTextBox.Focus()
            Else
                PrintLabel(Controllabel, $"Формат номера не определен!", 12, 193, Color.Red)
            End If
            Return False
            Exit Function
        End If
        If SNID > 0 Then
            If PCInfo(6) = 51 Then
                If CheckSNBufer(SNFormat(1), SNID) = True Then
                    Mess = SNFormat(4)
                    Res = SNFormat(0)
                    If SNBufer(0) = 0 Or SNBufer(1) = 0 Then
                        col = If(Res = False, Color.Red, Color.Green)
                        PrintLabel(Controllabel, Mess, 12, 193, col)
                    End If
                    SerialTextBox.Enabled = Res
                    SerialTextBox.Clear()
                Else
                    SerialTextBox.Enabled = False
                End If
            Else
                Mess = SNFormat(4)
                Res = SNFormat(0)
                SNBufer(0) = SNFormat(3)
                SNBufer(3) = SerialTextBox.Text
                col = If(Res = False, Color.Red, Color.Green)
                PrintLabel(Controllabel, Mess, 12, 193, col)
                SerialTextBox.Enabled = Res
                SerialTextBox.Clear()
            End If
        ElseIf SNID = 0 And PCInfo(6) = 30 And SNFormat(1) = 3 Then
            PrintLabel(Controllabel, $"Серийный номер FAS {SerialTextBox.Text} не зарегистрирован в базе!", 12, 193, Color.Red)
            Res = False
            SerialTextBox.Enabled = False
        End If
        Return Res
    End Function
#End Region
#Region "2. Проверка предыдущего шага и загрузка данных о плате"
    Private Function GetPreStep(_snbuf As ArrayList, stepId As Integer) As Boolean
        Dim newArr As ArrayList
        For i = 0 To 1
            newArr = New ArrayList(SelectListString($"Use FAS 
            select tt.PCBID,
            (select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, 
            (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepID,tt.TestResultID, tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where  {If(stepId = 51, "PCBID", "SNID")}  = {_snbuf(i)}) tt
            where  tt.num = 1 "))
            If newArr.Count > 0 Then
                SNBufer(1) = newArr(0)
                If PCInfo(6) = 51 Then
                    If newArr(4) = 51 Or newArr(4) = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                    Exit For
                ElseIf PCInfo(6) = 52 Then
                    If newArr(4) = 51 Or newArr(4) = 52 Then
                        Return True
                    Else
                        Return False
                    End If
                    Exit For
                ElseIf PCInfo(6) = 53 Then
                    If newArr(4) = 53 Or newArr(4) = 52 Then
                        Return True
                    Else
                        Return False
                    End If
                    Exit For
                End If
            End If
        Next
        If newArr.Count = 0 Then
            Return True
        End If
    End Function


#End Region
#Region "3. Запись в базу"
    Private Sub WriteToDB()
        If PCInfo(6) = 51 Then
            RunCommand($"use fas
          insert into [FAS].[dbo].[FAS_Bunch_Decode] ([PCBIDTOP],[PCBIDBOT],[Date],[UserID],[LOTID],[FASSNID])values
          ({SNBufer(1)},{SNBufer(0)},CURRENT_TIMESTAMP,{UserInfo(0)},{LOTID},{SNBufer(2)})
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],[SNID])values
          ({SNBufer(0)},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{SNBufer(2)})")
            PrintLabel(Controllabel, $"Номера LED и Драйвер определены и записаны в базу!", 12, 193, Color.Green)
            CurrentLogUpdate(ShiftCounter(), SNBufer(3), SNBufer(4), SNBufer(5))
        Else
            RunCommand($"use fas
            insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],[SNID])values
            ({SNBufer(1)},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{SNBufer(0)})")
            PrintLabel(Controllabel, $"{If(PCInfo(6) = 53, "Высоковольтный тест пройден!", "Измерение мощности пройдено!")}", 12, 193, Color.Green)
            CurrentLogUpdate(ShiftCounter(), SNBufer(3), SNBufer(4), SNBufer(5))
        End If

        SNBufer = New ArrayList From {0, 0, 0, "", "", ""}
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
#Region "5. Проверка буффера SN"
    Private Function CheckSNBufer(format As Integer, snid As Integer) As Boolean
        Select Case format
            Case 1
                If SNBufer(0) = 0 Then
                    PrintLabel(Controllabel, $"Номер LED {SNBufer(3)} определен.{vbCrLf}Отсканируйте номер драйвера", 12, 193, Color.Red)
                    Return CheckBunch(format, snid)
                ElseIf SNBufer(0) = snid And SNBufer(1) = 0 Then
                    PrintLabel(Controllabel, $"Номер LED {SNBufer(3)} уже был отсканирован.{vbCrLf}Выполните сброс и повторите сканирование номеров LED и Драйвера", 12, 193, Color.Red)
                    Return False
                End If
            Case 2
                If SNBufer(1) = 0 Then
                    PrintLabel(Controllabel, $"Номер драйвера {SNBufer(3)} определен.{vbCrLf}Отсканируйте номер LED", 12, 193, Color.Red)
                    Return CheckBunch(format, snid)
                ElseIf SNBufer(0) <> 0 And SNBufer(1) = 0 Then
                    If CheckBunch(format, snid) = True Then
                        PrintLabel(Controllabel, "Номера ВОТ и ТОР определены и записаны в базу!", 12, 193, Color.Green)
                        Return True
                    End If
                ElseIf SNBufer(0) = 0 And SNBufer(1) = snid Then
                    PrintLabel(Controllabel, $"Номер Драйвера {SNBufer(3)} уже был отсканирован.{vbCrLf}Выполните сброс и повторите сканирование номеров LED и Драйвера", 12, 193, Color.Red)
                    Return False
                End If
        End Select
    End Function
#End Region
#Region "6. Проверка связки SN ТОР ВОТ"
    Private Function CheckBunch(formatIndex As Integer, snid As Integer)
        If SNBufer(formatIndex - 1) = 0 And SelectInt($"SELECT ID FROM [FAS].[dbo].[FAS_Bunch_Decode] where {If(formatIndex = 1, "PCBIDBOT", "PCBIDTOP")} = {snid}") = 0 Then
            SNBufer(formatIndex - 1) = snid
            SNBufer(formatIndex + 2) = SerialTextBox.Text
            Return True
        Else
            PrintLabel(Controllabel, $"Номер {If(formatIndex = 1, "LED", "Драйвер")} {SerialTextBox.Text} уже был связан с номером {If(formatIndex = 2, "Драйвер", "LED")} {SelectString($"SELECT (select Content from SMDCOMPONETS.dbo.LazerBase L 
               where L.IDLaser = B.{If(formatIndex = 2, "PCBIDBOT", "PCBIDTOP")} ) 
               FROM [FAS].[dbo].[FAS_Bunch_Decode] B
               where {If(formatIndex = 1, "PCBIDBOT", "PCBIDTOP")} = {snid}")}.{vbCrLf}Выполните сброс и повторите сканирование номеров LED и Драйвер", 12, 193, Color.Red)
            Return False
        End If
    End Function
#End Region
#Region "7. Проверка связки SN ТОР FAS"
    Private Function CheckBunchFAS(formatIndex As Integer, snid As Integer)
        If SNBufer(formatIndex - 1) = 0 Then
            Dim tempArr As New ArrayList()
            If SNFormat(1) = 2 Then
                tempArr = SelectListString($"SELECT PCBIDTOP,PCBIDBOT,FASSNID FROM [FAS].[dbo].[FAS_Bunch_Decode] where PCBIDTOP =  {snid}")
            ElseIf SNFormat(1) = 3 Then
                tempArr = SelectListString($"SELECT PCBIDTOP,PCBIDBOT,FASSNID FROM [FAS].[dbo].[FAS_Bunch_Decode] where FASSNID =  {snid}")
            End If
            If tempArr.Count > 0 Then
                If IsDBNull(tempArr(2)) Then
                    SNBufer(formatIndex - 1) = snid
                    SNBufer(formatIndex + 2) = SerialTextBox.Text
                    Return True
                ElseIf tempArr(2) <> 0 Then
                    Dim fasSN As String = SelectString($"SELECT [SN] FROM [FAS].[dbo].[Ct_FASSN_reg] where  LOTID = {LOTID} And ID = {tempArr(2)}")
                    Dim topSN As String = SelectString($"select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser = {tempArr(0)}")
                    PrintLabel(Controllabel, $"Номер {If(formatIndex = 2, $"TOP {topSN}", $"FAS {fasSN}")} уже был связан с номером {If(formatIndex = 2, $"FAS {fasSN}", $"TOP {topSN}")}. {vbCrLf}Выполните сброс и повторите сканирование номеров ТОР и ВОТ", 12, 193, Color.Red)
                    Return False
                End If
            Else
                SNBufer(formatIndex - 1) = snid
                SNBufer(formatIndex + 2) = SerialTextBox.Text
                Return True
            End If
        ElseIf SNBufer(formatIndex - 1) <> 0 Then
            PrintLabel(Controllabel, $"Номер {If(formatIndex = 3, "FAS", "TOP")} {SerialTextBox.Text} уже был связан с номером 
                {If(formatIndex = 2, "FAS", "TOP")} {SelectString($"SELECT (select Content from SMDCOMPONETS.dbo.LazerBase L 
                where L.IDLaser = B.{If(formatIndex = 2, "FASSNID", "PCBIDTOP")} ) 
                FROM [FAS].[dbo].[FAS_Bunch_Decode] B
                where {If(formatIndex = 3, "FAS", "TOP")} = {snid}")}.{vbCrLf}Выполните сброс и повторите сканирование номеров ТОР и ВОТ", 12, 193, Color.Red)
            Return False
        End If
    End Function
#End Region
#Region "8. Функция запролнения LogGrid "
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN1 As String, SN2 As String, SN3 As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN1, SN2, SN3, Date.Now)
        DG_UpLog.Sort(DG_UpLog.Columns(4), System.ComponentModel.ListSortDirection.Descending)
    End Sub
#End Region
#Region "9. Функция выбора номера для печати "
    Private Function getLabelSN() As ArrayList
        Dim tpArray As New ArrayList(
        SelectListString($"use fas
         declare @PrintSN as nvarchar (16) = (select top 1 [Lamp_SN] FROM [FAS].[dbo].[PR_Lamp_SN] where [IsUsed] = 0)
         update [FAS].[dbo].[PR_Lamp_SN] set [IsUsed] = 1, [PrintDate] = CURRENT_TIMESTAMP, [UserID] = 11 where  [Lamp_SN] =  @PrintSN --[IsUsed] = 1
         declare @SNRegID as int = (SELECT ID FROM [FAS].[dbo].[Ct_FASSN_reg] where [SN] = @PrintSN)
         if (SELECT ID FROM [FAS].[dbo].[Ct_FASSN_reg] where [SN] = @PrintSN) is null
         insert into [FAS].[dbo].[Ct_FASSN_reg] values (@PrintSN,20223,11,26,9,CURRENT_TIMESTAMP)
         SELECT ID, SN FROM [FAS].[dbo].[Ct_FASSN_reg] where [SN] = @PrintSN"))
        SNBufer(2) = tpArray(0)
        SNBufer(5) = tpArray(1)
        Return tpArray
    End Function
#End Region
#Region "10. Функция печати на этикетке"
    Private Sub Print(ByVal content As String)
        If CB_DefaultPrinter.Text <> "" Then
            RawPrinterHelper.SendStringToPrinter(CB_DefaultPrinter.Text, content)
        Else
            MsgBox("Принтер не выбран или не подключен")
        End If
    End Sub
#End Region
#Region "11. Определение и сохранение координат"
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
#Region "12. GetLabelContent"
    Private Function GetLabelContent(sn As String, x As Integer, y As Integer) As String
        Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW650
^LL0201
^LS0
^BY3,3,107^FT{81 + x},{124 + y}^BCN,,N,N
^FD>:{Mid(sn, 1, 8)}>5{Mid(sn, 9)}^FS
^FT{174 + x},{172 + y}^A0N,46,45^FH\^FD{sn}^FS
^PQ3,0,1,Y^XZ
"
    End Function
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
        BT_ClearSN_Click(sender, e)
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
                declare @content as nvarchar(50) = '{TB_GetPCPInfo.Text}'
                declare @pcbid as int
                declare @i int = 1
                select @pcbid = (SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = @content)
                    if  @pcbid is null
                        select @pcbid = (select [PCBIDTOP]  FROM [FAS].[dbo].[FAS_Bunch_Decode] where FASSNID = (select ID from Ct_FASSN_reg where sn = @content))
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
                where PCBID  = @pcbid) tt
                order by num")
        TB_GetPCPInfo.Enabled = False


    End Sub
#End Region
End Class




