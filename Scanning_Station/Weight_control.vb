Imports System.Deployment.Application
Imports Library3

Public Class Weight_control
#Region "Переменные"
    Dim LOTID, IDApp, MaxW, MinW As Integer
    Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
    Dim LOTInfo As New ArrayList() 'LOTInfo = (Model,LOT,SMTRangeChecked,SMTStartRange,SMTEndRange,ParseLog)
    Dim UserInfo As New ArrayList() 'UserInfo = ( UserID, Name, User Group)
    Dim ShiftCounterInfo As New ArrayList() 'ShiftCounterInfo = (ShiftCounterID,ShiftCounter,LOTCounter)
    Dim LenSN, StartStepID As Integer, PreStepID As Integer, NextStepID As Integer
    Dim SNFormat As ArrayList
#End Region
#Region "Load form"
    Public Sub New(LOTIDWF As Integer, IDApp As Integer)
        InitializeComponent()
        Me.LOTID = LOTIDWF
        Me.IDApp = IDApp
    End Sub
    Private Sub Weight_control_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myVersion As Version
        If ApplicationDeployment.IsNetworkDeployed Then
            myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion
        End If
        LB_SW_Wers.Text = String.Concat("v", myVersion)
        PrintLabel(Controllabel, "", 12, 234, Color.Red)
        PrintLabel(LB_defoltWht, "", 12, 234, Color.Red)
        CB_COMPorts_Click(sender, e) 'обновление списка СОМ портов
        'Проверка подключенных весов

        'WeightSerialPort.PortName = "COM23"
        WeightSerialPort.BaudRate = 9600
        'Try
        '    WeightSerialPort.Open()
        'Catch ex As Exception
        '    PrintLabel(Controllabel, $"COM23 не найден.{vbCrLf}Проверьте подключение COM порта. {vbCrLf}Требуется перезапуск приложения!", 11, 262, Color.Red)
        '    Exit Sub
        'End Try
        'получение данных о станции
        LoadGridFromDB(DG_StepList, "USE FAS SELECT [ID],[StepName],[Description] FROM [FAS].[dbo].[Ct_StepScan]")

        PCInfo = GetPCInfo(IDApp)
        LabelAppName.Text = PCInfo(1)
        Label_StationName.Text = PCInfo(5)
        LB_CurrentStep.Text = PCInfo(7)
        Lebel_StationLine.Text = PCInfo(3)
#Region "Расшифровка PCInfo"
        '"App_ID = " & PCInfo(0) & vbCrLf &
        '"App_Caption = " & PCInfo(1) & vbCrLf &
        '"lineID = " & PCInfo(2) & vbCrLf &
        '"LineName = " & PCInfo(3) & vbCrLf &
        '"StationID = " & PCInfo(4) & vbCrLf &
        '"StationName = " & PCInfo(5) & vbCrLf &
        '"CT_ScanStepID = " & PCInfo(6) & vbCrLf &
        '"CT_ScanStep = " & PCInfo(7) & vbCrLf 'PCInfo
#End Region
        LOTInfo = GetCurrentContractLot(LOTID)
        LenSN = GetLenSN(LOTInfo(8))
#Region "LOT Info Расшифровка"
        '"Model = " & LOTInfo(0) & vbCrLf &
        '"LOT = " & LOTInfo(1) & vbCrLf &
        '"CheckFormatSN_SMT = " & LOTInfo(2) & vbCrLf &
        '"SMTNumberFormat = " & LOTInfo(3) & vbCrLf &
        '"SMTRangeChecked = " & LOTInfo(4) & vbCrLf &
        '"SMTStartRange = " & LOTInfo(5) & vbCrLf &
        '"SMTEndRange = " & LOTInfo(6) & vbCrLf &
        '"CheckFormatSN_FAS = " & LOTInfo(7) & vbCrLf &
        '"FASNumberFormat = " & LOTInfo(8) & vbCrLf &
        '"FASRangeChecked = " & LOTInfo(9) & vbCrLf &
        '"FASStartRange = " & LOTInfo(10) & vbCrLf &
        '"FASEndRange = " & LOTInfo(11) & vbCrLf &
        '"SingleSN = " & LOTInfo(12) & vbCrLf &
        '"ParseLog = " & LOTInfo(13) & vbCrLf &
        '"StepSequence = " & LOTInfo(14) & vbCrLf &
        '"BoxCapacity = " & LOTInfo(15) & vbCrLf &
        '"PalletCapacity = " & LOTInfo(16) & vbCrLf &
        '"LiterIndex = " & LOTInfo(17) & vbCrLf &
        '"PreRackStage = " & LOTInfo(18) &
        '"LenSN = " & LenSN & vbCrLf 'LOTInfo
