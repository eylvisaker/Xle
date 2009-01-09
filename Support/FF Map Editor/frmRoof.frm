VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmRoof 
   Caption         =   "Edit Roof"
   ClientHeight    =   9210
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8160
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
   ScaleHeight     =   614
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   544
   StartUpPosition =   3  'Windows Default
   Begin MSComCtl2.FlatScrollBar sbDown 
      Height          =   4695
      Left            =   4560
      TabIndex        =   24
      Top             =   0
      Width           =   255
      _ExtentX        =   450
      _ExtentY        =   8281
      _Version        =   393216
      Appearance      =   0
      Orientation     =   1179648
   End
   Begin MSComCtl2.FlatScrollBar sbRight 
      Height          =   255
      Left            =   0
      TabIndex        =   23
      Top             =   4680
      Width           =   4575
      _ExtentX        =   8070
      _ExtentY        =   450
      _Version        =   393216
      Appearance      =   0
      Arrows          =   65536
      Orientation     =   1179649
   End
   Begin VB.CommandButton cmdCopy 
      Caption         =   "Copy From Ground"
      Height          =   855
      Left            =   4440
      TabIndex        =   22
      Top             =   6720
      Width           =   1335
   End
   Begin VB.CheckBox chkDrawGround 
      Caption         =   "Draw Ground"
      Height          =   495
      Left            =   4440
      TabIndex        =   21
      Top             =   6000
      Width           =   3135
   End
   Begin VB.TextBox txtTargetY 
      Height          =   405
      Left            =   6600
      TabIndex        =   20
      Text            =   "Text2"
      Top             =   3120
      Width           =   1335
   End
   Begin VB.TextBox txtTargetX 
      Height          =   405
      Left            =   5040
      TabIndex        =   19
      Text            =   "Text1"
      Top             =   3120
      Width           =   1335
   End
   Begin VB.TextBox txtAnchorY 
      Height          =   405
      Left            =   6600
      TabIndex        =   12
      Text            =   "Text2"
      Top             =   2280
      Width           =   1335
   End
   Begin VB.TextBox txtAnchorX 
      Height          =   405
      Left            =   5040
      TabIndex        =   11
      Text            =   "Text1"
      Top             =   2280
      Width           =   1215
   End
   Begin VB.HScrollBar hsbRoofIndex 
      Height          =   255
      LargeChange     =   5
      Left            =   5040
      Max             =   40
      Min             =   1
      TabIndex        =   9
      Top             =   480
      Value           =   1
      Width           =   2895
   End
   Begin VB.TextBox txtRoofY 
      Height          =   495
      Left            =   6600
      TabIndex        =   6
      Text            =   "Text2"
      Top             =   1320
      Width           =   1215
   End
   Begin VB.TextBox txtRoofX 
      Height          =   495
      Left            =   5040
      TabIndex        =   5
      Text            =   "txtRoofX"
      Top             =   1320
      Width           =   1215
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "Done"
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
      Left            =   6720
      TabIndex        =   4
      Top             =   8520
      Width           =   1215
   End
   Begin VB.PictureBox Picture2 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3840
      Left            =   240
      ScaleHeight     =   252
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   252
      TabIndex        =   1
      Top             =   5280
      Width           =   3840
   End
   Begin VB.PictureBox Picture1 
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   4665
      Left            =   0
      ScaleHeight     =   311
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   303
      TabIndex        =   0
      Top             =   0
      Width           =   4545
   End
   Begin VB.Label Label6 
      Caption         =   "Anchor Target"
      Height          =   375
      Left            =   5040
      TabIndex        =   18
      Top             =   2760
      Width           =   1815
   End
   Begin VB.Label lblCurrentTile 
      Caption         =   "Label6"
      Height          =   375
      Left            =   5160
      TabIndex        =   17
      Top             =   5280
      Width           =   3000
   End
   Begin VB.Label lblTile 
      Caption         =   "Label6"
      Height          =   375
      Left            =   5160
      TabIndex        =   16
      Top             =   4920
      Width           =   3000
   End
   Begin VB.Label lblY 
      Caption         =   "Label6"
      Height          =   375
      Left            =   5160
      TabIndex        =   15
      Top             =   4560
      Width           =   3000
   End
   Begin VB.Label lblx 
      Caption         =   "X:"
      Height          =   255
      Left            =   5160
      TabIndex        =   14
      Top             =   4200
      Width           =   3000
   End
   Begin VB.Label Label2 
      Caption         =   "Current Position:"
      Height          =   375
      Left            =   5040
      TabIndex        =   13
      Top             =   3720
      Width           =   2055
   End
   Begin VB.Label Label3 
      Caption         =   "Anchor Point"
      Height          =   255
      Left            =   5040
      TabIndex        =   10
      Top             =   1920
      Width           =   1335
   End
   Begin VB.Label lblEditing 
      Caption         =   "Editing Roof"
      Height          =   375
      Left            =   5040
      TabIndex        =   8
      Top             =   120
      Width           =   3015
   End
   Begin VB.Label Label1 
      Caption         =   "Roof Dimensions"
      Height          =   255
      Left            =   5040
      TabIndex        =   7
      Top             =   960
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
      TabIndex        =   3
      Top             =   5280
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
      TabIndex        =   2
      Top             =   5040
      Width           =   4215
   End
