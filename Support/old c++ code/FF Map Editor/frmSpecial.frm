VERSION 5.00
Begin VB.Form frmSpecial 
   Caption         =   "Form2"
   ClientHeight    =   6135
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7830
   LinkTopic       =   "Form2"
   ScaleHeight     =   6135
   ScaleWidth      =   7830
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame frmDoor 
      Caption         =   "Door"
      Height          =   1335
      Left            =   2880
      TabIndex        =   55
      Top             =   120
      Visible         =   0   'False
      Width           =   3015
      Begin VB.ComboBox cboDoorKey 
         Height          =   315
         ItemData        =   "frmSpecial.frx":0000
         Left            =   600
         List            =   "frmSpecial.frx":0010
         Style           =   2  'Dropdown List
         TabIndex        =   57
         Top             =   360
         Width           =   2055
      End
      Begin VB.Label Label10 
         Caption         =   "Key:"
         Height          =   255
         Left            =   120
         TabIndex        =   56
         Top             =   360
         Width           =   975
      End
   End
   Begin VB.Frame frmChest 
      Caption         =   "Treasure Chest"
      Height          =   1335
      Left            =   2880
      TabIndex        =   49
      Top             =   120
      Visible         =   0   'False
      Width           =   4335
      Begin VB.TextBox txtGold 
         Enabled         =   0   'False
         Height          =   285
         Left            =   1080
         TabIndex        =   53
         Top             =   720
         Width           =   1455
      End
      Begin VB.OptionButton optTreasure 
         Caption         =   "Gold:"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   52
         Top             =   720
         Width           =   735
      End
      Begin VB.OptionButton optTreasure 
         Caption         =   "Item:"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   51
         Top             =   360
         Width           =   735
      End
      Begin VB.ComboBox cboChestItem 
         Enabled         =   0   'False
         Height          =   315
         ItemData        =   "frmSpecial.frx":0040
         Left            =   1080
         List            =   "frmSpecial.frx":0089
         Style           =   2  'Dropdown List
         TabIndex        =   50
         Top             =   360
         Width           =   2295
      End
   End
   Begin VB.Frame frmSpeakWith 
      Caption         =   "Speak With"
      Height          =   2055
      Left            =   2880
      TabIndex        =   46
      Top             =   120
      Visible         =   0   'False
      Width           =   3735
      Begin VB.OptionButton optSpeakWith 
         Caption         =   "Wizard of Potions"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   59
         Top             =   720
         Width           =   2295
      End
      Begin VB.OptionButton optSpeakWith 
         Caption         =   "Cassandra"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   47
         Top             =   360
         Width           =   1335
      End
   End
   Begin VB.TextBox txtSpcHeight 
      Height          =   285
      Left            =   1440
      TabIndex        =   41
      Text            =   "1"
      Top             =   1560
      Width           =   1215
   End
   Begin VB.TextBox txtSpcWidth 
      Height          =   285
      Left            =   1440
      TabIndex        =   40
      Text            =   "1"
      Top             =   1200
      Width           =   1215
   End
   Begin VB.Frame Frame1 
      Caption         =   "Select Special Event Type"
      Height          =   3255
      Left            =   120
      TabIndex        =   33
      Top             =   2760
      Width           =   2655
      Begin VB.OptionButton optType 
         Caption         =   "Take"
         Height          =   195
         Index           =   25
         Left            =   120
         TabIndex        =   58
         Top             =   2760
         Width           =   2055
      End
      Begin VB.OptionButton optType 
         Caption         =   "Door"
         Height          =   195
         Index           =   24
         Left            =   120
         TabIndex        =   54
         Top             =   2400
         Width           =   1815
      End
      Begin VB.OptionButton optType 
         Caption         =   "Treasure Chest"
         Height          =   195
         Index           =   23
         Left            =   120
         TabIndex        =   48
         Top             =   2040
         Width           =   1815
      End
      Begin VB.OptionButton optType 
         Caption         =   "Magic Ice"
         Height          =   195
         Index           =   22
         Left            =   120
         TabIndex        =   44
         Top             =   1680
         Width           =   2415
      End
      Begin VB.OptionButton optType 
         Caption         =   "Map Change"
         Height          =   255
         Index           =   1
         Left            =   120
         TabIndex        =   2
         Top             =   360
         Width           =   2400
      End
      Begin VB.OptionButton optType 
         Caption         =   "Store"
         Height          =   255
         Index           =   2
         Left            =   120
         TabIndex        =   3
         Top             =   680
         Width           =   2400
      End
      Begin VB.OptionButton optType 
         Caption         =   "Teleport"
         Height          =   255
         Index           =   20
         Left            =   120
         TabIndex        =   4
         Top             =   1000
         Width           =   2400
      End
      Begin VB.OptionButton optType 
         Caption         =   "Special Conversation"
         Height          =   255
         Index           =   21
         Left            =   120
         TabIndex        =   5
         Top             =   1320
         Width           =   2400
      End
   End
   Begin VB.TextBox txtLocY 
      Height          =   285
      Left            =   1440
      TabIndex        =   1
      Text            =   "Text2"
      Top             =   720
      Width           =   1215
   End
   Begin VB.TextBox txtLocX 
      Height          =   285
      Left            =   1440
      TabIndex        =   0
      Text            =   "txtLocX"
      Top             =   360
      Width           =   1215
   End
   Begin VB.Frame frmMapChange 
      Caption         =   "Map Change"
      Height          =   1935
      Left            =   2880
      TabIndex        =   26
      Top             =   120
      Visible         =   0   'False
      Width           =   2655
      Begin VB.CheckBox chkAutoChange 
         Caption         =   "Change without asking?"
         Height          =   255
         Left            =   120
         TabIndex        =   43
         Top             =   1560
         Width           =   2415
      End
      Begin VB.TextBox txtNewX 
         Height          =   285
         Left            =   960
         TabIndex        =   7
         Top             =   720
         Width           =   1455
      End
      Begin VB.TextBox txtNewY 
         Height          =   285
         Left            =   960
         TabIndex        =   8
         Top             =   1080
         Width           =   1455
      End
      Begin VB.TextBox txtNewMap 
         Height          =   285
         Left            =   960
         TabIndex        =   6
         Top             =   360
         Width           =   1455
      End
      Begin VB.Label Label2 
         Caption         =   "X"
         Height          =   255
         Left            =   120
         TabIndex        =   31
         Top             =   720
         Width           =   735
      End
      Begin VB.Label Label3 
         Caption         =   "Y"
         Height          =   255
         Left            =   120
         TabIndex        =   30
         Top             =   1080
         Width           =   735
      End
      Begin VB.Label Label4 
         Caption         =   "New Map"
         Height          =   255
         Left            =   120
         TabIndex        =   28
         Top             =   360
         Width           =   1095
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   5280
      TabIndex        =   36
      Top             =   5640
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   6600
      TabIndex        =   37
      Top             =   5640
      Width           =   1095
   End
   Begin VB.Frame frmStore 
      Caption         =   "Store"
      Height          =   4455
      Left            =   2880
      TabIndex        =   9
      Top             =   120
      Width           =   4815
      Begin VB.OptionButton optStoreType 
         Caption         =   "Magic"
         Height          =   255
         Index           =   15
         Left            =   2280
         TabIndex        =   45
         Top             =   2880
         Width           =   1695
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Vault"
         Height          =   255
         Index           =   14
         Left            =   2280
         TabIndex        =   42
         Top             =   2520
         Width           =   2295
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Bank"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   10
         Top             =   360
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Jail"
         Height          =   255
         Index           =   9
         Left            =   2280
         TabIndex        =   19
         Top             =   720
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Healer"
         Height          =   255
         Index           =   8
         Left            =   2280
         TabIndex        =   18
         Top             =   360
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Raft / Climing gear"
         Height          =   255
         Index           =   7
         Left            =   240
         TabIndex        =   17
         Top             =   2880
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Lending"
         Height          =   255
         Index           =   6
         Left            =   240
         TabIndex        =   16
         Top             =   2520
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Blackjack"
         Height          =   255
         Index           =   5
         Left            =   240
         TabIndex        =   15
         Top             =   2160
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Armor Training"
         Height          =   255
         Index           =   4
         Left            =   240
         TabIndex        =   14
         Top             =   1800
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Armor"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   12
         Top             =   1080
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Weapon"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   11
         Top             =   720
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Weapon Training"
         Height          =   255
         Index           =   3
         Left            =   240
         TabIndex        =   13
         Top             =   1440
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Fortune Teller"
         Height          =   255
         Index           =   10
         Left            =   2280
         TabIndex        =   20
         Top             =   1080
         Width           =   1785
      End
      Begin VB.TextBox txtName 
         Height          =   285
         Left            =   1440
         TabIndex        =   27
         Top             =   3360
         Width           =   2655
      End
      Begin VB.TextBox txtPrice 
         Height          =   285
         Left            =   1440
         TabIndex        =   29
         Text            =   "100"
         Top             =   3720
         Width           =   1095
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Flip Flop"
         Height          =   255
         Index           =   11
         Left            =   2280
         TabIndex        =   21
         Top             =   1440
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Pawn"
         Height          =   255
         Index           =   12
         Left            =   2280
         TabIndex        =   22
         Top             =   1800
         Width           =   1785
      End
      Begin VB.OptionButton optStoreType 
         Caption         =   "Food"
         Height          =   255
         Index           =   13
         Left            =   2280
         TabIndex        =   23
         Top             =   2160
         Width           =   1785
      End
      Begin VB.Label Label8 
         Caption         =   "Store Name"
         Height          =   255
         Left            =   240
         TabIndex        =   25
         Top             =   3360
         Width           =   1215
      End
      Begin VB.Label Label6 
         Caption         =   "Price Factor"
         Height          =   255
         Left            =   240
         TabIndex        =   24
         Top             =   3720
         Width           =   1215
      End
   End
   Begin VB.Label Label9 
      Caption         =   "Height"
      Height          =   375
      Left            =   240
      TabIndex        =   39
      Top             =   1560
      Width           =   1335
   End
   Begin VB.Label Label1 
      Caption         =   "Width"
      Height          =   255
      Left            =   240
      TabIndex        =   38
      Top             =   1200
      Width           =   1215
   End
   Begin VB.Label lblSpecialData 
      Caption         =   "Label5"
      Height          =   495
      Left            =   2880
      TabIndex        =   35
      Top             =   4920
      Width           =   4575
   End
   Begin VB.Label Label5 
      Caption         =   "Special Data"
      Height          =   255
      Left            =   2880
      TabIndex        =   34
      Top             =   4680
      Width           =   1215
   End
   Begin VB.Label Label7 
      Caption         =   "Special Event Location"
      Height          =   495
      Left            =   240
      TabIndex        =   32
      Top             =   360
      Width           =   1215
   End
