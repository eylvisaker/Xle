using AgateLib.DisplayLib;

namespace ERY.Xle.Services
{
    public interface IXleGameFactory : IXleService
    {
        void LoadSurfaces();
        FontSurface Font { get; }
        Surface Monsters { get; }
        Surface Character { get; }

        void SetGameSpeed(GameState GameState, int p);

        void CheatLevel(Player player, int level);

        int MailItemID { get; }
        int ClimbingGearItemID { get; }
        int HealingItemID { get; }
    }
}
