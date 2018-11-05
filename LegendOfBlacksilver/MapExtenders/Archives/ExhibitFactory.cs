using AgateLib;
using AgateLib.Foundation;
using Xle.Blacksilver.MapExtenders.Archives.Exhibits;

namespace Xle.Blacksilver.MapExtenders.Archives
{
    public interface IExhibitFactory
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

    [Singleton]
    public class ExhibitFactory : IExhibitFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public ExhibitFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public Blacksmith Blacksmith() => serviceLocator.Resolve<Blacksmith>();
        public CrystalTears CrystalTears() => serviceLocator.Resolve<CrystalTears>();
        public DarkWand DarkWand() => serviceLocator.Resolve<DarkWand>();
        public FlaxtonIncense FlaxtonIncense() => serviceLocator.Resolve<FlaxtonIncense>();
        public GameOfHonor GameOfHonor() => serviceLocator.Resolve<GameOfHonor>();
        public IslandRetreat IslandRetreat() => serviceLocator.Resolve<IslandRetreat>();
        public KloryksCage KloryksCage() => serviceLocator.Resolve<KloryksCage>();
        public MagicEtherium MagicEtherium() => serviceLocator.Resolve<MagicEtherium>();
        public MarthbaneTunnels MarthbaneTunnels() => serviceLocator.Resolve<MarthbaneTunnels>();
        public MetalWork MetalWork() => serviceLocator.Resolve<MetalWork>();
        public MorningStar MorningStar() => serviceLocator.Resolve<MorningStar>();
        public Mountains Mountains() => serviceLocator.Resolve<Mountains>();
        public SingingCrystal SingingCrystal() => serviceLocator.Resolve<SingingCrystal>();
        public StormingGear StormingGear() => serviceLocator.Resolve<StormingGear>();
        public TheWealthy TheWealthy() => serviceLocator.Resolve<TheWealthy>();
        public UnderwaterPort UnderwaterPort() => serviceLocator.Resolve<UnderwaterPort>();
        public VaseOfSouls VaseOfSouls() => serviceLocator.Resolve<VaseOfSouls>();
    }
}
