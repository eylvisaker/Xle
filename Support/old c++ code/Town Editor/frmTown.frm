VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   9600
   ClientLeft      =   90
   ClientTop       =   660
   ClientWidth     =   10290
   BeginProperty Font 
      Name            =   "Times New Roman"
      Size            =   12
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   ScaleHeight     =   640
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   686
   Begin MSComCtl2.FlatScrollBar sbDown 
      Height          =   5295
      Left            =   4920
      TabIndex        =   26
      Top             =   0
      Width           =   255
      _ExtentX        =   450
      _ExtentY        =   9340
      _Version        =   393216
      Appearance      =   0
      Orientation     =   1179648
   End
   Begin MSComCtl2.FlatScrollBar sbRight 
      Height          =   255
      Left            =   0
      TabIndex        =   25
      Top             =   5280
      Width           =   4935
      _ExtentX        =   8705
      _ExtentY        =   450
      _Version        =   393216
      Appearance      =   0
      Arrows          =   65536
      Orientation     =   1179649
   End
   Begin VB.CommandButton cmdGuard 
      Caption         =   "&Guard"
      Height          =   405
      Left            =   4080
      TabIndex        =   24
      Top             =   8520
      Width           =   1215
   End
   Begin VB.CommandButton cmdObject 
      Caption         =   "Well"
      Height          =   375
      Index           =   3
      Left            =   4080
      TabIndex        =   23
      Top             =   8040
      Width           =   1215
   End
   Begin VB.CommandButton cmdObject 
      Caption         =   "Crystal Ball"
      Height          =   375
      Index           =   2
      Left            =   4080
      TabIndex        =   22
      Top             =   7560
      Width           =   1215
   End
   Begin VB.CommandButton cmdObject 
      Caption         =   "Merchant"
      Height          =   375
      Index           =   1
      Left            =   4080
      TabIndex        =   21
      Top             =   7080
      Width           =   1215
   End
   Begin VB.CommandButton cmdObject 
      Caption         =   "Tree"
      Height          =   405
      Index           =   0
      Left            =   4080
      TabIndex        =   20
      Top             =   6600
      Width           =   1215
   End
   Begin VB.CheckBox chkRandom 
      Caption         =   "Random"
      Height          =   375
      Left            =   4080
      TabIndex        =   19
      Top             =   6120
      Width           =   1215
   End
   Begin VB.TextBox Text7 
      Height          =   3855
      Left            =   5400
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   18
      Top             =   5640
      Width           =   4815
   End
   Begin VB.CommandButton cmdSpecial 
      Caption         =   "Place &Store"
      Height          =   375
      Left            =   4080
      TabIndex        =   17
      Top             =   5640
      Width           =   1215
   End
   Begin MSComDlg.CommonDialog cmdDialog 
      Left            =   9720
      Top             =   2040
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CommandButton cmdFill 
      Caption         =   "Fill Box"
      Height          =   495
      Left            =   6600
      TabIndex        =   14
      Top             =   4560
      Width           =   1215
   End
   Begin VB.CommandButton cmdSetCorner 
      Caption         =   "Set Corner"
      Height          =   495
      Left            =   6600
      TabIndex        =   13
      Top             =   2160
      Width           =   1215
   End
   Begin VB.TextBox txtCY 
      Height          =   405
      Index           =   1
      Left            =   7680
      TabIndex        =   12
      Top             =   3720
      Width           =   1215
   End
   Begin VB.TextBox txtCX 
      Height          =   405
      Index           =   1
      Left            =   7680
      TabIndex        =   11
      Top             =   3360
      Width           =   1215
   End
   Begin VB.OptionButton optCorner 
      Caption         =   "Corner 2"
      Height          =   375
      Index           =   1
      Left            =   7680
      TabIndex        =   10
      Top             =   2880
      Width           =   1215
   End
   Begin VB.OptionButton optCorner 
      Caption         =   "Corner 1"
      Height          =   375
      Index           =   0
      Left            =   5400
      TabIndex        =   9
      Top             =   2880
      Width           =   1215
   End
   Begin VB.TextBox txtCY 
      Height          =   405
      Index           =   0
      Left            =   5400
      TabIndex        =   8
      Top             =   3720
      Width           =   1215
   End
   Begin VB.TextBox txtCX 
      Height          =   405
      Index           =   0
      Left            =   5400
      TabIndex        =   7
      Top             =   3360
      Width           =   1215
   End
   Begin VB.PictureBox Picture2 
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3900
      Left            =   0
      ScaleHeight     =   256
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   256
      TabIndex        =   1
      Top             =   5640
      Width           =   3900
   End
   Begin VB.PictureBox Picture1 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5295
      Left            =   0
      ScaleHeight     =   349
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   325
      TabIndex        =   0
      Top             =   0
      Width           =   4935
   End
   Begin VB.Label Label2 
      Alignment       =   2  'Center
      Caption         =   "Y"
      Height          =   495
      Left            =   6840
      TabIndex        =   16
      Top             =   3840
      Width           =   735
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "X"
      Height          =   495
      Left            =   6840
      TabIndex        =   15
      Top             =   3360
      Width           =   735
   End
   Begin VB.Label lblCurrentTile 
      Caption         =   "current tile"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5280
      TabIndex        =   6
      Top             =   1560
      Width           =   4695
   End
   Begin VB.Label lblTile 
      Caption         =   "Tile:"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   5280
      TabIndex        =   5
      Top             =   1200
      Width           =   4575
   End
   Begin VB.Label lblY 
      Caption         =   "Y:"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   5280
      TabIndex        =   4
      Top             =   840
      Width           =   4500
   End
   Begin VB.Label lblX 
      Caption         =   "X:"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   5280
      TabIndex        =   3
      Top             =   480
      Width           =   4500
   End
   Begin VB.Label lblDim 
      Caption         =   "Map Dimensions"
      Height          =   375
      Left            =   5280
      TabIndex        =   2
      Top             =   120
      Width           =   3975
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuNew 
         Caption         =   "&New..."
      End
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open..."
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save"
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuSaveAs 
         Caption         =   "S&ave as..."
      End
      Begin VB.Menu mnuSep0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuQuit 
         Caption         =   "&Quit"
      End
   End
   Begin VB.Menu mnuOptions 
      Caption         =   "&Options"
      Begin VB.Menu mnuProperties 
         Caption         =   "Map &Properties"
         Shortcut        =   ^P
      End
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Const maxSpecial = 20

