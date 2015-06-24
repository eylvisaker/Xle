using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Data;

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

        public abstract EquipmentInfo Info(XleData data);

        public string BaseName(XleData data) { return Info(data).Name; }

        public string NameWithQuality(XleData data) { return QualityName(data) + " " + BaseName(data); }
        public string QualityName(XleData data) { return data.QualityList[Quality]; }

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

        public int Price(XleData data) { return Info(data).Prices[quality]; } 
    }

    public class WeaponItem : Equipment
    {
        public override EquipmentInfo Info(XleData data)
        {
            return data.WeaponList[ID]; 
        }
    }

    public class ArmorItem : Equipment
    {
        public override EquipmentInfo Info(XleData data)
        {
            return data.ArmorList[ID]; 
        }
    }
}