End
Attribute VB_Name = "frmSpecial"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public sType As Integer
Public setProperties As Boolean
Public sData As String
Public Changing As Boolean

Private Sub cboChestItem_Change()
    UpdateControls
End Sub

Private Sub cboChestItem_Click()
    UpdateControls
End Sub

Private Sub cboChestItem_LostFocus()
    UpdateControls
End Sub

Private Sub cboDoorKey_Change()
    UpdateControls
End Sub

Private Sub cboDoorKey_Click()
    UpdateControls
End Sub

Private Sub cboDoorKey_LostFocus()
    UpdateControls
End Sub

Private Sub chkAutoChange_Click()
    UpdateControls
End Sub

Private Sub cmdCancel_Click()
    SelectedOK = False
    
    Me.Hide
    
End Sub

Private Sub Form_Load()
    UpdateControls
    
End Sub
Private Sub Form_Paint()
    On Local Error Resume Next
    Dim a As Integer
    Dim b$, c, i
    Dim tSdata As String
    
    tSdata = sData
    
    If setProperties Then
          
        Changing = True
        
        If sType = 1 Or sType = 20 Then
            optType(sType).value = True
            
            a = Asc(Mid(tSdata, 1)) * 256
            a = a + Asc(Mid(tSdata, 2))
            
            txtNewMap = Str(a)
            
            a = Asc(Mid(tSdata, 3)) * 256
            a = a + Asc(Mid(tSdata, 4))
            
            txtNewX = Str(a)
            
            a = Asc(Mid(tSdata, 5)) * 256
            a = a + Asc(Mid(tSdata, 6))
            
            txtNewY = Str(a)
        
            If Asc(Mid(tSdata, 7)) = 11 Then
                chkAutoChange.value = 1
            Else
                chkAutoChange.value = 0
            End If
        
        ElseIf sType = 21 Then
            optType(sType).value = True
            optSpeakWith(Asc(sData)) = True
        
        ElseIf sType = 22 Then
            optType(sType).value = True
           ' do nothing else
            
        ElseIf sType = 23 Or sType = 25 Then
            optType(sType).value = True
            If Asc(Mid(sData, 1, 1)) = 0 Then
                optTreasure(0) = True
                cboChestItem.ListIndex = Asc(Mid(sData, 2, 1)) - 1
                
            Else
                optTreasure(1) = True
                txtGold = Asc(Mid(sData, 2, 1)) * 256 + Asc(Mid(sData, 3, 1))
            End If
            
        ElseIf sType = 24 Then
            
            optType(sType).value = True
            cboDoorKey.ListIndex = Asc(Mid(sData, 1, 1)) - 4
            
        ElseIf sType >= 2 And sType <= optStoreType.UBound + 2 Then
        
            optType(2).value = True
            optStoreType(sType - 2).value = True
            
            For i = 1 To Len(tSdata)
                c = Mid(tSdata, i, 1)
                
                If c <> "\" Then
                    b$ = b$ + c
                Else
                    Exit For
                End If
                
            Next
            
            txtName = b$
            
            i = i + 2
            
            txtPrice = Asc(Mid(tSdata, i, 1)) * 256 + Asc(Mid(tSdata, i + 1, 1))
                      
            
        End If
        
        setProperties = False
        
        Changing = False
        
        UpdateControls
    End If

    
