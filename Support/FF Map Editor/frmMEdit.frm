VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmMEdit 
   Caption         =   "Form1"
   ClientHeight    =   9465
   ClientLeft      =   90
   ClientTop       =   660
   ClientWidth     =   11385
   BeginProperty Font 
      Name            =   "Times New Roman"
      Size            =   12
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmMEdit.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   631
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   759
   Begin MSComctlLib.StatusBar StatusBar1 
      Align           =   2  'Align Bottom
      Height          =   375
      Left            =   0
      TabIndex        =   27
      Top             =   9090
      Width           =   11385
      _ExtentX        =   20082
      _ExtentY        =   661
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   1
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
            AutoSize        =   2
         EndProperty
      EndProperty
   End
   Begin VB.Frame frmBottom 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   4215
      Left            =   0
      TabIndex        =   20
      Top             =   5160
      Width           =   11295
      Begin MSComCtl2.FlatScrollBar sbSpecial 
         Height          =   255
         Left            =   8040
         TabIndex        =   29
         Top             =   1200
         Width           =   3135
         _ExtentX        =   5530
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   0
         Arrows          =   65536
         Orientation     =   1179649
      End
      Begin VB.CommandButton cmdFill 
         Caption         =   "Fill Selection"
         Height          =   735
         Left            =   7920
         TabIndex        =   28
         Top             =   120
         Width           =   1095
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
         Left            =   240
         ScaleHeight     =   256
         ScaleMode       =   3  'Pixel
         ScaleWidth      =   256
         TabIndex        =   23
         Top             =   240
         Width           =   3900
      End
      Begin VB.TextBox Text7 
         Height          =   3735
         Left            =   4320
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   3  'Both
         TabIndex        =   22
         Top             =   480
         Width           =   3375
      End
      Begin VB.CheckBox chkRandom 
         Caption         =   "Randomize Tiles"
         Height          =   375
         Left            =   4320
         TabIndex        =   21
         Top             =   0
         Width           =   2175
      End
      Begin VB.Label lblSpcCount 
         Caption         =   "Specials: 0"
         Height          =   255
         Left            =   8040
         TabIndex        =   31
         Top             =   1920
         Width           =   2775
      End
      Begin VB.Label lblFindSpecial 
         Caption         =   "Find Special:"
         Height          =   375
         Left            =   8040
         TabIndex        =   30
         Top             =   1560
         Width           =   2175
      End
      Begin VB.Label Label5 
         BeginProperty Font 
            Name            =   "Courier"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   3855
         Left            =   0
         TabIndex        =   25
         Top             =   240
         Width           =   255
      End
      Begin VB.Label Label4 
         Caption         =   " 0    1   2   3    4   5   6    7   8   9   A    B   C   D   E   F"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   240
         TabIndex        =   24
         Top             =   0
         Width           =   4215
      End
   End
   Begin VB.Frame frmRight 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   5175
      Left            =   5400
      TabIndex        =   3
      Top             =   0
      Width           =   5775
      Begin VB.CommandButton cmdPlaceSpecial 
         Caption         =   "Place &Special"
         Height          =   495
         Left            =   120
         TabIndex        =   13
         Top             =   240
         Width           =   1575
      End
      Begin VB.CommandButton cmdObject 
         Caption         =   "Place &Object"
         Height          =   405
         Left            =   120
         TabIndex        =   12
         Top             =   2280
         Width           =   1575
      End
      Begin VB.CommandButton cmdGuard 
         Caption         =   "Place &Guard"
         Height          =   405
         Left            =   120
         TabIndex        =   11
         Top             =   1800
         Width           =   1575
      End
      Begin VB.CommandButton cmdModifySpecial 
         Caption         =   "Modify &Special"
         Enabled         =   0   'False
         Height          =   495
         Left            =   120
         TabIndex        =   10
         Top             =   720
         Width           =   1575
      End
      Begin VB.CommandButton cmdDeleteSpecial 
         Caption         =   "&Delete Special"
         Enabled         =   0   'False
         Height          =   495
         Left            =   120
         TabIndex        =   9
         Top             =   1200
         Width           =   1575
      End
      Begin VB.ListBox lstPreDef 
         Height          =   2340
         Left            =   1800
         TabIndex        =   8
         Top             =   2520
         Width           =   2415
      End
      Begin VB.CommandButton cmdRoof 
         Caption         =   "&Roof"
         Height          =   375
         Left            =   120
         TabIndex        =   7
         Top             =   2760
         Width           =   1575
      End
      Begin VB.CheckBox chkDrawRoof 
         Caption         =   "Show Roof"
         Height          =   375
         Left            =   120
         TabIndex        =   6
         Top             =   3240
         Width           =   1575
      End
      Begin VB.CheckBox chkRestrict 
         Caption         =   "Restrict Drawing"
         Height          =   525
         Left            =   120
         TabIndex        =   5
         ToolTipText     =   "Restricts tiles drawn to selection"
         Top             =   4080
         Width           =   1455
      End
      Begin VB.CheckBox chkDrawGuards 
         Caption         =   "Show Guards"
         Height          =   285
         Left            =   120
         TabIndex        =   4
         Top             =   3600
         Value           =   1  'Checked
         Width           =   1575
      End
      Begin VB.Label lblImport 
         Caption         =   "Import:"
         BeginProperty Font 
            Name            =   "Courier"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   1800
         TabIndex        =   26
         Top             =   1200
         Width           =   3855
      End
      Begin VB.Label lblDim 
         Caption         =   "Map Dimensions"
         Height          =   375
         Left            =   1800
         TabIndex        =   19
         Top             =   120
         Width           =   4080
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
         Height          =   255
         Left            =   1800
         TabIndex        =   18
         Top             =   480
         Width           =   4080
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
         Height          =   255
         Left            =   1800
         TabIndex        =   17
         Top             =   720
         Width           =   3840
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
         Height          =   255
         Left            =   1800
         TabIndex        =   16
         Top             =   960
         Width           =   3720
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
         Height          =   255
         Left            =   1800
         TabIndex        =   15
         Top             =   1440
         Width           =   3840
      End
      Begin VB.Label Label3 
         Caption         =   "Pre-defined objects:"
         Height          =   255
         Left            =   1800
         TabIndex        =   14
         Top             =   2160
         Width           =   1935
      End
   End
   Begin MSComCtl2.FlatScrollBar sbDown 
      Height          =   4815
      Left            =   5040
      TabIndex        =   2
      Top             =   0
      Width           =   255
      _ExtentX        =   450
      _ExtentY        =   8493
      _Version        =   393216
      Appearance      =   0
      Orientation     =   1179648
   End
   Begin MSComCtl2.FlatScrollBar sbRight 
      Height          =   255
      Left            =   0
      TabIndex        =   1
      Top             =   4800
      Width           =   5055
      _ExtentX        =   8916
      _ExtentY        =   450
      _Version        =   393216
      Appearance      =   0
      Arrows          =   65536
      Orientation     =   1179649
   End
   Begin MSComDlg.CommonDialog cmdDialog 
      Left            =   1440
      Top             =   -120
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      CancelError     =   -1  'True
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
      Height          =   4800
      Left            =   0
      ScaleHeight     =   316
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   332
      TabIndex        =   0
      Top             =   0
      Width           =   5040
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
      Begin VB.Menu mnuImport 
         Caption         =   "&Import..."
      End
      Begin VB.Menu mnuFileSep0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save"
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuSaveAs 
         Caption         =   "S&ave as..."
      End
      Begin VB.Menu mnuFinalize 
         Caption         =   "&Finalize && Save..."
      End
      Begin VB.Menu mnuSep0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuQuit 
         Caption         =   "&Quit"
      End
   End
   Begin VB.Menu mnuOptions 
      Caption         =   "&Map"
      Begin VB.Menu mnuProperties 
         Caption         =   "&Properties..."
         Shortcut        =   ^P
      End
      Begin VB.Menu mnuSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuPlaceSpecial 
         Caption         =   "Place &Special"
      End
      Begin VB.Menu mnuModifySpecial 
         Caption         =   "Modify &Special"
      End
      Begin VB.Menu mnuDeleteSpecial 
         Caption         =   "&Delete Special"
      End
      Begin VB.Menu mnuSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuRefreshTiles 
         Caption         =   "&Refresh Tiles"
         Shortcut        =   +{F5}
      End
   End
   Begin VB.Menu mnuTitleImport 
      Caption         =   "&Import"
      Begin VB.Menu mnuImportRefresh 
         Caption         =   "&Refresh"
         Shortcut        =   ^R
      End
      Begin VB.Menu mnuParameters 
         Caption         =   "&Parameters..."
      End
      Begin VB.Menu mnuImportSep0 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSaveMapping 
         Caption         =   "&Save Mappings..."
      End
      Begin VB.Menu mnuLoadMapping 
         Caption         =   "&Load Mappings..."
      End
   End
