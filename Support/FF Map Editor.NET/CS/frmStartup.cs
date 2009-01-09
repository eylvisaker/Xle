#define Win32
using System.Diagnostics;
using System;
using System.Windows.Forms;
using AxMSComCtl2;
using ERY.Xle;
using ERY.AgateLib;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Collections;
using Microsoft.VisualBasic.Compatibility;
using System.Linq;
using System.IO;
using ERY.AgateLib.WinForms;
using Microsoft.VisualBasic.CompilerServices;

namespace XleMapEditor
{
	partial class frmStartup : System.Windows.Forms.Form
	{
		
		#region Default Instance
		
		private static frmStartup defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmStartup Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmStartup();
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
		private void cmdCreateMap_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			frmProperties.Default.SetDefaults();
			frmProperties.Default.ShowDialog();
			
			
			if (MainModule.SelectedOK == true)
			{
				MainModule.StartNewMap = true;
				this.Hide();
			}
			
		}
		
		private void cmdExit_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			Display.Dispose();
			ProjectData.EndApp();
		}
		
		private void cmdImport_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			try
			{
				
				cmdDialogOpen.Title = "Import Map";
				
				//cmdDialogOpen.FileName = "E:\Legacy\."
				//UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
				cmdDialogOpen.Filter = "Export Files|*.export|All Files (*.*)|*.*";
				cmdDialogOpen.FilterIndex = 1;
				
				cmdDialogOpen.InitialDirectory = (new global::Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\..\\Included Maps";
				
				cmdDialogOpen.DefaultExt = "map";
				
				cmdDialogOpen.ShowDialog();
				
				MainModule.fileName = cmdDialogOpen.FileName;
				
				MainModule.StartNewMap = false;
				MainModule.ImportMap = true;
				
				this.Hide();
				
			}
			catch
			{
				
			}
			
		}
		
		private void cmdOpenMap_Click(System.Object eventSender, System.EventArgs eventArgs)
		{
			
			try
			{
				
				cmdDialogOpen.Title = "Open Map";
				
				//UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
				cmdDialogOpen.Filter = "All Map Files|*.map;*.twn|Outside Map Files (*.map)|*.map|Town Map Files (*.twn)|*.twn|All Files (*.*)|*.*";
				cmdDialogOpen.FilterIndex = 1;
				
				
				cmdDialogOpen.InitialDirectory = MainModule.LotaPath + "\\Included Maps";
				
				
				cmdDialogOpen.DefaultExt = "map";
				
				cmdDialogOpen.ShowDialog();
				
				MainModule.fileName = cmdDialogOpen.FileName;
				
				MainModule.StartNewMap = false;
				
				this.Hide();
				
				
			}
			catch (Exception)
			{
				
			}
			
		}
		
		private void frmStartup_FormClosed(System.Object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
		{
			ProjectData.EndApp();
		}
	}
}
