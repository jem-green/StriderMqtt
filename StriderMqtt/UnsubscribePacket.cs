using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class UnsubscribePacket : IdentifiedPacket
    {
        internal const byte PacketTypeCode = 0x0A;

        internal string[] Topics { get; set; }

        internal UnsubscribePacket()
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

            foreach (string topic in this.Topics)
            {
                writer.AppendTextField(topic);
            }
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Clients should not send unsubscribe packets");
        }
    }
}
