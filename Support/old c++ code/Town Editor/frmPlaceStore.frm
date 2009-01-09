VERSION 5.00
Begin VB.Form frmPlaceStore 
   Caption         =   "Add Store"
   ClientHeight    =   5385
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6210
   LinkTopic       =   "Form2"
   ScaleHeight     =   5385
   ScaleWidth      =   6210
   StartUpPosition =   3  'Windows Default
   Begin VB.OptionButton optStoreType 
      Caption         =   "Food"
      Height          =   385
      Index           =   13
      Left            =   120
      TabIndex        =   23
      Top             =   4800
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Pawn"
      Height          =   385
      Index           =   12
      Left            =   120
      TabIndex        =   22
      Top             =   4440
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Flip Flop"
      Height          =   385
      Index           =   11
      Left            =   120
      TabIndex        =   21
      Top             =   4080
      Width           =   1785
   End
   Begin VB.CommandButton Command1 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   495
      Left            =   4920
      TabIndex        =   20
      Top             =   4800
      Width           =   1215
   End
   Begin VB.TextBox txtPrice 
      Height          =   285
      Left            =   3240
      TabIndex        =   17
      Text            =   "100"
      Top             =   1320
      Width           =   1095
   End
   Begin VB.TextBox txtY 
      Height          =   285
      Left            =   4440
      TabIndex        =   15
      Text            =   "Text3"
      Top             =   840
      Width           =   1215
   End
   Begin VB.TextBox txtX 
      Height          =   285
      Left            =   3240
      TabIndex        =   14
      Text            =   "Text2"
      Top             =   840
      Width           =   1095
   End
   Begin VB.TextBox txtName 
      Height          =   285
      Left            =   3240
      TabIndex        =   11
      Top             =   360
      Width           =   2655
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Fortune Teller"
      Height          =   385
      Index           =   10
      Left            =   120
      TabIndex        =   10
      Top             =   3720
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Weapon Training"
      Height          =   375
      Index           =   3
      Left            =   120
      TabIndex        =   9
      Top             =   1200
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Weapon"
      Height          =   375
      Index           =   1
      Left            =   120
      TabIndex        =   8
      Top             =   480
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Armor"
      Height          =   375
      Index           =   2
      Left            =   120
      TabIndex        =   7
      Top             =   840
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Armor Training"
      Height          =   375
      Index           =   4
      Left            =   120
      TabIndex        =   6
      Top             =   1560
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Blackjack"
      Height          =   375
      Index           =   5
      Left            =   120
      TabIndex        =   5
      Top             =   1920
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Lending"
      Height          =   375
      Index           =   6
      Left            =   120
      TabIndex        =   4
      Top             =   2280
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Raft / Climing gear"
      Height          =   375
      Index           =   7
      Left            =   120
      TabIndex        =   3
      Top             =   2640
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Healer"
      Height          =   375
      Index           =   8
      Left            =   120
      TabIndex        =   2
      Top             =   3000
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Jail"
      Height          =   375
      Index           =   9
      Left            =   120
      TabIndex        =   1
      Top             =   3360
      Width           =   1785
   End
   Begin VB.OptionButton optStoreType 
      Caption         =   "Bank"
      Height          =   375
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1785
   End
   Begin VB.Label lblSpecialData 
      Caption         =   "Label5"
      Height          =   255
      Left            =   3240
      TabIndex        =   19
      Top             =   4440
      Width           =   2895
   End
   Begin VB.Label Label4 
      Caption         =   "Special Data"
      Height          =   255
      Left            =   2040
      TabIndex        =   18
      Top             =   4440
      Width           =   1215
   End
   Begin VB.Label Label3 
      Caption         =   "Price Adjust"
      Height          =   255
      Left            =   2040
      TabIndex        =   16
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Label Label2 
      Caption         =   "Location"
      Height          =   255
      Left            =   2040
      TabIndex        =   13
      Top             =   840
      Width           =   1215
   End
   Begin VB.Label Label1 
      Caption         =   "Store Name:"
      Height          =   255
      Left            =   2040
      TabIndex        =   12
      Top             =   360
      Width           =   1215
   End
End
Attribute VB_Name = "frmPlaceStore"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
    Me.Hide
    
End Sub

Private Sub Form_Load()
    GetData
End Sub

Private Sub Label5_Click()

End Sub

Private Sub txtName_Change()
    UpdateData
End Sub

Private Sub txtPrice_Change()
    UpdateData
End Sub

Private Sub UpdateData()
    lblSpecialData = ""
    
    lblSpecialData = lblSpecialData & txtName & "\0" & Chr(txtPrice)
    
    
End Sub

Private Sub GetData()

End Sub
Private Sub txtX_Change()
    UpdateData
End Sub

Private Sub txtY_Change()
    UpdateData
End Sub
