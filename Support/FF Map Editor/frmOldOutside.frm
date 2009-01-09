VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmOldOutside 
   Caption         =   "LotA Map Editor"
   ClientHeight    =   7875
   ClientLeft      =   165
   ClientTop       =   735
   ClientWidth     =   10635
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   9.75
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   ScaleHeight     =   525
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   709
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox Picture2 
      Height          =   3840
      Left            =   6360
      ScaleHeight     =   252
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   252
      TabIndex        =   23
      Top             =   600
      Width           =   3840
   End
   Begin VB.TextBox Text7 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2895
      Left            =   6360
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   21
      Top             =   4560
      Width           =   3855
   End
   Begin VB.CommandButton Command3 
      Caption         =   "Place Special"
      Height          =   375
      Left            =   3840
      TabIndex        =   20
      Top             =   6360
      Width           =   1575
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Large"
      Height          =   375
      Left            =   2760
      TabIndex        =   16
      Top             =   7080
      Width           =   1695
   End
   Begin VB.TextBox Text3 
      Height          =   345
      Left            =   1080
      TabIndex        =   14
      Text            =   "Text3"
      Top             =   7080
      Width           =   1335
   End
   Begin VB.TextBox Text6 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   2640
      TabIndex        =   11
      Text            =   "Text6"
      Top             =   6240
      Width           =   735
   End
   Begin VB.TextBox Text5 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   1920
      TabIndex        =   10
      Text            =   "Text5"
      Top             =   6240
      Width           =   615
   End
   Begin VB.TextBox Text4 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   1200
      TabIndex        =   9
      Text            =   "Text4"
      Top             =   5280
      Width           =   615
   End
   Begin VB.OptionButton op2 
      Caption         =   "Bottom Right"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   4440
      TabIndex        =   8
      Top             =   2520
      Width           =   1215
   End
   Begin VB.OptionButton op1 
      Caption         =   "Top Left"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   4440
      TabIndex        =   7
      Top             =   2040
      Width           =   1215
   End
   Begin VB.TextBox Text2 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   360
      TabIndex        =   6
      Text            =   "Text2"
      Top             =   5280
      Width           =   735
   End
   Begin VB.TextBox Text1 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4440
      TabIndex        =   0
      Text            =   "Text1"
      Top             =   1680
      Width           =   1215
   End
   Begin MSComDlg.CommonDialog cmdDialog 
      Left            =   6840
      Top             =   4560
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Set"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3600
      TabIndex        =   4
      Top             =   4920
      Width           =   1095
   End
   Begin VB.PictureBox Picture1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BorderStyle     =   0  'None
      CausesValidation=   0   'False
      FillStyle       =   0  'Solid
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   4080
      Left            =   600
      ScaleHeight     =   272
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   241
      TabIndex        =   1
      Top             =   720
      Width           =   3615
   End
   Begin VB.Label lblCurrentTile 
      Caption         =   "Tile:"
      Height          =   735
      Left            =   4320
      TabIndex        =   24
      Top             =   3840
      Width           =   1455
   End
   Begin VB.Label Label7 
      Caption         =   "Label7"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Index           =   0
      Left            =   600
      TabIndex        =   22
      Top             =   120
      Width           =   3735
   End
   Begin VB.Label Label9 
      Caption         =   "Label9"
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
      Left            =   6240
      TabIndex        =   19
      Top             =   120
      Width           =   2655
   End
   Begin VB.Label Label8 
      Caption         =   "Label8"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   4095
      Left            =   120
      TabIndex        =   18
      Top             =   720
      Width           =   375
   End
   Begin VB.Label Label7 
      Caption         =   "Label7"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Index           =   1
      Left            =   600
      TabIndex        =   17
      Top             =   360
      Width           =   3615
   End
   Begin VB.Label Label6 
      Caption         =   "Tile:"
      Height          =   375
      Left            =   240
      TabIndex        =   15
      Top             =   7080
      Width           =   855
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
      Left            =   6000
      TabIndex        =   13
      Top             =   600
      Width           =   375
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
      Left            =   6360
      TabIndex        =   12
      Top             =   360
      Width           =   4215
   End
   Begin VB.Label Label3 
      Caption         =   "Label3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4440
      TabIndex        =   5
      Top             =   1320
      Width           =   1455
   End
   Begin VB.Label Label2 
      Caption         =   "Label2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4440
      TabIndex        =   3
      Top             =   840
      Width           =   1335
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
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
      Left            =   4440
      TabIndex        =   2
      Top             =   360
      Width           =   1215
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuNew 
         Caption         =   "&New"
      End
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open"
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save"
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
   Begin VB.Menu mnuMap 
      Caption         =   "&Map"
      Begin VB.Menu mnuFast 
         Caption         =   "&Draw Fast"
      End
      Begin VB.Menu mnuProperties 
         Caption         =   "&Properties"
      End
   End
