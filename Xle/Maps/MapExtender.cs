using AgateLib;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Services.Commands;
using Xle.Services.Game;
using Xle.Services.MapLoad;
using Xle.Services.Menus;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using Xle.XleEventTypes.Extenders;

namespace Xle.Maps
{
    public interface IMapExtender
    {
        XleMap TheMap { get; set; }
        int WaitTimeAfterStep { get; }
        XleMapRenderer MapRenderer { get; }
        IReadOnlyList<IEventExtender> Events { get; }

        int MapID { get; }
        bool IsAngry { get; set; }

        IEnumerable<IEventExtender> EventsAt(int v);
        void SetCommands(ICommandList commands);
        Task PlayerCursorMovement(Direction dir);
        void OnUpdate(GameTime time);
        void OnLoad();
        Task OnAfterEntry();
        void ModifyEntryPoint(MapEntryParams entryParams);
        Task LeaveMap();
        void CheckSounds(GameTime time);
        Task AfterExecuteCommand(Keys cmd);
        bool CanPlayerStepInto(Point corridorPt);
        bool CanPlayerStepIntoImpl(int v, int targetY);
    }

    [InjectProperties]
    public class MapExtender : IMapExtender
    {
        private XleMap mTheMap;
        private List<IEventExtender> mEvents = new List<IEventExtender>();
        private XleMapRenderer mapRenderer;

        public IXleSubMenu SubMenu { get; set; }

        public XleMap TheMap
        {
            get { return mTheMap; }
            set
            {
                Require.ArgumentNotNull(value, "value");
                mTheMap = value;
            }
        }

        public XleMapRenderer MapRenderer
        {
            get { return mapRenderer; }
            set
            {
                mapRenderer = value;
                OnMapRendererSet();
            }
        }


        public ICommandFactory CommandFactory { get; set; }
        public IXleGameControl GameControl { get; set; }
        public ITextArea TextArea { get; set; }
        public GameState GameState { get; set; }
        public Random Random { get; set; }
        public XleData Data { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleInput Input { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public IMapChanger MapChanger { get; set; }

        protected Player Player { get { return GameState.Player; } }
        public IReadOnlyList<IEventExtender> Events => mEvents;

        public bool IsAngry
        {
            get { return TheMap.Guards.IsAngry; }
            set
            {
                TheMap.Guards.IsAngry = value;

                OnSetAngry(value);
            }
        }

        protected virtual void OnMapRendererSet()
        {
        }

        protected virtual void OnSetAngry(bool value)
        {
        }

        public virtual XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return new XleMapRenderer();
        }

        public virtual int GetOutsideTile(Point playerPoint, int x, int y)
        {
            return TheMap.OutsideTile;
        }

        public virtual void OnLoad()
        {
            SetColorScheme(TheMap.ColorScheme);

            foreach (var evt in Events)
            {
                evt.OnLoad();
            }
        }

        public virtual async Task OnAfterEntry()
        { }

        public virtual async Task AfterPlayerStep()
        {
            bool didEvent = false;

            foreach (var evt in EventsAt(Player.X, Player.Y, 0))
            {
                await evt.StepOn();
                didEvent = true;
            }

            await AfterStepImpl(didEvent);
        }

        public virtual void SetColorScheme(ColorScheme scheme)
        {
            throw new NotImplementedException();
        }

        public virtual int StepSize
        {
            get { return 1; }
        }

        public virtual void ModifyEntryPoint(MapEntryParams entryParams)
        {
        }

        public virtual async Task AfterExecuteCommand(Keys cmd)
        {
        }

        public virtual void SetCommands(ICommandList commands)
        {

        }

        public virtual double ChanceToHitPlayer(Guard guard)
        {
            return (Player.Attribute[Attributes.dexterity] / 80.0);
        }

        public virtual int RollDamageToPlayer(Guard guard)
        {
            int armorType = Player.CurrentArmor.ID;

            double damage = guard.Attack / 99.0 *
                               (120 + Random.NextDouble() * 250) /
                               Math.Pow(armorType + 3, 0.8) /
                                   Math.Pow(Player.Attribute[Attributes.endurance], 0.8) + 3;

            return (int)Math.Round(damage);
        }

        public virtual void CastSpell(MagicSpell magic)
        {
        }

        public virtual bool RollSpellFizzle(MagicSpell magic)
        {
            return Random.Next(10) < 5;
        }

        public virtual int RollSpellDamage(MagicSpell magic, int distance)
        {
            return (int)((magic.ID + 0.5) * 15 * (Random.NextDouble() + 1));
        }
        public virtual bool CanPlayerStepInto(Point pt)
        {
            return CanPlayerStepIntoImpl(pt.X, pt.Y);
        }
        public virtual bool CanPlayerStepIntoImpl(int xx, int yy)
        {
            return true;
        }

        public async Task LeaveMap()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine("Leave " + TheMap.MapName);
            await TextArea.PrintLine();

            await GameControl.WaitAsync(GameState.GameSpeed.LeaveMapTime);

            MapChanger.ReturnToPreviousMap();

            await TextArea.PrintLine();
        }

