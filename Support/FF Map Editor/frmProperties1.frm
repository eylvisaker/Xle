VERSION 5.00
Begin VB.Form frmProperties1 
   Caption         =   "Map Properties"
   ClientHeight    =   2550
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6420
   LinkTopic       =   "Form2"
   ScaleHeight     =   2550
   ScaleWidth      =   6420
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame frmGuards 
      Caption         =   "Guards"
      Height          =   1815
      Left            =   3240
      TabIndex        =   9
      Top             =   120
      Width           =   3015
      Begin VB.TextBox txtColor 
         Height          =   285
         Left            =   1200
         TabIndex        =   17
         Text            =   "Text4"
         Top             =   1320
         Width           =   1695
      End
      Begin VB.TextBox txtDefense 
         Height          =   285
         Left            =   1200
         TabIndex        =   12
         Text            =   "Text3"
         Top             =   960
         Width           =   1695
      End
      Begin VB.TextBox txtAttack 
         Height          =   285
         Left            =   1200
         TabIndex        =   11
         Text            =   "Text2"
         Top             =   600
         Width           =   1695
      End
      Begin VB.TextBox txtHP 
         Height          =   285
         Left            =   1200
         TabIndex        =   10
         Text            =   "Text1"
         Top             =   240
         Width           =   1695
      End
      Begin VB.Label Label7 
         Caption         =   "Color (0=deflt)"
         Height          =   255
         Left            =   120
         TabIndex        =   16
         Top             =   1320
         Width           =   1055
      End
      Begin VB.Label Label6 
         Caption         =   "Defense"
         Height          =   255
         Left            =   120
         TabIndex        =   15
         Top             =   960
         Width           =   1055
      End
      Begin VB.Label Label5 
         Caption         =   "Attack"
         Height          =   255
         Left            =   120
         TabIndex        =   14
         Top             =   600
         Width           =   1055
      End
      Begin VB.Label Label4 
         Caption         =   "HP (+/- 10%)"
         Height          =   255
         Left            =   120
         TabIndex        =   13
         Top             =   240
         Width           =   1055
      End
   End
   Begin VB.TextBox txtMapType 
      Height          =   285
      Left            =   1080
      TabIndex        =   8
      Text            =   "Text1"
      Top             =   1320
      Width           =   2005
   End
   Begin VB.TextBox txtFileOffset 
      Height          =   285
      Left            =   1080
      TabIndex        =   6
      Top             =   960
      Width           =   2005
   End
   Begin VB.TextBox txtDefaultTile 
      Height          =   285
      Left            =   1080
      TabIndex        =   4
      Text            =   "Text1"
      Top             =   600
      Width           =   2005
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   4680
      TabIndex        =   2
      Top             =   2040
      Width           =   1215
   End
   Begin VB.TextBox txtName 
      Height          =   285
      Left            =   1080
      MaxLength       =   16
      TabIndex        =   0
      Text            =   "Text1"
      Top             =   240
      Width           =   2005
   End
   Begin VB.Label Label3 
      Caption         =   "Map Type:"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   1320
      Width           =   975
   End
   Begin VB.Label Label2 
      Caption         =   "File Offset:"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   960
      Width           =   855
   End
   Begin VB.Label lblDefaultTile 
      Caption         =   "Default Tile:"
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   600
      Width           =   975
   End
   Begin VB.Label Label1 
      Caption         =   "Map Name:"
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   240
      Width           =   1215
   End
End
Attribute VB_Name = "frmProperties1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdOK_Click()
    Me.Hide
    
End Sub

Private Sub Frame1_DragDrop(Source As Control, X As Single, Y As Single)

End Sub