#End Region
        L_LOT.Text = LOTInfo(1)
        L_Model.Text = LOTInfo(0)

        GB_AutoSettings.Visible = True
        GB_AutoSettings.Location = New Point(10, 12)
        TB_AutoSetSNin.Focus()
        'запуск счетчика продукции за день
        CurrentTimeTimer.Start()
        ShiftCounterInfo = ShiftCounterStart(PCInfo(4), IDApp, LOTID)
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_LOTCounter.Text = ShiftCounterInfo(2)
        'Запуск программы
        '___________________________________________________________
        'GB_UserData.Location = New Point(10, 12)
        'TB_RFIDIn.Focus()
    End Sub
#End Region
#Region "определение списка COM портов"
    Private Function CB_COMPorts_Click(sender As Object, e As EventArgs) Handles CB_COMPorts.Click
        CB_COMPorts.Items.Clear()
        CB_COMPorts.Text = ""
        CB_COMPorts.Items.AddRange(IO.Ports.SerialPort.GetPortNames())
        If CB_COMPorts.Items.Count <> 0 Then
            CB_COMPorts.Text = CB_COMPorts.Items(0)
            PrintLabel(LB_defoltWht, "", 16, 315, Color.Red)
            TB_AutoSetSNin.Enabled = True
            Return True
        Else
            PrintLabel(LB_defoltWht, "Ни один COM порт не подключен!", 16, 315, Color.Red)
            TB_AutoSetSNin.Enabled = False
            Return False
        End If
    End Function
#End Region
#Region "Определение номера сом порта для взвешивания"
    Private Sub CB_COMPorts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CB_COMPorts.SelectedIndexChanged
        If CB_COMPorts.Text <> "" Then
            WeightSerialPort.PortName = CB_COMPorts.Text
        End If
    End Sub
#End Region
#Region "Часы в программе"    'Часы в программе
    Private Sub CurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles CurrentTimeTimer.Tick
        CurrrentTimeLabel.Text = TimeString
    End Sub 'Часы в программе
#End Region
#Region "регистрация пользователя"     'регистрация пользователя
    Private Sub TB_RFIDIn_KeyDown(sender As Object, e As KeyEventArgs) Handles TB_RFIDIn.KeyDown
        TB_RFIDIn.MaxLength = 10
        If e.KeyCode = Keys.Enter And TB_RFIDIn.TextLength = 10 Then ' если длина номера равна 10, то запускаем процесс
            UserInfo = GetUserData(TB_RFIDIn.Text, GB_UserData, GB_WorkAria, L_UserName, TB_RFIDIn)
            '__UserID =  UserInfo(0) 
            '__Name =  UserInfo(1) 
            '__User Group =  UserInfo(2)
            GB_AutoSettings.Visible = False
            SerialTextBox.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            TB_RFIDIn.Clear()
        End If
    End Sub 'регистрация пользователя
