Option Explicit On
Friend Class frmStartup
	Inherits System.Windows.Forms.Form
	Private Sub cmdCreateMap_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCreateMap.Click
		frmProperties.SetDefaults()
		frmProperties.ShowDialog()
		
		
		If SelectedOK = True Then
			StartNewMap = True
			Me.Hide()
		End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Display.Dispose()
        End
	End Sub
	
	Private Sub cmdImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdImport.Click
        Try

            cmdDialogOpen.Title = "Import Map"

            'cmdDialogOpen.FileName = "E:\Legacy\."
            'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            cmdDialogOpen.Filter = "Export Files|*.export|All Files (*.*)|*.*"
            cmdDialogOpen.FilterIndex = 1

            cmdDialogOpen.InitialDirectory = My.Application.Info.DirectoryPath & "\..\Included Maps"

            cmdDialogOpen.DefaultExt = "map"

            cmdDialogOpen.ShowDialog()

            fileName = cmdDialogOpen.FileName

            StartNewMap = False
            ImportMap = True

            Me.Hide()

        Catch

        End Try

    End Sub
	
	Private Sub cmdOpenMap_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOpenMap.Click
		
        Try

            cmdDialogOpen.Title = "Open Map"

            'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            cmdDialogOpen.Filter = "All Map Files|*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
            cmdDialogOpen.FilterIndex = 1


            cmdDialogOpen.InitialDirectory = LotaPath & "\Included Maps"


            cmdDialogOpen.DefaultExt = "map"

            cmdDialogOpen.ShowDialog()

            fileName = cmdDialogOpen.FileName

            StartNewMap = False

            Me.Hide()


        Catch ex As Exception

        End Try
		
	End Sub
	
	Private Sub frmStartup_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		End
	End Sub
End Class