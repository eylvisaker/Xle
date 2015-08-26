using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Maps.Dungeons
{
    public enum DungeonTile
    {
        Wall = 0,
        Empty = 0x10,
        CeilingHole = 0x11,
        FloorHole = 0x12,
        PoisonGasVent = 0x13,
        SlimeSplotch = 0x14,
        TripWire = 0x15,
        GasVent = 0x16,
        Urn = 0x1d,
        Box = 0x1e,

        HiddenCeilingHole = 0x21,
        HiddenFloorHole = 0x22,
        HiddenPoisonGasVent = 0x23,
        HiddenSlimeSplotch = 0x24,
        HiddenTripWire = 0x25,
        HiddenGasVent = 0x26,

        Chest = 0x30,
    }
}