End
Attribute VB_Name = "frmMEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private oldw, oldh
Private offset
Private validMap As Boolean

Private Sub chkDrawGuards_Click()
    blit
End Sub

Private Sub chkDrawRoof_Click()
    blit
End Sub

Private Sub cmdDeleteSpecial_Click()
    Dim i As Integer
    
    For i = 1 To maxSpecial
        If specialx(i) = x1 And specialy(i) = y1 Then
            specialx(i) = 0
            specialy(i) = 0
            specialwidth(i) = 0
            specialheight(i) = 0
            special(i) = 0
            
        End If
    Next
       
    FillSpecial
    blit
    
    SetPos x1, y1
    

End Sub

Private Sub cmdFill_Click()
    Dim xdif, ydif
    Dim tile, r
    Dim i, j
    
    xdif = x2 - x1
    ydif = x2 - x1
    
    If (xdif = 0) Then
        xdif = xdif + 1
    End If
    
    If ydif = 0 Then
        ydif = ydif + 1
    End If

    tile = currentTile
    If chkRandom.value = 1 Then
        If currentTile = 7 Then
            r = Int(Rnd(1) * 4)
            
            If r < 2 Then tile = currentTile + r
            If r > 1 Then tile = currentTile + r + 14
            
        ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
            r = Int(Rnd(1) * 2)
            
            tile = currentTile + r
            
        End If
    End If
    
    For i = x1 To x2 Step Sgn(xdif)
        For j = y1 To y2 Step Sgn(ydif)
            If chkRandom.value = 1 Then
                If currentTile = 7 Then
                    r = Int(Rnd(1) * 4)
                    
                    If r < 2 Then tile = currentTile + r
                    If r > 1 Then tile = currentTile + r + 14
                    
                ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                    r = Int(Rnd(1) * 2)
                    
                    tile = currentTile + r
                    
                End If
            End If

            PaintLoc i, j, tile

        Next
    Next
    
    blit
        
End Sub

Private Sub cmdGuard_Click()
    Dim i As Integer
    Dim found As Boolean
    
    For i = 0 To 100
        If guard(i).x = x1 And guard(i).y = y1 Then
            guard(i).x = 0
            guard(i).y = 0
            found = True
        End If
    Next
    
    If found <> True Then
        For i = 0 To 100
            If guard(i).x = 0 And guard(i).y = 0 Then
                guard(i).x = x1
                guard(i).y = y1
                
                blit
                Exit Sub
                
            End If
        Next
    End If
    
    blit
End Sub

Private Sub cmdModifySpecial_Click()
    Dim i, j, s
    
    
    For i = 1 To maxSpecial
        If specialx(i) = x1 And specialy(i) = y1 Then
            frmSpecial.Changing = True
            
            frmSpecial.sData = specialdata(i)
            frmSpecial.txtLocX = specialx(i)
            frmSpecial.txtLocY = specialy(i)
            frmSpecial.sType = special(i)
            frmSpecial.setProperties = True
            frmSpecial.txtSpcWidth = specialwidth(i)
            frmSpecial.txtSpcHeight = specialheight(i)
            
            frmSpecial.Changing = False
            
            s = i
        End If
    Next
    
    frmSpecial.show 1
    
    If SelectedOK Then
        special(s) = frmSpecial.sType
        specialx(s) = frmSpecial.txtLocX
        specialy(s) = frmSpecial.txtLocY
        specialdata(s) = frmSpecial.sData
        specialwidth(s) = frmSpecial.txtSpcWidth
        specialheight(s) = frmSpecial.txtSpcHeight
        
    End If
    
    SetPos x1, y1
    
    FillSpecial

End Sub

Private Sub cmdObject_Click()

    Dim Index As Integer
    Dim i, j
    
    Index = lstPreDef.ListIndex

    For j = 0 To PreDefObjects(Index).height - 1
        For i = 0 To PreDefObjects(Index).width - 1
            If PreDefObjects(Index).Matrix(i, j) > -1 Then
                PaintLoc x1 + i, y1 + j, PreDefObjects(Index).Matrix(i, j)
            End If
        Next
    Next
    
    blit
End Sub

Private Sub cmdPlaceSpecial_Click()
    
    Dim sx, sy, i, j
    
    frmSpecial.SetDefaults
    
    frmSpecial.txtLocX = x1
    frmSpecial.txtLocY = y1
    frmSpecial.txtSpcWidth = x2 - x1 + 1
    frmSpecial.txtSpcHeight = y2 - y1 + 1
    
    frmSpecial.show 1
    
    If SelectedOK Then
        For i = 1 To maxSpecial
            If special(i) = 0 Then
                
                special(i) = frmSpecial.sType
                
                specialx(i) = frmSpecial.txtLocX
                specialy(i) = frmSpecial.txtLocY
                
                specialwidth(i) = frmSpecial.txtSpcWidth
                specialheight(i) = frmSpecial.txtSpcHeight
                
                specialdata(i) = frmSpecial.sData
                
                Exit For
            End If
        Next
        
        SetPos x1, y1
        
        FillSpecial
    End If
    
    blit
    
