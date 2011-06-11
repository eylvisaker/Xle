using System;
using System.Linq;
using System.Windows.Forms;
using ERY.Xle;
using Microsoft.VisualBasic;

namespace XleMapEditor
{
	partial class frmProperties : System.Windows.Forms.Form
	{
		private bool newMap = true;
		private XleMap map;
		bool loading = false;

		public frmProperties()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			cboTypes.Items.AddRange(XleFactory.MapTypes.ToArray());

		}
		public XleMap TheMap
		{
			get { return map; }
			set
			{
				map = value;
				newMap = false;
				propertyGrid1.SelectedObject = TheMap;

				LoadProperties();
				cboTypes.Enabled = false;
			}
		}
		public void LoadProperties()
		{
			try
			{
				loading = true;

				txtName.Text = TheMap.MapName;
				txtDefaultTile.Text = TheMap.DefaultTile.ToString();

				cboTypes.SelectedItem = TheMap.GetType();
				cboTypes.Enabled = false;

				Text = txtName.Text.TrimStart() + " Properties";
				txtWidth.Text = TheMap.Width.ToString();
				txtHeight.Text = TheMap.Height.ToString();
				cboTileset.Text = TheMap.TileSet;
			}
			finally
			{
				loading = false;
			}

			UpdateControls();

		}

		private void cmdOK_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			AssignProperties();
		}

		public void AssignProperties()
		{
			TheMap.MapName = txtName.Text;
			TheMap.DefaultTile = int.Parse(txtDefaultTile.Text);
			TheMap.TileSet = cboTileset.Text;

			int newWidth = int.Parse(txtWidth.Text);
			int newHeight = int.Parse(txtHeight.Text);

			if (newMap)
			{
				TheMap.InitializeMap(newWidth, newHeight);
			}

			if (newWidth != TheMap.Width || newHeight != TheMap.Height)
			{
				//TheMap.Resize(newSize.Width, newSize.Height);
			}
		}

		private void frmProperties_Load(object sender, System.EventArgs e)
		{
			cboTypes.Items.AddRange(XleFactory.MapTypes.ToArray());
		}


		private void txtAttack_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtBpT_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtColor_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtDefaultTile_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtDefense_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtHeight_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtHP_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}
		private void txtName_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}

		private void UpdateControls()
		{
			if (loading)
				return;

			bool OKEnabled = true;

			int result;

			if (cboTypes.SelectedItem == null)
				grpTileset.Enabled = false;
			else
				grpTileset.Enabled = true;


			if (txtName.Text == "" || txtHeight.Text == "" || txtWidth.Text == "")
			{
				OKEnabled = false;
			}

			if (int.TryParse(txtWidth.Text, out result) && int.TryParse(txtHeight.Text, out result))
			{
				if (int.Parse(txtWidth.Text) < 1) OKEnabled = false;
				if (int.Parse(txtHeight.Text) < 1) OKEnabled = false;
			}
			else
			{
				OKEnabled = false;
			}

			if (cboTileset.Items.Count == 0)
			{
				cboTileset.Items.AddRange(map.AvailableTilesets.ToArray());

				if (cboTileset.Items.Count > 0)
					cboTileset.SelectedIndex = 0;
			}

			cmdOK.Enabled = OKEnabled;

		}

		public void SetDefaults()
		{
			txtName.Text = "";
		}

		private void txtWidth_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
		{
			UpdateControls();
		}

		private void cboTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (loading)
				return;

			if (cboTypes.SelectedItem == null)
			{
				map = null;
				UpdateControls();
				return;
			}

			Type t = (Type)cboTypes.SelectedItem;

			map = (XleMap)Activator.CreateInstance(t);
			cboTileset.Items.Clear();

			UpdateControls();
		}

	}
}
