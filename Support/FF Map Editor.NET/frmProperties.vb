Option Explicit On
Friend Class frmProperties
	Inherits System.Windows.Forms.Form
    Public mType As Integer, theTiles As String
    Public mBuyRaftX, mbuyraftmap, mBuyRaftY As Integer
    Public setProperties As Boolean


    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        SelectedOK = False

        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Dim i As Short

        SelectedOK = True

        For i = 1 To 6
            If optType(i).Checked = True Then
                'UPGRADE_WARNING: Couldn't resolve default property of object mType. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                mType = i
            End If

        Next

        For i = 0 To 3
            If optTiles(i).Checked = True Then
                'UPGRADE_WARNING: Couldn't resolve default property of object theTiles. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                theTiles = optTiles(i).Tag.ToString()
            End If
        Next

        mBuyRaftX = Integer.Parse(txtBuyRaftX.Text)
        mBuyRaftY = Integer.Parse(txtBuyRaftY.Text)
        mbuyraftmap = Integer.Parse(txtBuyRaftMap.Text)

        Me.Hide()

    End Sub

    Private Sub cmdDefaults_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDefaults.Click
        If MsgBox("Restore all properties to defaults?", MsgBoxStyle.YesNo, "Defaults?") = MsgBoxResult.Yes Then
            SetDefaults()
        End If
    End Sub

    Private Sub frmProperties_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cboTypes.Items.AddRange(XleFactory.MapTypes.ToArray())
    End Sub

    Private Sub frmProperties_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Dim i As Short

        'UPGRADE_WARNING: Couldn't resolve default property of object setProperties. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If setProperties = True Then
            optType(CShort(mType)).Checked = True

            For i = 0 To 3
                'UPGRADE_WARNING: Couldn't resolve default property of object theTiles. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If optTiles(i).Tag.ToString() = theTiles Then
                    optTiles(i).Checked = True
                End If
            Next


            'UPGRADE_WARNING: Couldn't resolve default property of object setProperties. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            setProperties = False
        End If

        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event optType.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        Dim sender As RadioButton = DirectCast(eventSender, RadioButton)

        If sender.Checked Then
            Dim Index As Integer = optType.GetIndex(sender)
            If Index = MainModule.EnumMapType.mapOutside Then
                optTiles(0).Checked = True
            ElseIf Index = MainModule.EnumMapType.maptown Then
                optTiles(1).Checked = True
            End If

            UpdateControls()
        End If
    End Sub

    'UPGRADE_WARNING: Event txtAttack.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtAttack_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtAttack.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtBpT.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtBpT_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtColor.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtColor_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtColor.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtDefaultTile.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtDefaultTile_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDefaultTile.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtDefense.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtDefense_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDefense.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtFileOffset.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtFileOffset_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtHeight.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtHeight_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtHeight.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtHP.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtHP_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtHP.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event txtName.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtName_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtName.TextChanged
        UpdateControls()
    End Sub

    Private Sub UpdateControls()
        Dim OKEnabled As Boolean
        Dim CharEnabled As Boolean
        Dim GuardsEnabled As Boolean
        Dim TilesEnabled As Boolean

        Dim i As Short

        OKEnabled = False

        For i = 1 To 6
            If optType(i).Checked = True Then
                OKEnabled = True

                If i = MainModule.EnumMapType.maptown Or i = MainModule.EnumMapType.mapCastle Or i = MainModule.EnumMapType.mapTemple Then
                    CharEnabled = True
                End If
            End If
        Next

        TilesEnabled = True

        If optType(CShort(MainModule.EnumMapType.mapDungeon)).Checked = True Or optType(CShort(MainModule.EnumMapType.mapMuseum)).Checked = True Then
            TilesEnabled = False
        End If

        If txtName.Text = "" Or txtHeight.Text = "" Or txtWidth.Text = "" Then
            OKEnabled = False
        End If

        If IsNumeric(txtWidth.Text) And IsNumeric(txtHeight.Text) Then
            If Val(txtWidth.Text) < 1 Or Val(txtWidth.Text) > maxMapSize Or Val(txtHeight.Text) < 1 Or Val(txtHeight.Text) > maxMapSize Then
                OKEnabled = False
            End If
        Else
            OKEnabled = False
        End If

        ' TODO: Fix these:
        'If optType(MainModule.EnumMapType.maptown).Checked = True Or optType(MainModule.EnumMapType.mapCastle).Checked = True Then
        '    GuardsEnabled = True
        'Else
        '    GuardsEnabled = False
        'End If

        'If optType(MainModule.EnumMapType.mapDungeon).Checked = True Then
        '    frmDungeon.Visible = True
        '    frmDungeon.Top = frmChar.Top

        'Else
        '    frmDungeon.Visible = False
        'End If

        cmdOK.Enabled = OKEnabled
        frmChar.Visible = CharEnabled
        frmGuards.Visible = GuardsEnabled

    End Sub

    Public Sub SetDefaults()
        Dim i As Short

        For i = 1 To 6
            optType(i).Checked = False
        Next

        txtName.Text = ""

    End Sub
	
	'UPGRADE_WARNING: Event txtWidth.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtWidth_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtWidth.TextChanged
		UpdateControls()
	End Sub
End Class