using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal abstract class IdentifiedPacket : PacketBase
    {
        /// <summary>
        /// Packet identifier
        /// </summary>
        internal ushort PacketId { get; set; }
    }
}
