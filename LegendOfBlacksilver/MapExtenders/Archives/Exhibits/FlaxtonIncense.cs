﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.XleMapTypes.MuseumDisplays;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class FlaxtonIncense : LobExhibit
    {
        public FlaxtonIncense()
            : base("Flaxton Incense", Coin.WhiteDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.FlaxtonIncense; }
        }

        public override bool IsClosed
        {
            get { return Story.EatenFlaxton; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to partake?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("It's sour but doesn't taste that bad.");

                Story.EatenFlaxton = true;
            }
            else
                ReturnGem();
        }
    }
}
