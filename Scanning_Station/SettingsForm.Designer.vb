<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GB_NotVisibleElements = New System.Windows.Forms.GroupBox()
        Me.LineList = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label_IDApp = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LabelStationID = New System.Windows.Forms.Label()
        Me.DG_LineList = New System.Windows.Forms.DataGridView()
        Me.DG_LotList = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Lebel_StationLine = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BT_LineSettings = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LabelAppName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label_StationName = New System.Windows.Forms.Label()
        Me.DG_LOTListPresent = New System.Windows.Forms.DataGridView()
        Me.FullLOTCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ModelName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LOT_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.L_Result = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.GB_NotVisibleElements.SuspendLayout()
        CType(Me.DG_LineList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DG_LotList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DG_LOTListPresent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GB_NotVisibleElements
        '
        Me.GB_NotVisibleElements.Controls.Add(Me.LineList)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label4)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label6)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label_IDApp)
        Me.GB_NotVisibleElements.Controls.Add(Me.Label5)
        Me.GB_NotVisibleElements.Controls.Add(Me.LabelStationID)
        Me.GB_NotVisibleElements.Controls.Add(Me.DG_LineList)
        Me.GB_NotVisibleElements.Controls.Add(Me.DG_LotList)
        Me.GB_NotVisibleElements.Location = New System.Drawing.Point(1337, 35)
        Me.GB_NotVisibleElements.Margin = New System.Windows.Forms.Padding(4)
        Me.GB_NotVisibleElements.Name = "GB_NotVisibleElements"
        Me.GB_NotVisibleElements.Padding = New System.Windows.Forms.Padding(4)
        Me.GB_NotVisibleElements.Size = New System.Drawing.Size(379, 379)
        Me.GB_NotVisibleElements.TabIndex = 44
        Me.GB_NotVisibleElements.TabStop = False
        Me.GB_NotVisibleElements.Text = "Неотображаемые элементы"
        '
        'LineList
        '
        Me.LineList.AutoSize = True
        Me.LineList.Location = New System.Drawing.Point(35, 25)
        Me.LineList.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LineList.Name = "LineList"
        Me.LineList.Size = New System.Drawing.Size(43, 13)
        Me.LineList.TabIndex = 36
        Me.LineList.Text = "LineList"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.Location = New System.Drawing.Point(196, 108)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 20)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "ID_App"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(53, 176)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "LotList"
        '
        'Label_IDApp
        '
        Me.Label_IDApp.AutoSize = True
        Me.Label_IDApp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_IDApp.Location = New System.Drawing.Point(292, 108)
        Me.Label_IDApp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_IDApp.Name = "Label_IDApp"
        Me.Label_IDApp.Size = New System.Drawing.Size(19, 20)
        Me.Label_IDApp.TabIndex = 32
        Me.Label_IDApp.Text = "4"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.Location = New System.Drawing.Point(29, 110)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(86, 20)
        Me.Label5.TabIndex = 35
        Me.Label5.Text = "StationID"
        '
        'LabelStationID
        '
        Me.LabelStationID.AutoSize = True
        Me.LabelStationID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelStationID.Location = New System.Drawing.Point(152, 108)
        Me.LabelStationID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelStationID.Name = "LabelStationID"
        Me.LabelStationID.Size = New System.Drawing.Size(21, 20)
        Me.LabelStationID.TabIndex = 34
        Me.LabelStationID.Text = "X"
        '
        'DG_LineList
        '
        Me.DG_LineList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_LineList.Location = New System.Drawing.Point(35, 49)
        Me.DG_LineList.Margin = New System.Windows.Forms.Padding(4)
        Me.DG_LineList.Name = "DG_LineList"
        Me.DG_LineList.Size = New System.Drawing.Size(156, 55)
        Me.DG_LineList.TabIndex = 27
        Me.DG_LineList.Visible = False
        '
        'DG_LotList
        '
        Me.DG_LotList.AllowUserToAddRows = False
        Me.DG_LotList.AllowUserToDeleteRows = False
        Me.DG_LotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_LotList.Location = New System.Drawing.Point(35, 196)
        Me.DG_LotList.Margin = New System.Windows.Forms.Padding(4)
        Me.DG_LotList.Name = "DG_LotList"
        Me.DG_LotList.ReadOnly = True
        Me.DG_LotList.Size = New System.Drawing.Size(208, 150)
        Me.DG_LotList.TabIndex = 22
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.GroupBox1)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.LabelAppName)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Label_StationName)
        Me.GroupBox3.Location = New System.Drawing.Point(905, 13)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Size = New System.Drawing.Size(287, 297)
        Me.GroupBox3.TabIndex = 42
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Информация о рабочей станции"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Lebel_StationLine)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.BT_LineSettings)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 178)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(287, 118)
        Me.GroupBox1.TabIndex = 41
        Me.GroupBox1.TabStop = False
        '
        'Lebel_StationLine
        '
        Me.Lebel_StationLine.AutoSize = True
        Me.Lebel_StationLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Lebel_StationLine.Location = New System.Drawing.Point(88, 20)
        Me.Lebel_StationLine.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Lebel_StationLine.Name = "Lebel_StationLine"
        Me.Lebel_StationLine.Size = New System.Drawing.Size(43, 20)
        Me.Lebel_StationLine.TabIndex = 30
        Me.Lebel_StationLine.Text = "Line"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 23)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Линия:"
        '
        'BT_LineSettings
        '
        Me.BT_LineSettings.FlatAppearance.BorderSize = 0
        Me.BT_LineSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BT_LineSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BT_LineSettings.Location = New System.Drawing.Point(181, 32)
        Me.BT_LineSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.BT_LineSettings.Name = "BT_LineSettings"
        Me.BT_LineSettings.Size = New System.Drawing.Size(97, 79)
        Me.BT_LineSettings.TabIndex = 30
        Me.BT_LineSettings.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.BT_LineSettings.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 33)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(176, 16)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Название приложения"
        '
        'LabelAppName
        '
        Me.LabelAppName.AutoSize = True
        Me.LabelAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.LabelAppName.Location = New System.Drawing.Point(3, 53)
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
        Me.Label1.Location = New System.Drawing.Point(5, 96)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(151, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Название станции:"
        '
        'Label_StationName
        '
        Me.Label_StationName.AutoSize = True
        Me.Label_StationName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label_StationName.Location = New System.Drawing.Point(4, 127)
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
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DG_LOTListPresent.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.DG_LOTListPresent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG_LOTListPresent.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FullLOTCode, Me.LOT, Me.ModelName, Me.LOT_ID, Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9})
        Me.DG_LOTListPresent.Location = New System.Drawing.Point(13, 13)
        Me.DG_LOTListPresent.Margin = New System.Windows.Forms.Padding(4)
        Me.DG_LOTListPresent.Name = "DG_LOTListPresent"
        Me.DG_LOTListPresent.ReadOnly = True
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.DG_LOTListPresent.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.DG_LOTListPresent.Size = New System.Drawing.Size(827, 332)
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
        Me.LOT_ID.Width = 85
        '
        'Column1
        '
        Me.Column1.HeaderText = "SMTNumberFormat"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 167
        '
        'Column2
        '
        Me.Column2.HeaderText = "FASNumberFormat"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 164
        '
        'Column3
        '
        Me.Column3.HeaderText = "CheckSMTRange"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 154
        '
        'Column4
        '
        Me.Column4.HeaderText = "CheckFASRange"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 151
        '
        'Column5
        '
        Me.Column5.HeaderText = "SMTStart"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 97
        '
        'Column6
        '
        Me.Column6.HeaderText = "SMTEnd"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 92
        '
        'Column7
        '
        Me.Column7.HeaderText = "FASStart"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Width = 94
        '
        'Column8
        '
        Me.Column8.HeaderText = "FASEnd"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Width = 89
        '
        'Column9
        '
        Me.Column9.HeaderText = "SingleSN"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        Me.Column9.Width = 98
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
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(13, 498)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(479, 167)
        Me.TextBox1.TabIndex = 46
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1745, 789)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.GB_NotVisibleElements)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.DG_LOTListPresent)
        Me.Controls.Add(Me.L_Result)
        Me.Name = "SettingsForm"
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GB_NotVisibleElements.ResumeLayout(False)
        Me.GB_NotVisibleElements.PerformLayout()
        CType(Me.DG_LineList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DG_LotList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DG_LOTListPresent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GB_NotVisibleElements As GroupBox
    Friend WithEvents LineList As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label_IDApp As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents LabelStationID As Label
    Friend WithEvents DG_LineList As DataGridView
    Friend WithEvents DG_LotList As DataGridView
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Lebel_StationLine As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BT_LineSettings As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents LabelAppName As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label_StationName As Label
    Friend WithEvents DG_LOTListPresent As DataGridView
    Friend WithEvents FullLOTCode As DataGridViewTextBoxColumn
    Friend WithEvents LOT As DataGridViewTextBoxColumn
    Friend WithEvents ModelName As DataGridViewTextBoxColumn
    Friend WithEvents LOT_ID As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents Column9 As DataGridViewTextBoxColumn
    Friend WithEvents L_Result As Label
    Friend WithEvents TextBox1 As TextBox
End Class
