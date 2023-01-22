using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    public class Packet
    {
        internal const byte PacketTypeOffset = 0x04;
        internal const byte PacketTypeMask = 0xF0;

        internal const byte DupFlagOffset = 0x03;
        internal const byte DupFlagMask = 0x08;

        internal const byte RetainFlagOffset = 0x00;
        internal const byte RetainFlagMask = 0x01;

        internal const byte QosLevelOffset = 0x01;
        internal const byte QosLevelMask = 0x06;

        internal const byte PacketFlagsBitMask = 0x0F;
        internal const byte ZeroedHeaderFlagBits = 0x00;
        internal const byte Qos1HeaderFlagBits = 0x02;

        public const uint MaxRemainingLength = 268435455;

        internal const uint MaxPacketId = 65535;

        public const ushort MinTopicLength = 1;
        public const ushort MaxTopicLength = 65535;
    }
}