Private Type Point
    X As Integer
    Y As Integer
End Type

Dim DX7 As New DirectX7
Dim DDraw7 As DirectDraw7

Dim DDSBack As DirectDrawSurface7
Dim DDSPrimary As DirectDrawSurface7
Dim DDSTiles As DirectDrawSurface7
Dim DDSTemp As DirectDrawSurface7
Dim DDSChar As DirectDrawSurface7

Dim ddsd As DDSURFACEDESC2
Dim ddsd1 As DDSURFACEDESC2
Dim ddClipper As DirectDrawClipper

Dim backWidth As Long
Dim backHeight As Long

Dim dispX As Integer
Dim dispY As Integer

Dim x1 As Integer, x2 As Integer
Dim y1 As Integer, y2 As Integer

Dim map(1000, 1000) As Integer
Dim mapWidth As Integer
Dim mapHeight As Integer
Dim mapName As String * 16
Dim mapType As Integer

Dim picTilesX As Integer
Dim picTilesY As Integer
Dim leftX As Integer, topY As Integer
Dim filename As String
Dim defaultTile As Integer

Dim special(maxSpecial) As Integer
Dim specialx(maxSpecial) As Integer, specialy(maxSpecial) As Integer
Dim specialdata(maxSpecial) As String * 100

Dim currentTile As Integer
Dim fileOffset As Integer

Dim guard(100) As Point
Dim guardAttack As Integer
Dim guardDefense As Integer
Dim guardColor As Integer
Dim guardHP As Integer

Dim LotaPath As String

Const TileSize = 16

