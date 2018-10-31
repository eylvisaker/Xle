using AgateLib;
using Xle;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xle.Serialization
{
    public interface IGamePersistance
    {
        IEnumerable<string> FindExistingGames();

        bool GameExists(string name);

        Player LoadPlayer(string file);

        void Delete(string selectedFile);

        void Save(Player player);
    }

    [Singleton]
    public class GamePersistance : IGamePersistance
    {
        private string savedDirectory = "Saved";

        public GamePersistance()
        {
            Directory.CreateDirectory(savedDirectory);
        }

        public void Delete(string name)
        {
            File.Delete($"{savedDirectory}/{name}.chr");
        }

        public IEnumerable<string> FindExistingGames()
        {
            return Directory.GetFiles(savedDirectory)
                .Select(x => Path.GetFileNameWithoutExtension(x));
        }

        public bool GameExists(string name)
        {
            return File.Exists($"{savedDirectory}/{name}.chr");
        }

        public Player LoadPlayer(string name)
        {
            return Player.LoadPlayer($"{savedDirectory}/{name}.chr");
        }

        public void Save(Player player)
        {
            player.SavePlayer($"{savedDirectory}/{player.Name}.chr");
        }
    }
}