#End Region
#Region "условия для возврата в окно настроек"  ' условия для возврата в окно настроек
    Dim OpenSettings As Boolean
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles BT_OpenSettings.Click, BT_LogOut.Click
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
#Region "Установка дефорлта для весов"  ' условия для возврата в окно настроек
    Private Sub TB_AutoSetSNin_KeyDown(sender As Object, e As KeyEventArgs) Handles TB_AutoSetSNin.KeyDown
        If e.KeyCode = Keys.Enter And TB_AutoSetSNin.TextLength = LenSN Then
            Dim Ref_Wght = GetWeight()
            If Ref_Wght > 0 Then
                MaxW = Ref_Wght + Num_Deviation.Value
                MinW = Ref_Wght - Num_Deviation.Value
                PrintLabel(LB_defoltWht, $"Этолонная масса установлена!{ vbCrLf }MinW: {MinW}; MaxW: {MaxW}", 10, 280, Color.Green)
                'Запуск программы
                '___________________________________________________________
                GB_UserData.Visible = True
                GB_UserData.Enabled = True
                GB_UserData.Location = New Point(10, 400)
                TB_RFIDIn.Focus()
            Else
                PrintLabel(LB_defoltWht, $"Этолонная масса не установлена!", 10, 280, Color.Red)
                Exit Sub
            End If

        Else
            PrintLabel(LB_defoltWht, "Номер не соответствует формату!", 10, 280, Color.Red)
        End If
    End Sub
#End Region
#Region "Обращение к весам"  ' условия для возврата в окно настроек
    Private Function GetWeight() As Integer
        Dim sender As Object, e As EventArgs
        Dim ResHex, ResHexOut, ResText As String, intSize As Integer
        Dim arrBuffer() As Byte = New Byte(1024) {}
        WeightSerialPort.Open()
        For i = 1 To 3
            WeightSerialPort.Write("000000" & vbCr)
            System.Threading.Thread.Sleep(100)
            While WeightSerialPort.BytesToRead() > 0
                intSize = WeightSerialPort.Read(arrBuffer, 0, 1024)
            End While
            ResHex += System.BitConverter.ToString(arrBuffer, 0, intSize) '/ Читаем ответ приемника из СОМ порта. Переводим массив в текст
            ResHex = Replace(ResHex, "-", "") '/ Удаляем лишние символы (бит конвертер добавляет в текст символ "-")
            If InStr(ResHex, "0D1E") = 43 And i < 4 Then
                ResHexOut = Mid(ResHex, 29, 6) '/ выбираем из полученного ответа символы соответствующие SCID в HEX 
                For x = 0 To ResHexOut.Length - 1 Step 2 '/ Цикл чтения по два символа из выделенной строки
                    ResText &= ChrW(CInt("&H" & ResHexOut.Substring(x, 2))) '/ Перевод значения TextHex в Text
                Next
                WeightSerialPort.Write("!" & vbCr)
                Exit For
            End If
        Next
        WeightSerialPort.Close()
        Return If(ResText = Nothing, 0, Int32.Parse(ResText))
        'Return 45
    End Function
#End Region