End Sub

Private Sub cmdRoof_Click()
    Dim i, j
    Dim found As Boolean
    
    frmRoof.RoofIndex = 1
    
    For i = 1 To maxRoofs
        If x1 >= Roofs(i).anchorTarget.x - Roofs(i).anchor.x And _
           y1 >= Roofs(i).anchorTarget.y - Roofs(i).anchor.y And _
           x1 < Roofs(i).anchorTarget.x - Roofs(i).anchor.x + Roofs(i).width And _
           y1 < Roofs(i).anchorTarget.y - Roofs(i).anchor.y + Roofs(i).height _
           Then
            found = True
            frmRoof.RoofIndex = i
            'frmRoof.txtAnchorX = Roofs(i).anchor.X
            'frmRoof.txtAnchorY = Roofs(i).anchor.Y
            'frmRoof.txtTargetX = Roofs(i).anchorTarget.X
            'frmRoof.txtTargetY = Roofs(i).anchorTarget.Y
            'frmRoof.txtRoofX = Roofs(i).width
            'frmRoof.txtRoofY = Roofs(i).height
            frmRoof.SetControls
            
            Exit For
        End If
    Next
    
    If found = False Then
        For i = 1 To maxRoofs
            If Roofs(i).anchorTarget.x = 0 And Roofs(i).anchorTarget.y = 0 Then
        
                frmRoof.RoofIndex = i
                Roofs(i).width = x2 - x1 + 1
                Roofs(i).height = y2 - y1 + 1
                Roofs(i).anchorTarget.x = x1
                Roofs(i).anchorTarget.y = y1
                frmRoof.chkDrawGround = 1
                frmRoof.SetControls
                
                Exit For
            End If
        Next
    End If
    
    CreateBackBuffer frmRoof.Picture1.width, frmRoof.Picture1.height
    
    frmRoof.show 1
    
    CreateBackBuffer Picture1.width, Picture1.height
    
    
End Sub

Private Sub cmdSetCorner_Click()
    Dim i
    
    For i = 0 To 1
        If optCorner(i).value = True Then
            txtCX(i) = x1
            txtCY(i) = y1
        End If
    Next
    
End Sub


Private Sub Form_Activate()
    UpdateScreen = True
    
End Sub

Private Sub Form_Deactivate()
    UpdateScreen = False
End Sub

Private Sub Form_Load()
    Dim i As Integer
    
    For i = 0 To 15
        Label5 = Label5 & Hex$(i) & vbCrLf
    Next
    
    CreateSurfaces Picture1.width, Picture1.height
    
    Do While Not validMap
    
        frmStartup.show 1
        
        If StartNewMap Then
            AssignProperties
            NewMap False
            
        ElseIf ImportMap Then
            ImportNewMap
            
        Else
            OpenMap
            
        End If
    
    Loop
    
    x1 = 0
    x2 = 0
    y1 = 0
    y2 = 0
    
    blit
End Sub

Private Sub Form_Paint()
    blit
End Sub

Sub blit()
    Dim ddrval As Long
    Dim r1 As RECT
    Dim r2 As RECT
    
    If UpdateScreen = True Then
    
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
        
    End If
    

End Sub

