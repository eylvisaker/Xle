using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Data;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle
{
    public abstract class Equipment : IXleSerializable
    {
        int id;
        int quality;

        public virtual int ID { get { return id; } set { id = value; } }
        public int Quality
        {
            get { return quality; }
            set
            {
                if (value < 0 || value > 4)
                    throw new ArgumentOutOfRangeException();
                quality = value;
            }
        }

        public abstract EquipmentInfo Info { get; }

        public string BaseName { get { return Info.Name; } }

        public string NameWithQuality { get { return QualityName + " " + BaseName; } }
        public string QualityName { get { return XleCore.Data.QualityList[Quality]; } }

        public static int Sorter(Equipment a, Equipment b)
        {
            if (a.id == b.id)
                return a.quality.CompareTo(b.quality);
            else
                return a.id.CompareTo(b.id);
        }

        public void WriteData(XleSerializationInfo info)
        {
            info.Write("ID", ID);
            info.Write("Quality", Quality);
        }
        public void ReadData(XleSerializationInfo info)
        {
            ID = info.ReadInt32("ID");
            Quality = info.ReadInt32("Quality");
        }

        public int Price { get { return Info.Prices[quality]; } }
    }

    public class WeaponItem : Equipment
    {
        public override EquipmentInfo Info
        {
            get { return XleCore.Data.WeaponList[ID]; }
        }
    }

    public class ArmorItem : Equipment
    {
        public override EquipmentInfo Info
        {
            get { return XleCore.Data.ArmorList[ID]; }
        }
    }
}
