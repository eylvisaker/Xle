using AgateLib.Display;
using ERY.Xle.Serialization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ERY.Xle.Maps
{
    public class GuardList : IXleSerializable, IList<Guard>
    {
        private double lastGuardAnim = 0;
        private int guardAnimFrame;
        private List<Guard> mGuards = new List<Guard>();

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("GuardDefaultAttack", DefaultAttack);
            info.Write("GuardDefaultDefense", DefaultDefense);
            info.Write("GuardDefaultHP", DefaultHP);
            info.Write("GuardDefaultColor", DefaultColor.ToArgb());
            info.Write("Guards", mGuards);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            mGuards = info.ReadList<Guard>("Guards");
            DefaultAttack = info.ReadInt32("GuardDefaultAttack");
            DefaultColor = ColorX.FromArgb(info.ReadInt32("GuardDefaultColor").ToString("X8"));
            DefaultDefense = info.ReadInt32("GuardDefaultDefense");
            DefaultHP = info.ReadInt32("GuardDefaultHP");

            InitializeGuardData();
        }

        public int DefaultAttack { get; set; }
        public int DefaultDefense { get; set; }
        public int DefaultHP { get; set; }
        public Color DefaultColor { get; set; }

        public bool IsAngry { get; set; }

        public void AnimateGuards()
        {
            int animTime = 460;

            if (IsAngry)
                animTime = 150;

            if (lastGuardAnim + animTime <= Timing.TotalMilliseconds)
            {
                guardAnimFrame++;

                lastGuardAnim = Timing.TotalMilliseconds;
            }
        }

        public bool GuardInSpot(int x, int y)
        {
            foreach (var guard in this)
            {
                if ((guard.X == x - 1 || guard.X == x || guard.X == x + 1) &&
                    (guard.Y == y - 1 || guard.Y == y || guard.Y == y + 1))
                {
                    return true;
                }
            }

            return false;
        }

        public void InitializeGuardData()
        {
            foreach (var guard in this)
            {
                guard.Attack = DefaultAttack;
                guard.Defense = DefaultDefense;
                guard.HP = DefaultHP;
                guard.Color = DefaultColor;
                guard.Facing = Direction.South;

                if (guard.Color.ToArgb() == "00000000")
                    guard.Color = XleColor.Yellow;
            }
        }

        public int IndexOf(Guard item)
        {
            return mGuards.IndexOf(item);
        }

        void IList<Guard>.Insert(int index, Guard item)
        {
            mGuards.Insert(index, item);
        }

        void IList<Guard>.RemoveAt(int index)
        {
            mGuards.RemoveAt(index);
        }

        public Guard this[int index]
        {
            get { return mGuards[index]; }
            set { mGuards[index] = value; }
        }

        public void Add(Guard item)
        {
            mGuards.Add(item);
        }

        public void Clear()
        {
            mGuards.Clear();
        }

        public bool Contains(Guard item)
        {
            return mGuards.Contains(item);
        }

        void ICollection<Guard>.CopyTo(Guard[] array, int arrayIndex)
        {
            mGuards.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mGuards.Count; }
        }

        bool ICollection<Guard>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Guard item)
        {
            return mGuards.Remove(item);
        }

        public IEnumerator<Guard> GetEnumerator()
        {
            return mGuards.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int AnimFrame { get { return guardAnimFrame; } }
    }
}