End
Attribute VB_Name = "frmRoof"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public RoofIndex
Dim RoofTile As Integer
Dim roofLeftX As Integer, roofTopY As Integer
Dim x2, y2
Dim setting As Boolean


Private Sub Check1_Click()

End Sub

Private Sub chkDrawGround_Click()
    RoofBlit
    
End Sub

Private Sub cmdDone_Click()
    Me.Hide
    
End Sub

Private Sub Form_Load()
    CreateBackBuffer Picture1.width, Picture1.height
    SetControls
    
End Sub

Private Sub Form_Paint()
    UpdateControls
    RoofBlit
End Sub

Private Sub Form_Unload(Cancel As Integer)
    CreateBackBuffer frmMEdit.Picture1.width, frmMEdit.Picture1.height
    
End Sub

Private Sub hsbRoofIndex_Change()
    RoofIndex = hsbRoofIndex.value
    
    SetControls
    
End Sub

Private Sub Picture1_Paint()
    RoofBlit
End Sub

Sub RoofBlit()
    Dim ddrval As Long
    Dim r1 As RECT
    Dim r2 As RECT
    
    
    RoofDraw
    
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

Private Sub cmdCopy_Click()
    Dim i As Integer, j As Integer
    Dim t As Point
    
    If MsgBox("Overwrite current roof?", vbYesNo, "Overwrite?") = vbNo Then Exit Sub
    
    For j = 0 To Roofs(RoofIndex).height - 1
        For i = 0 To Roofs(RoofIndex).width - 1
            t.x = Roofs(RoofIndex).anchorTarget.x - Roofs(RoofIndex).anchor.x + i
            t.y = Roofs(RoofIndex).anchorTarget.y - Roofs(RoofIndex).anchor.y + j
            
            Roofs(RoofIndex).Matrix(i, j) = Map(t.x, t.y)
        Next
    Next
    
    RoofBlit
    
End Sub

Private Sub RoofDraw()
    Dim ddrval As Long
    Dim r1 As RECT
    Dim r2 As RECT
    Dim j, i, a, tilex, tiley
    Dim xx, yy, roofCenterX, roofCenterY
    Dim t As Point
    Dim tile As Point
    
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
    
    picTilesX = Int(Picture1.ScaleWidth / TileSize / 2 + 1)
    picTilesY = Int(Picture1.ScaleHeight / TileSize / 2)
        
    xx = 0
    yy = 0
    

    sbRight.Max = mapWidth
    sbRight.Min = 0
    sbRight.LargeChange = picTilesX * 2 - 2
    sbDown.LargeChange = picTilesY * 2 - 2
    sbDown.Max = mapHeight
    sbDown.Min = 0
    
    roofCenterX = sbRight.value
    roofCenterY = sbDown.value
    
    roofLeftX = roofCenterX - picTilesX
    roofTopY = roofCenterY - picTilesY
    
    'roofLeftX = x2 - picTilesX
    'roofTopY = y2 - picTilesY
    
    For j = roofCenterY - picTilesY To roofCenterY + picTilesY + 1
        For i = roofCenterX - picTilesX To roofCenterX + picTilesX + 1
            If i >= 0 And i < Roofs(RoofIndex).width And j >= 0 And j < Roofs(RoofIndex).height Then
            
                a = Roofs(RoofIndex).Matrix(i, j)
                                
                
                If chkDrawGround <> 0 And a = 127 Then
                    't = Roofs(k).matrix(i - Roofs(k).anchorTarget.X + Roofs(k).anchor.Y, _
                    '           j - Roofs(k).anchorTarget.Y + Roofs(k).anchor.Y)
                    t.x = Roofs(RoofIndex).anchorTarget.x - Roofs(RoofIndex).anchor.x + i
                    t.y = Roofs(RoofIndex).anchorTarget.y - Roofs(RoofIndex).anchor.y + j
                    
                    If (t.y < 0 Or t.x < 0) Then
                        a = 127
                    Else
                        a = Map(t.x, t.y)
                    End If
                    
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
        
    DDSBack.SetForeColor vbCyan
               
    r1.Left = (Roofs(RoofIndex).anchor.x - roofLeftX) * TileSize
    r1.Right = r1.Left + TileSize
    r1.Top = (Roofs(RoofIndex).anchor.y - roofTopY) * TileSize
    r1.Bottom = r1.Top + TileSize
        
    DDSBack.DrawBox r1.Left, r1.Top, r1.Right, r1.Bottom
    
    DDSBack.SetForeColor vbWhite
    DDSBack.DrawBox (x2 - roofLeftX) * TileSize, (y2 - roofTopY) * TileSize, (x2 - roofLeftX + 1) * TileSize, (y2 - roofTopY) * TileSize
    
    lblx.Caption = "x2:   " & x2 & "  0x" & Hex$(x2)
    lblY.Caption = "y2:   " & y2 & "  0x" & Hex$(y2)
    
    If x2 < 0 Or x2 >= Roofs(RoofIndex).width Or y2 < 0 Or y2 >= Roofs(RoofIndex).height Then
        lblTile.Caption = "Tile: Out of range"
    Else
        lblTile.Caption = "Tile: " & Roofs(RoofIndex).Matrix(x2, y2) & "   0x" & Hex$(Roofs(RoofIndex).Matrix(x2, y2))
    End If
    
    lblCurrentTile = "Current Tile: " & RoofTile & "   0x" & Hex$(RoofTile)
    
    
