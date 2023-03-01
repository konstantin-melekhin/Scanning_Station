<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SilverLabelPrint
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.LB_SW_Wers = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.GB_SelectedLOT = New System.Windows.Forms.GroupBox()
        Me.L_UserName = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LB_SN_InLOT_Ready = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Num_X = New System.Windows.Forms.NumericUpDown()
        Me.BT_Save_Coordinats = New System.Windows.Forms.Button()
        Me.Num_Y = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CB_DefaultPrinter = New System.Windows.Forms.ComboBox()
        Me.DG_SN_ForPrint = New System.Windows.Forms.DataGridView()
        Me.Num_SNToPrint = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LB_COUNT_INLOT = New System.Windows.Forms.Label()
        Me.Controllabel = New System.Windows.Forms.Label()
        Me.DG_Printed_SN = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BT_SN_Reapload = New System.Windows.Forms.Button()
        Me.GB_UserData = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TB_RFIDIn = New System.Windows.Forms.TextBox()
        Me.GB_WorkAria = New System.Windows.Forms.GroupBox()
        Me.BT_OpenSettings = New System.Windows.Forms.Button()
        Me.BT_Refrash = New System.Windows.Forms.Button()
        Me.BT_GenAndPrintSN = New System.Windows.Forms.Button()
        Me.BT_LOGInClose = New System.Windows.Forms.Button()
        Me.GB_SelectedLOT.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.Num_X, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Num_Y, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_SN_ForPrint, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Num_SNToPrint, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_Printed_SN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_UserData.SuspendLayout()
        Me.GB_WorkAria.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(1085, 18)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(237, 94)
        Me.TextBox1.TabIndex = 81
        '
        'LB_SW_Wers
        '
        Me.LB_SW_Wers.AutoSize = True
        Me.LB_SW_Wers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_SW_Wers.Location = New System.Drawing.Point(204, 22)
        Me.LB_SW_Wers.Name = "LB_SW_Wers"
        Me.LB_SW_Wers.Size = New System.Drawing.Size(88, 20)
        Me.LB_SW_Wers.TabIndex = 63
        Me.LB_SW_Wers.Text = "SW_Wers"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label19.Location = New System.Drawing.Point(18, 22)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(183, 20)
        Me.Label19.TabIndex = 62
        Me.Label19.Text = "Версия приложения:"
        '
        'GB_SelectedLOT
        '
        Me.GB_SelectedLOT.Controls.Add(Me.L_UserName)
        Me.GB_SelectedLOT.Controls.Add(Me.Label5)
        Me.GB_SelectedLOT.Controls.Add(Me.LB_SW_Wers)
        Me.GB_SelectedLOT.Controls.Add(Me.LB_SN_InLOT_Ready)
        Me.GB_SelectedLOT.Controls.Add(Me.GroupBox1)
        Me.GB_SelectedLOT.Controls.Add(Me.Label19)
        Me.GB_SelectedLOT.Controls.Add(Me.Controllabel)
        Me.GB_SelectedLOT.Controls.Add(Me.DG_SN_ForPrint)
        Me.GB_SelectedLOT.Controls.Add(Me.Num_SNToPrint)
        Me.GB_SelectedLOT.Controls.Add(Me.Label3)
        Me.GB_SelectedLOT.Controls.Add(Me.Label4)
        Me.GB_SelectedLOT.Controls.Add(Me.LB_COUNT_INLOT)
        Me.GB_SelectedLOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_SelectedLOT.Location = New System.Drawing.Point(15, 19)
        Me.GB_SelectedLOT.Name = "GB_SelectedLOT"
        Me.GB_SelectedLOT.Size = New System.Drawing.Size(1050, 567)
        Me.GB_SelectedLOT.TabIndex = 78
        Me.GB_SelectedLOT.TabStop = False
        Me.GB_SelectedLOT.Text = "Настройки ЛОТа"
        '
        'L_UserName
        '
        Me.L_UserName.AutoSize = True
        Me.L_UserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.L_UserName.Location = New System.Drawing.Point(478, 22)
        Me.L_UserName.Name = "L_UserName"
        Me.L_UserName.Size = New System.Drawing.Size(174, 20)
        Me.L_UserName.TabIndex = 81
        Me.L_UserName.Text = "Имя пользователя:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.Location = New System.Drawing.Point(298, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(174, 20)
        Me.Label5.TabIndex = 82
        Me.Label5.Text = "Имя пользователя:"
        '
        'LB_SN_InLOT_Ready
        '
        Me.LB_SN_InLOT_Ready.AutoSize = True
        Me.LB_SN_InLOT_Ready.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_SN_InLOT_Ready.Location = New System.Drawing.Point(466, 102)
        Me.LB_SN_InLOT_Ready.Name = "LB_SN_InLOT_Ready"
        Me.LB_SN_InLOT_Ready.Size = New System.Drawing.Size(30, 31)
        Me.LB_SN_InLOT_Ready.TabIndex = 79
        Me.LB_SN_InLOT_Ready.Text = "0"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Num_X)
        Me.GroupBox1.Controls.Add(Me.BT_Save_Coordinats)
        Me.GroupBox1.Controls.Add(Me.Num_Y)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.CB_DefaultPrinter)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(686, 22)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(343, 170)
        Me.GroupBox1.TabIndex = 77
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Настройки координат принтера"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(106, 20)
        Me.Label6.TabIndex = 64
        Me.Label6.Text = "Корекция X"
        '
        'Num_X
        '
        Me.Num_X.Location = New System.Drawing.Point(119, 102)
        Me.Num_X.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Num_X.Name = "Num_X"
        Me.Num_X.Size = New System.Drawing.Size(69, 26)
        Me.Num_X.TabIndex = 62
        '
        'BT_Save_Coordinats
        '
        Me.BT_Save_Coordinats.FlatAppearance.BorderSize = 0
        Me.BT_Save_Coordinats.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Save_Coordinats.Location = New System.Drawing.Point(194, 100)
        Me.BT_Save_Coordinats.Name = "BT_Save_Coordinats"
        Me.BT_Save_Coordinats.Size = New System.Drawing.Size(58, 63)
        Me.BT_Save_Coordinats.TabIndex = 65
        Me.BT_Save_Coordinats.UseVisualStyleBackColor = True
        '
        'Num_Y
        '
        Me.Num_Y.Location = New System.Drawing.Point(119, 128)
        Me.Num_Y.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Num_Y.Name = "Num_Y"
        Me.Num_Y.Size = New System.Drawing.Size(69, 26)
        Me.Num_Y.TabIndex = 63
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 133)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(106, 20)
        Me.Label7.TabIndex = 64
        Me.Label7.Text = "Корекция Y"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(271, 20)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "Выберите принтер для печати"
        '
        'CB_DefaultPrinter
        '
        Me.CB_DefaultPrinter.FormattingEnabled = True
        Me.CB_DefaultPrinter.Location = New System.Drawing.Point(6, 68)
        Me.CB_DefaultPrinter.Name = "CB_DefaultPrinter"
        Me.CB_DefaultPrinter.Size = New System.Drawing.Size(303, 28)
        Me.CB_DefaultPrinter.TabIndex = 47
        '
        'DG_SN_ForPrint
        '
        Me.DG_SN_ForPrint.AllowUserToAddRows = False
        Me.DG_SN_ForPrint.AllowUserToDeleteRows = False
        Me.DG_SN_ForPrint.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_SN_ForPrint.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DG_SN_ForPrint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_SN_ForPrint.Location = New System.Drawing.Point(0, 195)
        Me.DG_SN_ForPrint.Name = "DG_SN_ForPrint"
        Me.DG_SN_ForPrint.ReadOnly = True
        Me.DG_SN_ForPrint.Size = New System.Drawing.Size(1029, 330)
        Me.DG_SN_ForPrint.TabIndex = 76
        '
        'Num_SNToPrint
        '
        Me.Num_SNToPrint.Increment = New Decimal(New Integer() {2, 0, 0, 0})
        Me.Num_SNToPrint.Location = New System.Drawing.Point(12, 68)
        Me.Num_SNToPrint.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.Num_SNToPrint.Name = "Num_SNToPrint"
        Me.Num_SNToPrint.Size = New System.Drawing.Size(201, 26)
        Me.Num_SNToPrint.TabIndex = 60
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(290, 20)
        Me.Label3.TabIndex = 55
        Me.Label3.Text = "Количество номеров для печати"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 161)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(409, 31)
        Me.Label4.TabIndex = 73
        Me.Label4.Text = "Таблица номеров для печати"
        '
        'LB_COUNT_INLOT
        '
        Me.LB_COUNT_INLOT.AutoSize = True
        Me.LB_COUNT_INLOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_COUNT_INLOT.Location = New System.Drawing.Point(7, 102)
        Me.LB_COUNT_INLOT.Name = "LB_COUNT_INLOT"
        Me.LB_COUNT_INLOT.Size = New System.Drawing.Size(453, 31)
        Me.LB_COUNT_INLOT.TabIndex = 80
        Me.LB_COUNT_INLOT.Text = "Всего доступно номеров в ЛОТе"
        '
        'Controllabel
        '
        Me.Controllabel.AutoSize = True
        Me.Controllabel.Location = New System.Drawing.Point(54, 345)
        Me.Controllabel.Name = "Controllabel"
        Me.Controllabel.Size = New System.Drawing.Size(63, 20)
        Me.Controllabel.TabIndex = 72
        Me.Controllabel.Text = "Label1"
        '
        'DG_Printed_SN
        '
        Me.DG_Printed_SN.AllowUserToAddRows = False
        Me.DG_Printed_SN.AllowUserToDeleteRows = False
        Me.DG_Printed_SN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_Printed_SN.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DG_Printed_SN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_Printed_SN.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.DG_Printed_SN.Location = New System.Drawing.Point(1085, 120)
        Me.DG_Printed_SN.Name = "DG_Printed_SN"
        Me.DG_Printed_SN.Size = New System.Drawing.Size(237, 465)
        Me.DG_Printed_SN.TabIndex = 82
        '
        'Column1
        '
        Me.Column1.HeaderText = "#"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 39
        '
        'Column2
        '
        Me.Column2.HeaderText = "SN"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 47
        '
        'BT_SN_Reapload
        '
        Me.BT_SN_Reapload.Location = New System.Drawing.Point(1085, 608)
        Me.BT_SN_Reapload.Name = "BT_SN_Reapload"
        Me.BT_SN_Reapload.Size = New System.Drawing.Size(237, 23)
        Me.BT_SN_Reapload.TabIndex = 83
        Me.BT_SN_Reapload.Text = "Вернуть выбранный номер"
        Me.BT_SN_Reapload.UseVisualStyleBackColor = True
        '
        'GB_UserData
        '
        Me.GB_UserData.BackColor = System.Drawing.Color.NavajoWhite
        Me.GB_UserData.Controls.Add(Me.BT_LOGInClose)
        Me.GB_UserData.Controls.Add(Me.Label13)
        Me.GB_UserData.Controls.Add(Me.TB_RFIDIn)
        Me.GB_UserData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GB_UserData.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_UserData.Location = New System.Drawing.Point(1344, 12)
        Me.GB_UserData.Name = "GB_UserData"
        Me.GB_UserData.Size = New System.Drawing.Size(449, 197)
        Me.GB_UserData.TabIndex = 85
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
        'GB_WorkAria
        '
        Me.GB_WorkAria.Controls.Add(Me.GB_SelectedLOT)
        Me.GB_WorkAria.Controls.Add(Me.BT_OpenSettings)
        Me.GB_WorkAria.Controls.Add(Me.BT_SN_Reapload)
        Me.GB_WorkAria.Controls.Add(Me.DG_Printed_SN)
        Me.GB_WorkAria.Controls.Add(Me.BT_Refrash)
        Me.GB_WorkAria.Controls.Add(Me.TextBox1)
        Me.GB_WorkAria.Controls.Add(Me.BT_GenAndPrintSN)
        Me.GB_WorkAria.Location = New System.Drawing.Point(2, 2)
        Me.GB_WorkAria.Name = "GB_WorkAria"
        Me.GB_WorkAria.Size = New System.Drawing.Size(1416, 681)
        Me.GB_WorkAria.TabIndex = 86
        Me.GB_WorkAria.TabStop = False
        '
        'BT_OpenSettings
        '
        Me.BT_OpenSettings.FlatAppearance.BorderSize = 0
        Me.BT_OpenSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_OpenSettings.Image = Global.Scanning_Station.My.Resources.Resources.package_utilities
        Me.BT_OpenSettings.Location = New System.Drawing.Point(962, 592)
        Me.BT_OpenSettings.Name = "BT_OpenSettings"
        Me.BT_OpenSettings.Size = New System.Drawing.Size(82, 81)
        Me.BT_OpenSettings.TabIndex = 84
        Me.BT_OpenSettings.UseVisualStyleBackColor = True
        '
        'BT_Refrash
        '
        Me.BT_Refrash.FlatAppearance.BorderSize = 0
        Me.BT_Refrash.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_Refrash.Image = Global.Scanning_Station.My.Resources.Resources.refresh__2_
        Me.BT_Refrash.Location = New System.Drawing.Point(436, 592)
        Me.BT_Refrash.Name = "BT_Refrash"
        Me.BT_Refrash.Size = New System.Drawing.Size(75, 55)
        Me.BT_Refrash.TabIndex = 75
        Me.BT_Refrash.UseVisualStyleBackColor = True
        '
        'BT_GenAndPrintSN
        '
        Me.BT_GenAndPrintSN.FlatAppearance.BorderSize = 0
        Me.BT_GenAndPrintSN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_GenAndPrintSN.Image = Global.Scanning_Station.My.Resources.Resources.Print_icon
        Me.BT_GenAndPrintSN.Location = New System.Drawing.Point(15, 591)
        Me.BT_GenAndPrintSN.Name = "BT_GenAndPrintSN"
        Me.BT_GenAndPrintSN.Size = New System.Drawing.Size(74, 66)
        Me.BT_GenAndPrintSN.TabIndex = 74
        Me.BT_GenAndPrintSN.UseVisualStyleBackColor = True
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
        'SilverLabelPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1799, 813)
        Me.Controls.Add(Me.GB_WorkAria)
        Me.Controls.Add(Me.GB_UserData)
        Me.Name = "SilverLabelPrint"
        Me.Text = "SilverLabelPrint"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GB_SelectedLOT.ResumeLayout(False)
        Me.GB_SelectedLOT.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.Num_X, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Num_Y, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_SN_ForPrint, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Num_SNToPrint, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_Printed_SN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_UserData.ResumeLayout(False)
        Me.GB_UserData.PerformLayout()
        Me.GB_WorkAria.ResumeLayout(False)
        Me.GB_WorkAria.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents LB_SW_Wers As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents GB_SelectedLOT As GroupBox
    Friend WithEvents Num_SNToPrint As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Num_X As NumericUpDown
    Friend WithEvents BT_Save_Coordinats As Button
    Friend WithEvents Num_Y As NumericUpDown
    Friend WithEvents Label7 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents CB_DefaultPrinter As ComboBox
    Friend WithEvents DG_SN_ForPrint As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents BT_Refrash As Button
    Friend WithEvents BT_GenAndPrintSN As Button
    Friend WithEvents Controllabel As Label
    Friend WithEvents LB_SN_InLOT_Ready As Label
    Friend WithEvents LB_COUNT_INLOT As Label
    Friend WithEvents DG_Printed_SN As DataGridView
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents BT_SN_Reapload As Button
    Friend WithEvents BT_OpenSettings As Button
    Friend WithEvents GB_UserData As GroupBox
    Friend WithEvents BT_LOGInClose As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents TB_RFIDIn As TextBox
    Friend WithEvents L_UserName As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents GB_WorkAria As GroupBox
End Class