        /// <summary>
        /// Executes the movement of the player in a certain direction.
        /// Assumes validation has already been performed. Call CanPlayerStep
        /// first to check to see if the movement is valid.
        /// </summary>
        /// <param name="stepDirection"></param>
        public virtual async Task MovePlayer(Point stepDirection)
        {
            Point newPoint = new Point(Player.X + stepDirection.X, Player.Y + stepDirection.Y);

            BeforeStepOn(newPoint.X, newPoint.Y);

            Player.Location = newPoint;

            await AfterPlayerStep();
        }

        public void BeforeStepOn(int x, int y)
        {
            foreach (var evt in EventsAt(x, y, 0))
            {
                evt.BeforeStepOn();
            }
        }

        /// <summary>
        /// Called after the player steps.
        /// </summary>
        /// <param name="didEvent">True if there was an event that occured at this location</param>
        /// <param name="player"></param>
        protected virtual async Task AfterStepImpl(bool didEvent)
        {

        }

        public async Task<bool> CanPlayerStep(Point stepDirection)
        {
            return await CanPlayerStep(stepDirection.X, stepDirection.Y);
        }

        protected virtual async Task<bool> CanPlayerStep(int dx, int dy)
        {
            IEventExtender evt = GetEvent(Player.X + dx, Player.Y + dy, 0);

            if (evt != null)
            {
                bool allowStep = await evt.TryToStepOn(dx, dy);

                if (allowStep == false)
                    return false;
            }

            return CanPlayerStepIntoImpl(Player.X + dx, Player.Y + dy);
        }

        public virtual async Task PlayerCursorMovement(Direction dir)
        {

        }

        public virtual void CheckSounds(GameTime time)
        {
        }

        public void CreateEventExtenders(IEventExtenderFactory eventFactory)
        {
            mEvents.Clear();

            foreach (var evt in TheMap.Events)
            {
                mEvents.Add(eventFactory.Create(this, evt, typeof(EventExtender)));
            }
        }

        public void OnUpdate(GameTime time)
        {
            foreach (var evt in Events)
            {
                evt.OnUpdate(time);
            }
        }

        public IEnumerable<IEventExtender> EventsAt(int border)
        {
            int px = Player.X;
            int py = Player.Y;

            return EventsAt(px, py, border);
        }
        public IEnumerable<IEventExtender> EventsAt(int px, int py, int border)
        {
            foreach (var e in mEvents)
            {
                bool found = false;

                if (e.Enabled == false)
                    continue;

                var rectangle = e.Rectangle;

                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int x = px + i;
                        int y = py + j;

                        if (x >= rectangle.X - border && y >= rectangle.Y - border &&
                            x < rectangle.Right + border && y < rectangle.Bottom + border)
                        {
                            found = true;

                        }
                    }
                }

                if (found)
                    yield return e;
            }
        }

        /// <summary>
        /// returns the special event at the specified location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IEventExtender GetEvent(int x, int y, int border)
        {
            for (int i = 0; i < mEvents.Count; i++)
            {
                IEventExtender e = mEvents[i];

                if (x >= e.Rectangle.X - border && y >= e.Rectangle.Y - border &&
                    x < e.Rectangle.Right + border && y < e.Rectangle.Bottom + border)
                {
                    return e;
                }
            }

            return null;
        }

        public virtual int WaitTimeAfterStep
        {
            get
            {
                return GameState.GameSpeed.GeneralStepTime;
            }
        }

        public int MapID { get { return TheMap.MapID; } }
    }
}