End Sub

Private Sub UpdateControls()
    Dim a As Integer
    Dim mapChangeEnable As Boolean, storeEnable As Boolean, okEnable As Boolean
    Dim i As Integer
    Dim c As String * 1
    
    
    If Not Changing Then
        On Local Error Resume Next
        
        okEnable = True
        
        For i = optType.LBound To optType.UBound
            If optType(i).value = True Then
                a = 1
            End If
        Next
        
        If a = 0 Then okEnable = False
            
        sData = ""
                
        If Not IsNumeric(txtSpcWidth) And Not IsNumeric(txtSpcHeight) Then
            okEnable = False
        End If
        
        If optType(2).value = True Then
            storeEnable = True
            
            If txtName.Text = "" Or Not IsNumeric(txtPrice) Then
                okEnable = False
            End If
            
            
            sData = sData & txtName.Text & "\0" & Chr(Val(txtPrice) \ 256) & Chr(Val(txtPrice) Mod 256)
            
            a = 0
            
            For i = 0 To optStoreType.UBound
                If optStoreType(i).value = True Then
                    a = 1
                End If
            Next
            
            If a = 0 Then okEnable = False
            
        ElseIf optType(1).value = True Or optType(20).value = True Then
            mapChangeEnable = True
            
            If txtNewMap = "" Then okEnable = False
            If txtNewX = "" Then okEnable = False
            If txtNewY = "" Then okEnable = False
            
            a = Val(txtNewMap) \ 256
            sData = sData & Chr(a)
            
            a = Val(txtNewMap) Mod 256
            sData = sData & Chr(a)
            
            a = Val(txtNewX) \ 256
            sData = sData & Chr(a)
            
            a = Val(txtNewX) Mod 256
            sData = sData & Chr(a)
            
            a = Val(txtNewY) \ 256
            sData = sData & Chr(a)
            
            a = Val(txtNewY) Mod 256
            sData = sData & Chr(a)
            
            If chkAutoChange.value = 1 Then a = 11 Else a = 10
            sData = sData & Chr(a)
            
        ElseIf optType(21) = True Then
            
            a = optSpeakWith.LBound - 1
            For i = optSpeakWith.LBound To optSpeakWith.UBound
                If optSpeakWith(i) = True Then
                    a = i
                End If
                
            Next
            
            If a < optSpeakWith.LBound Then
                okEnable = False
            Else
                sData = sData & Chr(a)
            End If
            
        ElseIf optType(23) = True Or optType(25) = True Then
            
            If optTreasure(0) = True Then
                sData = sData & Chr(0)
                
                sData = sData & Chr(cboChestItem.ListIndex + 1)
                
            ElseIf optTreasure(1) = True Then
                sData = sData & Chr(1)
                
                If IsNumeric(txtGold) Then
                    sData = sData & Chr(txtGold \ 256)
                    sData = sData & Chr(txtGold Mod 256)
                Else
                
                    okEnable = False
                End If
                
            End If
            
        ElseIf optType(24) = True Then
            
            If cboDoorKey.ListIndex > -1 Then
                sData = sData & Chr(cboDoorKey.ListIndex + 4)
            Else
                okEnable = False
            End If
            
        End If
        
        lblSpecialData = ""
        For i = 1 To Len(sData)
            lblSpecialData = lblSpecialData & Hex$(Asc(Mid(sData, i, 1))) & " "
        Next
        
        frmStore.Visible = storeEnable
        frmMapChange.Visible = mapChangeEnable
        cmdOK.Enabled = okEnable
    
    End If
    