Private Sub draw()
    Dim ddrval As Long
    Dim r1 As RECT
    Dim r2 As RECT
    Dim j, i, a, tilex, tiley, k
    Dim xx, yy, centerx, centery, t
    
    
    r1.Bottom = backHeight
    r1.Right = backWidth
    
    If DDSBack.isLost Then
        DDSBack.restore
        DDSPrimary.restore
        DDSTiles.restore
        DDSChar.restore
        DDSTemp.restore
    End If
    
    DDSBack.BltColorFill r1, &H555555
    
    'On Local Error Resume Next
    
    lblDim = "Map Dimensions: " & mapWidth & " x " & mapHeight
    If fileName <> "" Then
        Me.Caption = "LotA Town Editor - " & fileName
    End If
    
    picTilesX = Int(Picture1.ScaleWidth / TileSize / 2)
    picTilesY = Int(Picture1.ScaleHeight / TileSize / 2)
        
    xx = 0
    yy = 0
    
    sbRight.max = mapWidth
    sbRight.min = 0
    sbRight.LargeChange = picTilesX * 2 - 2
    sbDown.LargeChange = picTilesY * 2 - 2
    sbDown.max = mapHeight
    sbDown.min = 0
    
    centerx = sbRight.value
    centery = sbDown.value
    
    leftX = centerx - picTilesX
    topy = centery - picTilesY
    
    sbSpecial.min = 0
    sbSpecial.max = NumSpecials
    sbSpecial.LargeChange = 5
    sbSpecial.SmallChange = 1
    
    lblSpcCount = "Specials: " & sbSpecial.max & " count."
    
    For j = dispY - picTilesY To dispY + picTilesY + 1
        For i = dispX - picTilesX To dispX + picTilesX + 1
            If i >= 0 And i < mapWidth And j >= 0 And j < mapHeight Then
            
                a = Map(i, j)
                
                If chkDrawRoof.value <> 0 Then
                    For k = 1 To maxRoofs
                        If (i >= Roofs(k).anchorTarget.x - Roofs(k).anchor.x And _
                           j >= Roofs(k).anchorTarget.y - Roofs(k).anchor.y) And _
                           (i < Roofs(k).anchorTarget.x - Roofs(k).anchor.x + Roofs(k).width And _
                           j < Roofs(k).anchorTarget.y - Roofs(k).anchor.y + Roofs(k).height) _
                           Then
                            
                            t = Roofs(k).Matrix(i - Roofs(k).anchorTarget.x + Roofs(k).anchor.x, _
                                        j - Roofs(k).anchorTarget.y + Roofs(k).anchor.y)
                            
                            If t <> 127 Then a = t
                            
                        End If
                    Next
                
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
    
    If chkDrawGuards.value And (MapType = maptown Or MapType = mapCastle) Then
        For i = 0 To 100
            If guard(i).x >= leftX And guard(i).x < leftX + 2 * picTilesX + 1 And _
                guard(i).y >= topy And guard(i).y < topy + 2 * picTilesY + 1 And _
                guard(i).x > 0 And guard(i).y > 0 Then
                
                r1.Top = 5 * 32
                r1.Left = 0 * 32
                r1.Right = r1.Left + 64
                r1.Bottom = r1.Top + 32
                
                r2.Left = (guard(i).x - leftX) * TileSize
                r2.Top = (guard(i).y - topy) * TileSize
                r2.Right = r2.Left + 32
                r2.Bottom = r2.Top + 32
                
                Dim ret As Long
                
                ret = DDSBack.Blt(r2, DDSChar, r1, DDBLT_WAIT Or DDBLT_KEYSRC)
                
            End If
        Next
    End If
    
    DDSBack.SetForeColor vbCyan
    
    For i = 1 To maxSpecial
        If specialx(i) >= leftX Or specialx(i) + specialwidth(i) < leftX + 2 * picTilesX + 1 Or _
            specialy(i) >= topy Or specialy(i) + specialheight(i) < topy + 2 * picTilesY + 1 Or _
            special(i) > 0 Then
            
            r1.Left = (specialx(i) - leftX) * TileSize
            r1.Right = r1.Left + specialwidth(i) * TileSize
            r1.Top = (specialy(i) - topy) * TileSize
            r1.Bottom = r1.Top + specialheight(i) * TileSize
            
            DDSBack.DrawBox r1.Left, r1.Top, r1.Right, r1.Bottom
        End If
    Next
    
    DDSBack.SetForeColor vbYellow
    
    If MapType <> mapOutside Then
        ''''''''''''''''''''''''''
        ''  Draw Roofs
        For i = 1 To maxRoofs
            If (leftX < Roofs(i).anchorTarget.x - Roofs(i).anchor.x Or _
               topy < Roofs(i).anchorTarget.y - Roofs(i).anchor.y) Or _
               (leftX + picTilesX * 2 > Roofs(i).anchorTarget.x - Roofs(i).anchor.x + Roofs(i).width Or _
               topy + picTilesY * 2 > Roofs(i).anchorTarget.y - Roofs(i).anchor.y + Roofs(i).height) _
               Then
    
                r1.Left = (Roofs(i).anchorTarget.x - leftX) * TileSize
                r1.Right = r1.Left + TileSize
                r1.Top = (Roofs(i).anchorTarget.y - topy) * TileSize
                r1.Bottom = r1.Top + TileSize
                
                DDSBack.DrawBox r1.Left, r1.Top, r1.Right, r1.Bottom
    
                r1.Left = (Roofs(i).anchorTarget.x - Roofs(i).anchor.x - leftX) * TileSize
                r1.Right = r1.Left + Roofs(i).width * TileSize
                r1.Top = (Roofs(i).anchorTarget.y - Roofs(i).anchor.y - topy) * TileSize
                r1.Bottom = r1.Top + Roofs(i).height * TileSize
                
                DDSBack.DrawBox r1.Left, r1.Top, r1.Right, r1.Bottom
              
            End If
        Next
    End If
    
    DDSBack.SetForeColor vbWhite
    DDSBack.DrawBox (x1 - leftX) * TileSize, (y1 - topy) * TileSize, (x2 - leftX + 1) * TileSize, (y2 - topy + 1) * TileSize
    
    lblx.Caption = "x1:   " & x1 & "  0x" & Hex$(x1) & "    x2: " & x2
    lblY.Caption = "y1:   " & y1 & "  0x" & Hex$(y1) & "    y2: " & y2
    
    If x1 < 0 Or x1 > mapWidth Or y1 < 0 Or y1 > mapHeight Then
        lblTile.Caption = "Tile: Out of range"
    Else
        lblTile.Caption = "Tile: " & Map(x1, y1) & "   0x" & Hex$(Map(x1, y1))
    End If
    
    If ImportMap Then
        If ImportLocation(x1, y1) >= 0 And ImportLocation(x1, y1) <= UBound(ImportedData) Then
            lblImport.Caption = "Import: " & ImportedData(ImportLocation(x1, y1))
        Else
            lblImport.Caption = "Import: Out of Range"
        End If
    Else
        lblImport.Caption = ""
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

    Select Case MapType
    Case mapOutside
        cmdGuard.Visible = False
    Case mapDungeon
        cmdGuard.Visible = False
    Case mapMuseum
        cmdGuard.Visible = False
    Case maptown
        cmdGuard.Visible = True
    Case mapCastle
        cmdGuard.Visible = True
    End Select
    
End Sub

Private Sub FillSpecial()
    Dim i As Integer, j As Integer
    Dim stopChecking As Boolean
    
    Text7.Text = ""
    For i = 1 To maxSpecial
        If special(i) > 0 Then
            If specialx(i) = 0 And specialy(i) = 0 Then
                specialheight(i) = 0
                specialwidth(i) = 0
            End If
            
            Text7.Text = Text7.Text & "Store #" & i & ": Type " & special(i)
            Text7.Text = Text7.Text & vbCrLf & "    At Point: (" & specialx(i) & ", " & specialy(i) & ")"
            Text7.Text = Text7.Text & vbCrLf & "    Data: " & specialdata(i)
            Text7.Text = Text7.Text & vbCrLf & "          "
    
            For j = 1 To Len(RTrim(specialdata(i)))
                Text7.Text = Text7.Text & Hex$(Asc(Mid(specialdata(i), j, 1))) & "  "
            Next
            
            
            Text7.Text = Text7.Text & vbCrLf & vbCrLf
        End If
    
    Next
End Sub

Private Sub Form_Resize()
    frmRight.Left = Me.ScaleWidth - frmRight.width
    frmBottom.Top = Me.ScaleHeight - frmBottom.height - StatusBar1.height
    
    Picture1.width = frmRight.Left - sbDown.width - Picture1.Left
    Picture1.height = frmBottom.Top - sbRight.height - Picture1.Top
    
    sbDown.Left = Picture1.Left + Picture1.width
    sbRight.Top = Picture1.Top + Picture1.height
        
    sbDown.height = Picture1.height
    sbRight.width = Picture1.width
    
    CreateBackBuffer Picture1.width, Picture1.height
    
End Sub

Private Sub Form_Unload(Cancel As Integer)
    End
End Sub

Private Sub lblX_Click()
    Dim xx As Integer
    xx = InputBox("Enter X:")
    
    SetPos xx, y1
    
    blit
End Sub

Private Sub lblY_Click()
    Dim yy As Integer
    yy = InputBox("Enter Y:")
    
    SetPos x1, yy
    
    blit
End Sub


Private Sub mnuFinalize_Click()
    
    If MsgBox("Are you sure you wish to finalize this imported map?" & vbCrLf & vbCrLf & "You will have to edit it as a standard map from now on!", vbQuestion Or vbYesNo, "Finalize?") = vbYes Then
        SetPropertiesForm
        
        frmProperties.optType(MapType).value = True
        frmProperties.txtName = ""

        
        frmProperties.show 1
        
        If SelectedOK Then
            AssignProperties
            
            mnuSaveAs_Click
            ImportMap = False
            
            SetMenus
            
            UpdateScreen = True
            blit
        End If
            
    End If
    
End Sub

