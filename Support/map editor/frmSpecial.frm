VERSION 5.00
Begin VB.Form frmSpecial 
   Caption         =   "Form2"
   ClientHeight    =   2760
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6180
   LinkTopic       =   "Form2"
   ScaleHeight     =   2760
   ScaleWidth      =   6180
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   495
      Left            =   4560
      TabIndex        =   8
      Top             =   2040
      Width           =   1215
   End
   Begin VB.TextBox txtNewMap 
      Height          =   285
      Left            =   4320
      TabIndex        =   5
      Top             =   240
      Width           =   1575
   End
   Begin VB.TextBox txtNewY 
      Height          =   285
      Left            =   4320
      TabIndex        =   7
      Top             =   960
      Width           =   1575
   End
   Begin VB.TextBox txtNewX 
      Height          =   285
      Left            =   4320
      TabIndex        =   6
      Top             =   600
      Width           =   1575
   End
   Begin VB.OptionButton Option 
      Caption         =   "Option5"
      Height          =   495
      Index           =   5
      Left            =   360
      TabIndex        =   4
      Top             =   2040
      Width           =   2400
   End
   Begin VB.OptionButton Option 
      Caption         =   "Option4"
      Height          =   495
      Index           =   4
      Left            =   360
      TabIndex        =   3
      Top             =   1680
      Width           =   2400
   End
   Begin VB.OptionButton Option 
      Caption         =   "Option3"
      Height          =   495
      Index           =   3
      Left            =   360
      TabIndex        =   2
      Top             =   1320
      Width           =   2400
   End
   Begin VB.OptionButton Option 
      Caption         =   "Option2"
      Height          =   495
      Index           =   2
      Left            =   360
      TabIndex        =   1
      Top             =   960
      Width           =   2400
   End
   Begin VB.OptionButton Option 
      Caption         =   "Map Change"
      Height          =   255
      Index           =   1
      Left            =   360
      TabIndex        =   0
      Top             =   720
      Width           =   2400
   End
   Begin VB.Label Label4 
      Caption         =   "New Map"
      Height          =   255
      Left            =   3600
      TabIndex        =   12
      Top             =   240
      Width           =   1095
   End
   Begin VB.Label Label3 
      Caption         =   "Y"
      Height          =   255
      Left            =   3600
      TabIndex        =   11
      Top             =   960
      Width           =   735
   End
   Begin VB.Label Label2 
      Caption         =   "X"
      Height          =   255
      Left            =   3600
      TabIndex        =   10
      Top             =   600
      Width           =   735
   End
   Begin VB.Label Label1 
      Caption         =   "Select special type:"
      Height          =   255
      Left            =   240
      TabIndex        =   9
      Top             =   360
      Width           =   1815
   End
End
Attribute VB_Name = "frmSpecial"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Option2_Click()

End Sub

Private Sub Command1_Click()
    Me.Hide
End Sub

