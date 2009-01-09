using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERY.Xle;
using System.IO;
using System.Runtime.InteropServices;


namespace XleMapEditor
{
    public partial class frmImport : Form
    {
        public frmImport()
        {
            InitializeComponent();
        }

        IntPtr mData;

        public XleMap TheMap { get; set; }

        public DialogResult DoImport(IWin32Window owner, string filename)
        {
            byte[] data;

            using (Stream st = File.OpenRead(filename))
            {
                data = new byte[st.Length];

                if (st.Length > int.MaxValue)
                    throw new InvalidOperationException("File is too large.");
                st.Read(data, 0, (int)st.Length);
            }

            this.mData = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, mData, data.Length);

            if (ShowDialog(owner) == DialogResult.Cancel)
            {
                return DialogResult.Cancel;
            }

            return DialogResult.OK;
        }
    }
}
