using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    internal interface IInternalTransport : IMqttTransport, IDisposable { }
}
