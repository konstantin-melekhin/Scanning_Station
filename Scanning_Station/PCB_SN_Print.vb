Imports Library3
Imports System.Deployment.Application
Imports System.Drawing.Printing
Imports System.IO


Public Class PCB_SN_Print

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
    Private Sub PCB_SN_Print_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                If LOTID = 20201 Then
                    Print(GetLabelContent(SerialTextBox.Text, 0, 0))
                    WriteToDB1(SNFormat(3))
                    SerialTextBox.Clear()
                Else
                    Dim dataToPrint As New ArrayList(CheckstepResult(GetPreStep(SNFormat(3))))
                    If dataToPrint(7) = True Then
                        Print(GetLabelContent(dataToPrint(3), 0, 0))
                        WriteToDB(dataToPrint)
                        SerialTextBox.Clear()
                    End If
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
        SNFormat = GetScanSNFormat(LOTInfo(3), SerialTextBox.Text)
        If SNFormat(0) = False And LOTID = 20201 Then
            SNFormat = GetScanSNFormat(LOTInfo(8), SerialTextBox.Text)
            Dim S As ArrayList = New ArrayList(SNFormat)
            SNFormat = New ArrayList
            SNFormat.Add(S(0))
            SNFormat.Add(S(1))
            SNFormat.Add(S(2))
            SNFormat.Add(SelectString($"Use fas declare @SN as nvarchar (50) = '{SerialTextBox.Text}'
                 if  (select SN from Ct_FASSN_reg where SN = @SN)=@SN
                    select id FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = @SN;
                 else
                    insert into [FAS].[dbo].[Ct_FASSN_reg]  values (@SN,{LOTID},11,11,9,CURRENT_TIMESTAMP) 
                    select id FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = @SN;"))
            SNFormat.Add($"Формат номера {SerialTextBox.Text} соответствует FAS!")
        End If
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
            from  (SELECT *, ROW_NUMBER() over(partition by pcbid order by stepdate desc) num FROM [FAS].[dbo].[Ct_OperLog] where LOTID = {LOTID} and  pcbID  = {_snid}) tt
            where  tt.num = 1 "))
        Return newArr
    End Function
#End Region


#Region "3. Запись в базу"
    Private Sub WriteToDB(snBufer As ArrayList)
        RunCommand($"use fas         
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID])values
          ({snBufer(2)},{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)})")
        CurrentLogUpdate(ShiftCounter(), snBufer(3))
    End Sub
    Private Sub WriteToDB1(snid As Integer)
        RunCommand($"use fas         
          insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],SNID)values
          (0,{LOTID},{PCInfo(6)},2,CURRENT_TIMESTAMP,{UserInfo(0)},{PCInfo(2)},{snid})")
        CurrentLogUpdate(ShiftCounter(), SerialTextBox.Text)
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
    Private Sub CurrentLogUpdate(ShtCounter As Integer, SN1 As String)
        ' заполняем строку таблицы
        Me.DG_UpLog.Rows.Add(ShtCounter, SN1, Date.Now)
        DG_UpLog.Sort(DG_UpLog.Columns(2), System.ComponentModel.ListSortDirection.Descending)
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
                PrintLabel(Controllabel, $"Номер {prestep(3)} повторно отправлен на печать!", 12, 193, Color.Green)
                prestep.Add(True)
                Return prestep
            Else
                prestep.Add(False)
                PrintLabel(Controllabel, $"Печать номера {prestep(3)} отменена!", 12, 193, Color.DarkOrange)
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
#Region "GetLabelContent"
    Public Function GetLabelContent(sn As String, x As Integer, y As Integer) As String
        If LOTID = 20201 Then
            Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW650
