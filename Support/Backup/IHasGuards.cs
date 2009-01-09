using System;
namespace ERY.Xle
{
    public interface IHasGuards
    {
        void AnimateGuards();
        bool GuardInSpot(int x, int y);
        System.Collections.Generic.List<Guard> Guards { get; }
        bool IsAngry { get; set; }
        void UpdateGuards(Player player);
    }
}
