Imports Library3
Imports System.Deployment.Application

Public Class Banch_S_Numbers_Smart
    Dim LOTID, IDApp As Integer
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim StartStep As String, PreStep As String, NextStep As String
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim LOTInfo As New ArrayList() 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim StepSequence As String()
    Dim PCBCheckRes As New ArrayList()
    Dim SNFormat As ArrayList, SNID As Integer
    Dim SNBufer As New ArrayList From {0, 0, 0, "", "", ""} 'SNBufer = (BOT,TOP,FAS,SNBot,SNTop,SNFAs )
#Region "Загрузка рабочей формы"
    Public Sub New(LOTIDWF As Integer, IDApp As Integer)
        InitializeComponent()
        Me.LOTID = LOTIDWF
        Me.IDApp = IDApp
    End Sub
    Private Sub Banch_S_Numbers_Smart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                If (SNBufer(2) <> 0) And SNBufer(1) <> 0 Then
                    Dim ResStep As Boolean = GetPreStep(SNBufer, PCInfo(6))
                    If ResStep = True Then
                        WriteToDB()
                    ElseIf ResStep = False And PCInfo(6) = 43 Then
                        PrintLabel(Controllabel, $"Плата с номерами: ТОР {SNBufer(4)} и ВОТ {SNBufer(3)} {vbCrLf}имеет не верный предыдущий шаг!", 12, 193, Color.Red)
                    ElseIf ResStep = False And PCInfo(6) = 30 Then
                        PrintLabel(Controllabel, $"Плата с номерами: ТОР {SNBufer(4)} и ВОТ {SNBufer(5)} {vbCrLf}имеет не верный предыдущий шаг!", 12, 193, Color.Red)
                    End If
                End If
            End If
        End If
    End Sub
