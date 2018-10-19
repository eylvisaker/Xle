using Xle.Ancients.MapExtenders.Castle.Events;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Castle
{
    public class CastleUpper : CastleGround
    {
        public CastleUpper()
        {
            CastleLevel = 2;
        }

        public override void OnAfterEntry()
        {
            if (Story.Invisible == false)
            {
                TextArea.PrintLine("Private level!");

                IsAngry = true;
            }
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            return TheMap.OutsideTile;
        }
    }
}
