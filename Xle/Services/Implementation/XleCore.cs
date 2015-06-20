using System;
using System.Linq;
using System.Windows.Input;

using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;
using AgateLib.Platform;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation
{
    public class XleCore : IXleService
    {
        #region --- Static Members ---


        public const int myWindowWidth = 640;
        public const int myWindowHeight = 400;

        public static Random random = new Random();

        private static bool AcceptKey
        {
            get { return inst.input.AcceptKey; }
            set { inst.input.AcceptKey = value; }
        }

        private static XleCore inst;

        [Obsolete("Use systemState.Factory instead.")]
        public static XleGameFactory Factory
        {
            get
            {
                if (inst == null)
                    return null;

                return inst.mFactory;
            }
        }

        #endregion

        [Obsolete("Use systemState.Factory instead.")]
        private XleGameFactory mFactory { get { return (XleGameFactory)systemState.Factory; } }

        XleData mData
        {
            get { return systemState.Data; }
            set { systemState.Data = value; }
        }

        public static TextArea TextArea { get; private set; }
        XleSystemState systemState;

        public XleCore(
            XleSystemState systemState,
            IXleInput input,
            ICommandList commands,
            ICommandExecutor commandExecutor,
            IXleRenderer renderer,
            ITextArea textArea,
            GameState gameState)
        {
            inst = this;
            this.systemState = systemState;
            this.input = input;
            this.commands = commands;
            this.commandExecutor = commandExecutor;

            Renderer = (XleRenderer)renderer;
            Renderer.PlayerColor = XleColor.White;

            TextArea = (TextArea)textArea;
            Options = new XleOptions();

            GameState = gameState;
        }

        static Size windowBorderSize = new Size(20, 20);
        private IXleInput input;
        private ICommandList commands;
        private ICommandExecutor commandExecutor;

        [Obsolete("Call Display.Clear(clearColor) instead.")]
        public static void SetOrthoProjection(Color clearColor)
        {
            Display.Clear(clearColor);
        }
        [Obsolete("Use Renderer.SetProjectionAndBackColors instead")]
        public static void SetProjectionAndBackColors(ColorScheme cs)
        {
            SetOrthoProjection(cs.BorderColor);
            int hp = cs.HorizontalLinePosition * 16 + 8;

            Display.FillRect(new Rectangle(0, 0, 640, 400), cs.BackColor);
            Display.FillRect(0, hp, 640, 400 - hp, cs.TextAreaBackColor);
        }

        [Obsolete("Use SystemState.ReturnToTitle instead")]
        public static bool ReturnToTitle
        {
            get { return XleCore.inst.systemState.ReturnToTitle; }
            set { XleCore.inst.systemState.ReturnToTitle = value; }
        }

        public static GameState GameState { get; set; }
        [Obsolete("Use IXleRenderer as a service instead.")]
        public static XleRenderer Renderer { get; set; }
        public static XleOptions Options { get; set; }
        [Obsolete("Use XleData as a service instead.")]
        public static XleData Data { get { return inst.mData; } }

        [Obsolete("Use method on XleData instead.")]
        public static string GetWeaponName(int weaponID, int qualityID)
        {
            return Data.QualityList[qualityID] + " " + Data.WeaponList[weaponID].Name;
        }
        [Obsolete("Use method on XleData instead.")]
        public static string GetArmorName(int armorID, int qualityID)
        {
            return Data.QualityList[qualityID] + " " + Data.ArmorList[armorID].Name;
        }

        [Obsolete("Use method on XleData instead.")]
        public static int WeaponCost(int item, int quality)
        {
            return Data.WeaponList[item].Prices[quality];
        }
        [Obsolete("Use method on XleData instead.")]
        public static int ArmorCost(int item, int quality)
        {
            return Data.ArmorList[item].Prices[quality];
        }

        [Obsolete("Use MapLoader service instead.")]
        public static XleMap LoadMap(int mapID)
        {
            string file = "Maps/" + Data.MapList[mapID].Filename;

            return XleMap.LoadMap(file, mapID);
        }

        [Obsolete("Use MapLoader service instead.")]
        public static void SetTilesAndCommands()
        {
            inst.commands.Items.Clear();

            GameState.MapExtender.SetCommands(inst.commands);
            inst.commandExecutor.ResetCurrentCommand();

            XleCore.LoadTiles(GameState.Map.TileImage);
        }

        [Obsolete("Use Screen as a service and call Redraw")]
        public static void Redraw()
        {
            inst.RedrawImpl();
        }
        [Obsolete("Use Screen as a service and call Redraw")]
        private void RedrawImpl()
        {
            Update();

            Display.BeginFrame();

            Renderer.Draw();

            Display.EndFrame();
            XleCore.KeepAlive();

            if (AgateConsole.IsVisible == false)
            {
                CheckArrowKeys();
            }
        }

        [Obsolete("Use Screen as a service")]
        private void Update()
        {
            Renderer.UpdateAnim();

            if (GameState != null && GameState.Map != null)
            {
                GameState.Map.OnUpdate(GameState, Display.DeltaTime / 1000.0);
            }
        }
        [Obsolete("Use input as a service")]
        private void CheckArrowKeys()
        {
            if (AcceptKey == false)
                return;
            if (GameState == null)
                return;

            try
            {
                AcceptKey = false;

                if (Keyboard.Keys[KeyCode.Down]) inst.commandExecutor.DoCommand(KeyCode.Down);
                else if (Keyboard.Keys[KeyCode.Left]) inst.commandExecutor.DoCommand(KeyCode.Left);
                else if (Keyboard.Keys[KeyCode.Up]) inst.commandExecutor.DoCommand(KeyCode.Up);
                else if (Keyboard.Keys[KeyCode.Right]) inst.commandExecutor.DoCommand(KeyCode.Right);
            }
            finally
            {
                AcceptKey = true;
            }
        }

        public static void FlashHPWhileSound(Color clr)
        {
            FlashHPWhileSound(clr, Renderer.FontColor);
        }
        public static void FlashHPWhileSound(Color clr, Color clr2)
        {
            FlashHPWhile(clr, clr2, () => SoundMan.IsAnyPlaying());

        }
        public static void FlashHPWhile(Color clr, Color clr2, Func<bool> pred)
        {
            Renderer.FlashHPWhile(clr, clr2, pred);

        }


        /****************************************************************************
         *	void ChangeScreenMode()													*
         *																			*
         *																			*
         *  This function toggles between full screen and windowed.	 It currently	*
         *	will end the program if it fails.										*
         *																			*
         *	Parameters:	none.														*
         *  Returns:	void														*
         ****************************************************************************/
        public static void ChangeScreenMode()
        {
            //Rectangle rect;
            /*
            //  Release all the directdraw surfaces so we can change modes.
            g.Unlock();
            g.ReleaseFont();
            DDDestroySurfaces();

            if (g_bFullScreen)
            {
                // Go to windowed
                if (!DDCreateSurfaces(false, actualWindowWidth, actualWindowHeight, g_iBpp))
                {
                    MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);

                    return;
                }

                // Restore the window size and position
                MoveWindow(g.hwnd(), g.screenLeft, g.screenTop,
                    actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME) * 2,
                    actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME) * 2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION), true);

            }
            else
            {
                // Save the window size and position
                GetWindowRect(g.hwnd(), &rect);

                g.screenLeft = rect.left;
                g.screenTop = rect.top;

                // Go to fullscreen mode
                if (!DDCreateSurfaces(true, actualWindowWidth, actualWindowHeight, g_iBpp))
                {
                    MessageBox(NULL, TEXT("Couldn't create DirectDraw surfaces!"), TEXT("Failed"), 0);

                    return;
                }

                // Cover up the top left section of the screen
                MoveWindow(g.hwnd(), -GetSystemMetrics(SM_CXSIZEFRAME), -GetSystemMetrics(SM_CYCAPTION) - GetSystemMetrics(SM_CYSIZEFRAME),
                    actualWindowWidth + GetSystemMetrics(SM_CXSIZEFRAME) * 2,
                    actualWindowHeight + GetSystemMetrics(SM_CYSIZEFRAME) * 2 + GetSystemMetrics(SM_CYMENU) + GetSystemMetrics(SM_CYCAPTION), true);

            }

            g.LoadFont();
            g.Lock();
            g.ResetTimers();		// reset the animation timers
            */
        }

        /****************************************************************************
         *	void wait(int howLong)													*
         *																			*
         *  														*
         *																			*
         *	Parameters:	the number of milliseconds to wait							*
         *  Returns:	void														*
         ****************************************************************************/
        /// <summary>
        /// This function is a message-friendly wait that will continue screen
        /// drawing and animation.  It allows the code to pause for a specified
        /// amount of time.	
        /// </summary>
        /// <param name="howLong"></param>
        public static void Wait(int howLong)
        {
            Wait(howLong, false, Redraw);
        }
        public static void Wait(int howLong, Action redraw)
        {
            Wait(howLong, false, redraw);
        }
        public static void Wait(int howLong, bool keyBreak, Action redraw)
        {
            IStopwatch watch = Timing.CreateStopWatch();

            do
            {
                Renderer.UpdateAnim();

                redraw();
                XleCore.KeepAlive();

                if (keyBreak && Keyboard.AnyKeyPressed)
                    break;

            } while (watch.TotalMilliseconds < howLong);
        }

        /// <summary>
        /// This function creates a sub menu in the top of the map section and
        /// forces the player to chose an option from the list provided.	
        /// </summary>
        /// <param name="title"></param>
        /// <param name="choice"></param>
        /// <param name="items">A MenuItemList collection of menu items</param>
        /// <returns>The choice the user made.</returns>
        [Obsolete("Use IXleMenu service instead")]
        public static int SubMenu(string title, int choice, MenuItemList items)
        {
            return SubMenu(title, choice, items, XleColor.Black);
        }

        [Obsolete("Use IXleMenu service instead")]
        private static int SubMenu(string title, int choice, MenuItemList items, Color backColor)
        {
            SubMenu menu = new SubMenu();

            menu.title = title;
            menu.value = choice;
            menu.theList = items;
            menu.BackColor = backColor;

            return RunSubMenu(menu);
        }

        [Obsolete("Use IXleMenu service instead")]
        private static int RunSubMenu(SubMenu menu)
        {
            for (int i = 0; i < menu.theList.Count; i++)
            {
                if (menu.theList[i].Length + 6 > menu.width)
                {
                    menu.width = menu.theList[i].Length + 6;
                }

            }

            string displayTitle = "Choose " + menu.title;

            if (displayTitle.Length + 2 > menu.width)
            {
                menu.width = displayTitle.Length + 2;
            }

            Action redraw = () =>
            {
                Renderer.UpdateAnim();

                Display.BeginFrame();
                XleCore.SetProjectionAndBackColors(GameState.Map.ColorScheme);

                Renderer.Draw();
                DrawMenu(menu);

                Display.EndFrame();
                XleCore.KeepAlive();
            };

            KeyCode key;

            do
            {
                key = WaitForKey(redraw);

                if (key == KeyCode.Up)
                {
                    menu.value--;
                    if (menu.value < 0)
                        menu.value = 0;
                }
                if (key == KeyCode.Down)
                {
                    menu.value++;
                    if (menu.value >= menu.theList.Count)
                        menu.value = menu.theList.Count - 1;
                }
                else if (key >= KeyCode.D0)
                {
                    int v;

                    if (key >= KeyCode.A)
                    {
                        v = (int)(key) - (int)(KeyCode.A);
                    }
                    else
                    {
                        v = key - KeyCode.D0;
                    }

                    if (v < menu.theList.Count)
                    {
                        menu.value = v;
                        key = KeyCode.Return;
                    }
                }
            } while (key != KeyCode.Return);

            Wait(300);

            return menu.value;
        }

        [Obsolete("Use XleGameControl as a service instead.")]
        public static void KeepAlive()
        {
            if (GameState.Map != null)
            {
                XleCore.GameState.MapExtender.CheckSounds(GameState);
            }

            Core.KeepAlive();

            if (Display.CurrentWindow.IsClosed)
                throw new MainWindowClosedException();
        }

        /// <summary>
        /// Waits for one of the specified keys, while redrawing the screen.
        /// </summary>
        /// <param name="keys">A list of keys which will break out of the wait. 
        /// Pass none for any key to break out.</param>
        /// <returns></returns>
        public static KeyCode WaitForKey(params KeyCode[] keys)
        {
            return WaitForKey(Redraw, keys);
        }

        /// <summary>
        /// Waits for one of the specified keys, while calling the delegate
        /// to redraw the screen.
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="keys">A list of keys which will break out of the wait. 
        /// Pass none for any key to break out.</param>
        /// <returns></returns>
        public static KeyCode WaitForKey(Action redraw, params KeyCode[] keys)
        {
            KeyCode key = KeyCode.None;
            bool done = false;

            InputEventHandler keyhandler = e => key = e.KeyCode;

            PromptToContinue = PromptToContinueOnWait;

            Keyboard.ReleaseAllKeys();
            Keyboard.KeyDown += keyhandler;

            do
            {
                redraw();

                if (Display.CurrentWindow.IsClosed == true)
                {
                    if (keys.Length > 0)
                        key = keys[0];
                    else
                        key = KeyCode.Escape;

                    break;
                }

                if ((keys == null || keys.Length == 0) && key != KeyCode.None)
                    break;

                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == key)
                    {
                        done = true;
                        break;
                    }
                }

            } while (!done && Display.CurrentWindow.IsClosed == false);


            Keyboard.KeyDown -= keyhandler;

            PromptToContinue = false;
            PromptToContinueOnWait = true;

            return key;
        }

        /// <summary>
        /// Set to false to have WaitForKey not display a prompt 
        /// with the standard drawing method.
        /// </summary>
        public static bool PromptToContinueOnWait { get; set; }
        /// <summary>
        /// Set to true to show the (press to cont) prompt.
        /// </summary>
        public static bool PromptToContinue { get; set; }

        /// <summary>
        /// Draws the submenu created by SubMenu.
        /// </summary>
        /// <param name="menu"></param>
        [Obsolete("Use IMenuRenderer service instead.")]
        static void DrawMenu(SubMenu menu)
        {
            string thestring;
            int xx, yy, i = 0, height;
            string buffer;
            Color fontColor = GameState.Map.ColorScheme.TextColor;

            xx = 624 - menu.width * 16;
            yy = 16;
            height = (menu.theList.Count + 3) * 16;

            var vertLine = GameState.Map.ColorScheme.VerticalLinePosition;

            if (xx < vertLine + 16)
            {
                xx = vertLine + 16;
                i = 1;
            }

            Display.FillRect(xx, yy, 624 - xx, height, menu.BackColor);


            if (i == 0)
            {
                xx += 16;
            }

            thestring = menu.title;

            Renderer.WriteText(xx + (int)((624 - xx) / 32) * 16 - (int)(thestring.Length / 2) * 16,
                       yy, thestring, fontColor);

            yy += 16;

            for (i = 0; i < menu.theList.Count; i++)
            {
                yy += 16;
                buffer = menu.theList[i];

                if (i > 9)
                    thestring = ((char)(i + 'A' - 10)).ToString();
                else
                    thestring = i.ToString();

                thestring += ". " + buffer;

                Renderer.WriteText(xx, yy, thestring);

                if (i == menu.value)
                {
                    int xx1;

                    xx1 = xx + thestring.Length * 16;
                    Renderer.WriteText(xx1, yy, "`");
                }
            }
        }


        /// <summary>
        /// Gives the player a yes/no choice, returning 0 if the player chose yes and
        /// 1 if the player chose no.
        /// </summary>
        /// <param name="defaultAtNo">Pass true to have the cursor start at no.</param>
        /// <returns>Returns 0 if the player chose yes, 1 if the player chose no.</returns>
        public static int QuickMenuYesNo(bool defaultAtNo = false)
        {
            return XleCore.QuickMenu(new MenuItemList("Yes", "No"), 3, defaultAtNo ? 1 : 0);
        }
        /// <summary>
        /// This function creates a quick menu at the bottow of the screen,
        /// allowing the player to pick from a few choices.	
        /// </summary>
        /// <param name="items">The items in the list.</param>
        /// <param name="spaces"></param>
        /// <returns></returns>
        public static int QuickMenu(MenuItemList items, int spaces)
        {
            return QuickMenu(items, spaces, 0, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
        }
        public static int QuickMenu(MenuItemList items, int spaces, int value = 0)
        {
            return QuickMenu(items, spaces, value, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
        }
        public static int QuickMenu(MenuItemList items, int spaces, int value, Color clrInit)
        {
            return QuickMenu(items, spaces, value, clrInit, clrInit);
        }
        public static int QuickMenu(MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
        {
            return QuickMenu(Redraw, items, spaces, value, clrInit, clrChanged);
        }

        public static int QuickMenu(Action redraw, MenuItemList items, int spaces)
        {
            return QuickMenu(redraw, items, spaces, 0, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
        }
        public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value)
        {
            return QuickMenu(redraw, items, spaces, value, XleCore.Renderer.FontColor, XleCore.Renderer.FontColor);
        }
        public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value, Color clrInit)
        {
            return QuickMenu(redraw, items, spaces, value, clrInit, clrInit);
        }

        public static int QuickMenu(Action redraw, MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
        {
            return QuickMenuImpl(redraw, items, spaces, value, clrInit, clrChanged);
        }
        public static int QuickMenuImpl(Action redraw, MenuItemList items, int spaces, int value, Color clrInit, Color clrChanged)
        {
            int[] spacing = new int[items.Count];
            int last = 0;
            string tempLine = "Choose: ";
            string topLine;
            string bulletLine;
            int lineIndex = XleCore.TextArea.CursorLocation.Y;
            Color[] colors = new Color[40];

            if (lineIndex >= 4)
                lineIndex = 3;

            System.Diagnostics.Debug.Assert(value >= 0);
            System.Diagnostics.Debug.Assert(value < items.Count);

            if (value < 0)
                value = 0;

            for (int i = 0; i < 40; i++)
                colors[i] = clrChanged;


            spacing[0] = 8;

            // Construct the temporary line
            for (int i = 0; i < items.Count; i++)
            {
                bulletLine = items[i];

                tempLine += bulletLine + new string(' ', spaces);

                spacing[i] += last + bulletLine.Length - 1;
                last = spacing[i] + spaces + 1;
            }

            XleCore.TextArea.PrintLine(tempLine, clrInit);
            XleCore.TextArea.PrintLine();

            topLine = tempLine;
            tempLine = new string(' ', spacing[value]) + "`";

            XleCore.TextArea.RewriteLine(lineIndex + 1, tempLine, clrInit);
            PromptToContinueOnWait = false;

            KeyCode key;

            do
            {
                key = WaitForKey(redraw);

                if (key == KeyCode.Left)
                {
                    value--;
                    if (value < 0)
                        value = 0;
                }
                if (key == KeyCode.Right)
                {
                    value++;
                    if (value >= items.Count)
                        value = items.Count - 1;
                }
                else if (key >= KeyCode.D0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        bulletLine = items[i];

                        if (key - KeyCode.A ==
                            char.ToUpperInvariant(bulletLine[0]) - 'A')
                        {
                            value = i;
                            key = KeyCode.Return;
                        }
                    }
                }

                tempLine = new string(' ', spacing[value]) + "`";

                if (key != KeyCode.None)
                {
                    XleCore.TextArea.RewriteLine(lineIndex, topLine, clrChanged);
                    XleCore.TextArea.RewriteLine(lineIndex + 1, tempLine, clrChanged);
                }


            } while (key != KeyCode.Return && Display.CurrentWindow.IsClosed == false);

            Wait(100, redraw);

            XleCore.TextArea.PrintLine();

            return value;

        }

        /****************************************************************************
         *	void CheckJoystick()													*
         *																			*
         *  This function checks the current state of the joystick, and generates	*
         *	a key event if there is any action happeninGlobal.  It also handles the		*
         *	menu when the button is held.											*
         *																			*
         *	Parameters:	none														*
         *  Returns:	void														*
         ****************************************************************************/
        //static int buttonTime = 0;		// time when the button was held down
        //static int buttonHeld = 0;		// are they holding the button down?
        //static int lastMove = 0;

        [Obsolete("Use IEquipmentPicker service instead.")]
        public static ArmorItem PickArmor(ArmorItem defaultItem)
        {
            return PickArmor(GameState, defaultItem);
        }
        [Obsolete("Use IEquipmentPicker service instead.")]
        public static ArmorItem PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null)
        {
            MenuItemList theList = new MenuItemList();

            theList.Add("Nothing");
            theList.AddRange(state.Player.Armor.Select(x => x.NameWithQuality));

            int sel = XleCore.SubMenu("Pick Armor", state.Player.Armor.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Armor[sel - 1];
        }

        [Obsolete("Use IEquipmentPicker service instead.")]
        public static WeaponItem PickWeapon(WeaponItem defaultItem)
        {
            return PickWeapon(GameState, defaultItem);
        }
        [Obsolete("Use IEquipmentPicker service instead.")]
        public static WeaponItem PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null)
        {
            MenuItemList theList = new MenuItemList();

            theList.Add("Nothing");
            theList.AddRange(state.Player.Weapons.Select(x => x.NameWithQuality));

            int sel = XleCore.SubMenu("Pick Weapon", state.Player.Weapons.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Weapons[sel - 1];
        }

        static void CheckJoystick()
        {
            return;
            /*
            HRESULT hr;
            DIJOYSTATE js;					// DInput joystick state 
            int key = 0;

            if (g.pJoystick)
            {
                do
                {
                    // Poll the device to read the current state
                    hr = g.pJoystick->Poll();

                    // Get the input's device state
                    hr = g.pJoystick->GetDeviceState(sizeof(DIJOYSTATE), &js);

                    if (hr == DIERR_INPUTLOST || hr == DIERR_NOTACQUIRED)
                    {
                        // DInput is telling us that the input stream has been
                        // interrupted. We aren't tracking any state between polls, so
                        // we don't have any special reset that needs to be done. We
                        // just re-acquire and try again.
                        hr = g.pJoystick->Acquire();
                        if (FAILED(hr))
                            return;
                    }
                }
                while (DIERR_INPUTLOST == hr);

                if (FAILED(hr))
                    return;

                if (js.lX < -500)
                {
                    key = VK_LEFT;
                }
                else if (js.lX > 500)
                {
                    key = VK_RIGHT;
                }
                else if (js.lY < -500)
                {
                    key = VK_UP;
                }
                else if (js.lY > 500)
                {
                    key = VK_DOWN;
                }
                else if (js.rgbButtons[0] & 0x80 && buttonHeld == 0)
                {
                    key = VK_RETURN;
                }

                if (g.quickMenu == true && key > 0 && buttonHeld == 0)
                {
                    if (lastMove + 200 < clock())
                    {
                        lastMove = clock();
                        g.menuKey = key;
                    }
                }
                else if (key > 0 && (key != VK_RETURN || g.commandMode != 10))
                {
                    lastMove = clock();
                    g.menuKey = key;
                }


                if (js.rgbButtons[0] & 0x80 && buttonHeld == 0 && g.commandMode == 10)
                {
                    buttonHeld = 1;
                    buttonTime = clock();
                    OutputDebugstring("Held button.\n");

                    //g.commandMode = (CmdMode)10;

                }
                else if (js.rgbButtons[0] & 0x80 && g.commandMode == 20 && buttonHeld == 0)
                {

                    g.commandMode = cmdEnterCommand;
                    buttonHeld = 2;

                    g.menuKey = VK_RETURN;

                    g.LeftMenuActive = false;
                    lastMove = clock();

                }
                else if (js.rgbButtons[0] & 0x80 && buttonHeld == 1)
                {
                    if (g.quickMenu == true)
                    {
                        g.menuKey = VK_RETURN;
                        buttonHeld = 2;
                    }
                    else if (clock() - buttonTime > 700 && g.commandMode == cmdEnterCommand)
                    {
                        // TODO:  wtf???
                        g.commandMode = (CmdMode)20;
                        buttonHeld = 2;

                        g.LeftMenuActive = true;
                        lastMove = clock();

                    }
                }
                else if (js.rgbButtons[0] & 0x80)
                {
                    buttonHeld = 2;

                }
                else if (buttonHeld == 2 && !(js.rgbButtons[0] & 0x80))
                {
                    buttonHeld = 0;
                }
                else if (buttonHeld == 1)
                {
                    if (g.commandMode == (CmdMode)20)
                        g.commandMode = cmdEnterCommand;

                    buttonHeld = 2;

                    g.menuKey = VK_RETURN;
                }

            }
            */
        }

        /// <summary>
        /// Asks the user to choose a number.
        /// </summary>
        /// <param name="max">The maximum value the user is allowed to select.</param>
        /// <returns></returns>
        [Obsolete("Use NumberPicker service instead")]
        public static int ChooseNumber(int max)
        {
            return ChooseNumber(Redraw, max);
        }
        [Obsolete("Use NumberPicker service instead")]
        public static int ChooseNumber(Action redraw, int max)
        {
            int method = 0;
            int amount = 0;

            XleCore.TextArea.PrintLine();

            XleCore.TextArea.Print("Enter number by ", XleColor.White);
            XleCore.TextArea.Print("keyboard", XleColor.Yellow);
            XleCore.TextArea.Print(" or ", XleColor.White);
            XleCore.TextArea.Print("joystick", XleColor.Cyan);

            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine();

            KeyCode key;


            do
            {
                XleCore.PromptToContinueOnWait = false;

                key = WaitForKey(redraw);

                if (method == 0)
                {
                    switch (key)
                    {
                        case KeyCode.None:
                            break;

                        case KeyCode.Right:
                        case KeyCode.Up:
                        case KeyCode.Left:
                        case KeyCode.Down:

                            XleCore.TextArea.PrintLine("Use joystick - press button when done");
                            XleCore.TextArea.PrintLine();
                            XleCore.TextArea.PrintLine("  Horizontal - Slow change", XleColor.Cyan);
                            XleCore.TextArea.PrintLine("  Vertical   - Fast change", XleColor.Cyan);
                            XleCore.TextArea.PrintLine("                          - 0 -");

                            method = 2;

                            break;
                        default:
                            XleCore.TextArea.PrintLine("Keyboard entry-press return when done", XleColor.Yellow);
                            XleCore.TextArea.PrintLine();
                            XleCore.TextArea.PrintLine();
                            XleCore.TextArea.PrintLine("                          - 0 -");
                            method = 1;

                            break;
                    }

                }

                if (method == 1)
                {
                    if (key >= KeyCode.D0 && key <= KeyCode.D9)
                        amount = 10 * amount + key - KeyCode.D0;

                    if (key == KeyCode.BackSpace)
                        amount /= 10;

                    if (amount > max)
                        amount = max;

                    if (amount < 0)
                        amount = 0;

                    XleCore.TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }
                else if (method == 2)
                {
                    switch (key)
                    {
                        case KeyCode.Right:
                            amount++;
                            break;
                        case KeyCode.Up:
                            amount += 20;
                            break;
                        case KeyCode.Left:
                            amount--;
                            break;
                        case KeyCode.Down:
                            amount -= 20;
                            break;
                    }

                    if (amount > max)
                        amount = max;

                    if (amount < 0)
                        amount = 0;

                    XleCore.TextArea.RewriteLine(4, "                          - " + amount.ToString() + " -");
                }


            } while (key != KeyCode.Return);

            XleCore.PromptToContinueOnWait = true;
            XleCore.TextArea.PrintLine();

            return amount;
        }

        [Obsolete("Use XleRenderer.LoadTiles instead")]
        public static void LoadTiles(string tileset)
        {
            Renderer.LoadTiles(tileset);
        }

        public static void ChangeMap(Player player, int mMapID, int targetEntryPoint)
        {
            ChangeMapImpl(player, mMapID, targetEntryPoint, 0, 0);
        }
        public static void ChangeMap(Player player, int mMapID, Point targetPoint)
        {
            ChangeMapImpl(player, mMapID, -1, targetPoint.X, targetPoint.Y);
        }
        static void ChangeMapImpl(Player player, int mMapID, int targetEntryPoint, int targetX, int targetY)
        {
            if (GameState.Map == null)
            {
                player.MapID = mMapID;
                return;
            }

            var saveMap = GameState.Map;
            var saveX = player.X;
            var saveY = player.Y;

            if (GameState.Map is Outside)
            {
                player.SetReturnLocation(player.MapID, player.X, player.Y, Direction.South);
            }

            bool actualChangeMap = saveMap.MapID != mMapID;

            if (mMapID == 0)
                actualChangeMap = false;

            try
            {
                if (actualChangeMap)
                {
                    GameState.Map = LoadMap(mMapID);
                    player.MapID = mMapID;

                    if (GameState.Map.GetType() == saveMap.GetType() &&
                        GameState.Map.Guards != null)
                    {
                        GameState.Map.Guards.IsAngry = saveMap.Guards.IsAngry;
                    }

                    TextArea.Clear();
                }

                if (targetEntryPoint < 0 || targetEntryPoint >= GameState.Map.EntryPoints.Count)
                {
                    player.X = targetX;
                    player.Y = targetY;

                    if (targetEntryPoint >= 0)
                    {
                        TextArea.PrintLine("Failed to find entry point " + targetEntryPoint.ToString(), XleColor.Yellow);
                        TextArea.PrintLine();
                    }
                }
                else
                {
                    if (actualChangeMap)
                        GameState.MapExtender.OnBeforeEntry(GameState, ref targetEntryPoint);

                    var ep = GameState.Map.EntryPoints[targetEntryPoint];

                    player.X = ep.Location.X;
                    player.Y = ep.Location.Y;
                    player.DungeonLevel = ep.DungeonLevel;

                    if (ep.Facing != Direction.None)
                    {
                        player.FaceDirection = ep.Facing;
                    }
                }

                SetTilesAndCommands();

                if (actualChangeMap)
                {
                    GameState.Map.OnLoad(player);
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
                System.Diagnostics.Debug.Print(e.StackTrace);

                player.MapID = saveMap.MapID;
                GameState.Map = saveMap;
                player.X = saveX;
                player.Y = saveY;

                throw;
            }

            if (actualChangeMap)
                GameState.MapExtender.OnAfterEntry(GameState);
        }

        public static bool EnableDebugMode
        {
            get { return Options.EnableDebugMode; }
            set { Options.EnableDebugMode = value; }
        }

        public static void PlayerIsDead()
        {
            Factory.PlayerIsDead(GameState);
        }
    }
}