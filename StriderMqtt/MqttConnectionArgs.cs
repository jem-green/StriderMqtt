using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    public class MqttConnectionArgs
    {
        public string Hostname { get; set; }
        public int Port { get; set; }

        public bool Secure { get; set; }

        public MqttProtocolVersion ProtocolVersion { get; set; }

        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool CleanSession { get; set; }

        public TimeSpan Keepalive { get; set; }

        public WillMessage WillMessage { get; set; }

        public TimeSpan ReadTimeout { get; set; }
        public TimeSpan WriteTimeout { get; set; }

        public MqttConnectionArgs()
        {
            this.ProtocolVersion = MqttProtocolVersion.V3_1_1;
            this.Keepalive = TimeSpan.FromSeconds(60);

            this.Port = 1883;
            this.CleanSession = true;

            this.ReadTimeout = TimeSpan.FromSeconds(10);
            this.WriteTimeout = TimeSpan.FromSeconds(10);
        }
    }
}
