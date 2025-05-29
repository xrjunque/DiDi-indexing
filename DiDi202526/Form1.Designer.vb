<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmDiDi
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.BtnMode = New System.Windows.Forms.Button()
        Me.BtnGenerate = New System.Windows.Forms.Button()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.LblNumOfKeys = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.LblList = New System.Windows.Forms.Label()
        Me.CBox_Mode = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgv = New System.Windows.Forms.DataGridView()
        Me.BtnRemoveAll = New System.Windows.Forms.Button()
        Me.ChkUnicity = New System.Windows.Forms.CheckBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripCpySelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripCpyToList = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnClearList = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnMode
        '
        Me.BtnMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnMode.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMode.Location = New System.Drawing.Point(57, 291)
        Me.BtnMode.Name = "BtnMode"
        Me.BtnMode.Size = New System.Drawing.Size(200, 27)
        Me.BtnMode.TabIndex = 3
        Me.BtnMode.Text = "Add/Remove/Find key list"
        Me.BtnMode.UseVisualStyleBackColor = True
        '
        'BtnGenerate
        '
        Me.BtnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnGenerate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGenerate.Location = New System.Drawing.Point(55, 383)
        Me.BtnGenerate.Name = "BtnGenerate"
        Me.BtnGenerate.Size = New System.Drawing.Size(75, 26)
        Me.BtnGenerate.TabIndex = 8
        Me.BtnGenerate.Text = "Generate"
        Me.BtnGenerate.UseVisualStyleBackColor = True
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDown1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NumericUpDown1.Location = New System.Drawing.Point(172, 334)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {3000000, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(93, 25)
        Me.NumericUpDown1.TabIndex = 6
        Me.NumericUpDown1.ThousandsSeparator = True
        Me.NumericUpDown1.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'LblNumOfKeys
        '
        Me.LblNumOfKeys.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblNumOfKeys.AutoSize = True
        Me.LblNumOfKeys.BackColor = System.Drawing.Color.Transparent
        Me.LblNumOfKeys.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNumOfKeys.Location = New System.Drawing.Point(37, 335)
        Me.LblNumOfKeys.Name = "LblNumOfKeys"
        Me.LblNumOfKeys.Size = New System.Drawing.Size(132, 17)
        Me.LblNumOfKeys.TabIndex = 5
        Me.LblNumOfKeys.Text = "#of keys to generate:"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 428)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(696, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(64, 17)
        Me.ToolStripStatusLabel1.Text = "Message:"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(515, 17)
        Me.ToolStripStatusLabel2.Spring = True
        Me.ToolStripStatusLabel2.Text = " "
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'LblList
        '
        Me.LblList.AutoSize = True
        Me.LblList.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblList.Location = New System.Drawing.Point(16, 15)
        Me.LblList.Name = "LblList"
        Me.LblList.Size = New System.Drawing.Size(27, 17)
        Me.LblList.TabIndex = 0
        Me.LblList.Text = "List"
        '
        'CBox_Mode
        '
        Me.CBox_Mode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBox_Mode.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CBox_Mode.FormattingEnabled = True
        Me.CBox_Mode.Items.AddRange(New Object() {"Add list to Index", "Remove list from index", "Find list at Index"})
        Me.CBox_Mode.Location = New System.Drawing.Point(57, 264)
        Me.CBox_Mode.Name = "CBox_Mode"
        Me.CBox_Mode.Size = New System.Drawing.Size(200, 25)
        Me.CBox_Mode.TabIndex = 2
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.Location = New System.Drawing.Point(58, 324)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(207, 2)
        Me.Panel1.TabIndex = 4
        '
        'dgv
        '
        Me.dgv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgv.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(162, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(216, Byte), Integer))
        Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv.Location = New System.Drawing.Point(3, 40)
        Me.dgv.Name = "dgv"
        Me.dgv.Size = New System.Drawing.Size(313, 218)
        Me.dgv.TabIndex = 1
        '
        'BtnRemoveAll
        '
        Me.BtnRemoveAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnRemoveAll.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRemoveAll.Location = New System.Drawing.Point(159, 383)
        Me.BtnRemoveAll.Name = "BtnRemoveAll"
        Me.BtnRemoveAll.Size = New System.Drawing.Size(106, 26)
        Me.BtnRemoveAll.TabIndex = 9
        Me.BtnRemoveAll.Text = "Remove All"
        Me.BtnRemoveAll.UseVisualStyleBackColor = True
        '
        'ChkUnicity
        '
        Me.ChkUnicity.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkUnicity.AutoSize = True
        Me.ChkUnicity.BackColor = System.Drawing.Color.Transparent
        Me.ChkUnicity.Checked = True
        Me.ChkUnicity.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkUnicity.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkUnicity.Location = New System.Drawing.Point(92, 360)
        Me.ChkUnicity.Name = "ChkUnicity"
        Me.ChkUnicity.Size = New System.Drawing.Size(113, 21)
        Me.ChkUnicity.TabIndex = 7
        Me.ChkUnicity.Text = "Append unicity"
        Me.ChkUnicity.UseVisualStyleBackColor = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripCpySelected, Me.ToolStripCpyToList})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(185, 48)
        '
        'ToolStripCpySelected
        '
        Me.ToolStripCpySelected.Name = "ToolStripCpySelected"
        Me.ToolStripCpySelected.Size = New System.Drawing.Size(184, 22)
        Me.ToolStripCpySelected.Text = "Copy Selection"
        '
        'ToolStripCpyToList
        '
        Me.ToolStripCpyToList.Name = "ToolStripCpyToList"
        Me.ToolStripCpyToList.Size = New System.Drawing.Size(184, 22)
        Me.ToolStripCpyToList.Text = "Copy selection to list"
        '
        'BtnClearList
        '
        Me.BtnClearList.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClearList.Location = New System.Drawing.Point(198, 8)
        Me.BtnClearList.Name = "BtnClearList"
        Me.BtnClearList.Size = New System.Drawing.Size(107, 25)
        Me.BtnClearList.TabIndex = 10
        Me.BtnClearList.Text = "Clear list"
        Me.BtnClearList.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.Color.SteelBlue
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 12)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgv)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CBox_Mode)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ChkUnicity)
        Me.SplitContainer1.Panel1.Controls.Add(Me.BtnClearList)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LblList)
        Me.SplitContainer1.Panel1.Controls.Add(Me.BtnMode)
        Me.SplitContainer1.Panel1.Controls.Add(Me.BtnGenerate)
        Me.SplitContainer1.Panel1.Controls.Add(Me.BtnRemoveAll)
        Me.SplitContainer1.Panel1.Controls.Add(Me.NumericUpDown1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LblNumOfKeys)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Size = New System.Drawing.Size(696, 413)
        Me.SplitContainer1.SplitterDistance = 327
        Me.SplitContainer1.SplitterWidth = 8
        Me.SplitContainer1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(270, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select text && mouse right click to copy to List"
        '
        'FrmDiDi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(696, 450)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.MinimumSize = New System.Drawing.Size(690, 489)
        Me.Name = "FrmDiDi"
        Me.Text = "Differential directory -- DiDi"
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnMode As Button
    Friend WithEvents BtnGenerate As Button
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents LblNumOfKeys As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents LblList As Label
    Friend WithEvents CBox_Mode As ComboBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents dgv As DataGridView
    Friend WithEvents BtnRemoveAll As Button
    Friend WithEvents ChkUnicity As CheckBox
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ToolStripCpySelected As ToolStripMenuItem
    Friend WithEvents ToolStripCpyToList As ToolStripMenuItem
    Friend WithEvents BtnClearList As Button
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Label1 As Label
End Class
