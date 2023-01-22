﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    public class OutgoingFlow
    {
        /// <summary>
        /// The packetId used for publishing.
        /// </summary>
        public ushort PacketId { get; set; }

        public string Topic { get; set; }

        public MqttQos Qos { get; set; }

        public bool Retain { get; set; }

        /// <summary>
        /// The payload that will be published.
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// Received Flag, to be used with QoS2.
        /// This flag determines if the `Pubrec` packet was received from broker.
        /// </summary>
        public bool Received { get; set; }
    }
}