Private Sub cmdFill_Click()
    xdif = txtCX(1) - txtCX(0)
    ydif = txtCY(1) - txtCY(0)
    
    If (xdif = 0) Then
        xdif = xdif + 1
    End If
    
    If ydif = 0 Then
        ydif = ydif + 1
    End If

    tile = currentTile
    If chkRandom.Value = 1 Then
        If currentTile = 7 Then
            r = Int(Rnd(1) * 4)
            
            If r < 2 Then tile = currentTile + r
            If r > 1 Then tile = currentTile + r + 14
            
        ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
            r = Int(Rnd(1) * 2)
            
            tile = currentTile + r
            
        End If
    End If
    
    For i = txtCX(0) To txtCX(1) Step Sgn(xdif)
        For j = txtCY(0) To txtCY(1) Step Sgn(ydif)
            If chkRandom.Value = 1 Then
                If currentTile = 7 Then
                    r = Int(Rnd(1) * 4)
                    
                    If r < 2 Then tile = currentTile + r
                    If r > 1 Then tile = currentTile + r + 14
                    
                ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                    r = Int(Rnd(1) * 2)
                    
                    tile = currentTile + r
                    
                End If
            End If

            map(i, j) = tile

        Next
    Next
    
    blit
        
End Sub

Private Sub cmdGuard_Click()
    
    For i = 0 To 100
        If guard(i).X = x1 And guard(i).Y = y1 Then
            guard(i).X = 0
            guard(i).Y = 0
            found = True
        End If
    Next
    
    If found <> True Then
        For i = 0 To 100
            If guard(i).X = 0 And guard(i).Y = 0 Then
                guard(i).X = x1
                guard(i).Y = y1
                
                blit
                Exit Sub
                
            End If
        Next
    End If
    
    blit
End Sub

Private Sub cmdObject_Click(Index As Integer)
    Select Case Index
    Case 0
        tile = 251 - 48
        For j = 0 To 2
            For i = 0 To 2
                map(x1 + i, y1 + j) = tile + i + j * 16
            Next
        Next
        
        map(x1 + 1, y1 + 3) = 64
        map(x1 + 2, y1 + 3) = 48
        map(x1 + 3, y1 + 3) = 65
        map(x1 + 3, y1 + 1) = 49
        map(x1 + 3, y1 + 2) = 48
        map(x1, y1 + 3) = 4
        map(x1 + 3, y1) = 4
    Case 1
        tile = 254 - 32
        For j = 0 To 1
            For i = 0 To 1
                map(x1 + i, y1 + j) = tile + i + j * 16
            Next
        Next
    Case 2
        tile = 248 - 32
        For j = 0 To 0
            For i = 0 To 2
                map(x1 + i, y1 + j) = tile + i + j * 16
            Next
        Next
    Case 3
        tile = 246 - 32
        For j = 0 To 1
            For i = 0 To 1
                map(x1 + i, y1 + j) = tile + i + j * 16
            Next
        Next

    End Select
    
    blit
End Sub

Private Sub cmdSetCorner_Click()
    For i = 0 To 1
        If optCorner(i).Value = True Then
            txtCX(i) = x1
            txtCY(i) = y1
        End If
    Next
    
End Sub

Private Sub cmdSpecial_Click()
        
    For i = 1 To maxSpecial
        If specialx(i) = x1 And specialy(i) = y1 Then
            specialx(i) = 0
            specialy(i) = 0
            special(i) = 0
            
            FillSpecial
            blit
            Exit Sub
            
        End If
    Next
        
    frmPlaceStore.Show 1
    
    For i = 1 To maxSpecial
        If special(i) = 0 Then
            For j = 0 To 13
                If frmPlaceStore.optStoreType(j).Value = True Then
                    special(i) = j + 2
                End If
            Next
            
            specialx(i) = x1
            specialy(i) = y1
            
            specialdata(i) = frmPlaceStore.lblSpecialData
            
            Exit For
        End If
    Next
    
    FillSpecial
End Sub

