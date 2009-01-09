using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ERY.Xle;

namespace XleMapEditor
{
	partial class frmSpecial : System.Windows.Forms.Form
	{
        XleEvent evt;

        public XleMap TheMap { get; set; }
        public XleEvent Event
        {
            get { return evt; }
            set
            {
                evt = value;
                propertyGrid1.SelectedObject = evt;
            }
        }
        public Rectangle EventRect { get; set; }

        public frmSpecial()
        {
            InitializeComponent();

            cboType.Items.AddRange(XleFactory.EventTypes.ToArray());
            cboType.Enabled = false;
        }

        public DialogResult ShowNewEvent(IWin32Window owner)
        {
            cboType.Enabled = true;

            return ShowDialog(Owner);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type type = (Type)cboType.SelectedItem;

            Event = (XleEvent)Activator.CreateInstance(type);

            Event.X = EventRect.X;
            Event.Y = EventRect.Y;
            Event.Width = EventRect.Width;
            Event.Height = EventRect.Height;
        }
    }
}
