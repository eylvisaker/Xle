
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.WinForms;
using ERY.Xle;
using Agate = AgateLib.Geometry;


// TODO: fix all mouse down events to use integers instead of singles!
//
namespace XleMapEditor
{
	partial class frmMapEdit : System.Windows.Forms.Form
	{
		EditorState mState;

		EditorState State
		{
			get { return mState; }
			set
			{
				mState = value;

				mapView.State = value;
				tilePicker.State = value;
			}
		}


		MapData copiedData;

		public XleMap TheMap { get { return State.TheMap; } }

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

			tile = tilePicker.SelectedTileIndex;
			if (chkRandom.Checked)
			{
				if (tilePicker.SelectedTileIndex == 7)
				{
					r = MainModule.Random.Next(4);

					if (r < 2)
						tile = tilePicker.SelectedTileIndex + r;
					if (r > 1)
					{
						tile = tilePicker.SelectedTileIndex + r + 14;
					}

				}
				else if (tilePicker.SelectedTileIndex == 2 || tilePicker.SelectedTileIndex == 129 || tilePicker.SelectedTileIndex == 182)
				{
					r = MainModule.Random.Next(2);

					tile = tilePicker.SelectedTileIndex + r;

				}
			}

			for (i = mapView.SelRect.Left; i < mapView.SelRect.Right; i++)
			{
				for (j = mapView.SelRect.Top; j < mapView.SelRect.Bottom; j++)
				{
					if (System.Convert.ToInt32(chkRandom.CheckState) == 1)
					{
						if (tilePicker.SelectedTileIndex == 7)
						{
							r = MainModule.Random.Next(4);

							if (r < 2)
							{
								tile = tilePicker.SelectedTileIndex + r;
							}
							if (r > 1)
							{
								tile = tilePicker.SelectedTileIndex + r + 14;
							}

						}
						else if (tilePicker.SelectedTileIndex == 2 || tilePicker.SelectedTileIndex == 129 || tilePicker.SelectedTileIndex == 182)
						{
							r = MainModule.Random.Next(2);

							tile = tilePicker.SelectedTileIndex + r;

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

				if (g.X == mapView.SelRect.X && g.Y == mapView.SelRect.Y)
				{
					guards.Guards.RemoveAt(i);
					i--;
					found = true;
				}
			}

			if (found == false)
			{
				Guard g = new Guard();

				g.X = mapView.SelRect.X;
				g.Y = mapView.SelRect.Y;

				guards.Guards.Add(g);

			}

			Redraw();
		}

		private void cmdModifySpecial_Click(object eventSender, EventArgs eventArgs)
		{
			for (int i = 0; i < TheMap.Events.Count; i++)
			{
				XleEvent evt = TheMap.Events[i];

				if (evt.X == mapView.SelRect.X && evt.Y == mapView.SelRect.Y)
				{
					frmEvent frm = new frmEvent();

					frm.TheMap = TheMap;
					frm.Event = evt;

					frm.ShowDialog();

					UpdatePos();

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
						PaintLoc(mapView.SelRect.X + i, mapView.SelRect.Y + j, MainModule.PreDefObjects[index].Matrix[i, j]);
					}
				}
			}

			Redraw();
		}

		private void cmdPlaceSpecial_Click(object eventSender, EventArgs eventArgs)
		{
			frmEvent frm = new frmEvent();

			frm.EventRect = mapView.SelRect;

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
			frm.State = State;

			for (int i = 0; i < roof.Roofs.Count; i++)
			{
				if (roof.Roofs[i].PointInRoof(mapView.SelRect.X, mapView.SelRect.Y) == false)
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

		bool UpdateScreen;

		//UPGRADE_WARNING: Form event frmMEdit.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
		private void frmMEdit_Activated(object eventSender, EventArgs eventArgs)
		{
			UpdateScreen = true;

		}

		//UPGRADE_WARNING: Form event frmMEdit.Deactivate has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
		private void frmMEdit_Deactivate(object eventSender, EventArgs eventArgs)
		{
			UpdateScreen = false;
		}

		private void frmMEdit_Load(object eventSender, EventArgs eventArgs)
		{
			mapView.CreateDisplayWindow();
			tilePicker.CreateDisplayWindow();

			MainModule.CreateSurfaces();

			cmdDialogOpen.InitialDirectory = MainModule.LotaPath;
			cmdDialogSave.InitialDirectory = MainModule.LotaPath;

			Redraw();
		}

		private void frmMEdit_Paint(object eventSender, System.Windows.Forms.PaintEventArgs eventArgs)
		{

		}


		public void Redraw()
		{
			if (UpdateScreen == false)
				return;
			if (State == null)
				return;

			mapView.Redraw();
			tilePicker.Redraw();

			if (copiedData == null || mapView.SelRect.Width > 1 || mapView.SelRect.Height > 1 ||
				copiedData.Width + mapView.SelRect.X >= TheMap.Width ||
				copiedData.Height + mapView.SelRect.Y >= TheMap.Height)
			{
				btnPaste.Enabled = false;
				pasteToolStripMenuItem.Enabled = false;
			}
			else
			{
				btnPaste.Enabled = true;
				pasteToolStripMenuItem.Enabled = true;
			}


			lblDim.Text = "Map Dimensions: " + TheMap.Width + " x " + TheMap.Height;
			if (MainModule.fileName != "")
			{
				this.Text = "Xle Map Editor - " + MainModule.fileName;
			}

			nudEvent.Minimum = 0;
			nudEvent.Maximum = TheMap.Events.Count - 1;

			lblEventCount.Text = "Event Count: " + TheMap.Events.Count;


			lblX.Text = "x:   " + mapView.SelRect.X + "    width: " + mapView.SelRect.Width;
			lblY.Text = "y:   " + mapView.SelRect.Y + "    height: " + mapView.SelRect.Height;

			if (mapView.SelRect.X < 0 || mapView.SelRect.X > TheMap.Width || mapView.SelRect.Y < 0 || mapView.SelRect.Y > TheMap.Height)
				lblTile.Text = "Tile: Out of range";
			else
				lblTile.Text = "Tile: " + TheMap[mapView.SelRect.X, mapView.SelRect.Y] + "   0x" + string.Format("{0:X}", TheMap[mapView.SelRect.X, mapView.SelRect.Y]);

			lblCurrentTile.Text = "Current Tile: " + tilePicker.SelectedTileIndex + "   0x" + string.Format("{0:X}", tilePicker.SelectedTileIndex);

			if (TheMap is IHasGuards)
				cmdGuard.Visible = true;
			else
				cmdGuard.Visible = false;
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

		public void mnuNew_Click(object eventSender, EventArgs eventArgs)
		{

			NewMap();

		}

		private void ImportNewMap(string filename)
		{
			frmImport frm = new frmImport();

			EditorState newstate = new EditorState();

			frm.DoImport(filename);
		}

		private void NewMap()
		{
			frmProperties prop = new frmProperties();
			prop.Text = "New Map";

			if (prop.ShowDialog() == DialogResult.Cancel)
				return;

			EditorState state = new EditorState
			{
				TheMap = prop.TheMap,
			};

			state.LoadTiles();
			State = state;

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
			if (State == null)
				return;

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
			State.LoadTiles();
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

		private void mapView_TileMouseDown(object sender, TileMouseEventArgs e)
		{
			int Shift = System.Convert.ToInt32(System.Windows.Forms.Control.ModifierKeys) / 0x10000;

			if (UpdateScreen == false)
				return;

			if (e.Button == MouseButtons.Left)
			{
				UpdatePos();
			}
			else if (e.Button == MouseButtons.Right)
			{
				// TODO: fix this hack
				mapView_TileMouseMove(sender, e);
			}

			Redraw();
		}
		private void mapView_TileMouseMove(object sender, TileMouseEventArgs e)
		{
			if (UpdateScreen == false)
				return;

			if (e.Button != MouseButtons.Right)
				return;

			if (chkRestrict.CheckState != 0)
			{
				if (mapView.SelRect.Contains(e.Tile.X, e.Tile.Y) == false)
					return;
			}

			if (chkRandom.CheckState == 0)
			{
				PaintLoc(e.Tile.X, e.Tile.Y, tilePicker.SelectedTileIndex);
			}
			else
			{
				if (tilePicker.SelectedTileIndex == 7)
				{
					int r = MainModule.Random.Next(4);
					int tile = tilePicker.SelectedTileIndex;

					if (r < 2)
					{
						tile = tilePicker.SelectedTileIndex + r;
					}
					if (r > 1)
					{
						tile = tilePicker.SelectedTileIndex + r + 14;
					}

					PaintLoc(e.Tile.X, e.Tile.Y, tile);
				}
				else if (tilePicker.SelectedTileIndex == 2 || tilePicker.SelectedTileIndex == 129 || tilePicker.SelectedTileIndex == 182)
				{
					int r = MainModule.Random.Next(2);
					int tile = tilePicker.SelectedTileIndex + r;

					PaintLoc(e.Tile.X, e.Tile.Y, tile);

				}
				else
				{
					PaintLoc(e.Tile.X, e.Tile.Y, tilePicker.SelectedTileIndex);
				}
			}
		}

		private void tilePicker_TilePick(object sender, TilePickEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				PaintLoc(mapView.SelRect.X, mapView.SelRect.Y, e.Tile);
				Redraw();
			}

		}

		public void mnuSave_Click(object eventSender, EventArgs eventArgs)
		{
			SaveMap();
		}

		private void SaveMap()
		{
			if (string.IsNullOrEmpty(MainModule.fileName))
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
			EditorState s = new EditorState();

			s.TheMap = XleMap.LoadMap(MainModule.fileName, 0);
			s.LoadTiles();

			State = s;

			Redraw();
		}


		public void UpdatePos()
		{
			if (TheMap == null)
				return;

			bool found = false;

			for (int i = 0; i < TheMap.Events.Count; i++)
			{
				XleEvent evt = TheMap.Events[i];

				if (evt.X == mapView.SelRect.X && evt.Y == mapView.SelRect.Y)
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

		private void PaintLoc(int x, int y, int value)
		{
			TheMap[x, y] = value;
		}

		int EventIndex
		{
			get { return (int)nudEvent.Value; }
		}

		private void btnFindEvent_Click(object sender, EventArgs e)
		{
			mapView.SelRect = new Rectangle(
				TheMap.Events[EventIndex].X,
				TheMap.Events[EventIndex].Y,
				1, 1);

			mapView.CenterOnSel();
		}

		private void frmMEdit_KeyDown(object sender, KeyEventArgs e)
		{
			bool refresh = false;

			//if (e.KeyCode == Keys.Down)
			//{
			//    if (e.Shift == false)
			//        p1.Y++; 
			//    p2.Y++;

			//    refresh = true;
			//}
			//else if (e.KeyCode == Keys.Up)
			//{
			//    if (e.Shift == false)
			//        p1.Y--;
			//    p2.Y--;
			//    refresh = true;
			//}
			//else if (e.KeyCode == Keys.Left)
			//{
			//    if (e.Shift == false)
			//        p1.X--;
			//    p2.X--;
			//    refresh = true;
			//}
			//else if (e.KeyCode == Keys.Right)
			//{
			//    if (e.Shift == false)
			//        p1.X++;
			//    p2.X++;
			//    refresh = true;
			//}

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
			copiedData = TheMap.ReadMapData(
				mapView.SelRect.X, mapView.SelRect.Y, mapView.SelRect.Width, mapView.SelRect.Height);

			Redraw();
		}
		void Paste()
		{
			TheMap.WriteMapData(copiedData, mapView.SelRect.X, mapView.SelRect.Y);

			Redraw();
		}

		private void btnProperties_Click(object sender, EventArgs e)
		{
			mnuProperties_Click(sender, e);
		}

		private void chkDrawRoof_CheckedChanged(object sender, EventArgs e)
		{
			mapView.DrawRoofs = chkDrawRoof.Checked;
		}

		private void chkDrawGuards_CheckedChanged(object sender, EventArgs e)
		{
			mapView.DrawGuards = chkDrawGuards.Checked;
		}

		private void mapView_TileMouseUp(object sender, TileMouseEventArgs e)
		{

		}

		private void btnZoomOut_Click(object sender, EventArgs e)
		{
			State.DisplaySize--;

			mapView.Redraw();
		}

		private void btnZoomIn_Click(object sender, EventArgs e)
		{
			State.DisplaySize++;

			mapView.Redraw();
		}


	}
}