using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class CastleEvent : LobEvent
    {
        protected DurekCastle DurekCastleObject { get { return MapExtender as DurekCastle; } }
    }
}
