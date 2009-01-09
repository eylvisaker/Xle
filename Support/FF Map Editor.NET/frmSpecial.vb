Option Explicit On
Friend Class frmSpecial
	Inherits System.Windows.Forms.Form
	
    Public sType As Integer
    Public setProperties As Boolean
    Public sData As New VB6.FixedLengthString(100)
    Public Changing As Boolean

    'UPGRADE_WARNING: Event cboChestItem.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    'UPGRADE_WARNING: ComboBox event cboChestItem.Change was upgraded to cboChestItem.TextChanged which has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
    Private Sub cboChestItem_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboChestItem.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event cboChestItem.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub cboChestItem_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboChestItem.SelectedIndexChanged
        UpdateControls()
    End Sub

    Private Sub cboChestItem_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboChestItem.Leave
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event cboDoorKey.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    'UPGRADE_WARNING: ComboBox event cboDoorKey.Change was upgraded to cboDoorKey.TextChanged which has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
    Private Sub cboDoorKey_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboDoorKey.TextChanged
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event cboDoorKey.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub cboDoorKey_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboDoorKey.SelectedIndexChanged
        UpdateControls()
    End Sub

    Private Sub cboDoorKey_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboDoorKey.Leave
        UpdateControls()
    End Sub

    'UPGRADE_WARNING: Event chkAutoChange.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkAutoChange_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkAutoChange.CheckStateChanged
        UpdateControls()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        SelectedOK = False

        Me.Hide()

    End Sub

    Private Sub frmSpecial_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        UpdateControls()

    End Sub
    Private Sub frmSpecial_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Dim a, i As Integer
        Dim b As String
        Dim c As String
        Dim tSdata As String

        tSdata = sData.Value

        If setProperties Then

            Changing = True

            If sType = 1 Or sType = 20 Then
                optType(CShort(sType)).Checked = True

                a = Asc(Mid(tSdata, 1)) * 256
                a = a + Asc(Mid(tSdata, 2))

                txtNewMap.Text = Str(a)

                a = Asc(Mid(tSdata, 3)) * 256
                a = a + Asc(Mid(tSdata, 4))

                txtNewX.Text = Str(a)

                a = Asc(Mid(tSdata, 5)) * 256
                a = a + Asc(Mid(tSdata, 6))

                txtNewY.Text = Str(a)

                If Asc(Mid(tSdata, 7)) = 11 Then
                    chkAutoChange.CheckState = System.Windows.Forms.CheckState.Checked
                Else
                    chkAutoChange.CheckState = System.Windows.Forms.CheckState.Unchecked
                End If

            ElseIf sType = 21 Then
                optType(CShort(sType)).Checked = True
                optSpeakWith(CShort(Asc(sData.Value))).Checked = True

            ElseIf sType = 22 Then
                optType(CShort(sType)).Checked = True
                ' do nothing else

            ElseIf sType = 23 Or sType = 25 Then
                optType(CShort(sType)).Checked = True
                If Asc(Mid(tSdata, 1, 1)) = 0 Then
                    optTreasure(0).Checked = True
                    cboChestItem.SelectedIndex = Asc(Mid(tSdata, 2, 1)) - 1

                Else
                    optTreasure(1).Checked = True
                    txtGold.Text = CStr(Asc(Mid(tSdata, 2, 1)) * 256 + Asc(Mid(tSdata, 3, 1)))
                End If

            ElseIf sType = 24 Then

                optType(CShort(sType)).Checked = True
                cboDoorKey.SelectedIndex = Asc(Mid(tSdata, 1, 1)) - 4

            ElseIf sType >= 2 And sType <= optStoreType.UBound + 2 Then

                optType(2).Checked = True
                optStoreType(CShort(sType - 2)).Checked = True

                For i = 1 To Len(tSdata)
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object c. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    c = Mid(tSdata, i, 1)

                    'UPGRADE_WARNING: Couldn't resolve default property of object c. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If c <> "\" Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object c. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        b = b + c
                    Else
                        Exit For
                    End If

                Next

                txtName.Text = b

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                i = i + 2

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                txtPrice.Text = CStr(Asc(Mid(tSdata, i, 1)) * 256 + Asc(Mid(tSdata, i + 1, 1)))


            End If

            setProperties = False

            Changing = False

            UpdateControls()
        End If


    End Sub

    Private Sub UpdateControls()
        Dim a As Integer
        Dim storeEnable, mapChangeEnable, okEnable As Boolean
        Dim i As Integer
        Dim c As Char


        If Not Changing Then
            'On Error Resume Next

            okEnable = True

            For i = optType.LBound To optType.UBound
                If optType(CShort(i)).Checked = True Then
                    a = 1
                End If
            Next

            If a = 0 Then okEnable = False

            sData = New VB6.FixedLengthString(100)

            If Not IsNumeric(txtSpcWidth.Text) And Not IsNumeric(txtSpcHeight.Text) Then
                okEnable = False
            End If

            If optType(2).Checked = True Then
                storeEnable = True

                If txtName.Text = "" Or Not IsNumeric(txtPrice.Text) Then
                    okEnable = False
                End If


                'UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                sData.Value = sData.Value & txtName.Text & "\0" & Chr(Integer.Parse(txtPrice.Text) \ 256) & Chr(Integer.Parse(txtPrice.Text) Mod 256)

                a = 0

                For i = 0 To optStoreType.UBound
                    If optStoreType(CShort(i)).Checked = True Then
                        a = 1
                    End If
                Next

                If a = 0 Then okEnable = False

            ElseIf optType(1).Checked = True Or optType(20).Checked = True Then
                mapChangeEnable = True

                If txtNewMap.Text = "" Then okEnable = False
                If txtNewX.Text = "" Then okEnable = False
                If txtNewY.Text = "" Then okEnable = False

                a = Integer.Parse(txtNewMap.Text) \ 256
                sData.Value = sData.Value & Chr(a)

                'UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                a = Integer.Parse(txtNewMap.Text) Mod 256
                sData.Value = sData.Value & Chr(a)

                a = Integer.Parse(txtNewX.Text) \ 256
                sData.Value = sData.Value & Chr(a)

                'UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                a = Integer.Parse(txtNewX.Text) Mod 256
                sData.Value = sData.Value & Chr(a)

                a = Integer.Parse(txtNewY.Text) \ 256
                sData.Value = sData.Value & Chr(a)

                'UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                a = Integer.Parse(txtNewY.Text) Mod 256
                sData.Value = sData.Value & Chr(a)

                If chkAutoChange.CheckState = 1 Then a = 11 Else a = 10
                sData.Value = sData.Value & Chr(a)

            ElseIf optType(21).Checked = True Then

                a = optSpeakWith.LBound - 1
                For i = optSpeakWith.LBound To optSpeakWith.UBound
                    If optSpeakWith(CShort(i)).Checked = True Then
                        a = i
                    End If

                Next

                If a < optSpeakWith.LBound Then
                    okEnable = False
                Else
                    sData.Value = sData.Value & Chr(a)
                End If

            ElseIf optType(23).Checked = True Or optType(25).Checked = True Then

                If optTreasure(0).Checked = True Then
                    sData.Value = sData.Value & Chr(0)

                    sData.Value = sData.Value & Chr(cboChestItem.SelectedIndex + 1)

                ElseIf optTreasure(1).Checked = True Then
                    sData.Value = sData.Value & Chr(1)

                    If IsNumeric(txtGold.Text) Then
                        sData.Value = sData.Value & Chr(Integer.Parse(txtGold.Text) \ 256)
                        sData.Value = sData.Value & Chr(Integer.Parse(txtGold.Text) Mod 256)
                    Else

                        okEnable = False
                    End If

                End If

            ElseIf optType(24).Checked = True Then

                If cboDoorKey.SelectedIndex > -1 Then
                    sData.Value = sData.Value & Chr(cboDoorKey.SelectedIndex + 4)
                Else
                    okEnable = False
                End If

            End If

            lblSpecialData.Text = ""
            For i = 1 To Len(sData)
                lblSpecialData.Text = lblSpecialData.Text & Hex(Asc(Mid(sData.Value, i, 1))) & " "
            Next

            frmStore.Visible = storeEnable
            frmMapChange.Visible = mapChangeEnable
            cmdOK.Enabled = okEnable

        End If

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        UpdateControls()

        Dim i As Integer

        SelectedOK = True

        sType = 0

        If optType(2).Checked Then
            For i = 0 To optStoreType.Count - 1
                If optStoreType(CShort(i)).Checked = True Then
                    sType = i + 2
                End If
            Next
        ElseIf optType(1).Checked Then
            sType = 1
        Else
            For i = 20 To optType.UBound
                If optType(CShort(i)).Checked = True Then
                    sType = i
                End If
            Next


        End If

        'sData = lblSpecialData

        Me.Hide()


    End Sub

    'UPGRADE_WARNING: Event optSpeakWith.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optSpeakWith_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSpeakWith.CheckedChanged
        'If eventSender.Checked Then
        '    Dim Index As Integer = optSpeakWith.GetIndex(eventSender)
        '    UpdateControls()
        'End If
    End Sub

    'UPGRADE_WARNING: Event optTreasure.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optTreasure_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optTreasure.CheckedChanged
        'If eventSender.Checked Then
        '    Dim Index As Integer = optTreasure.GetIndex(eventSender)
        '    cboChestItem.Enabled = False
        '    txtGold.Enabled = False

        '    If Index = 0 Then cboChestItem.Enabled = True
        '    If Index = 1 Then txtGold.Enabled = True

        '    UpdateControls()
        'End If
    End Sub

    'UPGRADE_WARNING: Event optType.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        'If eventSender.Checked Then
        '    Dim Index As Integer = optType.GetIndex(eventSender)
        '    frmSpeakWith.Visible = False
        '    frmStore.Visible = False
        '    frmMapChange.Visible = False
        '    frmChest.Visible = False
        '    frmDoor.Visible = False


        '    Select Case Index
        '        Case 2
        '            frmStore.Visible = True
        '        Case 1, 20
        '            frmMapChange.Visible = True
        '        Case 21
        '            frmSpeakWith.Visible = True
        '        Case 23, 25
        '            frmChest.Visible = True
        '        Case 24
        '            frmDoor.Visible = True
        '    End Select

        '    UpdateControls()
        'End If
    End Sub

    Public Sub SetDefaults()
        Dim i As Integer

        optType(1).Checked = False
        optType(2).Checked = False
        optType(20).Checked = False
        optType(21).Checked = False

        For i = 0 To optStoreType.Count - 1
            optStoreType(CShort(i)).Checked = False
        Next

        txtLocX.Text = CStr(x1)
        txtLocY.Text = CStr(y1)

        txtName.Text = ""
        txtNewX.Text = ""
        txtNewY.Text = ""

        txtGold.Text = ""
        cboDoorKey.SelectedIndex = -1
        cboChestItem.SelectedIndex = -1

        optTreasure(0).Checked = False
        optTreasure(0).Checked = False

        txtSpcWidth.Text = "1"
        txtSpcHeight.Text = "1"

        txtPrice.Text = "100"

        UpdateControls()

    End Sub
	
	'UPGRADE_WARNING: Event txtGold.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtGold_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtGold.TextChanged
		UpdateControls()
	End Sub
	
	'UPGRADE_WARNING: Event txtName.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtName_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtName.TextChanged
		UpdateControls()
	End Sub
	
	Private Sub txtName_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtName.Leave
		UpdateControls()
	End Sub
	
	Private Sub txtNewX_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtNewX.Leave
		UpdateControls()
	End Sub
	Private Sub txtNewY_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtNewY.Leave
		UpdateControls()
	End Sub
	
	Private Sub txtNewMap_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtNewMap.Leave
		UpdateControls()
	End Sub
	
	'UPGRADE_WARNING: Event txtPrice.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtPrice_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtPrice.TextChanged
		UpdateControls()
	End Sub
End Class