End
Attribute VB_Name = "frmOldOutside"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim DX7 As New DirectX7
Dim DDraw7 As DirectDraw7

Dim DDSBack As DirectDrawSurface7
Dim DDSPrimary As DirectDrawSurface7
Dim DDSTiles As DirectDrawSurface7
Dim DDSTemp As DirectDrawSurface7

Dim ddsd As DDSURFACEDESC2
Dim ddsd1 As DDSURFACEDESC2
Dim ddClipper As DirectDrawClipper

Const TileSize = 16
Const maxSpecial = 40
Const fileOffset = 65

Dim lastName As String
Dim map(0 To 1000, 0 To 1000) As Integer
Dim x1 As Integer, y1 As Integer
Dim xx As Integer, yy As Integer
Dim leftX As Integer, topY As Integer
Dim starting As Boolean
Dim mapWidth, mapHeight
Dim filename As String
Dim special(maxSpecial) As Integer
Dim specialx(maxSpecial) As Integer, specialy(maxSpecial) As Integer
Dim specialdata(maxSpecial) As String * 5
Dim mapName As String * 16
Dim mapType As Integer
Dim fast As Boolean

Dim currentTile As Integer

Private Sub Command1_Click()

    If op1.Value = True Then
        Text2.Text = x1
        Text4.Text = y1
    ElseIf op2.Value = True Then
        Text5.Text = x1
        Text6.Text = y1
    End If
        
End Sub

Private Sub Command2_Click()
    
    Dim a As Integer
    
    a = 0
    
    If IsNumeric(Text2) And IsNumeric(Text4) And IsNumeric(Text5) And IsNumeric(Text6) Then
        For i = 1 To Len(Text3)
            b = Left(Right(Text3, i), 1)
            If b >= "A" Or b >= "a" Then
                b = Asc(b) - 55
            End If
            a = a + b * 16 ^ (i - 1)
        Next
        
        For j = Text4 To Text6
            For i = Text2 To Text5
                map(i, j) = a
            Next
        Next
        
        blit
    End If
End Sub

Private Sub Command3_Click()
    
    For i = 1 To maxSpecial
        If specialx(i) = x1 And specialy(i) = y1 Then
            special(i) = 0
            leaving = True
        End If
    Next
    
    If leaving = True Then blit: Exit Sub
    
    frmSpecial.Show 1
    
    For i = 1 To 5
        If frmSpecial.Option(i).Value = True Then
            spType = i
        End If
    Next
    
    For i = 1 To maxSpecial
        If special(i) = 0 Then
            special(i) = spType
            specialx(i) = x1
            specialy(i) = y1
            
            Exit For
        End If
    Next
    
    Select Case spType
    Case 1
        specialdata(i) = Chr(frmSpecial.txtNewMap) & Chr(frmSpecial.txtNewX) & Chr(frmSpecial.txtNewY)
        
    End Select
    
    blit
End Sub

Private Sub Form_KeyPress(KeyAscii As Integer)
    a = Chr$(KeyAscii)
    
    Select Case a
    Case "4"
        x1 = x1 - 1
    Case "6"
        x1 = x1 + 1
    Case "8"
        y1 = y1 - 1
    Case "2"
        y1 = y1 + 1
    Case "+"
        map(x1, y1) = map(x1, y1) + 1
    Case "-"
        map(x1, y1) = map(x1, y1) - 1
    End Select

    blit
End Sub