Private Sub Form_Load()
    Randomize Timer
    Picture1.Width = 324
    Picture1.Height = 324
    Picture2.Top = Picture1.Height + 16 + Picture1.Top + 16 + sbRight.Height
    
    LotaPath = App.Path & "\.."
    
    Set DDraw7 = DX7.DirectDrawCreate("")
    
    Call DDraw7.SetCooperativeLevel(Me.hWnd, DDSCL_NORMAL)
    
    ddsd.lFlags = DDSD_CAPS
    ddsd.ddsCaps.lCaps = DDSCAPS_PRIMARYSURFACE
    
    ddsd1.lFlags = DDSD_CAPS Or DDSD_WIDTH Or DDSD_HEIGHT
    ddsd1.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
    ddsd1.lWidth = Picture1.ScaleWidth
    ddsd1.lHeight = Picture1.ScaleHeight
    
    backWidth = ddsd1.lWidth
    backHeight = ddsd1.lHeight
    
    Set DDSPrimary = DDraw7.CreateSurface(ddsd)
    Set DDSBack = DDraw7.CreateSurface(ddsd1)
    
    ddsd.lFlags = DDSD_CAPS Or DDSD_CKSRCBLT
    ddsd.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
    
    Set DDSTiles = DDraw7.CreateSurfaceFromFile(LotaPath & "\images\towntiles.bmp", ddsd)
    
    'ddsd.ddckCKSrcBlt.low = 0
    'ddsd.ddckCKSrcBlt.high = 0
    
    Set DDSChar = DDraw7.CreateSurfaceFromFile(LotaPath & "\images\character.bmp", ddsd)
    
    Set DDSTemp = DDraw7.CreateSurface(ddsd)
    
    Set ddClipper = DDraw7.CreateClipper(0)

    DDSBack.SetForeColor (&HFFFFFF)
    DDSBack.SetFont Picture1.Font

    NewMap 100, 100
    
    blit
End Sub

Private Sub form_paint()
    blit
End Sub

Sub blit()
    Dim ddrval As Long
    Dim r1 As RECT
    Dim r2 As RECT
    
    draw
    
    'Gets the bounding rect for the entire window handle, stores in r1
    Call DX7.GetWindowRect(Picture1.hWnd, r1)
    
    r2.Bottom = backHeight
    r2.Right = backWidth
    
    ddrval = DDSPrimary.Blt(r1, DDSBack, r2, DDBLT_WAIT)
    
    Call DX7.GetWindowRect(Picture2.hWnd, r1)
    r2.Bottom = 256
    r2.Right = 256
    
    ddrval = DDSPrimary.Blt(r1, DDSTiles, r2, DDBLT_WAIT)
    

End Sub

