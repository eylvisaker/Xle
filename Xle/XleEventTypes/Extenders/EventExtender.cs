using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Game;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.XleEventTypes.Extenders
{
    public class EventExtender
    {
        public EventExtender()
        {
            Enabled = true;
        }

        public XleEvent TheEvent { get; set; }

        public ITextArea TextArea { get; set; }
        public GameState GameState { get; set; }
        public IXleGameControl GameControl { get; set; }
        public ISoundMan SoundMan { get; set; }

        protected Player Player { get { return GameState.Player; } }
        protected XleMap Map { get { return GameState.Map; } }
        protected MapExtender MapExtender { get { return GameState.MapExtender; } }

        /// <summary>
        /// Function called when player speaks in a square inside or next
        /// to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool Speak()
        {
            return false;
        }

        /// <summary>
        /// Function called when player executes Rob in a square inside or next
        /// to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <returns></returns>
        public virtual bool Rob()
        {
            return false;
        }

        /// <summary>
        /// Function called when the player executes the Open command inside
        /// or next to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool Open()
        {
            return false;
        }

        /// <summary>
        /// Function called when the player executes the Take command inside
        /// or next to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool Take()
        {
            return false;
        }

        /// <summary>
        /// Function called when the player walks inside
        /// the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool StepOn()
        {
            return false;
        }

        /// <summary>
        /// Function called when the player tries to walk inside
        /// the XleEvent.
        /// 
        /// This is before the position is updated.  Returns false to 
        /// block the player from stepping there, and true if the
        /// player can walk there.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="allowStep"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual void TryToStepOn(int dx, int dy, out bool allowStep)
        {
            allowStep = true;
        }

        /// <summary>
        /// Function called when the player uses an item
        /// or next to the XleEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool Use(int item)
        {
            return false;
        }

        /// <summary>
        /// Function called when the player eXamines next
        /// to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool Xamine()
        {
            return false;
        }

        public virtual void BeforeStepOn()
        {

        }

        public virtual void OnLoad()
        { }

        public virtual void OnUpdate(double deltaTime)
        {
        }

        public virtual bool Enabled { get; set; }

        public Rectangle Rectangle
        {
            get { return TheEvent.Rectangle; }
            set { TheEvent.Rectangle = value; }
        }

    }
}
