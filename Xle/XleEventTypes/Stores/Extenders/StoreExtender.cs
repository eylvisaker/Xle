using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
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
        protected void StoreDeclinePlayer()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("Sorry.  I can't talk to you.");
            GameControl.Wait(500);
        }
        protected virtual void DisplayLoanOverdueMessage()
        {
            throw new NotImplementedException();
        }

        public override bool Speak(GameState state)
        {
            if (AllowInteractionWhenLoanOverdue == false)
            {
                if (IsLoanOverdue())
                {
                    StoreDeclinePlayer();
                    return true;
                }
            }

            return SpeakImpl(state);
        }

        protected virtual bool SpeakImpl(GameState state)
        {
            return StoreNotImplementedMessage();
        }

        protected bool StoreNotImplementedMessage()
        {
            TextArea.PrintLine();
            TextArea.PrintLine(TheEvent.ShopName, XleColor.Yellow);
            TextArea.PrintLine("");
            TextArea.PrintLine("A Sign Says, ");
            TextArea.PrintLine("Closed for remodelling.");

            SoundMan.PlaySound(LotaSound.Medium);

            TextArea.PrintLine();

            GameControl.Wait(1000);

            return true;
        }

        public sealed override bool Rob(GameState state)
        {
            if (AllowRobWhenNotAngry == false && Map.Guards.IsAngry == false)
            {
                RobFail();
                return true;
            }

            MapExtender.IsAngry = true;

            return RobImpl(state);
        }

        protected virtual bool RobImpl(GameState state)
        {
            if (Robbed)
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("No items within reach here.");
                GameControl.Wait(1000);
                return true;
            }

            int value = RobValue();

            if (value == 0)
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("There's nothing to really carry here.");
                GameControl.Wait(1000);
                return true;
            }

            Player.Gold += value;
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("You get " + value.ToString() + " gold.", XleColor.Yellow);
            GameControl.Wait(1000);
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
        public virtual void RobFail()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("The merchant won't let you rob.");

            GameControl.Wait(1000);
        }

        /// <summary>
        /// Gets whether or not this type of event allows the player
        /// to rob it when the town isn't angry at him.
        /// </summary>
        public virtual bool AllowRobWhenNotAngry { get { return false; } }

        public virtual void CheckOfferMuseumCoin(Player player)
        {
            if (Random.Next(1000) < 45 && robbing == false)
            {
                MuseumCoinSale.OfferMuseumCoin();
            }
        }
    }
}