End Sub



Private Sub Picture2_Paint()
    RoofBlit
End Sub
Private Sub Picture1_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
    Dim xx, yy, r, tile, i
    

    xx = Int(x \ 16) + roofLeftX
    yy = Int(y \ 16) + roofTopY
    
    Debug.Print "X: " & x & "  XX: " & xx & "        Y: " & y & "  YY: " & yy
    
        
    If Button = 1 Then
       
        SetPos xx, yy
        
    ElseIf Button = 2 And xx >= 0 And yy >= 0 Then
                
        Roofs(RoofIndex).Matrix(xx, yy) = RoofTile
        
    End If
    

    RoofBlit
End Sub

Private Sub Picture1_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
    Dim xx, yy, r, tile
    
    xx = x
    yy = y
    
    xx = Int(xx \ 16) + roofLeftX
    yy = Int(yy \ 16) + roofTopY
    
    If Button = 2 And xx >= 0 And yy >= 0 Then
        
        Roofs(RoofIndex).Matrix(xx, yy) = RoofTile
       
        RoofBlit
    End If
    
End Sub

Private Sub Picture2_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
    Dim xx, yy, tile
    
    xx = x
    yy = y
    
    xx = Int(xx / 16)
    yy = Int(yy / 16)
 
    tile = (yy * 16 + xx)
    
    If Button = 2 Then
        Roofs(RoofIndex).Matrix(x1, y1) = tile
    End If
    
    RoofTile = tile
    
    RoofBlit
End Sub

Private Sub SetPos(ByVal xx As Integer, ByVal yy As Integer)
    x2 = xx
    y2 = yy
    
End Sub

Public Sub UpdateControls()
    lblEditing.Caption = "Editing roof #: " & hsbRoofIndex.value
    
    If setting = False Then
        If IsNumeric(txtRoofX) Then Roofs(RoofIndex).width = txtRoofX
        If IsNumeric(txtRoofY) Then Roofs(RoofIndex).height = txtRoofY
        If IsNumeric(txtAnchorX) Then Roofs(RoofIndex).anchor.x = txtAnchorX
        If IsNumeric(txtAnchorY) Then Roofs(RoofIndex).anchor.y = txtAnchorY
        If IsNumeric(txtTargetX) Then Roofs(RoofIndex).anchorTarget.x = txtTargetX
        If IsNumeric(txtTargetY) Then Roofs(RoofIndex).anchorTarget.y = txtTargetY
        
        RoofBlit
    End If
        
End Sub


Private Sub sbDown_Change()
    RoofBlit
End Sub

Private Sub sbRight_Change()
    RoofBlit
    
End Sub

Private Sub txtAnchorX_Change()
    UpdateControls

End Sub

Private Sub txtAnchorY_Change()
    UpdateControls

End Sub

Private Sub txtRoofX_Change()
    UpdateControls
    
End Sub

Public Sub SetControls()
    setting = True
    
    txtRoofX = Roofs(RoofIndex).width
    txtRoofY = Roofs(RoofIndex).height
    txtAnchorX = Roofs(RoofIndex).anchor.x
    txtAnchorY = Roofs(RoofIndex).anchor.y
    txtTargetX = Roofs(RoofIndex).anchorTarget.x
    txtTargetY = Roofs(RoofIndex).anchorTarget.y
    
    hsbRoofIndex.value = RoofIndex
        
    setting = False
    
    RoofBlit
End Sub

Private Sub txtRoofY_Change()
    UpdateControls
End Sub

Private Sub txtTargetX_Change()
    UpdateControls

End Sub

Private Sub txtTargetY_Change()
    UpdateControls

End Sub
