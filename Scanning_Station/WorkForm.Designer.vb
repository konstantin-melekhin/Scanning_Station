<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WorkForm
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GB_WorkAria = New System.Windows.Forms.GroupBox()
        Me.BT_Fail = New System.Windows.Forms.Button()
        Me.BT_Pass = New System.Windows.Forms.Button()
        Me.DG_UpLog = New System.Windows.Forms.DataGridView()
        Me.Num = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SCIDTab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CASIDTab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HDCP = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CERT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.CurrrentTimeLabel = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LB_LOTCounter = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label_ShiftCounter = New System.Windows.Forms.Label()
        Me.Controllabel = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SerialTextBox = New System.Windows.Forms.TextBox()
        Me.BT_OpenSettings = New System.Windows.Forms.Button()
        Me.GB_UserData = New System.Windows.Forms.GroupBox()
        Me.BT_LOGInClose = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TB_RFIDIn = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.CurrentTimeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.DG_StepList = New System.Windows.Forms.DataGridView()
        Me.GB_ErrorCode = New System.Windows.Forms.GroupBox()
        Me.DG_ErrorCodes = New System.Windows.Forms.DataGridView()
        Me.BT_SeveErCode = New System.Windows.Forms.Button()
        Me.TB_Description = New System.Windows.Forms.TextBox()
        Me.CB_ErrorCode = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.FASDataSet = New Scanning_Station.FASDataSet()
        Me.FASErrorCodeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.FAS_ErrorCodeTableAdapter = New Scanning_Station.FASDataSetTableAdapters.FAS_ErrorCodeTableAdapter()
        Me.GB_WorkAria.SuspendLayout()
        CType(Me.DG_UpLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GB_UserData.SuspendLayout()
        CType(Me.DG_StepList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_ErrorCode.SuspendLayout()
        CType(Me.DG_ErrorCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FASDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FASErrorCodeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GB_WorkAria
        '
        Me.GB_WorkAria.Controls.Add(Me.BT_Fail)
        Me.GB_WorkAria.Controls.Add(Me.BT_Pass)
        Me.GB_WorkAria.Controls.Add(Me.DG_UpLog)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox5)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox1)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox6)
        Me.GB_WorkAria.Controls.Add(Me.Controllabel)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox4)
        Me.GB_WorkAria.Controls.Add(Me.Label2)
        Me.GB_WorkAria.Controls.Add(Me.SerialTextBox)
        Me.GB_WorkAria.Controls.Add(Me.BT_OpenSettings)
        Me.GB_WorkAria.Location = New System.Drawing.Point(12, 12)
        Me.GB_WorkAria.Name = "GB_WorkAria"
        Me.GB_WorkAria.Size = New System.Drawing.Size(1365, 801)
        Me.GB_WorkAria.TabIndex = 24
        Me.GB_WorkAria.TabStop = False
        Me.GB_WorkAria.Visible = False
        '
        'BT_Fail
        '
        Me.BT_Fail.Enabled = False
        Me.BT_Fail.FlatAppearance.BorderSize = 0
        Me.BT_Fail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Fail.Image = Global.Scanning_Station.My.Resources.Resources.agt_action_fail__1_
        Me.BT_Fail.Location = New System.Drawing.Point(438, 363)
        Me.BT_Fail.Name = "BT_Fail"
        Me.BT_Fail.Size = New System.Drawing.Size(87, 91)
        Me.BT_Fail.TabIndex = 27
        Me.BT_Fail.UseVisualStyleBackColor = True
        '
        'BT_Pass
        '
        Me.BT_Pass.Enabled = False
        Me.BT_Pass.FlatAppearance.BorderSize = 0
        Me.BT_Pass.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Pass.Image = Global.Scanning_Station.My.Resources.Resources.agt_action_success__1_
        Me.BT_Pass.Location = New System.Drawing.Point(17, 363)
        Me.BT_Pass.Name = "BT_Pass"
        Me.BT_Pass.Size = New System.Drawing.Size(97, 91)
        Me.BT_Pass.TabIndex = 26
        Me.BT_Pass.UseVisualStyleBackColor = True
        '
        'DG_UpLog
        '
        Me.DG_UpLog.AllowUserToAddRows = False
        Me.DG_UpLog.AllowUserToDeleteRows = False
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_UpLog.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle6
        Me.DG_UpLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_UpLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_UpLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.DG_UpLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_UpLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Num, Me.SNumber, Me.SCIDTab, Me.CASIDTab, Me.HDCP, Me.CERT})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DG_UpLog.DefaultCellStyle = DataGridViewCellStyle8
        Me.DG_UpLog.Location = New System.Drawing.Point(17, 460)
        Me.DG_UpLog.Name = "DG_UpLog"
        Me.DG_UpLog.ReadOnly = True
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_UpLog.RowHeadersDefaultCellStyle = DataGridViewCellStyle9
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_UpLog.RowsDefaultCellStyle = DataGridViewCellStyle10
        Me.DG_UpLog.Size = New System.Drawing.Size(1342, 299)
        Me.DG_UpLog.TabIndex = 25
        '
        'Num
        '
        Me.Num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Num.HeaderText = "№"
        Me.Num.Name = "Num"
        Me.Num.ReadOnly = True
        Me.Num.Width = 48
        '
        'SNumber
        '
        Me.SNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.SNumber.HeaderText = "Серийный номер"
        Me.SNumber.Name = "SNumber"
        Me.SNumber.ReadOnly = True
        Me.SNumber.Width = 143
        '
        'SCIDTab
        '
        Me.SCIDTab.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.SCIDTab.HeaderText = "Результат"
        Me.SCIDTab.Name = "SCIDTab"
        Me.SCIDTab.ReadOnly = True
        Me.SCIDTab.Width = 112
        '
        'CASIDTab
        '
        Me.CASIDTab.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CASIDTab.HeaderText = "Дата"
        Me.CASIDTab.Name = "CASIDTab"
        Me.CASIDTab.ReadOnly = True
        Me.CASIDTab.Width = 69
        '
        'HDCP
        '
        Me.HDCP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.HDCP.HeaderText = "Код ошибки"
        Me.HDCP.Name = "HDCP"
        Me.HDCP.ReadOnly = True
        Me.HDCP.Width = 108
        '
        'CERT
        '
        Me.CERT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CERT.HeaderText = "Примечание"
        Me.CERT.Name = "CERT"
        Me.CERT.ReadOnly = True
        Me.CERT.Width = 126
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox5.Controls.Add(Me.CurrrentTimeLabel)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox5.Location = New System.Drawing.Point(610, 301)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(219, 55)
        Me.GroupBox5.TabIndex = 24
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Время"
        '
        'CurrrentTimeLabel
        '
        Me.CurrrentTimeLabel.AutoSize = True
        Me.CurrrentTimeLabel.BackColor = System.Drawing.SystemColors.Control
        Me.CurrrentTimeLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CurrrentTimeLabel.Location = New System.Drawing.Point(6, 18)
        Me.CurrrentTimeLabel.Name = "CurrrentTimeLabel"
        Me.CurrrentTimeLabel.Size = New System.Drawing.Size(156, 29)
        Me.CurrrentTimeLabel.TabIndex = 6
        Me.CurrrentTimeLabel.Text = "Current TIME"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.LightCoral
        Me.GroupBox1.Controls.Add(Me.LB_LOTCounter)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(310, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(348, 117)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Выпуск по лоту"
        '
        'LB_LOTCounter
        '
        Me.LB_LOTCounter.AutoSize = True
        Me.LB_LOTCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 50.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_LOTCounter.Location = New System.Drawing.Point(6, 25)
        Me.LB_LOTCounter.Name = "LB_LOTCounter"
        Me.LB_LOTCounter.Size = New System.Drawing.Size(109, 76)
        Me.LB_LOTCounter.TabIndex = 0
        Me.LB_LOTCounter.Text = "99"
        '
        'GroupBox6
        '
        Me.GroupBox6.BackColor = System.Drawing.Color.RosyBrown
        Me.GroupBox6.Controls.Add(Me.Label_ShiftCounter)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(17, 8)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(641, 117)
        Me.GroupBox6.TabIndex = 24
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Выпуск за смену"
        '
        'Label_ShiftCounter
        '
        Me.Label_ShiftCounter.AutoSize = True
        Me.Label_ShiftCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 50.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_ShiftCounter.Location = New System.Drawing.Point(6, 25)
        Me.Label_ShiftCounter.Name = "Label_ShiftCounter"
        Me.Label_ShiftCounter.Size = New System.Drawing.Size(109, 76)
        Me.Label_ShiftCounter.TabIndex = 0
        Me.Label_ShiftCounter.Text = "99"
        '
        'Controllabel
        '
        Me.Controllabel.AutoSize = True
        Me.Controllabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Controllabel.Location = New System.Drawing.Point(26, 155)
        Me.Controllabel.Name = "Controllabel"
        Me.Controllabel.Size = New System.Drawing.Size(217, 29)
        Me.Controllabel.TabIndex = 21
        Me.Controllabel.Text = "CONTROLLABEL"
        '
        'GroupBox4
        '
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
        Me.GroupBox4.Location = New System.Drawing.Point(840, 8)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(519, 227)
        Me.GroupBox4.TabIndex = 10
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Информация о ЛОТе и станции"
        '
        'LB_CurrentStep
        '
        Me.LB_CurrentStep.AutoSize = True
        Me.LB_CurrentStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_CurrentStep.Location = New System.Drawing.Point(228, 46)
        Me.LB_CurrentStep.Name = "LB_CurrentStep"
        Me.LB_CurrentStep.Size = New System.Drawing.Size(64, 20)
        Me.LB_CurrentStep.TabIndex = 20
        Me.LB_CurrentStep.Text = "fasend"
        '
        'LabelAppName
        '
        Me.LabelAppName.AutoSize = True
        Me.LabelAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelAppName.Location = New System.Drawing.Point(228, 19)
        Me.LabelAppName.Name = "LabelAppName"
        Me.LabelAppName.Size = New System.Drawing.Size(64, 20)
        Me.LabelAppName.TabIndex = 20
        Me.LabelAppName.Text = "fasend"
        '
        'L_UserName
        '
        Me.L_UserName.AutoSize = True
        Me.L_UserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_UserName.Location = New System.Drawing.Point(228, 181)
        Me.L_UserName.Name = "L_UserName"
        Me.L_UserName.Size = New System.Drawing.Size(174, 20)
        Me.L_UserName.TabIndex = 19
        Me.L_UserName.Text = "Имя пользователя:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.Location = New System.Drawing.Point(44, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(181, 20)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Название операции:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.Location = New System.Drawing.Point(51, 181)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(174, 20)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Имя пользователя:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.Location = New System.Drawing.Point(21, 19)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(204, 20)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Название приложения:"
        '
        'Label_StationName
        '
        Me.Label_StationName.AutoSize = True
        Me.Label_StationName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_StationName.Location = New System.Drawing.Point(228, 73)
        Me.Label_StationName.Name = "Label_StationName"
        Me.Label_StationName.Size = New System.Drawing.Size(33, 20)
        Me.Label_StationName.TabIndex = 16
        Me.Label_StationName.Text = "ПК"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.Location = New System.Drawing.Point(100, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(125, 20)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Название ПК:"
        '
        'L_Model
        '
        Me.L_Model.AutoSize = True
        Me.L_Model.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_Model.Location = New System.Drawing.Point(228, 154)
        Me.L_Model.Name = "L_Model"
        Me.L_Model.Size = New System.Drawing.Size(57, 20)
        Me.L_Model.TabIndex = 16
        Me.L_Model.Text = "Model"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.Location = New System.Drawing.Point(144, 154)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(81, 20)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Модель:"
        '
        'L_LOT
        '
        Me.L_LOT.AutoSize = True
        Me.L_LOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_LOT.Location = New System.Drawing.Point(228, 127)
        Me.L_LOT.Name = "L_LOT"
        Me.L_LOT.Size = New System.Drawing.Size(42, 20)
        Me.L_LOT.TabIndex = 16
        Me.L_LOT.Text = "LOT"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.Location = New System.Drawing.Point(75, 127)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(147, 20)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Название ЛОТа:"
        '
        'Lebel_StationLine
        '
        Me.Lebel_StationLine.AutoSize = True
        Me.Lebel_StationLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Lebel_StationLine.Location = New System.Drawing.Point(228, 100)
        Me.Lebel_StationLine.Name = "Lebel_StationLine"
        Me.Lebel_StationLine.Size = New System.Drawing.Size(43, 20)
        Me.Lebel_StationLine.TabIndex = 16
        Me.Lebel_StationLine.Text = "Line"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(158, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 20)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Линия:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.Location = New System.Drawing.Point(14, 297)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(161, 25)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Serial Number"
        '
        'SerialTextBox
        '
        Me.SerialTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.SerialTextBox.Location = New System.Drawing.Point(17, 325)
        Me.SerialTextBox.Name = "SerialTextBox"
        Me.SerialTextBox.Size = New System.Drawing.Size(508, 31)
        Me.SerialTextBox.TabIndex = 1
        '
        'BT_OpenSettings
        '
        Me.BT_OpenSettings.FlatAppearance.BorderSize = 0
        Me.BT_OpenSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_OpenSettings.Image = Global.Scanning_Station.My.Resources.Resources.settings__1_
        Me.BT_OpenSettings.Location = New System.Drawing.Point(1298, 241)
        Me.BT_OpenSettings.Name = "BT_OpenSettings"
        Me.BT_OpenSettings.Size = New System.Drawing.Size(61, 56)
        Me.BT_OpenSettings.TabIndex = 22
        Me.BT_OpenSettings.UseVisualStyleBackColor = True
        '
        'GB_UserData
        '
        Me.GB_UserData.BackColor = System.Drawing.Color.NavajoWhite
        Me.GB_UserData.Controls.Add(Me.BT_LOGInClose)
        Me.GB_UserData.Controls.Add(Me.Label13)
        Me.GB_UserData.Controls.Add(Me.TB_RFIDIn)
        Me.GB_UserData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GB_UserData.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_UserData.Location = New System.Drawing.Point(1505, 15)
        Me.GB_UserData.Name = "GB_UserData"
        Me.GB_UserData.Size = New System.Drawing.Size(449, 197)
        Me.GB_UserData.TabIndex = 30
        Me.GB_UserData.TabStop = False
        Me.GB_UserData.Text = "Регистрация пользователя"
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
        Me.TB_RFIDIn.Text = "0000181218"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(1516, 255)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(197, 141)
        Me.TextBox1.TabIndex = 31
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(1742, 255)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox2.Size = New System.Drawing.Size(197, 141)
        Me.TextBox2.TabIndex = 31
        '
        'CurrentTimeTimer
        '
        Me.CurrentTimeTimer.Interval = 1000
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(1517, 411)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox3.Size = New System.Drawing.Size(197, 141)
        Me.TextBox3.TabIndex = 31
        '
        'DG_StepList
        '
        Me.DG_StepList.AllowUserToAddRows = False
        Me.DG_StepList.AllowUserToDeleteRows = False
        Me.DG_StepList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_StepList.Location = New System.Drawing.Point(1742, 411)
        Me.DG_StepList.Name = "DG_StepList"
        Me.DG_StepList.ReadOnly = True
        Me.DG_StepList.Size = New System.Drawing.Size(197, 141)
        Me.DG_StepList.TabIndex = 32
        '
        'GB_ErrorCode
        '
        Me.GB_ErrorCode.Controls.Add(Me.DG_ErrorCodes)
        Me.GB_ErrorCode.Controls.Add(Me.BT_SeveErCode)
        Me.GB_ErrorCode.Controls.Add(Me.TB_Description)
        Me.GB_ErrorCode.Controls.Add(Me.CB_ErrorCode)
        Me.GB_ErrorCode.Controls.Add(Me.Label8)
        Me.GB_ErrorCode.Controls.Add(Me.Label4)
        Me.GB_ErrorCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_ErrorCode.Location = New System.Drawing.Point(1491, 478)
        Me.GB_ErrorCode.Name = "GB_ErrorCode"
        Me.GB_ErrorCode.Size = New System.Drawing.Size(419, 335)
        Me.GB_ErrorCode.TabIndex = 33
        Me.GB_ErrorCode.TabStop = False
        Me.GB_ErrorCode.Text = "Регистрация кода ошибки"
        Me.GB_ErrorCode.Visible = False
        '
        'DG_ErrorCodes
        '
        Me.DG_ErrorCodes.AllowUserToAddRows = False
        Me.DG_ErrorCodes.AllowUserToDeleteRows = False
        Me.DG_ErrorCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_ErrorCodes.Location = New System.Drawing.Point(299, 37)
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
        Me.BT_SeveErCode.Location = New System.Drawing.Point(341, 250)
        Me.BT_SeveErCode.Name = "BT_SeveErCode"
        Me.BT_SeveErCode.Size = New System.Drawing.Size(72, 79)
        Me.BT_SeveErCode.TabIndex = 3
        Me.BT_SeveErCode.UseVisualStyleBackColor = True
        '
        'TB_Description
        '
        Me.TB_Description.Location = New System.Drawing.Point(16, 189)
        Me.TB_Description.Multiline = True
        Me.TB_Description.Name = "TB_Description"
        Me.TB_Description.Size = New System.Drawing.Size(304, 140)
        Me.TB_Description.TabIndex = 2
        '
        'CB_ErrorCode
        '
        Me.CB_ErrorCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.CB_ErrorCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CB_ErrorCode.FormattingEnabled = True
        Me.CB_ErrorCode.Location = New System.Drawing.Point(16, 97)
        Me.CB_ErrorCode.Name = "CB_ErrorCode"
        Me.CB_ErrorCode.Size = New System.Drawing.Size(266, 39)
        Me.CB_ErrorCode.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 155)
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
        'FASDataSet
        '
        Me.FASDataSet.DataSetName = "FASDataSet"
        Me.FASDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'FASErrorCodeBindingSource
        '
        Me.FASErrorCodeBindingSource.DataMember = "FAS_ErrorCode"
        Me.FASErrorCodeBindingSource.DataSource = Me.FASDataSet
        '
        'FAS_ErrorCodeTableAdapter
        '
        Me.FAS_ErrorCodeTableAdapter.ClearBeforeFill = True
        '
        'WorkForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1981, 841)
        Me.Controls.Add(Me.GB_ErrorCode)
        Me.Controls.Add(Me.DG_StepList)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.GB_UserData)
        Me.Controls.Add(Me.GB_WorkAria)
        Me.Name = "WorkForm"
        Me.Text = "WorkForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GB_WorkAria.ResumeLayout(False)
        Me.GB_WorkAria.PerformLayout()
        CType(Me.DG_UpLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GB_UserData.ResumeLayout(False)
        Me.GB_UserData.PerformLayout()
        CType(Me.DG_StepList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_ErrorCode.ResumeLayout(False)
        Me.GB_ErrorCode.PerformLayout()
        CType(Me.DG_ErrorCodes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FASDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FASErrorCodeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GB_WorkAria As GroupBox
    Friend WithEvents DG_UpLog As DataGridView
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents CurrrentTimeLabel As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LB_LOTCounter As Label
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents Label_ShiftCounter As Label
    Friend WithEvents Controllabel As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents LabelAppName As Label
    Friend WithEvents L_UserName As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label_StationName As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents L_Model As Label
    Friend WithEvents BT_OpenSettings As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents L_LOT As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Lebel_StationLine As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents SerialTextBox As TextBox
    Friend WithEvents GB_UserData As GroupBox
    Friend WithEvents BT_LOGInClose As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents TB_RFIDIn As TextBox
    Friend WithEvents LB_CurrentStep As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Num As DataGridViewTextBoxColumn
    Friend WithEvents SNumber As DataGridViewTextBoxColumn
    Friend WithEvents SCIDTab As DataGridViewTextBoxColumn
    Friend WithEvents CASIDTab As DataGridViewTextBoxColumn
    Friend WithEvents HDCP As DataGridViewTextBoxColumn
    Friend WithEvents CERT As DataGridViewTextBoxColumn
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents CurrentTimeTimer As Timer
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents BT_Fail As Button
    Friend WithEvents BT_Pass As Button
    Friend WithEvents DG_StepList As DataGridView
    Friend WithEvents GB_ErrorCode As GroupBox
    Friend WithEvents BT_SeveErCode As Button
    Friend WithEvents TB_Description As TextBox
    Friend WithEvents CB_ErrorCode As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents DG_ErrorCodes As DataGridView
    Friend WithEvents FASDataSet As FASDataSet
    Friend WithEvents FASErrorCodeBindingSource As BindingSource
    Friend WithEvents FAS_ErrorCodeTableAdapter As FASDataSetTableAdapters.FAS_ErrorCodeTableAdapter
End Class
