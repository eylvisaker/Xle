using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using ERY.Xle;
using AgateLib.Serialization.Xle;

namespace XleMapEditor
{
	public partial class frmImport : Form
	{
		public frmImport()
		{
			InitializeComponent();

			tilePicker.CreateDisplayWindow();
			mapView.CreateDisplayWindow();

			State = new EditorState();
			rawDataChain = new int[DataWidth * DataHeight];

			cboIntSize.SelectedIndex = 0;

			cboMapType.Items.AddRange(XleFactory.MapTypes.ToArray());
			cboMapType.SelectedIndex = 0;

			cboTileSize.SelectedIndex = 0;
		}

		IntPtr mData;
		int mDataLength;
		int[] rawDataChain;
		MapData mUnconvertedMap;

		MappingInfo mMapping = new MappingInfo();

		public EditorState State { get; set; }

		public void DoImport(string filename)
		{
			byte[] data;

			using (Stream st = File.OpenRead(filename))
			{
				data = new byte[st.Length];

				if (st.Length > int.MaxValue)
					throw new InvalidOperationException("File is too large.");
				st.Read(data, 0, (int)st.Length);
			}

			mDataLength = data.Length;
			this.mData = Marshal.AllocHGlobal(data.Length);
			Marshal.Copy(data, 0, mData, data.Length);

			nudOffset.Maximum = data.Length;
			Import();

			Show();
		}

		private void cboMapType_SelectedIndexChanged(object sender, EventArgs e)
		{
			Type theType = (Type)cboMapType.SelectedItem;
			State.TheMap = (XleMap)Activator.CreateInstance(theType);

			cboTiles.Items.Clear();
			cboTiles.Items.AddRange(State.TheMap.AvailableTilesets.ToArray());

			if (cboTiles.Items.Count > 0)
				cboTiles.SelectedIndex = 0;

			mapView.State = State;
			tilePicker.State = State;

			Import();
		}
		private void nudOffset_ValueChanged(object sender, EventArgs e)
		{
			Import();
		}
		private void nudWidth_ValueChanged(object sender, EventArgs e)
		{
			rawDataChain = new int[DataWidth * DataHeight];

			Import();
		}
		private void nudHeight_ValueChanged(object sender, EventArgs e)
		{
			rawDataChain = new int[DataWidth * DataHeight];

			Import();
		}
		private void cboIntSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			nudOffset.Increment = IntSize;

			Import();
		}
		private void cboTileSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			mMapTileSize = int.Parse(cboTileSize.Text);

