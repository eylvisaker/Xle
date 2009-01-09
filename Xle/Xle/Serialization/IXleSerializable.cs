using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.Xle.Serialization
{
    public interface IXleSerializable
    {
        void WriteData(XleSerializationInfo info);
        void ReadData(XleSerializationInfo info);
    }
}
