﻿using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xle.Ancients;
using Xle.Serialization;

namespace Xle.Ancients.TitleScreen
{
    public abstract class FileMenu : TitleState
    {
        private int menuSelection;
        private int page;
        private List<string> files = new List<string>();

        private int maxPages { get { return (files.Count - 1) / 8; } }

        private bool titleDone;
        protected TextWindow filesWindow = new TextWindow();

        public FileMenu(IGamePersistance gamePersistance)
        {
            files.AddRange(gamePersistance.FindExistingGames());

            filesWindow.Location = new Point(11, 8);

            Windows.Add(filesWindow);
        }

        private int FileStartIndex { get { return page * 8; } }

        protected override void DrawWindows(SpriteBatch spriteBatch)
        {
            base.DrawWindows(spriteBatch);

            Point pt = filesWindow.Location;

            pt.X -= 2;
            pt.Y += menuSelection;

            TextRenderer.WriteText(spriteBatch, pt.X * 16, pt.Y * 16, "`");
        }
        public override void Update(GameTime time)
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

        public override async Task KeyPress(Keys keyCode, string keyString)
        {
            if (keyCode == Keys.Down)
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
            else if (keyCode == Keys.Up)
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
            else if (keyCode >= Keys.D0 && keyCode <= Keys.D9)
            {
                menuSelection = keyCode - Keys.D0;

                keyCode = Keys.Enter;
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

            if (keyCode == Keys.Enter)
            {
                SoundMan.PlaySound(LotaSound.TitleAccept);

                await Wait(500);

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