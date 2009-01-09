<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmProperties
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
	Public WithEvents _txtMail_3 As System.Windows.Forms.TextBox
	Public WithEvents _txtMail_2 As System.Windows.Forms.TextBox
	Public WithEvents _txtMail_1 As System.Windows.Forms.TextBox
	Public WithEvents _txtMail_0 As System.Windows.Forms.TextBox
	Public WithEvents txtBuyRaftMap As System.Windows.Forms.TextBox
	Public WithEvents txtBuyRaftY As System.Windows.Forms.TextBox
	Public WithEvents txtBuyRaftX As System.Windows.Forms.TextBox
	Public WithEvents txtDefaultTile As System.Windows.Forms.TextBox
	Public WithEvents Label15 As System.Windows.Forms.Label
	Public WithEvents Label14 As System.Windows.Forms.Label
	Public WithEvents Label13 As System.Windows.Forms.Label
	Public WithEvents Label12 As System.Windows.Forms.Label
	Public WithEvents lblDefaultTile As System.Windows.Forms.Label
	Public WithEvents frmChar As System.Windows.Forms.GroupBox
	Public WithEvents txtDungLevels As System.Windows.Forms.TextBox
	Public WithEvents txtDungMonster As System.Windows.Forms.TextBox
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents frmDungeon As System.Windows.Forms.GroupBox
	Public WithEvents _optTiles_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optTiles_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optTiles_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optTiles_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optTiles_3 As System.Windows.Forms.RadioButton
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents cmdDefaults As System.Windows.Forms.Button
	Public WithEvents txtHP As System.Windows.Forms.TextBox
	Public WithEvents txtAttack As System.Windows.Forms.TextBox
	Public WithEvents txtDefense As System.Windows.Forms.TextBox
	Public WithEvents txtColor As System.Windows.Forms.TextBox
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents frmGuards As System.Windows.Forms.GroupBox
    Public WithEvents txtWidth As System.Windows.Forms.TextBox
	Public WithEvents txtHeight As System.Windows.Forms.TextBox
    Public WithEvents txtName As System.Windows.Forms.TextBox
    Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents optTiles As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtMail As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.frmChar = New System.Windows.Forms.GroupBox
        Me._txtMail_3 = New System.Windows.Forms.TextBox
        Me._txtMail_2 = New System.Windows.Forms.TextBox
        Me._txtMail_1 = New System.Windows.Forms.TextBox
        Me._txtMail_0 = New System.Windows.Forms.TextBox
        Me.txtBuyRaftMap = New System.Windows.Forms.TextBox
        Me.txtBuyRaftY = New System.Windows.Forms.TextBox
        Me.txtBuyRaftX = New System.Windows.Forms.TextBox
        Me.txtDefaultTile = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblDefaultTile = New System.Windows.Forms.Label
        Me.frmDungeon = New System.Windows.Forms.GroupBox
        Me.txtDungLevels = New System.Windows.Forms.TextBox
        Me.txtDungMonster = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me._optTiles_2 = New System.Windows.Forms.RadioButton
        Me._optTiles_1 = New System.Windows.Forms.RadioButton
        Me._optTiles_0 = New System.Windows.Forms.RadioButton
        Me._optTiles_4 = New System.Windows.Forms.RadioButton
        Me._optTiles_3 = New System.Windows.Forms.RadioButton
        Me.cmdDefaults = New System.Windows.Forms.Button
        Me.frmGuards = New System.Windows.Forms.GroupBox
        Me.txtHP = New System.Windows.Forms.TextBox
        Me.txtAttack = New System.Windows.Forms.TextBox
        Me.txtDefense = New System.Windows.Forms.TextBox
        Me.txtColor = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cboTypes = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtWidth = New System.Windows.Forms.TextBox
        Me.txtHeight = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.optTiles = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtMail = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.frmChar.SuspendLayout()
        Me.frmDungeon.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.frmGuards.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.optTiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'frmChar
        '
        Me.frmChar.BackColor = System.Drawing.SystemColors.Control
        Me.frmChar.Controls.Add(Me._txtMail_3)
        Me.frmChar.Controls.Add(Me._txtMail_2)
        Me.frmChar.Controls.Add(Me._txtMail_1)
        Me.frmChar.Controls.Add(Me._txtMail_0)
        Me.frmChar.Controls.Add(Me.txtBuyRaftMap)
        Me.frmChar.Controls.Add(Me.txtBuyRaftY)
        Me.frmChar.Controls.Add(Me.txtBuyRaftX)
        Me.frmChar.Controls.Add(Me.txtDefaultTile)
        Me.frmChar.Controls.Add(Me.Label15)
        Me.frmChar.Controls.Add(Me.Label14)
        Me.frmChar.Controls.Add(Me.Label13)
        Me.frmChar.Controls.Add(Me.Label12)
        Me.frmChar.Controls.Add(Me.lblDefaultTile)
        Me.frmChar.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmChar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmChar.Location = New System.Drawing.Point(8, 264)
        Me.frmChar.Name = "frmChar"
        Me.frmChar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmChar.Size = New System.Drawing.Size(193, 169)
        Me.frmChar.TabIndex = 34
        Me.frmChar.TabStop = False
        Me.frmChar.Text = "Characteristics"
        '
        '_txtMail_3
        '
        Me._txtMail_3.AcceptsReturn = True
        Me._txtMail_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtMail_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtMail_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtMail_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMail.SetIndex(Me._txtMail_3, CType(3, Integer))
        Me._txtMail_3.Location = New System.Drawing.Point(136, 128)
        Me._txtMail_3.MaxLength = 0
        Me._txtMail_3.Name = "_txtMail_3"
        Me._txtMail_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtMail_3.Size = New System.Drawing.Size(33, 20)
        Me._txtMail_3.TabIndex = 54
        '
        '_txtMail_2
        '
        Me._txtMail_2.AcceptsReturn = True
        Me._txtMail_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtMail_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtMail_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtMail_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMail.SetIndex(Me._txtMail_2, CType(2, Integer))
        Me._txtMail_2.Location = New System.Drawing.Point(96, 128)
        Me._txtMail_2.MaxLength = 0
        Me._txtMail_2.Name = "_txtMail_2"
        Me._txtMail_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtMail_2.Size = New System.Drawing.Size(33, 20)
        Me._txtMail_2.TabIndex = 53
        '
        '_txtMail_1
        '
        Me._txtMail_1.AcceptsReturn = True
        Me._txtMail_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtMail_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtMail_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtMail_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMail.SetIndex(Me._txtMail_1, CType(1, Integer))
        Me._txtMail_1.Location = New System.Drawing.Point(56, 128)
        Me._txtMail_1.MaxLength = 0
        Me._txtMail_1.Name = "_txtMail_1"
        Me._txtMail_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtMail_1.Size = New System.Drawing.Size(33, 20)
        Me._txtMail_1.TabIndex = 52
        '
        '_txtMail_0
        '
        Me._txtMail_0.AcceptsReturn = True
        Me._txtMail_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtMail_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtMail_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtMail_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMail.SetIndex(Me._txtMail_0, CType(0, Integer))
        Me._txtMail_0.Location = New System.Drawing.Point(16, 128)
        Me._txtMail_0.MaxLength = 0
        Me._txtMail_0.Name = "_txtMail_0"
        Me._txtMail_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtMail_0.Size = New System.Drawing.Size(33, 20)
        Me._txtMail_0.TabIndex = 50
        '
        'txtBuyRaftMap
        '
        Me.txtBuyRaftMap.AcceptsReturn = True
        Me.txtBuyRaftMap.BackColor = System.Drawing.SystemColors.Window
        Me.txtBuyRaftMap.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBuyRaftMap.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBuyRaftMap.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBuyRaftMap.Location = New System.Drawing.Point(96, 88)
        Me.txtBuyRaftMap.MaxLength = 0
        Me.txtBuyRaftMap.Name = "txtBuyRaftMap"
        Me.txtBuyRaftMap.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBuyRaftMap.Size = New System.Drawing.Size(57, 20)
        Me.txtBuyRaftMap.TabIndex = 49
        Me.txtBuyRaftMap.Text = "1"
        '
        'txtBuyRaftY
        '
        Me.txtBuyRaftY.AcceptsReturn = True
        Me.txtBuyRaftY.BackColor = System.Drawing.SystemColors.Window
        Me.txtBuyRaftY.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBuyRaftY.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBuyRaftY.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBuyRaftY.Location = New System.Drawing.Point(96, 64)
        Me.txtBuyRaftY.MaxLength = 0
        Me.txtBuyRaftY.Name = "txtBuyRaftY"
        Me.txtBuyRaftY.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBuyRaftY.Size = New System.Drawing.Size(57, 20)
        Me.txtBuyRaftY.TabIndex = 47
        Me.txtBuyRaftY.Text = "0"
        '
        'txtBuyRaftX
        '
        Me.txtBuyRaftX.AcceptsReturn = True
        Me.txtBuyRaftX.BackColor = System.Drawing.SystemColors.Window
        Me.txtBuyRaftX.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBuyRaftX.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBuyRaftX.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBuyRaftX.Location = New System.Drawing.Point(96, 40)
        Me.txtBuyRaftX.MaxLength = 0
        Me.txtBuyRaftX.Name = "txtBuyRaftX"
        Me.txtBuyRaftX.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBuyRaftX.Size = New System.Drawing.Size(57, 20)
        Me.txtBuyRaftX.TabIndex = 46
        Me.txtBuyRaftX.Text = "0"
        '
        'txtDefaultTile
        '
        Me.txtDefaultTile.AcceptsReturn = True
        Me.txtDefaultTile.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefaultTile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefaultTile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefaultTile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefaultTile.Location = New System.Drawing.Point(72, 16)
        Me.txtDefaultTile.MaxLength = 0
        Me.txtDefaultTile.Name = "txtDefaultTile"
        Me.txtDefaultTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefaultTile.Size = New System.Drawing.Size(110, 20)
        Me.txtDefaultTile.TabIndex = 15
        Me.txtDefaultTile.Text = "0"
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label15.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label15.Location = New System.Drawing.Point(8, 112)
        Me.Label15.Name = "Label15"
        Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label15.Size = New System.Drawing.Size(65, 17)
        Me.Label15.TabIndex = 51
        Me.Label15.Text = "Mail Route"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(24, 88)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(65, 17)
        Me.Label14.TabIndex = 48
        Me.Label14.Text = "Buy Raft Map"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(24, 64)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(89, 17)
        Me.Label13.TabIndex = 45
        Me.Label13.Text = "Buy Raft Y"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(24, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(97, 17)
        Me.Label12.TabIndex = 44
        Me.Label12.Text = "Buy Raft X"
        '
        'lblDefaultTile
        '
        Me.lblDefaultTile.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefaultTile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefaultTile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultTile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefaultTile.Location = New System.Drawing.Point(8, 16)
        Me.lblDefaultTile.Name = "lblDefaultTile"
        Me.lblDefaultTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefaultTile.Size = New System.Drawing.Size(65, 17)
        Me.lblDefaultTile.TabIndex = 35
        Me.lblDefaultTile.Text = "Default Tile:"
        '
        'frmDungeon
        '
        Me.frmDungeon.BackColor = System.Drawing.SystemColors.Control
        Me.frmDungeon.Controls.Add(Me.txtDungLevels)
        Me.frmDungeon.Controls.Add(Me.txtDungMonster)
        Me.frmDungeon.Controls.Add(Me.Label11)
        Me.frmDungeon.Controls.Add(Me.Label10)
        Me.frmDungeon.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmDungeon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmDungeon.Location = New System.Drawing.Point(8, 392)
        Me.frmDungeon.Name = "frmDungeon"
        Me.frmDungeon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmDungeon.Size = New System.Drawing.Size(193, 121)
        Me.frmDungeon.TabIndex = 38
        Me.frmDungeon.TabStop = False
        Me.frmDungeon.Text = "Dungeon Level Characteristics"
        '
        'txtDungLevels
        '
        Me.txtDungLevels.AcceptsReturn = True
        Me.txtDungLevels.BackColor = System.Drawing.SystemColors.Window
        Me.txtDungLevels.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDungLevels.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDungLevels.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDungLevels.Location = New System.Drawing.Point(64, 48)
        Me.txtDungLevels.MaxLength = 0
        Me.txtDungLevels.Name = "txtDungLevels"
        Me.txtDungLevels.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDungLevels.Size = New System.Drawing.Size(121, 20)
        Me.txtDungLevels.TabIndex = 42
        '
        'txtDungMonster
        '
        Me.txtDungMonster.AcceptsReturn = True
        Me.txtDungMonster.BackColor = System.Drawing.SystemColors.Window
        Me.txtDungMonster.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDungMonster.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDungMonster.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDungMonster.Location = New System.Drawing.Point(104, 24)
        Me.txtDungMonster.MaxLength = 0
        Me.txtDungMonster.Name = "txtDungMonster"
        Me.txtDungMonster.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDungMonster.Size = New System.Drawing.Size(81, 20)
        Me.txtDungMonster.TabIndex = 40
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(8, 48)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(89, 17)
        Me.Label11.TabIndex = 41
        Me.Label11.Text = "Levels"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(8, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(105, 17)
        Me.Label10.TabIndex = 39
        Me.Label10.Text = "Monster Str Factor"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me._optTiles_2)
        Me.Frame2.Controls.Add(Me._optTiles_1)
        Me.Frame2.Controls.Add(Me._optTiles_0)
        Me.Frame2.Controls.Add(Me._optTiles_4)
        Me.Frame2.Controls.Add(Me._optTiles_3)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 168)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(401, 89)
        Me.Frame2.TabIndex = 36
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Tile Set"
        '
        '_optTiles_2
        '
        Me._optTiles_2.BackColor = System.Drawing.SystemColors.Control
        Me._optTiles_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTiles_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTiles_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTiles.SetIndex(Me._optTiles_2, CType(2, Integer))
        Me._optTiles_2.Location = New System.Drawing.Point(16, 64)
        Me._optTiles_2.Name = "_optTiles_2"
        Me._optTiles_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTiles_2.Size = New System.Drawing.Size(161, 17)
        Me._optTiles_2.TabIndex = 43
        Me._optTiles_2.TabStop = True
        Me._optTiles_2.Tag = "CastleTiles.bmp"
        Me._optTiles_2.Text = "CastleTiles.bmp"
        Me._optTiles_2.UseVisualStyleBackColor = False
        '
        '_optTiles_1
        '
        Me._optTiles_1.BackColor = System.Drawing.SystemColors.Control
        Me._optTiles_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTiles_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTiles_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTiles.SetIndex(Me._optTiles_1, CType(1, Integer))
        Me._optTiles_1.Location = New System.Drawing.Point(16, 40)
        Me._optTiles_1.Name = "_optTiles_1"
        Me._optTiles_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTiles_1.Size = New System.Drawing.Size(161, 17)
        Me._optTiles_1.TabIndex = 12
        Me._optTiles_1.TabStop = True
        Me._optTiles_1.Tag = "TownTiles.bmp"
        Me._optTiles_1.Text = "TownTiles.bmp"
        Me._optTiles_1.UseVisualStyleBackColor = False
        '
        '_optTiles_0
        '
        Me._optTiles_0.BackColor = System.Drawing.SystemColors.Control
        Me._optTiles_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTiles_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTiles_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTiles.SetIndex(Me._optTiles_0, CType(0, Integer))
        Me._optTiles_0.Location = New System.Drawing.Point(16, 16)
        Me._optTiles_0.Name = "_optTiles_0"
        Me._optTiles_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTiles_0.Size = New System.Drawing.Size(113, 17)
        Me._optTiles_0.TabIndex = 11
        Me._optTiles_0.TabStop = True
        Me._optTiles_0.Tag = "Tiles.bmp"
        Me._optTiles_0.Text = "Tiles.bmp"
        Me._optTiles_0.UseVisualStyleBackColor = False
        '
        '_optTiles_4
        '
        Me._optTiles_4.BackColor = System.Drawing.SystemColors.Control
        Me._optTiles_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTiles_4.Enabled = False
        Me._optTiles_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTiles_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTiles.SetIndex(Me._optTiles_4, CType(4, Integer))
        Me._optTiles_4.Location = New System.Drawing.Point(192, 40)
        Me._optTiles_4.Name = "_optTiles_4"
        Me._optTiles_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTiles_4.Size = New System.Drawing.Size(201, 17)
        Me._optTiles_4.TabIndex = 14
        Me._optTiles_4.TabStop = True
        Me._optTiles_4.Tag = "LOB TownTiles.bmp"
        Me._optTiles_4.Text = "LOB TownTiles.bmp"
        Me._optTiles_4.UseVisualStyleBackColor = False
        '
        '_optTiles_3
        '
        Me._optTiles_3.BackColor = System.Drawing.SystemColors.Control
        Me._optTiles_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTiles_3.Enabled = False
        Me._optTiles_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTiles_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTiles.SetIndex(Me._optTiles_3, CType(3, Integer))
        Me._optTiles_3.Location = New System.Drawing.Point(192, 16)
        Me._optTiles_3.Name = "_optTiles_3"
        Me._optTiles_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTiles_3.Size = New System.Drawing.Size(201, 17)
        Me._optTiles_3.TabIndex = 13
        Me._optTiles_3.TabStop = True
        Me._optTiles_3.Tag = "LOB Tiles.bmp"
        Me._optTiles_3.Text = "LOB Tiles.bmp"
        Me._optTiles_3.UseVisualStyleBackColor = False
        '
        'cmdDefaults
        '
        Me.cmdDefaults.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDefaults.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDefaults.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefaults.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDefaults.Location = New System.Drawing.Point(208, 400)
        Me.cmdDefaults.Name = "cmdDefaults"
        Me.cmdDefaults.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDefaults.Size = New System.Drawing.Size(65, 25)
        Me.cmdDefaults.TabIndex = 28
        Me.cmdDefaults.TabStop = False
        Me.cmdDefaults.Text = "Defaults"
        Me.cmdDefaults.UseVisualStyleBackColor = False
        '
        'frmGuards
        '
        Me.frmGuards.BackColor = System.Drawing.SystemColors.Control
        Me.frmGuards.Controls.Add(Me.txtHP)
        Me.frmGuards.Controls.Add(Me.txtAttack)
        Me.frmGuards.Controls.Add(Me.txtDefense)
        Me.frmGuards.Controls.Add(Me.txtColor)
        Me.frmGuards.Controls.Add(Me.Label4)
        Me.frmGuards.Controls.Add(Me.Label5)
        Me.frmGuards.Controls.Add(Me.Label6)
        Me.frmGuards.Controls.Add(Me.Label7)
        Me.frmGuards.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmGuards.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmGuards.Location = New System.Drawing.Point(208, 264)
        Me.frmGuards.Name = "frmGuards"
        Me.frmGuards.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmGuards.Size = New System.Drawing.Size(201, 121)
        Me.frmGuards.TabIndex = 18
        Me.frmGuards.TabStop = False
        Me.frmGuards.Text = "Guards"
        '
        'txtHP
        '
        Me.txtHP.AcceptsReturn = True
        Me.txtHP.BackColor = System.Drawing.SystemColors.Window
        Me.txtHP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHP.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHP.Location = New System.Drawing.Point(80, 16)
        Me.txtHP.MaxLength = 0
        Me.txtHP.Name = "txtHP"
        Me.txtHP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHP.Size = New System.Drawing.Size(113, 20)
        Me.txtHP.TabIndex = 19
        '
        'txtAttack
        '
        Me.txtAttack.AcceptsReturn = True
        Me.txtAttack.BackColor = System.Drawing.SystemColors.Window
        Me.txtAttack.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAttack.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAttack.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAttack.Location = New System.Drawing.Point(80, 40)
        Me.txtAttack.MaxLength = 0
        Me.txtAttack.Name = "txtAttack"
        Me.txtAttack.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAttack.Size = New System.Drawing.Size(113, 20)
        Me.txtAttack.TabIndex = 20
        '
        'txtDefense
        '
        Me.txtDefense.AcceptsReturn = True
        Me.txtDefense.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefense.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefense.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefense.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefense.Location = New System.Drawing.Point(80, 64)
        Me.txtDefense.MaxLength = 0
        Me.txtDefense.Name = "txtDefense"
        Me.txtDefense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefense.Size = New System.Drawing.Size(113, 20)
        Me.txtDefense.TabIndex = 21
        '
        'txtColor
        '
        Me.txtColor.AcceptsReturn = True
        Me.txtColor.BackColor = System.Drawing.SystemColors.Window
        Me.txtColor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtColor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtColor.Location = New System.Drawing.Point(80, 88)
        Me.txtColor.MaxLength = 0
        Me.txtColor.Name = "txtColor"
        Me.txtColor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtColor.Size = New System.Drawing.Size(113, 20)
        Me.txtColor.TabIndex = 23
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(8, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(70, 17)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "HP (+/- 10%)"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(8, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(71, 17)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Attack"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(8, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(71, 17)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "Defense"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(8, 88)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(71, 17)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Color (0=deflt)"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboTypes)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.txtWidth)
        Me.Frame1.Controls.Add(Me.txtHeight)
        Me.Frame1.Controls.Add(Me.txtName)
        Me.Frame1.Controls.Add(Me.Label8)
        Me.Frame1.Controls.Add(Me.Label3)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(401, 153)
        Me.Frame1.TabIndex = 16
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Map Type"
        '
        'cboTypes
        '
        Me.cboTypes.FormattingEnabled = True
        Me.cboTypes.Location = New System.Drawing.Point(76, 51)
        Me.cboTypes.Name = "cboTypes"
        Me.cboTypes.Size = New System.Drawing.Size(203, 22)
        Me.cboTypes.TabIndex = 33
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(54, 14)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Map Type"
        '
        'txtWidth
        '
        Me.txtWidth.AcceptsReturn = True
        Me.txtWidth.BackColor = System.Drawing.SystemColors.Window
        Me.txtWidth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWidth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWidth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWidth.Location = New System.Drawing.Point(76, 94)
        Me.txtWidth.MaxLength = 0
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWidth.Size = New System.Drawing.Size(89, 20)
        Me.txtWidth.TabIndex = 8
        '
        'txtHeight
        '
        Me.txtHeight.AcceptsReturn = True
        Me.txtHeight.BackColor = System.Drawing.SystemColors.Window
        Me.txtHeight.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHeight.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeight.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHeight.Location = New System.Drawing.Point(76, 118)
        Me.txtHeight.MaxLength = 0
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHeight.Size = New System.Drawing.Size(89, 20)
        Me.txtHeight.TabIndex = 9
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(76, 26)
        Me.txtName.MaxLength = 16
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(201, 20)
        Me.txtName.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(33, 118)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(37, 14)
        Me.Label8.TabIndex = 31
        Me.Label8.Text = "Height"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(36, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(34, 14)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Width"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(13, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(57, 14)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Map Name"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(280, 400)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(65, 25)
        Me.cmdCancel.TabIndex = 30
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(352, 400)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(65, 25)
        Me.cmdOK.TabIndex = 32
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'optType
        '
        '
        'frmProperties
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(427, 434)
        Me.Controls.Add(Me.frmChar)
        Me.Controls.Add(Me.frmDungeon)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.cmdDefaults)
        Me.Controls.Add(Me.frmGuards)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmProperties"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Map Properties"
        Me.frmChar.ResumeLayout(False)
        Me.frmChar.PerformLayout()
        Me.frmDungeon.ResumeLayout(False)
        Me.frmDungeon.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.frmGuards.ResumeLayout(False)
        Me.frmGuards.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.optTiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboTypes As System.Windows.Forms.ComboBox
    Public WithEvents Label2 As System.Windows.Forms.Label
#End Region 
End Class