using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace StriderMqtt
{
	internal class UnsubackPacket : IdentifiedPacket
	{
		internal const byte PacketTypeCode = 0x0B;

		internal UnsubackPacket()
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

			if (reader.RemainingLength != 2)
			{
				throw new MqttProtocolException("Unsuback packet received with invalid remaining length");
			}

			this.PacketId = reader.ReadIntegerField();
		}
	}
}
