﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using Microsoft.Xna.Framework;

namespace Xle.Serialization.TypeSerializers
{
    internal class RectangleSerializer : XleTypeSerializerBase<Rectangle>
    {
        public override void Serialize(XleSerializationInfo info, Rectangle value)
        {
            info.Write("X", value.X, true);
            info.Write("Y", value.Y, true);
            info.Write("Width", value.Width, true);
            info.Write("Height", value.Height, true);
        }

        public override Rectangle Deserialize(XleSerializationInfo info)
        {
            return new Rectangle(
                info.ReadInt32("X"),
                info.ReadInt32("Y"),
                info.ReadInt32("Width"),
            info.ReadInt32("Height"));
        }
    }
}
