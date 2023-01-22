using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class ConnackPacket : PacketBase
    {
        internal const byte PacketTypeCode = 0x02;

        private const byte SessionPresentFlag = 0x01;

        internal bool SessionPresent
        {
            get;
            private set;
        }

        internal ConnackReturnCode ReturnCode
        {
            get;
            private set;
        }

        internal ConnackPacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Clients should not send connack packets");
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                if ((reader.FixedHeaderFirstByte & Packet.PacketFlagsBitMask) != Packet.ZeroedHeaderFlagBits)
                {
                    throw new MqttProtocolException("Connack packet received with invalid header flags");
                }
            }

            if (reader.RemainingLength != 2)
            {
                throw new MqttProtocolException("Connack packet received with invalid remaining length");
            }

            this.SessionPresent = (reader.ReadByte() & SessionPresentFlag) > 0;
            this.ReturnCode = (ConnackReturnCode)reader.ReadByte();
        }
    }
}
