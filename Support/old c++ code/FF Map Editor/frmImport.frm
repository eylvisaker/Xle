VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmImport 
   Caption         =   "Import Parameters"
   ClientHeight    =   6120
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   6300
   LinkTopic       =   "Form1"
   ScaleHeight     =   6120
   ScaleWidth      =   6300
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtAreaHeight 
      Height          =   285
      Left            =   960
      TabIndex        =   30
      Text            =   "Text1"
      Top             =   4800
      Width           =   735
   End
   Begin VB.TextBox txtAreaWidth 
      Height          =   285
      Left            =   960
      TabIndex        =   26
      Text            =   "Text1"
      Top             =   4440
      Width           =   735
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Reset Mappings..."
      Height          =   615
      Left            =   120
      TabIndex        =   25
      Top             =   5280
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4080
      TabIndex        =   24
      Top             =   5520
      Width           =   975
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   5160
      TabIndex        =   23
      Top             =   5520
      Width           =   975
   End
   Begin VB.Frame Frame2 
      Caption         =   "Tile Set"
      Height          =   1335
      Left            =   120
      TabIndex        =   17
      Top             =   2640
      Width           =   6015
      Begin VB.OptionButton optTiles 
         Caption         =   "LOB Tiles.bmp"
         Enabled         =   0   'False
         Height          =   255
         Index           =   3
         Left            =   2880
         TabIndex        =   22
         Tag             =   "LOB Tiles.bmp"
         Top             =   240
         Width           =   3015
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "LOB TownTiles.bmp"
         Enabled         =   0   'False
         Height          =   255
         Index           =   4
         Left            =   2880
         TabIndex        =   21
         Tag             =   "LOB TownTiles.bmp"
         Top             =   600
         Width           =   3015
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "Tiles.bmp"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   20
         Tag             =   "Tiles.bmp"
         Top             =   240
         Width           =   1695
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "TownTiles.bmp"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   19
         Tag             =   "TownTiles.bmp"
         Top             =   600
         Width           =   2415
      End
      Begin VB.OptionButton optTiles 
         Caption         =   "CastleTiles.bmp"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   18
         Tag             =   "CastleTiles.bmp"
         Top             =   960
         Width           =   2415
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Map Type"
      Height          =   2295
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   6015
      Begin VB.CheckBox chkAuto 
         Caption         =   "Auto"
         Height          =   255
         Left            =   4920
         TabIndex        =   16
         Top             =   1080
         Width           =   855
      End
      Begin VB.OptionButton optType 
         Caption         =   "Museum"
         Enabled         =   0   'False
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   11
         Top             =   360
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Outside"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   10
         Top             =   648
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Town"
         Height          =   255
         Index           =   3
         Left            =   240
         TabIndex        =   9
         Top             =   936
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Castle"
         Height          =   255
         Index           =   5
         Left            =   240
         TabIndex        =   8
         Top             =   1512
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Dungeon"
         Enabled         =   0   'False
         Height          =   255
         Index           =   4
         Left            =   240
         TabIndex        =   7
         Top             =   1224
         Width           =   1250
      End
      Begin VB.OptionButton optType 
         Caption         =   "Temple"
         Height          =   255
         Index           =   6
         Left            =   240
         TabIndex        =   6
         Top             =   1800
         Width           =   1250
      End
      Begin VB.TextBox txtFileOffset 
         Height          =   285
         Left            =   2760
         TabIndex        =   5
         Top             =   360
         Width           =   855
      End
      Begin VB.TextBox txtHeight 
         Height          =   285
         Left            =   2760
         TabIndex        =   4
         Top             =   1440
         Width           =   1935
      End
      Begin VB.TextBox txtWidth 
         Height          =   285
         Left            =   2760
         TabIndex        =   3
         Top             =   1080
         Width           =   1935
      End
      Begin VB.TextBox txtBpT 
         Height          =   285
         Left            =   4680
         Locked          =   -1  'True
         TabIndex        =   2
         Text            =   "1"
         Top             =   1800
         Width           =   1095
      End
      Begin MSComCtl2.UpDown upd 
         Height          =   285
         Left            =   4080
         TabIndex        =   1
         Top             =   1800
         Width           =   600
         _ExtentX        =   1058
         _ExtentY        =   503
         _Version        =   393216
         Value           =   1
         Alignment       =   0
         OrigLeft        =   5520
         OrigTop         =   1800
         OrigRight       =   5760
         OrigBottom      =   2055
         Max             =   2
         Min             =   1
         Orientation     =   1
         Enabled         =   -1  'True
      End
      Begin VB.Label Label2 
         Caption         =   "File Offset"
         Height          =   255
         Left            =   1800
         TabIndex        =   15
         Top             =   360
         Width           =   855
      End
      Begin VB.Label Label3 
         Caption         =   "Width"
         Height          =   255
         Left            =   1800
         TabIndex        =   14
         Top             =   1080
         Width           =   855
      End
      Begin VB.Label Label8 
         Caption         =   "Height"
         Height          =   255
         Left            =   1800
         TabIndex        =   13
         Top             =   1440
         Width           =   855
      End
      Begin VB.Label Label9 
         Caption         =   "Bytes / Tile (Recommend 1)"
         Height          =   255
         Left            =   1800
         TabIndex        =   12
         Top             =   1800
         Width           =   2175
      End
   End
   Begin VB.Label Label5 
      Caption         =   "Height"
      Height          =   255
      Left            =   360
      TabIndex        =   29
      Top             =   4800
      Width           =   615
   End
   Begin VB.Label Label4 
      Caption         =   "Width"
      Height          =   255
      Left            =   360
      TabIndex        =   28
      Top             =   4440
      Width           =   855
   End
   Begin VB.Label Label1 
      Caption         =   "Area Mappings"
      Height          =   255
      Left            =   120
      TabIndex        =   27
      Top             =   4080
      Width           =   1095
   End
