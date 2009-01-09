VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmProperties 
   Caption         =   "Map Properties"
   ClientHeight    =   6510
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6405
   LinkTopic       =   "Form1"
   ScaleHeight     =   6510
   ScaleWidth      =   6405
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame frmChar 
      Caption         =   "Characteristics"
      Height          =   2535
      Left            =   120
      TabIndex        =   34
      Top             =   3960
      Width           =   2895
      Begin VB.TextBox txtMail 
         Height          =   285
         Index           =   3
         Left            =   2040
         TabIndex        =   54
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtMail 
         Height          =   285
         Index           =   2
         Left            =   1440
         TabIndex        =   53
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtMail 
         Height          =   285
         Index           =   1
         Left            =   840
         TabIndex        =   52
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtMail 
         Height          =   285
         Index           =   0
         Left            =   240
         TabIndex        =   50
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtBuyRaftMap 
         Height          =   285
         Left            =   1440
         TabIndex        =   49
         Text            =   "1"
         Top             =   1320
         Width           =   855
      End
      Begin VB.TextBox txtBuyRaftY 
         Height          =   285
         Left            =   1440
         TabIndex        =   47
         Text            =   "0"
         Top             =   960
         Width           =   855
      End
      Begin VB.TextBox txtBuyRaftX 
         Height          =   285
         Left            =   1440
         TabIndex        =   46
         Text            =   "0"
         Top             =   600
         Width           =   855
      End
      Begin VB.TextBox txtDefaultTile 
         Height          =   285
         Left            =   1080
         TabIndex        =   15
         Text            =   "0"
         Top             =   240
         Width           =   1650
      End
      Begin VB.Label Label15 
         Caption         =   "Mail Route"
         Height          =   255
         Left            =   120
         TabIndex        =   51
         Top             =   1680
         Width           =   975
      End
      Begin VB.Label Label14 
         Caption         =   "Buy Raft Map"
         Height          =   255
         Left            =   360
         TabIndex        =   48
         Top             =   1320
         Width           =   975
      End
      Begin VB.Label Label13 
         Caption         =   "Buy Raft Y"
         Height          =   255
         Left            =   360
         TabIndex        =   45
         Top             =   960
         Width           =   1335
      End
      Begin VB.Label Label12 
         Caption         =   "Buy Raft X"
         Height          =   255
         Left            =   360
         TabIndex        =   44
         Top             =   600
         Width           =   1455
      End
      Begin VB.Label lblDefaultTile 
         Caption         =   "Default Tile:"
         Height          =   255
         Left            =   120
         TabIndex        =   35
         Top             =   240
         Width           =   975
      End
   End
   Begin VB.Frame frmDungeon 
      Caption         =   "Dungeon Level Characteristics"
      Height          =   1815
      Left            =   120
      TabIndex        =   38
      Top             =   5880
      Width           =   2895
      Begin VB.TextBox txtDungLevels 
         Height          =   285
         Left            =   960
         TabIndex        =   42
         Top             =   720
         Width           =   1815
      End
      Begin VB.TextBox txtDungMonster 
         Height          =   285
         Left            =   1560
         TabIndex        =   40
         Top             =   360
         Width           =   1215
      End
      Begin VB.Label Label11 
         Caption         =   "Levels"
         Height          =   255
         Left            =   120
         TabIndex        =   41
         Top             =   720
         Width           =   1335
      End
      Begin VB.Label Label10 
         Caption         =   "Monster Str Factor"
         Height          =   255
         Left            =   120
         TabIndex        =   39
         Top             =   360
         Width           =   1575
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "Tile Set"
      Height          =   1335
      Left            =   120
      TabIndex        =   36
      Top             =   2520
      Width           =   6015
      Begin VB.OptionButton optTiles 
         Caption         =   "CastleTiles.bmp"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   43
         Tag             =   "CastleTiles.bmp"
         Top             =   960
         Width           =   2415
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "TownTiles.bmp"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   12
         Tag             =   "TownTiles.bmp"
         Top             =   600
         Width           =   2415
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "Tiles.bmp"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   11
         Tag             =   "Tiles.bmp"
         Top             =   240
         Width           =   1695
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "LOB TownTiles.bmp"
         Enabled         =   0   'False
         Height          =   255
         Index           =   4
         Left            =   2880
         TabIndex        =   14
         Tag             =   "LOB TownTiles.bmp"
         Top             =   600
         Width           =   3015
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "LOB Tiles.bmp"
         Enabled         =   0   'False
         Height          =   255
         Index           =   3
         Left            =   2880
         TabIndex        =   13
         Tag             =   "LOB Tiles.bmp"
         Top             =   240
         Width           =   3015
      End
   End
   Begin VB.CommandButton cmdDefaults 
      Caption         =   "Defaults"
      Height          =   375
      Left            =   3120
      TabIndex        =   28
      TabStop         =   0   'False
      Top             =   6000
      Width           =   975
   End
   Begin VB.Frame frmGuards 
      Caption         =   "Guards"
      Height          =   1815
      Left            =   3120
      TabIndex        =   18
      Top             =   3960
      Width           =   3015
      Begin VB.TextBox txtHP 
         Height          =   285
         Left            =   1200
         TabIndex        =   19
         Top             =   240
         Width           =   1695
      End
      Begin VB.TextBox txtAttack 
         Height          =   285
         Left            =   1200
         TabIndex        =   20
         Top             =   600
         Width           =   1695
      End
      Begin VB.TextBox txtDefense 
         Height          =   285
         Left            =   1200
         TabIndex        =   21
         Top             =   960
         Width           =   1695
      End
      Begin VB.TextBox txtColor 
         Height          =   285
         Left            =   1200
         TabIndex        =   23
         Top             =   1320
         Width           =   1695
      End
      Begin VB.Label Label4 
         Caption         =   "HP (+/- 10%)"
         Height          =   255
         Left            =   120
         TabIndex        =   26
         Top             =   240
         Width           =   1050
      End
      Begin VB.Label Label5 
         Caption         =   "Attack"
         Height          =   255
         Left            =   120
         TabIndex        =   25
         Top             =   600
         Width           =   1055
      End
      Begin VB.Label Label6 
         Caption         =   "Defense"
         Height          =   255
         Left            =   120
         TabIndex        =   24
         Top             =   960
         Width           =   1055
      End
      Begin VB.Label Label7 
         Caption         =   "Color (0=deflt)"
         Height          =   255
         Left            =   120
         TabIndex        =   22
         Top             =   1320
         Width           =   1055
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Map Type"
      Height          =   2295
      Left            =   120
      TabIndex        =   16
      Top             =   120
      Width           =   6015
      Begin MSComCtl2.UpDown upd 
         Height          =   285
         Left            =   4080
         TabIndex        =   37
         Top             =   1800
         Width           =   600
         _ExtentX        =   1058
         _ExtentY        =   503
         _Version        =   393216
         Value           =   1
         Alignment       =   0
         BuddyControl    =   "txtBpT"
         BuddyDispid     =   196636
         OrigLeft        =   5520
         OrigTop         =   1800
         OrigRight       =   5760
         OrigBottom      =   2055
         Max             =   2
         Min             =   1
         Orientation     =   1
         SyncBuddy       =   -1  'True
         BuddyProperty   =   65547
         Enabled         =   -1  'True
      End
      Begin VB.TextBox txtBpT 
         Height          =   285
         Left            =   4680
         Locked          =   -1  'True
         TabIndex        =   10
         Text            =   "1"
         Top             =   1800
         Width           =   1095
      End
      Begin VB.TextBox txtWidth 
         Height          =   285
         Left            =   2760
         TabIndex        =   8
         Top             =   1080
         Width           =   3015
      End
      Begin VB.TextBox txtHeight 
         Height          =   285
         Left            =   2760
         TabIndex        =   9
         Top             =   1440
         Width           =   3015
      End
      Begin VB.TextBox txtFileOffset 
         Height          =   285
         Left            =   2760
         TabIndex        =   7
         Top             =   720
         Width           =   3015
      End
      Begin VB.OptionButton optType 
         Caption         =   "Temple"
         Height          =   255
         Index           =   6
         Left            =   240
         TabIndex        =   5
         Top             =   1800
         Width           =   1250
      End
      Begin VB.TextBox txtName 
         Height          =   285
         Left            =   2760
         MaxLength       =   16
         TabIndex        =   6
         Top             =   360
         Width           =   3015
      End
      Begin VB.OptionButton optType 
         Caption         =   "Dungeon"
         Enabled         =   0   'False
         Height          =   255
         Index           =   4
         Left            =   240
         TabIndex        =   3
         Top             =   1224
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Castle"
         Height          =   255
         Index           =   5
         Left            =   240
         TabIndex        =   4
         Top             =   1512
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Town"
         Height          =   255
         Index           =   3
         Left            =   240
         TabIndex        =   2
         Top             =   936
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Outside"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   1
         Top             =   648
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Museum"
         Enabled         =   0   'False
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   0
         Top             =   360
         Width           =   1250
      End
      Begin VB.Label Label9 
         Caption         =   "Bytes / Tile (Recommend 1)"
         Height          =   255
         Left            =   1800
         TabIndex        =   33
         Top             =   1800
         Width           =   2175
      End
      Begin VB.Label Label8 
         Caption         =   "Height"
         Height          =   255
         Left            =   1800
         TabIndex        =   31
         Top             =   1440
         Width           =   855
      End
      Begin VB.Label Label3 
         Caption         =   "Width"
         Height          =   255
         Left            =   1800
         TabIndex        =   29
         Top             =   1080
         Width           =   855
      End
      Begin VB.Label Label2 
         Caption         =   "File Offset"
         Height          =   255
         Left            =   1800
         TabIndex        =   27
         Top             =   720
         Width           =   855
      End
      Begin VB.Label Label1 
         Caption         =   "Map Name"
         Height          =   255
         Left            =   1800
         TabIndex        =   17
         Top             =   360
         Width           =   975
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4200
      TabIndex        =   30
      TabStop         =   0   'False
      Top             =   6000
      Width           =   975
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   5280
      TabIndex        =   32
      TabStop         =   0   'False
      Top             =   6000
      Width           =   975
   End
