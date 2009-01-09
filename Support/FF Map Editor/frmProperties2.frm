VERSION 5.00
Begin VB.Form frmProperties2 
   Caption         =   "Map Properties"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form2"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   3360
      TabIndex        =   8
      Top             =   2520
      Width           =   1095
   End
   Begin VB.OptionButton Option 
      Caption         =   "Castle"
      Height          =   495
      Index           =   5
      Left            =   1080
      TabIndex        =   7
      Top             =   2400
      Width           =   1215
   End
   Begin VB.OptionButton Option 
      Caption         =   "Dungeon"
      Height          =   495
      Index           =   4
      Left            =   1080
      TabIndex        =   6
      Top             =   2040
      Width           =   1215
   End
   Begin VB.OptionButton Option 
      Caption         =   "Museum"
      Height          =   495
      Index           =   1
      Left            =   1080
      TabIndex        =   5
      Top             =   960
      Width           =   1215
   End
   Begin VB.OptionButton Option 
      Caption         =   "Town"
      Height          =   495
      Index           =   3
      Left            =   1080
      TabIndex        =   4
      Top             =   1680
      Width           =   1215
   End
   Begin VB.OptionButton Option 
      Caption         =   "Outside"
      Height          =   495
      Index           =   2
      Left            =   1080
      TabIndex        =   3
      Top             =   1320
      Width           =   1215
   End
   Begin VB.TextBox txtName 
      Height          =   285
      Left            =   1080
      MaxLength       =   16
      TabIndex        =   0
      Text            =   "Text1"
      Top             =   600
      Width           =   1815
   End
   Begin VB.Label Label2 
      Caption         =   "Map Type"
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   1080
      Width           =   975
   End
   Begin VB.Label Label1 
      Caption         =   "Map Name"
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   600
      Width           =   975
   End
End
Attribute VB_Name = "frmProperties2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Option1_Click()

End Sub

Private Sub cmdOK_Click()
    Me.Hide
    
End Sub

