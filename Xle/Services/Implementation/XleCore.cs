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
        public const int myWindowWidth = 640;

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

        [Obsolete("Use systemState.Factory instead.")]
        private XleGameFactory mFactory { get { return (XleGameFactory)systemState.Factory; } }

        XleData mData
        {
            get { return systemState.Data; }
            set { systemState.Data = value; }
        }

        [Obsolete("Use ITextArea service instead.")]
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

            TextArea = (TextArea)textArea;
            Options = new XleOptions();

            GameState = gameState;
        }

        private IXleInput input;
        private ICommandList commands;
        private ICommandExecutor commandExecutor;

        public static GameState GameState { get; set; }
        [Obsolete("Use IXleRenderer as a service instead.")]
        public static XleRenderer Renderer { get; set; }
        public static XleOptions Options { get; set; }
        [Obsolete("Use XleData as a service instead.")]
        public static XleData Data { get { return inst.mData; } }

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

            LoadTiles(GameState.Map.TileImage);
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
            KeepAlive();

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
                GameState.MapExtender.OnUpdate(Display.DeltaTime / 1000.0);
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

        /// <summary>
        /// This function is a message-friendly wait that will continue screen
        /// drawing and animation.  It allows the code to pause for a specified
        /// amount of time.	
        /// </summary>
        /// <param name="howLong"></param>
        [Obsolete("Use IXleGameControl.Wait instead.")]
        public static void Wait(int howLong)
        {
            Wait(howLong, false, Redraw);
        }
        [Obsolete("Use IXleGameControl.Wait instead.")]
        public static void Wait(int howLong, Action redraw)
        {
            Wait(howLong, false, redraw);
        }
        [Obsolete("Use IXleGameControl.Wait instead.")]
        public static void Wait(int howLong, bool keyBreak, Action redraw)
        {
            IStopwatch watch = Timing.CreateStopWatch();

            do
            {
                Renderer.UpdateAnim();

                redraw();
                KeepAlive();

                if (keyBreak && Keyboard.AnyKeyPressed)
                    break;

            } while (watch.TotalMilliseconds < howLong);
        }

        [Obsolete("Use XleGameControl as a service instead.")]
        public static void KeepAlive()
        {
            if (GameState.Map != null)
            {
                GameState.MapExtender.CheckSounds(GameState);
            }

            Core.KeepAlive();

            if (Display.CurrentWindow.IsClosed)
                throw new MainWindowClosedException();
        }

        public static bool PromptToContinueOnWait { get; set; }

        public static bool PromptToContinue { get; set; }

        [Obsolete("Use XleRenderer.LoadTiles instead")]
        public static void LoadTiles(string tileset)
        {
            Renderer.LoadTiles(tileset);
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
                    GameState.MapExtender.OnLoad(GameState);
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

    }
}