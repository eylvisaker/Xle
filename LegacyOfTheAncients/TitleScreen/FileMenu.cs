using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.TitleScreen
{
	public abstract class FileMenu : TitleState
	{
		int menuSelection;
		int page;
		List<string> files = new List<string>();
		int maxPages { get { return (files.Count - 1) / 8; } }
		bool titleDone;
		protected TextWindow filesWindow = new TextWindow();

		public FileMenu()
		{
			SetFiles();

			filesWindow.Location = new Point(11, 8);

			Windows.Add(filesWindow);
		}

		private void SetFiles()
		{
			string savedDirectory = "Saved";

			if (Directory.Exists(savedDirectory) == false)
				Directory.CreateDirectory(savedDirectory);

			string[] sourceFiles = Directory.GetFiles(savedDirectory);

			files.AddRange(sourceFiles);
		}

		int FileStartIndex { get { return page * 8; } }

		protected override void DrawWindows()
		{
			base.DrawWindows();

			Point pt = filesWindow.Location;

			pt.X -= 2;
			pt.Y += menuSelection;

			Renderer.WriteText(pt.X * 16, pt.Y * 16, "`");
		}
		public override void Update()
		{
			filesWindow.Clear();

			if (page == 0)
				filesWindow.WriteLine("0.  Cancel");
			else
				filesWindow.WriteLine("0.  Previous Page");

			for (int i = 0; i < 8; i++)
			{
				filesWindow.Write((i + 1).ToString());
				filesWindow.Write(".  ");

				if (files.Count <= FileStartIndex + i)
					filesWindow.WriteLine("Empty");
				else
				{
					filesWindow.WriteLine(Path.GetFileNameWithoutExtension(files[i]));
				}
			}

			if (page < maxPages)
				filesWindow.WriteLine("9.  Next Page");
		}

		public override void KeyDown(KeyCode keyCode, string keyString)
		{
			if (keyCode == KeyCode.Down)
			{
				menuSelection++;

				if (menuSelection >= 9)
					menuSelection = 9;
				else if (files.Count < FileStartIndex + menuSelection)
				{
					menuSelection = 9;
				}

				SoundMan.PlaySound(LotaSound.TitleCursor);
			}
			else if (keyCode == KeyCode.Up)
			{
				do
				{
					menuSelection--;

					if (menuSelection <= 0)
						break;

				} while (menuSelection > 0 &&
					files.Count < FileStartIndex + menuSelection);

				if (menuSelection < 0)
					menuSelection = 0;

				SoundMan.PlaySound(LotaSound.TitleCursor);
			}
			else if (keyCode >= KeyCode.D0 && keyCode <= KeyCode.D9)
			{
				menuSelection = keyCode - KeyCode.D0;

				keyCode = KeyCode.Return;
			}

			if (menuSelection == 9 && page == maxPages)
			{
				menuSelection = 8;

				do
				{
					menuSelection--;

					if (menuSelection <= 0)
						break;

				} while (menuSelection > 0 && files.Count < FileStartIndex + menuSelection);

			}

			if (keyCode == KeyCode.Return)
			{
				SoundMan.PlaySound(LotaSound.TitleAccept);

				Wait(500);

				if (menuSelection == 0)
				{
					if (page > 0)
					{
						page--;
					}
					else
					{
						UserSelectedCancel();
					}
				}
				else if (menuSelection == 9)
				{
					page++;

					if (page > maxPages)
						page = maxPages;
				}
				else
				{
					int index = FileStartIndex + menuSelection - 1;

					if (index < files.Count)
					{
						string file = files[index];

						if (string.IsNullOrEmpty(file) == false)
						{
							UserSelectedFile(file);
						}
					}
				}
			}
		}

		protected abstract void UserSelectedFile(string file);
		protected abstract void UserSelectedCancel();
	}
}