^LL0201
^LS0
^FO64,0^GFA,05760,05760,00060,:Z64:
eJzt1UGu2yAQANCxvGDJtqtykopzfbXfJuqiyxwhN2nJCXqDhqoXwMriE31kOgPYxvmO8ttV1c9IkTIyz7I9wwBQo0aNGjVq1Hgz0QQQdvtSCBb6YBoHPXANIAyABEyBq/t2ROtn2zuyPOBdXmED2hE8WlzdBA/0L6jFKnnbqh5/2bZ4Gwgggwb5GquTDQotw8do6EkM9HetB6E76PVkbR8tN5jft9x0+HU7XCsBmJGqDTAy2072ST/etAwd/nANWm6EYgF8a/nKSl0YSrrF2snqyYqVDbZxjRNK7PGDYDLQ1WTbyTK0WFzXWHEqbEsdQFaipeSMhbi21BLiW1x3yPasHywLY7I9WkqesBDZNk62v0yXrJS07v3UG2dzwZcvLCXUyvk7F1ZCsu9W1oZkA1lMAnNkpb6yXbRQWqHlLllsAkrG1qW+ipYdk+3Br+1gBosg2l20mNDmif1MVnw52k+0Fl/syg6L/bxYn/bRZD0t9VsWd+VxtjGBaH22KtoW22LbhoCLTmtrkt0rR5Y5bOmVtcPP2e4PpfXcRcvROuwobrm+ss83bZuemX9XF7LS8BCGW/YgVrYZkz3BA9le/4EFsrjlTvAYLbywH4/zd/56ZftoWbYjWYv7aGWn+npR1hetivYHfLBU3mixUIvtN23qZ0Vzg41AYwnbdNPmfnai7Oc06/hkmY3bYWRmsXI37yN6AFXsI7JSY6mixfZGS3NyZfP+TbbcvzSf0dJRxLMNazvNDUOWRZvmhn1p6VwobRvcYil5zvPK0nmUrUi2PI+saOY5qclScklzsnV4DsYPvNjyHIxW6jxjyVKS5nPj6e2wXvH4TZaql+1m0PW/jWr/b1ujRo0a/178Bi7i5/k=:14E2
^BY3,3,57^FT77,135^BCN,,N,N
^FD>:{Mid(sn, 1, 3)}>5{Mid(sn, 4)}^FS
^FT141,173^A0N,38,38^FH\^FD{sn}^FS
^PQ1,0,1,Y^XZ
"
        Else
            Return $"
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW650
^LL0201
^LS0
^FO64,0^GFA,06144,06144,00064,:Z64:
eJzt1bFu2zAQBuAjOHDkG5QvUoiv1LFDKrHw0NGPVBkd+hoMMnQsgQzhwOr630l2ZLlFjQ5dqnOcOIY/WeJ/PBHttddee+211157/eOyzJxW/7fbj0RuFLhST2G0hShkcsVUGtK9nlNgvvihUmymGs53+xE+RXiXyeB0hslUy+Vun+Hz4i1PxGyqwyXd68vsY4Z3wMwfq2f5LC4o/MljuUIK5ezLYFo8Vf94t7c1JIdnzH4kl+Oh+lMN43SvN7Dy/FDgfQ6HIj71ax9H+V1bRxrschx5ufh29mNI7649czOyRpNtjROCtU9MMVlklM+ezt7B4yDl2luu8G76wSOCsd8Q9+iQUbnxJCcc0sY7bvAeaY4Ixn7nUbyXjBff+7FWvVb8igmHald+CvKjvYKzQdzZzxnP67/yOCATrfKD91n9Vz6W8DjZyRfx4aTel61HdEv/iW8E3+P9eGBXYhpsc7OXc9T+E38q79UPZJt0bll5M6o3k5U+w8qXWEK1qS39D+8ey0OQUCdYomX/LZ5I/ABfZ1/Fax/o/lt8BDENPm3OX44acQxsbOzzA+aDejN7mn1+K15OPqCPXveP+HDj8cCgEF9n/5Q78Q7fiz7AMq89b32Hh9H87KTewqO7kQaemVb9A29vfevRf5rffP72OXdCItYN+xgxrbyb+q1/eKi+qR+S+pfxjfhhnD3Fm/y3fs4fXyX9Dx/U0y98GG9832rU/LHa4s3iJzL51qdt/l1X8XL2efbJP2j87nd+3X8rj5GTZOAnL31bMQL9dv1Cmjb930V47R+dn/bsXQFC/vEqv/BFPfZfXvZfF0qNny4eqTOpZz60uO6fo3jNf73/1V/2P96A19bD8Jd7hojFV+Sn/jJ/8KfzGUw9ZufW06r/xTv+LB7zL2Ho2RdO4j33F58WLy+Hq/lxLNK/QS4U81c2tn1mEu+mTjzJYNN9v3jZVq/7/5hJE8AXldbL0LelUefGil2ofpD7P7pLb//wPi/3hpua7xvyub+r3f/ffq+99trrP6mfz1AYDg==:569B
^BY3,3,57^FT118,135^BCN,,N,N
^FD>;{Mid(sn, 1, 4)}>6{Mid(sn, 5)}^FS
^FT211,173^A0N,38,38^FH\^FD{sn}^FS
^PQ1,0,1,Y^XZ
"
        End If
    End Function
#End Region

#Region "Проверка регистрации платы на THT Start и на гравировщике"
    Private Function CheckPCB(PCBSN As String) As ArrayList
        Dim PCBRes As New ArrayList()
        'прерка таблицы лазер
        Dim PCBID As Integer
        If LOTInfo(2) = True Then
            PCBID = SelectInt($"use SMDCOMPONETS SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = '{PCBSN}'")
        ElseIf LOTInfo(2) = False And LOTInfo(7) = True Then
            PCBID = SelectInt($"use FAS SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{PCBSN}'")
            If PCBID = 0 Then
                PCBID = SelectInt($"use FAS insert into [FAS].[dbo].[Ct_FASSN_reg] 
                    ([SN],[LOTID],[UserID],[AppID],[LineID],[RegDate])values
                    ('{PCBSN}',{LOTID}, {1},26,{PCInfo(2)},CURRENT_TIMESTAMP)
                    SELECT [ID] FROM [FAS].[dbo].[Ct_FASSN_reg] where SN = '{PCBSN}'")
            End If
        End If
        If PCBID = 0 Then
            PrintLabel(Controllabel, "Плата " & PCBSN & " не зарегистрирована в базе!", 12, 193, Color.Red)
            SerialTextBox.Enabled = False
            BT_Pause.Focus()
            PCBRes.Add(False)
            PCBRes.Add("Плата не зарегистрирована в базе!")
            'CurrentLogUpdate(Label_ShiftCounter.Text, SerialTextBox.Text, "Error", "", "Плата не зарегистрирована в базе!")
        ElseIf LOTInfo(2) = True Then
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
        ElseIf LOTInfo(2) = False And LOTInfo(7) = True Then
            PCBRes.Add(True)
            PCBRes.Add(PCBID)
            PCBRes.Add(PCBSN)
        End If
        Return PCBRes
    End Function
#End Region

End Class