Private Sub Form_Load()
    Set DDraw7 = DX7.DirectDrawCreate("")
    
    Picture1.Width = Int(Picture1.Width / 16) * 16
    Picture1.Height = Int(Picture1.Height / 16) * 16
    
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
    
    Set DDSTiles = DDraw7.CreateSurfaceFromFile("c:\my documents\programming\c++\lota\images\tiles.bmp", ddsd)
    Set DDSTemp = DDraw7.CreateSurface(ddsd)
    
    Set ddClipper = DDraw7.CreateClipper(0)

    DDSBack.SetForeColor (&HFFFFFF)
    DDSBack.SetFont Picture1.Font
    

    For i = 0 To 15
        Label5 = Label5 & Hex$(i) & vbCrLf
    Next
    
    starting = True
    mnuNew_Click
    starting = False
    
    blit


End Sub

Private Sub draw()
    Dim r1 As RECT
    Dim r2 As RECT
    
    On Local Error Resume Next

    DDSBack.BltColorFill r1, &H555555

    Label9 = "Map Dimensions: " & mapWidth & " x " & mapHeight
    If filename <> "" Then
        Me.Caption = "LotA Map Editor - " & filename
    End If
    
    picTilesX = Int(Picture1.ScaleWidth / TileSize / 2)
    picTilesY = Int(Picture1.ScaleHeight / TileSize / 2)
        
    xx = 0
    yy = 0
    
    centerx = 0
    centery = 0
    
    leftX = x1 - picTilesX
    topY = y1 - picTilesY
    
    For j = y1 - picTilesY To y1 + picTilesY + 1
        For i = x1 - picTilesX To x1 + picTilesX + 1
            If i >= 0 And i <= mapWidth And j >= 0 And j <= mapWidth Then
            
                a = map(i, j)
                                
                If i = x1 And j = y1 Then
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

            End If
            
            xx = xx + 1
        Next
        yy = yy + 1
        xx = 0

    Next

    DDSBack.DrawBox centerx * TileSize, centery * TileSize, centerx * TileSize + 16, centery * TileSize + 16

    Picture1.Line (centerx * TileSize, centery * TileSize)-((centerx + 1) * TileSize, centery * TileSize), &HFFFF00, BF
    Picture1.Line -((centerx + 1) * TileSize, (centery + 1) * TileSize), &HFFFF00, BF
    Picture1.Line -(centerx * TileSize, (centery + 1) * TileSize), &HFFFF00, BF
    Picture1.Line -(centerx * TileSize, centery * TileSize), &HFFFF00, BF
    
    Label1.Caption = "x1:    " & x1 & "  0x" & Hex$(x1)
    Label2.Caption = "y1:    " & y1 & "  0x" & Hex$(y1)
    
    If x1 < 0 Or x1 > mapWidth Or y1 < 0 Or y1 > mapHeight Then
        Label3.Caption = "Tile: Out of range"
    Else
        Label3.Caption = "Tile: " & map(x1, y1) & "   0x" & Hex$(map(x1, y1))
    End If
    
    Label7(0).Caption = ""
    Label7(1).Caption = ""
    For i = leftX To leftX + picTilesX * 2 + 2
        Label7(i Mod 2) = Label7(i Mod 2) & i
        Label7((i + 1) Mod 2) = Label7((i + 1) Mod 2) & "  "
    Next
    
    Label8.Caption = ""
    For i = topY To topY + picTilesY * 2 + 2
        Label8 = Label8 & i & vbCrLf
    Next

    Text7.Text = ""
    For i = 1 To maxSpecial
        If special(i) > 0 Then
            Text7.Text = Text7.Text & "Special #" & i & ": Type "
            If special(i) = 1 Then
                Text7 = Text7.Text & "Map Change"
            Else
                Text7 = Text7.Text & special(i)
            End If
            
            Text7.Text = Text7.Text & vbCrLf & "    At Point: (" & specialx(i) & ", " & specialy(i) & ")"
            Text7.Text = Text7.Text & vbCrLf & "    Data: "
            
            For j = 1 To 5
                Text7.Text = Text7.Text & Hex$(Asc(Mid(specialdata(i), j, 1))) & "  "
            Next
            
            Text7.Text = Text7.Text & vbCrLf & vbCrLf
        End If
        
        If specialx(i) = x1 And specialy(i) = y1 And stopchecking = False Then
            Command3.Caption = "Delete Special"
            stopchecking = True
        ElseIf stopchecking = False Then
            Command3.Caption = "Place Special"
        End If
    Next
    
    lblCurrentTile = "Current Tile:" & vbCrLf & currentTile & "  0x" & Hex$(currentTile)
    
