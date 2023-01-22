using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class PubrelPacket : IdentifiedPacket
    {
        internal const byte PacketTypeCode = 0x06;

        internal PubrelPacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                writer.SetFixedHeader(PacketType, MqttQos.AtLeastOnce);
            }
            else
            {
                writer.SetFixedHeader(PacketType);
            }

            writer.AppendIntegerField(PacketId);
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                if ((reader.FixedHeaderFirstByte & Packet.PacketFlagsBitMask) != Packet.Qos1HeaderFlagBits)
                {
                    throw new MqttProtocolException("Pubrel packet received with invalid header flags");
                }
            }

            if (reader.RemainingLength != 2)
            {
                throw new MqttProtocolException("Remaining length of the incoming pubrel packet is invalid");
            }

            this.PacketId = reader.ReadIntegerField();
        }
    }

}
