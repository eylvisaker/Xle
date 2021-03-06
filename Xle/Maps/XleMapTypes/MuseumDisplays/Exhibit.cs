﻿using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Game;
using Xle.Menus;
using Xle.ScreenModel;
using Xle.XleSystem;

namespace Xle.Maps.XleMapTypes.MuseumDisplays
{
    [InjectProperties]
    public abstract class Exhibit
    {
        protected Exhibit(string name)
        {
            Name = name;
        }

        public GameState GameState { get; set; }
        public XleData Data { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public ITextArea TextArea { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleGameControl GameControl { get; set; }

        protected Player Player { get { return GameState.Player; } }

        public string Name { get; private set; }
        public virtual string LongName { get { return Name; } }

        public abstract Color ExhibitColor { get; }

        public abstract int ExhibitID { get; }

        /// <summary>
        /// Gets the color of the text in the museum. Defaults to ExhibitColor.
        /// </summary>
        public virtual Color TitleColor
        {
            get { return ExhibitColor; }
        }
        protected virtual string RawText
        {
            get
            {
                if (Data.ExhibitInfo.ContainsKey(ExhibitID))
                {
                    var exinfo = Data.ExhibitInfo[ExhibitID];

                    if (exinfo.Text.ContainsKey(1))
                        return Data.ExhibitInfo[ExhibitID].Text[1];
                    else
                        return "This exhibit does not have any text with key 1.";
                }
                else
                {
                    return "This exhibit is not working.";
                }
            }

        }

        public virtual void Update(GameTime time)
        {

        }

        protected int ImageID { get; set; }

        public virtual async Task RunExhibit()
        {
            if (await CheckOfferReread() == false)
                return;

            try
            {
                await ReadRawText(RawText);
            }
            catch(Exception ex)
            {
                Debugger.Break();
            }
            
            if (HasBeenVisited == false)
                MarkAsVisited();
        }

        protected abstract void MarkAsVisited();

        /// <summary>
        /// Returns true if we are reading the exhibit for the first time,
        /// or if the player answered yes to rereading the exhibit.
        /// </summary>
        /// <param name="Player"></param>
        /// <returns></returns>
        protected async Task<bool> CheckOfferReread()
        {
            if (HasBeenVisited)
            {
                return await OfferReread();
            }

            return true;
        }

        /// <summary>
        /// Asks the player if they want to reread the description of the exhibit.
        /// </summary>
        /// <returns>True if the player chose yes, false otherwise.</returns>
        protected async Task<bool> OfferReread()
        {
            TextArea.Clear();
            await TextArea.PrintLine("Do you want to reread the");
            await TextArea.PrintLine("description of this exhibit?");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
                return true;
            else
                return false;
        }

        protected virtual Color ArticleTextColor { get { return XleColor.Cyan; } }
        protected virtual int TextAreaMargin { get { return 0; } }

        protected async Task ReadRawText(string rawtext)
        {
            TextArea.Margin = TextAreaMargin;
            TextArea.Clear(true);

            int ip = 0;
            int line = 4;
            Color clr = ArticleTextColor;
            ColorStringBuilder text = new ColorStringBuilder();
            bool waiting = true;

            while (ip < rawtext.Length)
            {
                // ignore any \r characters. we will interpret \n as the new line character.
                if (rawtext[ip] == '\r') { ip++; continue; }

                if (rawtext[ip] == '\n')
                {
                    line--;
                    text = new ColorStringBuilder();

                    int i = 1;
                    while (rawtext[ip + i] == ' ')
                        i++;

                    if (rawtext[ip + i] == '|')
                        ip += i - 1;

                    await TextArea.PrintLine();
                }
                else if (rawtext[ip] == '|' && (text.Text == null || text.Text.Trim() == ""))
                {
                    text.Clear();
                }
                else if (rawtext[ip] != '`')
                {
                    text.AddText(rawtext[ip].ToString(), clr);
                    await TextArea.Print(rawtext[ip].ToString(), clr);

                    if (waiting)
                    {
                        string punctuation = ",.!";
                        int time = 30;

                        if (punctuation.Contains(rawtext[ip].ToString()))
                            time = 350 * (1 + punctuation.IndexOf(rawtext[ip]));

                        await GameControl.WaitAsync(time, true);
                    }
                }
                else
                {
                    int next = rawtext.IndexOf('`', ip + 1);
                    if (next < 0)
                        throw new ArgumentException("Text had unmatched quote!");

                    string substr = rawtext.Substring(ip + 1, next - ip - 1);

                    ip = next;

                    if (substr.StartsWith("image:"))
                    {
                        int image = int.Parse(substr.Substring(6));

                        ImageID = image;
                    }
                    else
                    {
                        switch (substr)
                        {
                            case "": break;
                            case "white": clr = XleColor.White; break;
                            case "cyan": clr = XleColor.Cyan; break;
                            case "yellow": clr = XleColor.Yellow; break;
                            case "green": clr = XleColor.Green; break;
                            case "purple": clr = XleColor.Purple; break;

                            case "pause":
                                await GameControl.WaitForKey();

                                break;

                            case "clear":
                                TextArea.Clear(true);
                                line = 4;
                                break;

                            case "sound:VeryGood":
                                SoundMan.PlaySound(LotaSound.VeryGood);
                                break;

                            case "wait:off":
                                waiting = false;
                                break;

                            case "wait:on":
                                waiting = true;
                                break;

                            default:
                                System.Diagnostics.Trace.WriteLine("Failed to understand command: " + substr);
                                break;
                        }
                    }
                }

                ip++;
            }

            TextArea.Margin = 1;
            await TextArea.PrintLine();
        }

        public virtual bool IsClosed
        {
            get { return false; }
        }

        /// <summary>
        /// Returns true if we should draw the static before a coin is inserted.
        /// </summary>
        public virtual bool StaticBeforeCoin
        {
            get { return true; }
        }

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle displayRect);

        public abstract string InsertCoinText { get; }

        public abstract bool RequiresCoin { get; }

        public abstract bool HasBeenVisited { get; }

        public virtual string IntroductionText
        {
            get { return "You see a plaque.  It Reads..."; }
        }

        public abstract string UseCoinMessage { get; }

        public abstract bool PlayerHasCoin { get; }
        public abstract void UseCoin();
    }
}