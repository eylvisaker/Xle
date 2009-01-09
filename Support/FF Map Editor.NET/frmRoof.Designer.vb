<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmRoof
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdCopy As System.Windows.Forms.Button
	Public WithEvents chkDrawGround As System.Windows.Forms.CheckBox
	Public WithEvents txtTargetY As System.Windows.Forms.TextBox
	Public WithEvents txtTargetX As System.Windows.Forms.TextBox
	Public WithEvents txtAnchorY As System.Windows.Forms.TextBox
	Public WithEvents txtAnchorX As System.Windows.Forms.TextBox
	Public WithEvents hsbRoofIndex As System.Windows.Forms.HScrollBar
	Public WithEvents txtRoofY As System.Windows.Forms.TextBox
	Public WithEvents txtRoofX As System.Windows.Forms.TextBox
	Public WithEvents cmdDone As System.Windows.Forms.Button
	Public WithEvents Picture2 As System.Windows.Forms.PictureBox
	Public WithEvents Picture1 As System.Windows.Forms.PictureBox
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents lblCurrentTile As System.Windows.Forms.Label
	Public WithEvents lblTile As System.Windows.Forms.Label
	Public WithEvents lblY As System.Windows.Forms.Label
	Public WithEvents lblx As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents lblEditing As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCopy = New System.Windows.Forms.Button
        Me.chkDrawGround = New System.Windows.Forms.CheckBox
        Me.txtTargetY = New System.Windows.Forms.TextBox
        Me.txtTargetX = New System.Windows.Forms.TextBox
        Me.txtAnchorY = New System.Windows.Forms.TextBox
        Me.txtAnchorX = New System.Windows.Forms.TextBox
        Me.hsbRoofIndex = New System.Windows.Forms.HScrollBar
        Me.txtRoofY = New System.Windows.Forms.TextBox
        Me.txtRoofX = New System.Windows.Forms.TextBox
        Me.cmdDone = New System.Windows.Forms.Button
        Me.Picture2 = New System.Windows.Forms.PictureBox
        Me.Picture1 = New System.Windows.Forms.PictureBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblCurrentTile = New System.Windows.Forms.Label
        Me.lblTile = New System.Windows.Forms.Label
        Me.lblY = New System.Windows.Forms.Label
        Me.lblx = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblEditing = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.sbDown = New System.Windows.Forms.VScrollBar
        Me.sbRight = New System.Windows.Forms.HScrollBar
        CType(Me.Picture2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopy.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopy.Location = New System.Drawing.Point(296, 448)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopy.Size = New System.Drawing.Size(89, 57)
        Me.cmdCopy.TabIndex = 22
        Me.cmdCopy.Text = "Copy From Ground"
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'chkDrawGround
        '
        Me.chkDrawGround.BackColor = System.Drawing.SystemColors.Control
        Me.chkDrawGround.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDrawGround.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDrawGround.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDrawGround.Location = New System.Drawing.Point(296, 400)
        Me.chkDrawGround.Name = "chkDrawGround"
        Me.chkDrawGround.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDrawGround.Size = New System.Drawing.Size(209, 33)
        Me.chkDrawGround.TabIndex = 21
        Me.chkDrawGround.Text = "Draw Ground"
        Me.chkDrawGround.UseVisualStyleBackColor = False
        '
        'txtTargetY
        '
        Me.txtTargetY.AcceptsReturn = True
        Me.txtTargetY.BackColor = System.Drawing.SystemColors.Window
        Me.txtTargetY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTargetY.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTargetY.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTargetY.Location = New System.Drawing.Point(440, 208)
        Me.txtTargetY.MaxLength = 0
        Me.txtTargetY.Name = "txtTargetY"
        Me.txtTargetY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTargetY.Size = New System.Drawing.Size(89, 27)
        Me.txtTargetY.TabIndex = 20
        Me.txtTargetY.Text = "Text2"
        '
        'txtTargetX
        '
        Me.txtTargetX.AcceptsReturn = True
        Me.txtTargetX.BackColor = System.Drawing.SystemColors.Window
        Me.txtTargetX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTargetX.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTargetX.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTargetX.Location = New System.Drawing.Point(336, 208)
        Me.txtTargetX.MaxLength = 0
        Me.txtTargetX.Name = "txtTargetX"
        Me.txtTargetX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTargetX.Size = New System.Drawing.Size(89, 27)
        Me.txtTargetX.TabIndex = 19
        Me.txtTargetX.Text = "Text1"
        '
        'txtAnchorY
        '
        Me.txtAnchorY.AcceptsReturn = True
        Me.txtAnchorY.BackColor = System.Drawing.SystemColors.Window
        Me.txtAnchorY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAnchorY.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnchorY.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAnchorY.Location = New System.Drawing.Point(440, 152)
        Me.txtAnchorY.MaxLength = 0
        Me.txtAnchorY.Name = "txtAnchorY"
        Me.txtAnchorY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAnchorY.Size = New System.Drawing.Size(89, 27)
        Me.txtAnchorY.TabIndex = 12
        Me.txtAnchorY.Text = "Text2"
        '
        'txtAnchorX
        '
        Me.txtAnchorX.AcceptsReturn = True
        Me.txtAnchorX.BackColor = System.Drawing.SystemColors.Window
        Me.txtAnchorX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAnchorX.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnchorX.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAnchorX.Location = New System.Drawing.Point(336, 152)
        Me.txtAnchorX.MaxLength = 0
        Me.txtAnchorX.Name = "txtAnchorX"
        Me.txtAnchorX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAnchorX.Size = New System.Drawing.Size(81, 27)
        Me.txtAnchorX.TabIndex = 11
        Me.txtAnchorX.Text = "Text1"
        '
        'hsbRoofIndex
        '
        Me.hsbRoofIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.hsbRoofIndex.LargeChange = 5
        Me.hsbRoofIndex.Location = New System.Drawing.Point(336, 32)
        Me.hsbRoofIndex.Maximum = 44
        Me.hsbRoofIndex.Minimum = 1
        Me.hsbRoofIndex.Name = "hsbRoofIndex"
        Me.hsbRoofIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.hsbRoofIndex.Size = New System.Drawing.Size(193, 17)
        Me.hsbRoofIndex.TabIndex = 9
        Me.hsbRoofIndex.TabStop = True
        Me.hsbRoofIndex.Value = 1
        '
        'txtRoofY
        '
        Me.txtRoofY.AcceptsReturn = True
        Me.txtRoofY.BackColor = System.Drawing.SystemColors.Window
        Me.txtRoofY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRoofY.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRoofY.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRoofY.Location = New System.Drawing.Point(440, 88)
        Me.txtRoofY.MaxLength = 0
        Me.txtRoofY.Name = "txtRoofY"
        Me.txtRoofY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRoofY.Size = New System.Drawing.Size(81, 33)
        Me.txtRoofY.TabIndex = 6
        Me.txtRoofY.Text = "Text2"
        '
        'txtRoofX
        '
        Me.txtRoofX.AcceptsReturn = True
        Me.txtRoofX.BackColor = System.Drawing.SystemColors.Window
        Me.txtRoofX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRoofX.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRoofX.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRoofX.Location = New System.Drawing.Point(336, 88)
        Me.txtRoofX.MaxLength = 0
        Me.txtRoofX.Name = "txtRoofX"
        Me.txtRoofX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRoofX.Size = New System.Drawing.Size(81, 33)
        Me.txtRoofX.TabIndex = 5
        Me.txtRoofX.Text = "txtRoofX"
        '
        'cmdDone
        '
        Me.cmdDone.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDone.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDone.Location = New System.Drawing.Point(448, 568)
        Me.cmdDone.Name = "cmdDone"
        Me.cmdDone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDone.Size = New System.Drawing.Size(81, 33)
        Me.cmdDone.TabIndex = 4
        Me.cmdDone.Text = "Done"
        Me.cmdDone.UseVisualStyleBackColor = False
        '
        'Picture2
        '
        Me.Picture2.BackColor = System.Drawing.SystemColors.Control
        Me.Picture2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Picture2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Picture2.Location = New System.Drawing.Point(16, 352)
        Me.Picture2.Name = "Picture2"
        Me.Picture2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Picture2.Size = New System.Drawing.Size(256, 256)
        Me.Picture2.TabIndex = 1
        '
        'Picture1
        '
        Me.Picture1.BackColor = System.Drawing.SystemColors.Control
        Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Picture1.Location = New System.Drawing.Point(0, 0)
        Me.Picture1.Name = "Picture1"
        Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Picture1.Size = New System.Drawing.Size(303, 311)
        Me.Picture1.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(336, 184)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(121, 25)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Anchor Target"
        '
        'lblCurrentTile
        '
        Me.lblCurrentTile.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentTile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentTile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentTile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentTile.Location = New System.Drawing.Point(344, 352)
        Me.lblCurrentTile.Name = "lblCurrentTile"
        Me.lblCurrentTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentTile.Size = New System.Drawing.Size(200, 25)
        Me.lblCurrentTile.TabIndex = 17
        Me.lblCurrentTile.Text = "Label6"
        '
        'lblTile
        '
        Me.lblTile.BackColor = System.Drawing.SystemColors.Control
        Me.lblTile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTile.Location = New System.Drawing.Point(344, 328)
        Me.lblTile.Name = "lblTile"
        Me.lblTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTile.Size = New System.Drawing.Size(200, 25)
        Me.lblTile.TabIndex = 16
        Me.lblTile.Text = "Label6"
        '
        'lblY
        '
        Me.lblY.BackColor = System.Drawing.SystemColors.Control
        Me.lblY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblY.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblY.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblY.Location = New System.Drawing.Point(344, 304)
        Me.lblY.Name = "lblY"
        Me.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblY.Size = New System.Drawing.Size(200, 25)
        Me.lblY.TabIndex = 15
        Me.lblY.Text = "Label6"
        '
        'lblx
        '
        Me.lblx.BackColor = System.Drawing.SystemColors.Control
        Me.lblx.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblx.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblx.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblx.Location = New System.Drawing.Point(344, 280)
        Me.lblx.Name = "lblx"
        Me.lblx.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblx.Size = New System.Drawing.Size(200, 17)
        Me.lblx.TabIndex = 14
        Me.lblx.Text = "X:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(336, 248)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(137, 25)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Current Position:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(336, 128)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(89, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Anchor Point"
        '
        'lblEditing
        '
        Me.lblEditing.BackColor = System.Drawing.SystemColors.Control
        Me.lblEditing.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEditing.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEditing.Location = New System.Drawing.Point(336, 8)
        Me.lblEditing.Name = "lblEditing"
        Me.lblEditing.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEditing.Size = New System.Drawing.Size(201, 25)
        Me.lblEditing.TabIndex = 8
        Me.lblEditing.Text = "Editing Roof"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(336, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(145, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Roof Dimensions"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(0, 352)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(17, 257)
        Me.Label5.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(16, 336)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(281, 17)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = " 0    1   2   3    4   5   6    7   8   9   A    B   C   D   E   F"
        '
        'sbDown
        '
        Me.sbDown.Location = New System.Drawing.Point(304, 0)
        Me.sbDown.Name = "sbDown"
        Me.sbDown.Size = New System.Drawing.Size(17, 311)
        Me.sbDown.TabIndex = 25
        '
        'sbRight
        '
        Me.sbRight.Location = New System.Drawing.Point(0, 312)
        Me.sbRight.Name = "sbRight"
        Me.sbRight.Size = New System.Drawing.Size(303, 17)
        Me.sbRight.TabIndex = 26
        '
        'frmRoof
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(544, 614)
        Me.Controls.Add(Me.sbRight)
        Me.Controls.Add(Me.sbDown)
        Me.Controls.Add(Me.cmdCopy)
        Me.Controls.Add(Me.chkDrawGround)
        Me.Controls.Add(Me.txtTargetY)
        Me.Controls.Add(Me.txtTargetX)
        Me.Controls.Add(Me.txtAnchorY)
        Me.Controls.Add(Me.txtAnchorX)
        Me.Controls.Add(Me.hsbRoofIndex)
        Me.Controls.Add(Me.txtRoofY)
        Me.Controls.Add(Me.txtRoofX)
        Me.Controls.Add(Me.cmdDone)
        Me.Controls.Add(Me.Picture2)
        Me.Controls.Add(Me.Picture1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblCurrentTile)
        Me.Controls.Add(Me.lblTile)
        Me.Controls.Add(Me.lblY)
        Me.Controls.Add(Me.lblx)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblEditing)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmRoof"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Edit Roof"
        CType(Me.Picture2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sbDown As System.Windows.Forms.VScrollBar
    Friend WithEvents sbRight As System.Windows.Forms.HScrollBar
#End Region 
End Class