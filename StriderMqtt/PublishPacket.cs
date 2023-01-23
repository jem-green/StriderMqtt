using System;
using System.Text;

namespace StriderMqtt
{
    internal class PublishPacket : IdentifiedPacket
    {
		internal const byte PacketTypeCode = 0x03;

		/// <summary>
		/// Duplicate message flag
		/// </summary>
		internal bool DupFlag { get; set; }

		/// <summary>
		/// Quality of Service, see `MqttQualityOfService`
		/// </summary>
		internal MqttQos QosLevel { get; set; }

		/// <summary>
		/// Retain message flag
		/// </summary>
		internal bool Retain { get; set; }

        /// <summary>
        /// Gets or sets the topic to send the application message.
        /// </summary>
        /// <value>The topic.</value>
		internal string Topic { get; set; }

        /// <summary>
        /// Gets or sets the Application Message to be sent to the broker.
        /// </summary>
        /// <value>The application message.</value>
		internal byte[] Message { get; set; }
        
        internal PublishPacket()
        {
            this.PacketType = PacketTypeCode;
        }

		internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
			if (protocolVersion == MqttProtocolVersion.V3_1_1)
			{
				if (this.QosLevel == MqttQos.AtMostOnce && this.PacketId > 0)
				{
					throw new ArgumentException("When using QoS 0 (at most once) the PacketId must not be set");
				}
			}

			writer.SetFixedHeader(PacketType, DupFlag, QosLevel, Retain);

			// variable header
			writer.AppendTextField(this.Topic);

			if (this.QosLevel > MqttQos.AtMostOnce)
			{
				writer.AppendIntegerField(this.PacketId);
			}

			writer.Append(this.Message);
        }

        internal void Validate()
        {
            ValidateTopic();
        }

		private void ValidateTopic()
		{
			// topic can't contain wildcards
			if ((this.Topic.IndexOf('#') != -1) || (this.Topic.IndexOf('+') != -1))
			{
				throw new ArgumentException("Cannot use wildcards when publishing");
			}

			// check topic length
			if ((this.Topic.Length < Packet.MinTopicLength) || (this.Topic.Length > Packet.MaxTopicLength))
			{
				throw new ArgumentException("Invalid topic length");
			}
		}

        internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
		{
			this.DupFlag = reader.Dup;
			this.QosLevel = reader.QosLevel;
			this.Retain = reader.Retain;

			this.Topic = reader.ReadTextField();

			if (QosLevel > MqttQos.AtMostOnce)
			{
				this.PacketId = reader.ReadIntegerField();
			}

			this.Message = reader.ReadToEnd();
		}
    }












}
