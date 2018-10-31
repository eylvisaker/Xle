using Xle.Blacksilver.MapExtenders.Archives.Exhibits;
using Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Archives
{
    public interface IExhibitFactory : IXleFactory
    {
        MetalWork MetalWork();
        SingingCrystal SingingCrystal();
        IslandRetreat IslandRetreat();

        GameOfHonor GameOfHonor();
        StormingGear StormingGear();
        TheWealthy TheWealthy();

        Mountains Mountains();
        MagicEtherium MagicEtherium();
        VaseOfSouls VaseOfSouls();


        MorningStar MorningStar();
        MarthbaneTunnels MarthbaneTunnels();

        UnderwaterPort UnderwaterPort();
        DarkWand DarkWand();

        Blacksmith Blacksmith();
        FlaxtonIncense FlaxtonIncense();
        KloryksCage KloryksCage();

        CrystalTears CrystalTears();
    }
}