Private Sub mnuImport_Click()
    On Local Error GoTo ErrorHandler
    
    cmdDialog.DialogTitle = "Import Map"
    
    cmdDialog.fileName = "E:\Legacy\."
    cmdDialog.Filter = "Export Files|*.export|All Files (*.*)|*.*"
    cmdDialog.FilterIndex = 1
    
    cmdDialog.InitDir = App.path & "\..\Included Maps"
    
    cmdDialog.DefaultExt = "map"
    
    cmdDialog.ShowOpen
    
    fileName = cmdDialog.fileName
    
    StartNewMap = False
    ImportMap = True
            
    ImportNewMap
    
ErrorHandler:
    
End Sub

Private Sub mnuImportRefresh_Click()
    RecalibrateImport
    
End Sub

Private Sub mnuLoadMapping_Click()
    
    On Local Error GoTo ErrorHandler
    
    cmdDialog.DialogTitle = "Load Mapping"
    

    cmdDialog.Filter = "Mapping File (*.mpn)|*.mpn"
    cmdDialog.FilterIndex = 1
    cmdDialog.InitDir = App.path & "\."
    cmdDialog.fileName = ""
        
    cmdDialog.DefaultExt = "mpn"
    
    cmdDialog.ShowOpen
          
    LoadMapping cmdDialog.fileName
    
ErrorHandler:
End Sub

Private Sub mnuNew_Click()
        
    NewMap True

End Sub

Private Sub ImportNewMap()
    Dim fso As New FileSystemObject
    Dim theFile As file
    Dim fileStream As TextStream
    Dim fileNum As Integer
    Dim i As Integer
    
    fileNum = FreeFile
    
    Set theFile = fso.GetFile(fileName)
    ReDim ImportedData(theFile.Size)
    
    Open fileName For Binary As #fileNum
    
    For i = 0 To theFile.Size - 1
        Get #fileNum, , ImportedData(i)
    Next
    
    ResetImportDefinitions
    
    frmImport.SetDefaults
    frmImport.show 1
    
    If Not frmImport.ClickedOK Then Exit Sub
    
    validMap = True
    
    For i = 1 To maxRoofs
        Roofs(i).anchor.x = 0
        Roofs(i).anchor.y = 0
        Roofs(i).anchorTarget.x = 0
        Roofs(i).anchorTarget.y = 0
        Roofs(i).height = 0
        Roofs(i).width = 0
        
    Next
        
    For i = 0 To 100
        guard(i).x = 0
        guard(i).y = 0
    Next
    
    For i = 1 To maxSpecial
        special(i) = 0
        specialx(i) = 0
        specialy(i) = 0
        specialdata(i) = ""
        specialwidth(i) = 0
        specialheight(i) = 0
        
    Next
    
    SetMenus
    LoadTiles TileSet
    blit
End Sub

Private Sub NewMap(show As Boolean)
    Dim i, j, k
    
    If (show = True) Then
        frmProperties.SetDefaults
        frmProperties.Caption = "New Map"
        
        frmProperties.show 1
    Else
        frmProperties.SetDefaults
        
    End If
    
    If SelectedOK Or Not show Then
           
        For i = 0 To mapWidth
            For j = 0 To mapHeight
                PaintLoc i, j, 0
            Next
        Next
    
        sbRight.max = mapWidth
        sbRight.min = 0
        sbRight.value = 0
        
        sbDown.max = mapHeight
        sbDown.min = 0
        sbDown.value = 0
        
        For i = 0 To 100
            guard(i).x = 0
            guard(i).y = 0
        Next
        
        For i = 0 To maxSpecial
            special(i) = 0
            specialx(i) = 0
            specialy(i) = 0
            specialdata(i) = 0
            specialwidth(i) = 0
        Next
        
        NumRoofs = 0
        For i = 1 To maxRoofs
            Roofs(i).anchor.x = 0
            Roofs(i).anchor.y = 0
            Roofs(i).anchorTarget.x = 0
            Roofs(i).anchorTarget.y = 0
            Roofs(i).height = 0
            Roofs(i).width = 0
            
            For j = 0 To 100
                For k = 0 To 100
                    Roofs(i).Matrix(j, k) = 127
                Next
            Next
        Next
        
        LoadTiles TileSet
        
        validMap = True
    End If
    
    SetMenus
    
End Sub

Private Sub mnuParameters_Click()
    frmImport.show 1
    
    LoadTiles TileSet
    blit
End Sub

Private Sub mnuProperties_Click()
    SetPropertiesForm
    
    frmProperties.show 1
    
    If SelectedOK Then
        AssignProperties
    End If
        
End Sub

Private Sub mnuQuit_Click()
    End
End Sub


Private Sub mnuRefreshTiles_Click()
    LoadTiles TileSet
End Sub

Private Sub mnuSaveAs_Click()
    On Local Error GoTo ErrorHandler
    
    cmdDialog.DialogTitle = "Save Map"
    
    cmdDialog.Filter = "Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
    cmdDialog.FilterIndex = 2
    cmdDialog.InitDir = LotaPath & "\included maps\."
    cmdDialog.fileName = ""
    
    cmdDialog.ShowSave
    
    fileName = cmdDialog.fileName
    
    mnuSave_Click
    
ErrorHandler:
End Sub



Private Sub mnuSaveMapping_Click()
    On Local Error GoTo ErrorHandler
    
    cmdDialog.DialogTitle = "Save Mapping"
    
    cmdDialog.Filter = "Mapping File (*.mpn)|*.mpn"
    cmdDialog.FilterIndex = 1
    cmdDialog.InitDir = App.path & "\."
    cmdDialog.fileName = ""
    
    cmdDialog.ShowSave
    
    SaveMapping cmdDialog.fileName
    
ErrorHandler:
    
End Sub

Private Sub Picture1_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
    Dim xx, yy, r, tile, i
    
    xx = x
    yy = y
    
    xx = Int(xx / 16) + leftX
    yy = Int(yy / 16) + topy
    
    If UpdateScreen = False Then Exit Sub
    
    If Button = 1 Then
       
        SetPos xx, yy
        
    ElseIf Button = 2 And xx >= 0 And yy >= 0 Then
        Picture1_MouseMove Button, Shift, x, y
        
    End If
    

    blit
End Sub

Private Sub Picture1_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
    Dim xx, yy, r, tile
    
    xx = x
    yy = y
    
    xx = Int(xx / 16) + leftX
    yy = Int(yy / 16) + topy
    
    If UpdateScreen = False Then Exit Sub

    If Button = 1 Then
        
        SetRightPos xx, yy
        blit
        
    ElseIf Button = 2 And xx >= 0 And yy >= 0 Then
        
        If ImportMap Then
            PaintLoc xx, yy, currentTile
        Else
            If chkRestrict.value <> 0 Then
                If xx < x1 Or xx > x2 Or yy < y1 Or yy > y2 Then Exit Sub
            End If
            
            If (chkRandom.value = 0) Then
                PaintLoc xx, yy, currentTile
            Else
                If currentTile = 7 Then
                    r = Int(Rnd(1) * 4)
                    
                    If r < 2 Then tile = currentTile + r
                    If r > 1 Then tile = currentTile + r + 14
                    
                    PaintLoc xx, yy, tile
                ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                    r = Int(Rnd(1) * 2)
                    
                    tile = currentTile + r
                    
                    PaintLoc xx, yy, tile
                    
                Else
                    PaintLoc xx, yy, currentTile
                End If
            End If
        End If
        
        blit
    End If
    
