using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ERY.Xle.Maps
{
    class XleTypeBinder : AgateLib.Serialization.Xle.ITypeBinder
    {
        Dictionary<string, Type> typemap = new Dictionary<string, Type>();
        private AgateLib.Serialization.Xle.ITypeBinder typeBinder;

        XleTypeBinder()
        {
            Assembly ass = Assembly.GetExecutingAssembly();

            MapSpecificStoreToGenericStore("Bank");
            MapSpecificStoreToGenericStore("Blackjack");
            MapSpecificStoreToGenericStore("FlipFlop");
            MapSpecificStoreToGenericStore("Buyback");
            MapSpecificStore("Armor", typeof(StoreEquipment));
            MapSpecificStore("Weapon", typeof(StoreEquipment));
            MapSpecificStoreToGenericStore("ArmorTraining");
            MapSpecificStoreToGenericStore("WeaponTraining");
            MapSpecificStoreToGenericStore("Food");
            MapSpecificStoreToGenericStore("Lending");
            MapSpecificStoreToGenericStore("Jail");
            MapSpecificStoreToGenericStore("Fortune");
            MapSpecificStoreToGenericStore("Magic");
            MapSpecificStoreToGenericStore("Vault");
            MapSpecificStoreToGenericStore("Healer");

            typemap.Add("ERY.Xle.XleEventTypes.LeaveEvent", typeof(Script));
            typemap.Add("ERY.Xle.XleEvent", typeof(XleEvent));
            typemap.Add("ERY.Xle.TileSet", typeof(TileSet));
            typemap.Add("ERY.Xle.Maps.XleMapTypes.Castle", typeof(CastleMap));
        }

        private void MapSpecificStore(string p, Type targetType)
        {
            typemap.Add("ERY.Xle.XleEventTypes.Stores.Store" + p, targetType);
        }
        private void MapSpecificStoreToGenericStore(string p)
        {
            MapSpecificStore(p, typeof(Store));
        }

        public XleTypeBinder(AgateLib.Serialization.Xle.ITypeBinder typeBinder)
            : this()
        {
            this.typeBinder = typeBinder;
        }
        public Type GetType(string typename)
        {
            if (typemap.ContainsKey(typename))
                return typemap[typename];
            else
                return typeBinder.GetType(typename);
        }
    }
}