End
Attribute VB_Name = "frmImport"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public mType, theTiles
Public ClickedOK As Boolean

Private Sub chkAuto_Click()
    UpdateControls
    
    
End Sub

Public Sub SetDefaults()
    Dim i As Integer
        
    For i = 1 To 6
        optType(i).value = False
    Next
    
    txtFileOffset = "0"
    txtAreaWidth = "1"
    txtAreaHeight = "1"
    
    chkAuto.value = 1
    
End Sub


Private Sub cmdCancel_Click()
    
    ClickedOK = False
    Me.Hide
    
End Sub

Private Sub cmdOK_Click()
    Dim i As Integer
    
    If chkAuto Then AutoHeightWidth = True
    
    ImportOffset = txtFileOffset
    AreaWidth = txtAreaWidth
    AreaHeight = txtAreaHeight
    
    
    For i = 1 To 6
        If optType(i) = True Then
            MapType = i
        End If
        
    Next
    
    For i = 0 To 3
        If optTiles(i) = True Then
            TileSet = optTiles(i).Tag
        End If
    Next
    

    RecalibrateImport
        
    ClickedOK = True
    Me.Hide
    
End Sub

Private Sub optTiles_Click(Index As Integer)
    UpdateControls
End Sub

Private Sub optType_Click(Index As Integer)
    If Index = mapOutside Then
        optTiles(0) = True
    ElseIf Index = maptown Then
        optTiles(1) = True
    ElseIf Index = mapCastle Then
        optTiles(2) = True
    End If
  
    UpdateControls
End Sub

Private Sub UpdateControls()
    Dim i As Integer, ct As Integer
    Dim invalid As Boolean
    
    invalid = False
    
    If chkAuto Then
        txtWidth.Enabled = False
        txtHeight.Enabled = False
    Else
        If Not IsNumeric(txtWidth) Then invalid = True
        If Not IsNumeric(txtHeight) Then invalid = True
        
        txtWidth.Enabled = True
        txtHeight.Enabled = True
        
    End If
    
    
    For i = 1 To 6
        If optType(i).value = True Then
            Exit For
        End If
    Next
    
    If i = 7 Then invalid = True
    
    For i = 0 To 4
        If optTiles(i).value = True Then
            Exit For
        End If
        
    Next
    
    If i = 5 Then invalid = True
    
    If Not IsNumeric(txtFileOffset) Then invalid = True
    If Not IsNumeric(txtAreaWidth) Then invalid = True
    If Not IsNumeric(txtAreaHeight) Then invalid = True
    
    If invalid Then
        cmdOK.Enabled = False
    Else
        cmdOK.Enabled = True
    End If
    
End Sub

Private Sub txtAreaHeight_Change()
    UpdateControls
    
End Sub

Private Sub txtAreaWidth_Change()
    UpdateControls
    
End Sub

Private Sub txtFileOffset_Change()
    UpdateControls
    
End Sub

Private Sub txtHeight_Change()
    UpdateControls
    
End Sub

Private Sub txtWidth_Change()
    UpdateControls
End Sub