End Sub

Private Sub Picture1_Paint()
    blit
End Sub

Private Sub Picture2_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
    Dim xx, yy, tile
    
    xx = x
    yy = y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
 
    tile = (yy * 16 + xx)
    
    If UpdateScreen = False Then Exit Sub

    If Button = 2 Then
        PaintLoc x1, y1, tile
    End If
    
    currentTile = tile
    
    blit
End Sub

Private Sub Picture2_Paint()
    blit
End Sub

Private Sub Text4_Change()

End Sub
Private Sub mnuSave_Click()
    
    
    If fileName = "" Then
        mnuSaveAs_Click
        
        If fileName = "" Then Exit Sub
        
    End If

    SaveMap
    
End Sub

Private Sub SaveMap()
    Dim path As String
    Dim offset As Long
    Dim file As Integer
    Dim mn As String * 16
    Dim i, j, k
    Dim test As String * 1
    
    mn = mapName
    
    On Local Error Resume Next
    
    If fileName <> "" Then
        path = fileName
        
        file = FreeFile
        
        
        Kill path
        Open path For Binary As #file
                
        offset = 1
        
        Dim a As String * 1
        
        a = Chr(Int(mapWidth \ 256))                        '0
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(mapWidth Mod 256))                      '1
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(mapHeight \ 256))                       '2
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(mapHeight Mod 256))                     '3
        Put #file, offset, a: offset = offset + 1
                
        a = Chr(Int(fileOffset \ 256))                      '4
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int(fileOffset Mod 256))                    '5
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(MapType)                                    '6
        Put #file, offset, a: offset = offset + 1
                
        Put #file, offset, mn: offset = offset + Len(mn)    '7

        Put #file, offset, "@": offset = offset + 1         '23
        
        Put #file, offset, defaultTile: offset = offset + 1 '24
        
        a = Chr(Int(guardHP \ 256))
        Put #file, offset, a: offset = offset + 1           '25
        
        a = Chr(Int(guardHP Mod 256))
        Put #file, offset, a: offset = offset + 1           '26
        
        a = Chr(Int(guardAttack \ 256))
        Put #file, offset, a: offset = offset + 1           '27
        
        a = Chr(Int(guardAttack Mod 256))
        Put #file, offset, a: offset = offset + 1           '28
        
        a = Chr(Int(guardDefense \ 256))
        Put #file, offset, a: offset = offset + 1           '29
        
        a = Chr(Int(guardDefense Mod 256))
        Put #file, offset, a: offset = offset + 1           '30
        
        a = Chr(Int(guardColor \ 256))
        Put #file, offset, a: offset = offset + 1           '31
        
        a = Chr(Int(guardColor Mod 256))
        Put #file, offset, a: offset = offset + 1           '32
        
        If TileSet = "Tiles.bmp" Then
            a = Chr(0)
        ElseIf TileSet = "TownTiles.bmp" Then
            a = Chr(1)
        ElseIf TileSet = "CastleTiles.bmp" Then
            a = Chr(2)
        ElseIf TileSet = "LOB Tiles.bmp" Then
            a = Chr(3)
        ElseIf TileSet = "LOB TownTiles.bmp" Then
            a = Chr(4)
        End If
        
        Put #file, 34, a: offset = offset + 1           '33

        a = Chr(BuyRaftMap)                                  '36  Buy Raft Map
        Put #file, 37, a
        
        a = Chr(BuyRaftX \ 256)
        Put #file, 38, a                                '37  Buy Raft X
        a = Chr(BuyRaftX Mod 256)
        Put #file, 39, a                                '38  Buy Raft X
        
        a = Chr(BuyRaftY \ 256)
        Put #file, 40, a                                '39  Buy Raft y
        a = Chr(BuyRaftY Mod 256)
        Put #file, 41, a                                '40  Buy Raft Y
        
        a = Chr(120)
        Put #file, 42, a                             '41 special count
        
        a = Chr(mail(0))
        Put #file, 43, a                            ' 42 mail 0
        
        a = Chr(mail(1))
        Put #file, 44, a                           ' 43 mail 1
        
        a = Chr(mail(2))
        Put #file, 45, a                           ' 44 mail 2
        
        a = Chr(mail(3))
        Put #file, 46, a                           ' 45 mail 3
        
        offset = fileOffset + 1
        
        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
                Dim b As Byte
                
                b = Map(i, j)
                
                Put #file, offset, b
                offset = offset + 1
                
            Next
        Next
        
        'offset = (mapHeight + 1) * mapWidth + 1
        
        For i = 1 To maxSpecial
            Put #file, offset, special(i): offset = offset + 1
            
            a = Chr(Int(specialx(i) \ 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialx(i) Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialy(i) \ 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialy(i) Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialwidth(i) \ 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialwidth(i) Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialheight(i) \ 256))
            Put #file, offset, a: offset = offset + 1
            
            a = Chr(Int(specialheight(i) Mod 256))
            Put #file, offset, a: offset = offset + 1
            
            Put #file, offset, specialdata(i): offset = offset + Len(specialdata(i))
        Next
        
        If MapType = maptown Or MapType = mapCastle Then
        
            Put #file, offset, "5555557": offset = offset + Len("5555557")
            
            For i = 0 To 100
                a = Chr(Int(guard(i).x \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(guard(i).x Mod 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(guard(i).y \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(guard(i).y Mod 256))
                Put #file, offset, a: offset = offset + 1
            Next
            
            a = Chr(Int((offset - 1) \ 256))
            Put #file, 35, a                              ' 34 (roof offset)
            
            a = Chr(Int((offset - 1) Mod 256))
            Put #file, 36, a                             ' 35 (roof offset)
            
            For i = 1 To maxRoofs
                ' anchor
                a = Chr(Int(Roofs(i).anchor.x \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).anchor.x Mod 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).anchor.y \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).anchor.y Mod 256))
                Put #file, offset, a: offset = offset + 1
               
                ' anchortarget
                a = Chr(Int(Roofs(i).anchorTarget.x \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).anchorTarget.x Mod 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).anchorTarget.y \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).anchorTarget.y Mod 256))
                Put #file, offset, a: offset = offset + 1
                
                '  Dimensions
                a = Chr(Int(Roofs(i).width \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).width Mod 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).height \ 256))
                Put #file, offset, a: offset = offset + 1
                
                a = Chr(Int(Roofs(i).height Mod 256))
                Put #file, offset, a: offset = offset + 1
            
                '  Data
                For j = 0 To Roofs(i).height - 1
                    For k = 0 To Roofs(i).width - 1
                        a = Chr(Roofs(i).Matrix(k, j))
                        Put #file, offset, a: offset = offset + 1
                    Next
                Next
            Next
            
        End If
        
        Close file
    
        StatusBar1.Panels(1).Text = "Saved successfully: " & Time
        
    End If
    
    blit

