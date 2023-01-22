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


    internal class PingrespPacket : PacketBase
    {
        internal const byte PacketTypeCode = 0x0D;

        internal PingrespPacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Clients should not send pingresp packets");
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                if ((reader.FixedHeaderFirstByte & Packet.PacketFlagsBitMask) != Packet.ZeroedHeaderFlagBits)
                {
                    throw new MqttProtocolException("Pingresp packet received with invalid header flags");
                }
            }

            if (reader.RemainingLength != 0)
            {
                throw new MqttProtocolException("Pingresp packet received with invalid remaining length");
            }
        }
    }
}
