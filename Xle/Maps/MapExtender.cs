using System;
using System.Collections.Generic;
using System.Linq;

using AgateLib.Mathematics.Geometry;
using AgateLib.InputLib;
using AgateLib.Quality;

using ERY.Xle.Data;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.MapLoad;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.Maps
{
    public class MapExtender : IMapExtender
    {
        XleMap mTheMap;
        List<EventExtender> mEvents = new List<EventExtender>();
        XleMapRenderer mapRenderer;

        public IXleSubMenu SubMenu { get; set; }

        public XleMap TheMap
        {
            get { return mTheMap; }
            set
            {
                Condition.RequireArgumentNotNull(value, "value");
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
        public IReadOnlyList<EventExtender> Events { get { return mEvents; } }

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

        public virtual void OnAfterEntry()
        { }

        public virtual void AfterPlayerStep()
        {
            bool didEvent = false;

            foreach (var evt in EventsAt(Player.X, Player.Y, 0))
            {
                evt.StepOn();
                didEvent = true;
            }

            AfterStepImpl(didEvent);
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

        public virtual void AfterExecuteCommand(KeyCode cmd)
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

        public void LeaveMap()
        {
            TextArea.PrintLine();
            TextArea.PrintLine("Leave " + TheMap.MapName);
            TextArea.PrintLine();

            GameControl.Wait(GameState.GameSpeed.LeaveMapTime);

            MapChanger.ReturnToPreviousMap();

            TextArea.PrintLine();
        }

        /// <summary>
        /// Executes the movement of the player in a certain direction.
        /// Assumes validation has already been performed. Call CanPlayerStep
        /// first to check to see if the movement is valid.
        /// </summary>
        /// <param name="stepDirection"></param>
        public virtual void MovePlayer(Point stepDirection)
        {
            Point newPoint = new Point(Player.X + stepDirection.X, Player.Y + stepDirection.Y);

            BeforeStepOn(newPoint.X, newPoint.Y);

            Player.Location = newPoint;

            AfterPlayerStep();
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
        protected virtual void AfterStepImpl(bool didEvent)
        {

        }

        public bool CanPlayerStep(Point stepDirection)
        {
            return CanPlayerStep(stepDirection.X, stepDirection.Y);
        }

        protected virtual bool CanPlayerStep(int dx, int dy)
        {
            EventExtender evt = GetEvent(Player.X + dx, Player.Y + dy, 0);

            if (evt != null)
            {
                bool allowStep;

                evt.TryToStepOn(dx, dy, out allowStep);

                if (allowStep == false)
                    return false;
            }

            return CanPlayerStepIntoImpl(Player.X + dx, Player.Y + dy);
        }

        public virtual void PlayerCursorMovement(Direction dir)
        {

        }

        public virtual void CheckSounds()
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

        public void OnUpdate(double deltaTime)
        {
            foreach (var evt in Events)
            {
                evt.OnUpdate(deltaTime);
            }
        }

        public IEnumerable<EventExtender> EventsAt(int border)
        {
            int px = Player.X;
            int py = Player.Y;

            return EventsAt(px, py, border);
        }
        public IEnumerable<EventExtender> EventsAt(int px, int py, int border)
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
        public EventExtender GetEvent(int x, int y, int border)
        {
            for (int i = 0; i < mEvents.Count; i++)
            {
                EventExtender e = mEvents[i];

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
