﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WF_for_FAS_SN
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle49 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle50 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle51 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle52 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle53 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle54 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle55 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle56 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BT_LOGInClose = New System.Windows.Forms.Button()
        Me.DG_UpLog = New System.Windows.Forms.DataGridView()
        Me.Num = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SCIDTab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CASIDTab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HDCP = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CERT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DG_ErrorCodes = New System.Windows.Forms.DataGridView()
        Me.BT_SeveErCode = New System.Windows.Forms.Button()
        Me.TB_Description = New System.Windows.Forms.TextBox()
        Me.CB_ErrorCode = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BT_Pass = New System.Windows.Forms.Button()
        Me.BT_Fail = New System.Windows.Forms.Button()
        Me.BT_CloseErrMode = New System.Windows.Forms.Button()
        Me.SerialTextBox = New System.Windows.Forms.TextBox()
        Me.GB_UserData = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TB_RFIDIn = New System.Windows.Forms.TextBox()
        Me.GB_ErrorCode = New System.Windows.Forms.GroupBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.CB_CheckID = New System.Windows.Forms.CheckBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Num_X = New System.Windows.Forms.NumericUpDown()
        Me.BT_Save_Coordinats = New System.Windows.Forms.Button()
        Me.Num_Y = New System.Windows.Forms.NumericUpDown()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.CB_DefaultPrinter = New System.Windows.Forms.ComboBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DG_PCBInfoFromDB = New System.Windows.Forms.DataGridView()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.DG_StepList = New System.Windows.Forms.DataGridView()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.FAS_ErrorCodeTableAdapter = New Scanning_Station.FASDataSetTableAdapters.FAS_ErrorCodeTableAdapter()
        Me.FASErrorCodeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.FASDataSet = New Scanning_Station.FASDataSet()
        Me.CurrentTimeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.BT_Pause = New System.Windows.Forms.Button()
        Me.CurrrentTimeLabel = New System.Windows.Forms.Label()
        Me.LB_SW_Wers = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.LB_Procent = New System.Windows.Forms.Label()
        Me.LB_Yield = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label_ShiftCounter = New System.Windows.Forms.Label()
        Me.CB_Quality = New System.Windows.Forms.CheckBox()
        Me.CB_GoldSample = New System.Windows.Forms.CheckBox()
        Me.BT_OpenSettings = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.CB_User_Input = New System.Windows.Forms.CheckBox()
        Me.LB_CurrentStep = New System.Windows.Forms.Label()
        Me.LabelAppName = New System.Windows.Forms.Label()
        Me.L_UserName = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label_StationName = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.L_Model = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.L_LOT = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Lebel_StationLine = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LB_FailLotRes = New System.Windows.Forms.Label()
        Me.LB_PassLotRes = New System.Windows.Forms.Label()
        Me.LB_LOTCounter = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BT_PCBInfo = New System.Windows.Forms.Button()
        Me.GB_WorkAria = New System.Windows.Forms.GroupBox()
        Me.LB_CurrentErrCode = New System.Windows.Forms.Label()
        Me.Controllabel = New System.Windows.Forms.Label()
        Me.GB_ScanMode = New System.Windows.Forms.GroupBox()
        Me.BT_CleareSN = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.GB_PCBInfoMode = New System.Windows.Forms.GroupBox()
        Me.DG_PCB_Steps = New System.Windows.Forms.DataGridView()
        Me.TB_GetPCPInfo = New System.Windows.Forms.TextBox()
        CType(Me.DG_UpLog, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_ErrorCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_UserData.SuspendLayout()
        Me.GB_ErrorCode.SuspendLayout()
        CType(Me.Num_X, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Num_Y, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DG_PCBInfoFromDB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_StepList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FASErrorCodeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FASDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GB_WorkAria.SuspendLayout()
        Me.GB_ScanMode.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GB_PCBInfoMode.SuspendLayout()
        CType(Me.DG_PCB_Steps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BT_LOGInClose
        '
        Me.BT_LOGInClose.BackColor = System.Drawing.Color.Transparent
        Me.BT_LOGInClose.FlatAppearance.BorderSize = 0
        Me.BT_LOGInClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_LOGInClose.ForeColor = System.Drawing.Color.Transparent
        Me.BT_LOGInClose.Image = Global.Scanning_Station.My.Resources.Resources.close
        Me.BT_LOGInClose.Location = New System.Drawing.Point(362, 74)
        Me.BT_LOGInClose.Name = "BT_LOGInClose"
        Me.BT_LOGInClose.Size = New System.Drawing.Size(53, 59)
        Me.BT_LOGInClose.TabIndex = 2
        Me.BT_LOGInClose.UseVisualStyleBackColor = False
        '
        'DG_UpLog
        '
        Me.DG_UpLog.AllowUserToAddRows = False
        Me.DG_UpLog.AllowUserToDeleteRows = False
        DataGridViewCellStyle49.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_UpLog.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle49
        Me.DG_UpLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_UpLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle50.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle50.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle50.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle50.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle50.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle50.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_UpLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle50
        Me.DG_UpLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_UpLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Num, Me.SNumber, Me.SCIDTab, Me.CASIDTab, Me.HDCP, Me.CERT})
        Me.DG_UpLog.Location = New System.Drawing.Point(19, 109)
        Me.DG_UpLog.Name = "DG_UpLog"
        Me.DG_UpLog.ReadOnly = True
        DataGridViewCellStyle51.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_UpLog.RowsDefaultCellStyle = DataGridViewCellStyle51
        Me.DG_UpLog.Size = New System.Drawing.Size(1268, 283)
        Me.DG_UpLog.TabIndex = 25
        '
        'Num
        '
        Me.Num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Num.HeaderText = "№"
        Me.Num.Name = "Num"
        Me.Num.ReadOnly = True
        Me.Num.Width = 50
        '
        'SNumber
        '
        Me.SNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.SNumber.HeaderText = "Серийный номер"
        Me.SNumber.Name = "SNumber"
        Me.SNumber.ReadOnly = True
        Me.SNumber.Width = 159
        '
        'SCIDTab
        '
        Me.SCIDTab.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.SCIDTab.HeaderText = "Результат"
        Me.SCIDTab.Name = "SCIDTab"
        Me.SCIDTab.ReadOnly = True
        Me.SCIDTab.Width = 123
        '
        'CASIDTab
        '
        Me.CASIDTab.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CASIDTab.HeaderText = "Дата"
        Me.CASIDTab.Name = "CASIDTab"
        Me.CASIDTab.ReadOnly = True
        Me.CASIDTab.Width = 77
        '
        'HDCP
        '
        Me.HDCP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.HDCP.HeaderText = "Код ошибки"
        Me.HDCP.Name = "HDCP"
        Me.HDCP.ReadOnly = True
        Me.HDCP.Width = 123
        '
        'CERT
        '
        Me.CERT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CERT.HeaderText = "Примечание"
        Me.CERT.Name = "CERT"
        Me.CERT.ReadOnly = True
        Me.CERT.Width = 139
        '
        'DG_ErrorCodes
        '
        Me.DG_ErrorCodes.AllowUserToAddRows = False
        Me.DG_ErrorCodes.AllowUserToDeleteRows = False
        Me.DG_ErrorCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_ErrorCodes.Location = New System.Drawing.Point(488, 37)
        Me.DG_ErrorCodes.Name = "DG_ErrorCodes"
        Me.DG_ErrorCodes.ReadOnly = True
        Me.DG_ErrorCodes.Size = New System.Drawing.Size(101, 80)
        Me.DG_ErrorCodes.TabIndex = 34
        Me.DG_ErrorCodes.Visible = False
        '
        'BT_SeveErCode
        '
        Me.BT_SeveErCode.FlatAppearance.BorderSize = 0
        Me.BT_SeveErCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_SeveErCode.Image = Global.Scanning_Station.My.Resources.Resources._3floppy_mount
        Me.BT_SeveErCode.Location = New System.Drawing.Point(517, 250)
        Me.BT_SeveErCode.Name = "BT_SeveErCode"
        Me.BT_SeveErCode.Size = New System.Drawing.Size(72, 79)
        Me.BT_SeveErCode.TabIndex = 3
        Me.BT_SeveErCode.UseVisualStyleBackColor = True
        '
        'TB_Description
        '
        Me.TB_Description.Location = New System.Drawing.Point(10, 177)
        Me.TB_Description.Multiline = True
        Me.TB_Description.Name = "TB_Description"
        Me.TB_Description.Size = New System.Drawing.Size(495, 140)
        Me.TB_Description.TabIndex = 2
        '
        'CB_ErrorCode
        '
        Me.CB_ErrorCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.CB_ErrorCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CB_ErrorCode.FormattingEnabled = True
        Me.CB_ErrorCode.Location = New System.Drawing.Point(10, 97)
        Me.CB_ErrorCode.Name = "CB_ErrorCode"
        Me.CB_ErrorCode.Size = New System.Drawing.Size(266, 39)
        Me.CB_ErrorCode.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(10, 143)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(314, 31)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Введите комментарий"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(283, 31)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Укажите код отказа"
        '
        'BT_Pass
        '
        Me.BT_Pass.FlatAppearance.BorderSize = 0
        Me.BT_Pass.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Pass.Image = Global.Scanning_Station.My.Resources.Resources.agt_action_success__1_
        Me.BT_Pass.Location = New System.Drawing.Point(17, 17)
        Me.BT_Pass.Name = "BT_Pass"
        Me.BT_Pass.Size = New System.Drawing.Size(97, 91)
        Me.BT_Pass.TabIndex = 26
        Me.BT_Pass.UseVisualStyleBackColor = True
        Me.BT_Pass.Visible = False
        '
        'BT_Fail
        '
        Me.BT_Fail.FlatAppearance.BorderSize = 0
        Me.BT_Fail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Fail.Image = Global.Scanning_Station.My.Resources.Resources.agt_action_fail__1_
        Me.BT_Fail.Location = New System.Drawing.Point(833, 17)
        Me.BT_Fail.Name = "BT_Fail"
        Me.BT_Fail.Size = New System.Drawing.Size(87, 91)
        Me.BT_Fail.TabIndex = 27
        Me.BT_Fail.UseVisualStyleBackColor = True
        Me.BT_Fail.Visible = False
        '
        'BT_CloseErrMode
        '
        Me.BT_CloseErrMode.FlatAppearance.BorderSize = 0
        Me.BT_CloseErrMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_CloseErrMode.Image = Global.Scanning_Station.My.Resources.Resources.icons8_стрелка_влево_в_круге_2_64
        Me.BT_CloseErrMode.Location = New System.Drawing.Point(536, 19)
        Me.BT_CloseErrMode.Name = "BT_CloseErrMode"
        Me.BT_CloseErrMode.Size = New System.Drawing.Size(53, 55)
        Me.BT_CloseErrMode.TabIndex = 35
        Me.BT_CloseErrMode.UseVisualStyleBackColor = True
        '
        'SerialTextBox
        '
        Me.SerialTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.SerialTextBox.Location = New System.Drawing.Point(189, 47)
        Me.SerialTextBox.Name = "SerialTextBox"
        Me.SerialTextBox.Size = New System.Drawing.Size(508, 31)
        Me.SerialTextBox.TabIndex = 1
        '
        'GB_UserData
        '
        Me.GB_UserData.BackColor = System.Drawing.Color.NavajoWhite
        Me.GB_UserData.Controls.Add(Me.BT_LOGInClose)
        Me.GB_UserData.Controls.Add(Me.Label13)
        Me.GB_UserData.Controls.Add(Me.TB_RFIDIn)
        Me.GB_UserData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GB_UserData.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_UserData.Location = New System.Drawing.Point(1507, 15)
        Me.GB_UserData.Name = "GB_UserData"
        Me.GB_UserData.Size = New System.Drawing.Size(449, 197)
        Me.GB_UserData.TabIndex = 80
        Me.GB_UserData.TabStop = False
        Me.GB_UserData.Text = "Регистрация пользователя"
        Me.GB_UserData.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(7, 45)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(321, 25)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Отсканируйте свой бэйджик"
        '
        'TB_RFIDIn
        '
        Me.TB_RFIDIn.Location = New System.Drawing.Point(11, 88)
        Me.TB_RFIDIn.Name = "TB_RFIDIn"
        Me.TB_RFIDIn.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TB_RFIDIn.Size = New System.Drawing.Size(345, 31)
        Me.TB_RFIDIn.TabIndex = 0
        '
        'GB_ErrorCode
        '
        Me.GB_ErrorCode.Controls.Add(Me.BT_CloseErrMode)
        Me.GB_ErrorCode.Controls.Add(Me.DG_ErrorCodes)
        Me.GB_ErrorCode.Controls.Add(Me.BT_SeveErCode)
        Me.GB_ErrorCode.Controls.Add(Me.TB_Description)
        Me.GB_ErrorCode.Controls.Add(Me.CB_ErrorCode)
        Me.GB_ErrorCode.Controls.Add(Me.Label8)
        Me.GB_ErrorCode.Controls.Add(Me.Label4)
        Me.GB_ErrorCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_ErrorCode.Location = New System.Drawing.Point(1463, 428)
        Me.GB_ErrorCode.Name = "GB_ErrorCode"
        Me.GB_ErrorCode.Size = New System.Drawing.Size(595, 335)
        Me.GB_ErrorCode.TabIndex = 81
        Me.GB_ErrorCode.TabStop = False
        Me.GB_ErrorCode.Text = "Регистрация кода ошибки"
        Me.GB_ErrorCode.Visible = False
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(7, 19)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(186, 13)
        Me.Label22.TabIndex = 48
        Me.Label22.Text = "Выберите принтер для печати"
        '
        'CB_CheckID
        '
        Me.CB_CheckID.AutoSize = True
        Me.CB_CheckID.Location = New System.Drawing.Point(8, 152)
        Me.CB_CheckID.Name = "CB_CheckID"
        Me.CB_CheckID.Size = New System.Drawing.Size(75, 17)
        Me.CB_CheckID.TabIndex = 66
        Me.CB_CheckID.Text = "CheckID"
        Me.CB_CheckID.UseVisualStyleBackColor = True
        Me.CB_CheckID.Visible = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(7, 86)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(76, 13)
        Me.Label20.TabIndex = 64
        Me.Label20.Text = "Корекция X"
        Me.Label20.Visible = False
        '
        'Num_X
        '
        Me.Num_X.Location = New System.Drawing.Point(119, 80)
        Me.Num_X.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Num_X.Name = "Num_X"
        Me.Num_X.Size = New System.Drawing.Size(69, 20)
        Me.Num_X.TabIndex = 62
        Me.Num_X.Visible = False
        '
        'BT_Save_Coordinats
        '
        Me.BT_Save_Coordinats.FlatAppearance.BorderSize = 0
        Me.BT_Save_Coordinats.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Save_Coordinats.Image = Global.Scanning_Station.My.Resources.Resources._3floppy_mount
        Me.BT_Save_Coordinats.Location = New System.Drawing.Point(194, 61)
        Me.BT_Save_Coordinats.Name = "BT_Save_Coordinats"
        Me.BT_Save_Coordinats.Size = New System.Drawing.Size(72, 80)
        Me.BT_Save_Coordinats.TabIndex = 65
        Me.BT_Save_Coordinats.UseVisualStyleBackColor = True
        Me.BT_Save_Coordinats.Visible = False
        '
        'Num_Y
        '
        Me.Num_Y.Location = New System.Drawing.Point(119, 106)
        Me.Num_Y.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Num_Y.Name = "Num_Y"
        Me.Num_Y.Size = New System.Drawing.Size(69, 20)
        Me.Num_Y.TabIndex = 63
        Me.Num_Y.Visible = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(7, 111)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(76, 13)
        Me.Label21.TabIndex = 64
        Me.Label21.Text = "Корекция Y"
        Me.Label21.Visible = False
        '
        'CB_DefaultPrinter
        '
        Me.CB_DefaultPrinter.FormattingEnabled = True
        Me.CB_DefaultPrinter.Location = New System.Drawing.Point(6, 35)
        Me.CB_DefaultPrinter.Name = "CB_DefaultPrinter"
        Me.CB_DefaultPrinter.Size = New System.Drawing.Size(293, 21)
        Me.CB_DefaultPrinter.TabIndex = 47
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CB_CheckID)
        Me.GroupBox3.Controls.Add(Me.Label20)
        Me.GroupBox3.Controls.Add(Me.Num_X)
        Me.GroupBox3.Controls.Add(Me.BT_Save_Coordinats)
        Me.GroupBox3.Controls.Add(Me.Num_Y)
        Me.GroupBox3.Controls.Add(Me.Label21)
        Me.GroupBox3.Controls.Add(Me.Label22)
        Me.GroupBox3.Controls.Add(Me.CB_DefaultPrinter)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(1529, 764)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(310, 178)
        Me.GroupBox3.TabIndex = 83
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Настройки координат принтера"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(399, 23)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(106, 13)
        Me.Label18.TabIndex = 33
        Me.Label18.Text = "DG_PCBInfoFromDB"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(230, 107)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(70, 13)
        Me.Label16.TabIndex = 33
        Me.Label16.Text = "DG_StepList "
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label18)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.DG_PCBInfoFromDB)
        Me.GroupBox2.Controls.Add(Me.TextBox2)
        Me.GroupBox2.Controls.Add(Me.DG_StepList)
        Me.GroupBox2.Controls.Add(Me.TextBox1)
        Me.GroupBox2.Controls.Add(Me.TextBox3)
        Me.GroupBox2.Location = New System.Drawing.Point(1529, 217)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(605, 188)
        Me.GroupBox2.TabIndex = 82
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "GroupBox2"
        Me.GroupBox2.Visible = False
        '
        'DG_PCBInfoFromDB
        '
        Me.DG_PCBInfoFromDB.AllowUserToAddRows = False
        Me.DG_PCBInfoFromDB.AllowUserToDeleteRows = False
        Me.DG_PCBInfoFromDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_PCBInfoFromDB.Location = New System.Drawing.Point(402, 42)
        Me.DG_PCBInfoFromDB.Name = "DG_PCBInfoFromDB"
        Me.DG_PCBInfoFromDB.ReadOnly = True
        Me.DG_PCBInfoFromDB.Size = New System.Drawing.Size(158, 53)
        Me.DG_PCBInfoFromDB.TabIndex = 32
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(230, 20)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox2.Size = New System.Drawing.Size(154, 75)
        Me.TextBox2.TabIndex = 31
        '
        'DG_StepList
        '
        Me.DG_StepList.AllowUserToAddRows = False
        Me.DG_StepList.AllowUserToDeleteRows = False
        Me.DG_StepList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_StepList.Location = New System.Drawing.Point(230, 123)
        Me.DG_StepList.Name = "DG_StepList"
        Me.DG_StepList.ReadOnly = True
        Me.DG_StepList.Size = New System.Drawing.Size(159, 57)
        Me.DG_StepList.TabIndex = 32
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(26, 20)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(180, 75)
        Me.TextBox1.TabIndex = 31
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(27, 102)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox3.Size = New System.Drawing.Size(179, 78)
        Me.TextBox3.TabIndex = 31
        '
        'FAS_ErrorCodeTableAdapter
        '
        Me.FAS_ErrorCodeTableAdapter.ClearBeforeFill = True
        '
        'FASErrorCodeBindingSource
        '
        Me.FASErrorCodeBindingSource.DataMember = "FAS_ErrorCode"
        Me.FASErrorCodeBindingSource.DataSource = Me.FASDataSet
        '
        'FASDataSet
        '
        Me.FASDataSet.DataSetName = "FASDataSet"
        Me.FASDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CurrentTimeTimer
        '
        Me.CurrentTimeTimer.Interval = 1000
        '
        'BT_Pause
        '
        Me.BT_Pause.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BT_Pause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BT_Pause.Location = New System.Drawing.Point(25, 114)
        Me.BT_Pause.Name = "BT_Pause"
        Me.BT_Pause.Size = New System.Drawing.Size(18, 23)
        Me.BT_Pause.TabIndex = 29
        Me.BT_Pause.Text = "P"
        Me.BT_Pause.UseVisualStyleBackColor = False
        '
        'CurrrentTimeLabel
        '
        Me.CurrrentTimeLabel.AutoSize = True
        Me.CurrrentTimeLabel.BackColor = System.Drawing.SystemColors.Control
        Me.CurrrentTimeLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CurrrentTimeLabel.Location = New System.Drawing.Point(25, 18)
        Me.CurrrentTimeLabel.Name = "CurrrentTimeLabel"
        Me.CurrrentTimeLabel.Size = New System.Drawing.Size(156, 29)
        Me.CurrrentTimeLabel.TabIndex = 6
        Me.CurrrentTimeLabel.Text = "Current TIME"
        '
        'LB_SW_Wers
        '
        Me.LB_SW_Wers.AutoSize = True
        Me.LB_SW_Wers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_SW_Wers.Location = New System.Drawing.Point(281, 18)
        Me.LB_SW_Wers.Name = "LB_SW_Wers"
        Me.LB_SW_Wers.Size = New System.Drawing.Size(76, 16)
        Me.LB_SW_Wers.TabIndex = 34
        Me.LB_SW_Wers.Text = "SW_Wers"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label19.Location = New System.Drawing.Point(124, 18)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(159, 16)
        Me.Label19.TabIndex = 33
        Me.Label19.Text = "Версия приложения:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Red
        Me.Label15.Location = New System.Drawing.Point(6, 112)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(259, 31)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Карантин по лоту:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(55, 81)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(210, 31)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Успех по лоту:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label12.Location = New System.Drawing.Point(28, 50)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(237, 31)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "За день по лоту:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label17.Location = New System.Drawing.Point(458, 18)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(187, 31)
        Me.Label17.TabIndex = 1
        Me.Label17.Text = "Yield по лоту"
        '
        'LB_Procent
        '
        Me.LB_Procent.AutoSize = True
        Me.LB_Procent.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_Procent.Location = New System.Drawing.Point(610, 61)
        Me.LB_Procent.Name = "LB_Procent"
        Me.LB_Procent.Size = New System.Drawing.Size(68, 55)
        Me.LB_Procent.TabIndex = 0
        Me.LB_Procent.Text = "%"
        '
        'LB_Yield
        '
        Me.LB_Yield.AutoSize = True
        Me.LB_Yield.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_Yield.Location = New System.Drawing.Point(440, 61)
        Me.LB_Yield.Name = "LB_Yield"
        Me.LB_Yield.Size = New System.Drawing.Size(178, 55)
        Me.LB_Yield.TabIndex = 0
        Me.LB_Yield.Text = "100,00"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label10.Location = New System.Drawing.Point(41, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(224, 31)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "За день общее:"
        '
        'Label_ShiftCounter
        '
        Me.Label_ShiftCounter.AutoSize = True
        Me.Label_ShiftCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_ShiftCounter.Location = New System.Drawing.Point(270, 19)
        Me.Label_ShiftCounter.Name = "Label_ShiftCounter"
        Me.Label_ShiftCounter.Size = New System.Drawing.Size(46, 31)
        Me.Label_ShiftCounter.TabIndex = 0
        Me.Label_ShiftCounter.Text = "99"
        '
        'CB_Quality
        '
        Me.CB_Quality.AutoSize = True
        Me.CB_Quality.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CB_Quality.Image = Global.Scanning_Station.My.Resources.Resources.help__1_1
        Me.CB_Quality.Location = New System.Drawing.Point(67, 99)
        Me.CB_Quality.Name = "CB_Quality"
        Me.CB_Quality.Size = New System.Drawing.Size(36, 24)
        Me.CB_Quality.TabIndex = 33
        Me.CB_Quality.UseVisualStyleBackColor = True
        '
        'CB_GoldSample
        '
        Me.CB_GoldSample.AutoSize = True
        Me.CB_GoldSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CB_GoldSample.Image = Global.Scanning_Station.My.Resources.Resources.Fav
        Me.CB_GoldSample.Location = New System.Drawing.Point(6, 134)
        Me.CB_GoldSample.Name = "CB_GoldSample"
        Me.CB_GoldSample.Size = New System.Drawing.Size(40, 28)
        Me.CB_GoldSample.TabIndex = 33
        Me.CB_GoldSample.UseVisualStyleBackColor = True
        Me.CB_GoldSample.Visible = False
        '
        'BT_OpenSettings
        '
        Me.BT_OpenSettings.FlatAppearance.BorderSize = 0
        Me.BT_OpenSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_OpenSettings.Image = Global.Scanning_Station.My.Resources.Resources.package_utilities
        Me.BT_OpenSettings.Location = New System.Drawing.Point(1231, 228)
        Me.BT_OpenSettings.Name = "BT_OpenSettings"
        Me.BT_OpenSettings.Size = New System.Drawing.Size(82, 81)
        Me.BT_OpenSettings.TabIndex = 22
        Me.BT_OpenSettings.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label23)
        Me.GroupBox4.Controls.Add(Me.CB_Quality)
        Me.GroupBox4.Controls.Add(Me.CB_GoldSample)
        Me.GroupBox4.Controls.Add(Me.LB_SW_Wers)
        Me.GroupBox4.Controls.Add(Me.CB_User_Input)
        Me.GroupBox4.Controls.Add(Me.Label19)
        Me.GroupBox4.Controls.Add(Me.LB_CurrentStep)
        Me.GroupBox4.Controls.Add(Me.LabelAppName)
        Me.GroupBox4.Controls.Add(Me.L_UserName)
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.Label_StationName)
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Controls.Add(Me.L_Model)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.L_LOT)
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Controls.Add(Me.Lebel_StationLine)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(17, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(496, 178)
        Me.GroupBox4.TabIndex = 10
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Информация о ЛОТе и станции"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label23.Location = New System.Drawing.Point(460, 18)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(30, 13)
        Me.Label23.TabIndex = 36
        Me.Label23.Text = "FAS"
        '
        'CB_User_Input
        '
        Me.CB_User_Input.AutoSize = True
        Me.CB_User_Input.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CB_User_Input.Image = Global.Scanning_Station.My.Resources.Resources.User
        Me.CB_User_Input.Location = New System.Drawing.Point(6, 95)
        Me.CB_User_Input.Name = "CB_User_Input"
        Me.CB_User_Input.Size = New System.Drawing.Size(44, 32)
        Me.CB_User_Input.TabIndex = 33
        Me.CB_User_Input.UseVisualStyleBackColor = True
        '
        'LB_CurrentStep
        '
        Me.LB_CurrentStep.AutoSize = True
        Me.LB_CurrentStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_CurrentStep.Location = New System.Drawing.Point(283, 54)
        Me.LB_CurrentStep.Name = "LB_CurrentStep"
        Me.LB_CurrentStep.Size = New System.Drawing.Size(64, 20)
        Me.LB_CurrentStep.TabIndex = 20
        Me.LB_CurrentStep.Text = "fasend"
        '
        'LabelAppName
        '
        Me.LabelAppName.AutoSize = True
        Me.LabelAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelAppName.Location = New System.Drawing.Point(283, 34)
        Me.LabelAppName.Name = "LabelAppName"
        Me.LabelAppName.Size = New System.Drawing.Size(64, 20)
        Me.LabelAppName.TabIndex = 20
        Me.LabelAppName.Text = "fasend"
        '
        'L_UserName
        '
        Me.L_UserName.AutoSize = True
        Me.L_UserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_UserName.Location = New System.Drawing.Point(283, 74)
        Me.L_UserName.Name = "L_UserName"
        Me.L_UserName.Size = New System.Drawing.Size(174, 20)
        Me.L_UserName.TabIndex = 19
        Me.L_UserName.Text = "Имя пользователя:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.Location = New System.Drawing.Point(102, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(181, 20)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Название операции:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(279, 20)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Имя последнего пользователя:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.Location = New System.Drawing.Point(79, 34)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(204, 20)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Название приложения:"
        '
        'Label_StationName
        '
        Me.Label_StationName.AutoSize = True
        Me.Label_StationName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_StationName.Location = New System.Drawing.Point(283, 94)
        Me.Label_StationName.Name = "Label_StationName"
        Me.Label_StationName.Size = New System.Drawing.Size(33, 20)
        Me.Label_StationName.TabIndex = 16
        Me.Label_StationName.Text = "ПК"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.Location = New System.Drawing.Point(158, 94)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(125, 20)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Название ПК:"
        '
        'L_Model
        '
        Me.L_Model.AutoSize = True
        Me.L_Model.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_Model.Location = New System.Drawing.Point(283, 154)
        Me.L_Model.Name = "L_Model"
        Me.L_Model.Size = New System.Drawing.Size(57, 20)
        Me.L_Model.TabIndex = 16
        Me.L_Model.Text = "Model"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.Location = New System.Drawing.Point(202, 154)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(81, 20)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Модель:"
        '
        'L_LOT
        '
        Me.L_LOT.AutoSize = True
        Me.L_LOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_LOT.Location = New System.Drawing.Point(283, 134)
        Me.L_LOT.Name = "L_LOT"
        Me.L_LOT.Size = New System.Drawing.Size(42, 20)
        Me.L_LOT.TabIndex = 16
        Me.L_LOT.Text = "LOT"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.Location = New System.Drawing.Point(136, 134)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(147, 20)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Название ЛОТа:"
        '
        'Lebel_StationLine
        '
        Me.Lebel_StationLine.AutoSize = True
        Me.Lebel_StationLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Lebel_StationLine.Location = New System.Drawing.Point(283, 114)
        Me.Lebel_StationLine.Name = "Lebel_StationLine"
        Me.Lebel_StationLine.Size = New System.Drawing.Size(43, 20)
        Me.Lebel_StationLine.TabIndex = 16
        Me.Lebel_StationLine.Text = "Line"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(216, 114)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 20)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Линия:"
        '
        'LB_FailLotRes
        '
        Me.LB_FailLotRes.AutoSize = True
        Me.LB_FailLotRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_FailLotRes.ForeColor = System.Drawing.Color.Red
        Me.LB_FailLotRes.Location = New System.Drawing.Point(271, 112)
        Me.LB_FailLotRes.Name = "LB_FailLotRes"
        Me.LB_FailLotRes.Size = New System.Drawing.Size(46, 31)
        Me.LB_FailLotRes.TabIndex = 0
        Me.LB_FailLotRes.Text = "99"
        '
        'LB_PassLotRes
        '
        Me.LB_PassLotRes.AutoSize = True
        Me.LB_PassLotRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_PassLotRes.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LB_PassLotRes.Location = New System.Drawing.Point(271, 81)
        Me.LB_PassLotRes.Name = "LB_PassLotRes"
        Me.LB_PassLotRes.Size = New System.Drawing.Size(46, 31)
        Me.LB_PassLotRes.TabIndex = 0
        Me.LB_PassLotRes.Text = "99"
        '
        'LB_LOTCounter
        '
        Me.LB_LOTCounter.AutoSize = True
        Me.LB_LOTCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_LOTCounter.Location = New System.Drawing.Point(270, 50)
        Me.LB_LOTCounter.Name = "LB_LOTCounter"
        Me.LB_LOTCounter.Size = New System.Drawing.Size(46, 31)
        Me.LB_LOTCounter.TabIndex = 0
        Me.LB_LOTCounter.Text = "99"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.LB_Procent)
        Me.GroupBox1.Controls.Add(Me.LB_Yield)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label_ShiftCounter)
        Me.GroupBox1.Controls.Add(Me.LB_FailLotRes)
        Me.GroupBox1.Controls.Add(Me.LB_PassLotRes)
        Me.GroupBox1.Controls.Add(Me.LB_LOTCounter)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(533, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(706, 178)
        Me.GroupBox1.TabIndex = 32
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Счетчик выпуска продукции"
        '
        'BT_PCBInfo
        '
        Me.BT_PCBInfo.FlatAppearance.BorderSize = 0
        Me.BT_PCBInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_PCBInfo.Image = Global.Scanning_Station.My.Resources.Resources.Symbol_Information
        Me.BT_PCBInfo.Location = New System.Drawing.Point(1245, 10)
        Me.BT_PCBInfo.Name = "BT_PCBInfo"
        Me.BT_PCBInfo.Size = New System.Drawing.Size(75, 68)
        Me.BT_PCBInfo.TabIndex = 31
        Me.BT_PCBInfo.UseVisualStyleBackColor = True
        '
        'GB_WorkAria
        '
        Me.GB_WorkAria.Controls.Add(Me.GroupBox1)
        Me.GB_WorkAria.Controls.Add(Me.BT_PCBInfo)
        Me.GB_WorkAria.Controls.Add(Me.BT_OpenSettings)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox4)
        Me.GB_WorkAria.Controls.Add(Me.LB_CurrentErrCode)
        Me.GB_WorkAria.Controls.Add(Me.Controllabel)
        Me.GB_WorkAria.Controls.Add(Me.GB_ScanMode)
        Me.GB_WorkAria.Location = New System.Drawing.Point(3, 3)
        Me.GB_WorkAria.Name = "GB_WorkAria"
        Me.GB_WorkAria.Size = New System.Drawing.Size(1326, 715)
        Me.GB_WorkAria.TabIndex = 79
        Me.GB_WorkAria.TabStop = False
        '
        'LB_CurrentErrCode
        '
        Me.LB_CurrentErrCode.AutoSize = True
        Me.LB_CurrentErrCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_CurrentErrCode.Location = New System.Drawing.Point(12, 270)
        Me.LB_CurrentErrCode.Name = "LB_CurrentErrCode"
        Me.LB_CurrentErrCode.Size = New System.Drawing.Size(242, 29)
        Me.LB_CurrentErrCode.TabIndex = 21
        Me.LB_CurrentErrCode.Text = "LB_CurrentErrCode"
        '
        'Controllabel
        '
        Me.Controllabel.AutoSize = True
        Me.Controllabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Controllabel.Location = New System.Drawing.Point(12, 193)
        Me.Controllabel.Name = "Controllabel"
        Me.Controllabel.Size = New System.Drawing.Size(217, 29)
        Me.Controllabel.TabIndex = 21
        Me.Controllabel.Text = "CONTROLLABEL"
        '
        'GB_ScanMode
        '
        Me.GB_ScanMode.Controls.Add(Me.BT_CleareSN)
        Me.GB_ScanMode.Controls.Add(Me.Label2)
        Me.GB_ScanMode.Controls.Add(Me.GroupBox5)
        Me.GB_ScanMode.Controls.Add(Me.BT_Pause)
        Me.GB_ScanMode.Controls.Add(Me.DG_UpLog)
        Me.GB_ScanMode.Controls.Add(Me.BT_Pass)
        Me.GB_ScanMode.Controls.Add(Me.BT_Fail)
        Me.GB_ScanMode.Controls.Add(Me.SerialTextBox)
        Me.GB_ScanMode.Location = New System.Drawing.Point(6, 311)
        Me.GB_ScanMode.Name = "GB_ScanMode"
        Me.GB_ScanMode.Size = New System.Drawing.Size(1301, 398)
        Me.GB_ScanMode.TabIndex = 30
        Me.GB_ScanMode.TabStop = False
        '
        'BT_CleareSN
        '
        Me.BT_CleareSN.FlatAppearance.BorderSize = 0
        Me.BT_CleareSN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_CleareSN.Image = Global.Scanning_Station.My.Resources.Resources.edittrash
        Me.BT_CleareSN.Location = New System.Drawing.Point(703, 15)
        Me.BT_CleareSN.Name = "BT_CleareSN"
        Me.BT_CleareSN.Size = New System.Drawing.Size(66, 94)
        Me.BT_CleareSN.TabIndex = 28
        Me.BT_CleareSN.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.Location = New System.Drawing.Point(186, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(367, 25)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Строка ввода серийного номера"
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox5.Controls.Add(Me.CurrrentTimeLabel)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox5.Location = New System.Drawing.Point(928, 44)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(219, 55)
        Me.GroupBox5.TabIndex = 24
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Время"
        '
        'GB_PCBInfoMode
        '
        Me.GB_PCBInfoMode.Controls.Add(Me.DG_PCB_Steps)
        Me.GB_PCBInfoMode.Controls.Add(Me.TB_GetPCPInfo)
        Me.GB_PCBInfoMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_PCBInfoMode.Location = New System.Drawing.Point(28, 746)
        Me.GB_PCBInfoMode.Name = "GB_PCBInfoMode"
        Me.GB_PCBInfoMode.Size = New System.Drawing.Size(1301, 394)
        Me.GB_PCBInfoMode.TabIndex = 32
        Me.GB_PCBInfoMode.TabStop = False
        Me.GB_PCBInfoMode.Visible = False
        '
        'DG_PCB_Steps
        '
        Me.DG_PCB_Steps.AllowUserToAddRows = False
        Me.DG_PCB_Steps.AllowUserToDeleteRows = False
        DataGridViewCellStyle52.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_PCB_Steps.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle52
        Me.DG_PCB_Steps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_PCB_Steps.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle53.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle53.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle53.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle53.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle53.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle53.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_PCB_Steps.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle53
        Me.DG_PCB_Steps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle54.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle54.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle54.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle54.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle54.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle54.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DG_PCB_Steps.DefaultCellStyle = DataGridViewCellStyle54
        Me.DG_PCB_Steps.Location = New System.Drawing.Point(14, 109)
        Me.DG_PCB_Steps.Name = "DG_PCB_Steps"
        Me.DG_PCB_Steps.ReadOnly = True
        DataGridViewCellStyle55.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle55.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle55.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle55.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle55.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle55.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle55.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_PCB_Steps.RowHeadersDefaultCellStyle = DataGridViewCellStyle55
        DataGridViewCellStyle56.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_PCB_Steps.RowsDefaultCellStyle = DataGridViewCellStyle56
        Me.DG_PCB_Steps.Size = New System.Drawing.Size(1273, 276)
        Me.DG_PCB_Steps.TabIndex = 26
        '
        'TB_GetPCPInfo
        '
        Me.TB_GetPCPInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TB_GetPCPInfo.Location = New System.Drawing.Point(187, 47)
        Me.TB_GetPCPInfo.Name = "TB_GetPCPInfo"
        Me.TB_GetPCPInfo.Size = New System.Drawing.Size(508, 31)
        Me.TB_GetPCPInfo.TabIndex = 1
        '
        'WF_for_FAS_SN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1924, 1061)
        Me.Controls.Add(Me.GB_UserData)
        Me.Controls.Add(Me.GB_ErrorCode)
        Me.Controls.Add(Me.GB_PCBInfoMode)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GB_WorkAria)
        Me.Name = "WF_for_FAS_SN"
        Me.Text = "WF_for_FAS_SN"
        CType(Me.DG_UpLog, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_ErrorCodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_UserData.ResumeLayout(False)
        Me.GB_UserData.PerformLayout()
        Me.GB_ErrorCode.ResumeLayout(False)
        Me.GB_ErrorCode.PerformLayout()
        CType(Me.Num_X, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Num_Y, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DG_PCBInfoFromDB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_StepList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FASErrorCodeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FASDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GB_WorkAria.ResumeLayout(False)
        Me.GB_WorkAria.PerformLayout()
        Me.GB_ScanMode.ResumeLayout(False)
        Me.GB_ScanMode.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GB_PCBInfoMode.ResumeLayout(False)
        Me.GB_PCBInfoMode.PerformLayout()
        CType(Me.DG_PCB_Steps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BT_LOGInClose As Button
    Friend WithEvents DG_UpLog As DataGridView
    Friend WithEvents Num As DataGridViewTextBoxColumn
    Friend WithEvents SNumber As DataGridViewTextBoxColumn
    Friend WithEvents SCIDTab As DataGridViewTextBoxColumn
    Friend WithEvents CASIDTab As DataGridViewTextBoxColumn
    Friend WithEvents HDCP As DataGridViewTextBoxColumn
    Friend WithEvents CERT As DataGridViewTextBoxColumn
    Friend WithEvents DG_ErrorCodes As DataGridView
    Friend WithEvents BT_SeveErCode As Button
    Friend WithEvents TB_Description As TextBox
    Friend WithEvents CB_ErrorCode As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents BT_Pass As Button
    Friend WithEvents BT_Fail As Button
    Friend WithEvents BT_CloseErrMode As Button
    Friend WithEvents SerialTextBox As TextBox
    Friend WithEvents GB_UserData As GroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents TB_RFIDIn As TextBox
    Friend WithEvents GB_ErrorCode As GroupBox
    Friend WithEvents Label22 As Label
    Friend WithEvents CB_CheckID As CheckBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Num_X As NumericUpDown
    Friend WithEvents BT_Save_Coordinats As Button
    Friend WithEvents Num_Y As NumericUpDown
    Friend WithEvents Label21 As Label
    Friend WithEvents CB_DefaultPrinter As ComboBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents DG_PCBInfoFromDB As DataGridView
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents DG_StepList As DataGridView
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents FAS_ErrorCodeTableAdapter As FASDataSetTableAdapters.FAS_ErrorCodeTableAdapter
    Friend WithEvents FASErrorCodeBindingSource As BindingSource
    Friend WithEvents FASDataSet As FASDataSet
    Friend WithEvents CurrentTimeTimer As Timer
    Friend WithEvents BT_Pause As Button
    Friend WithEvents CurrrentTimeLabel As Label
    Friend WithEvents LB_SW_Wers As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents LB_Procent As Label
    Friend WithEvents LB_Yield As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label_ShiftCounter As Label
    Friend WithEvents CB_Quality As CheckBox
    Friend WithEvents CB_GoldSample As CheckBox
    Friend WithEvents BT_OpenSettings As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents CB_User_Input As CheckBox
    Friend WithEvents LB_CurrentStep As Label
    Friend WithEvents LabelAppName As Label
    Friend WithEvents L_UserName As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label_StationName As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents L_Model As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents L_LOT As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Lebel_StationLine As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents LB_FailLotRes As Label
    Friend WithEvents LB_PassLotRes As Label
    Friend WithEvents LB_LOTCounter As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents BT_PCBInfo As Button
    Friend WithEvents GB_WorkAria As GroupBox
    Friend WithEvents LB_CurrentErrCode As Label
    Friend WithEvents Controllabel As Label
    Friend WithEvents GB_ScanMode As GroupBox
    Friend WithEvents BT_CleareSN As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents GB_PCBInfoMode As GroupBox
    Friend WithEvents DG_PCB_Steps As DataGridView
    Friend WithEvents TB_GetPCPInfo As TextBox
    Friend WithEvents Label23 As Label
End Class
