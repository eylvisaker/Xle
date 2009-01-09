<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmStartup
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
	Public WithEvents cmdImport As System.Windows.Forms.Button
	Public cmdDialogOpen As System.Windows.Forms.OpenFileDialog
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdOpenMap As System.Windows.Forms.Button
	Public WithEvents cmdCreateMap As System.Windows.Forms.Button
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmStartup))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdImport = New System.Windows.Forms.Button
		Me.cmdDialogOpen = New System.Windows.Forms.OpenFileDialog
		Me.cmdExit = New System.Windows.Forms.Button
		Me.cmdOpenMap = New System.Windows.Forms.Button
		Me.cmdCreateMap = New System.Windows.Forms.Button
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Text = "Legacy of the Ancients Map Editor"
		Me.ClientSize = New System.Drawing.Size(351, 143)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.Icon = CType(resources.GetObject("frmStartup.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmStartup"
		Me.cmdImport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdImport.Text = "&Import Existing Map"
		Me.cmdImport.Size = New System.Drawing.Size(81, 33)
		Me.cmdImport.Location = New System.Drawing.Point(224, 64)
		Me.cmdImport.TabIndex = 3
		Me.cmdImport.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
		Me.cmdImport.CausesValidation = True
		Me.cmdImport.Enabled = True
		Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdImport.TabStop = True
		Me.cmdImport.Name = "cmdImport"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdExit
		Me.cmdExit.Text = "E&xit"
		Me.cmdExit.Size = New System.Drawing.Size(57, 17)
		Me.cmdExit.Location = New System.Drawing.Point(144, 112)
		Me.cmdExit.TabIndex = 2
		Me.cmdExit.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Enabled = True
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.TabStop = True
		Me.cmdExit.Name = "cmdExit"
		Me.cmdOpenMap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOpenMap.Text = "&Open Existing Map"
		Me.cmdOpenMap.Size = New System.Drawing.Size(89, 33)
		Me.cmdOpenMap.Location = New System.Drawing.Point(128, 64)
		Me.cmdOpenMap.TabIndex = 1
		Me.cmdOpenMap.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOpenMap.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOpenMap.CausesValidation = True
		Me.cmdOpenMap.Enabled = True
		Me.cmdOpenMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOpenMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOpenMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOpenMap.TabStop = True
		Me.cmdOpenMap.Name = "cmdOpenMap"
		Me.cmdCreateMap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCreateMap.Text = "Create &New Map"
		Me.cmdCreateMap.Size = New System.Drawing.Size(81, 33)
		Me.cmdCreateMap.Location = New System.Drawing.Point(40, 64)
		Me.cmdCreateMap.TabIndex = 0
		Me.cmdCreateMap.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdCreateMap.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCreateMap.CausesValidation = True
		Me.cmdCreateMap.Enabled = True
		Me.cmdCreateMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCreateMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCreateMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCreateMap.TabStop = True
		Me.cmdCreateMap.Name = "cmdCreateMap"
		Me.Image1.Size = New System.Drawing.Size(192, 32)
		Me.Image1.Location = New System.Drawing.Point(80, 16)
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Enabled = True
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Name = "Image1"
		Me.Controls.Add(cmdImport)
		Me.Controls.Add(cmdExit)
		Me.Controls.Add(cmdOpenMap)
		Me.Controls.Add(cmdCreateMap)
		Me.Controls.Add(Image1)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class