Private Sub draw()
    Dim ddrval As Long
    Dim r1 As RECT
    Dim r2 As RECT
    Dim j, i, a, tilex, tiley
    Dim xx, yy, centerx, centery
    
    r1.Bottom = backHeight
    r1.Right = backWidth
    
    DDSBack.restore
    
    DDSBack.BltColorFill r1, &H555555
    
    'On Local Error Resume Next
    
    lblDim = "Map Dimensions: " & mapWidth & " x " & mapHeight
    If filename <> "" Then
        Me.Caption = "LotA Town Editor - " & filename
    End If
    
    picTilesX = Int(Picture1.ScaleWidth / TileSize / 2)
    picTilesY = Int(Picture1.ScaleHeight / TileSize / 2)
        
    xx = 0
    yy = 0
    
    sbRight.LargeChange = picTilesX * 2 - 2
    sbDown.LargeChange = picTilesY * 2 - 2
    
    centerx = sbRight.Value
    centery = sbDown.Value
    
    leftX = centerx - picTilesX
    topY = centery - picTilesY
    
    For j = dispY - picTilesY To dispY + picTilesY + 1
        For i = dispX - picTilesX To dispX + picTilesX + 1
            If i >= 0 And i < mapWidth And j >= 0 And j < mapHeight Then

                a = map(i, j)
                                
                If i = dispX And j = dispY Then
                    centerx = xx
                    centery = yy
                
                End If
                                    
                tilex = (a Mod 16) * 16
                tiley = Int(a / 16) * 16
                                
                r1.Left = tilex
                r1.Right = tilex + 16
                r1.Top = tiley
                r1.Bottom = tiley + 16
                
                DDSBack.BltFast xx * TileSize, yy * TileSize, DDSTiles, r1, DDBLTFAST_WAIT
                
                
                
            Else
                'Picture1.Line (xx * tilesize, yy * tilesize)-((xx + 1) * tilesize, (yy + 1) * tilesize), vbBlack, BF

            End If
            
            xx = xx + 1
        Next
        yy = yy + 1
        xx = 0

    Next
    
    For i = 0 To 100
        If guard(i).X >= leftX And guard(i).X < leftX + 2 * picTilesX + 1 And _
            guard(i).Y >= topY And guard(i).Y < topY + 2 * picTilesY + 1 And _
            guard(i).X > 0 And guard(i).Y > 0 Then
            
            r1.Top = 5 * 8
            r1.Left = 0 * 32
            r1.Right = r1.Left + 64
            r1.Bottom = r1.Top + 8
            
            r2.Left = (guard(i).X - leftX) * TileSize
            r2.Top = (guard(i).Y - topY) * TileSize
            r2.Right = r2.Left + 32
            r2.Bottom = r2.Top + 32
            
            DDSBack.Blt r2, DDSChar, r1, DDBLTFAST_WAIT Or DDBLT_KEYSRC
            
        End If
    Next
    
    '' draw selection box
    DDSBack.DrawBox (x1 - leftX) * TileSize, (y1 - topY) * TileSize, (x2 - leftX + 1) * TileSize, (y2 - topY + 1) * TileSize
    
    'Picture1.Line (centerx * TileSize, centery * TileSize)-((centerx + 1) * TileSize, centery * TileSize), &HFFFF00, BF
    'Picture1.Line -((centerx + 1) * TileSize, (centery + 1) * TileSize), &HFFFF00, BF
    'Picture1.Line -(centerx * TileSize, (centery + 1) * TileSize), &HFFFF00, BF
    'Picture1.Line -(centerx * TileSize, centery * TileSize), &HFFFF00, BF
    
    lblX.Caption = "x1:   " & x1 & "  0x" & Hex$(x1) & "    x2: " & x2
    lblY.Caption = "y1:   " & y1 & "  0x" & Hex$(y1) & "    y2: " & y2
    
    If x1 < 0 Or x1 > mapWidth Or y1 < 0 Or y1 > mapHeight Then
        lblTile.Caption = "Tile: Out of range"
    Else
        lblTile.Caption = "Tile: " & map(x1, y1) & "   0x" & Hex$(map(x1, y1))
    End If
    
    lblCurrentTile = "Current Tile: " & currentTile & "   0x" & Hex$(currentTile)
    
   ' Label7(0).Caption = ""
   ' Label7(1).Caption = ""
   ' For i = leftX - 1 To leftX + picTilesX * 2 + 2
   '     Label7(i Mod 2) = Label7(i Mod 2) & i
   '     Label7((i + 1) Mod 2) = Label7((i + 1) Mod 2) & "  "
   ' Next
   '
   ' Label8.Caption = ""
   ' For i = topY - 1 To topY + picTilesY * 2 + 2
   '     Label8 = Label8 & i & vbCrLf
   ' Next


    
End Sub

Private Sub FillSpecial()
    Text7.Text = ""
    For i = 1 To maxSpecial
        If special(i) > 0 Then
            Text7.Text = Text7.Text & "Store #" & i & ": Type " & special(i)
            Text7.Text = Text7.Text & vbCrLf & "    At Point: (" & specialx(i) & ", " & specialy(i) & ")"
            Text7.Text = Text7.Text & vbCrLf & "    Data: " & specialdata(i)
            Text7.Text = Text7.Text & vbCrLf & "          "
    
            For j = 1 To Len(RTrim(specialdata(i)))
                Text7.Text = Text7.Text & Hex$(Asc(Mid(specialdata(i), j, 1))) & "  "
            Next
    
            Text7.Text = Text7.Text & vbCrLf & vbCrLf
        End If
    
        If specialx(i) = x1 And specialy(i) = y1 And stopchecking = False Then
            cmdSpecial.Caption = "Del Store"
            stopchecking = True
        ElseIf stopchecking = False Then
            cmdSpecial.Caption = "Place Store"
        End If
    Next

