using AgateLib.Display;
using Microsoft.Xna.Framework.Graphics;

namespace Xle.Services.Game
{
    public interface IXleGameFactory : IXleService
    {
        void LoadSurfaces();
        Font Font { get; }
        Texture2D Monsters { get; }
        Texture2D Character { get; }

        void SetGameSpeed(GameState GameState, int p);

        void CheatLevel(Player player, int level);

        int MailItemID { get; }
        int ClimbingGearItemID { get; }
        int HealingItemID { get; }
    }
}
