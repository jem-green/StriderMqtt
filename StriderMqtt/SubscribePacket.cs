using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class SubscribePacket : IdentifiedPacket
    {
        internal const byte PacketTypeCode = 0x08;

        private const byte QosPartMask = 0x03;

        /// <summary>
        /// List of topics to subscribe
        /// </summary>
		internal string[] Topics { get; set; }

        /// <summary>
        /// List of QOS Levels related to topics
        /// </summary>
		internal MqttQos[] QosLevels { get; set; }


        internal SubscribePacket()
        {
            this.PacketType = PacketTypeCode;
        }

        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            Validate();

            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                writer.SetFixedHeader(PacketType, MqttQos.AtLeastOnce);
            }
            else
            {
                writer.SetFixedHeader(PacketType);
            }

            writer.AppendIntegerField(PacketId);

            for (int i = 0; i < Topics.Length; i++)
            {
                writer.AppendTextField(this.Topics[i]);
                writer.Append((byte)(((byte)this.QosLevels[i]) & QosPartMask));
            }
        }

        private void Validate()
        {
            if (Topics.Length != QosLevels.Length)
            {
                throw new ArgumentException("The length of Topics should match the length of QosLevels");
            }

            foreach (string topic in Topics)
            {
                if (String.IsNullOrEmpty(topic) || topic.Length > Packet.MaxTopicLength)
                {
                    throw new ArgumentException("Invalid topic length");
                }
            }
        }

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Clients should not receive subscribe packets");
        }
    }
}
