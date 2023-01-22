using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class DisconnectPacket : PacketBase
    {
        internal const byte PacketTypeCode = 0x0E;

        internal DisconnectPacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Disconnect packet should not be received");
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            writer.SetFixedHeader(PacketType);
        }
    }
}
