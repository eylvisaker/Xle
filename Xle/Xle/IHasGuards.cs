using System;
using AgateLib.Geometry;

namespace ERY.Xle
{
    public interface IHasGuards
    {
        int DefaultAttack { get; set; }
        int DefaultDefense { get; set; }
        int DefaultHP { get; set; }
        Color DefaultColor { get; set; }

        void AnimateGuards();
        bool GuardInSpot(int x, int y);
        System.Collections.Generic.List<Guard> Guards { get; }
        bool IsAngry { get; set; }
        void UpdateGuards(Player player);
    }
}