End
Attribute VB_Name = "frmProperties"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public mType, theTiles
Public mbuyraftmap As Integer, mBuyRaftX As Integer, mBuyRaftY As Integer
Public setProperties


Private Sub cmdCancel_Click()
    SelectedOK = False
    
    Me.Hide
    
End Sub

Private Sub cmdOK_Click()
    Dim i As Integer
    
    SelectedOK = True
    
    For i = 1 To 6
        If optType(i) = True Then
            mType = i
        End If
        
    Next
    
    For i = 0 To 3
        If optTiles(i) = True Then
            theTiles = optTiles(i).Tag
        End If
    Next
    
    mBuyRaftX = txtBuyRaftX
    mBuyRaftY = txtBuyRaftY
    mbuyraftmap = txtBuyRaftMap
    
    Me.Hide
    
End Sub

Private Sub cmdDefaults_Click()
    If MsgBox("Restore all properties to defaults?", vbYesNo, "Defaults?") = vbYes Then
        SetDefaults
    End If
End Sub

Private Sub Form_Paint()
    Dim i As Integer
    
    If setProperties = True Then
        optType(mType) = True
        
        For i = 0 To 3
            If optTiles(i).Tag = theTiles Then
                optTiles(i) = True
            End If
        Next
        
        
        setProperties = False
    End If
    
    UpdateControls
