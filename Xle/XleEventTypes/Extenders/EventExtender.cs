using AgateLib;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.Game;
using Xle.ScreenModel;
using Xle.XleSystem;

namespace Xle.XleEventTypes.Extenders
{
    public interface IEventExtender
    {
        bool Enabled { get; }

        Rectangle Rectangle { get; }

        void OnLoad();
        void OnUpdate(GameTime time);

        Task BeforeStepOn();
        Task<bool> TryToStepOn(int dx, int dy);

        Task<bool> StepOn();
        Task<bool> Open();
        Task<bool> Rob();
        Task<bool> Speak();
        Task<bool> Take();
        Task<bool> Use(int item);
    }

    [InjectProperties]
    public class EventExtender : IEventExtender
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

        public virtual bool Enabled { get; set; }

        public Rectangle Rectangle
        {
            get { return TheEvent.Rectangle; }
            set { TheEvent.Rectangle = value; }
        }

        protected Player Player { get { return GameState.Player; } }
        protected XleMap Map { get { return GameState.Map; } }
        protected IMapExtender MapExtender { get { return GameState.MapExtender; } }

        /// <summary>
        /// Function called when player speaks in a square inside or next
        /// to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task<bool> Speak()
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Function called when player executes Rob in a square inside or next
        /// to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <returns></returns>
        public virtual Task<bool> Rob()
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Function called when the player executes the Open command inside
        /// or next to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task<bool> Open()
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Function called when the player executes the Take command inside
        /// or next to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task<bool> Take()
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Function called when the player walks inside
        /// the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task<bool> StepOn()
        {
            return Task.FromResult(false);
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
        public virtual Task<bool> TryToStepOn(int dx, int dy)
        {
            return Task.FromResult(true);
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
        public virtual Task<bool> Use(int item)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Function called when the player eXamines next
        /// to the LotaEvent.
        /// 
        /// Returns true if handled by the event.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task<bool> Xamine()
        {
            return Task.FromResult(false);
        }

        public virtual Task BeforeStepOn()
        {
            return Task.CompletedTask;
        }

        public virtual void OnLoad()
        { }

        public virtual void OnUpdate(GameTime time)
        {
        }
    }
}
