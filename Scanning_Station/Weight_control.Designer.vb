<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Weight_control
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Weight_control))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label_ShiftCounter = New System.Windows.Forms.Label()
        Me.LB_LOTCounter = New System.Windows.Forms.Label()
        Me.BT_OpenSettings = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.LB_CurrentStep = New System.Windows.Forms.Label()
        Me.LB_SW_Wers = New System.Windows.Forms.Label()
        Me.LabelAppName = New System.Windows.Forms.Label()
        Me.L_UserName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label_StationName = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.L_Model = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.L_LOT = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Lebel_StationLine = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.CurrrentTimeLabel = New System.Windows.Forms.Label()
        Me.Controllabel = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.ScanDateLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BT_LogOut = New System.Windows.Forms.Button()
        Me.TB_RFIDIn = New System.Windows.Forms.TextBox()
        Me.GB_UserData = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.SerialTextBox = New System.Windows.Forms.TextBox()
        Me.CurrentTimeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.GB_NotVisible = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.DG_StepList = New System.Windows.Forms.DataGridView()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MinTB = New System.Windows.Forms.TextBox()
        Me.MaxTB = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GB_AutoSettings = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CB_COMPorts = New System.Windows.Forms.ComboBox()
        Me.LB_defoltWht = New System.Windows.Forms.Label()
        Me.Num_Deviation = New System.Windows.Forms.NumericUpDown()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TB_AutoSetSNin = New System.Windows.Forms.TextBox()
        Me.GB_ManualSettings = New System.Windows.Forms.GroupBox()
        Me.WeightSerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.GB_WorkAria = New System.Windows.Forms.GroupBox()
        Me.BT_Pause = New System.Windows.Forms.Button()
        Me.DG_UpLog = New System.Windows.Forms.DataGridView()
        Me.Num = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CASIDTab = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BT_CleareSN = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GB_UserData.SuspendLayout()
        Me.GB_NotVisible.SuspendLayout()
        CType(Me.DG_StepList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_AutoSettings.SuspendLayout()
        CType(Me.Num_Deviation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_ManualSettings.SuspendLayout()
        Me.GB_WorkAria.SuspendLayout()
        CType(Me.DG_UpLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.Label_ShiftCounter)
        Me.GroupBox1.Controls.Add(Me.LB_LOTCounter)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(10, 10)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(311, 124)
        Me.GroupBox1.TabIndex = 40
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Счетчик выпуска продукции"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label20.Location = New System.Drawing.Point(6, 70)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(161, 39)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "По лоту:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label21.Location = New System.Drawing.Point(18, 27)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(149, 39)
        Me.Label21.TabIndex = 1
        Me.Label21.Text = "Общий:"
        '
        'Label_ShiftCounter
        '
        Me.Label_ShiftCounter.AutoSize = True
        Me.Label_ShiftCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_ShiftCounter.Location = New System.Drawing.Point(161, 27)
        Me.Label_ShiftCounter.Name = "Label_ShiftCounter"
        Me.Label_ShiftCounter.Size = New System.Drawing.Size(57, 39)
        Me.Label_ShiftCounter.TabIndex = 0
        Me.Label_ShiftCounter.Text = "99"
        '
        'LB_LOTCounter
        '
        Me.LB_LOTCounter.AutoSize = True
        Me.LB_LOTCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_LOTCounter.Location = New System.Drawing.Point(161, 70)
        Me.LB_LOTCounter.Name = "LB_LOTCounter"
        Me.LB_LOTCounter.Size = New System.Drawing.Size(57, 39)
        Me.LB_LOTCounter.TabIndex = 0
        Me.LB_LOTCounter.Text = "99"
        '
        'BT_OpenSettings
        '
        Me.BT_OpenSettings.FlatAppearance.BorderSize = 0
        Me.BT_OpenSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_OpenSettings.Location = New System.Drawing.Point(996, 271)
        Me.BT_OpenSettings.Name = "BT_OpenSettings"
        Me.BT_OpenSettings.Size = New System.Drawing.Size(82, 81)
        Me.BT_OpenSettings.TabIndex = 39
        Me.BT_OpenSettings.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.LB_CurrentStep)
        Me.GroupBox4.Controls.Add(Me.LB_SW_Wers)
        Me.GroupBox4.Controls.Add(Me.LabelAppName)
        Me.GroupBox4.Controls.Add(Me.L_UserName)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Controls.Add(Me.Label_StationName)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.L_Model)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Controls.Add(Me.L_LOT)
        Me.GroupBox4.Controls.Add(Me.Label18)
        Me.GroupBox4.Controls.Add(Me.Lebel_StationLine)
        Me.GroupBox4.Controls.Add(Me.Label19)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(625, 10)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(472, 178)
        Me.GroupBox4.TabIndex = 38
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Информация о ЛОТе и станции"
        '
        'LB_CurrentStep
        '
        Me.LB_CurrentStep.AutoSize = True
        Me.LB_CurrentStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_CurrentStep.Location = New System.Drawing.Point(187, 64)
        Me.LB_CurrentStep.Name = "LB_CurrentStep"
        Me.LB_CurrentStep.Size = New System.Drawing.Size(55, 16)
        Me.LB_CurrentStep.TabIndex = 20
        Me.LB_CurrentStep.Text = "fasend"
        '
        'LB_SW_Wers
        '
        Me.LB_SW_Wers.AutoSize = True
        Me.LB_SW_Wers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_SW_Wers.Location = New System.Drawing.Point(187, 29)
        Me.LB_SW_Wers.Name = "LB_SW_Wers"
        Me.LB_SW_Wers.Size = New System.Drawing.Size(76, 16)
        Me.LB_SW_Wers.TabIndex = 20
        Me.LB_SW_Wers.Text = "SW_Wers"
        '
        'LabelAppName
        '
        Me.LabelAppName.AutoSize = True
        Me.LabelAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelAppName.Location = New System.Drawing.Point(187, 46)
        Me.LabelAppName.Name = "LabelAppName"
        Me.LabelAppName.Size = New System.Drawing.Size(55, 16)
        Me.LabelAppName.TabIndex = 20
        Me.LabelAppName.Text = "fasend"
        '
        'L_UserName
        '
        Me.L_UserName.AutoSize = True
        Me.L_UserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_UserName.Location = New System.Drawing.Point(187, 82)
        Me.L_UserName.Name = "L_UserName"
        Me.L_UserName.Size = New System.Drawing.Size(150, 16)
        Me.L_UserName.TabIndex = 19
        Me.L_UserName.Text = "Имя пользователя:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(27, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(162, 16)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Название операции:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.Location = New System.Drawing.Point(30, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(159, 16)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Версия приложения:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.Location = New System.Drawing.Point(39, 82)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(150, 16)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Имя пользователя:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.Location = New System.Drawing.Point(9, 46)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(180, 16)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Название приложения:"
        '
        'Label_StationName
        '
        Me.Label_StationName.AutoSize = True
        Me.Label_StationName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_StationName.Location = New System.Drawing.Point(187, 100)
        Me.Label_StationName.Name = "Label_StationName"
        Me.Label_StationName.Size = New System.Drawing.Size(28, 16)
        Me.Label_StationName.TabIndex = 16
        Me.Label_StationName.Text = "ПК"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.Location = New System.Drawing.Point(79, 100)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(110, 16)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Название ПК:"
        '
        'L_Model
        '
        Me.L_Model.AutoSize = True
        Me.L_Model.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_Model.Location = New System.Drawing.Point(187, 154)
        Me.L_Model.Name = "L_Model"
        Me.L_Model.Size = New System.Drawing.Size(51, 16)
        Me.L_Model.TabIndex = 16
        Me.L_Model.Text = "Model"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label12.Location = New System.Drawing.Point(121, 154)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(68, 16)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Модель:"
        '
        'L_LOT
        '
        Me.L_LOT.AutoSize = True
        Me.L_LOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_LOT.Location = New System.Drawing.Point(187, 136)
        Me.L_LOT.Name = "L_LOT"
        Me.L_LOT.Size = New System.Drawing.Size(37, 16)
        Me.L_LOT.TabIndex = 16
        Me.L_LOT.Text = "LOT"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label18.Location = New System.Drawing.Point(59, 136)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(130, 16)
        Me.Label18.TabIndex = 16
        Me.Label18.Text = "Название ЛОТа:"
        '
        'Lebel_StationLine
        '
        Me.Lebel_StationLine.AutoSize = True
        Me.Lebel_StationLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Lebel_StationLine.Location = New System.Drawing.Point(187, 118)
        Me.Lebel_StationLine.Name = "Lebel_StationLine"
        Me.Lebel_StationLine.Size = New System.Drawing.Size(37, 16)
        Me.Lebel_StationLine.TabIndex = 16
        Me.Lebel_StationLine.Text = "Line"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label19.Location = New System.Drawing.Point(132, 118)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(57, 16)
        Me.Label19.TabIndex = 16
        Me.Label19.Text = "Линия:"
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox5.Controls.Add(Me.CurrrentTimeLabel)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox5.Location = New System.Drawing.Point(730, 301)
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
        'Controllabel
        '
        Me.Controllabel.AutoSize = True
        Me.Controllabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Controllabel.Location = New System.Drawing.Point(26, 198)
        Me.Controllabel.Name = "Controllabel"
        Me.Controllabel.Size = New System.Drawing.Size(217, 29)
        Me.Controllabel.TabIndex = 21
        Me.Controllabel.Text = "CONTROLLABEL"
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox3.Controls.Add(Me.ScanDateLabel)
        Me.GroupBox3.Location = New System.Drawing.Point(683, 305)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(221, 51)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "ScanDate"
        Me.GroupBox3.Visible = False
        '
        'ScanDateLabel
        '
        Me.ScanDateLabel.AutoSize = True
        Me.ScanDateLabel.BackColor = System.Drawing.SystemColors.Control
        Me.ScanDateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.ScanDateLabel.Location = New System.Drawing.Point(6, 16)
        Me.ScanDateLabel.Name = "ScanDateLabel"
        Me.ScanDateLabel.Size = New System.Drawing.Size(126, 25)
        Me.ScanDateLabel.TabIndex = 6
        Me.ScanDateLabel.Text = "SCAN TIME"
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
        'BT_LogOut
        '
        Me.BT_LogOut.BackColor = System.Drawing.Color.Transparent
        Me.BT_LogOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BT_LogOut.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.BT_LogOut.FlatAppearance.BorderSize = 0
        Me.BT_LogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_LogOut.ForeColor = System.Drawing.Color.Transparent
        Me.BT_LogOut.Location = New System.Drawing.Point(334, 59)
        Me.BT_LogOut.Name = "BT_LogOut"
        Me.BT_LogOut.Size = New System.Drawing.Size(78, 89)
        Me.BT_LogOut.TabIndex = 30
        Me.BT_LogOut.UseVisualStyleBackColor = False
        '
        'TB_RFIDIn
        '
        Me.TB_RFIDIn.Location = New System.Drawing.Point(11, 88)
        Me.TB_RFIDIn.Name = "TB_RFIDIn"
        Me.TB_RFIDIn.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TB_RFIDIn.Size = New System.Drawing.Size(305, 31)
        Me.TB_RFIDIn.TabIndex = 0
        '
        'GB_UserData
        '
        Me.GB_UserData.BackColor = System.Drawing.Color.Tan
        Me.GB_UserData.Controls.Add(Me.BT_LogOut)
        Me.GB_UserData.Controls.Add(Me.Label13)
        Me.GB_UserData.Controls.Add(Me.TB_RFIDIn)
        Me.GB_UserData.Enabled = False
        Me.GB_UserData.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_UserData.Location = New System.Drawing.Point(1147, 6)
        Me.GB_UserData.Name = "GB_UserData"
        Me.GB_UserData.Size = New System.Drawing.Size(429, 177)
        Me.GB_UserData.TabIndex = 55
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
        'SerialTextBox
        '
        Me.SerialTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.SerialTextBox.Location = New System.Drawing.Point(17, 321)
        Me.SerialTextBox.Name = "SerialTextBox"
        Me.SerialTextBox.Size = New System.Drawing.Size(481, 31)
        Me.SerialTextBox.TabIndex = 37
        '
        'CurrentTimeTimer
        '
        Me.CurrentTimeTimer.Interval = 1000
        '
        'GB_NotVisible
        '
        Me.GB_NotVisible.Controls.Add(Me.Label17)
        Me.GB_NotVisible.Controls.Add(Me.DG_StepList)
        Me.GB_NotVisible.Location = New System.Drawing.Point(1147, 204)
        Me.GB_NotVisible.Name = "GB_NotVisible"
        Me.GB_NotVisible.Size = New System.Drawing.Size(228, 133)
        Me.GB_NotVisible.TabIndex = 54
        Me.GB_NotVisible.TabStop = False
        Me.GB_NotVisible.Text = "NotVisible"
        Me.GB_NotVisible.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(15, 16)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(70, 13)
        Me.Label17.TabIndex = 55
        Me.Label17.Text = "DG_StepList "
        '
        'DG_StepList
        '
        Me.DG_StepList.AllowUserToAddRows = False
        Me.DG_StepList.AllowUserToDeleteRows = False
        Me.DG_StepList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_StepList.Location = New System.Drawing.Point(15, 32)
        Me.DG_StepList.Name = "DG_StepList"
        Me.DG_StepList.ReadOnly = True
        Me.DG_StepList.Size = New System.Drawing.Size(159, 57)
        Me.DG_StepList.TabIndex = 54
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Button1.Location = New System.Drawing.Point(111, 111)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(113, 41)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "ok"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MinTB
        '
        Me.MinTB.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.MinTB.Location = New System.Drawing.Point(86, 67)
        Me.MinTB.Name = "MinTB"
        Me.MinTB.Size = New System.Drawing.Size(138, 38)
        Me.MinTB.TabIndex = 5
        '
        'MaxTB
        '
        Me.MaxTB.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.MaxTB.Location = New System.Drawing.Point(86, 23)
        Me.MaxTB.Name = "MaxTB"
        Me.MaxTB.Size = New System.Drawing.Size(138, 38)
        Me.MaxTB.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 67)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(57, 31)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Min"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.Location = New System.Drawing.Point(16, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 31)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Max"
        '
        'GB_AutoSettings
        '
        Me.GB_AutoSettings.Controls.Add(Me.Label4)
        Me.GB_AutoSettings.Controls.Add(Me.CB_COMPorts)
        Me.GB_AutoSettings.Controls.Add(Me.LB_defoltWht)
        Me.GB_AutoSettings.Controls.Add(Me.Num_Deviation)
        Me.GB_AutoSettings.Controls.Add(Me.Label16)
        Me.GB_AutoSettings.Controls.Add(Me.Label10)
        Me.GB_AutoSettings.Controls.Add(Me.TB_AutoSetSNin)
        Me.GB_AutoSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_AutoSettings.Location = New System.Drawing.Point(14, 687)
        Me.GB_AutoSettings.Name = "GB_AutoSettings"
        Me.GB_AutoSettings.Size = New System.Drawing.Size(870, 366)
        Me.GB_AutoSettings.TabIndex = 57
        Me.GB_AutoSettings.TabStop = False
        Me.GB_AutoSettings.Text = "Настройка весов"
        Me.GB_AutoSettings.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(566, 25)
        Me.Label4.TabIndex = 50
        Me.Label4.Text = "Выберите COM порт к которому подключены весы"
        '
        'CB_COMPorts
        '
        Me.CB_COMPorts.FormattingEnabled = True
        Me.CB_COMPorts.Location = New System.Drawing.Point(21, 55)
        Me.CB_COMPorts.Name = "CB_COMPorts"
        Me.CB_COMPorts.Size = New System.Drawing.Size(303, 33)
        Me.CB_COMPorts.TabIndex = 49
        '
        'LB_defoltWht
        '
        Me.LB_defoltWht.AutoSize = True
        Me.LB_defoltWht.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_defoltWht.Location = New System.Drawing.Point(16, 334)
        Me.LB_defoltWht.Name = "LB_defoltWht"
        Me.LB_defoltWht.Size = New System.Drawing.Size(217, 29)
        Me.LB_defoltWht.TabIndex = 48
        Me.LB_defoltWht.Text = "CONTROLLABEL"
        '
        'Num_Deviation
        '
        Me.Num_Deviation.Location = New System.Drawing.Point(606, 91)
        Me.Num_Deviation.Name = "Num_Deviation"
        Me.Num_Deviation.Size = New System.Drawing.Size(120, 31)
        Me.Num_Deviation.TabIndex = 47
        Me.Num_Deviation.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label16.Location = New System.Drawing.Point(16, 205)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(344, 25)
        Me.Label16.TabIndex = 46
        Me.Label16.Text = "Окно ввода серийного номера"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label10.Location = New System.Drawing.Point(16, 93)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(833, 100)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = resources.GetString("Label10.Text")
        '
        'TB_AutoSetSNin
        '
        Me.TB_AutoSetSNin.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TB_AutoSetSNin.Location = New System.Drawing.Point(16, 233)
        Me.TB_AutoSetSNin.Name = "TB_AutoSetSNin"
        Me.TB_AutoSetSNin.Size = New System.Drawing.Size(525, 31)
        Me.TB_AutoSetSNin.TabIndex = 0
        Me.TB_AutoSetSNin.Text = "SBB16X09AG000052"
        '
        'GB_ManualSettings
        '
        Me.GB_ManualSettings.Controls.Add(Me.Button1)
        Me.GB_ManualSettings.Controls.Add(Me.MinTB)
        Me.GB_ManualSettings.Controls.Add(Me.MaxTB)
        Me.GB_ManualSettings.Controls.Add(Me.Label8)
        Me.GB_ManualSettings.Controls.Add(Me.Label9)
        Me.GB_ManualSettings.Location = New System.Drawing.Point(1147, 343)
        Me.GB_ManualSettings.Name = "GB_ManualSettings"
        Me.GB_ManualSettings.Size = New System.Drawing.Size(282, 175)
        Me.GB_ManualSettings.TabIndex = 56
        Me.GB_ManualSettings.TabStop = False
        Me.GB_ManualSettings.Text = "Ручная настройка весов"
        Me.GB_ManualSettings.Visible = False
        '
        'WeightSerialPort
        '
        Me.WeightSerialPort.PortName = "COM23"
        '
        'GB_WorkAria
        '
        Me.GB_WorkAria.Controls.Add(Me.BT_Pause)
        Me.GB_WorkAria.Controls.Add(Me.DG_UpLog)
        Me.GB_WorkAria.Controls.Add(Me.BT_CleareSN)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox1)
        Me.GB_WorkAria.Controls.Add(Me.BT_OpenSettings)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox4)
        Me.GB_WorkAria.Controls.Add(Me.SerialTextBox)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox5)
        Me.GB_WorkAria.Controls.Add(Me.Controllabel)
        Me.GB_WorkAria.Controls.Add(Me.GroupBox3)
        Me.GB_WorkAria.Controls.Add(Me.Label2)
        Me.GB_WorkAria.Location = New System.Drawing.Point(4, 6)
        Me.GB_WorkAria.Name = "GB_WorkAria"
        Me.GB_WorkAria.Size = New System.Drawing.Size(1103, 665)
        Me.GB_WorkAria.TabIndex = 53
        Me.GB_WorkAria.TabStop = False
        Me.GB_WorkAria.Visible = False
        '
        'BT_Pause
        '
        Me.BT_Pause.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BT_Pause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BT_Pause.Location = New System.Drawing.Point(26, 371)
        Me.BT_Pause.Name = "BT_Pause"
        Me.BT_Pause.Size = New System.Drawing.Size(18, 20)
        Me.BT_Pause.TabIndex = 41
        Me.BT_Pause.Text = "P"
        Me.BT_Pause.UseVisualStyleBackColor = False
        '
        'DG_UpLog
        '
        Me.DG_UpLog.AllowUserToAddRows = False
        Me.DG_UpLog.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_UpLog.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DG_UpLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_UpLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_UpLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DG_UpLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_UpLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Num, Me.SNumber, Me.Column2, Me.CASIDTab})
        Me.DG_UpLog.Location = New System.Drawing.Point(17, 358)
        Me.DG_UpLog.Name = "DG_UpLog"
        Me.DG_UpLog.ReadOnly = True
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_UpLog.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DG_UpLog.Size = New System.Drawing.Size(969, 283)
        Me.DG_UpLog.TabIndex = 43
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
        'Column2
        '
        Me.Column2.HeaderText = "Масса, кг"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 104
        '
        'CASIDTab
        '
        Me.CASIDTab.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.CASIDTab.HeaderText = "Дата"
        Me.CASIDTab.Name = "CASIDTab"
        Me.CASIDTab.ReadOnly = True
        Me.CASIDTab.Width = 77
        '
        'BT_CleareSN
        '
        Me.BT_CleareSN.FlatAppearance.BorderSize = 0
        Me.BT_CleareSN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_CleareSN.Location = New System.Drawing.Point(504, 287)
        Me.BT_CleareSN.Name = "BT_CleareSN"
        Me.BT_CleareSN.Size = New System.Drawing.Size(68, 69)
        Me.BT_CleareSN.TabIndex = 42
        Me.BT_CleareSN.UseVisualStyleBackColor = True
        '
        'Weight_control
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1649, 1053)
        Me.Controls.Add(Me.GB_UserData)
        Me.Controls.Add(Me.GB_NotVisible)
        Me.Controls.Add(Me.GB_AutoSettings)
        Me.Controls.Add(Me.GB_ManualSettings)
        Me.Controls.Add(Me.GB_WorkAria)
        Me.Name = "Weight_control"
        Me.Text = "Weight_control"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GB_UserData.ResumeLayout(False)
        Me.GB_UserData.PerformLayout()
        Me.GB_NotVisible.ResumeLayout(False)
        Me.GB_NotVisible.PerformLayout()
        CType(Me.DG_StepList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_AutoSettings.ResumeLayout(False)
        Me.GB_AutoSettings.PerformLayout()
        CType(Me.Num_Deviation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_ManualSettings.ResumeLayout(False)
        Me.GB_ManualSettings.PerformLayout()
        Me.GB_WorkAria.ResumeLayout(False)
        Me.GB_WorkAria.PerformLayout()
        CType(Me.DG_UpLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label_ShiftCounter As Label
    Friend WithEvents LB_LOTCounter As Label
    Friend WithEvents BT_OpenSettings As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents LB_CurrentStep As Label
    Friend WithEvents LB_SW_Wers As Label
    Friend WithEvents LabelAppName As Label
    Friend WithEvents L_UserName As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label_StationName As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents L_Model As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents L_LOT As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Lebel_StationLine As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents CurrrentTimeLabel As Label
    Friend WithEvents Controllabel As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents ScanDateLabel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BT_LogOut As Button
    Friend WithEvents TB_RFIDIn As TextBox
    Friend WithEvents GB_UserData As GroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents SerialTextBox As TextBox
    Friend WithEvents CurrentTimeTimer As Timer
    Friend WithEvents GB_NotVisible As GroupBox
    Friend WithEvents Label17 As Label
    Friend WithEvents DG_StepList As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents MinTB As TextBox
    Friend WithEvents MaxTB As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents GB_AutoSettings As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents CB_COMPorts As ComboBox
    Friend WithEvents LB_defoltWht As Label
    Friend WithEvents Num_Deviation As NumericUpDown
    Friend WithEvents Label16 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents TB_AutoSetSNin As TextBox
    Friend WithEvents GB_ManualSettings As GroupBox
    Friend WithEvents WeightSerialPort As IO.Ports.SerialPort
    Friend WithEvents GB_WorkAria As GroupBox
    Friend WithEvents BT_Pause As Button
    Friend WithEvents DG_UpLog As DataGridView
    Friend WithEvents Num As DataGridViewTextBoxColumn
    Friend WithEvents SNumber As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents CASIDTab As DataGridViewTextBoxColumn
    Friend WithEvents BT_CleareSN As Button
End Class
