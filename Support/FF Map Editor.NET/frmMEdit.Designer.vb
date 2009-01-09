<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMEdit
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
	Public WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
    Public WithEvents cmdFill As System.Windows.Forms.Button
	Public WithEvents Picture2 As System.Windows.Forms.PictureBox
	Public WithEvents Text7 As System.Windows.Forms.TextBox
	Public WithEvents chkRandom As System.Windows.Forms.CheckBox
	Public WithEvents lblSpcCount As System.Windows.Forms.Label
	Public WithEvents lblFindSpecial As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents frmBottom As System.Windows.Forms.Panel
	Public WithEvents cmdPlaceSpecial As System.Windows.Forms.Button
	Public WithEvents cmdObject As System.Windows.Forms.Button
	Public WithEvents cmdGuard As System.Windows.Forms.Button
	Public WithEvents cmdModifySpecial As System.Windows.Forms.Button
	Public WithEvents cmdDeleteSpecial As System.Windows.Forms.Button
	Public WithEvents lstPreDef As System.Windows.Forms.ListBox
	Public WithEvents cmdRoof As System.Windows.Forms.Button
	Public WithEvents chkDrawRoof As System.Windows.Forms.CheckBox
	Public WithEvents chkRestrict As System.Windows.Forms.CheckBox
	Public WithEvents chkDrawGuards As System.Windows.Forms.CheckBox
	Public WithEvents lblImport As System.Windows.Forms.Label
	Public WithEvents lblDim As System.Windows.Forms.Label
	Public WithEvents lblX As System.Windows.Forms.Label
	Public WithEvents lblY As System.Windows.Forms.Label
	Public WithEvents lblTile As System.Windows.Forms.Label
	Public WithEvents lblCurrentTile As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents frmRight As System.Windows.Forms.Panel
    Public cmdDialogOpen As System.Windows.Forms.OpenFileDialog
    Public cmdDialogSave As System.Windows.Forms.SaveFileDialog
    Public WithEvents Picture1 As System.Windows.Forms.PictureBox
    Public WithEvents mnuNew As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuOpen As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuImport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFileSep0 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSaveAs As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFinalize As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep0 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuQuit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuProperties As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep2 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPlaceSpecial As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuModifySpecial As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDeleteSpecial As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSep1 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuRefreshTiles As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuImportRefresh As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuParameters As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuImportSep0 As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuSaveMapping As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuLoadMapping As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTitleImport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMEdit))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkRestrict = New System.Windows.Forms.CheckBox
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip
        Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.frmBottom = New System.Windows.Forms.Panel
        Me.cmdFill = New System.Windows.Forms.Button
        Me.Picture2 = New System.Windows.Forms.PictureBox
        Me.Text7 = New System.Windows.Forms.TextBox
        Me.chkRandom = New System.Windows.Forms.CheckBox
        Me.lblSpcCount = New System.Windows.Forms.Label
        Me.lblFindSpecial = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.frmRight = New System.Windows.Forms.Panel
        Me.cmdPlaceSpecial = New System.Windows.Forms.Button
        Me.cmdObject = New System.Windows.Forms.Button
        Me.cmdGuard = New System.Windows.Forms.Button
        Me.cmdModifySpecial = New System.Windows.Forms.Button
        Me.cmdDeleteSpecial = New System.Windows.Forms.Button
        Me.lstPreDef = New System.Windows.Forms.ListBox
        Me.cmdRoof = New System.Windows.Forms.Button
        Me.chkDrawRoof = New System.Windows.Forms.CheckBox
        Me.chkDrawGuards = New System.Windows.Forms.CheckBox
        Me.lblImport = New System.Windows.Forms.Label
        Me.lblDim = New System.Windows.Forms.Label
        Me.lblX = New System.Windows.Forms.Label
        Me.lblY = New System.Windows.Forms.Label
        Me.lblTile = New System.Windows.Forms.Label
        Me.lblCurrentTile = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmdDialogOpen = New System.Windows.Forms.OpenFileDialog
        Me.cmdDialogSave = New System.Windows.Forms.SaveFileDialog
        Me.Picture1 = New System.Windows.Forms.PictureBox
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOpen = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSep0 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSaveAs = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFinalize = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep0 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuQuit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPlaceSpecial = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuModifySpecial = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDeleteSpecial = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRefreshTiles = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTitleImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImportRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuParameters = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImportSep0 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSaveMapping = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuLoadMapping = New System.Windows.Forms.ToolStripMenuItem
        Me.sbDown = New System.Windows.Forms.VScrollBar
        Me.sbRight1 = New System.Windows.Forms.HScrollBar
        Me.sbSpecial = New System.Windows.Forms.HScrollBar
        Me.StatusBar1.SuspendLayout()
        Me.frmBottom.SuspendLayout()
        CType(Me.Picture2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.frmRight.SuspendLayout()
        CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkRestrict
        '
        Me.chkRestrict.BackColor = System.Drawing.SystemColors.Control
        Me.chkRestrict.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRestrict.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRestrict.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRestrict.Location = New System.Drawing.Point(8, 272)
        Me.chkRestrict.Name = "chkRestrict"
        Me.chkRestrict.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRestrict.Size = New System.Drawing.Size(97, 35)
        Me.chkRestrict.TabIndex = 5
        Me.chkRestrict.Text = "Restrict Drawing"
        Me.ToolTip1.SetToolTip(Me.chkRestrict, "Restricts tiles drawn to selection")
        Me.chkRestrict.UseVisualStyleBackColor = False
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 663)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(780, 25)
        Me.StatusBar1.TabIndex = 27
        '
        '_StatusBar1_Panel1
        '
        Me._StatusBar1_Panel1.AutoSize = False
        Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
        Me._StatusBar1_Panel1.Size = New System.Drawing.Size(96, 25)
        Me._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmBottom
        '
        Me.frmBottom.BackColor = System.Drawing.SystemColors.Control
        Me.frmBottom.Controls.Add(Me.sbSpecial)
        Me.frmBottom.Controls.Add(Me.cmdFill)
        Me.frmBottom.Controls.Add(Me.Picture2)
        Me.frmBottom.Controls.Add(Me.Text7)
        Me.frmBottom.Controls.Add(Me.chkRandom)
        Me.frmBottom.Controls.Add(Me.lblSpcCount)
        Me.frmBottom.Controls.Add(Me.lblFindSpecial)
        Me.frmBottom.Controls.Add(Me.Label5)
        Me.frmBottom.Controls.Add(Me.Label4)
        Me.frmBottom.Cursor = System.Windows.Forms.Cursors.Default
        Me.frmBottom.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmBottom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmBottom.Location = New System.Drawing.Point(10, 371)
        Me.frmBottom.Name = "frmBottom"
        Me.frmBottom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmBottom.Size = New System.Drawing.Size(753, 281)
        Me.frmBottom.TabIndex = 20
        Me.frmBottom.Text = "Frame1"
        '
        'cmdFill
        '
        Me.cmdFill.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFill.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFill.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFill.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFill.Location = New System.Drawing.Point(528, 8)
        Me.cmdFill.Name = "cmdFill"
        Me.cmdFill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFill.Size = New System.Drawing.Size(73, 49)
        Me.cmdFill.TabIndex = 28
        Me.cmdFill.Text = "Fill Selection"
        Me.cmdFill.UseVisualStyleBackColor = False
        '
        'Picture2
        '
        Me.Picture2.BackColor = System.Drawing.SystemColors.Control
        Me.Picture2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Picture2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Picture2.Location = New System.Drawing.Point(16, 16)
        Me.Picture2.Name = "Picture2"
        Me.Picture2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Picture2.Size = New System.Drawing.Size(260, 260)
        Me.Picture2.TabIndex = 23
        Me.Picture2.TabStop = False
        '
        'Text7
        '
        Me.Text7.AcceptsReturn = True
        Me.Text7.BackColor = System.Drawing.SystemColors.Window
        Me.Text7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Text7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text7.Location = New System.Drawing.Point(288, 32)
        Me.Text7.MaxLength = 0
        Me.Text7.Multiline = True
        Me.Text7.Name = "Text7"
        Me.Text7.ReadOnly = True
        Me.Text7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text7.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Text7.Size = New System.Drawing.Size(225, 249)
        Me.Text7.TabIndex = 22
        Me.Text7.WordWrap = False
        '
        'chkRandom
        '
        Me.chkRandom.BackColor = System.Drawing.SystemColors.Control
        Me.chkRandom.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRandom.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRandom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRandom.Location = New System.Drawing.Point(288, 0)
        Me.chkRandom.Name = "chkRandom"
        Me.chkRandom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRandom.Size = New System.Drawing.Size(145, 25)
        Me.chkRandom.TabIndex = 21
        Me.chkRandom.Text = "Randomize Tiles"
        Me.chkRandom.UseVisualStyleBackColor = False
        '
        'lblSpcCount
        '
        Me.lblSpcCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblSpcCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSpcCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpcCount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSpcCount.Location = New System.Drawing.Point(536, 128)
        Me.lblSpcCount.Name = "lblSpcCount"
        Me.lblSpcCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSpcCount.Size = New System.Drawing.Size(185, 17)
        Me.lblSpcCount.TabIndex = 31
        Me.lblSpcCount.Text = "Specials: 0"
        '
        'lblFindSpecial
        '
        Me.lblFindSpecial.BackColor = System.Drawing.SystemColors.Control
        Me.lblFindSpecial.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFindSpecial.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFindSpecial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFindSpecial.Location = New System.Drawing.Point(536, 104)
        Me.lblFindSpecial.Name = "lblFindSpecial"
        Me.lblFindSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFindSpecial.Size = New System.Drawing.Size(145, 25)
        Me.lblFindSpecial.TabIndex = 30
        Me.lblFindSpecial.Text = "Find Special:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(0, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(17, 257)
        Me.Label5.TabIndex = 25
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(16, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(281, 17)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = " 0    1   2   3    4   5   6    7   8   9   A    B   C   D   E   F"
        '
        'frmRight
        '
        Me.frmRight.BackColor = System.Drawing.SystemColors.Control
        Me.frmRight.Controls.Add(Me.cmdPlaceSpecial)
        Me.frmRight.Controls.Add(Me.cmdObject)
        Me.frmRight.Controls.Add(Me.cmdGuard)
        Me.frmRight.Controls.Add(Me.cmdModifySpecial)
        Me.frmRight.Controls.Add(Me.cmdDeleteSpecial)
        Me.frmRight.Controls.Add(Me.lstPreDef)
        Me.frmRight.Controls.Add(Me.cmdRoof)
        Me.frmRight.Controls.Add(Me.chkDrawRoof)
        Me.frmRight.Controls.Add(Me.chkRestrict)
        Me.frmRight.Controls.Add(Me.chkDrawGuards)
        Me.frmRight.Controls.Add(Me.lblImport)
        Me.frmRight.Controls.Add(Me.lblDim)
        Me.frmRight.Controls.Add(Me.lblX)
        Me.frmRight.Controls.Add(Me.lblY)
        Me.frmRight.Controls.Add(Me.lblTile)
        Me.frmRight.Controls.Add(Me.lblCurrentTile)
        Me.frmRight.Controls.Add(Me.Label3)
        Me.frmRight.Cursor = System.Windows.Forms.Cursors.Default
        Me.frmRight.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmRight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmRight.Location = New System.Drawing.Point(370, 27)
        Me.frmRight.Name = "frmRight"
        Me.frmRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmRight.Size = New System.Drawing.Size(385, 345)
        Me.frmRight.TabIndex = 3
        Me.frmRight.Text = "Frame1"
        '
        'cmdPlaceSpecial
        '
        Me.cmdPlaceSpecial.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPlaceSpecial.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPlaceSpecial.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPlaceSpecial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPlaceSpecial.Location = New System.Drawing.Point(8, 16)
        Me.cmdPlaceSpecial.Name = "cmdPlaceSpecial"
        Me.cmdPlaceSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPlaceSpecial.Size = New System.Drawing.Size(105, 33)
        Me.cmdPlaceSpecial.TabIndex = 13
        Me.cmdPlaceSpecial.Text = "Place &Special"
        Me.cmdPlaceSpecial.UseVisualStyleBackColor = False
        '
        'cmdObject
        '
        Me.cmdObject.BackColor = System.Drawing.SystemColors.Control
        Me.cmdObject.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdObject.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdObject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdObject.Location = New System.Drawing.Point(8, 152)
        Me.cmdObject.Name = "cmdObject"
        Me.cmdObject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdObject.Size = New System.Drawing.Size(105, 27)
        Me.cmdObject.TabIndex = 12
        Me.cmdObject.Text = "Place &Object"
        Me.cmdObject.UseVisualStyleBackColor = False
        '
        'cmdGuard
        '
        Me.cmdGuard.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGuard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGuard.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGuard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGuard.Location = New System.Drawing.Point(8, 120)
        Me.cmdGuard.Name = "cmdGuard"
        Me.cmdGuard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGuard.Size = New System.Drawing.Size(105, 27)
        Me.cmdGuard.TabIndex = 11
        Me.cmdGuard.Text = "Place &Guard"
        Me.cmdGuard.UseVisualStyleBackColor = False
        '
        'cmdModifySpecial
        '
        Me.cmdModifySpecial.BackColor = System.Drawing.SystemColors.Control
        Me.cmdModifySpecial.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdModifySpecial.Enabled = False
        Me.cmdModifySpecial.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdModifySpecial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdModifySpecial.Location = New System.Drawing.Point(8, 48)
        Me.cmdModifySpecial.Name = "cmdModifySpecial"
        Me.cmdModifySpecial.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdModifySpecial.Size = New System.Drawing.Size(105, 33)
        Me.cmdModifySpecial.TabIndex = 10
        Me.cmdModifySpecial.Text = "Modify &Special"
        Me.cmdModifySpecial.UseVisualStyleBackColor = False
        '
        'cmdDeleteSpecial
        '
        Me.cmdDeleteSpecial.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteSpecial.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteSpecial.Enabled = False
        Me.cmdDeleteSpecial.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteSpecial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteSpecial.Location = New System.Drawing.Point(8, 80)
        Me.cmdDeleteSpecial.Name = "cmdDeleteSpecial"
        Me.cmdDeleteSpecial.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteSpecial.Size = New System.Drawing.Size(105, 33)
        Me.cmdDeleteSpecial.TabIndex = 9
        Me.cmdDeleteSpecial.Text = "&Delete Special"
        Me.cmdDeleteSpecial.UseVisualStyleBackColor = False
        '
        'lstPreDef
        '
        Me.lstPreDef.BackColor = System.Drawing.SystemColors.Window
        Me.lstPreDef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstPreDef.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstPreDef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstPreDef.ItemHeight = 14
        Me.lstPreDef.Location = New System.Drawing.Point(120, 168)
        Me.lstPreDef.Name = "lstPreDef"
        Me.lstPreDef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lstPreDef.Size = New System.Drawing.Size(161, 158)
        Me.lstPreDef.TabIndex = 8
        '
        'cmdRoof
        '
        Me.cmdRoof.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRoof.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRoof.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRoof.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRoof.Location = New System.Drawing.Point(8, 184)
        Me.cmdRoof.Name = "cmdRoof"
        Me.cmdRoof.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRoof.Size = New System.Drawing.Size(105, 25)
        Me.cmdRoof.TabIndex = 7
        Me.cmdRoof.Text = "&Roof"
        Me.cmdRoof.UseVisualStyleBackColor = False
        '
        'chkDrawRoof
        '
        Me.chkDrawRoof.BackColor = System.Drawing.SystemColors.Control
        Me.chkDrawRoof.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDrawRoof.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDrawRoof.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDrawRoof.Location = New System.Drawing.Point(8, 216)
        Me.chkDrawRoof.Name = "chkDrawRoof"
        Me.chkDrawRoof.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDrawRoof.Size = New System.Drawing.Size(105, 25)
        Me.chkDrawRoof.TabIndex = 6
        Me.chkDrawRoof.Text = "Show Roof"
        Me.chkDrawRoof.UseVisualStyleBackColor = False
        '
        'chkDrawGuards
        '
        Me.chkDrawGuards.BackColor = System.Drawing.SystemColors.Control
        Me.chkDrawGuards.Checked = True
        Me.chkDrawGuards.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDrawGuards.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDrawGuards.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDrawGuards.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDrawGuards.Location = New System.Drawing.Point(8, 240)
        Me.chkDrawGuards.Name = "chkDrawGuards"
        Me.chkDrawGuards.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDrawGuards.Size = New System.Drawing.Size(105, 19)
        Me.chkDrawGuards.TabIndex = 4
        Me.chkDrawGuards.Text = "Show Guards"
        Me.chkDrawGuards.UseVisualStyleBackColor = False
        '
        'lblImport
        '
        Me.lblImport.BackColor = System.Drawing.SystemColors.Control
        Me.lblImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblImport.Location = New System.Drawing.Point(120, 80)
        Me.lblImport.Name = "lblImport"
        Me.lblImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblImport.Size = New System.Drawing.Size(257, 17)
        Me.lblImport.TabIndex = 26
        Me.lblImport.Text = "Import:"
        '
        'lblDim
        '
        Me.lblDim.BackColor = System.Drawing.SystemColors.Control
        Me.lblDim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDim.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDim.Location = New System.Drawing.Point(120, 8)
        Me.lblDim.Name = "lblDim"
        Me.lblDim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDim.Size = New System.Drawing.Size(272, 25)
        Me.lblDim.TabIndex = 19
        Me.lblDim.Text = "Map Dimensions"
        '
        'lblX
        '
        Me.lblX.BackColor = System.Drawing.SystemColors.Control
        Me.lblX.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblX.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblX.Location = New System.Drawing.Point(120, 32)
        Me.lblX.Name = "lblX"
        Me.lblX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblX.Size = New System.Drawing.Size(272, 17)
        Me.lblX.TabIndex = 18
        Me.lblX.Text = "X:"
        '
        'lblY
        '
        Me.lblY.BackColor = System.Drawing.SystemColors.Control
        Me.lblY.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblY.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblY.Location = New System.Drawing.Point(120, 48)
        Me.lblY.Name = "lblY"
        Me.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblY.Size = New System.Drawing.Size(256, 17)
        Me.lblY.TabIndex = 17
        Me.lblY.Text = "Y:"
        '
        'lblTile
        '
        Me.lblTile.BackColor = System.Drawing.SystemColors.Control
        Me.lblTile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTile.Location = New System.Drawing.Point(120, 64)
        Me.lblTile.Name = "lblTile"
        Me.lblTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTile.Size = New System.Drawing.Size(248, 17)
        Me.lblTile.TabIndex = 16
        Me.lblTile.Text = "Tile:"
        '
        'lblCurrentTile
        '
        Me.lblCurrentTile.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentTile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentTile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentTile.Location = New System.Drawing.Point(120, 96)
        Me.lblCurrentTile.Name = "lblCurrentTile"
        Me.lblCurrentTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentTile.Size = New System.Drawing.Size(256, 17)
        Me.lblCurrentTile.TabIndex = 15
        Me.lblCurrentTile.Text = "current tile"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(120, 144)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(129, 17)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Pre-defined objects:"
        '
        'Picture1
        '
        Me.Picture1.BackColor = System.Drawing.SystemColors.Control
        Me.Picture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Picture1.Location = New System.Drawing.Point(10, 27)
        Me.Picture1.Name = "Picture1"
        Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Picture1.Size = New System.Drawing.Size(336, 320)
        Me.Picture1.TabIndex = 0
        Me.Picture1.TabStop = False
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuOptions, Me.mnuTitleImport})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(780, 24)
        Me.MainMenu1.TabIndex = 28
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNew, Me.mnuOpen, Me.mnuImport, Me.mnuFileSep0, Me.mnuSave, Me.mnuSaveAs, Me.mnuFinalize, Me.mnuSep0, Me.mnuQuit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuNew
        '
        Me.mnuNew.Name = "mnuNew"
        Me.mnuNew.Size = New System.Drawing.Size(162, 22)
        Me.mnuNew.Text = "&New..."
        '
        'mnuOpen
        '
        Me.mnuOpen.Name = "mnuOpen"
        Me.mnuOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuOpen.Size = New System.Drawing.Size(162, 22)
        Me.mnuOpen.Text = "&Open..."
        '
        'mnuImport
        '
        Me.mnuImport.Name = "mnuImport"
        Me.mnuImport.Size = New System.Drawing.Size(162, 22)
        Me.mnuImport.Text = "&Import..."
        '
        'mnuFileSep0
        '
        Me.mnuFileSep0.Name = "mnuFileSep0"
        Me.mnuFileSep0.Size = New System.Drawing.Size(159, 6)
        '
        'mnuSave
        '
        Me.mnuSave.Enabled = False
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSave.Size = New System.Drawing.Size(162, 22)
        Me.mnuSave.Text = "&Save"
        '
        'mnuSaveAs
        '
        Me.mnuSaveAs.Name = "mnuSaveAs"
        Me.mnuSaveAs.Size = New System.Drawing.Size(162, 22)
        Me.mnuSaveAs.Text = "S&ave as..."
        '
        'mnuFinalize
        '
        Me.mnuFinalize.Name = "mnuFinalize"
        Me.mnuFinalize.Size = New System.Drawing.Size(162, 22)
        Me.mnuFinalize.Text = "&Finalize && Save..."
        '
        'mnuSep0
        '
        Me.mnuSep0.Name = "mnuSep0"
        Me.mnuSep0.Size = New System.Drawing.Size(159, 6)
        '
        'mnuQuit
        '
        Me.mnuQuit.Name = "mnuQuit"
        Me.mnuQuit.Size = New System.Drawing.Size(162, 22)
        Me.mnuQuit.Text = "&Quit"
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProperties, Me.mnuSep2, Me.mnuPlaceSpecial, Me.mnuModifySpecial, Me.mnuDeleteSpecial, Me.mnuSep1, Me.mnuRefreshTiles})
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.Size = New System.Drawing.Size(43, 20)
        Me.mnuOptions.Text = "&Map"
        '
        'mnuProperties
        '
        Me.mnuProperties.Name = "mnuProperties"
        Me.mnuProperties.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuProperties.Size = New System.Drawing.Size(191, 22)
        Me.mnuProperties.Text = "&Properties..."
        '
        'mnuSep2
        '
        Me.mnuSep2.Name = "mnuSep2"
        Me.mnuSep2.Size = New System.Drawing.Size(188, 6)
        '
        'mnuPlaceSpecial
        '
        Me.mnuPlaceSpecial.Name = "mnuPlaceSpecial"
        Me.mnuPlaceSpecial.Size = New System.Drawing.Size(191, 22)
        Me.mnuPlaceSpecial.Text = "Place &Special"
        '
        'mnuModifySpecial
        '
        Me.mnuModifySpecial.Name = "mnuModifySpecial"
        Me.mnuModifySpecial.Size = New System.Drawing.Size(191, 22)
        Me.mnuModifySpecial.Text = "Modify &Special"
        '
        'mnuDeleteSpecial
        '
        Me.mnuDeleteSpecial.Name = "mnuDeleteSpecial"
        Me.mnuDeleteSpecial.Size = New System.Drawing.Size(191, 22)
        Me.mnuDeleteSpecial.Text = "&Delete Special"
        '
        'mnuSep1
        '
        Me.mnuSep1.Name = "mnuSep1"
        Me.mnuSep1.Size = New System.Drawing.Size(188, 6)
        '
        'mnuRefreshTiles
        '
        Me.mnuRefreshTiles.Name = "mnuRefreshTiles"
        Me.mnuRefreshTiles.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F5), System.Windows.Forms.Keys)
        Me.mnuRefreshTiles.Size = New System.Drawing.Size(191, 22)
        Me.mnuRefreshTiles.Text = "&Refresh Tiles"
        '
        'mnuTitleImport
        '
        Me.mnuTitleImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuImportRefresh, Me.mnuParameters, Me.mnuImportSep0, Me.mnuSaveMapping, Me.mnuLoadMapping})
        Me.mnuTitleImport.Name = "mnuTitleImport"
        Me.mnuTitleImport.Size = New System.Drawing.Size(55, 20)
        Me.mnuTitleImport.Text = "&Import"
        '
        'mnuImportRefresh
        '
        Me.mnuImportRefresh.Name = "mnuImportRefresh"
        Me.mnuImportRefresh.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.mnuImportRefresh.Size = New System.Drawing.Size(165, 22)
        Me.mnuImportRefresh.Text = "&Refresh"
        '
        'mnuParameters
        '
        Me.mnuParameters.Name = "mnuParameters"
        Me.mnuParameters.Size = New System.Drawing.Size(165, 22)
        Me.mnuParameters.Text = "&Parameters..."
        '
        'mnuImportSep0
        '
        Me.mnuImportSep0.Name = "mnuImportSep0"
        Me.mnuImportSep0.Size = New System.Drawing.Size(162, 6)
        '
        'mnuSaveMapping
        '
        Me.mnuSaveMapping.Name = "mnuSaveMapping"
        Me.mnuSaveMapping.Size = New System.Drawing.Size(165, 22)
        Me.mnuSaveMapping.Text = "&Save Mappings..."
        '
        'mnuLoadMapping
        '
        Me.mnuLoadMapping.Name = "mnuLoadMapping"
        Me.mnuLoadMapping.Size = New System.Drawing.Size(165, 22)
        Me.mnuLoadMapping.Text = "&Load Mappings..."
        '
        'sbDown
        '
        Me.sbDown.Location = New System.Drawing.Point(346, 27)
        Me.sbDown.Maximum = 32767
        Me.sbDown.Name = "sbDown"
        Me.sbDown.Size = New System.Drawing.Size(17, 320)
        Me.sbDown.TabIndex = 29
        '
        'sbRight1
        '
        Me.sbRight1.Location = New System.Drawing.Point(10, 347)
        Me.sbRight1.Maximum = 32767
        Me.sbRight1.Name = "sbRight1"
        Me.sbRight1.Size = New System.Drawing.Size(336, 17)
        Me.sbRight1.TabIndex = 1
        '
        'sbSpecial
        '
        Me.sbSpecial.Location = New System.Drawing.Point(539, 87)
        Me.sbSpecial.Name = "sbSpecial"
        Me.sbSpecial.Size = New System.Drawing.Size(189, 17)
        Me.sbSpecial.TabIndex = 32
        '
        'frmMEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(780, 688)
        Me.Controls.Add(Me.sbRight1)
        Me.Controls.Add(Me.sbDown)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.frmBottom)
        Me.Controls.Add(Me.frmRight)
        Me.Controls.Add(Me.Picture1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(6, 44)
        Me.Name = "frmMEdit"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Form1"
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        Me.frmBottom.ResumeLayout(False)
        Me.frmBottom.PerformLayout()
        CType(Me.Picture2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.frmRight.ResumeLayout(False)
        CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sbDown As System.Windows.Forms.VScrollBar
    Friend WithEvents sbRight1 As System.Windows.Forms.HScrollBar
    Friend WithEvents sbSpecial As System.Windows.Forms.HScrollBar
#End Region 
End Class