			Import();
		}

		int mMapTileSize;

		int IntSize
		{
			get { return int.Parse(cboIntSize.Text); }
		}
		int DataWidth
		{
			get { return (int)nudWidth.Value; }
		}
		int DataHeight
		{
			get { return (int)nudHeight.Value; }
		}
		int Offset
		{
			get { return (int)nudOffset.Value; }
		}
		int MapTileSize
		{
			get { return mMapTileSize; }
		}
		int MapWidth
		{
			get { return DataWidth * MapTileSize; }
		}
		int MapHeight
		{
			get { return DataHeight * MapTileSize; }
		}
		bool Rle
		{
			get { return chkRLE.Checked; }
		}

		private void cboTiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			State.TheMap.TileSet = cboTiles.Text;
			State.LoadTiles();

			mapView.Redraw();
			tilePicker.Redraw();
		}

		unsafe private void CopyByBytes(IntPtr data, int length)
		{
			byte[] bar = new byte[length];

			int max = Math.Min(DataWidth * DataHeight, length);

			Marshal.Copy(data, bar, 0, length);

			if (Rle)
			{
				// decode run-length-encoded data.
				int index = 0;
				for (int i = 0; i < length; i += 2)
				{
					for (int j = 0; j < bar[i]; j++, index++)
					{
						if (index >= rawDataChain.Length)
							break;

						rawDataChain[index] = bar[i + 1];
					}

					if (index >= rawDataChain.Length)
						break;

				}
			}
			else
			{
				for (int i = 0; i < DataWidth * DataHeight; i++)
				{
					if (i >= bar.Length)
						rawDataChain[i] = 0;
					else
						rawDataChain[i] = bar[i];
				}
			}
		}

		private void Import()
		{
			if (mData == IntPtr.Zero)
				return;

			unsafe
			{
				byte* ptr = (byte*)mData + Offset;
				IntPtr data = (IntPtr)ptr;

				switch (IntSize)
				{
					case 1:
						CopyByBytes(data, mDataLength - Offset);
						break;
					case 2:
						short[] sar = new short[DataWidth * DataHeight];
						Marshal.Copy(data, sar, 0, DataWidth * DataHeight);

						for (int i = 0; i < DataWidth * DataHeight; i++)
						{
							rawDataChain[i] = sar[i];
						}
						break;

					case 4:
						Marshal.Copy(data, rawDataChain, 0, DataWidth * DataHeight);

						break;
				}
			}

			if (MapWidth != State.TheMap.Width || MapHeight != State.TheMap.Height)
			{
				State.TheMap.InitializeMap(MapWidth, MapHeight);
				mUnconvertedMap = new MapData(DataWidth, DataHeight);
			}

			int index = 0;

			for (int y = 0; y < DataHeight; y++)
			{
				for (int x = 0; x < DataWidth; x++)
				{
					int oldTile = rawDataChain[index];
					int tile = oldTile;
					int targetX = x * MapTileSize;
					int targetY = y * MapTileSize;

					if (mMapping.ContainsKey(oldTile) &&
						mMapping[oldTile].Width == MapTileSize)
					{
						MapData dat = mMapping[oldTile];

						State.TheMap.WriteMapData(dat,
							targetX, targetY);
					}
					else
					{
						for (int j = 0; j < MapTileSize; j++)
						{
							for (int i = 0; i < MapTileSize; i++)
							{
								State.TheMap[targetX + i, targetY + j] = tile;
							}
						}

					}

					mUnconvertedMap[x, y] = oldTile;
					index++;
				}
			}
			mapView.Redraw();
		}


		private void frmImport_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.PageUp)
			{
				try
				{
					nudOffset.Value -= DataWidth * DataHeight * IntSize / 2;
				}
				catch (ArgumentOutOfRangeException)
				{
					System.Media.SystemSounds.Beep.Play();
				}

				e.Handled = true;
			}
			else if (e.KeyCode == Keys.PageDown)
			{
				try
				{
					nudOffset.Value += DataWidth * DataHeight * IntSize / 2;
				}
				catch (ArgumentOutOfRangeException)
				{
					System.Media.SystemSounds.Beep.Play();
				}

				e.Handled = true;
			}
		}

		private void tilePicker_TilePick(object sender, TilePickEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				WriteMapping();
		}

		private void WriteMapping()
		{
			int sourceTile = mUnconvertedMap[
				mapView.SelRect.X / MapTileSize,
				mapView.SelRect.Y / MapTileSize];
			int offsetX = mapView.SelRect.X % MapTileSize;
			int offsetY = mapView.SelRect.Y % MapTileSize;

			MapData map;

			if (mMapping.ContainsKey(sourceTile))
			{
				map = mMapping[sourceTile];

				if (map.Width != MapTileSize)
				{
					map = new MapData(MapTileSize, MapTileSize);
					map.SetValue(sourceTile);
				}
			}
			else
			{
				map = new MapData(MapTileSize, MapTileSize);
				map.SetValue(sourceTile);
			}

			map[offsetX, offsetY] = tilePicker.SelectedTileIndex;
			mMapping[sourceTile] = map;

			Import();
		}

		private void zoomOut_Click(object sender, EventArgs e)
		{
			State.DisplaySize--;

			mapView.Redraw();
		}

		private void zoomIn_Click(object sender, EventArgs e)
		{
			State.DisplaySize++;

			mapView.Redraw();
		}

		private void btnLoadMapping_Click(object sender, EventArgs e)
		{
			if (openFile.ShowDialog() == DialogResult.Cancel)
				return;

			XleSerializer ser = new XleSerializer(mMapping.GetType());

			using (FileStream st = File.OpenRead(openFile.FileName))
			{
				mMapping = (MappingInfo)ser.Deserialize(st);
			}

			Import();
		}
		private void btnSaveMapping_Click(object sender, EventArgs e)
		{
			if (saveFile.ShowDialog() == DialogResult.Cancel)
				return;

			XleSerializer ser = new XleSerializer(mMapping.GetType());

			using (FileStream st = File.Open(saveFile.FileName, FileMode.Create))
			{
				ser.Serialize(st, mMapping);
			}
		}

		private void mapView_Load(object sender, EventArgs e)
		{

		}

		private void mapView_TileMouseDown(object sender, TileMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				mapView_TileMouseMove(sender, e);
		}

		private void mapView_TileMouseMove(object sender, TileMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				WriteMapping();
		}

		private void btnSaveMap_Click(object sender, EventArgs e)
		{
			if (xmfSave.ShowDialog() == DialogResult.OK)
			{
				XleMap.SaveMap(State.TheMap, xmfSave.FileName);
			}
		}

		private void btnSaveToMap_Click(object sender, EventArgs e)
		{
			if (xmfSave.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(xmfSave.FileName) == false)
				{
					MessageBox.Show("You did not select an existing XMF file.");
				}
				else
				{
					var map = XleMap.LoadMap(xmfSave.FileName, 0);

					map.InitializeMap(State.TheMap.Width, State.TheMap.Height);
					State.TheMap = map;

					Import();

					XleMap.SaveMap(map, xmfSave.FileName);
				}
			}
		}

		private void chkRLE_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void btnPageUp_Click(object sender, EventArgs e)
		{
			try
			{
				nudOffset.Value -= DataWidth * DataHeight;
			}
			catch
			{
				nudOffset.Value = 0;
			}
		}

		private void btnPageDown_Click(object sender, EventArgs e)
		{
			try
			{
				nudOffset.Value += DataWidth * DataHeight;
			}
			catch
			{
				nudOffset.Value = nudOffset.Maximum;
			}
		}

		private void btnLineUp_Click(object sender, EventArgs e)
		{
			try
			{
				nudOffset.Value -= DataWidth;
			}
			catch
			{
				nudOffset.Value = 0;
			}
		}

		private void btnLineDown_Click(object sender, EventArgs e)
		{
			try
			{
				nudOffset.Value += DataWidth;
			}
			catch
			{
				nudOffset.Value = nudOffset.Maximum;
			}
		}


	}

	public class MappingInfo : IDictionary<int, MapData>, IXleSerializable
	{
		Dictionary<int, MapData> mMapping = new Dictionary<int, MapData>();

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			int[] keys = mMapping.Keys.ToArray();
			MapData[] data = new MapData[keys.Length];

			for (int i = 0; i < keys.Length; i++)
				data[i] = mMapping[keys[i]];

			info.Write("Keys", keys);
			info.Write("Data", data);
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			int[] keys = info.ReadInt32Array("Keys");
			MapData[] data = info.ReadArray<MapData>("Data");

			for (int i = 0; i < keys.Length; i++)
			{
				mMapping[keys[i]] = data[i];
			}
		}

		#endregion

		#region IDictionary<int,MapData> Members

		public void Add(int key, MapData value)
		{
			mMapping.Add(key, value);
		}

		public bool ContainsKey(int key)
		{
			return mMapping.ContainsKey(key);
		}

		public ICollection<int> Keys
		{
			get { return mMapping.Keys; }
		}

		public bool Remove(int key)
		{
			return mMapping.Remove(key);
		}

		public bool TryGetValue(int key, out MapData value)
		{
			return mMapping.TryGetValue(key, out value);
		}

		public ICollection<MapData> Values
		{
			get { return mMapping.Values; }
		}

		public MapData this[int key]
		{
			get
			{
				return mMapping[key];
			}
			set
			{
				mMapping[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<int,MapData>> Members

		public void Add(KeyValuePair<int, MapData> item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(KeyValuePair<int, MapData> item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(KeyValuePair<int, MapData>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return mMapping.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<int, MapData> item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<KeyValuePair<int,MapData>> Members

		public IEnumerator<KeyValuePair<int, MapData>> GetEnumerator()
		{
			return mMapping.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

	}

}