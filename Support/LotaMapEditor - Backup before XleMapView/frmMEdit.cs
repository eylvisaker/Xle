
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ERY.AgateLib;
using ERY.AgateLib.WinForms;
using ERY.Xle;
using Agate = ERY.AgateLib.Geometry;


// TODO: fix all mouse down events to use integers instead of singles!
//
namespace XleMapEditor
{
    partial class frmMEdit : System.Windows.Forms.Form
    {

        #region Default Instance

        private static frmMEdit defaultInstance;

        /// <summary>
        /// Added by the VB.Net to C# Converter to support default instance behavour in C#
        /// </summary>
        [Obsolete]
        public static frmMEdit Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new frmMEdit();
                    defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
                }

                return defaultInstance;
            }
        }

        static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
        {
            defaultInstance = null;
        }

        #endregion

        int dispX, dispY;
        bool validMap;
        MapData copiedData;

        private ERY.AgateLib.DisplayWindow mainWindow;
        private ERY.AgateLib.DisplayWindow tilesWindow;

        public XleMap TheMap { get; set; }

        Point p1;
        Point p2;
        Rectangle SelRect
        {
            get
            {
                Rectangle retval = new Rectangle();
                Size sz = new Size(1, 1);

                retval = Rectangle.Union(new Rectangle(p1, sz), new Rectangle(p2, sz));

                if (retval.Width == 0) retval.Width++;
                if (retval.Height == 0) retval.Height++;

                return retval;
            }
        }
        private void chkDrawGuards_CheckStateChanged(object eventSender, EventArgs eventArgs)
        {
            Redraw();
        }
        private void chkDrawRoof_CheckStateChanged(object eventSender, EventArgs eventArgs)
        {
            Redraw();
        }

        private void cmdDeleteSpecial_Click(object eventSender, EventArgs eventArgs)
        {
            if (TheMap.Events.Count == 0)
                return;

            if (MessageBox.Show("Delete event #" + EventIndex.ToString() + Environment.NewLine +
                TheMap.Events[EventIndex].GetType().ToString()) == DialogResult.Cancel)
                return;

            TheMap.Events.RemoveAt(EventIndex);
        }
        private void cmdFill_Click(object eventSender, EventArgs eventArgs)
        {
            int tile;
            int r;
            int i;
            int j;

            tile = MainModule.currentTile;
            if (chkRandom.Checked)
            {
                if (MainModule.currentTile == 7)
                {
                    r = MainModule.Random.Next(4);

                    if (r < 2)
                        tile = MainModule.currentTile + r;
                    if (r > 1)
                    {
                        tile = MainModule.currentTile + r + 14;
                    }

                }
                else if (MainModule.currentTile == 2 || MainModule.currentTile == 129 || MainModule.currentTile == 182)
                {
                    r = MainModule.Random.Next(2);

                    tile = MainModule.currentTile + r;

                }
            }

            for (i = SelRect.Left; i < SelRect.Right; i++)
            {
                for (j = SelRect.Top; j < SelRect.Bottom; j++)
                {
                    if (System.Convert.ToInt32(chkRandom.CheckState) == 1)
                    {
                        if (MainModule.currentTile == 7)
                        {
                            r = MainModule.Random.Next(4);

                            if (r < 2)
                            {
                                tile = MainModule.currentTile + r;
                            }
                            if (r > 1)
                            {
                                tile = MainModule.currentTile + r + 14;
                            }

                        }
                        else if (MainModule.currentTile == 2 || MainModule.currentTile == 129 || MainModule.currentTile == 182)
                        {
                            r = MainModule.Random.Next(2);

                            tile = MainModule.currentTile + r;

                        }
                    }

                    PaintLoc(i, j, tile);

                }
            }

            Redraw();

        }
        private void cmdGuard_Click(object eventSender, EventArgs eventArgs)
        {
            bool found = false;

            if (TheMap is IHasGuards == false)
                return;

            IHasGuards guards = (IHasGuards)TheMap;

            for (int i = 0; i < guards.Guards.Count; i++)
            {
                Guard g = guards.Guards[i];

                if (g.X == p1.X && g.Y == p1.Y)
                {
                    guards.Guards.RemoveAt(i);
                    i--;
                    found = true;
                }
            }

            if (found == false)
            {
                Guard g = new Guard();

                g.X = p1.X;
                g.Y = p1.Y;

                guards.Guards.Add(g);

            }

            Redraw();
        }

        private void cmdModifySpecial_Click(object eventSender, EventArgs eventArgs)
        {
            for (int i = 0; i < TheMap.Events.Count; i++)
            {
                XleEvent evt = TheMap.Events[i];

                if (evt.X == p1.X && evt.Y == p1.Y)
                {
                    frmSpecial frm = new frmSpecial();

                    frm.TheMap = TheMap;
                    frm.Event = evt;

                    frm.ShowDialog();

                    SetPos(p1.X, p1.Y);

                    return;
                }
            }


        }

        private void cmdObject_Click(object eventSender, EventArgs eventArgs)
        {
            int index = lstPreDef.SelectedIndex;
            if (index == -1)
                return;

            for (int j = 0; j <= MainModule.PreDefObjects[index].height - 1; j++)
            {
                for (int i = 0; i <= MainModule.PreDefObjects[index].width - 1; i++)
                {
                    if (MainModule.PreDefObjects[index].Matrix[i, j] > -1)
                    {
                        PaintLoc(p1.X + i, p1.Y + j, MainModule.PreDefObjects[index].Matrix[i, j]);
                    }
                }
            }

            Redraw();
        }

        private void cmdPlaceSpecial_Click(object eventSender, EventArgs eventArgs)
        {
            frmSpecial frm = new frmSpecial();

            frm.EventRect = SelRect;

            if (frm.ShowNewEvent(this) == DialogResult.OK)
            {
                TheMap.Events.Add(frm.Event);
            }

            Redraw();

        }

        private void cmdRoof_Click(object eventSender, EventArgs eventArgs)
        {

            if (TheMap is IHasRoofs == false)
                return;

            IHasRoofs roof = (IHasRoofs)TheMap;
            frmRoof frm = new frmRoof();
            frm.TheMap = TheMap;

            for (int i = 0; i < roof.Roofs.Count; i++)
            {
                if (roof.Roofs[i].PointInRoof(p1.X, p1.Y) == false)
                    continue;

                frm.RoofIndex = i;

                break;
            }

            frm.ShowDialog();

            Redraw();
        }

        private void cmdSetCorner_Click()
        {
            //			object optCorner;
            //			object txtCY;
            //			object txtCX;
            //			object i;
            // TODO: I have no idea what this function is supposed to do.

            //For i = 0 To 1
            //    'UPGRADE_WARNING: Couldn't resolve default property of object optCorner(i).value. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            //    If optCorner(i).value = True Then
            //        'UPGRADE_WARNING: Couldn't resolve default property of object txtCX(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            //        txtCX(i) = x1
            //        'UPGRADE_WARNING: Couldn't resolve default property of object txtCY(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            //        txtCY(i) = y1
            //    End If
            //Next

        }


        //UPGRADE_WARNING: Form event frmMEdit.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        private void frmMEdit_Activated(object eventSender, EventArgs eventArgs)
        {
            MainModule.UpdateScreen = true;

        }

        //UPGRADE_WARNING: Form event frmMEdit.Deactivate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        private void frmMEdit_Deactivate(object eventSender, EventArgs eventArgs)
        {
            MainModule.UpdateScreen = false;
        }

        private void frmMEdit_Load(object eventSender, EventArgs eventArgs)
        {
            mainWindow = new ERY.AgateLib.DisplayWindow(Picture1);
            tilesWindow = new ERY.AgateLib.DisplayWindow(Picture2);

            MainModule.CreateSurfaces((Picture1.Width), (Picture1.Height));

            cmdDialogOpen.InitialDirectory = MainModule.LotaPath;
            cmdDialogSave.InitialDirectory = MainModule.LotaPath;

            Redraw();
        }

        private void frmMEdit_Paint(object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
        {

        }

        Size tilesDrawn;
        Point topLeft;

        public void Redraw()
        {
            if (MainModule.UpdateScreen == false)
                return;
            if (TheMap == null)
                return;

            if (copiedData == null || p2 != p1 ||
                copiedData.Width + p1.X >= TheMap.Width ||
                copiedData.Height + p1.Y >= TheMap.Height)
            {
                btnPaste.Enabled = false;
                pasteToolStripMenuItem.Enabled = false;
            }
            else
            {
                btnPaste.Enabled = true;
                pasteToolStripMenuItem.Enabled = true;
            }

            int tiley;
            int a;
            int tilex;
            int t;
            int xx;
            int yy;
            Agate.Rectangle srcRect = new ERY.AgateLib.Geometry.Rectangle();
            Agate.Rectangle destRect = new ERY.AgateLib.Geometry.Rectangle();

            Display.RenderTarget = mainWindow;

            Display.BeginFrame();
            Display.Clear(Agate.Color.FromArgb(0x55, 0x55, 0x55));

            lblDim.Text = "Map Dimensions: " + TheMap.Width + " x " + TheMap.Height;
            if (MainModule.fileName != "")
            {
                this.Text = "Xle Map Editor - " + MainModule.fileName;
            }

            tilesDrawn.Width = (int)Math.Ceiling(Picture1.ClientRectangle.Width / (double)MainModule.TileSize);
            tilesDrawn.Height = (int)Math.Ceiling(Picture1.ClientRectangle.Height / (double)MainModule.TileSize);


            xx = 0;
            yy = 0;

            sbRight.Maximum = TheMap.Width;
            sbRight.Minimum = 0;
            sbRight.LargeChange = (int)Math.Max(tilesDrawn.Width - 2, 1);
            sbDown.LargeChange = (int)Math.Max(tilesDrawn.Height - 2, 1);
            sbDown.Maximum = TheMap.Height;
            sbDown.Minimum = 0;

            topLeft.X = sbRight.Value - 1;
            topLeft.Y = sbDown.Value - 1;

            nudEvent.Minimum = 0;
            nudEvent.Maximum = TheMap.Events.Count - 1;

            lblEventCount.Text = "Event Count: " + TheMap.Events.Count;

            for (int j = topLeft.Y; j <= topLeft.Y + tilesDrawn.Height + 1; j++)
            {
                for (int i = topLeft.X; i <= topLeft.X + tilesDrawn.Width + 1; i++)
                {
                    if (i >= 0 && i < TheMap.Width && j >= 0 && j < TheMap.Height)
                    {
                        a = TheMap[i, j];

                        if (chkDrawRoof.CheckState != 0 && TheMap is IHasRoofs)
                        {
                            IHasRoofs hasroofs = (IHasRoofs)TheMap;

                            foreach (Roof r in hasroofs.Roofs)
                            {
                                if (r.PointInRoof(i, j) == false)
                                    continue;

                                t = r[i - r.X, j - r.Y];

                                if (t != 127)
                                    a = t;
                            }

                        }

                        tilex = (a % 16) * 16;
                        tiley = (a / 16) * 16;

                        srcRect.X = tilex;
                        srcRect.Y = tiley;
                        srcRect.Width = MainModule.TileSize;
                        srcRect.Height = MainModule.TileSize;

                        destRect.X = xx * MainModule.TileSize;
                        destRect.Y = yy * MainModule.TileSize;
                        destRect.Width = MainModule.TileSize;
                        destRect.Height = MainModule.TileSize;

                        MainModule.TileSurface.Draw(srcRect, destRect);
                    }

                    xx++;
                }

                yy++;
                xx = 0;

            }

            if (chkDrawGuards.Checked && TheMap is IHasGuards)
            {
                IHasGuards g = TheMap as IHasGuards;

                for (int i = 0; i < g.Guards.Count; i++)
                {
                    if (g.Guards[i].X > topLeft.X && g.Guards[i].X < topLeft.X + tilesDrawn.Width &&
                        g.Guards[i].Y > topLeft.Y && g.Guards[i].Y < topLeft.Y + tilesDrawn.Height)
                    {
                        srcRect.Y = 5 * 32;
                        srcRect.X = 0 * 32;
                        srcRect.Width = 32;
                        srcRect.Height = 32;

                        destRect.X = (g.Guards[i].X - topLeft.X) * MainModule.TileSize;
                        destRect.Y = (g.Guards[i].Y - topLeft.Y) * MainModule.TileSize;
                        destRect.Width = 32;
                        destRect.Height = 32;

                        MainModule.CharSurface.Draw(srcRect, destRect);

                    }
                }
            }


            for (int i = 0; i < TheMap.Events.Count; i++)
            {
                XleEvent evt = TheMap.Events[i];

                if (evt.Rectangle.Right < topLeft.X) continue;
                if (evt.Rectangle.Left > topLeft.X + tilesDrawn.Width) continue;
                if (evt.Rectangle.Bottom < topLeft.Y) continue;
                if (evt.Rectangle.Top > topLeft.Y + tilesDrawn.Height) continue;

                srcRect.X = (evt.X - topLeft.X) * MainModule.TileSize;
                srcRect.Y = (evt.Y - topLeft.Y) * MainModule.TileSize;
                srcRect.Width = evt.Width * MainModule.TileSize;
                srcRect.Height = evt.Height * MainModule.TileSize;

                Display.DrawRect(srcRect, Agate.Color.Yellow);
            }


            if (TheMap is IHasRoofs)
            {
                ///'''''''''''''''''''''''
                //'  Draw Roofs rectangles
                IHasRoofs r = TheMap as IHasRoofs;

                for (int i = 0; i < r.Roofs.Count; i++)
                {
                    Roof current = r.Roofs[i];

                    if (topLeft.X + tilesDrawn.Width < current.X) continue;
                    if (topLeft.Y + tilesDrawn.Height < current.Y) continue;
                    if (topLeft.X > current.X + current.Width) continue;
                    if (topLeft.Y > current.Y + current.Height) continue;


                    //srcRect.X = (current.X - topLeft.X) * MainModule.TileSize;
                    //srcRect.Width = MainModule.TileSize;
                    //srcRect.Y = (MainModule.Roofs[i].anchorTarget.Y - MainModule.topy) * MainModule.TileSize;
                    //srcRect.Height = MainModule.TileSize;

                    //Display.DrawRect(srcRect, Agate.Color.Yellow);

                    srcRect.X = (current.X - topLeft.X) * MainModule.TileSize;
                    srcRect.Width = current.Width * MainModule.TileSize;
                    srcRect.Y = (current.Y - topLeft.Y) * MainModule.TileSize;
                    srcRect.Height = current.Height * MainModule.TileSize;

                    Display.DrawRect(srcRect, Agate.Color.FromArgb(255, 60, 255, 180));

                }
            }

            Display.DrawRect(new Agate.Rectangle(
                (SelRect.X - topLeft.X) * MainModule.TileSize,
                (SelRect.Y - topLeft.Y) * MainModule.TileSize,
                SelRect.Width * MainModule.TileSize,
                SelRect.Height * MainModule.TileSize), Agate.Color.White);

            Display.EndFrame();

            lblX.Text = "x1:   " + p1.X + "  0x" + string.Format("{0:X}", p1.X) + "    x2: " + p2.X;
            lblY.Text = "y1:   " + p1.Y + "  0x" + string.Format("{0:X}", p1.Y) + "    y2: " + p2.Y;

            if (p1.X < 0 || p1.X > TheMap.Width || p1.Y < 0 || p1.Y > TheMap.Height)
                lblTile.Text = "Tile: Out of range";
            else
                lblTile.Text = "Tile: " + TheMap[p1.X, p1.Y] + "   0x" + string.Format("{0:X}", TheMap[p1.X, p1.Y]);

            lblCurrentTile.Text = "Current Tile: " + MainModule.currentTile + "   0x" + string.Format("{0:X}", MainModule.currentTile);

            if (TheMap is IHasGuards)
                cmdGuard.Visible = true;
            else
                cmdGuard.Visible = false;

            Display.RenderTarget = tilesWindow;
            Display.BeginFrame();

            MainModule.TileSurface.Draw();

            srcRect.X = (MainModule.currentTile % 16) * MainModule.TileSize;
            srcRect.Y = (MainModule.currentTile / 16) * MainModule.TileSize;
            srcRect.Width = MainModule.TileSize;
            srcRect.Height = MainModule.TileSize;

            Display.DrawRect(srcRect, Agate.Color.Yellow);

            srcRect.X--; srcRect.Y--; srcRect.Width += 2; srcRect.Height += 2;
            Display.DrawRect(srcRect, Agate.Color.Yellow);
            Display.EndFrame();

        }

        private void FillSpecial()
        {

            //int i;
            //int j;
            //
            //for (i = 1; i <= MainModule.maxSpecial; i++)
            //{
            //    if (MainModule.special[i] > 0)
            //    {
            //        if (MainModule.specialx[i] == 0 && MainModule.specialy[i] == 0)
            //        {
            //            MainModule.specialheight[i] = 0;
            //            MainModule.specialwidth[i] = 0;
            //        }

            //        txtEvent.Text = txtEvent.Text + "Store #" + i + ": Type " + MainModule.special[i];
            //        txtEvent.Text = txtEvent.Text + System.Environment.NewLine + "    At Point: (" + MainModule.specialx[i] + ", " + MainModule.specialy[i] + ")";
            //        txtEvent.Text = txtEvent.Text + System.Environment.NewLine + "    Data: " + MainModule.specialdata[i].Value;
            //        txtEvent.Text = txtEvent.Text + System.Environment.NewLine + "          ";

            //        for (j = 1; j <= Strings.RTrim(MainModule.specialdata[i].Value).Length; j++)
            //        {
            //            txtEvent.Text = txtEvent.Text + Conversion.Hex(Strings.Asc(Strings.Mid(MainModule.specialdata[i].Value, j, 1))) + "  ";
            //        }


            //        txtEvent.Text = txtEvent.Text + System.Environment.NewLine + System.Environment.NewLine;
            //    }

            //}
        }

        private void frmMEdit_FormClosed(object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
        {
            Display.Dispose();
        }

        [Obsolete]
        public void mnuFinalize_Click(object eventSender, EventArgs eventArgs)
        {

            if (MessageBox.Show(this, 
                "Are you sure you wish to finalize this imported map" + System.Environment.NewLine + 
                System.Environment.NewLine + "You will have to edit it as a standard map from now on!", 
                "Map Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmProperties prop = new frmProperties();

                //TODO: Fix this
                //frmProperties.optType(MapType).Checked = True
                //frmProperties.Default.txtName.Text = "";

                if (prop.ShowDialog() == DialogResult.OK)
                {
                    mnuSaveAs_Click(mnuSaveAs, new EventArgs());

                    SetMenus();

                    MainModule.UpdateScreen = true;
                    Redraw();
                }

            }

        }

        public void mnuImport_Click(object eventSender, EventArgs eventArgs)
        {
            cmdDialogOpen.Title = "Import Map";

            cmdDialogOpen.Filter = "Export Files|*.export|All Files (*.*)|*.*";
            cmdDialogOpen.FilterIndex = 2;

            cmdDialogOpen.ShowDialog();

            ImportNewMap(cmdDialogOpen.FileName);
        }

        public void mnuImportRefresh_Click(object eventSender, EventArgs eventArgs)
        {
            MainModule.RecalibrateImport();

        }

        public void mnuLoadMapping_Click(object eventSender, EventArgs eventArgs)
        {
            cmdDialogOpen.Title = "Load Mapping";


            cmdDialogOpen.Filter = "Mapping File (*.mpn)|*.mpn";
            cmdDialogOpen.FilterIndex = 1;
            cmdDialogOpen.FileName = "";

            cmdDialogOpen.DefaultExt = "mpn";

            cmdDialogOpen.ShowDialog();

            MainModule.LoadMapping(cmdDialogOpen.FileName);

        }

        public void mnuNew_Click(object eventSender, EventArgs eventArgs)
        {

            NewMap();

        }

        private void ImportNewMap(string filename)
        {
            frmImport frm = new frmImport();

            if (frm.DoImport(this, filename) == DialogResult.OK)
            {
                TheMap = frm.TheMap;
            }
        }

        private void NewMap()
        {
            int j;
            int i;

            frmProperties prop = new frmProperties();
            prop.Text = "New Map";

            if (prop.ShowDialog() == DialogResult.Cancel)
                return;

            TheMap = prop.TheMap;

            for (i = 0; i <= TheMap.Width; i++)
            {
                for (j = 0; j <= TheMap.Height; j++)
                {
                    PaintLoc(i, j, 0);
                }
            }

            sbRight.Maximum = TheMap.Width;
            sbRight.Minimum = 0;
            sbRight.Value = 0;

            sbDown.Maximum = TheMap.Height;
            sbDown.Minimum = 0;
            sbDown.Value = 0;


            MainModule.LoadTiles(TheMap.TileSet);

            validMap = true;

            SetMenus();
            Redraw();
        }

        public void mnuParameters_Click(object eventSender, EventArgs eventArgs)
        {
            // TODO: Figure out what this is
            //frmImport.ShowDialog()

            //LoadTiles(TileSet)
            //blit()
        }

        public void mnuProperties_Click(object eventSender, EventArgs eventArgs)
        {
            frmProperties frm = new frmProperties();

            frm.TheMap = TheMap;
            frm.ShowDialog(this);
        }

        public void mnuQuit_Click(object eventSender, EventArgs eventArgs)
        {
            Display.Dispose();
            this.Dispose();
        }


        public void mnuRefreshTiles_Click(object eventSender, EventArgs eventArgs)
        {
            MainModule.LoadTiles(TheMap.TileSet);
        }

        public void mnuSaveAs_Click(object eventSender, EventArgs eventArgs)
        {
            cmdDialogSave.Title = "Save Map";

            cmdDialogSave.Filter = "Xml Map Format (*.xmf)|*.xmf|All Files (*.*)|*.*";
            cmdDialogSave.FilterIndex = 1;
            cmdDialogSave.FileName = "";

            if (cmdDialogSave.ShowDialog() == DialogResult.OK)
            {
                MainModule.fileName = cmdDialogSave.FileName;

                mnuSave_Click(mnuSave, new EventArgs());
            }
        }



        public void mnuSaveMapping_Click(object eventSender, EventArgs eventArgs)
        {

            cmdDialogOpen.Title = "Save Mapping";
            cmdDialogSave.Title = "Save Mapping";

            cmdDialogOpen.Filter = "Mapping File (*.mpn)|*.mpn";
            cmdDialogSave.Filter = "Mapping File (*.mpn)|*.mpn";
            cmdDialogOpen.FilterIndex = 1;
            cmdDialogSave.FilterIndex = 1;
            cmdDialogOpen.FileName = "";
            cmdDialogSave.FileName = "";

            cmdDialogSave.ShowDialog();
            cmdDialogOpen.FileName = cmdDialogSave.FileName;

            MainModule.SaveMapping(cmdDialogOpen.FileName);

            //ErrorHandler:
            1.GetHashCode(); //nop

        }

        private void Picture1_MouseDown(object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
        {
            int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
            float x = eventArgs.X;
            float y = eventArgs.Y;
            int yy;
            int xx;

            xx = (int)x;
            yy = (int)y;

            xx = (xx / MainModule.TileSize) + topLeft.X;
            yy = (yy / MainModule.TileSize) + topLeft.Y;

            if (MainModule.UpdateScreen == false)
                return;

            if (eventArgs.Button == MouseButtons.Left)
            {
                SetPos(xx, yy);
            }
            else if (eventArgs.Button == MouseButtons.Right && xx >= 0 && yy >= 0)
            {
                // TODO: fix this hack
                Picture1_MouseMove(Picture1, eventArgs);
            }

            Redraw();
        }

        private void Picture1_MouseMove(object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
        {
            int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
            float x = eventArgs.X;
            float y = eventArgs.Y;
            int r;
            int xx;
            int yy;
            int tile = 0;
            // 			int xx;
            // 			int yy;
            // 			int tile;

            xx = (int)x;
            yy = (int)y;

            xx = (xx / MainModule.TileSize) + topLeft.X;
            yy = (yy / MainModule.TileSize) + topLeft.Y;

            if (MainModule.UpdateScreen == false)
                return;

            if (eventArgs.Button == MouseButtons.Left)
            {
                SetPosTwo(xx, yy);
                Redraw();
            }
            else if (eventArgs.Button == MouseButtons.Right && xx >= 0 && yy >= 0)
            {
                if (MainModule.ImportMap)
                {
                    PaintLoc(xx, yy, MainModule.currentTile);
                }
                else
                {
                    if (chkRestrict.CheckState != 0)
                    {
                        if (xx < p1.X || xx > p2.X || yy < p1.Y || yy > p2.Y)
                            return;
                    }

                    if (chkRandom.CheckState == 0)
                    {
                        PaintLoc(xx, yy, MainModule.currentTile);
                    }
                    else
                    {
                        if (MainModule.currentTile == 7)
                        {
                            r = MainModule.Random.Next(4);

                            if (r < 2)
                            {
                                tile = MainModule.currentTile + r;
                            }
                            if (r > 1)
                            {
                                tile = MainModule.currentTile + r + 14;
                            }

                            PaintLoc(xx, yy, tile);
                        }
                        else if (MainModule.currentTile == 2 || MainModule.currentTile == 129 || MainModule.currentTile == 182)
                        {
                            r = MainModule.Random.Next(2);

                            tile = MainModule.currentTile + r;

                            PaintLoc(xx, yy, tile);

                        }
                        else
                        {
                            PaintLoc(xx, yy, MainModule.currentTile);
                        }
                    }
                }

                Redraw();
            }

        }

        private void Picture1_Paint(object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
        {
            Redraw();
        }

        private void Picture2_MouseDown(object eventSender, System.Windows.Forms.MouseEventArgs eventArgs)
        {
            int Button = System.Convert.ToInt32(eventArgs.Button) / 0x100000;
            int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;
            int x = eventArgs.X;
            int y = eventArgs.Y;
            int yy;
            int xx;
            int tile;

            xx = x;
            yy = y;

            xx = xx / 16;
            yy = yy / 16;

            tile = yy * 16 + xx;

            if (MainModule.UpdateScreen == false)
            {
                return;
            }

            if (Button == 2)
            {
                PaintLoc(p1.X, p1.Y, tile);
            }

            MainModule.currentTile = tile;

            Redraw();
        }

        private void Picture2_Paint(object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
        {
            Redraw();
        }

        public void mnuSave_Click(object eventSender, EventArgs eventArgs)
        {
            
            SaveMap();
        }

        private void SaveMap()
        {
            if (MainModule.fileName == "")
            {
                mnuSaveAs_Click(mnuSaveAs, new EventArgs());
                return;
            }

            if (Path.GetExtension(MainModule.fileName) != ".xmf")
            {
                throw new ArgumentException();
                //old_SaveMap();
            }

            string filename = MainModule.fileName;

            XleMap.SaveMap(TheMap, filename);

            //XleMap saveMap = TheMap;

            //switch (MainModule.MapType)
            //{
            //    case 1:
            //        saveMap = new ERY.Xle.XleMapTypes.Museum();
            //        break;

            //    case 2:
            //        saveMap = new ERY.Xle.XleMapTypes.Outside();
            //        break;

            //    case 3:
            //        saveMap = new ERY.Xle.XleMapTypes.Town();
            //        break;

            //    case 4:
            //        saveMap = new ERY.Xle.XleMapTypes.Dungeon();
            //        break;

            //    case 5:
            //        saveMap = new ERY.Xle.XleMapTypes.Castle();
            //        break;

            //}

            //saveMap.InitializeMap(MainModule.mapWidth, MainModule.mapHeight);
            //saveMap.MapName = MainModule.mapName.Value.Trim();

            //for (j = 0; j <= MainModule.mapHeight - 1; j++)
            //{
            //    for (i = 0; i <= MainModule.mapWidth - 1; i++)
            //    {
            //        saveMap[i, j] = MainModule.mMap[i, j];
            //    }
            //}

            //if (typeof(ERY.Xle.XleMapTypes.Town).IsAssignableFrom(saveMap.GetType()))
            //{
            //    ERY.Xle.XleMapTypes.Town t = (ERY.Xle.XleMapTypes.Town) saveMap;

            //    t.OutsideTile = MainModule.defaultTile;

            //    t.BuyRaftMap = MainModule.BuyRaftMap;
            //    t.BuyRaftPt = new Agate.Point(MainModule.BuyRaftX, MainModule.BuyRaftY);

            //    for (i = 0; i <= MainModule.mail.GetUpperBound(0); i++)
            //    {
            //        if (MainModule.mail[i] != 0)
            //        {
            //            t.Mail.Add(MainModule.mail[i]);
            //        }
            //    }

            //}

            //if (typeof(IHasGuards).IsAssignableFrom(saveMap.GetType()))
            //{
            //    IHasGuards h = (IHasGuards) saveMap;

            //    for (i = 1; i <= MainModule.guard.GetUpperBound(0); i++)
            //    {
            //        Guard g = new Guard();

            //        g.Location = Interop.Convert(MainModule.guard[i]);
            //        g.HP = MainModule.guardHP;
            //        g.Facing = Direction.South;
            //        g.Attack = MainModule.guardAttack;
            //        g.Defense = MainModule.guardDefense;

            //        if (g.Location.IsEmpty == false)
            //        {
            //            h.Guards.Add(g);
            //        }

            //    }
            //}

            //if (typeof(IHasRoofs).IsAssignableFrom(saveMap.GetType()))
            //{
            //    for (i = 1; i <= MainModule.maxRoofs; i++)
            //    {
            //        Roof r = new Roof();

            //        if (MainModule.Roofs[i].width == 0 && MainModule.Roofs[i].height == 0)
            //        {
            //            continue;
            //        }

            //        r.SetSize(MainModule.Roofs[i].width, MainModule.Roofs[i].height);
            //        r.Location = new Agate.Point(MainModule.Roofs[i].anchorTarget.X - MainModule.Roofs[i].anchor.X, MainModule.Roofs[i].anchorTarget.Y - MainModule.Roofs[i].anchor.Y);

            //        for (int k = 0; k <= MainModule.Roofs[i].width - 1; k++)
            //        {
            //            for (j = 0; j <= MainModule.Roofs[i].height - 1; j++)
            //            {
            //                r[k, j] = MainModule.Roofs[i].Matrix[k, j];
            //            }
            //        }

            //        ((IHasRoofs) saveMap).Roofs.Add(r);
            //    }
            //}

            //for (i = 1; i <= NumSpecials(); i++)
            //{
            //    XleEvent evt = null;
            //    char[] c = MainModule.specialdata[i].Value.ToCharArray();

            //    switch (MainModule.special[i])
            //    {
            //        case 1:
            //            ERY.Xle.XleEventTypes.ChangeMapEvent e = new ERY.Xle.XleEventTypes.ChangeMapEvent();

            //            e.MapID = Strings.Asc(c[0]) * 256 + Strings.Asc(c[1]);
            //            e.Location = new Agate.Point(Strings.Asc(c[2]) * 256 + Strings.Asc(c[3]), Strings.Asc(c[4]) * 256 + Strings.Asc(c[5]));

            //            if (Strings.Asc(c[6]) < 11 || Strings.Asc(c[6]) == 32)
            //            {
            //                e.Ask = true;
            //            }
            //            else
            //            {
            //                e.Ask = false;
            //            }

            //            evt = e;
            //            break;

            //        case 2:
            //            ERY.Xle.XleEventTypes.StoreBank e_1 = new ERY.Xle.XleEventTypes.StoreBank();
            //            evt = e_1;
            //            break;

            //        case 3:
            //            ERY.Xle.XleEventTypes.StoreWeapon e_2 = new ERY.Xle.XleEventTypes.StoreWeapon();
            //            evt = e_2;
            //            break;

            //        case 4:
            //            ERY.Xle.XleEventTypes.StoreArmor e_3 = new ERY.Xle.XleEventTypes.StoreArmor();
            //            evt = e_3;
            //            break;

            //        case 5:
            //            ERY.Xle.XleEventTypes.StoreWeaponTraining e_4 = new ERY.Xle.XleEventTypes.StoreWeaponTraining();
            //            evt = e_4;
            //            break;

            //        case 6:
            //            ERY.Xle.XleEventTypes.StoreArmorTraining e_5 = new ERY.Xle.XleEventTypes.StoreArmorTraining();
            //            evt = e_5;
            //            break;

            //        case 7:
            //            ERY.Xle.XleEventTypes.StoreBlackjack e_6 = new ERY.Xle.XleEventTypes.StoreBlackjack();
            //            evt = e_6;
            //            break;

            //        case 8:
            //            ERY.Xle.XleEventTypes.StoreLending e_7 = new ERY.Xle.XleEventTypes.StoreLending();
            //            evt = e_7;
            //            break;

            //        case 9:
            //            ERY.Xle.XleEventTypes.StoreRaft e_8 = new ERY.Xle.XleEventTypes.StoreRaft();
            //            evt = e_8;
            //            break;

            //        case 10:
            //            ERY.Xle.XleEventTypes.StoreHealer e_9 = new ERY.Xle.XleEventTypes.StoreHealer();
            //            evt = e_9;
            //            break;

            //        case 11:
            //            ERY.Xle.XleEventTypes.StoreJail e_10 = new ERY.Xle.XleEventTypes.StoreJail();
            //            evt = e_10;
            //            break;

            //        case 12:
            //            ERY.Xle.XleEventTypes.StoreFortune e_11 = new ERY.Xle.XleEventTypes.StoreFortune();
            //            evt = e_11;
            //            break;

            //        case 13:
            //            ERY.Xle.XleEventTypes.StoreFlipFlop e_12 = new ERY.Xle.XleEventTypes.StoreFlipFlop();
            //            evt = e_12;
            //            break;

            //        case 14:
            //            ERY.Xle.XleEventTypes.StoreBuyback e_13 = new ERY.Xle.XleEventTypes.StoreBuyback();
            //            evt = e_13;
            //            break;

            //        case 15:
            //            ERY.Xle.XleEventTypes.StoreFood e_14 = new ERY.Xle.XleEventTypes.StoreFood();
            //            evt = e_14;
            //            break;

            //        case 16:
            //            ERY.Xle.XleEventTypes.StoreVault e_15 = new ERY.Xle.XleEventTypes.StoreVault();
            //            evt = e_15;
            //            break;

            //        case 17:
            //            ERY.Xle.XleEventTypes.StoreMagic e_16 = new ERY.Xle.XleEventTypes.StoreMagic();
            //            evt = e_16;
            //            break;

            //        case 23:
            //        case 25:
            //            ERY.Xle.XleEventTypes.ItemAvailableEvent e_17;

            //            if (MainModule.special[i] == 23)
            //            {
            //                e_17 = new ERY.Xle.XleEventTypes.TreasureChestEvent();
            //            }
            //            else
            //            {
            //                e_17 = new ERY.Xle.XleEventTypes.TakeEvent();
            //            }

            //            evt = e_17;

            //            if (Strings.Asc(MainModule.specialdata[i].Value[0]) == 0)
            //            {
            //                e_17.ContainsItem = true;
            //                e_17.Contents = Strings.Asc(MainModule.specialdata[i].Value[1]);

            //            }
            //            else
            //            {
            //                e_17.ContainsItem = false;
            //                e_17.Contents = Strings.Asc(MainModule.specialdata[i].Value[1]) * 256 + Strings.Asc(MainModule.specialdata[i].Value[2]);
            //            }
            //            break;

            //        case 24:
            //            ERY.Xle.XleEventTypes.Door e_18 = new ERY.Xle.XleEventTypes.Door();
            //            evt = e_18;

            //            e_18.RequiredItem = Strings.Asc(MainModule.specialdata[i].Value[0]);
            //            break;

            //    }

            //    if ((evt) is ERY.Xle.XleEventTypes.Store)
            //    {
            //        ERY.Xle.XleEventTypes.Store st = (ERY.Xle.XleEventTypes.Store) evt;

            //        st.ShopName = MainModule.specialdata[i].Value;

            //        // TODO: Fix this:
            //        if (st.ShopName.Contains("\\\\"))
            //        {
            //            //st.ShopName = st.ShopName.Split("\\", )(0)
            //        }
            //        else if (st.ShopName.Contains("\\0"))
            //        {
            //            //st.ShopName = st.ShopName.Split(Chr(0))(0)
            //        }
            //        else
            //        {
            //        }

            //    }
            //    //enum StoreType
            //    //{
            //        //	storeBank = 2,					// 2
            //        //	storeWeapon,					// 3
            //        //	storeArmor,						// 4
            //        //	storeWeaponTraining,			// 5
            //        //	storeArmorTraining,				// 6
            //        //	storeBlackjack,					// 7
            //        //	storeLending,					// 8
            //        //	storeRaft,						// 9
            //        //	storeHealer,					// 10
            //        //	storeJail,						// 11
            //        //	storeFortune,					// 12
            //        //	storeFlipFlop,					// 13
            //        //	storeBuyback,					// 14
            //        //	storeFood,						// 15
            //        //	storeVault,						// 16
            //        //	storeMagic						// 17
            //        //};

            //        if (evt != null)
            //        {

            //            evt.X = MainModule.specialx[i];
            //            evt.Y = MainModule.specialy[i];
            //            evt.Width = MainModule.specialwidth[i];
            //            evt.Height = MainModule.specialheight[i];

            //            saveMap.Events.Add(evt);
            //        }

            //    }

            // Now serialize it.
            //System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //using (Stream ff = File.OpenWrite(MainModule.fileName))
            //{
            //    formatter.Serialize(ff, saveMap);

            //    ff.Flush();
            //}



        }
        //private void old_SaveMap()
        //{
        //    string path;
        //    int offset;
        //    int file;
        //    Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString mn = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(16);
        //    int j;
        //    int i;
        //    int k;
        //    Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString test = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(1);

        //    mn.Value = MainModule.mapName.Value;



        //    Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString a = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(1);
        //    byte b;
        //    if (MainModule.fileName != "")
        //    {
        //        path = MainModule.fileName;

        //        file = FileSystem.FreeFile();


        //        FileSystem.Kill(path);
        //        FileSystem.FileOpen(file, path, OpenMode.Binary, (OpenAccess) (-1), (OpenShare) (-1), -1);

        //        offset = 1;


        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapWidth / 256))); //0
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapWidth % 256))); //1
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapHeight / 256))); //2
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.mapHeight % 256))); //3
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.fileOffset / 256))); //4
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.fileOffset % 256))); //5
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.MapType)); //6
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++;

        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, mn.Value, offset, false);

        //        offset = offset + mn.Value.Length; //7

        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, "@", offset, false);

        //        offset++; //23

        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, MainModule.defaultTile, offset);

        //        offset++; //24

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardHP / 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //25

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardHP % 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //26

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardAttack / 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //27

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardAttack % 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //28

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardDefense / 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //29

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardDefense % 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //30

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardColor / 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //31

        //        a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guardColor % 256)));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, offset, false);

        //        offset++; //32

        //        if (MainModule.TileSet == "Tiles.bmp")
        //        {
        //            a.Value = System.Convert.ToString('\0');
        //        }
        //        else if (MainModule.TileSet == "TownTiles.bmp")
        //        {
        //            a.Value = System.Convert.ToString('\u0001');
        //        }
        //        else if (MainModule.TileSet == "CastleTiles.bmp")
        //        {
        //            a.Value = System.Convert.ToString('\u0002');
        //        }
        //        else if (MainModule.TileSet == "LOB Tiles.bmp")
        //        {
        //            a.Value = System.Convert.ToString('\u0003');
        //        }
        //        else if (MainModule.TileSet == "LOB TownTiles.bmp")
        //        {
        //            a.Value = System.Convert.ToString('\u0004');
        //        }

        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 34, false);

        //        offset++; //33

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftMap)); //36  Buy Raft Map
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 37, false);

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftX / 256));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 38, false); //37  Buy Raft X
        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftX % 256));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 39, false); //38  Buy Raft X

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftY / 256));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 40, false); //39  Buy Raft y
        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.BuyRaftY % 256));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 41, false); //40  Buy Raft Y

        //        a.Value = System.Convert.ToString('\u0078');
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 42, false); //41 special count

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[0]));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 43, false); // 42 mail 0

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[1]));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 44, false); // 43 mail 1

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[2]));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 45, false); // 44 mail 2

        //        a.Value = System.Convert.ToString(Strings.Chr(MainModule.mail[3]));
        //        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //        FileSystem.FilePut(file, a.Value, 46, false); // 45 mail 3

        //        offset = MainModule.fileOffset + 1;

        //        for (j = 0; j <= MainModule.mapHeight - 1; j++)
        //        {
        //            for (i = 0; i <= MainModule.mapWidth - 1; i++)
        //            {

        //                //UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                b = (byte) (MainModule.Map(i, j));

        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, b, offset);
        //                offset++;

        //            }
        //        }

        //        //offset = (mapHeight + 1) * mapWidth + 1

        //        for (i = 1; i <= MainModule.maxSpecial; i++)
        //        {
        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, MainModule.special[i], offset);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialx[i] / 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialx[i] % 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialy[i] / 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialy[i] % 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialwidth[i] / 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialwidth[i] % 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialheight[i] / 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.specialheight[i] % 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, offset, false);

        //            offset++;

        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, MainModule.specialdata[i], offset);
        //            //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //            offset = offset + Strings.Len(MainModule.specialdata[i]);
        //        }

        //        if (MainModule.MapType == System.Convert.ToInt32(MainModule.EnumMapType.maptown)|| MainModule.MapType == System.Convert.ToInt32(MainModule.EnumMapType.mapCastle))
        //        {

        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, "5555557", offset, false);

        //            offset = offset + "5555557".Length;

        //            for (i = 0; i <= 100; i++)
        //            {
        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].X / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].X % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].Y / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.guard[i].Y % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;
        //            }

        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int((offset - 1) / 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, 35, false); // 34 (roof offset)

        //            a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int((offset - 1) % 256)));
        //            //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //            FileSystem.FilePut(file, a.Value, 36, false); // 35 (roof offset)

        //            for (i = 1; i <= MainModule.maxRoofs; i++)
        //            {
        //                // anchor
        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.X / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.X % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.Y / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchor.Y % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                // anchortarget
        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.X / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.X % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.Y / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].anchorTarget.Y % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //  Dimensions
        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].width / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].width % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].height / 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                a.Value = System.Convert.ToString(Strings.Chr(Conversion.Int(MainModule.Roofs[i].height % 256)));
        //                //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                FileSystem.FilePut(file, a.Value, offset, false);

        //                offset++;

        //                //  Data
        //                //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                for (j = 0; j <= MainModule.Roofs[i].height - 1; j++)
        //                {
        //                    //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                    for (k = 0; k <= MainModule.Roofs[i].width - 1; k++)
        //                    {
        //                        //UPGRADE_WARNING: Couldn't resolve default property of object j. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                        //UPGRADE_WARNING: Couldn't resolve default property of object k. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                        //UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        //                        a.Value = System.Convert.ToString(Strings.Chr(MainModule.Roofs[i].Matrix[k, j]));
        //                        //UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //                        FileSystem.FilePut(file, a.Value, offset, false);

        //                        offset++;
        //                    }
        //                }
        //            }

        //        }

        //        FileSystem.FileClose(file);

        //        //UPGRADE_WARNING: Lower bound of collection StatusBar1.Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        //        StatusBar1.Items[1].Text = "Saved successfully: " + DateAndTime.TimeOfDay;

        //    }

        //    blit();

        //}
        public void mnuOpen_Click(object eventSender, EventArgs eventArgs)
        {
            OpenMapDialog();

        }

        private void OpenMapDialog()
        {
            try
            {

                cmdDialogOpen.Title = "Open Map";

                cmdDialogOpen.Filter = "All Map Files|*.bmf;*.map;*.twn;*.xmf|Xml Map Format files (*.xmf)|*.xmf|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*";
                cmdDialogOpen.FilterIndex = 2;

                cmdDialogOpen.InitialDirectory = MainModule.LotaPath + "\\Included Maps";
                cmdDialogOpen.FileName = "";

                cmdDialogOpen.DefaultExt = "map";

                if (cmdDialogOpen.ShowDialog() == DialogResult.Cancel)
                    return;

                MainModule.fileName = cmdDialogOpen.FileName;

                OpenMap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void OpenMap()
        {
            TheMap = XleMap.LoadMap(MainModule.fileName, 0);

            MainModule.LoadTiles(TheMap.TileSet);

            Redraw();
        }


        public void SetPos(int xx, int yy)
        {
            if (TheMap == null)
                return;

            bool found = false;

            p1.X = xx;
            p1.Y = yy;

            p2.X = xx;
            p2.Y = yy;

            for (int i = 0; i < TheMap.Events.Count; i++)
            {
                XleEvent evt = TheMap.Events[i];

                if (evt.X == p1.X && evt.Y == p1.Y)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                cmdPlaceEvent.Enabled = false;
                cmdModifySpecial.Enabled = true;
                cmdDeleteSpecial.Enabled = true;
            }
            else
            {
                cmdPlaceEvent.Enabled = true;
                cmdModifySpecial.Enabled = false;
                cmdDeleteSpecial.Enabled = false;
            }
        }

        public void SetPosTwo(int xx, int yy)
        {
            p2.X = xx;
            p2.Y = yy;
        }

        private void PaintLoc(int x, int y, int value)
        {
            if (MainModule.ImportMap)
            {
                MainModule.PaintArea(x, y, value);
            }
            else
            {
                TheMap[x, y] = value;
            }

        }

        private void SetMenus()
        {

            mnuSave.Enabled = !MainModule.ImportMap;
            mnuSaveAs.Enabled = !MainModule.ImportMap;
            mnuProperties.Enabled = !MainModule.ImportMap;
            mnuPlaceSpecial.Enabled = !MainModule.ImportMap;
            mnuModifySpecial.Enabled = !MainModule.ImportMap;
            mnuDeleteSpecial.Enabled = !MainModule.ImportMap;

            mnuFinalize.Enabled = MainModule.ImportMap;
        }

        int EventIndex
        {
            get { return (int)nudEvent.Value; }
        }

        private void btnFindEvent_Click(object sender, EventArgs e)
        {

            p1.X = TheMap.Events[EventIndex].X;
            p1.Y = TheMap.Events[EventIndex].Y;

            p2.X = p1.X;
            p2.Y = p1.Y;

            CenterOn(p1.X, p1.Y);

        }
        private void CenterOn(int x, int y)
        {
            sbRight.Value = Math.Max(0, x - topLeft.X + tilesDrawn.Width / 2);
            sbDown.Value = Math.Max(0, y - topLeft.Y + tilesDrawn.Height / 2);

            Redraw();
        }

        [Obsolete]
        private void SortSpecials()
        {
            int i;
            int j;
            int max;

            max = 0;

            for (i = 120; i >= 1; i--)
            {
                if (MainModule.special[i] == 0)
                {
                    if (i < max)
                    {
                        for (j = i; j <= max; j++)
                        {
                            MainModule.special[i] = MainModule.special[i + 1];
                            MainModule.specialx[i] = MainModule.specialx[i + 1];
                            MainModule.specialy[i] = MainModule.specialy[i + 1];
                            MainModule.specialdata[i] = MainModule.specialdata[i + 1];
                            MainModule.specialwidth[i] = MainModule.specialwidth[i + 1];
                            MainModule.specialheight[i] = MainModule.specialheight[i + 1];
                        }

                    }

                }
                else if (MainModule.special[i] > 0)
                {
                    if (i > max)
                    {
                        max = i;
                    }

                }

            }

        }

        [Obsolete]
        private int NumSpecials()
        {
            int i;

            SortSpecials();

            for (i = 1; i <= 120; i++)
            {
                if (MainModule.special[i] == 0)
                {
                    return i - 1;
                }
            }

            return 0;
        }


        private void sbDown_Scroll(object sender, ScrollEventArgs e)
        {
            dispY = sbDown.Value;
            Redraw();
        }
        private void sbRight_Scroll(object sender, ScrollEventArgs e)
        {
            dispX = sbRight.Value;
            Redraw();
        }

        private void frmMEdit_KeyDown(object sender, KeyEventArgs e)
        {
            bool refresh = false;

            if (e.KeyCode == Keys.Down)
            {
                if (e.Shift == false)
                    p1.Y++; 
                p2.Y++;

                refresh = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (e.Shift == false)
                    p1.Y--;
                p2.Y--;
                refresh = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (e.Shift == false)
                    p1.X--;
                p2.X--;
                refresh = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (e.Shift == false)
                    p1.X++;
                p2.X++;
                refresh = true;
            }

            if (refresh)
            {
                e.Handled = true;
                Redraw();
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewMap();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenMapDialog();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMap();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }
        private void btnPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        void Copy()
        {
            copiedData = TheMap.ReadMapData(SelRect.X, SelRect.Y, SelRect.Width, SelRect.Height);

            Redraw();
        }
        void Paste()
        {
            TheMap.WriteMapData(copiedData, p1.X, p1.Y);

            Redraw();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            mnuProperties_Click(sender, e);
        }

    }
}