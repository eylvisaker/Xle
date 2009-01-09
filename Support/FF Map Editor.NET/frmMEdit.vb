Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports Agate = ERY.AgateLib.Geometry

' TODO: fix all mouse down events to use integers instead of singles!
' 
Friend Class frmMEdit
	Inherits System.Windows.Forms.Form
    Private oldw, oldh As Integer
    Private offset As Integer
	Private validMap As Boolean

    Private mainWindow As ERY.AgateLib.DisplayWindow
    Private tilesWindow As ERY.AgateLib.DisplayWindow

	'UPGRADE_WARNING: Event chkDrawGuards.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub chkDrawGuards_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkDrawGuards.CheckStateChanged
		blit()
	End Sub
	
	'UPGRADE_WARNING: Event chkDrawRoof.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub chkDrawRoof_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkDrawRoof.CheckStateChanged
		blit()
	End Sub
	
	Private Sub cmdDeleteSpecial_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteSpecial.Click
        Dim i As Integer

        For i = 1 To maxSpecial
            If specialx(i) = x1 And specialy(i) = y1 Then
                specialx(i) = 0
                specialy(i) = 0
                specialwidth(i) = 0
                specialheight(i) = 0
                special(i) = 0

            End If
        Next

        FillSpecial()
        blit()

        SetPos(x1, y1)


    End Sub

    Private Sub cmdFill_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFill.Click
        Dim xdif, ydif As Integer
        Dim tile, r As Integer
        Dim i, j As Integer

        'UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        xdif = x2 - x1
        'UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ydif = x2 - x1

        'UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (xdif = 0) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            xdif = xdif + 1
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If ydif = 0 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ydif = ydif + 1
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        tile = currentTile
        If chkRandom.CheckState = 1 Then
            If currentTile = 7 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r = CInt(Rnd(1) * 4)

                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r < 2 Then tile = currentTile + r
                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r > 1 Then tile = currentTile + r + 14

            ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r = CInt(Rnd(1) * 2)

                'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tile = currentTile + r

            End If
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object xdif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        For i = x1 To x2 Step System.Math.Sign(xdif)
            'UPGRADE_WARNING: Couldn't resolve default property of object ydif. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For j = y1 To y2 Step System.Math.Sign(ydif)
                If chkRandom.CheckState = 1 Then
                    If currentTile = 7 Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        r = CInt(Rnd(1) * 4)

                        'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If r < 2 Then tile = currentTile + r
                        'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If r > 1 Then tile = currentTile + r + 14

                    ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        r = CInt(Rnd(1) * 2)

                        'UPGRADE_WARNING: Couldn't resolve default property of object r. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        tile = currentTile + r

                    End If
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object tile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PaintLoc(i, j, tile)

            Next
        Next

        blit()

    End Sub

    Private Sub cmdGuard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGuard.Click
        Dim i As Integer
        Dim found As Boolean

        For i = 0 To 100
            If guard(i).X = x1 And guard(i).Y = y1 Then
                guard(i).X = 0
                guard(i).Y = 0
                found = True
            End If
        Next

        If found <> True Then
            For i = 0 To 100
                If guard(i).X = 0 And guard(i).Y = 0 Then
                    guard(i).X = x1
                    guard(i).Y = y1

                    blit()
                    Exit Sub

                End If
            Next
        End If

        blit()
    End Sub

    Private Sub cmdModifySpecial_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdModifySpecial.Click
        Dim j, i, s As Integer


        For i = 1 To maxSpecial
            If specialx(i) = x1 And specialy(i) = y1 Then
                frmSpecial.Changing = True

                frmSpecial.sData = specialdata(i)
                frmSpecial.txtLocX.Text = CStr(specialx(i))
                frmSpecial.txtLocY.Text = CStr(specialy(i))
                frmSpecial.sType = special(i)
                frmSpecial.setProperties = True
                frmSpecial.txtSpcWidth.Text = CStr(specialwidth(i))
                frmSpecial.txtSpcHeight.Text = CStr(specialheight(i))

                frmSpecial.Changing = False

                s = i
            End If
        Next

        frmSpecial.ShowDialog()

        If SelectedOK Then
            special(s) = frmSpecial.sType
            specialx(s) = Integer.Parse(frmSpecial.txtLocX.Text)
            specialy(s) = Integer.Parse(frmSpecial.txtLocY.Text)
            specialdata(s) = frmSpecial.sData
            specialwidth(s) = Integer.Parse(frmSpecial.txtSpcWidth.Text)
            specialheight(s) = Integer.Parse(frmSpecial.txtSpcHeight.Text)

        End If

        SetPos(x1, y1)

        FillSpecial()

    End Sub

    Private Sub cmdObject_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdObject.Click

        Dim Index As Integer
        Dim i, j As Integer

        Index = lstPreDef.SelectedIndex

        For j = 0 To PreDefObjects(Index).height - 1
            For i = 0 To PreDefObjects(Index).width - 1
                'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PreDefObjects(Index).Matrix(i, j) > -1 Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PaintLoc(x1 + i, y1 + j, PreDefObjects(Index).Matrix(i, j))
                End If
            Next
        Next

        blit()
    End Sub

    Private Sub cmdPlaceSpecial_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPlaceSpecial.Click

        Dim i, sx, sy, j As Integer

        frmSpecial.SetDefaults()

        frmSpecial.txtLocX.Text = CStr(x1)
        frmSpecial.txtLocY.Text = CStr(y1)
        frmSpecial.txtSpcWidth.Text = CStr(x2 - x1 + 1)
        frmSpecial.txtSpcHeight.Text = CStr(y2 - y1 + 1)

        frmSpecial.ShowDialog()

        If SelectedOK Then
            For i = 1 To maxSpecial
                If special(i) = 0 Then

                    special(i) = frmSpecial.sType

                    specialx(i) = Integer.Parse(frmSpecial.txtLocX.Text)
                    specialy(i) = Integer.Parse(frmSpecial.txtLocY.Text)

                    specialwidth(i) = Integer.Parse(frmSpecial.txtSpcWidth.Text)
                    specialheight(i) = Integer.Parse(frmSpecial.txtSpcHeight.Text)

                    specialdata(i) = New VB6.FixedLengthString(100, frmSpecial.sData.Value)

                    Exit For
                End If
            Next

            SetPos(x1, y1)

            FillSpecial()
        End If

        blit()

    End Sub

    Private Sub cmdRoof_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRoof.Click
        Dim i, j As Integer
        Dim found As Boolean

        'UPGRADE_WARNING: Couldn't resolve default property of object frmRoof.RoofIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        frmRoof.RoofIndex = 1

        For i = 1 To maxRoofs
            'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If x1 >= Roofs(i).anchorTarget.X - Roofs(i).anchor.X And y1 >= Roofs(i).anchorTarget.Y - Roofs(i).anchor.Y And x1 < Roofs(i).anchorTarget.X - Roofs(i).anchor.X + Roofs(i).width And y1 < Roofs(i).anchorTarget.Y - Roofs(i).anchor.Y + Roofs(i).height Then
                found = True
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object frmRoof.RoofIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                frmRoof.RoofIndex = i
                'frmRoof.txtAnchorX = Roofs(i).anchor.X
                'frmRoof.txtAnchorY = Roofs(i).anchor.Y
                'frmRoof.txtTargetX = Roofs(i).anchorTarget.X
                'frmRoof.txtTargetY = Roofs(i).anchorTarget.Y
                'frmRoof.txtRoofX = Roofs(i).width
                'frmRoof.txtRoofY = Roofs(i).height
                frmRoof.SetControls()

                Exit For
            End If
        Next

        If found = False Then
            For i = 1 To maxRoofs
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Roofs(i).anchorTarget.X = 0 And Roofs(i).anchorTarget.Y = 0 Then

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object frmRoof.RoofIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    frmRoof.RoofIndex = i
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Roofs(i).width = x2 - x1 + 1
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Roofs(i).height = y2 - y1 + 1
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Roofs(i).anchorTarget.X = x1
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Roofs(i).anchorTarget.Y = y1
                    frmRoof.chkDrawGround.CheckState = System.Windows.Forms.CheckState.Checked
                    frmRoof.SetControls()

                    Exit For
                End If
            Next
        End If


        frmRoof.ShowDialog()



    End Sub

    Private Sub cmdSetCorner_Click()
        Dim optCorner As Object
        Dim txtCY As Object
        Dim txtCX As Object
        Dim i As Object
        ' TODO: I have no idea what this function is supposed to do.

        'For i = 0 To 1
        '    'UPGRADE_WARNING: Couldn't resolve default property of object optCorner(i).value. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        '    If optCorner(i).value = True Then
        '        'UPGRADE_WARNING: Couldn't resolve default property of object txtCX(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        '        txtCX(i) = x1
        '        'UPGRADE_WARNING: Couldn't resolve default property of object txtCY(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        '        txtCY(i) = y1
        '    End If
        'Next

    End Sub


    'UPGRADE_WARNING: Form event frmMEdit.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub frmMEdit_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        UpdateScreen = True

    End Sub

    'UPGRADE_WARNING: Form event frmMEdit.Deactivate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub frmMEdit_Deactivate(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Deactivate
        UpdateScreen = False
    End Sub

    Private Sub frmMEdit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        For i = 0 To 15
            Label5.Text = Label5.Text & Hex(i) & vbCrLf
        Next

        mainWindow = New ERY.AgateLib.DisplayWindow(Picture1)
        tilesWindow = New ERY.AgateLib.DisplayWindow(Picture2)

        CreateSurfaces((Picture1.Width), (Picture1.Height))

        cmdDialogOpen.InitialDirectory = LotaPath
        cmdDialogSave.InitialDirectory = LotaPath

        Do While Not validMap

            frmStartup.ShowDialog()

            If StartNewMap Then
                AssignProperties()
                NewMap(False)

            ElseIf ImportMap Then
                ImportNewMap()

            Else
                OpenMap()

            End If

        Loop

        x1 = 0
        x2 = 0
        y1 = 0
        y2 = 0

        blit()
    End Sub

    Private Sub frmMEdit_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

    End Sub

    Sub blit()
        If UpdateScreen = True Then

            draw()

        End If


    End Sub

    Private Sub draw()
        Dim tiley, a, j, i, tilex, k As Integer
        Dim centery, centerx, t As Integer
        Dim xx, yy As Integer
        Dim srcRect, destRect As Agate.Rectangle

        Display.RenderTarget = mainWindow

        Display.BeginFrame()
        Display.Clear(Agate.Color.FromArgb(&H55, &H55, &H55))


        lblDim.Text = "Map Dimensions: " & mapWidth & " x " & mapHeight
        If fileName <> "" Then
            Me.Text = "LotA Town Editor - " & fileName
        End If

        picTilesX = CInt(Picture1.ClientRectangle.Width / TileSize / 2)
        picTilesY = CInt(Picture1.ClientRectangle.Height / TileSize / 2)


        xx = 0
        yy = 0

        sbRight1.Maximum = mapWidth
        sbRight1.Minimum = 0
        sbRight1.LargeChange = picTilesX * 2 - 2
        sbDown.LargeChange = picTilesY * 2 - 2
        sbDown.Maximum = mapHeight
        sbDown.Minimum = 0


        centerx = sbRight1.Value
        centery = sbDown.Value

        leftX = centerx - picTilesX
        topy = centery - picTilesY

        sbSpecial.Minimum = 0
        sbSpecial.Maximum = NumSpecials()
        sbSpecial.LargeChange = 5
        sbSpecial.SmallChange = 1

        lblSpcCount.Text = "Specials: " & sbSpecial.Maximum & " count."

        For j = dispY - picTilesY To dispY + picTilesY + 1
            For i = dispX - picTilesX To dispX + picTilesX + 1
                If i >= 0 And i < mapWidth And j >= 0 And j < mapHeight Then

                    a = Map(i, j)

                    If chkDrawRoof.CheckState <> 0 Then
                        For k = 1 To maxRoofs
                            If (i >= Roofs(k).anchorTarget.X - Roofs(k).anchor.X And j >= Roofs(k).anchorTarget.Y - Roofs(k).anchor.Y) And (i < Roofs(k).anchorTarget.X - Roofs(k).anchor.X + Roofs(k).width And j < Roofs(k).anchorTarget.Y - Roofs(k).anchor.Y + Roofs(k).height) Then

                                t = Roofs(k).Matrix(i - Roofs(k).anchorTarget.X + Roofs(k).anchor.X, j - Roofs(k).anchorTarget.Y + Roofs(k).anchor.Y)

                                If t <> 127 Then a = t

                            End If
                        Next

                    End If

                    tilex = (a Mod 16) * 16
                    tiley = CInt(a / 16) * 16

                    srcRect.X = tilex
                    srcRect.Y = tiley
                    srcRect.Width = TileSize
                    srcRect.Height = TileSize

                    destRect.X = xx * TileSize
                    destRect.Y = yy * TileSize
                    destRect.Width = TileSize
                    destRect.Height = TileSize

                    TileSurface.Draw(srcRect, destRect)


                Else
                    'Picture1.Line (xx * tilesize, yy * tilesize)-((xx + 1) * tilesize, (yy + 1) * tilesize), vbBlack, BF

                End If

                xx = xx + 1
            Next

            yy = yy + 1
            xx = 0

        Next

        ' TODO: Fix this
        'If chkDrawGuards.CheckState And (MapType = MainModule.EnumMapType.maptown Or MapType = MainModule.EnumMapType.mapCastle) Then
        If chkDrawGuards.Checked Then
            For i = 0 To 100
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If guard(i).X >= leftX And guard(i).X < leftX + 2 * picTilesX + 1 And guard(i).Y >= topy And guard(i).Y < topy + 2 * picTilesY + 1 And guard(i).X > 0 And guard(i).Y > 0 Then

                    srcRect.Y = 5 * 32
                    srcRect.X = 0 * 32
                    srcRect.Width = 32
                    srcRect.Height = 32

                    destRect.X = (guard(i).X - leftX) * TileSize
                    destRect.Y = (guard(i).Y - topy) * TileSize
                    destRect.Width = 32
                    destRect.Height = 32


                    CharSurface.Draw(srcRect, destRect)

                End If
            Next
        End If


        For i = 1 To maxSpecial
            If specialx(i) >= leftX Or specialx(i) + specialwidth(i) < leftX + 2 * picTilesX + 1 Or specialy(i) >= topy Or specialy(i) + specialheight(i) < topy + 2 * picTilesY + 1 Or special(i) > 0 Then

                srcRect.X = (specialx(i) - leftX) * TileSize
                srcRect.Width = specialwidth(i) * TileSize
                srcRect.Y = (specialy(i) - topy) * TileSize
                srcRect.Height = specialheight(i) * TileSize

                'DDSBack.DrawBox(r1.Left, r1.Top, r1.Right, r1.Bottom)
                Display.DrawRect(srcRect, Agate.Color.Cyan)
            End If
        Next

        'DDSBack.SetForeColor(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow))

        If MapType <> MainModule.EnumMapType.mapOutside Then
            ''''''''''''''''''''''''''
            ''  Draw Roofs
            For i = 1 To maxRoofs
                If (leftX < Roofs(i).anchorTarget.X - Roofs(i).anchor.X Or topy < Roofs(i).anchorTarget.Y - Roofs(i).anchor.Y) Or (leftX + picTilesX * 2 > Roofs(i).anchorTarget.X - Roofs(i).anchor.X + Roofs(i).width Or topy + picTilesY * 2 > Roofs(i).anchorTarget.Y - Roofs(i).anchor.Y + Roofs(i).height) Then


                    srcRect.X = (Roofs(i).anchorTarget.X - leftX) * TileSize
                    srcRect.Width = TileSize
                    srcRect.Y = (Roofs(i).anchorTarget.Y - topy) * TileSize
                    srcRect.Height = TileSize

                    Display.DrawRect(srcRect, Agate.Color.Yellow)

                    srcRect.X = (Roofs(i).anchorTarget.X - Roofs(i).anchor.X - leftX) * TileSize
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    srcRect.Width = Roofs(i).width * TileSize
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    srcRect.Y = (Roofs(i).anchorTarget.Y - Roofs(i).anchor.Y - topy) * TileSize
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    srcRect.Height = Roofs(i).height * TileSize

                    Display.DrawRect(srcRect, Agate.Color.Yellow)
                End If
            Next
        End If

        Display.DrawRect(Agate.Rectangle.FromLTRB((x1 - leftX) * TileSize, (y1 - topy) * TileSize, (x2 - leftX + 1) * TileSize, (y2 - topy + 1) * TileSize), Agate.Color.White)

        Display.EndFrame()

        lblX.Text = "x1:   " & x1 & "  0x" & Hex(x1) & "    x2: " & x2
        lblY.Text = "y1:   " & y1 & "  0x" & Hex(y1) & "    y2: " & y2

        If x1 < 0 Or x1 > mapWidth Or y1 < 0 Or y1 > mapHeight Then
            lblTile.Text = "Tile: Out of range"
        Else
            lblTile.Text = "Tile: " & Map(x1, y1) & "   0x" & Hex(Map(x1, y1))
        End If

        If ImportMap Then
            If ImportLocation(x1, y1) >= 0 And ImportLocation(x1, y1) <= UBound(ImportedData) Then
                lblImport.Text = "Import: " & ImportedData(ImportLocation(x1, y1))
            Else
                lblImport.Text = "Import: Out of Range"
            End If
        Else
            lblImport.Text = ""
        End If

        lblCurrentTile.Text = "Current Tile: " & currentTile & "   0x" & Hex(currentTile)

        ' Label7(0).Caption = ""
        ' Label7(1).Caption = ""
        ' For i = leftX - 1 To leftX + picTilesX * 2 + 2
        '     Label7(i Mod 2) = Label7(i Mod 2) & i
        '     Label7((i + 1) Mod 2) = Label7((i + 1) Mod 2) & "  "
        ' Next
        '
        ' Label8.Caption = ""
        ' For i = topY - 1 To topY + picTilesY * 2 + 2
        '     Label8 = Label8 & i & vbCrLf
        ' Next

        Select Case MapType
            Case MainModule.EnumMapType.mapOutside
                cmdGuard.Visible = False
            Case MainModule.EnumMapType.mapDungeon
                cmdGuard.Visible = False
            Case MainModule.EnumMapType.mapMuseum
                cmdGuard.Visible = False
            Case MainModule.EnumMapType.maptown
                cmdGuard.Visible = True
            Case MainModule.EnumMapType.mapCastle
                cmdGuard.Visible = True
        End Select


        Display.RenderTarget = tilesWindow
        Display.BeginFrame()

        TileSurface.Draw()

        Display.EndFrame()

    End Sub

    Private Sub FillSpecial()
        Dim i, j As Integer

        Text7.Text = ""
        For i = 1 To maxSpecial
            If special(i) > 0 Then
                If specialx(i) = 0 And specialy(i) = 0 Then
                    specialheight(i) = 0
                    specialwidth(i) = 0
                End If

                Text7.Text = Text7.Text & "Store #" & i & ": Type " & special(i)
                Text7.Text = Text7.Text & vbCrLf & "    At Point: (" & specialx(i) & ", " & specialy(i) & ")"
                Text7.Text = Text7.Text & vbCrLf & "    Data: " & specialdata(i).Value
                Text7.Text = Text7.Text & vbCrLf & "          "

                For j = 1 To Len(RTrim(specialdata(i).Value))
                    Text7.Text = Text7.Text & Hex(Asc(Mid(specialdata(i).Value, j, 1))) & "  "
                Next


                Text7.Text = Text7.Text & vbCrLf & vbCrLf
            End If

        Next
    End Sub

    'UPGRADE_WARNING: Event frmMEdit.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub frmMEdit_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        frmRight.Left = Me.ClientRectangle.Width - frmRight.Width
        frmBottom.Top = Me.ClientRectangle.Height - frmBottom.Height - StatusBar1.Height

        Picture1.Width = frmRight.Left - sbDown.Width - Picture1.Left
        Picture1.Height = frmBottom.Top - sbRight1.Height - Picture1.Top

        sbDown.Left = Picture1.Left + Picture1.Width
        sbRight1.Top = Picture1.Top + Picture1.Height

        sbDown.Height = Picture1.Height
        sbRight1.Width = Picture1.Width


    End Sub

    Private Sub frmMEdit_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Display.Dispose()

        End
    End Sub

    Private Sub lblX_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lblX.Click
        Dim xx As Integer
        xx = Integer.Parse(InputBox("Enter X:"))

        SetPos(xx, y1)

        blit()
    End Sub

    Private Sub lblY_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lblY.Click
        Dim yy As Integer
        yy = Integer.Parse(InputBox("Enter Y:"))

        SetPos(x1, yy)

        blit()
    End Sub


    Public Sub mnuFinalize_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFinalize.Click

        If MsgBox("Are you sure you wish to finalize this imported map?" & vbCrLf & vbCrLf & "You will have to edit it as a standard map from now on!", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Finalize?") = MsgBoxResult.Yes Then
            SetPropertiesForm()

            'TODO: Fix this
            'frmProperties.optType(MapType).Checked = True
            frmProperties.txtName.Text = ""


            frmProperties.ShowDialog()

            If SelectedOK Then
                AssignProperties()

                mnuSaveAs_Click(mnuSaveAs, New System.EventArgs())
                ImportMap = False

                SetMenus()

                UpdateScreen = True
                blit()
            End If

        End If

    End Sub

    Public Sub mnuImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuImport.Click

        cmdDialogOpen.Title = "Import Map"
        cmdDialogSave.Title = "Import Map"

        cmdDialogOpen.FileName = "E:\Legacy\."
        cmdDialogSave.FileName = "E:\Legacy\."
        cmdDialogOpen.Filter = "Export Files|*.export|All Files (*.*)|*.*"
        cmdDialogSave.Filter = "Export Files|*.export|All Files (*.*)|*.*"
        cmdDialogOpen.FilterIndex = 1
        cmdDialogSave.FilterIndex = 1

        cmdDialogOpen.InitialDirectory = My.Application.Info.DirectoryPath & "\..\Included Maps"
        cmdDialogSave.InitialDirectory = My.Application.Info.DirectoryPath & "\..\Included Maps"

        cmdDialogOpen.DefaultExt = "map"
        cmdDialogSave.DefaultExt = "map"

        cmdDialogOpen.ShowDialog()
        cmdDialogSave.FileName = cmdDialogOpen.FileName

        fileName = cmdDialogOpen.FileName

        StartNewMap = False
        ImportMap = True

        ImportNewMap()

    End Sub

    Public Sub mnuImportRefresh_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuImportRefresh.Click
        RecalibrateImport()

    End Sub

    Public Sub mnuLoadMapping_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuLoadMapping.Click


        cmdDialogOpen.Title = "Load Mapping"
        cmdDialogSave.Title = "Load Mapping"


        cmdDialogOpen.Filter = "Mapping File (*.mpn)|*.mpn"
        cmdDialogSave.Filter = "Mapping File (*.mpn)|*.mpn"
        cmdDialogOpen.FilterIndex = 1
        cmdDialogSave.FilterIndex = 1
        cmdDialogOpen.InitialDirectory = My.Application.Info.DirectoryPath & "\."
        cmdDialogSave.InitialDirectory = My.Application.Info.DirectoryPath & "\."
        cmdDialogOpen.FileName = ""
        cmdDialogSave.FileName = ""

        cmdDialogOpen.DefaultExt = "mpn"
        cmdDialogSave.DefaultExt = "mpn"

        cmdDialogOpen.ShowDialog()
        cmdDialogSave.FileName = cmdDialogOpen.FileName

        LoadMapping(cmdDialogOpen.FileName)

    End Sub

    Public Sub mnuNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuNew.Click

        NewMap(True)

    End Sub

    Private Sub ImportNewMap()
        ' TODO: Fix or delete this
        'Dim fso As New Scripting.FileSystemObject
        'Dim theFile As Scripting.File
        'Dim fileNum As Integer
        'Dim i As Integer

        'fileNum = FreeFile()

        'theFile = fso.GetFile(fileName)
        'ReDim ImportedData(theFile.Size)

        'FileOpen(fileNum, fileName, OpenMode.Binary)

        'For i = 0 To theFile.Size - 1
        '    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        '    FileGet(fileNum, ImportedData(i))
        'Next

        'ResetImportDefinitions()

        'frmImport.SetDefaults()
        'frmImport.ShowDialog()

        'If Not frmImport.ClickedOK Then Exit Sub

        'validMap = True

        'For i = 1 To maxRoofs
        '    Roofs(i).anchor.X = 0
        '    Roofs(i).anchor.Y = 0
        '    Roofs(i).anchorTarget.X = 0
        '    Roofs(i).anchorTarget.Y = 0
        '    Roofs(i).height = 0
        '    Roofs(i).width = 0

        'Next

        'For i = 0 To 100
        '    guard(i).X = 0
        '    guard(i).Y = 0
        'Next

        'For i = 1 To maxSpecial
        '    special(i) = 0
        '    specialx(i) = 0
        '    specialy(i) = 0
        '    specialdata(i) = New VB6.FixedLengthString(100)
        '    specialwidth(i) = 0
        '    specialheight(i) = 0

        'Next

        'SetMenus()
        'LoadTiles(TileSet)
        'blit()
    End Sub

    Private Sub NewMap(ByRef show_Renamed As Boolean)
        Dim j, i, k As Integer

        If (show_Renamed = True) Then
            frmProperties.SetDefaults()
            frmProperties.Text = "New Map"

            frmProperties.ShowDialog()
        Else
            frmProperties.SetDefaults()

        End If

        If SelectedOK Or Not show_Renamed Then

            For i = 0 To mapWidth
                For j = 0 To mapHeight
                    PaintLoc(i, j, 0)
                Next
            Next

            sbRight1.Maximum = mapWidth
            sbRight1.Minimum = 0
            sbRight1.Value = 0

            sbDown.Maximum = mapHeight
            sbDown.Minimum = 0
            sbDown.Value = 0

            For i = 0 To 100
                guard(i).X = 0
                guard(i).Y = 0
            Next

            For i = 0 To maxSpecial
                special(i) = 0
                specialx(i) = 0
                specialy(i) = 0
                specialdata(i) = New VB6.FixedLengthString(100)
                specialwidth(i) = 0
            Next

            NumRoofs = 0
            For i = 1 To maxRoofs
                Roofs(i).anchor.X = 0
                Roofs(i).anchor.Y = 0
                Roofs(i).anchorTarget.X = 0
                Roofs(i).anchorTarget.Y = 0
                Roofs(i).height = 0
                Roofs(i).width = 0

                For j = 0 To 100
                    For k = 0 To 100
                        Roofs(i).Matrix(j, k) = 127
                    Next
                Next
            Next

            LoadTiles(TileSet)

            validMap = True
        End If

        SetMenus()

    End Sub

    Public Sub mnuParameters_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuParameters.Click
        ' TODO: Figure out what this is
        'frmImport.ShowDialog()

        'LoadTiles(TileSet)
        'blit()
    End Sub

    Public Sub mnuProperties_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuProperties.Click
        SetPropertiesForm()

        frmProperties.ShowDialog()

        If SelectedOK Then
            AssignProperties()
        End If

    End Sub

    Public Sub mnuQuit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuQuit.Click
        Display.Dispose()
        End
    End Sub


    Public Sub mnuRefreshTiles_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRefreshTiles.Click
        LoadTiles(TileSet)
    End Sub

    Public Sub mnuSaveAs_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSaveAs.Click

        cmdDialogOpen.Title = "Save Map"
        cmdDialogSave.Title = "Save Map"

        cmdDialogOpen.Filter = "Binary Map Files (*.bmf)|*.bmf|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
        cmdDialogSave.Filter = "Binary Map Files (*.bmf)|*.bmf|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
        cmdDialogOpen.FilterIndex = 2
        cmdDialogSave.FilterIndex = 0
        cmdDialogOpen.InitialDirectory = LotaPath & "\included maps\."
        cmdDialogSave.InitialDirectory = LotaPath & "\included maps\."
        cmdDialogOpen.FileName = ""
        cmdDialogSave.FileName = ""

        cmdDialogSave.ShowDialog()
        cmdDialogOpen.FileName = cmdDialogSave.FileName

        fileName = cmdDialogOpen.FileName

        mnuSave_Click(mnuSave, New System.EventArgs())

ErrorHandler:
    End Sub



    Public Sub mnuSaveMapping_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSaveMapping.Click

        cmdDialogOpen.Title = "Save Mapping"
        cmdDialogSave.Title = "Save Mapping"

        cmdDialogOpen.Filter = "Mapping File (*.mpn)|*.mpn"
        cmdDialogSave.Filter = "Mapping File (*.mpn)|*.mpn"
        cmdDialogOpen.FilterIndex = 1
        cmdDialogSave.FilterIndex = 1
        cmdDialogOpen.InitialDirectory = My.Application.Info.DirectoryPath & "\."
        cmdDialogSave.InitialDirectory = My.Application.Info.DirectoryPath & "\."
        cmdDialogOpen.FileName = ""
        cmdDialogSave.FileName = ""

        cmdDialogSave.ShowDialog()
        cmdDialogOpen.FileName = cmdDialogSave.FileName

        SaveMapping(cmdDialogOpen.FileName)

ErrorHandler:

    End Sub

    Private Sub Picture1_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles Picture1.MouseDown
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim yy, xx As Integer

        xx = CInt(x)
        yy = CInt(y)

        xx = CInt(xx \ 16) + leftX
        yy = CInt(yy \ 16) + topy

        If UpdateScreen = False Then Exit Sub

        If Button = 1 Then

            SetPos(xx, yy)

        ElseIf Button = 2 And xx >= 0 And yy >= 0 Then
            ' TODO: fix this hack
            Picture1_MouseMove(Picture1, New System.Windows.Forms.MouseEventArgs( _
                               DirectCast(Button * &H100000, System.Windows.Forms.MouseButtons), 0, xx, yy, 0))

        End If


        blit()
    End Sub

    Private Sub Picture1_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles Picture1.MouseMove
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim r, xx, yy, tile As Integer

        xx = CInt(x)
        yy = CInt(y)

        xx = CInt(xx / 16) + leftX
        yy = CInt(yy / 16) + topy

        If UpdateScreen = False Then Exit Sub

        If Button = 1 Then
            SetRightPos(xx, yy)
            blit()

        ElseIf Button = 2 And xx >= 0 And yy >= 0 Then

            If ImportMap Then
                PaintLoc(xx, yy, currentTile)
            Else
                If chkRestrict.CheckState <> 0 Then
                    If xx < x1 Or xx > x2 Or yy < y1 Or yy > y2 Then Exit Sub
                End If

                If (chkRandom.CheckState = 0) Then
                    PaintLoc(xx, yy, currentTile)
                Else
                    If currentTile = 7 Then
                        r = CInt(Rnd(1) * 4)

                        If r < 2 Then tile = currentTile + r
                        If r > 1 Then tile = currentTile + r + 14

                        PaintLoc(xx, yy, tile)
                    ElseIf currentTile = 2 Or currentTile = 129 Or currentTile = 182 Then
                        r = CInt(Rnd(1) * 2)

                        tile = currentTile + r

                        PaintLoc(xx, yy, tile)

                    Else
                        PaintLoc(xx, yy, currentTile)
                    End If
                End If
            End If

            blit()
        End If

    End Sub

    Private Sub Picture1_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles Picture1.Paint
        blit()
    End Sub

    Private Sub Picture2_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles Picture2.MouseDown
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Integer = eventArgs.X
        Dim y As Integer = eventArgs.Y
        Dim yy, xx, tile As Integer

        xx = CInt(x)
        yy = CInt(y)

        xx = CInt(xx / 16)
        yy = CInt(yy / 16)

        tile = (yy * 16 + xx)

        If UpdateScreen = False Then Exit Sub

        If Button = 2 Then
            PaintLoc(x1, y1, tile)
        End If

        currentTile = tile

        blit()
    End Sub

    Private Sub Picture2_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles Picture2.Paint
        blit()
    End Sub

    Private Sub Text4_Change()

    End Sub
    Public Sub mnuSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSave.Click


        If fileName = "" Then
            mnuSaveAs_Click(mnuSaveAs, New System.EventArgs())

            If fileName = "" Then Exit Sub

        End If

        SaveMap()

    End Sub

    Private Sub SaveMap()
        If Path.GetExtension(fileName) <> ".bmf" Then
            old_SaveMap()
            Return
        End If


        Dim saveMap As XleMap
        Dim i, j As Integer

        Select Case MapType
            Case 1
                saveMap = New XleMapTypes.Museum()

            Case 2
                saveMap = New XleMapTypes.Outside()

            Case 3
                saveMap = New XleMapTypes.Town

            Case 4
                saveMap = New XleMapTypes.Dungeon

            Case 5
                saveMap = New XleMapTypes.Castle

        End Select

        saveMap.InitializeMap(mapWidth, mapHeight)
        saveMap.MapName = mapName.Value.Trim()

        saveMap.TileSet = TileSet

        For j = 0 To mapHeight - 1
            For i = 0 To mapWidth - 1
                saveMap(i, j) = mMap(i, j)
            Next
        Next

        If GetType(XleMapTypes.Town).IsAssignableFrom(saveMap.GetType()) Then
            Dim t As XleMapTypes.Town = DirectCast(saveMap, XleMapTypes.Town)

            t.OutsideTile = defaultTile

            t.BuyRaftMap = BuyRaftMap
            t.BuyRaftPt = New Agate.Point(BuyRaftX, BuyRaftY)

            For i = 0 To mail.GetUpperBound(0)
                If mail(i) <> 0 Then
                    t.Mail.Add(mail(i))
                End If
            Next

        End If

        If GetType(IHasGuards).IsAssignableFrom(saveMap.GetType()) Then
            Dim h As IHasGuards = DirectCast(saveMap, IHasGuards)

            For i = 1 To guard.GetUpperBound(0)
                Dim g As New Guard

                g.Location = Interop.Convert(guard(i))
                g.HP = guardHP
                g.Facing = Direction.South
                g.Attack = guardAttack
                g.Defense = guardDefense

                If g.Location.IsEmpty = False Then
                    h.Guards.Add(g)
                End If

            Next
        End If

        If GetType(IHasRoofs).IsAssignableFrom(saveMap.GetType()) Then
            For i = 1 To maxRoofs
                Dim r As New Roof

                If Roofs(i).width = 0 And Roofs(i).height = 0 Then Continue For

                r.SetSize(Roofs(i).width, Roofs(i).height)
                r.Location = New Agate.Point(Roofs(i).anchorTarget.X - Roofs(i).anchor.X, Roofs(i).anchorTarget.Y - Roofs(i).anchor.Y)

                For k As Integer = 0 To Roofs(i).width - 1
                    For j = 0 To Roofs(i).height - 1
                        r(k, j) = Roofs(i).Matrix(k, j)
                    Next
                Next

                CType(saveMap, IHasRoofs).Roofs.Add(r)
            Next
        End If

        For i = 1 To NumSpecials()
            Dim evt As XleEvent = Nothing
            Dim c() As Char = specialdata(i).Value.ToCharArray()

            Select Case special(i)
                Case 1
                    Dim e As New XleEventTypes.ChangeMapEvent

                    e.MapID = Asc(c(0)) * 256 + Asc(c(1))
                    e.Location = New Agate.Point(Asc(c(2)) * 256 + Asc(c(3)), Asc(c(4)) * 256 + Asc(c(5)))

                    If (Asc(c(6)) < 11 Or Asc(c(6)) = 32) Then
                        e.Ask = True
                    Else
                        e.Ask = False
                    End If

                    evt = e

                Case 2
                    Dim e As New XleEventTypes.StoreBank
                    evt = e

                Case 3
                    Dim e As New XleEventTypes.StoreWeapon
                    evt = e

                Case 4
                    Dim e As New XleEventTypes.StoreArmor
                    evt = e

                Case 5
                    Dim e As New XleEventTypes.StoreWeaponTraining
                    evt = e

                Case 6
                    Dim e As New XleEventTypes.StoreArmorTraining
                    evt = e

                Case 7
                    Dim e As New XleEventTypes.StoreBlackjack
                    evt = e

                Case 8
                    Dim e As New XleEventTypes.StoreLending
                    evt = e

                Case 9
                    Dim e As New XleEventTypes.StoreRaft
                    evt = e

                Case 10
                    Dim e As New XleEventTypes.StoreHealer
                    evt = e

                Case 11
                    Dim e As New XleEventTypes.StoreJail
                    evt = e

                Case 12
                    Dim e As New XleEventTypes.StoreFortune
                    evt = e

                Case 13
                    Dim e As New XleEventTypes.StoreFlipFlop
                    evt = e

                Case 14
                    Dim e As New XleEventTypes.StoreBuyback
                    evt = e

                Case 15
                    Dim e As New XleEventTypes.StoreFood
                    evt = e

                Case 16
                    Dim e As New XleEventTypes.StoreVault
                    evt = e

                Case 17
                    Dim e As New XleEventTypes.StoreMagic
                    evt = e

                Case 23, 25
                    Dim e As XleEventTypes.ItemAvailableEvent

                    If special(i) = 23 Then
                        e = New XleEventTypes.TreasureChestEvent
                    Else
                        e = New XleEventTypes.TakeEvent
                    End If

                    evt = e

                    If Asc(specialdata(i).Value.Chars(0)) = 0 Then
                        e.ContainsItem = True
                        e.Contents = Asc(specialdata(i).Value.Chars(1))

                    Else
                        e.ContainsItem = False
                        e.Contents = Asc(specialdata(i).Value.Chars(1)) * 256 + Asc(specialdata(i).Value.Chars(2))
                    End If

                Case 24
                    Dim e As New XleEventTypes.Door
                    evt = e

                    e.RequiredItem = Asc(specialdata(i).Value.Chars(0))

            End Select

            If TypeOf (evt) Is XleEventTypes.Store Then
                Dim st As XleEventTypes.Store = DirectCast(evt, XleEventTypes.Store)

                st.ShopName = specialdata(i).Value

                ' TODO: Fix this:
                If st.ShopName.Contains("\\") Then
                    'st.ShopName = st.ShopName.Split("\\", )(0)
                ElseIf st.ShopName.Contains("\0") Then
                    'st.ShopName = st.ShopName.Split(Chr(0))(0)
                Else
                End If

            End If
            'enum StoreType
            '{
            '	storeBank = 2,					// 2
            '	storeWeapon,					// 3
            '	storeArmor,						// 4
            '	storeWeaponTraining,			// 5
            '	storeArmorTraining,				// 6
            '	storeBlackjack,					// 7
            '	storeLending,					// 8
            '	storeRaft,						// 9
            '	storeHealer,					// 10
            '	storeJail,						// 11
            '	storeFortune,					// 12
            '	storeFlipFlop,					// 13
            '	storeBuyback,					// 14
            '	storeFood,						// 15
            '	storeVault,						// 16
            '	storeMagic						// 17
            '};

            If Not evt Is Nothing Then

                evt.X = specialx(i)
                evt.Y = specialy(i)
                evt.Width = specialwidth(i)
                evt.Height = specialheight(i)

                saveMap.Events.Add(evt)
            End If

        Next

        '' Now serialize it.
        Dim formatter As Runtime.Serialization.IFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Using ff As Stream = File.OpenWrite(fileName)
            formatter.Serialize(ff, saveMap)

            ff.Flush()
        End Using


    End Sub
    Private Sub old_SaveMap()
        Dim path As String
        Dim offset As Integer
        Dim file As Integer
        Dim mn As New VB6.FixedLengthString(16)
        Dim j, i, k As Integer
        Dim test As New VB6.FixedLengthString(1)

        mn.Value = mapName.Value



        Dim a As New VB6.FixedLengthString(1)
        Dim b As Byte
        If fileName <> "" Then
            path = fileName

            file = FreeFile()


            Kill(path)
            FileOpen(file, path, OpenMode.Binary)

            offset = 1


            a.Value = Chr(Int(mapWidth \ 256)) '0
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            a.Value = Chr(Int(mapWidth Mod 256)) '1
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            a.Value = Chr(Int(mapHeight \ 256)) '2
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            a.Value = Chr(Int(mapHeight Mod 256)) '3
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            a.Value = Chr(Int(fileOffset \ 256)) '4
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            a.Value = Chr(Int(fileOffset Mod 256)) '5
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            a.Value = Chr(MapType) '6
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1

            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, mn.Value, offset) : offset = offset + Len(mn.Value) '7

            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, "@", offset) : offset = offset + 1 '23

            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, defaultTile, offset) : offset = offset + 1 '24

            a.Value = Chr(Int(guardHP \ 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '25

            a.Value = Chr(Int(guardHP Mod 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '26

            a.Value = Chr(Int(guardAttack \ 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '27

            a.Value = Chr(Int(guardAttack Mod 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '28

            a.Value = Chr(Int(guardDefense \ 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '29

            a.Value = Chr(Int(guardDefense Mod 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '30

            a.Value = Chr(Int(guardColor \ 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '31

            a.Value = Chr(Int(guardColor Mod 256))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, offset) : offset = offset + 1 '32

            If TileSet = "Tiles.bmp" Then
                a.Value = Chr(0)
            ElseIf TileSet = "TownTiles.bmp" Then
                a.Value = Chr(1)
            ElseIf TileSet = "CastleTiles.bmp" Then
                a.Value = Chr(2)
            ElseIf TileSet = "LOB Tiles.bmp" Then
                a.Value = Chr(3)
            ElseIf TileSet = "LOB TownTiles.bmp" Then
                a.Value = Chr(4)
            End If

            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 34) : offset = offset + 1 '33

            a.Value = Chr(BuyRaftMap) '36  Buy Raft Map
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 37)

            a.Value = Chr(BuyRaftX \ 256)
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 38) '37  Buy Raft X
            a.Value = Chr(BuyRaftX Mod 256)
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 39) '38  Buy Raft X

            a.Value = Chr(BuyRaftY \ 256)
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 40) '39  Buy Raft y
            a.Value = Chr(BuyRaftY Mod 256)
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 41) '40  Buy Raft Y

            a.Value = Chr(120)
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 42) '41 special count

            a.Value = Chr(mail(0))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 43) ' 42 mail 0

            a.Value = Chr(mail(1))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 44) ' 43 mail 1

            a.Value = Chr(mail(2))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 45) ' 44 mail 2

            a.Value = Chr(mail(3))
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(file, a.Value, 46) ' 45 mail 3

            offset = fileOffset + 1

            For j = 0 To mapHeight - 1
                For i = 0 To mapWidth - 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    b = CByte(Map(i, j))

                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, b, offset)
                    offset = offset + 1

                Next
            Next

            'offset = (mapHeight + 1) * mapWidth + 1

            For i = 1 To maxSpecial
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, special(i), offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialx(i) \ 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialx(i) Mod 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialy(i) \ 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialy(i) Mod 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialwidth(i) \ 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialwidth(i) Mod 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialheight(i) \ 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                a.Value = Chr(Int(specialheight(i) Mod 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, offset) : offset = offset + 1

                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, specialdata(i), offset)
                'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                offset = offset + Len(specialdata(i))
            Next

            If MapType = MainModule.EnumMapType.maptown Or MapType = MainModule.EnumMapType.mapCastle Then

                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, "5555557", offset) : offset = offset + Len("5555557")

                For i = 0 To 100
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(guard(i).X \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(guard(i).X Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(guard(i).Y \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(guard(i).Y Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1
                Next

                a.Value = Chr(Int((offset - 1) \ 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, 35) ' 34 (roof offset)

                a.Value = Chr(Int((offset - 1) Mod 256))
                'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                FilePut(file, a.Value, 36) ' 35 (roof offset)

                For i = 1 To maxRoofs
                    ' anchor
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchor.X \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchor.X Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchor.Y \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchor.Y Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    ' anchortarget
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchorTarget.X \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchorTarget.X Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchorTarget.Y \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).anchorTarget.Y Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    '  Dimensions
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).width \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).width Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).height \ 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    a.Value = Chr(Int(Roofs(i).height Mod 256))
                    'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FilePut(file, a.Value, offset) : offset = offset + 1

                    '  Data
                    'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    For j = 0 To Roofs(i).height - 1
                        'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        For k = 0 To Roofs(i).width - 1
                            'UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Couldn't resolve default property of object k. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            a.Value = Chr(Roofs(i).Matrix(k, j))
                            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            FilePut(file, a.Value, offset) : offset = offset + 1
                        Next
                    Next
                Next

            End If

            FileClose(file)

            'UPGRADE_WARNING: Lower bound of collection StatusBar1.Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            StatusBar1.Items.Item(1).Text = "Saved successfully: " & TimeOfDay

        End If

        blit()

    End Sub
    Public Sub mnuOpen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuOpen.Click

        Try

            cmdDialogOpen.Title = "Open Map"
            cmdDialogSave.Title = "Open Map"

            cmdDialogOpen.Filter = "All Map Files|*.bmf;*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
            cmdDialogSave.Filter = "All Map Files|*.bmf;*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*"
            cmdDialogOpen.FilterIndex = 1
            cmdDialogSave.FilterIndex = 1

            cmdDialogOpen.InitialDirectory = LotaPath & "\Included Maps"
            cmdDialogSave.InitialDirectory = LotaPath & "\Included Maps"
            cmdDialogOpen.FileName = ""
            cmdDialogSave.FileName = ""

            cmdDialogOpen.DefaultExt = "map"
            cmdDialogSave.DefaultExt = "map"

            cmdDialogOpen.ShowDialog()
            cmdDialogSave.FileName = cmdDialogOpen.FileName

            fileName = cmdDialogOpen.FileName


            OpenMap()

        Catch ex As Exception

        End Try

    End Sub
    Private Sub OpenMap()
        old_OpenMap()
    End Sub

    Private Sub old_OpenMap()
        Dim path As String
        Dim file As Integer
        Dim newOffset As Integer
        Dim tempName As String
        Dim j, i, k As Integer
        Dim b As String
        Dim ro As Integer

        path = fileName

        ' TODO: discard this method.

        Dim a As New VB6.FixedLengthString(1)
        If path <> "" Then
            file = FreeFile()
            FileOpen(file, path, OpenMode.Binary)


            For i = 1 To maxRoofs
                Roofs(i).anchor.X = 0
                Roofs(i).anchor.Y = 0
                Roofs(i).anchorTarget.X = 0
                Roofs(i).anchorTarget.Y = 0
                Roofs(i).height = 0
                Roofs(i).width = 0

            Next

            For i = 0 To 100
                guard(i).X = 0
                guard(i).Y = 0
            Next

            For i = 1 To maxSpecial
                special(i) = 0
                specialx(i) = 0
                specialy(i) = 0
                specialdata(i) = New VB6.FixedLengthString(100)
                specialwidth(i) = 0
                specialheight(i) = 0

            Next



            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 1)
            mapWidth = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 2)
            mapWidth = mapWidth + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 3)
            mapHeight = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 4)
            mapHeight = mapHeight + Asc(a.Value)

            If mapWidth > 3000 Or mapHeight > 3000 Then
                MsgBox("Bad File: " & path & vbCrLf & vbCrLf & "The data is invalid.  Please try a different file or replace it.")
                validMap = False

                Exit Sub
            End If

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 5)
            newOffset = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 6)
            newOffset = newOffset + Asc(a.Value)
            fileOffset = newOffset

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 7)
            MapType = Asc(a.Value)

            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = 8

            tempName = ""
            Do
                'FileGet(file, a.Value, offset)
                tempName = tempName & a.Value
                offset = offset + 1
            Loop Until a.Value = "@" Or offset > 25

            mapName.Value = RTrim(VB.Left(tempName, Len(tempName) - 1))

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 25)
            defaultTile = Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 26)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '25
            guardHP = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 27)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '26
            guardHP = guardHP + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 28)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '27
            guardAttack = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 29)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '28
            guardAttack = guardAttack + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 30)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '29
            guardDefense = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 31)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '30
            guardDefense = guardDefense + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 32)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '31
            guardColor = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 33)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '32
            guardColor = guardColor + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 34)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 '33
            If a.Value = Chr(0) Then
                TileSet = "Tiles.png"
            ElseIf a.Value = Chr(1) Then
                TileSet = "TownTiles.png"
            ElseIf a.Value = Chr(2) Then
                TileSet = "CastleTiles.png"
            ElseIf a.Value = Chr(3) Then
                TileSet = "LOB Tiles.png"
            ElseIf a.Value = Chr(4) Then
                TileSet = "LOB TownTiles.png"
            End If

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 37)
            BuyRaftMap = Asc(a.Value) '36  Buy Raft Map

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 38) '37  Buy Raft X
            BuyRaftX = Asc(a.Value) * 256
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 39) '38  Buy Raft X
            BuyRaftX = BuyRaftX + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 40) '39  Buy Raft y
            BuyRaftY = Asc(a.Value) * 256
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 41) '40  Buy Raft Y
            BuyRaftY = BuyRaftY + Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 42) '41  special count
            If Asc(a.Value) = 0 Then a.Value = Chr(20)
            specialCount = Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 35)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 ' 34 (Roof offset)
            'UPGRADE_WARNING: Couldn't resolve default property of object ro. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ro = Asc(a.Value) * 256

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 36)
            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = offset + 1 ' 35 (roof offset)
            'UPGRADE_WARNING: Couldn't resolve default property of object ro. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ro = ro + Asc(a.Value) + 1

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 43) ' 42 mail 0
            mail(0) = Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 44) ' 43 mail 1
            mail(1) = Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 45) ' 44 mail 2
            mail(2) = Asc(a.Value)

            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(file, a.Value, 46) ' 45 mail 3
            mail(3) = Asc(a.Value)

            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            offset = newOffset + 1

            For j = 0 To mapHeight - 1
                For i = 0 To mapWidth - 1

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)

                    mMap(i, j) = Asc(a.Value)

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1

                Next
            Next

            For i = 1 To specialCount
                FileGet(file, a.Value, offset)
                offset = offset + 1
                special(i) = Asc(a.Value)

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialx(i) = Asc(a.Value) * 256

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialx(i) = specialx(i) + Asc(a.Value)

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialy(i) = Asc(a.Value)

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialy(i) = specialy(i) + Asc(a.Value)

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialwidth(i) = Asc(a.Value) * 256

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialwidth(i) = specialwidth(i) + Asc(a.Value)

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialheight(i) = Asc(a.Value)

                FileGet(file, a.Value, offset)
                offset = offset + 1
                specialheight(i) = specialheight(i) + Asc(a.Value)

                FileGet(file, specialdata(i).Value, offset)
                offset = offset + 100
            Next

            For i = 1 To maxRoofs
                Roofs(i).anchor.X = 0
                Roofs(i).anchor.Y = 0
                Roofs(i).anchorTarget.X = 0
                Roofs(i).anchorTarget.Y = 0
                Roofs(i).height = 0
                Roofs(i).width = 0

            Next

            If MapType = MainModule.EnumMapType.maptown Or MapType = MainModule.EnumMapType.mapCastle Then
                Do Until VB.Right(b, 7) = "5555557" Or EOF(file)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1

                    b = b & a.Value
                Loop


                For i = 0 To 100
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    guard(i).X = Asc(a.Value) * 256
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    guard(i).X = guard(i).X + Asc(a.Value)

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    guard(i).Y = Asc(a.Value) * 256
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    guard(i).Y = guard(i).Y + Asc(a.Value)
                Next



                For i = 1 To maxRoofs
                    ' anchor
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchor.X = Asc(a.Value) * 256

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchor.X = Roofs(i).anchor.X + Asc(a.Value)

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchor.Y = Asc(a.Value)

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchor.Y = Roofs(i).anchor.Y + Asc(a.Value)

                    ' anchortarget
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchorTarget.X = Asc(a.Value) * 256

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchorTarget.X = Roofs(i).anchorTarget.X + Asc(a.Value)

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchorTarget.Y = Asc(a.Value) * 256

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).anchorTarget.Y = Roofs(i).anchorTarget.Y + Asc(a.Value)

                    '  Dimensions
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).width = Asc(a.Value) * 256

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).width = Roofs(i).width + Asc(a.Value)

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).height = Asc(a.Value) * 256

                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    FileGet(file, a.Value, offset)
                    'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    offset = offset + 1
                    Roofs(i).height = Roofs(i).height + Asc(a.Value)

                    '  Data
                    For j = 0 To Roofs(i).height - 1
                        For k = 0 To Roofs(i).width - 1
                            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            FileGet(file, a.Value, offset)
                            'UPGRADE_WARNING: Couldn't resolve default property of object offset. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            offset = offset + 1
                            Roofs(i).Matrix(k, j) = Asc(a.Value)
                        Next
                    Next

                Next

            End If

            FileClose(file)

            fileName = path

            x1 = 0
            x2 = 0
            y1 = 0
            y2 = 0


            sbRight1.Maximum = mapWidth
            sbDown.Maximum = mapHeight

            LoadTiles(TileSet)
            validMap = True
            ImportMap = False

        End If

        SetMenus()
        FillSpecial()
        blit()
    End Sub

    Public Sub SetPropertiesForm()
        Dim i As Integer

        With frmProperties
            .txtName.Text = mapName.Value
            .txtDefaultTile.Text = CStr(defaultTile)
            .mType = MapType
            .txtAttack.Text = CStr(guardAttack)
            .txtHP.Text = CStr(guardHP)
            .txtDefense.Text = CStr(guardDefense)
            .txtColor.Text = CStr(guardColor)
            .txtDefaultTile.Text = CStr(defaultTile)
            .Text = LTrim(mapName.Value) & " Properties"
            .txtWidth.Text = CStr(mapWidth)
            .txtHeight.Text = CStr(mapHeight)
            .theTiles = TileSet
            .txtBuyRaftMap.Text = CStr(BuyRaftMap)
            .txtBuyRaftX.Text = CStr(BuyRaftX)
            .txtBuyRaftY.Text = CStr(BuyRaftY)

            .setProperties = True

            ' TODO: remove control array.
            For i = 0 To 3
                .txtMail(CShort(i)).Text = CStr(mail(i))
            Next

        End With

        oldw = CInt(VB6.PixelsToTwipsX(Width))
        oldh = CInt(VB6.PixelsToTwipsY(Height))
    End Sub

    Public Sub AssignProperties()
        Dim i As Integer

        mapName.Value = frmProperties.txtName.Text

        If frmProperties.txtDefaultTile.Text <> "" Then defaultTile = Integer.Parse(frmProperties.txtDefaultTile.Text)
        If frmProperties.txtAttack.Text <> "" Then guardAttack = Integer.Parse(frmProperties.txtAttack.Text)
        If frmProperties.txtHP.Text <> "" Then guardHP = Integer.Parse(frmProperties.txtHP.Text)
        If frmProperties.txtDefense.Text <> "" Then guardDefense = Integer.Parse(frmProperties.txtDefense.Text)
        If frmProperties.txtColor.Text <> "" Then guardColor = Integer.Parse(frmProperties.txtColor.Text)
        If frmProperties.theTiles <> "" Then TileSet = frmProperties.theTiles

        Dim newSize As New Size(Int32.Parse(frmProperties.txtWidth.Text), Int32.Parse(frmProperties.txtHeight.Text))

        If newSize.Width <> TheMap.Width Or newSize.Height <> TheMap.Height Then
            'TheMap.Resize(newSize.Width, newSize.Height)
        End If


        'BuyRaftMap = frmProperties.mbuyraftmap
        'BuyRaftX = Integer.Parse(frmProperties.mBuyRaftX)
        'BuyRaftY = Integer.Parse(frmProperties.mBuyRaftY)

        For i = 0 To 3
            mail(i) = Integer.Parse(frmProperties.txtMail(CShort(i)).Text)
        Next


        If oldw > 0 And oldh > 0 Then
            If oldw <> mapWidth And oldh <> mapHeight Then

            End If
        End If


        SetPos(x1, y1)
        LoadTiles(TileSet)

    End Sub

    Public Sub SetPos(ByVal xx As Integer, ByVal yy As Integer)
        Dim stopChecking As Boolean
        Dim i As Integer
        x1 = xx
        y1 = yy

        x2 = xx
        y2 = yy

        For i = 1 To maxSpecial
            If specialx(i) = x1 And specialy(i) = y1 And stopChecking = False Then
                cmdPlaceSpecial.Enabled = False
                cmdModifySpecial.Enabled = True
                cmdDeleteSpecial.Enabled = True
                stopChecking = True
            ElseIf stopChecking = False Then
                cmdPlaceSpecial.Enabled = True
                cmdModifySpecial.Enabled = False
                cmdDeleteSpecial.Enabled = False
            End If
        Next

    End Sub

    Public Sub SetRightPos(ByVal xx As Integer, ByVal yy As Integer)
        x2 = xx
        y2 = yy
    End Sub

    Private Sub sbDown_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        dispY = sbDown.Value
        blit()
    End Sub

    Private Sub sbRight_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        dispX = sbRight1.Value
        blit()
    End Sub

    Private Sub PaintLoc(ByVal x As Integer, ByVal y As Integer, ByVal value As Integer)
        If ImportMap Then
            PaintArea(x, y, value)
        Else
            mMap(x, y) = value
        End If

    End Sub

    Private Sub SetMenus()

        mnuSave.Enabled = Not ImportMap
        mnuSaveAs.Enabled = Not ImportMap
        mnuProperties.Enabled = Not ImportMap
        mnuPlaceSpecial.Enabled = Not ImportMap
        mnuModifySpecial.Enabled = Not ImportMap
        mnuDeleteSpecial.Enabled = Not ImportMap

        mnuFinalize.Enabled = ImportMap
        mnuTitleImport.Enabled = ImportMap

    End Sub

    Private Sub sbSpecial_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        lblFindSpecial.Text = "Find Special: " & sbSpecial.Value

        If sbSpecial.Value = 0 Then Exit Sub

        x1 = specialx(sbSpecial.Value)
        x2 = x1

        y1 = specialy(sbSpecial.Value)
        y2 = y1

        sbRight1.Value = CInt(Math.Max(x1 - picTilesX / 5, 0))
        sbDown.Value = CInt(Math.Max(y1 - picTilesY / 5, 0))

        blit()

    End Sub

    Private Sub SortSpecials()
        Dim i, j As Integer
        Dim max, min As Integer

        max = 0
        min = 120

        For i = 120 To 1 Step -1
            If special(i) = 0 Then
                If i < max Then
                    For j = i To max
                        special(i) = special(i + 1)
                        specialx(i) = specialx(i + 1)
                        specialy(i) = specialy(i + 1)
                        specialdata(i) = specialdata(i + 1)
                        specialwidth(i) = specialwidth(i + 1)
                        specialheight(i) = specialheight(i + 1)
                    Next

                End If

            ElseIf special(i) > 0 Then
                If i > max Then
                    max = i
                End If

            End If

        Next

    End Sub

    Private Function NumSpecials() As Integer
        Dim i As Integer

        SortSpecials()

        For i = 1 To 120
            If special(i) = 0 Then
                Return i - 1
            End If
        Next

    End Function
End Class