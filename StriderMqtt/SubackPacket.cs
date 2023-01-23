using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class SubackPacket : IdentifiedPacket
    {
        internal const byte PacketTypeCode = 0x09;

        internal SubackReturnCode[] GrantedQosLevels
        {
            get;
            private set;
        }

        internal SubackPacket()
        {
            this.PacketType = PacketTypeCode;
        }


        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Clients should not send unsuback packets");
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                if ((reader.FixedHeaderFirstByte & Packet.PacketFlagsBitMask) != Packet.ZeroedHeaderFlagBits)
                {
                    throw new MqttProtocolException("Unsuback packet received with invalid header flags");
                }
            }

            this.PacketId = reader.ReadIntegerField();

            var bytes = reader.ReadToEnd();
            this.GrantedQosLevels = new SubackReturnCode[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] > (byte)SubackReturnCode.ExactlyOnceGranted && bytes[i] != (byte)SubackReturnCode.SubscriptionFailed)
                {
                    throw new MqttProtocolException(String.Format("Invalid qos level '{0}' received from broker", bytes[i]));
                }

                this.GrantedQosLevels[i] = (SubackReturnCode)bytes[i];
            }
        }
    }
}
