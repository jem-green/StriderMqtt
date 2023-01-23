using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class PingreqPacket : PacketBase
    {
        internal const byte PacketTypeCode = 0x0C;
        internal const byte PingreqFlagBits = 0x00;

        internal PingreqPacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            writer.SetFixedHeader(this.PacketType);
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Pingreq packet should not be received");
        }
    }


    
}
