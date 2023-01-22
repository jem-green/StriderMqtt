using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class PubcompPacket : IdentifiedPacket
    {
        internal const byte PacketTypeCode = 0x07;

        internal PubcompPacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            writer.SetFixedHeader(PacketType);
            writer.AppendIntegerField(PacketId);
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                if ((reader.FixedHeaderFirstByte & Packet.PacketFlagsBitMask) != Packet.ZeroedHeaderFlagBits)
                {
                    throw new MqttProtocolException("Pubcomp packet received with invalid header flags");
                }
            }

            if (reader.RemainingLength != 2)
            {
                throw new MqttProtocolException("Remaining length of the incoming pubcomp packet is invalid");
            }

            this.PacketId = reader.ReadIntegerField();
        }
    }
}