End Sub

Private Sub optType_Click(Index As Integer)
    If Index = mapOutside Then
        optTiles(0) = True
    ElseIf Index = maptown Then
        optTiles(1) = True
    End If
    
    If Index = mapDungeon Then
        txtFileOffset = 256
        txtFileOffset.Locked = True
    Else
        txtFileOffset = 64
        txtFileOffset.Locked = False
    End If
    
    UpdateControls
End Sub

Private Sub txtAttack_Change()
    UpdateControls
End Sub

Private Sub txtBpT_Change()
    UpdateControls
End Sub

Private Sub txtColor_Change()
    UpdateControls
End Sub

Private Sub txtDefaultTile_Change()
    UpdateControls
End Sub

Private Sub txtDefense_Change()
    UpdateControls
End Sub

Private Sub txtFileOffset_Change()
    UpdateControls
End Sub

Private Sub txtHeight_Change()
    UpdateControls
End Sub

Private Sub txtHP_Change()
    UpdateControls
End Sub

Private Sub txtName_Change()
    UpdateControls
End Sub

Private Sub UpdateControls()
    Dim OKEnabled As Boolean
    Dim CharEnabled As Boolean
    Dim GuardsEnabled As Boolean
    Dim TilesEnabled As Boolean
    
    Dim i As Integer
    
    OKEnabled = False
    
    For i = 1 To 6
        If optType(i).value = True Then
            OKEnabled = True
            
            If i = maptown Or i = mapCastle Or i = mapTemple Then
                CharEnabled = True
            End If
        End If
    Next
    
    TilesEnabled = True
    
    If optType(mapDungeon) = True Or optType(mapMuseum) = True Then
        TilesEnabled = False
    End If
    
    If txtName.Text = "" Or txtBpT = "" Or txtFileOffset = "" Or txtHeight = "" Or txtWidth = "" Then
        OKEnabled = False
    End If
    
    If IsNumeric(txtWidth) And IsNumeric(txtHeight) Then
        If Val(txtWidth) < 1 Or Val(txtWidth) > maxMapSize Or Val(txtHeight) < 1 Or Val(txtHeight) > maxMapSize Then
            OKEnabled = False
        End If
    Else
        OKEnabled = False
    End If
    
    If Not IsNumeric(txtFileOffset) Then
        OKEnabled = False
    End If

    If optType(maptown).value = True Or optType(mapCastle).value = True Then
        GuardsEnabled = True
    Else
        GuardsEnabled = False
    End If
    
    If optType(mapDungeon) = True Then
        frmDungeon.Visible = True
        frmDungeon.Top = frmChar.Top
        
    Else
        frmDungeon.Visible = False
    End If
    
    cmdOK.Enabled = OKEnabled
    frmChar.Visible = CharEnabled
    frmGuards.Visible = GuardsEnabled
    
End Sub

Public Sub SetDefaults()
    Dim i As Integer
        
    For i = 1 To 6
        optType(i).value = False
    Next
    
    txtName.Text = ""
    
    txtFileOffset = ""
    
    
End Sub

Private Sub txtWidth_Change()
    UpdateControls
End Sub

