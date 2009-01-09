Option Explicit On

Imports Agate = ERY.AgateLib.Geometry

Friend Class frmRoof
	Inherits System.Windows.Forms.Form
	
    Public RoofIndex As Integer
    Dim RoofTile As Integer
    Dim roofLeftX, roofTopY As Integer
    Dim x2, y2 As Integer
    Dim setting As Boolean

    Dim dispWindow As ERY.AgateLib.DisplayWindow
    Dim tilesWindow As ERY.AgateLib.DisplayWindow

    Private Sub Check1_Click()

    End Sub

    'UPGRADE_WARNING: Event chkDrawGround.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkDrawGround_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkDrawGround.CheckStateChanged
        RoofBlit()

    End Sub

    Private Sub cmdDone_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDone.Click
        Me.Hide()

    End Sub

    Private Sub frmRoof_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        dispWindow = New ERY.AgateLib.DisplayWindow(Picture1)
        tilesWindow = New ERY.AgateLib.DisplayWindow(Picture2)

        SetControls()

    End Sub

    Private Sub frmRoof_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        UpdateControls()
        RoofBlit()
    End Sub

    Private Sub hsbRoofIndex_Change(ByVal sender As Object, ByVal e As EventArgs) Handles hsbRoofIndex.ValueChanged

        RoofIndex = hsbRoofIndex.Value

        SetControls()

    End Sub

    Private Sub Picture1_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles Picture1.Paint
        RoofBlit()
    End Sub

    Sub RoofBlit()

        RoofDraw()

        Display.RenderTarget = tilesWindow
        Display.BeginFrame()

        TileSurface.Draw()

        Display.EndFrame()


    End Sub

    Private Sub cmdCopy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCopy.Click
        Dim i, j As Integer
        Dim t As Point

        If MsgBox("Overwrite current roof?", MsgBoxStyle.YesNo, "Overwrite?") = MsgBoxResult.No Then Exit Sub

        For j = 0 To Roofs(RoofIndex).height - 1
            For i = 0 To Roofs(RoofIndex).width - 1
                t.X = Roofs(RoofIndex).anchorTarget.X - Roofs(RoofIndex).anchor.X + i
                t.Y = Roofs(RoofIndex).anchorTarget.Y - Roofs(RoofIndex).anchor.Y + j

                Roofs(RoofIndex).Matrix(i, j) = Map(t.X, t.Y)
            Next
        Next

        RoofBlit()

    End Sub

    Private Sub RoofDraw()
        Dim ddrval As Integer
        Dim r1 As Agate.Rectangle
        Dim r2 As Agate.Rectangle
        Dim tilex, i, j, a, tiley As Integer
        Dim roofCenterX, xx, yy, roofCenterY As Integer
        Dim t As Agate.Point
        Dim tile As Agate.Point

        Display.RenderTarget = dispWindow

        Display.BeginFrame()
        Display.Clear(Agate.Color.FromArgb(&H55, &H55, &H55))


        picTilesX = CInt(Picture1.ClientRectangle.Width / TileSize / 2 + 1)
        picTilesY = CInt(Picture1.ClientRectangle.Height / TileSize / 2)

        xx = 0
        yy = 0


        sbRight.Maximum = mapWidth
        sbRight.Minimum = 0
        sbRight.LargeChange = picTilesX * 2 - 2
        sbDown.LargeChange = picTilesY * 2 - 2
        sbDown.Maximum = mapHeight
        sbDown.Minimum = 0

        roofCenterX = sbRight.Value
        roofCenterY = sbDown.Value

        roofLeftX = roofCenterX - picTilesX
        roofTopY = roofCenterY - picTilesY


        For j = roofCenterY - picTilesY To roofCenterY + picTilesY + 1
            For i = roofCenterX - picTilesX To roofCenterX + picTilesX + 1
                If i >= 0 And i < Roofs(RoofIndex).width And j >= 0 And j < Roofs(RoofIndex).height Then

                    a = Roofs(RoofIndex).Matrix(i, j)


                    If chkDrawGround.CheckState <> 0 And a = 127 Then
                        't = Roofs(k).matrix(i - Roofs(k).anchorTarget.X + Roofs(k).anchor.Y, _
                        ''           j - Roofs(k).anchorTarget.Y + Roofs(k).anchor.Y)
                        t.X = Roofs(RoofIndex).anchorTarget.x - Roofs(RoofIndex).anchor.x + i
                        t.Y = Roofs(RoofIndex).anchorTarget.y - Roofs(RoofIndex).anchor.y + j

                        If (t.Y < 0 Or t.X < 0) Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object a. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            a = 127
                        Else
                            'UPGRADE_WARNING: Couldn't resolve default property of object a. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            a = Map(t.X, t.Y)
                        End If

                    End If

                    tilex = (a Mod 16) * 16
                    tiley = CInt(a / 16) * 16

                    r1.X = tilex
                    r1.Width = 16
                    r1.Y = tiley
                    r1.Height = 16

                    r2.X = xx * TileSize
                    r2.Y = yy * TileSize
                    r2.Width = 16
                    r2.Height = 16

                    TileSurface.Draw(r1, r2)


                Else
                    'Picture1.Line (xx * tilesize, yy * tilesize)-((xx + 1) * tilesize, (yy + 1) * tilesize), vbBlack, BF

                End If

                xx = xx + 1
            Next
            yy = yy + 1
            xx = 0

        Next

        Display.EndFrame()

        r1.X = (Roofs(RoofIndex).anchor.x - roofLeftX) * TileSize
        r1.Width = TileSize
        r1.Y = (Roofs(RoofIndex).anchor.y - roofTopY) * TileSize
        r1.Width = TileSize

        Display.DrawRect(r1, Agate.Color.Cyan)

        Display.DrawRect(New Agate.Rectangle((x2 - roofLeftX) * TileSize, (y2 - roofTopY) * TileSize, (x2 - roofLeftX + 1) * TileSize, (y2 - roofTopY) * TileSize), Agate.Color.White)

        lblx.Text = "x2:   " & x2 & "  0x" & Hex(x2)
        lblY.Text = "y2:   " & y2 & "  0x" & Hex(y2)

        If x2 < 0 Or x2 >= Roofs(RoofIndex).width Or y2 < 0 Or y2 >= Roofs(RoofIndex).height Then
            lblTile.Text = "Tile: Out of range"
        Else
            lblTile.Text = "Tile: " & Roofs(RoofIndex).Matrix(x2, y2) & "   0x" & Hex(Roofs(RoofIndex).Matrix(x2, y2))
        End If

        lblCurrentTile.Text = "Current Tile: " & RoofTile & "   0x" & Hex(RoofTile)


    End Sub



    Private Sub Picture2_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles Picture2.Paint
        RoofBlit()
    End Sub
    Private Sub Picture1_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles Picture1.MouseDown
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Integer = eventArgs.X
        Dim y As Integer = eventArgs.Y
        Dim yy, xx As Integer


        xx = (x \ 16) + roofLeftX
        yy = (y \ 16) + roofTopY

        Debug.Print("X: " & x & "  XX: " & xx & "        Y: " & y & "  YY: " & yy)


        If Button = 1 Then

            SetPos(xx, yy)

        ElseIf Button = 2 And xx >= 0 And yy >= 0 Then

            Roofs(RoofIndex).Matrix(xx, yy) = RoofTile

        End If


        RoofBlit()
    End Sub

    Private Sub Picture1_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles Picture1.MouseMove
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Integer = eventArgs.X
        Dim y As Integer = eventArgs.Y
        Dim xx, yy As Integer

        xx = x
        yy = y

        xx = Int(xx \ 16) + roofLeftX
        yy = Int(yy \ 16) + roofTopY

        If Button = 2 And xx >= 0 And yy >= 0 Then

            Roofs(RoofIndex).Matrix(xx, yy) = RoofTile

            RoofBlit()
        End If

    End Sub

    Private Sub Picture2_MouseDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles Picture2.MouseDown
        Dim Button As Integer = eventArgs.Button \ &H100000
        Dim Shift As Integer = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Integer = eventArgs.X
        Dim y As Integer = eventArgs.Y
        Dim yy, xx, tile As Integer

        xx = x
        yy = y

        xx = CInt(xx / 16)
        yy = CInt(yy / 16)

        tile = (yy * 16 + xx)

        If Button = 2 Then
            Roofs(RoofIndex).Matrix(x1, y1) = tile
        End If

        RoofTile = tile

        RoofBlit()
    End Sub

    Private Sub SetPos(ByVal xx As Integer, ByVal yy As Integer)
        x2 = xx
        y2 = yy

    End Sub

    Public Sub UpdateControls()
        lblEditing.Text = "Editing roof #: " & hsbRoofIndex.Value

        If setting = False Then
            If IsNumeric(txtRoofX.Text) Then Roofs(RoofIndex).width = Integer.Parse(txtRoofX.Text)
            If IsNumeric(txtRoofY.Text) Then Roofs(RoofIndex).height = Integer.Parse(txtRoofY.Text)
            If IsNumeric(txtAnchorX.Text) Then Roofs(RoofIndex).anchor.X = Integer.Parse(txtAnchorX.Text)
            If IsNumeric(txtAnchorY.Text) Then Roofs(RoofIndex).anchor.Y = Integer.Parse(txtAnchorY.Text)
            If IsNumeric(txtTargetX.Text) Then Roofs(RoofIndex).anchorTarget.X = Integer.Parse(txtTargetX.Text)
            If IsNumeric(txtTargetY.Text) Then Roofs(RoofIndex).anchorTarget.Y = Integer.Parse(txtTargetY.Text)

            RoofBlit()
        End If

    End Sub


    Private Sub sbDown_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        RoofBlit()
    End Sub

    Private Sub sbRight_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        RoofBlit()

    End Sub

    'UPGRADE_WARNING: Event txtAnchorX.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtAnchorX_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtAnchorX.TextChanged
        UpdateControls()

    End Sub

    'UPGRADE_WARNING: Event txtAnchorY.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtAnchorY_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtAnchorY.TextChanged
        UpdateControls()

    End Sub

    'UPGRADE_WARNING: Event txtRoofX.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtRoofX_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtRoofX.TextChanged
        UpdateControls()

    End Sub

    Public Sub SetControls()
        setting = True

        txtRoofX.Text = CStr(Roofs(RoofIndex).width)
        txtRoofY.Text = CStr(Roofs(RoofIndex).height)
        txtAnchorX.Text = CStr(Roofs(RoofIndex).anchor.x)
        txtAnchorY.Text = CStr(Roofs(RoofIndex).anchor.y)
        txtTargetX.Text = CStr(Roofs(RoofIndex).anchorTarget.x)
        txtTargetY.Text = CStr(Roofs(RoofIndex).anchorTarget.y)

        hsbRoofIndex.Value = RoofIndex

        setting = False

        RoofBlit()
    End Sub

    Private Sub txtRoofY_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtRoofY.TextChanged
        UpdateControls()
    End Sub

    Private Sub txtTargetX_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtTargetX.TextChanged
        UpdateControls()

    End Sub

    Private Sub txtTargetY_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtTargetY.TextChanged
        UpdateControls()

    End Sub
    Private Sub hsbRoofIndex_Scroll(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ScrollEventArgs) Handles hsbRoofIndex.Scroll
        Select Case eventArgs.Type
            Case System.Windows.Forms.ScrollEventType.EndScroll
                hsbRoofIndex_Change(eventSender, eventArgs)
        End Select
    End Sub
End Class