End Sub

Private Sub mnuOpen_Click()
    
    On Local Error GoTo ErrorHandler
    
    cmdDialog.DialogTitle = "Open Map"
    
    cmdDialog.Filter = "All Map Files|*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
    cmdDialog.FilterIndex = 1
    
    cmdDialog.InitDir = LotaPath & "\Included Maps"
    cmdDialog.fileName = ""
    
    cmdDialog.DefaultExt = "map"
    
    cmdDialog.ShowOpen
          
    fileName = cmdDialog.fileName
    
    OpenMap
    
ErrorHandler:
End Sub

Private Sub OpenMap()
    Dim path As String
    Dim file As Integer
    Dim newOffset As Integer
    Dim tempName As String
    Dim i As Integer, j As Integer, k As Integer
    Dim b$
    Dim ro
    
    path = fileName
    
    
    If path <> "" Then
        file = FreeFile
        Open path For Binary As #file
        
            
        For i = 1 To maxRoofs
            Roofs(i).anchor.x = 0
            Roofs(i).anchor.y = 0
            Roofs(i).anchorTarget.x = 0
            Roofs(i).anchorTarget.y = 0
            Roofs(i).height = 0
            Roofs(i).width = 0
            
        Next
            
        For i = 0 To 100
            guard(i).x = 0
            guard(i).y = 0
        Next
        
        For i = 1 To maxSpecial
            special(i) = 0
            specialx(i) = 0
            specialy(i) = 0
            specialdata(i) = ""
            specialwidth(i) = 0
            specialheight(i) = 0
            
        Next
        
        
        Dim a As String * 1
        
        Get #file, 1, a
        mapWidth = Asc(a) * 256
        
        Get #file, 2, a
        mapWidth = mapWidth + Asc(a)
        
        Get #file, 3, a
        mapHeight = Asc(a) * 256
        
        Get #file, 4, a
        mapHeight = mapHeight + Asc(a)
        
        If mapWidth > 3000 Or mapHeight > 3000 Then
            MsgBox "Bad File: " & path & vbCrLf & vbCrLf & "The data is invalid.  Please try a different file or replace it."
            validMap = False
            
            Exit Sub
        End If
        
        Get #file, 5, a
        newOffset = Asc(a) * 256
        
        Get #file, 6, a
        newOffset = newOffset + Asc(a)
        fileOffset = newOffset
        
        Get #file, 7, a
        MapType = Asc(a)

        offset = 8
        
        tempName = ""
        Do
            Get #file, offset, a
            tempName = tempName & a
            offset = offset + 1
        Loop Until a = "@" Or offset > 25
        
        mapName = RTrim(Left(tempName, Len(tempName) - 1))
        
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

        Get #file, 34, a: offset = offset + 1           '33
        If a = Chr(0) Then
            TileSet = "Tiles.bmp"
        ElseIf a = Chr(1) Then
            TileSet = "TownTiles.bmp"
        ElseIf a = Chr(2) Then
            TileSet = "CastleTiles.bmp"
        ElseIf a = Chr(3) Then
            TileSet = "LOB Tiles.bmp"
        ElseIf a = Chr(4) Then
            TileSet = "LOB TownTiles.bmp"
        End If
        
        Get #file, 37, a
        BuyRaftMap = Asc(a)                                '36  Buy Raft Map
        
        Get #file, 38, a                                '37  Buy Raft X
        BuyRaftX = Asc(a) * 256
        Get #file, 39, a                                '38  Buy Raft X
        BuyRaftX = BuyRaftX + Asc(a)
        
        Get #file, 40, a                                '39  Buy Raft y
        BuyRaftY = Asc(a) * 256
        Get #file, 41, a                                '40  Buy Raft Y
        BuyRaftY = BuyRaftY + Asc(a)
        
        Get #file, 42, a                                '41  special count
        If Asc(a) = 0 Then a = Chr(20)
        specialCount = Asc(a)
        
        Get #file, 35, a: offset = offset + 1           ' 34 (Roof offset)
        ro = Asc(a) * 256
        
        Get #file, 36, a: offset = offset + 1           ' 35 (roof offset)
        ro = ro + Asc(a) + 1
        
        Get #file, 43, a                            ' 42 mail 0
        mail(0) = Asc(a)
        
        Get #file, 44, a                           ' 43 mail 1
        mail(1) = Asc(a)
        
        Get #file, 45, a                           ' 44 mail 2
        mail(2) = Asc(a)
        
        Get #file, 46, a                           ' 45 mail 3
        mail(3) = Asc(a)
        
        offset = newOffset + 1
        
        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
            
                Get #file, offset, a
                
                mMap(i, j) = Asc(a)
                
                offset = offset + 1
                
            Next
        Next
                
        For i = 1 To specialCount
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
            
            Get #file, offset, a: offset = offset + 1
            specialwidth(i) = Asc(a) * 256
            
            Get #file, offset, a: offset = offset + 1
            specialwidth(i) = specialwidth(i) + Asc(a)
           
            Get #file, offset, a: offset = offset + 1
            specialheight(i) = Asc(a)
           
            Get #file, offset, a: offset = offset + 1
            specialheight(i) = specialheight(i) + Asc(a)
            
            Get #file, offset, specialdata(i): offset = offset + Len(specialdata(i))
        Next
        
        For i = 1 To maxRoofs
            Roofs(i).anchor.x = 0
            Roofs(i).anchor.y = 0
            Roofs(i).anchorTarget.x = 0
            Roofs(i).anchorTarget.y = 0
            Roofs(i).height = 0
            Roofs(i).width = 0
            
        Next
        
        If MapType = maptown Or MapType = mapCastle Then
            Do Until Right(b$, 7) = "5555557" Or EOF(file)
                Get #file, offset, a: offset = offset + 1
                
                b$ = b$ + a
            Loop
        
        
            For i = 0 To 100
                Get #file, offset, a: offset = offset + 1
                guard(i).x = Asc(a) * 256
                Get #file, offset, a: offset = offset + 1
                guard(i).x = guard(i).x + Asc(a)
            
                Get #file, offset, a: offset = offset + 1
                guard(i).y = Asc(a) * 256
                Get #file, offset, a: offset = offset + 1
                guard(i).y = guard(i).y + Asc(a)
            Next
            

            
            For i = 1 To maxRoofs
                ' anchor
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchor.x = Asc(a) * 256
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchor.x = Roofs(i).anchor.x + Asc(a)
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchor.y = Asc(a)
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchor.y = Roofs(i).anchor.y + Asc(a)
                
                ' anchortarget
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchorTarget.x = Asc(a) * 256
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchorTarget.x = Roofs(i).anchorTarget.x + Asc(a)
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchorTarget.y = Asc(a) * 256
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).anchorTarget.y = Roofs(i).anchorTarget.y + Asc(a)
                
                '  Dimensions
                Get #file, offset, a: offset = offset + 1
                Roofs(i).width = Asc(a) * 256
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).width = Roofs(i).width + Asc(a)
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).height = Asc(a) * 256
                
                Get #file, offset, a: offset = offset + 1
                Roofs(i).height = Roofs(i).height + Asc(a)
                
                '  Data
                For j = 0 To Roofs(i).height - 1
                    For k = 0 To Roofs(i).width - 1
                        Get #file, offset, a: offset = offset + 1
                        Roofs(i).Matrix(k, j) = Asc(a)
                    Next
                Next
            
            Next
            
        End If

        Close file
        
        fileName = path
        
        x1 = 0
        x2 = 0
        y1 = 0
        y2 = 0
        
        
        sbRight.max = mapWidth
        sbDown.max = mapHeight
        
        LoadTiles TileSet
        validMap = True
        ImportMap = False
        
    End If
    
    SetMenus
    FillSpecial
    blit
