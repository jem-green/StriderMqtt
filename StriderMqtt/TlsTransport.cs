using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StriderMqtt
{
    internal class TlsTransport : IInternalTransport
    {
        private TcpClient tcpClient;
        private NetworkStream netstream;
        private SslStream sslStream;

        public Stream Stream
        {
            get
            {
                return this.sslStream;
            }
        }

        public bool IsClosed
        {
            get
            {
                return tcpClient == null || !tcpClient.Connected;
            }
        }

        internal TlsTransport(string hostname, int port)
        {
            tcpClient = TcpTransport.CreateTcpClient(hostname);
            this.tcpClient.Connect(hostname, port);

            this.netstream = this.tcpClient.GetStream();

            var validationCallback = new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            this.sslStream = new SslStream(netstream, false, validationCallback, null);

            try
            {
                sslStream.AuthenticateAsClient(hostname, null, SslProtocols.Default, false);
            }
            catch (AuthenticationException e)
            {
                throw new MqttClientException("Error validating server certificate", e);
            }
        }

        bool ValidateRemoteCertificate(object sender,
                        X509Certificate certificate, X509Chain chain,
                        SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            X509Chain myChain = new X509Chain();

            // You can alter how the chain is built/validated.
            myChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            myChain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

            // Do the preliminary validation.
            if (!myChain.Build(new X509Certificate2(certificate)))
            {
                return false;
            }

            // Make sure we have the correct number of elements.
            if (myChain.ChainElements.Count != myChain.ChainPolicy.ExtraStore.Count + 1)
            {
                return false;
            }

            // Make sure all the thumbprints of the CAs match up.
            // The first one should be 'primaryCert', leading up to the root CA.
            for (var i = 1; i < myChain.ChainElements.Count; i++)
            {
                if (myChain.ChainElements[i].Certificate.Thumbprint != myChain.ChainPolicy.ExtraStore[i - 1].Thumbprint)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetTimeouts(TimeSpan readTimeout, TimeSpan writeTimeout)
        {
            this.sslStream.ReadTimeout = (int)readTimeout.TotalMilliseconds;
            this.sslStream.WriteTimeout = (int)writeTimeout.TotalMilliseconds;
        }

        public bool Poll(int pollLimit)
        {
            var limitMicros = Conversions.MillisToMicros(pollLimit);
            return tcpClient.Client.Poll(limitMicros, SelectMode.SelectRead);
        }

        public void Dispose()
        {
            this.sslStream.Close();
            this.netstream.Close();
            this.tcpClient.Close();
        }
    }
}
