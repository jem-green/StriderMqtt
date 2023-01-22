using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace StriderMqtt
{
    internal class TcpTransport : IInternalTransport
    {
        private TcpClient tcpClient;
        private NetworkStream netstream;

        public Stream Stream
        {
            get
            {
                return this.netstream;
            }
        }

        public bool IsClosed
        {
            get
            {
                return tcpClient == null || !tcpClient.Connected;
            }
        }

        internal TcpTransport(string hostname, int port)
        {
            tcpClient = CreateTcpClient(hostname);
            this.tcpClient.Connect(hostname, port);
            this.netstream = this.tcpClient.GetStream();

        }

        public void SetTimeouts(TimeSpan readTimeout, TimeSpan writeTimeout)
        {
            this.netstream.ReadTimeout = (int)readTimeout.TotalMilliseconds;
            this.netstream.WriteTimeout = (int)writeTimeout.TotalMilliseconds;
        }

        public bool Poll(int pollLimit)
        {
            var limitMicros = Conversions.MillisToMicros(pollLimit);
            return tcpClient.Client.Poll(limitMicros, SelectMode.SelectRead);
        }

        public void Dispose()
        {
            this.netstream.Close();
            this.tcpClient.Close();
        }

        internal static TcpClient CreateTcpClient(string hostname)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
            bool hasIpv4Address = false;

            foreach (var ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    //return new TcpClient(AddressFamily.InterNetworkV6);
                }
                else if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    hasIpv4Address = true;
                }
            }

            if (hasIpv4Address)
            {
                return new TcpClient();
            }
            else
            {
                throw new MqttClientException("Error determining the address family of the host");
            }
        }
    }

}