#End Region
#Region "Кнопка очистки поля ввода номера"
    Private Sub BT_ClearSN_Click(sender As Object, e As EventArgs) Handles BT_ClearSN.Click
        BT_DisAssemble.Visible = False
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
        SNFormat = GetScanSNFormat(LOTInfo(19).Split(";")(0), LOTInfo(19).Split(";")(1), LOTInfo(19).Split(";")(2), SerialTextBox.Text, PCInfo(6))
        If (SNFormat(1) = 1) Or (SNFormat(1) = 2) Then
            SNID = SelectInt($"use SMDCOMPONETS select IDLaser  FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = '{SerialTextBox.Text}'")
        ElseIf SNFormat(1) = 3 Then
            SNID = SelectInt($"USE FAS Select [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{SerialTextBox.Text}'")
            If SNID = 0 Then
                SNID = SelectString($"insert into [FAS].[dbo].[Ct_FASSN_reg] ([SN],[LOTID],[UserID],[AppID],[LineID],[RegDate]) values
                            ('{SerialTextBox.Text}',{LOTID},11,11,11,CURRENT_TIMESTAMP)
                            Select [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{SerialTextBox.Text}'")
            End If
        End If
        If SNID > 0 Then
            If CheckSNBufer(SNFormat(1), SNID) = True Then
                Mess = SNFormat(4)
                Res = SNFormat(0)
                If SNBufer(2) = 0 Or SNBufer(1) = 0 Then
                    col = If(Res = False, Color.Red, Color.Green)
                    PrintLabel(Controllabel, Mess, 12, 193, col)
                End If
                SerialTextBox.Enabled = Res
                SerialTextBox.Clear()
            Else
                SerialTextBox.Enabled = False
            End If
        ElseIf SNID = 0 And PCInfo(6) = 30 And SNFormat(1) = 3 Then
            PrintLabel(Controllabel, $"Серийный номер FAS {SerialTextBox.Text} не зарегистрирован в базе!", 12, 193, Color.Red)
            Res = False
            SerialTextBox.Enabled = False
        End If
        Return Res
    End Function
#End Region
#Region "5. Проверка буффера SN"
    Private Function CheckSNBufer(format As Integer, snid As Integer) As Boolean
        Select Case format
            Case 2
                If SNBufer(0) = 0 Then
                    Return CheckBunchFAS(format, snid)
                ElseIf SNBufer(0) <> 0 And SNBufer(1) = 0 Then
                    If CheckBunch(format, snid) = True Then
                        PrintLabel(Controllabel, "Номера ВОТ и ТОР определены и записаны в базу!", 12, 193, Color.Green)
                        Return True
                    End If
                ElseIf SNBufer(0) = 0 And SNBufer(1) = snid Then
                    PrintLabel(Controllabel, $"Номер ВОТ {SNBufer(3)} уже был отсканирован.{vbCrLf}Выполните сброс и повторите сканирование номеров ТОР и ВОТ", 12, 193, Color.Red)
                    Return False
                End If
            Case 3
                If SNBufer(0) = 0 Then
                    Return CheckBunchFAS(format, snid)
                ElseIf SNBufer(0) <> 0 And SNBufer(1) = 0 Then
                    If CheckBunch(format, snid) = True Then
                        PrintLabel(Controllabel, "Номера ВОТ и ТОР определены и записаны в базу!", 12, 193, Color.Green)
                        Return True
                    End If
                ElseIf SNBufer(0) = 0 And SNBufer(1) = snid Then
                    PrintLabel(Controllabel, $"Номер ВОТ {SNBufer(3)} уже был отсканирован.{vbCrLf}Выполните сброс и повторите сканирование номеров ТОР и ВОТ", 12, 193, Color.Red)
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
            PrintLabel(Controllabel, $"Номер {If(formatIndex = 1, "ВОТ", "TOP")} {SerialTextBox.Text} уже был связан с номером {If(formatIndex = 2, "ВОТ", "TOP")} {SelectString($"SELECT (select Content from SMDCOMPONETS.dbo.LazerBase L 
               where L.IDLaser = B.{If(formatIndex = 2, "PCBIDBOT", "PCBIDTOP")} ) 
               FROM [FAS].[dbo].[FAS_Bunch_Decode] B
               where {If(formatIndex = 1, "PCBIDBOT", "PCBIDTOP")} = {snid}")}.{vbCrLf}Выполните сброс и повторите сканирование номеров ТОР и ВОТ", 12, 193, Color.Red)
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
                If tempArr.Count = 0 Then
                    RunCommand($"use fas
                      insert into [FAS].[dbo].[FAS_Bunch_Decode] ([PCBIDTOP],[Date],[UserID],[LOTID])values
                      ({snid},CURRENT_TIMESTAMP,{UserInfo(0)},{LOTID})")
                    tempArr = SelectListString($"SELECT PCBIDTOP,FASSNID FROM [FAS].[dbo].[FAS_Bunch_Decode] where PCBIDTOP =  {snid}")
                    'ElseIf
                End If
            ElseIf SNFormat(1) = 3 Then
                tempArr = SelectListString($"SELECT PCBIDTOP,PCBIDBOT,FASSNID FROM [FAS].[dbo].[FAS_Bunch_Decode] where FASSNID =  {snid}")
            End If
            If tempArr.Count > 0 Then
                'SNBufer
                If IsDBNull(tempArr(2)) Then
                    SNBufer(formatIndex - 1) = snid
                    SNBufer(formatIndex + 2) = SerialTextBox.Text
                    Return True
                ElseIf tempArr(0) <> 0 And tempArr(2) <> 0 Then
                    Dim fasSN As String = SelectString($"SELECT [SN] FROM [FAS].[dbo].[Ct_FASSN_reg] where  LOTID = {LOTID} And ID = {tempArr(2)}")
                    Dim topSN As String = SelectString($"select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser = {tempArr(0)}")
                    PrintLabel(Controllabel, $"Номер {If(formatIndex = 2, $"TOP {topSN}", $"FAS {fasSN}")} уже был связан с номером {If(formatIndex = 2, $"FAS {fasSN}", $"TOP {topSN}")}. {vbCrLf}Выполните сброс и повторите сканирование номеров ТОР и ВОТ{vbCrLf}Или произведите отвязку номеров.", 12, 193, Color.Red)
                    BT_DisAssemble.Visible = True
                    Return False
                End If
            Else
                SNBufer(formatIndex - 1) = snid
                SNBufer(formatIndex + 2) = SerialTextBox.Text
                'SNBufer
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
    Private Sub BT_DisAssemble_Click(sender As Object, e As EventArgs) Handles BT_DisAssemble.Click
        Dim SNarr As New ArrayList(SelectListString($"use fas   Select PCBIDTOP,FASSNID,(Select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =PCBIDTOP),
                        (select SN from Ct_FASSN_reg where id = FASSNID) 
                        FROM [FAS].[dbo].[FAS_Bunch_Decode]B where PCBIDTOP = {SNFormat(3)} or [FASSNID] = {SNFormat(3)}  "))
        RunCommand($"use fas
          Update [FAS].[dbo].[FAS_Bunch_Decode] set FASSNID = NULL where PCBIDTOP = {SNarr(0)} And LOTID = {LOTID}
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID], SNID)values
            ({SNarr(0)},{LOTID},48,2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{SNarr(1)})")
        BT_DisAssemble.Visible = False
        BT_ClearSN_Click(sender, e)
        PrintLabel(Controllabel, $"Номер {If(SNFormat(1) = 2, $"TOP {SNarr(2)}", $"FAS {SNarr(3)}")} успешно отвязан от номера {If(SNFormat(1) = 2, $"FAS {SNarr(3)}", $"TOP {SNarr(2)}")}!", 12, 193, Color.Green)

    End Sub
#End Region
#Region "3. Запись в базу"
    Private Sub WriteToDB()

        RunCommand($"use fas
            update [FAS].[dbo].[FAS_Bunch_Decode] set FASSNID = {SNBufer(2)} where PCBIDTOP = {SNBufer(1)} and LOTID = {LOTID}
            insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID], SNID)values
            ({SNBufer(1)},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{SNBufer(2)})
            update [FAS].[dbo].[CT_Aquarius] set PCBID = {SNBufer(1)} where SN = (select sn from Ct_FASSN_reg where id = {SNBufer(2)})")

        PrintLabel(Controllabel, $"Номера FAS и ТОР определены и записаны в базу!", 12, 193, Color.Green)
        CurrentLogUpdate(ShiftCounter(), SNBufer(5), SNBufer(4))
        SNBufer = New ArrayList From {0, 0, 0, "", "", ""}
    End Sub
#End Region
#Region "8. Функция запролнения LogGrid "
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN1 As String, SN2 As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN1, SN2, Date.Now)
        DG_UpLog.Sort(DG_UpLog.Columns(3), System.ComponentModel.ListSortDirection.Descending)
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
#Region "2. Проверка предыдущего шага и загрузка данных о плате"
    Private Function GetPreStep(_snbuf As ArrayList, stepId As Integer) As Boolean
        Dim newArr As ArrayList
        For i = 1 To 1
            newArr = New ArrayList(SelectListString($"Use FAS 
            select tt.PCBID,
            (select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
            tt.SNID, 
            (select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
            tt.StepID,tt.TestResultID, tt.StepDate 
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where  PCBID  = {_snbuf(i)}) tt
            where  tt.num = 1 "))
            If newArr.Count > 0 Then
                If newArr(4) = PreStepID Then
                    Return True
                    Exit For
                ElseIf newArr(4) = 48 Then
                    Return True
                    Exit For
                Else
                    Return False
                End If
            End If
        Next
    End Function
#End Region
End Class