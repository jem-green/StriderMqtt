using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class PubackPacket : IdentifiedPacket
    {
        internal const byte PacketTypeCode = 0x04;

        internal PubackPacket()
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
                    throw new MqttProtocolException("Puback packet received with invalid header flags");
                }
            }

            if (reader.RemainingLength != 2)
            {
                throw new MqttProtocolException("Remaining length of the incoming puback packet is invalid");
            }

            this.PacketId = reader.ReadIntegerField();
        }
    }
}
