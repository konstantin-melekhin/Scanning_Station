﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GB_NotVisibleElements = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.DG_Steps = New System.Windows.Forms.DataGridView()
        Me.DG_LineList = New System.Windows.Forms.DataGridView()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DG_LotList = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Lebel_StationLine = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LB_SelectLine = New System.Windows.Forms.Label()
        Me.LabelAppName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LB_CurrentStep = New System.Windows.Forms.Label()
        Me.Label_StationName = New System.Windows.Forms.Label()
        Me.DG_LOTListPresent = New System.Windows.Forms.DataGridView()
        Me.FullLOTCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ModelName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOT_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.L_Result = New System.Windows.Forms.Label()
        Me.GB_SelectLine = New System.Windows.Forms.GroupBox()
        Me.GB_LineSettings = New System.Windows.Forms.GroupBox()
        Me.BT_CloseLineSet = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CB_Steps = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.BT_SaveLine = New System.Windows.Forms.Button()
        Me.CB_Line = New System.Windows.Forms.ComboBox()
        Me.DG_AppList = New System.Windows.Forms.DataGridView()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.BT_SelectLot = New System.Windows.Forms.Button()
        Me.BT_RefreshLOT = New System.Windows.Forms.Button()
        Me.LB_SW_Wers = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.GB_NotVisibleElements.SuspendLayout()
        CType(Me.DG_Steps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_LineList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_LotList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.DG_LOTListPresent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_SelectLine.SuspendLayout()
        Me.GB_LineSettings.SuspendLayout()
        CType(Me.DG_AppList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GB_NotVisibleElements
        '
        Me.GB_NotVisibleElements.Controls.Add(Me.Label10)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label8)
        Me.GB_NotVisibleElements.Controls.Add(Me.DG_Steps)
        Me.GB_NotVisibleElements.Controls.Add(Me.DG_LineList)
        Me.GB_NotVisibleElements.Controls.Add(Me.TextBox1)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label4)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label6)
        Me.GB_NotVisibleElements.Controls.Add(Me.DG_LotList)
        Me.GB_NotVisibleElements.Location = New System.Drawing.Point(1337, 35)
        Me.GB_NotVisibleElements.Margin = New System.Windows.Forms.Padding(4)
        Me.GB_NotVisibleElements.Name = "GB_NotVisibleElements"
        Me.GB_NotVisibleElements.Padding = New System.Windows.Forms.Padding(4)
        Me.GB_NotVisibleElements.Size = New System.Drawing.Size(379, 255)
        Me.GB_NotVisibleElements.TabIndex = 44
        Me.GB_NotVisibleElements.TabStop = False
        Me.GB_NotVisibleElements.Text = "Неотображаемые элементы"
        Me.GB_NotVisibleElements.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(167, 116)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 13)
        Me.Label10.TabIndex = 47
        Me.Label10.Text = "Операция"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(4, 116)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(39, 13)
        Me.Label8.TabIndex = 47
        Me.Label8.Text = "Линия"
        '
        'DG_Steps
        '
        Me.DG_Steps.AllowUserToAddRows = False
        Me.DG_Steps.AllowUserToDeleteRows = False
        Me.DG_Steps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_Steps.Location = New System.Drawing.Point(170, 132)
        Me.DG_Steps.Name = "DG_Steps"
        Me.DG_Steps.ReadOnly = True
        Me.DG_Steps.Size = New System.Drawing.Size(144, 116)
        Me.DG_Steps.TabIndex = 48
        '
        'DG_LineList
        '
        Me.DG_LineList.AllowUserToAddRows = False
        Me.DG_LineList.AllowUserToDeleteRows = False
        Me.DG_LineList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_LineList.Location = New System.Drawing.Point(7, 132)
        Me.DG_LineList.Name = "DG_LineList"
        Me.DG_LineList.ReadOnly = True
        Me.DG_LineList.Size = New System.Drawing.Size(144, 116)
        Me.DG_LineList.TabIndex = 48
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(115, 35)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(257, 69)
        Me.TextBox1.TabIndex = 46
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(127, 19)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Station Info"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 18)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "LotList"
        '
        'DG_LotList
        '
        Me.DG_LotList.AllowUserToAddRows = False
        Me.DG_LotList.AllowUserToDeleteRows = False
        Me.DG_LotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_LotList.Location = New System.Drawing.Point(0, 35)
        Me.DG_LotList.Margin = New System.Windows.Forms.Padding(4)
        Me.DG_LotList.Name = "DG_LotList"
        Me.DG_LotList.ReadOnly = True
        Me.DG_LotList.Size = New System.Drawing.Size(100, 69)
        Me.DG_LotList.TabIndex = 22
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.LB_SW_Wers)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.Lebel_StationLine)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.LB_SelectLine)
        Me.GroupBox3.Controls.Add(Me.LabelAppName)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.LB_CurrentStep)
        Me.GroupBox3.Controls.Add(Me.Label_StationName)
        Me.GroupBox3.Location = New System.Drawing.Point(905, 13)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Size = New System.Drawing.Size(287, 270)
        Me.GroupBox3.TabIndex = 42
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Информация о рабочей станции"
        '
        'Lebel_StationLine
        '
        Me.Lebel_StationLine.AutoSize = True
        Me.Lebel_StationLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Lebel_StationLine.Location = New System.Drawing.Point(95, 214)
        Me.Lebel_StationLine.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Lebel_StationLine.Name = "Lebel_StationLine"
        Me.Lebel_StationLine.Size = New System.Drawing.Size(43, 20)
        Me.Lebel_StationLine.TabIndex = 30
        Me.Lebel_StationLine.Text = "Line"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 74)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(176, 16)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Название приложения"
        '
        'LB_SelectLine
        '
        Me.LB_SelectLine.AutoSize = True
        Me.LB_SelectLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_SelectLine.Location = New System.Drawing.Point(11, 216)
        Me.LB_SelectLine.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LB_SelectLine.Name = "LB_SelectLine"
        Me.LB_SelectLine.Size = New System.Drawing.Size(57, 16)
        Me.LB_SelectLine.TabIndex = 0
        Me.LB_SelectLine.Text = "Линия:"
        '
        'LabelAppName
        '
        Me.LabelAppName.AutoSize = True
        Me.LabelAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelAppName.Location = New System.Drawing.Point(7, 90)
        Me.LabelAppName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelAppName.Name = "LabelAppName"
        Me.LabelAppName.Size = New System.Drawing.Size(116, 20)
        Me.LabelAppName.TabIndex = 29
        Me.LabelAppName.Text = "FAS END555"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 157)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(151, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Название станции:"
        '
        'LB_CurrentStep
        '
        Me.LB_CurrentStep.AutoSize = True
        Me.LB_CurrentStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_CurrentStep.Location = New System.Drawing.Point(7, 110)
        Me.LB_CurrentStep.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LB_CurrentStep.Name = "LB_CurrentStep"
        Me.LB_CurrentStep.Size = New System.Drawing.Size(179, 20)
        Me.LB_CurrentStep.TabIndex = 0
        Me.LB_CurrentStep.Text = "Название Операции"
        '
        'Label_StationName
        '
        Me.Label_StationName.AutoSize = True
        Me.Label_StationName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_StationName.Location = New System.Drawing.Point(7, 173)
        Me.Label_StationName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_StationName.Name = "Label_StationName"
        Me.Label_StationName.Size = New System.Drawing.Size(165, 20)
        Me.Label_StationName.TabIndex = 0
        Me.Label_StationName.Text = "Название станции"
        '
        'DG_LOTListPresent
        '
        Me.DG_LOTListPresent.AllowUserToAddRows = False
        Me.DG_LOTListPresent.AllowUserToDeleteRows = False
        Me.DG_LOTListPresent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_LOTListPresent.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DG_LOTListPresent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_LOTListPresent.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FullLOTCode, Me.LOT, Me.ModelName, Me.LOT_ID})
        Me.DG_LOTListPresent.Location = New System.Drawing.Point(13, 13)
        Me.DG_LOTListPresent.Margin = New System.Windows.Forms.Padding(4)
        Me.DG_LOTListPresent.Name = "DG_LOTListPresent"
        Me.DG_LOTListPresent.ReadOnly = True
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_LOTListPresent.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.DG_LOTListPresent.Size = New System.Drawing.Size(812, 218)
        Me.DG_LOTListPresent.TabIndex = 45
        '
        'FullLOTCode
        '
        Me.FullLOTCode.HeaderText = "Спецификация"
        Me.FullLOTCode.Name = "FullLOTCode"
        Me.FullLOTCode.ReadOnly = True
        Me.FullLOTCode.Width = 143
        '
        'LOT
        '
        Me.LOT.HeaderText = "ЛОТ"
        Me.LOT.Name = "LOT"
        Me.LOT.ReadOnly = True
        Me.LOT.Width = 64
        '
        'ModelName
        '
        Me.ModelName.HeaderText = "Модель"
        Me.ModelName.Name = "ModelName"
        Me.ModelName.ReadOnly = True
        Me.ModelName.Width = 89
        '
        'LOT_ID
        '
        Me.LOT_ID.HeaderText = "LOT_ID"
        Me.LOT_ID.Name = "LOT_ID"
        Me.LOT_ID.ReadOnly = True
        Me.LOT_ID.Visible = False
        Me.LOT_ID.Width = 85
        '
        'L_Result
        '
        Me.L_Result.AutoSize = True
        Me.L_Result.Location = New System.Drawing.Point(1352, 9)
        Me.L_Result.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.L_Result.Name = "L_Result"
        Me.L_Result.Size = New System.Drawing.Size(49, 13)
        Me.L_Result.TabIndex = 43
        Me.L_Result.Text = "L_Result"
        Me.L_Result.Visible = False
        '
        'GB_SelectLine
        '
        Me.GB_SelectLine.Controls.Add(Me.GB_LineSettings)
        Me.GB_SelectLine.Controls.Add(Me.DG_AppList)
        Me.GB_SelectLine.Controls.Add(Me.Label9)
        Me.GB_SelectLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_SelectLine.Location = New System.Drawing.Point(1, 299)
        Me.GB_SelectLine.Name = "GB_SelectLine"
        Me.GB_SelectLine.Size = New System.Drawing.Size(881, 530)
        Me.GB_SelectLine.TabIndex = 48
        Me.GB_SelectLine.TabStop = False
        Me.GB_SelectLine.Text = "Выбор номера линии"
        Me.GB_SelectLine.Visible = False
        '
        'GB_LineSettings
        '
        Me.GB_LineSettings.Controls.Add(Me.BT_CloseLineSet)
        Me.GB_LineSettings.Controls.Add(Me.Label5)
        Me.GB_LineSettings.Controls.Add(Me.CB_Steps)
        Me.GB_LineSettings.Controls.Add(Me.Label7)
        Me.GB_LineSettings.Controls.Add(Me.BT_SaveLine)
        Me.GB_LineSettings.Controls.Add(Me.CB_Line)
        Me.GB_LineSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.GB_LineSettings.Location = New System.Drawing.Point(6, 43)
        Me.GB_LineSettings.Name = "GB_LineSettings"
        Me.GB_LineSettings.Size = New System.Drawing.Size(333, 270)
        Me.GB_LineSettings.TabIndex = 35
        Me.GB_LineSettings.TabStop = False
        '
        'BT_CloseLineSet
        '
        Me.BT_CloseLineSet.FlatAppearance.BorderSize = 0
        Me.BT_CloseLineSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_CloseLineSet.Image = Global.Scanning_Station.My.Resources.Resources.icons8_стрелка_влево_в_круге_2_64
        Me.BT_CloseLineSet.Location = New System.Drawing.Point(12, 195)
        Me.BT_CloseLineSet.Name = "BT_CloseLineSet"
        Me.BT_CloseLineSet.Size = New System.Drawing.Size(75, 64)
        Me.BT_CloseLineSet.TabIndex = 20
        Me.BT_CloseLineSet.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(294, 31)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Выберите Операцию"
        '
        'CB_Steps
        '
        Me.CB_Steps.FormattingEnabled = True
        Me.CB_Steps.Location = New System.Drawing.Point(12, 142)
        Me.CB_Steps.Name = "CB_Steps"
        Me.CB_Steps.Size = New System.Drawing.Size(288, 39)
        Me.CB_Steps.TabIndex = 19
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 21)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(243, 31)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Выберите Линию"
        '
        'BT_SaveLine
        '
        Me.BT_SaveLine.FlatAppearance.BorderSize = 0
        Me.BT_SaveLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_SaveLine.Image = Global.Scanning_Station.My.Resources.Resources._04
        Me.BT_SaveLine.Location = New System.Drawing.Point(246, 200)
        Me.BT_SaveLine.Name = "BT_SaveLine"
        Me.BT_SaveLine.Size = New System.Drawing.Size(54, 55)
        Me.BT_SaveLine.TabIndex = 17
        Me.BT_SaveLine.UseVisualStyleBackColor = True
        '
        'CB_Line
        '
        Me.CB_Line.FormattingEnabled = True
        Me.CB_Line.Location = New System.Drawing.Point(12, 55)
        Me.CB_Line.Name = "CB_Line"
        Me.CB_Line.Size = New System.Drawing.Size(288, 39)
        Me.CB_Line.TabIndex = 16
        '
        'DG_AppList
        '
        Me.DG_AppList.AllowUserToAddRows = False
        Me.DG_AppList.AllowUserToDeleteRows = False
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_AppList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DG_AppList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DG_AppList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DG_AppList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_AppList.Location = New System.Drawing.Point(6, 385)
        Me.DG_AppList.Name = "DG_AppList"
        Me.DG_AppList.ReadOnly = True
        Me.DG_AppList.Size = New System.Drawing.Size(846, 117)
        Me.DG_AppList.TabIndex = 34
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 335)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(132, 37)
        Me.Label9.TabIndex = 33
        Me.Label9.Text = "AppList"
        '
        'BT_SelectLot
        '
        Me.BT_SelectLot.FlatAppearance.BorderSize = 0
        Me.BT_SelectLot.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_SelectLot.Image = Global.Scanning_Station.My.Resources.Resources.play
        Me.BT_SelectLot.Location = New System.Drawing.Point(994, 281)
        Me.BT_SelectLot.Name = "BT_SelectLot"
        Me.BT_SelectLot.Size = New System.Drawing.Size(94, 79)
        Me.BT_SelectLot.TabIndex = 47
        Me.BT_SelectLot.UseVisualStyleBackColor = True
        '
        'BT_RefreshLOT
        '
        Me.BT_RefreshLOT.FlatAppearance.BorderSize = 0
        Me.BT_RefreshLOT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_RefreshLOT.Image = Global.Scanning_Station.My.Resources.Resources.refresh__2_
        Me.BT_RefreshLOT.Location = New System.Drawing.Point(832, 172)
        Me.BT_RefreshLOT.Name = "BT_RefreshLOT"
        Me.BT_RefreshLOT.Size = New System.Drawing.Size(61, 59)
        Me.BT_RefreshLOT.TabIndex = 46
        Me.BT_RefreshLOT.UseVisualStyleBackColor = True
        '
        'LB_SW_Wers
        '
        Me.LB_SW_Wers.AutoSize = True
        Me.LB_SW_Wers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LB_SW_Wers.Location = New System.Drawing.Point(165, 22)
        Me.LB_SW_Wers.Name = "LB_SW_Wers"
        Me.LB_SW_Wers.Size = New System.Drawing.Size(76, 16)
        Me.LB_SW_Wers.TabIndex = 34
        Me.LB_SW_Wers.Text = "SW_Wers"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label15.Location = New System.Drawing.Point(8, 22)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(159, 16)
        Me.Label15.TabIndex = 33
        Me.Label15.Text = "Версия приложения:"
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1745, 789)
        Me.Controls.Add(Me.GB_SelectLine)
        Me.Controls.Add(Me.BT_SelectLot)
        Me.Controls.Add(Me.BT_RefreshLOT)
        Me.Controls.Add(Me.GB_NotVisibleElements)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.DG_LOTListPresent)
        Me.Controls.Add(Me.L_Result)
        Me.Name = "SettingsForm"
        Me.Text = "Настройки программы"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GB_NotVisibleElements.ResumeLayout(False)
        Me.GB_NotVisibleElements.PerformLayout()
        CType(Me.DG_Steps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_LineList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_LotList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.DG_LOTListPresent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_SelectLine.ResumeLayout(False)
        Me.GB_SelectLine.PerformLayout()
        Me.GB_LineSettings.ResumeLayout(False)
        Me.GB_LineSettings.PerformLayout()
        CType(Me.DG_AppList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GB_NotVisibleElements As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents DG_LotList As DataGridView
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Lebel_StationLine As Label
    Friend WithEvents LB_SelectLine As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents LabelAppName As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label_StationName As Label
    Friend WithEvents DG_LOTListPresent As DataGridView
    Friend WithEvents L_Result As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents FullLOTCode As DataGridViewTextBoxColumn
    Friend WithEvents LOT As DataGridViewTextBoxColumn
    Friend WithEvents ModelName As DataGridViewTextBoxColumn
    Friend WithEvents LOT_ID As DataGridViewTextBoxColumn
    Friend WithEvents BT_RefreshLOT As Button
    Friend WithEvents BT_SelectLot As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents GB_SelectLine As GroupBox
    Friend WithEvents GB_LineSettings As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents BT_SaveLine As Button
    Friend WithEvents CB_Line As ComboBox
    Friend WithEvents DG_AppList As DataGridView
    Friend WithEvents Label9 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents CB_Steps As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents DG_LineList As DataGridView
    Friend WithEvents Label10 As Label
    Friend WithEvents DG_Steps As DataGridView
    Friend WithEvents BT_CloseLineSet As Button
    Friend WithEvents LB_CurrentStep As Label
    Friend WithEvents LB_SW_Wers As Label
    Friend WithEvents Label15 As Label
End Class