#Region "Обработка окна ввода серийного номера" 'окно ввода серийного номера платы 
    Dim SNID As Integer
    Private Sub SerialTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SerialTextBox.KeyDown
        Dim CurW, Msg, _descrp As String, Coll As Color
        If e.KeyCode = Keys.Enter Then
            'определение формата номера
            If GetFTSN() = True Then
                Dim _stepArr As ArrayList = New ArrayList(GetPreStep(SNID))
                If _stepArr.Count = 0 Then
                    PrintLabel(Controllabel, SerialTextBox.Text & " не не был зарегистрирован на FAS Start!", 12, 234, Color.Red)
                    Return
                ElseIf _stepArr.Count > 0 And _stepArr(4) = 25 And _stepArr(5) = 2 And _stepArr(0) IsNot DBNull.Value Then
                    CurW = GetWeight()
                    If CurW >= MinW And CurW <= MaxW Then
                        Msg = $"{SerialTextBox.Text}  взвешен,{vbCrLf}Масса приемника {CurW} грамм."
                        Coll = Color.Green
                        OperLogInsert(_stepArr(0), _stepArr(2), PCInfo(6), 2, Msg, CurW, $"{MinW};{MaxW}")
                    ElseIf CurW < MinW Then
                        Msg = $"{SerialTextBox.Text}  взвешен,{vbCrLf}Масса приемника {CurW} меньше минимальной!"
                        Coll = Color.Red
                    ElseIf CurW > MaxW Then
                        Msg = $"{SerialTextBox.Text}  взвешен,{vbCrLf}Масса приемника {CurW} больше максимальной!"
                        Coll = Color.Red
                    End If
                    PrintLabel(Controllabel, Msg, 12, 180, Coll)
                    ShiftCounter()
                    CurrentLogUpdate(ShiftCounterInfo(1), SerialTextBox.Text, CurW)
                    SerialTextBox.Clear()
                ElseIf _stepArr.Count > 0 And _stepArr(4) = 37 And _stepArr(5) = 2 And _stepArr(0) IsNot DBNull.Value Then
                    If MsgBox("Повторить взвешивание?", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                        CurW = GetWeight()
                        If CurW >= MinW And CurW <= MaxW Then
                            Msg = $"{SerialTextBox.Text}  взвешен, {vbCrLf}Масса приемника {CurW} грамм."
                            Coll = Color.Green
                            OperLogUpd(_stepArr(0), _stepArr(2), PCInfo(6), 2, Msg, CurW, $"{MinW};{MaxW}")
                        ElseIf CurW < MinW Then
                            Msg = $"{SerialTextBox.Text}  взвешен, {vbCrLf}Масса приемника {CurW} меньше минимальной!"
                            Coll = Color.Red
                        ElseIf CurW > MaxW Then
                            Msg = $"{SerialTextBox.Text}  взвешен, {vbCrLf}Масса приемника {CurW} больше максимальной!"
                            Coll = Color.Red
                        End If
                        PrintLabel(Controllabel, Msg, 12, 180, Coll)
                        ShiftCounter()
                        CurrentLogUpdate(ShiftCounterInfo(1), SerialTextBox.Text, CurW)
                        SerialTextBox.Clear()
                    End If
                Else
                    PrintLabel(Controllabel, SerialTextBox.Text & " имеет не верный шаг!", 12, 234, Color.Red)
                    SerialTextBox.Enabled = False
                End If
            Else
                'если введен не верный номер
                PrintLabel(Controllabel, $"{SerialTextBox.Text}  не соответствует шаблону!", 12, 180, Color.Red)
                SerialTextBox.Enabled = False
                BT_Pause.Focus()
            End If
        End If

    End Sub
#End Region
#Region "'1. Определение формата номера"
    Public Function GetFTSN() As Boolean
        Dim col As Color, Mess As String, Res As Boolean
        SNFormat = New ArrayList()
        SNFormat = GetSNFormat(LOTInfo(3), LOTInfo(8), LOTInfo(19), SerialTextBox.Text, LOTInfo(18), LOTInfo(2), LOTInfo(7))
        Res = SNFormat(0)
        Mess = SNFormat(3)
        col = If(Res = False, Color.Red, Color.Green)
        PrintLabel(Controllabel, Mess, 12, 193, col)
        SerialTextBox.Enabled = Res
        SNID = If(SNFormat(1) = 2,
                SelectInt($"USE FAS Select [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{SerialTextBox.Text}'"),
                SelectInt($"USE FAS SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where LOTID = {LOTID} and right (SN, 7) = '{CInt("&H" & Mid(SerialTextBox.Text, 7, 6))}'"))
        Return Res
    End Function
#End Region
#Region "Get SNID from CT_OperLog"
    Private Function AddSNToDB(SN As String) As Integer
        Dim _SNID As Integer
        _SNID = SelectInt("USE FAS SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '" & SN & "'")
        If _SNID = 0 Then
            _SNID = SelectInt($"USE FAS {vbCrLf}
                       insert into [FAS].[dbo].[Ct_FASSN_reg] ([SN],[LOTID],[UserID],[AppID],[LineID],[RegDate]) values{vbCrLf}
                       ('{SN}',{ LOTID },{UserInfo(0)},{PCInfo(0)},{PCInfo(2)}, CURRENT_TIMESTAMP){vbCrLf}
                       WAITFOR delay '00:00:00:100'{vbCrLf}
                       SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{SN}'")
        End If
        Return _SNID
    End Function
#End Region
#Region "Кнопка очистки поля ввода номера"
    Private Sub BT_CleareSN_Click(sender As Object, e As EventArgs) Handles BT_CleareSN.Click
        PrintLabel(Controllabel, "", Color.Black)
        SerialTextBox.Clear()
        SerialTextBox.Enabled = True
        SerialTextBox.Focus()
    End Sub
#End Region
#Region "запись в опер лог DB и Лога в программе"
    Private Sub OperLogUpd(_SNID As Integer, StepID As Integer, StepRes As Integer, Descr As String)
        RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[SNID],[Descriptions])values
                    ({LOTID},{StepID},{StepRes},CURRENT_TIMESTAMP,{ UserInfo(0) },{ PCInfo(2) },{ _SNID },'{ Descr }')")
    End Sub
    Private Sub OperLogInsert(_PCBID As Integer, _SNID As Integer, StepID As Integer, StepRes As Integer, Descr As String, weight As Decimal, MinMax As String)
        RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[SNID],[Descriptions])values
                    ({_PCBID},{LOTID},{StepID},{StepRes},CURRENT_TIMESTAMP,{ UserInfo(0) },{ PCInfo(2) },{ _SNID },'{ Descr }')")
        RunCommand($"insert into [FAS].[dbo].[CT_WeightStation] ([SNID],[CurWght],[WeightDate],[WeightByID],[Deviation],[LOTID]) values({_SNID },{weight},CURRENT_TIMESTAMP,{ UserInfo(0) }, '{MinMax}',{LOTID})")
    End Sub
    Private Sub OperLogUpd(_PCBID As Integer, _SNID As Integer, StepID As Integer, StepRes As Integer, Descr As String, weight As Decimal, MinMax As String)
        RunCommand($"insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
                    [StepByID],[LineID],[SNID],[Descriptions])values
                    ({_PCBID},{LOTID},{StepID},{StepRes},CURRENT_TIMESTAMP,{ UserInfo(0) },{ PCInfo(2) },{ _SNID },'{ Descr }')")
        RunCommand($"update [FAS].[dbo].[CT_WeightStation] SET [CurWght] = {weight},[WeightDate] = CURRENT_TIMESTAMP,[WeightByID]= { UserInfo(0) },[Deviation] = '{MinMax}',[LOTID] = {LOTID} where [SNID] = {_SNID }")
    End Sub
    'Функция заполнения LogGrid 
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN As String, Wgt As Integer)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN, Wgt, Date.Now)
        DG_UpLog.Sort(DG_UpLog.Columns(3), System.ComponentModel.ListSortDirection.Descending)
    End Sub
#End Region
#Region "Проверка предыдущего шага"
    Private Function GetPreStep(_SnID As Integer) As ArrayList
        Dim newArr As ArrayList = New ArrayList(SelectListString($"Use FAS 
select tt.PCBID,
(select Content from SMDCOMPONETS.dbo.LazerBase where IDLaser =  tt.PCBID) ,
tt.SNID, 
(select SN from Ct_FASSN_reg Rg where ID =  tt.SNID),
tt.StepID,tt.TestResultID, tt.StepDate 
from  (SELECT *, ROW_NUMBER() over(partition by snid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  SNID  = {_SnID}) tt
where  tt.num = 1"))
        Return newArr
    End Function
#End Region
#Region "Счетчик продукции"
    Private Sub ShiftCounter()
        ShiftCounterInfo(1) += 1
        ShiftCounterInfo(2) += 1
        Label_ShiftCounter.Text = ShiftCounterInfo(1)
        LB_LOTCounter.Text = ShiftCounterInfo(2)
        ShiftCounterUpdateCT(PCInfo(4), PCInfo(0), ShiftCounterInfo(0), ShiftCounterInfo(1), ShiftCounterInfo(2))
    End Sub
#End Region
End Class