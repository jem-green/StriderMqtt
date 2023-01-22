using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StriderMqtt
{
    public interface IMqttTransport
    {
        /// <summary>
        /// The Stream object to read from and write to.
        /// </summary>
        /// <value>The stream.</value>
        Stream Stream { get; }

        /// <summary>
        /// Gets a value indicating whether it is possible to read from and write to the Stream.
        /// </summary>
        /// <value><c>true</c> if Stream is connected and available; otherwise, <c>false</c>.</value>
        bool IsClosed { get; }

        /// <summary>
        /// Poll the connection for the specified pollLimit time.
        /// </summary>
        /// <param name="pollLimit">Poll limit time in milliseconds.</param>
        bool Poll(int pollLimit);
    }
}
