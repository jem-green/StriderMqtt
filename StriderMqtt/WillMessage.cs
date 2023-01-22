using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    public class WillMessage
    {
        public string Topic { get; set; }
        public byte[] Message { get; set; }
        public MqttQos Qos { get; set; }
        public bool Retain { get; set; }
    }
}