End Sub

Private Sub Label1_Click()
    x1 = InputBox("Enter X location: ")
    
    blit
End Sub

Private Sub Label2_Click()
    y1 = InputBox("Enter Y location: ")
    
    blit
End Sub

Private Sub mnuFast_Click()
    mnuFast.Checked = Not mnuFast.Checked
    
    fast = mnuFast.Checked
    
End Sub

Private Sub mnuNew_Click()
    If starting = False Then
        mapWidth = InputBox("Enter map width in tiles", "Width")
        mapHeight = InputBox("Enter map height in tiles", "Height")
    Else
        mapWidth = 100
        mapHeight = 100
    End If
    
    x1 = mapWidth / 2 + 1
    y1 = mapHeight / 2 + 1

    For i = 0 To mapWidth
        For j = 0 To mapHeight
            map(i, j) = 0
        Next
    Next

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

        
        offset = newOffset + 1
        
        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
            
                Get #file, offset, a
                
                map(i, j) = Asc(a)
                
                offset = offset + 1
                
            Next
        Next
                
        offset = mapHeight * mapWidth + 1
        
        For i = 1 To maxSpecial
            Get #file, offset, a: offset = offset + 1
            special(i) = Asc(a)
            
            Get #file, offset, a: offset = offset + 1
            specialx(i) = Asc(a) * 256
            
            Get #file, offset, a: offset = offset + 1
            specialx(i) = specialx(i) + Asc(a)
                        
            Get #file, offset, a: offset = offset + 1
            specialy(i) = Asc(a) * 256
            
            Get #file, offset, a: offset = offset + 1
            specialy(i) = specialy(i) + Asc(a)
            
            Get #file, offset, specialdata(i): offset = offset + Len(specialdata(i))
        Next

        Close file
        
        filename = Path
        
        If x1 > mapWidth Or y1 > mapHeight Then
            x1 = mapWidth / 2
            y1 = mapHeight / 2
        End If
            
    End If
    
    blit
End Sub

Private Sub mnuProperties_Click()
    frmProperties.txtName = mapName
    
    If mapType >= 1 And mapType <= 5 Then
        frmProperties.Option(mapType) = True
    End If
    
    frmProperties.Show 1
    
    mapName = frmProperties.txtName
    
    For i = 1 To 5
        If frmProperties.Option(i) = True Then
            mapType = i
            frmProperties.Option(i) = False
        End If
    Next
    
End Sub

Private Sub mnuQuit_Click()
    End
End Sub

Private Sub mnuSave_Click()
    On Local Error Resume Next
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
                
        a = Chr(Int((fileOffset - 1) / 256))                      '4
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(Int((fileOffset - 1) Mod 256))                  '5
        Put #file, offset, a: offset = offset + 1
        
        a = Chr(mapType)                                    '6
        Put #file, offset, a: offset = offset + 1
                
        Put #file, offset, mapName: offset = offset + Len(mapName)    '7

        Put #file, offset, "@": offset = offset + 1         '23

        Put #file, offset, "@": offset = offset + 1
        
        offset = 65
        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
            
                Put #file, offset, map(i, j)
                
                offset = offset + 1
                                
            Next
        Next
        
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
        
        
        Close file
    End If
    
    blit

End Sub

Private Sub mnuSaveAs_Click()
    cmdDialog.DialogTitle = "Hi"
    
    cmdDialog.ShowSave
    
    Path = cmdDialog.filename
    
    file = FreeFile
    
    filename = Path
    
    mnuSave_Click
    

End Sub

Private Sub Option1_Click()

End Sub


Private Sub Picture1_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
    xx = X
    yy = Y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
    
    If Button = 2 Then
       
        x1 = xx + leftX
        y1 = yy + topY
           
    ElseIf Button = 1 Then
        
        map(xx + leftX, yy + topY) = currentTile
    End If
    
    
    blit
End Sub

Private Sub Picture1_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
    xx = X
    yy = Y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
    
    If Button = 1 Then
        map(xx + leftX, yy + topY) = currentTile
        blit
    End If
    
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


Private Sub Picture1_Paint()
    blit
End Sub

Private Sub picture2_paint()
    blit
End Sub

Private Sub Text1_KeyPress(KeyAscii As Integer)
    Form_KeyPress (KeyAscii)
    
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