End Sub

Private Sub lblX_Click()
    x1 = InputBox("Enter X:")
    
    blit
End Sub

Private Sub lblY_Click()
    y1 = InputBox("Enter Y:")
    
    blit
End Sub

Private Sub mnuNew_Click()
    Dim w As Integer
    Dim h As Integer
    
    w = InputBox("Enter map width:")
    h = InputBox("Enter map height:")
    
    NewMap w, h

End Sub

Private Sub NewMap(w As Integer, h As Integer)
    mapWidth = w
    mapHeight = h
    
    sbRight.Max = w
    sbRight.Min = 1
    sbRight.Value = 1
    
    sbDown.Max = h
    sbDown.Min = 1
    sbDown.Value = 1
    
    For i = 0 To mapWidth
        For j = 0 To mapHeight
            map(i, j) = 0
        Next
    Next

    For i = 0 To 100
        guard(i).X = 0
        guard(i).Y = 0
    Next
    
    mapType = 3
    fileOffset = 64
    
End Sub

Private Sub mnuProperties_Click()
    frmProperties.txtName = mapName
    frmProperties.txtDefaultTile = defaultTile
    frmProperties.txtFileOffset = fileOffset
    frmProperties.txtMapType = mapType
    frmProperties.txtAttack = guardAttack
    frmProperties.txtHP = guardHP
    frmProperties.txtDefense = guardDefense
    frmProperties.txtColor = guardColor
    frmProperties.txtDefaultTile = defaultTile
    
    frmProperties.Show 1
    
    mapName = frmProperties.txtName
    defaultTile = frmProperties.txtDefaultTile
    fileOffset = frmProperties.txtFileOffset
    mapType = frmProperties.txtMapType
    guardAttack = frmProperties.txtAttack
    guardHP = frmProperties.txtHP
    guardDefense = frmProperties.txtDefense
    guardColor = frmProperties.txtColor
    
    
End Sub

Private Sub mnuQuit_Click()
    End
End Sub


Private Sub mnuSaveAs_Click()
    cmdDialog.DialogTitle = "Save As"
    cmdDialog.DefaultExt = ""
    cmdDialog.InitDir = "C:\my documents\programming\c++\lota\included maps"
    
    cmdDialog.ShowSave
    
    Path = cmdDialog.filename
    
    file = FreeFile
    
    filename = Path
    
    mnuSave_Click
    

End Sub

Private Sub Picture1_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
    xx = X
    yy = Y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
    
    If Button = 1 Then
       
        x1 = xx + leftX
        y1 = yy + topY
        
        x2 = x1
        y2 = y1
        
           
    ElseIf Button = 2 And xx + leftX >= 0 And yy + topY >= 0 Then
        
        If (chkRandom.Value = 0) Then
            map(xx + leftX, yy + topY) = currentTile
        Else
            If currentTile = 7 Then
                r = Int(Rnd(1) * 4)
                
                If r < 2 Then tile = currentTile + r
                If r > 1 Then tile = currentTile + r + 14
                
                map(xx + leftX, yy + topY) = tile
            ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                r = Int(Rnd(1) * 2)
                
                tile = currentTile + r
                
                map(xx + leftX, yy + topY) = tile
                
            Else
                map(xx + leftX, yy + topY) = currentTile
            End If
        End If
        
    End If
    
    
    blit
End Sub

