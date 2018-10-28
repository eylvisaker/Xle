﻿using Xle.Maps;
using Xle.Services;
using Xle.Services.Game;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Ancients.MapExtenders.Fortress.SecondArea
{
    public interface IFortressFinalActivator
    {
        bool CompendiumAttacking { get; set; }

        Guard Warlord { get; }

        event EventHandler WarlordCreated;

        void CreateWarlord();
        void Reset();
    }

    [Singleton]
    public class FortressFinalActivator : IFortressFinalActivator
    {
        public ITextArea TextArea { get; set; }
        public IXleGameControl GameControl { get; set; }
        public ISoundMan SoundMan { get; set; }

        public void Reset()
        {
            Warlord = null;
            CompendiumAttacking = false;
            WarlordCreated = null;
        }

        public Guard Warlord { get; private set; }

        public bool CompendiumAttacking { get; set; }

        public event EventHandler WarlordCreated;

        public void CreateWarlord()
        {
            var warlord = new Guard
            {
                X = 5,
                Y = 45,
                HP = 420,
                Color = XleColor.LightGreen,
                Name = "Warlord",
                OnGuardDead = (state, unused) => WarlordDead(unused),
                SkipAttacking = true,
                SkipMovement = true,
            };

            Warlord = warlord;
            OnWarlordCreated();
        }

        private void OnWarlordCreated()
        {
            if (WarlordCreated != null)
                WarlordCreated(this, EventArgs.Empty);
        }

        private bool WarlordDead(Guard unused)
        {
            this.Warlord = null;

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("        ** warlord killed **");

            for (int i = 0; i < 5; i++)
            {
                SoundMan.PlaySound(LotaSound.Good);
                GameControl.Wait(750);
            }
            GameControl.Wait(1000);

            SoundMan.PlaySoundSync(LotaSound.VeryGood);

            PrintSecurityAlertMessage();

            return true;
        }

        private void PrintSecurityAlertMessage()
        {
        }
    }
}
