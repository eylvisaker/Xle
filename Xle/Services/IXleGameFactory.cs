using AgateLib.DisplayLib;

namespace ERY.Xle.Services
{
    public interface IXleGameFactory : IXleService
    {
        void LoadSurfaces();
        FontSurface Font { get; }

        void SetGameSpeed(GameState GameState, int p);

        void CheatLevel(Player player, int level);

        void PlayerIsDead(GameState gameState);
    }
}