Private Sub Picture1_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
    xx = X
    yy = Y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
    
    If Button = 1 Then
        x2 = xx + leftX
        y2 = yy + topY
        
        blit
    ElseIf Button = 2 And xx + leftX >= 0 And yy + topY >= 0 Then
        
        If (chkRandom.Value = 0) Then
            map(xx + leftX, yy + topY) = currentTile
        Else
            If currentTile = 7 Then
                r = Int(Rnd(1) * 4)
                
                If r < 2 Then tile = currentTile + r
                If r > 1 Then tile = currentTile + r + 14
                
                map(xx + leftX, yy + topY) = tile
            ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                r = Int(Rnd(1) * 2)
                
                tile = currentTile + r
                
                map(xx + leftX, yy + topY) = tile
                
            Else
                map(xx + leftX, yy + topY) = currentTile
            End If
        End If
        
        blit
    End If
    
End Sub

Private Sub Picture1_Paint()
    blit
End Sub

Private Sub Picture2_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
    xx = X
    yy = Y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
 
    tile = (yy * 16 + xx)
    
    If Button = 2 Then
        map(x1, y1) = tile
    End If
    
    currentTile = tile
    
    blit
End Sub

Private Sub picture2_paint()
    blit
End Sub

Private Sub Text4_Change()

End Sub
Private Sub mnuSave_Click()
    On Local Error Resume Next
    
    Dim mn As String * 16
    
    mn = mapName
    
    If filename = "" Then
        mnuSaveAs_Click
    End If
    
    If filename <> "" Then
        Path = filename
        
        file = FreeFile
        
        
        Kill Path
        Open Path For Binary As #file
                
        offset = 1
        
        Dim a As String * 1
        
        a = Chr(Int(mapWidth / 256))                        '0
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(mapWidth Mod 256))                      '1
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(mapHeight / 256))                       '2
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(mapHeight Mod 256))                     '3
        Put #file, offset, a: offset = offset + 1
                
        a = Chr(Int(fileOffset / 256))                      '4
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(fileOffset Mod 256))                    '5
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(mapType)                                    '6
        Put #file, offset, a: offset = offset + 1
                
        Put #file, offset, mn: offset = offset + Len(mn)    '7

        Put #file, offset, "@": offset = offset + 1         '23
        
        Put #file, offset, defaultTile: offset = offset + 1 '24
        
        a = Chr(Int(guardHP / 256))
        Put #file, offset, a: offset = offset + 1           '25
        
        a = Chr(Int(guardHP Mod 256))
        Put #file, offset, a: offset = offset + 1           '26
        
        a = Chr(Int(guardAttack / 256))
        Put #file, offset, a: offset = offset + 1           '27
        
        a = Chr(Int(guardAttack Mod 256))
        Put #file, offset, a: offset = offset + 1           '28
        
        a = Chr(Int(guardDefense / 256))
        Put #file, offset, a: offset = offset + 1           '29
        
        a = Chr(Int(guardDefense Mod 256))
        Put #file, offset, a: offset = offset + 1           '30
        
        a = Chr(Int(guardColor / 256))
        Put #file, offset, a: offset = offset + 1           '31
        
        a = Chr(Int(guardColor Mod 256))
        Put #file, offset, a: offset = offset + 1           '32
        
        offset = fileOffset + 1
        
        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
            
                Put #file, offset, map(i, j)
                offset = offset + 1
                
            Next
        Next
        
        'offset = (mapHeight + 1) * mapWidth + 1
        
        For i = 1 To maxSpecial
            Put #file, offset, special(i): offset = offset + 1
            
            a = Chr(Int(specialx(i) / 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialx(i) Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialy(i) / 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialy(i) Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            Put #file, offset, specialdata(i): offset = offset + Len(specialdata(i))
        Next
        
        Put #file, offset, "5555557": offset = offset + Len("5555557")
        
        For i = 0 To 100
            a = Chr(Int(guard(i).X / 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(guard(i).X Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(guard(i).Y / 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(guard(i).Y Mod 256))
            Put #file, offset, a: offset = offset + 1
        Next
        
        Close file
    End If
    
    blit

End Sub

Private Sub mnuOpen_Click()
    
    cmdDialog.DialogTitle = "Hi"
    
    cmdDialog.Filter = "Map Files (*.map)"
    cmdDialog.InitDir = App.Path & "\..\Included Maps"
    
    cmdDialog.DefaultExt = "map"
    
    cmdDialog.ShowOpen
    
    Path = cmdDialog.filename
    
    file = FreeFile
    
    If Path <> "" Then
        Open Path For Binary As #file
        
        Dim a As String * 1
        
        Get #file, 1, a
        mapWidth = Asc(a) * 256
        
        Get #file, 2, a
        mapWidth = mapWidth + Asc(a)
        
        Get #file, 3, a
        mapHeight = Asc(a) * 256
        
        Get #file, 4, a
        mapHeight = mapHeight + Asc(a)
        
        Get #file, 5, a
        newOffset = Asc(a) * 256
        
        Get #file, 6, a
        newOffset = newOffset + Asc(a)
        
        Get #file, 7, a
        mapType = Asc(a)

        
        offset = 8
        
        tempname = ""
        Do
            Get #file, offset, a
            tempname = tempname & a
            offset = offset + 1
        Loop Until a = "@" Or offset > 25
        
        mapName = RTrim(Left(tempname, Len(tempname) - 1))
        
        Get #file, 25, a
        defaultTile = Asc(a)

        Get #file, 26, a: offset = offset + 1           '25
        guardHP = Asc(a) * 256
        
        Get #file, 27, a: offset = offset + 1           '26
        guardHP = guardHP + Asc(a)
        
        Get #file, 28, a: offset = offset + 1           '27
        guardAttack = Asc(a) * 256
        
        Get #file, 29, a: offset = offset + 1           '28
        guardAttack = guardAttack + Asc(a)
        
        Get #file, 30, a: offset = offset + 1           '29
        guardDefense = Asc(a) * 256
                
        Get #file, 31, a: offset = offset + 1           '30
        guardDefense = guardDefense + Asc(a)
        
        Get #file, 32, a: offset = offset + 1           '31
        guardColor = Asc(a) * 256
        
        Get #file, 33, a: offset = offset + 1           '32
        guardColor = guardColor + Asc(a)

        offset = newOffset + 1
        
        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
            
                Get #file, offset, a
                
                map(i, j) = Asc(a)
                
                offset = offset + 1
                
            Next
        Next
                
        For i = 1 To maxSpecial
            Get #file, offset, a: offset = offset + 1
            special(i) = Asc(a)
            
            Get #file, offset, a: offset = offset + 1
            specialx(i) = Asc(a) * 256
            
            Get #file, offset, a: offset = offset + 1
            specialx(i) = specialx(i) + Asc(a)
            
            Get #file, offset, a: offset = offset + 1
            specialy(i) = Asc(a)
            
            Get #file, offset, a: offset = offset + 1
            specialy(i) = specialy(i) + Asc(a)
            
            Get #file, offset, specialdata(i): offset = offset + Len(specialdata(i))
        Next
        
        Do Until Right(b$, 7) = "5555557" Or EOF(file)
            Get #file, offset, a: offset = offset + 1
            
            b$ = b$ + a
        Loop
        
        For i = 0 To 100
            Get #file, offset, a: offset = offset + 1
            guard(i).X = Asc(a) * 256
            Get #file, offset, a: offset = offset + 1
            guard(i).X = guard(i).X + Asc(a)
        
            Get #file, offset, a: offset = offset + 1
            guard(i).Y = Asc(a) * 256
            Get #file, offset, a: offset = offset + 1
            guard(i).Y = guard(i).Y + Asc(a)
        Next

        Close file
        
        filename = Path
        
        If x1 > mapWidth Or y1 > mapHeight Then
            x1 = mapWidth / 2
            y1 = mapHeight / 2
        End If
            
    End If
    
    FillSpecial
    blit
End Sub

Private Sub sbDown_Change()
    dispY = sbDown.Value
    blit
End Sub

Private Sub sbRight_Change()
    dispX = sbRight.Value
    blit
End Sub
