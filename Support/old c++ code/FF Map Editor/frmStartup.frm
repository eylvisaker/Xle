VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmStartup 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Legacy of the Ancients Map Editor"
   ClientHeight    =   2145
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5265
   Icon            =   "frmStartup.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2145
   ScaleWidth      =   5265
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdImport 
      Caption         =   "&Import Existing Map"
      Height          =   495
      Left            =   3360
      TabIndex        =   3
      Top             =   960
      Width           =   1215
   End
   Begin MSComDlg.CommonDialog cmdDialog 
      Left            =   120
      Top             =   1560
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      CancelError     =   -1  'True
   End
   Begin VB.CommandButton cmdExit 
      Cancel          =   -1  'True
      Caption         =   "E&xit"
      Height          =   255
      Left            =   2160
      TabIndex        =   2
      Top             =   1680
      Width           =   855
   End
   Begin VB.CommandButton cmdOpenMap 
      Caption         =   "&Open Existing Map"
      Height          =   495
      Left            =   1920
      TabIndex        =   1
      Top             =   960
      Width           =   1335
   End
   Begin VB.CommandButton cmdCreateMap 
      Caption         =   "Create &New Map"
      Height          =   495
      Left            =   600
      TabIndex        =   0
      Top             =   960
      Width           =   1215
   End
   Begin VB.Image Image1 
      Height          =   480
      Left            =   1192
      Picture         =   "frmStartup.frx":0442
      Top             =   240
      Width           =   2880
   End
End
Attribute VB_Name = "frmStartup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub cmdCreateMap_Click()
    frmProperties.SetDefaults
    frmProperties.show 1
    
    
    If SelectedOK = True Then
       StartNewMap = True
       Me.Hide
    End If
    
End Sub

Private Sub cmdExit_Click()
    End
End Sub

Private Sub cmdImport_Click()
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
    
    Me.Hide
        

ErrorHandler:
End Sub

Private Sub cmdOpenMap_Click()

    On Local Error GoTo ErrorHandler
    
    cmdDialog.DialogTitle = "Open Map"
    
    cmdDialog.Filter = "All Map Files|*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
    cmdDialog.FilterIndex = 1
    
    cmdDialog.InitDir = App.path & "\..\Included Maps"
    
    cmdDialog.DefaultExt = "map"
    
    cmdDialog.ShowOpen
    
    fileName = cmdDialog.fileName
    
    StartNewMap = False
    
    Me.Hide
        

ErrorHandler:

End Sub

Private Sub Form_Unload(Cancel As Integer)
    End
End Sub
