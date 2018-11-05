using AgateLib;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xle.Menus;
using Xle.XleEventTypes.Extenders;

namespace Xle.XleEventTypes.Stores.Extenders
{
    [InjectProperties]
    public class StoreExtender : EventExtender
    {
        private bool mRobbed = false;

        public Random Random { get; set; }
        public IMuseumCoinSale MuseumCoinSale { get; set; }

        public bool Robbed
        {
            get { return mRobbed; }
            set { mRobbed = value; }
        }

        /// <summary>
        /// Bool indicating whether or not we are robbing this store.
        /// </summary>
        protected bool robbing;

        public new Store TheEvent { get { return (Store)base.TheEvent; } }

        public virtual bool AllowInteractionWhenLoanOverdue { get { return false; } }

        /// <summary>
        /// Returns true if the loan for the player is over due and stores should not
        /// speak with him and optionally displays a message to the player.  
        /// Returns false if the current map has no lending association
        /// regardless of whether the player has an overdue loan.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="displayMessage">Pass true to display a default message indicating
        /// that the shop keeper will not talk to the player.</param>
        /// <returns>Returns true if the loan for the player is over due and stores should not
        /// speak with him and optionally displays a message to the player.  
        /// Returns false if the current map has no lending association
        /// regardless of whether the player has an overdue loan.</returns>
        public bool IsLoanOverdue()
        {
            if (Map.Events.Any(x => x is Store && x.ExtenderName == "StoreLending") == false)
            {
                return false;
            }

            if (Player.loan > 0 && Player.dueDate - Player.TimeDays <= 0)
            {
                return true;
            }
            else
                return false;
        }

        protected Task StoreSound(LotaSound sound) => GameControl.PlaySoundWait(sound);

        protected async Task Wait(int howLong)
        {
            await GameControl.WaitAsync(howLong);
        }

        protected async Task<Keys> WaitForKey()
        {
            return await GameControl.WaitForKey();
        }



        protected async Task StoreDeclinePlayer()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Sorry.  I can't talk to you.");
            await GameControl.WaitAsync(500);
        }

        protected virtual void DisplayLoanOverdueMessage()
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> Speak()
        {
            if (AllowInteractionWhenLoanOverdue == false)
            {
                if (IsLoanOverdue())
                {
                    await StoreDeclinePlayer();
                    return true;
                }
            }

            return await SpeakImplAsync();
        }

        protected virtual async Task<bool> SpeakImplAsync()
        {
            return await StoreNotImplementedMessage();
        }

        protected async Task<bool> StoreNotImplementedMessage()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine(TheEvent.ShopName, XleColor.Yellow);
            await TextArea.PrintLine("");
            await TextArea.PrintLine("A Sign Says, ");
            await TextArea.PrintLine("Closed for remodelling.");

            SoundMan.PlaySound(LotaSound.Medium);

            await TextArea.PrintLine();

            await GameControl.WaitAsync(1000);

            return true;
        }

        public sealed override async Task<bool> Rob()
        {
            if (AllowRobWhenNotAngry == false && Map.Guards.IsAngry == false)
            {
                await RobFail();
                return true;
            }

            MapExtender.IsAngry = true;

            return await RobCore();
        }

        protected virtual async Task<bool> RobCore()
        {
            if (Robbed)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("No items within reach here.");
                await GameControl.WaitAsync(1000);
                return true;
            }

            int value = RobValue();

            if (value == 0)
            {
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLine("There's nothing to really carry here.");
                await GameControl.WaitAsync(1000);
                return true;
            }

            Player.Gold += value;
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("You get " + value.ToString() + " gold.", XleColor.Yellow);
            await GameControl.WaitAsync(1000);
            Robbed = true;

            return true;
        }

        public virtual int RobValue()
        {
            return 0;
        }

        /// <summary>
        /// Method called when the player attempts to rob and should get the 
        /// message "the merchant won't let you rob."
        /// </summary>
        public virtual async Task RobFail()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("The merchant won't let you rob.");

            await GameControl.WaitAsync(1000);
        }

        /// <summary>
        /// Gets whether or not this type of event allows the player
        /// to rob it when the town isn't angry at him.
        /// </summary>
        public virtual bool AllowRobWhenNotAngry { get { return false; } }

        public virtual async Task CheckOfferMuseumCoin(Player player)
        {
            if (MuseumCoinSale.RollToOfferCoin() && robbing == false)
            {
                await MuseumCoinSale.OfferMuseumCoin();
            }
        }
    }
}
