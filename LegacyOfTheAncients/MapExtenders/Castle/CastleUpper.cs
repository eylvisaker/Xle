using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
    public class CastleUpper : CastleGround
    {
        public CastleUpper()
        {
            CastleLevel = 2;
        }

        public override void OnAfterEntry(GameState state)
        {
            if (Lota.Story.Invisible == false)
            {
                XleCore.TextArea.PrintLine("Private level!");

                IsAngry = true;
            }
        }

        public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
        {
            return TheMap.OutsideTile;
        }
    }
}
