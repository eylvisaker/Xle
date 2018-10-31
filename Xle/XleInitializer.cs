using AgateLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xle.Data;

namespace Xle
{
    public interface IInitializer
    {
        void Initialize();
    }

    [Singleton]
    public class LotaInitializer : IInitializer
    {
        private readonly IXleDataLoader dataLoader;

        public LotaInitializer(IXleDataLoader dataLoader)
        {
            this.dataLoader = dataLoader;
        }

        public void Initialize()
        {
            dataLoader.LoadGameFile("Game.xml");
            dataLoader.LoadDungeonMonsterSurfaces();
        }
    }
}