End Sub


Private Sub cmdOK_Click()
    UpdateControls
    
    Dim i As Integer
    
    SelectedOK = True
    
    sType = 0

    If optType(2) Then
        For i = 0 To optStoreType.Count - 1
            If optStoreType(i).value = True Then
                sType = i + 2
            End If
        Next
    ElseIf optType(1) Then
        sType = 1
    Else
        For i = 20 To optType.UBound
            If optType(i) = True Then
                sType = i
            End If
        Next
        
        
    End If
    
    'sData = lblSpecialData
    
    Me.Hide
    
    
End Sub

Private Sub optSpeakWith_Click(Index As Integer)
    UpdateControls
End Sub

Private Sub optTreasure_Click(Index As Integer)
    cboChestItem.Enabled = False
    txtGold.Enabled = False
    
    If Index = 0 Then cboChestItem.Enabled = True
    If Index = 1 Then txtGold.Enabled = True
    
    UpdateControls
End Sub

Private Sub optType_Click(Index As Integer)
    frmSpeakWith.Visible = False
    frmStore.Visible = False
    frmMapChange.Visible = False
    frmChest.Visible = False
    frmDoor.Visible = False
    
    
    Select Case Index
    Case 2
        frmStore.Visible = True
    Case 1, 20
        frmMapChange.Visible = True
    Case 21
        frmSpeakWith.Visible = True
    Case 23, 25
        frmChest.Visible = True
    Case 24
        frmDoor.Visible = True
    End Select
    
    UpdateControls
End Sub

Public Sub SetDefaults()
    Dim i As Integer
    
    optType(1) = False
    optType(2) = False
    optType(20) = False
    optType(21) = False
    
    For i = 0 To optStoreType.Count - 1
        optStoreType(i) = 0
    Next
    
    txtLocX = x1
    txtLocY = y1
    
    txtName = ""
    txtNewX = ""
    txtNewY = ""
    
    txtGold = ""
    cboDoorKey.ListIndex = -1
    cboChestItem.ListIndex = -1
    
    optTreasure(0) = False
    optTreasure(0) = False
    
    txtSpcWidth = "1"
    txtSpcHeight = "1"
        
    txtPrice = "100"
    
    UpdateControls
    
End Sub

Private Sub txtGold_Change()
    UpdateControls
End Sub

Private Sub txtName_Change()
    UpdateControls
End Sub

Private Sub txtName_LostFocus()
    UpdateControls
End Sub

Private Sub txtNewX_LostFocus()
    UpdateControls
End Sub
Private Sub txtNewY_LostFocus()
    UpdateControls
End Sub

Private Sub txtNewMap_LostFocus()
    UpdateControls
End Sub

Private Sub txtPrice_Change()
    UpdateControls
End Sub
