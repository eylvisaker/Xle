using Xle.Maps.XleMapTypes;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xle.Maps
{
    class XleTypeBinder : Xle.Serialization.ITypeBinder
    {
        Dictionary<string, Type> typemap = new Dictionary<string, Type>();
        private Xle.Serialization.ITypeBinder typeBinder;

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

            typemap.Add("Xle.XleEventTypes.LeaveEvent", typeof(Script));
            typemap.Add("Xle.XleEvent", typeof(XleEvent));
            typemap.Add("Xle.TileSet", typeof(TileSet));
            typemap.Add("Xle.Maps.XleMapTypes.Castle", typeof(CastleMap));
        }

        private void MapSpecificStore(string p, Type targetType)
        {
            typemap.Add("Xle.XleEventTypes.Stores.Store" + p, targetType);
        }
        private void MapSpecificStoreToGenericStore(string p)
        {
            MapSpecificStore(p, typeof(Store));
        }

        public XleTypeBinder(Xle.Serialization.ITypeBinder typeBinder)
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
