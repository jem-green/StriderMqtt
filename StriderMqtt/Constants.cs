using System;

namespace StriderMqtt
{
	/// <summary>
	/// MQTT protocol version enumeration
	/// </summary>
	public enum MqttProtocolVersion : byte
	{
		/// <summary>
		/// Version 3.1
		/// </summary>
		V3_1 = 0x03,

		/// <summary>
		/// Version 3.1.1
		/// </summary>
		V3_1_1 = 0x04
	}

	/// <summary>
	/// MQTT Quality Of Service levels enumeration
	/// </summary>
	public enum MqttQos : byte
	{
		/// <summary>
		/// QoS Level 0: At most once.
		/// </summary>
		AtMostOnce = 0x00,

		/// <summary>
		/// QoS Level 1: At least once.
		/// </summary>
		AtLeastOnce = 0x01,

		/// <summary>
		/// QoS Level 2 : The exactly once.
		/// </summary>
		ExactlyOnce = 0x02
	}

	public enum ConnackReturnCode : byte
	{
		Accepted = 0x00,
		UnacceptableProtocol = 0x01,
		IdentifierRejected = 0x02,
		BrokerUnavailable = 0x03,
		BadUsernameOrPassword = 0x04,
		NotAuthorized = 0x05
	}

	public enum SubackReturnCode : byte
	{
		AtMostOnceGranted = 0x00,
		AtLeastOnceGranted = 0x01,
		ExactlyOnceGranted = 0x02,
		SubscriptionFailed = 0x80
	}

    static class Conversions
    {
        internal static int MillisToMicros(int millis)
        {
            long total = 1000L * millis;
            if (total > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (total < int.MinValue)
            {
                return int.MinValue;
            }
            else
            {
                return (int)total;
            }
        }
    }
}