End Sub

Public Sub SetPropertiesForm()
    Dim i As Integer
    
    With frmProperties
        .txtName = mapName
        .txtDefaultTile = defaultTile
        .txtFileOffset = fileOffset
        .mType = MapType
        .txtAttack = guardAttack
        .txtHP = guardHP
        .txtDefense = guardDefense
        .txtColor = guardColor
        .txtDefaultTile = defaultTile
        .Caption = LTrim(mapName) & " Properties"
        .txtWidth = mapWidth
        .txtHeight = mapHeight
        .theTiles = TileSet
        .txtBuyRaftMap = BuyRaftMap
        .txtBuyRaftX = BuyRaftX
        .txtBuyRaftY = BuyRaftY
        
        .setProperties = True
        
        For i = 0 To 3
            .txtMail(i) = mail(i)
        Next
        
    End With
    
    oldw = width
    oldh = height
End Sub

Public Sub AssignProperties()
    Dim i As Integer
    
    mapName = frmProperties.txtName
    
    If frmProperties.txtDefaultTile <> "" Then defaultTile = frmProperties.txtDefaultTile
    fileOffset = frmProperties.txtFileOffset
    MapType = frmProperties.mType
    If frmProperties.txtAttack <> "" Then guardAttack = frmProperties.txtAttack
    If frmProperties.txtHP <> "" Then guardHP = frmProperties.txtHP
    If frmProperties.txtDefense <> "" Then guardDefense = frmProperties.txtDefense
    If frmProperties.txtColor <> "" Then guardColor = frmProperties.txtColor
    If frmProperties.theTiles <> "" Then TileSet = frmProperties.theTiles
    mapWidth = frmProperties.txtWidth
    mapHeight = frmProperties.txtHeight
    
    BuyRaftMap = frmProperties.mbuyraftmap
    BuyRaftX = CInt(frmProperties.mBuyRaftX)
    BuyRaftY = CInt(frmProperties.mBuyRaftY)
    
    For i = 0 To 3
        mail(i) = Val(frmProperties.txtMail(i))
    Next
    
    
    If oldw > 0 And oldh > 0 Then
        If oldw <> mapWidth And oldh <> mapHeight Then
        
        End If
    End If
    
    
    SetPos x1, y1
    LoadTiles TileSet
    
End Sub

Public Sub SetPos(ByVal xx As Integer, ByVal yy As Integer)
    Dim stopChecking As Boolean, i
    x1 = xx
    y1 = yy
    
    x2 = xx
    y2 = yy
    
    For i = 1 To maxSpecial
        If specialx(i) = x1 And specialy(i) = y1 And stopChecking = False Then
            cmdPlaceSpecial.Enabled = False
            cmdModifySpecial.Enabled = True
            cmdDeleteSpecial.Enabled = True
            stopChecking = True
        ElseIf stopChecking = False Then
            cmdPlaceSpecial.Enabled = True
            cmdModifySpecial.Enabled = False
            cmdDeleteSpecial.Enabled = False
        End If
    Next

End Sub

Public Sub SetRightPos(ByVal xx As Integer, ByVal yy As Integer)
    x2 = xx
    y2 = yy
End Sub

Private Sub sbDown_Change()
    dispY = sbDown.value
    blit
End Sub

Private Sub sbRight_Change()
    dispX = sbRight.value
    blit
End Sub

Private Sub PaintLoc(ByVal x As Integer, ByVal y As Integer, ByVal value As Integer)
    If ImportMap Then
        PaintArea x, y, value
    Else
        mMap(x, y) = value
    End If
    
End Sub

Private Sub SetMenus()
    
    mnuSave.Enabled = Not ImportMap
    mnuSaveAs.Enabled = Not ImportMap
    mnuProperties.Enabled = Not ImportMap
    mnuPlaceSpecial.Enabled = Not ImportMap
    mnuModifySpecial.Enabled = Not ImportMap
    mnuDeleteSpecial.Enabled = Not ImportMap
    
    mnuFinalize.Enabled = ImportMap
    mnuTitleImport.Enabled = ImportMap
        
End Sub

Private Sub sbSpecial_Change()
    lblFindSpecial = "Find Special: " & sbSpecial.value
    
    If sbSpecial.value = 0 Then Exit Sub
    
    x1 = specialx(sbSpecial.value)
    x2 = x1
    
    y1 = specialy(sbSpecial.value)
    y2 = y1
    
    sbRight.value = max(x1 - picTilesX / 5, 0)
    sbDown.value = max(y1 - picTilesY / 5, 0)
    
    blit
    
End Sub

Private Sub SortSpecials()
    Dim i As Integer, j As Integer
    Dim max As Integer, min As Integer
    
    max = 0
    min = 120
    
    For i = 120 To 1 Step -1
        If special(i) = 0 Then
            If i < max Then
                For j = i To max
                    special(i) = special(i + 1)
                    specialx(i) = specialx(i + 1)
                    specialy(i) = specialy(i + 1)
                    specialdata(i) = specialdata(i + 1)
                    specialwidth(i) = specialwidth(i + 1)
                    specialheight(i) = specialheight(i + 1)
                Next
                
            End If
            
        ElseIf special(i) > 0 Then
            If i > max Then
                max = i
            End If
        
        End If
        
    Next
    
End Sub

Private Function NumSpecials() As Integer
    Dim i As Integer
    
    SortSpecials
    
    For i = 1 To 120
        If special(i) = 0 Then
            NumSpecials = i - 1
            Exit Function
        End If
        
    Next
    
End Function
