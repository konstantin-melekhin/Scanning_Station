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
        CB_CheckID.Checked = True
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
            If SNFormat(0) = True Then
                If CB_CheckID.Checked = True Then
                    If CheckSberID(SerialTextBox.Text) <> SerialTextBox.Text Then
                        PrintLabel(Controllabel, "Номер не прошит в устройство! Вернуть на перепрошивку", 12, 193, Color.Red)
                        SerialTextBox.Enabled = False
                        Exit Sub
                    End If
                End If
                Dim dataToPrint As New ArrayList(CheckstepResult(GetPreStep(SNFormat(3))))
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
        SNFormat = New ArrayList()
        SNFormat = GetScanSNFormat(LOTInfo(8), SerialTextBox.Text)
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
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  SNID  = {_snid}) tt
            where  tt.num = 1 "))
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

    Private Sub GB_WorkAria_Enter(sender As Object, e As EventArgs) Handles GB_WorkAria.Enter

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
        End If
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



End Class

'Этикетка для SP1/SP2
'^XA
'^RS,,,3,N,,,2
'^RR3
'^XZ
'^XA
'^SZ2^JMA
'^MCY^PMN
'^PW1182
'~JSN
'^JZY
'^LH0,0^LRN
'^XZ
'~DGR:SSGFX000.GRF,60912,141,:Z64:eJzt3d9v3EaeIPCqVMAysGUy+1ZGKs0sFthnGgayZUxH9F6AvX+Dgg7JPRhzNAzsdhBBzR4e1A+nlV59OGH8L8y9BbjDTfXwIL1o44d78UMwqUYD0ctgQiHAhcHwmvf9kmyprZYdJ/GIurn+JpD7d3+a9ZssFgmBqEjHwcrzm70OGW1Qu7hlX/6iawvV/ss7VbTBkuZf2aliEdEL/3Qc7SZJOkUs4u3671sdK9qg9V/RsWIRCf55p2PEIiL88363hvOoN8qDjhGLuIV/ko4Ri8DidEMKUwN5u2vEImjXgBciafPNjYgHN6eaqeuY97s2nMc7N6eaqSPpGnAet24S5u2bVLpvTPWL8dZN4tCb0xpg3CRMcnNaA3KTmiaof29O00RuUjsJzeSNwrzftWA53u8asBQ3KZXIOw+6FizHg64BS3HrQdeCpbhJFTC5lXQtWIobhblBXWCyxrw8kq4By5F0DViOpGvAciRdA9axjr/YYMFPfqsTvSlE7Db/svwnf4QTvyELidsjPdS8qU/8GaHVD7/m2kLfjMOmTSg9DL9/SkshvNN5aHaIZ718yOY89yeln0dKu9Qcz+8U4WjOS38yyRjLBz0yNFVakioLHYfbuB8mXulNqzQP4h1eeLbiOZ0L18vDZA65yiXDfce3xsxd4RvzrAzT9GkxX8EEVfh9lf5eKP90fndaJT5gqnTuFWE2V3Gk+j0CGL8IJwe8/HA6yTjXDWaMmMpx3Rgwxit7Z9XBINisxqqXV15BD0TPyz+bVA1mFzCTSeWqDy1gPssAszL7wQ0MG8E3iL4mUbCZJpK4dJLuKhlzR+kaMz1ylCLT1FHB5iTzhNreIJtmsiuIydLdntSDbWJdobUUKoD3wi2rJE1FTwRRlgKmR+ixK6MsS10dx8bYaJaOEraCeQQYc8REPwDM1ijRgBlljlSRw1UQqQ8+IJu7jhI1Jr47yXwhljAjwMh//xAxgZRK3IVH4JaVCjAbIjAzlhBng5BMIIbtB1EAGLuFmOQShm8mbJQA5iFispEJiMsQIyKHqWCoPvg7xEjA7DaYd5X4p1+eY+juB1I+/LLFSHEvpUcKMFrRXfFLxNCxcT4gNNuT0VFG96MomLwUM20xJ4AJU1pjkszRInKZb7cO731MHjuACaelo4ZnVXbPL1/AfBwW/+7LsMW4IaPHBWIEZOBfQo48pQfW+ZiEk8MgOjmGO1FQGVvNqlFSXcawyRImmL2IUYPNL+8dkXuIiacHsGVolW0p8vgLxEAGRsyRJp8cxueYlB6rBjMWX8jAcgrJdETOJk+C6Pk5xrJqNMpehrm91+QZo5eTSW5+efeI/AIxEeaZiE6yLdFiRi1Gkq3DaJFMd9PRkdskU/ov+xKKR4Mxk+eAgWSK6zxjWDoapZcxdIQZ+Jh5AkvT46UMHEMGRswu6TmQgQGzD1tmkmWCPNon08kk3cMMPN4FjEKMxgwM2W7XCXSdgQ8AYxnkDL5LJqYIohwysI6wNNkrMSyxWLTHnui/D0U7S9Q7Ls3GUKoH/9YFzGHgIKanHk1TpfTfTKB03oLWdQoMzDP4UhKpR4CRWroqmGa7rg6s0oCBG/FHY8iYDpmaPI5sNoZXRICJPwLM+DLGI1U6gkoOakYbhbYifu6SynXqSs8vhi0Gbk2rvgqh0kt5Ebu0+m5UZ+CKOx7U00PA+Ln0iiCuoKLOoeaklesOyyFWeoCpTFwVFio9EUJFbMMMMCsZ2CUhGz0l2BwAJtkh3Lpkx4U63p8Uvt1RgBGOcyff2Cy18urmoMHQfcRsQHMAmI3IFdxIlgcamwNjucXmYFj6SQmVnkO2TVQVxpSu6yWA8VMo2n+6jKnzzVUPLoVYnYVEk6YTs1KJvhjR0lSUyLxWm/xDGPlGMPbNYAZvAsPMm8HEL58s9gOY5Vf+wJesYx3rWMc6bmZQ65OhHbzwSAiDt5x4hlrtEFJ4ydKzuZ+EiRHQuLhEE+NhW2Gu+NwX3vS6wYvQQo9q+WOKoa2yOfELr5DuJQwvw3xorYLxMQxpWTE0hF3MEF8Kv/gJGE/qPKbPlr4PRoO5Zc9gfO5vwBCTQM9p6eVKx4+KSBIRP5occ/2pfQlG9X8CRpAgiOhyp1mROIaedKKUcl15CQNpo2P4j0gNQ1qhHkSEsSsxL98JMzQFbNe3+gJ6jUNmaSmlP2Jju8CM8PuqPejghogJGozmjiZk7o9KPme5X+T4csCoOCBS2hkTAjoejE9L3xZhvsNzLw8ZozmpiBJeErLcZKvdj2oG3edhOoQ+cD48KOhcyhDGCjnZajG/koRWeyoshsk5RgacQydnHqYVn3slYKiBLRPEKkCM2WJ7NcaZzv3ZPMyhjw6fnjJekCpR0CWvvDPoTCeXMS53lf6HbCyE0pHQ1JXy/Wz3lianXoNJIT0UPKn/wfQIjDYaTJ0C3/xijJj/jhjrIUbomOgWA8nkTl3JHEazXYFjGOYqMgFMYKe9v84yvoqBoZqmGbxby0gq4khJsn3o2R1xC6Vpko72ENPXmhrP4C6iMYxm4hpz+ng8Hru9nhrAlsGXAyZqMIc1RllHcsbYCDGQLsKtMdEjo2iWrZZxF/c11JgAMKLGzA6JpHsMi/akovuIeYgYlmPRhqFVEdcF5fTbAwajMlcNKpNQyBuxkAuMS6A0SetKBzB0V0jEqBqjoruJpEfZX61icF+Dn9GDssFsQyUyK1jB9sgg0DGdk6MW4xtSYGkqoTw0W2Y2Q4w/Bwzc24YMLJSdtxgK9Yy22w3muJB34WcoGMzBT6kxJ9m7V2LyYYvRNWY4g0qNSaLgEdgup4g50RpqMSi5lh3C3YBhBr7AnFncOygBY3baPAPZkwXnmPIck/h5i7l3FUZpyNn7F8kUzQ6VhC1TY3brLXN7T+upIQox+0S5AXeCBUaR/iDHsg0YJQwWbcRwaA4iyDNNMrm4P6JOJgNDc8Q8z/72ygwsJxd5hiNmX0rYJPgIHdO9FjNpMWPYMpq7UM9MszE/cHs1Jqkx0qsxs1RAXiXMWl5jRi0GHjQ1xkiaZ1tXFG0HMfBurSOoHGvMrtLkSKpAQw2cHgLGE4h5R9f1DMEaWKhbBGAHT6unohicTc0tKP3xAIorVBGT1IWPesBgyC05ZzStd0wwrApxy8RYmvIs+9UKhv1aDb7LngnRiyN3wH4v5Wezca8gMx+Kx3A6yqBJVLgr8zsD2SmvsjTxS7+EqgcxlV8dlMWgmlqvhHoxB4ybfzb5I/98CM1YMvu9ZF8wesqhan6UMq9PZoD5ECs9m0ElcRlDoQbeyXbqvRA8Z1ADb8zSg+fQ8uXYaieGtJgd49dFewQoXmgXMaO5X43LfFBNjId2Axhet9p5gyklLRmZ8zIOwpSxugYmvhnyM3MFpolXjlKdVzz3vmLJgwfkjR6AfSXGfdWT6s2PoV+JeeVxl+vG3KTjLutYxzrW8f9TLJ0t4fzoibtvXfmOn36awdLBZ/6jJ29c+Q5aXvFgHX3vyg8pWEmREWb4lzYHY86HzsrLOQm4rR9m5qIlZZemVLww3QMGlfXHcMDsEN+uni/e96/C+DiGxq8a1scRW8x5p6XvFz7J/QXmYrcCu9yGL9/1i6qmws+HHxrmq5vhaozqudB3hU+y/4h36eTF7yilhqG7avYFMbuEedXZ8RL79KQ+YgQfHcSr39wPOQz94EUDVr+y2a7QqWQObHJqnCsw9ITIxzwImgMgzG5fYF7ZEZPNqgKi6bIE765ihn5eHRTVdO6Vg0+rrPQL6LsGXLF9xCSi8KZfjUajMpzOlfKPcGtQQdRHThAjZpiw+KFnh1JX4wJG1/JO9fmduZdXozKacaFYdbuEJIHxeYPRd+d38ronq+9dhSmqp2U1q2oMTsGwzMQcBtvEiM3k9lw9+nYySvf12TOlPjwimcTfJhkJohoD48aTfv6dDGZjDaNrdad6cme/N5hlX25mNcY7RAliNPwS/ejZu2WNCR6vYr55Z2D7fZ+P99Wg6HnuJYzn6rt2Mho7OmaqHzsk21tgPpnO8WxrwAQwKotgSE/Y5zDIVQEMvqbHZDP1RJ+lrotZRMZ43AowlvUUceCdwdYVGAIYyK9sX31aKNdTukqTmGvAWJklLg/um+lol+uIqYcRJ5MaowHz8KyqMXdO4vuJtDUmh2GoCBwtp0dki70rHrJUOC2GWUh4bXCQ6zwDWraC+fAZya2uMdsLjCENxtyfLGFsjSnMEWJiwAwC+BLE7AHmPZNBz50VbiDdwA3UbJd8ssCELaaAdL2fIIZzHBxfgeEWMR47KD77XPmV0t9Z0ibT3VHisvB7wIyr0FTqJHKUOUWMBUyMxQkw3l50P/GTCiozJp1YutF2oCqX7LB74mQJg2lLwqSCISpnUEKbWmc5wgNWNJj9jaEGtNJTC7WrZPsNBraRmY12Ieel7+1Fjqi3DDXLGBHVW+YL2DLKiaQTeYGCZPqEbd3eW0omGJgS8otRhsnECEuvwHiQp5o809tBTL/B1EUbailIJsBQwNxrMBPEsGQJw0WTZ04A47JI8ohpaTHPXMZAu3F/kqp6y/ArDqGGXl2aXChNiPHe2tbTWQKVHufUyKl1HNgyGR0jxhcxYDADwyeRIGjzDBFYmmKDGM6sZKbBbKaZJxalKYjJO0pD8cbdJVhTu1cM1jY8KMt+6fLKm28DZj7Q1cz0lOfxcSKi/NhVfz09Ztmv9ONxT+h/VNmhwu3Nie6fjTmJoFwJHU9lMPmiHzCWGskmXGl7/B83s9QFjAfGNNGx7ekBTfSjsas0FF11RdvRYAqXYaWnFUdMZry6oSRvB8Os9M6qg2o2D8/+1BPhcZFJRaGWHJ7O/WnVYqDJhBr44A+a0YmRdII1cAY1cIMpEVPht8DP1HcrXmg6V2Gxiulzz1qcjFF6OWBoGeuKJTynBcFuhz8q+PSZV9nSN98K4R1bQxrMbM6TEjBQRIRnQqnm7ARgk0RSeNwMSRFtMSEoEzmhDDHQ5Ziad+QZyzUpxFWY1whqxAsrsizNb7iU7LKePXBpAgG+nCa2GbGbn/D9r8Qs7ZW4tOsmqDsHl4SIYS3mhyY6/HiMvrh5qccQ1z2nS50mxHDyoD6J6fWnTrw0zI94rbsivCmruaxjHetYx192cLv6WHv2yN3kh9/eDGpfGbTg1ks2YLAOH+u8cve832CGyybe3Akv3ue9bJ8Ity95YuklpY9He2iFR2ydV3ZCNsgqpv25Sy3Wz8L01EDbWZrga51XDu3jF/55Wbz0I14Dg3Mu4niSERcxr2yEo5+H8XKvfK/EiROkyk9taOwZjMZYAb3g76vTOS9uYRdKRo9gxCgQo3ju53YTD6Rx7+wpLXnhTdJx7lnimd+ReudUDCNTU3i5D0N/ISURhW+M3WGs3k2Ek9Qnhc9WE8sFjD/3yhAxVT6007NqDPfhP8BAL72HGB3At9eLNjqaF4CZtpjfpuD1pynO/IHs+5VpMEKEWX2GiSOU1OR26Vtjq3SBoaNxMcxW18Z0Y8Bs+P0a8z38Nz07Hj/1NuBRwBx4/R5u1kDDtxc1Jn5bqWAKd8djLujkd64jNd19W+IsiPh7u1ljXBF/5MAI5iNX9OWAuOKdeDKd1BiDmGz/VpStHqLsDdx+r6fUNiE5hM1nOR+7PddVG6THx6SHGGb9SZLNEcMtUbLGHCDGGMeRAd0nKoiAbOMG46iYcyEjpmqMgJ+TTUfZAkMyQcwsXcEoDRj4/YMaM7Dxac65qzxX9QFzUK/tqVgeThPjXWBmkGr/gpikwRzWmPgcw2XkMBFETIqH8jmBEW6QbSYXmBlgtnavxCgvnNcY+6mNv853eLnAzDnuedWQAI+M4U0ysVIGX29W2X8eV4iZuzKmBSsBE0nTYpiMYIhqI5aLkztPAOPlR9MEc8k5ppqtZuCwvIT5Yz4czxcYGPPiZxMV3TWGtRn4sMH81xqTfS0Q4x1CBkbMtMEEiCkiVrSYXnyyjPkYMasZOCx7mEzkHFPmbOwtMONfYzLZZYyC8hH8AZLpXp1MUF4wmYSCoo3JNGkwv4kcgnlG3t57F4q2EwSIOU+mf4JkYqt5JpwrzMDkPAMXOTv2em6vycC/7gl8v8LzgJpkchFTAGZWZ+DjGrMvlIpwRlbUYOhvIk6EggzsiR5g3CAuljPwB7i7aeXsEDLoDSso2iqMAYNFu4CifeD3wsr/HDLw040eDAcl1MDTX0EGltA2uUoH+SaeGIJF+xhPC6FjEGLRjqeT0SgBjI1hCC5j5tYYF0p5jkX7LVKfJkT6Lok+SlemEsSIaSq9vK70cqj0oK4DTFHnmR7hcBMayhRepQFzUMYXmCqdh3Oo9OCpiIT5MJukIwOZzEKl5w7CjOOGJPjxFis9DhiNkys4VHrPVtrc2BtW72GVjphv2uYAatWw4oCZe1/UmNw3IYNauQ9diPEJJEfTHAj6W/q/w7k3TeHBCJoDPx21GB/3i/gZq5uD24UfWDtkFxiGp8Jc0QEYeOcjeXvxqL6yq3B51Q8cyAeXt7Yhl0fE543rYqwtX7i3jOGvj1k9MaNDzODS/asw2BW8NB/wHLPoPOQv3HshrsJcHe9fgbkc+HtfhlkcEMxffHgd61jHOtbxJgJnP3Q1mS66dJ9DTc9+yjkX53HVdQ9oQdgLj/PL41GcDL2CwePX3k9ffQMxyepj/DLG55eWThD8CgzOi/h511G4ageQe/nh8PLeEOFdgfn5WYamliXnHZjmh9VzOXpEtvt6euRxfYEEl+h69ojfYBgOwOEV9YwPeKkFTCwJH+GElBbf7CDBfgq3vO032Csd8SMcWY1L336Vkc10TwS6qneHKcQMd+d+bp7NI1uRjwCT63BUQv87xZ0O4r1dx6tY8fd5rocpF8q3vPjEsvJeHh4X4fE8HFWKmMm8x8nQVqysnhV+9luhI3hTEExWT1YJNsfwrbtKx2eAgSFHoLOEwLgD/mfJ1vhA1WtKTDPoNDkkHzzK/qeG/rcKAHN/11GnXPXjfFBPwtCRq/5Dzt2/L+IjddfYR5MxYLIDn5N4M0uF66pgNtvXNuMKMSudyRrjOCII7Ixs7op+oGmNuQMZOKFjTnB9gM1pWmOKQTSDD053XRhai3u7jtzad7UuBgTX3AgiIQbacfoyMjBkhJ+AmGPmeyTaHOGMexVtjXa1yVw3CLJsZWqRno4xb0LqmK18c+8C824fMcesxmxtIcYlz5+bTxCz7yDm8a6jN/ecQD5/jpiHgHFj7XKtIqOItWZz7BtzxN77T4jJACMBk95PMhzvHmV/dRkjJ7ggwFjH0myVm1+Kh2HRYJRAzBFgKsBsIkaRk+f2FDZmuu/eN0TMxvMGc/KcNhiFUyG4FhGMjqy107GfV4D5LyTaShCjo8ej7L8lsGXC/ChbmVqkEEOPC8S4m4U4wROZiG4xwyPm5YD5eBPzjCb/+qT6mujN9GAeAiZDzCFg/vXJEDAnusYIpkVsIIfbCpJJA+bwfyHmSAivGH47mlRJ5s3D/CRbmVokasyRixhn8xBPSoGxe9BipkdMBtbYh1EFmJgcP4HhP5SmfZxOcDsbt8l0/ATy5O093y62TGhUDMlEx1JOjpj6tsVAnmH1lhHw/uermNtpjXFijZj923vB/SXM5BgwBjFbBPeIHEvAqBoTwFsXeQYeRoyIlBvUeUYaEQGGAQYysNoCzAgxAjDj+5MUzyp7nq1MLfJSGP8CBkuTszn2xALjLmMGUVZPCPydjBiRUJoQ4zHA1KUpk9OMe0JCaQqgNEl4D2IixqVuMZujXcTEDErTJHXh/Xm2MpuHH3CXnu66WM/0cJZUoE/HeFag14eqdnbc1DNQTUBDWREj4+wI8sz47eCRQYw69aCeMfJb3N+gI7zD3Z6GeuaRtfEzDu/ODhQDzGzfhWTSW9m+nqVjrEqylb1F3AVM1dTAJRQTwPyfcT1zoQDM/BhrYMBA0R6eIiaczOPNalaGU+MyrIE51MBG7mQVvDXihQ81sJdHx+XQ2vALDu+ezBETVX4FGTicVUJ/UmXllRj2NrQWfZ7z5HgxdTPGyQJNM6Ody2NuD/dxEZuHial/S0nsXYP7IOuRLrXQkufQRtEcmhNPEDzTsmmb+kCCtglPCuLEXN3JuBi1m/NblJwv4fCS81Ts+a2oaahfY5qDrJsibGWdl80huArDu8JcTPm4eMGS4FVn8LyIec145bGzi46QuerpN465QQv4r2Md61jHzYymYsa69aK9wCUxsX2Jmnvsj4sn3sDsux+NES3GNBi+hPFfevrJm8VcxAsY2TRbdVvt3BAMu4GYn7Qkx2p4dk6L8AxXIpmWw1mpFO53mt/JPfN7/zchZwpewXPyS63lP5uQsQ06Kv3RYbXrw8MV9GppiRgYOs7mpN8Pp9ArHhbhpBpVo7nr9Vxevvaiqf60YvPw7CuOkuHsfyjl5zH9ow89axj1DY+5gld4MdkBzNAOU1bRFHrjJWI8PMtlmH0pEFPggWrAnFVeOSyqaTWp0gowvXpux+uFZil3GU1d4TAoro7q6zimuFxIbGHAkjkKXiGgVw0YnETD0hGMZDaNdBxXkTRNbEYQowbV7ID0t++eVf5GWJxNZ5NqfACYDb//2piA0V08C18Ih9eY7SCOKK6xEdgIBj27KmBsDyqXGmMyNhrtwiioxkhC08Q0GKJxknu/JFOrenxkz6aTbMz3AdN7xbIqlyJmdB8wY+E6DmCEegijaGolYEwUmOkeYg5qTBA/AgxNHRI9WsLMoPfeP8dQwLiImY1wbYSqt9N7/XV2AbPHGJ178xpTqBMNhaaQYpgDpqox1di2mKrBDM9azDBNqmmOGK+AsSD58EtvYvs1pqLZQQWYP/54zIFX1pjee3u4J3djgZmcIIbmlzDTFjMDjNmWRPfDwuUVCQ+96QIzAYwHmJfOPVuNoEmmdI/UGP7eHhZtB/IMwWQizwGDA0bARJBMuPoFJBNtMHQCyUQEYvra5WPAkLM2maazbExcz/+C/xgMjJgZHe2ROgNzX2Dd1mLiS5jY4OoX5xhFU8S4ARksMAowvd4CQwGzzw9fO5303zZFe/dthyWA6QkJpcnpKU1MHEcPCqXp6FYQRRIxlrF0l5Pgb4wCTI9mo8T+G3BX1QYk01dkQ4VnFmrD5E/T2XE6/gYxnnrtoo2VXsloxUuHzeH/nsB6Zo6Vngl/MzRxXenlceQXgBmms8phuAvCP3JcPA0PKr0SMfUZJojBtTM4ZOvqOMVKz3d7r4/xbEkLKE2scAh4Skgf2DIlNAfW+EGYxNAc7Hg2jrwnsKXC0WxD0NKfGG/XcXn+aVKEaVFjPnfZd6SvwonlBU/CUcVZ0xz8iHrmR4TBP+yK4/QRueLU1T9z3ETM6jGTDjDtya1XYMw1S9axjnWsYx3rWMc61rGOdaxjHetYxzrWsY51rGMd61jHOv7yQh2F+VDyPXbk0EQe3ZvZ43zobGz7/Pggve6TstRpi/kaMV88ntlnLebZGrPGrDFrzBqzxqwxa8was8asMWvMGrPGrDFrzP/7mPDi5rA6Px+7I0xz1R8MXlXni8VcF8av8rAqWdUuS/OtuXjCO7/+xfVhihrTbAU6tYsnYBudL85z3Zhmy1BzjinqRYauF6PwTI3zqy6yZLFIXn3t6O32NPFrxpAFplnlwG3Wbe61C/x1g+HtRRk8SD4M63WI8VoMN2GNyZsLV3WDcdtzUZltMf0OMb3F8lY5Q8ucFB1iFHGbJUa36/Ll0rxDjCbtyqgbDaZdH6srTLsYqV9jRHsp724wMWGH9Q1eX+dOtFeB6waTE/akvsFmiFmcGtwNxi7aI/ptElZWFR1iqCH32+//DDHv5d1ihi0mTPzK+KZLTIIX7asDlwZq/nSFIc3yi/W/hrQL7HWIWVzmoF6IaaNrTBt1Q7A44bYTzNIJjVjEaaeYktvFFa/w+gqsSwyfe8WgvSot/QruP+kQ483dT4v2zE56nBB/sZk6weyQ+88XmMySjS4xZIfcke2ohU5yMugU8x3R5xhT0LxTzClg2ssBU9NntksMPSaxrIeSEMbnSaeYjORysZic9Z6SLjEspVYuGifL5p1ieMKSCww0BteN8SuLu0QaDOGFXAgstE7Xj8nPMR7xygsMtEyLRX262DJujWnXXLD8q/NLOHWRZxpGO9q2npd0j2nXbbYbzHSJaffNtJhtmneNcc4xOSm6xOB3u4t1MLcs2e4aoxaYfzbE7xqjF5ghuf6d0pfzTLzIMzsXA7pOMGFVJeeHL7bbsVNXGL+qJtXikAZCOtpdX9/2akwzWGKI2e4Qg7FImTq/LC5H0BVmcQmjuiR9k3SLWSzw18Nyfmq6xSxWkKpr38WhsK4wiwtFFNixsl3uYCTnqyxTW2O2u8W0wY3GvVcbNwLj4QDOt53urj+PHjZUw7zezdgRJjSKNhXdNmKKol3vvRuM7dOmnazHTLZPOjzeFMQf0PqipDD61/BHta1TN5jg72i9Sj/kFcBkbrsnuCPMJw0GShFgjveJ1yEm3moxdU/r2X6XB9nv24w+xRuixsCWYd1hQpONFn0Y6Pb9brHHvBOMJtUoaZ/gdWnqEBPQZQzWM9eL8ascMU03PKCjyRJm+7o75H5V1JiqwSTLmGEHM4uWtgwjyxjfXjPmtWKNWWPWmDeG+Xr5RGDEdHkisF2+58bL99gacx5rzEXcIAxNALNNJLfepILhN2B2iKA5tzQJrxvDAANjO+3l/rSyVQKYKlG88AqWDE0HGDYmA6V1RKYZYOgkUY5Smif2ujEcMSwZSBkEZDIygBmZviu08hJjrxlzGzGEPJcqDmiWQJ5hiXkonEB9ODLRNWPeA4xD6BMtIs3SpjSZPwiHqPsJuW7MnWUMqzHUnCjA0IRcdzLdRcwOk4CRnIU1ZrQHGAmY687Amwlk4m8cGQDGYd/BPcJSARhNk3YaxzVjUrfGuKwePnCGmJgmr7x83Z8j8Os526sxgo0MPOQ6iIEa+NqXu8cRL6dPWkx9oUYlAMMMTdwfePMbx+CpRQ59WmFpglE5YrRX7Tk8ocmbuZbQj8RwwEgRBbLZMjliPGhCBz/07jeuSaD4QN8Xa+AWYwle5RWeiX/gvX8eTEqkkkGsKWZnKNDCFRKesV1g6DGRrtZWU5xcziojuApLOqquW1NfPWIOnavcNxqPd9cYVnSCqUNj4uC+z/Py0wljHetYxzrWsY51vIFIugYsR9I1YDmSrgHLkXQNWI6ka8ByJF0DliPpGrAcSdeA5Ui6BizFra4By3HrQdeCpVhjXhr/F3TG82Y=:747A
'^XA
'^FT872,257
'^CI28
'^A@N,21,21,TT0003M_^FD{Format(Date.Now, "MM.yyyy")}^FS
'^FO66,339
'^BY2^BCN,72,N,N^FD>:{Mid(sn, 1, 10)}>5{Mid(sn, 11)}^FS
'^FT111,438
'^CI0
'^AAN,27,15^FD{Mid(sn, 1)}^FS
'^FO0,35
'^XGR:SSGFX000.GRF,1,1^FS
'^PQ1,0,1,Y
'^XZ
'^XA
'^IDR:SSGFX000.GRF^XZ