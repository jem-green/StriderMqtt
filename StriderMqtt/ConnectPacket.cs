﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal class ConnectPacket : PacketBase
    {
        internal const byte PacketTypeCode = 0x01;

        internal const string ProtocolNameV3_1 = "MQIsdp";
        internal const string ProtocolNameV3_1_1 = "MQTT";

        // max length for client id (removed in 3.1.1)
        internal const int ClientIdMaxLength = 23;

        internal const ushort KeepAlivePeriodDefault = 60; // seconds

        // connect flags
        internal const byte UsernameFlagOffset = 0x07;
        internal const byte PasswordFlagOffset = 0x06;
        internal const byte WillRetainFlagOffset = 0x05;
        internal const byte WillQosFlagOffset = 0x03;
        internal const byte WillFlagOffset = 0x02;
        internal const byte CleanSessionFlagOffset = 0x01;


        internal MqttProtocolVersion ProtocolVersion { get; set; }

        internal string ClientId { get; set; }

        internal bool WillRetain { get; set; }
        internal MqttQos WillQosLevel { get; set; }
        internal bool WillFlag { get; set; }
        internal string WillTopic { get; set; }
        internal byte[] WillMessage { get; set; }

        internal string Username { get; set; }
        internal string Password { get; set; }

        internal bool CleanSession { get; set; }

        internal ushort KeepAlivePeriod { get; set; }


        internal ConnectPacket()
        {
            this.PacketType = PacketTypeCode;
        }


        /// <summary>
        /// Reads a Connect packet from the given stream.
		/// (This method should not be used since clients don't receive Connect packets)
        /// </summary>
        /// <param name="fixedHeaderFirstByte">Fixed header first byte previously read</param>
        /// <param name="stream">The stream to read from</param>
        /// <param name="protocolVersion">The protocol version to be used to read</param>
		internal override void Deserialize(PacketReader reader, MqttProtocolVersion protocolVersion)
        {
            throw new MqttProtocolException("Connect packet should not be received");
        }

        /// <summary>
        /// Writes the Connect packet to the given stream and using the given
        /// protocol version.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="protocolVersion">Protocol to be used to write</param>
        internal override void Serialize(PacketWriter writer, MqttProtocolVersion protocolVersion)
        {
            if (protocolVersion == MqttProtocolVersion.V3_1_1)
            {
                // will flag set, will topic and will message MUST be present
                if (this.WillFlag && (WillMessage == null || String.IsNullOrEmpty(WillTopic)))
                {
                    throw new MqttProtocolException("Last will message is invalid");
                }
                // willflag not set, retain must be 0 and will topic and message MUST NOT be present
                else if (!this.WillFlag && (this.WillRetain || WillMessage != null || !String.IsNullOrEmpty(WillTopic)))
                {
                    throw new MqttProtocolException("Last will message is invalid");
                }
            }

            if (this.WillFlag && ((this.WillTopic.Length < Packet.MinTopicLength) || (this.WillTopic.Length > Packet.MaxTopicLength)))
            {
                throw new MqttProtocolException("Invalid last will topic length");
            }

            writer.SetFixedHeader(PacketType);

            MakeVariableHeader(writer);
            MakePayload(writer);
        }

        void MakeVariableHeader(PacketWriter w)
        {
            if (this.ProtocolVersion == MqttProtocolVersion.V3_1)
            {
                w.AppendTextField(ProtocolNameV3_1);
            }
            else
            {
                w.AppendTextField(ProtocolNameV3_1_1);
            }

            w.Append((byte)this.ProtocolVersion);

            w.Append(MakeConnectFlags());
            w.AppendIntegerField(KeepAlivePeriod);
        }

        byte MakeConnectFlags()
        {
            byte connectFlags = 0x00;
            connectFlags |= (Username != null) ? (byte)(1 << UsernameFlagOffset) : (byte)0x00;
            connectFlags |= (Password != null) ? (byte)(1 << PasswordFlagOffset) : (byte)0x00;
            connectFlags |= (this.WillRetain) ? (byte)(1 << WillRetainFlagOffset) : (byte)0x00;

            if (this.WillFlag)
                connectFlags |= (byte)((byte)WillQosLevel << WillQosFlagOffset);

            connectFlags |= (this.WillFlag) ? (byte)(1 << WillFlagOffset) : (byte)0x00;
            connectFlags |= (this.CleanSession) ? (byte)(1 << CleanSessionFlagOffset) : (byte)0x00;

            return connectFlags;
        }

        void MakePayload(PacketWriter w)
        {
            w.AppendTextField(ClientId);

            if (!String.IsNullOrEmpty(WillTopic))
            {
                w.AppendTextField(WillTopic);
            }

            if (WillMessage != null)
            {
                w.AppendBytesField(WillMessage);
            }

            if (Username != null)
            {
                w.AppendTextField(Username);
            }

            if (Password != null)
            {
                w.AppendTextField(Password);
            }
        }
    }
}
