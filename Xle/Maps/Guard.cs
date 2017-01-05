using AgateLib.Geometry;
using ERY.Xle.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
    public class Guard : IXleSerializable
    {
        public Guard()
        {
            Name = "Guard";
            Facing = Direction.South;
            Color = XleColor.Yellow;
        }

        public Point Location
        {
            get { return new Point(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public int HP { get; set; }
        public Direction Facing { get; set; }
        public Color Color { get; set; }

        public int Attack { get; set; }
        public int Defense { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Method called when attacked by the player.
        /// Return true to cancel further processing of the attack.
        /// </summary>
        public Func<GameState, Guard, bool> OnPlayerAttack;
        public Func<GameState, Guard, bool> OnGuardDead;

        public bool SkipAttacking { get; set; }
        public bool SkipMovement { get; set; }

        #region IXleSerializable Members

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("X", X);
            info.Write("Y", Y);
            info.Write("Color", Color.ToArgb());

            info.Write("HP", HP);
            info.Write("Attack", Attack);
            info.Write("Defense", Defense);
        }

        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            X = info.ReadInt32("X");
            Y = info.ReadInt32("Y");
            Color = Color.FromArgb(info.ReadInt32("Color"));

            HP = info.ReadInt32("HP");
            Attack = info.ReadInt32("Attack");
            Defense = info.ReadInt32("Defense");
        }

        #endregion

        public string Name { get; set; }
    }
}
