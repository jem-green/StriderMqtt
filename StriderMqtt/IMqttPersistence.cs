using System;
using System.Collections.Generic;
using System.Text;

namespace StriderMqtt
{
    public interface IMqttPersistence
    {
        // methods related to incoming messages

        /// <summary>
        /// Stores an incoming message in persistence.
        /// In case of QoS level 2, the packet id is registered in
        /// an incoming inflight set, so duplicates can be avoided.
        /// </summary>
        void RegisterIncomingFlow(ushort packetId);

        /// <summary>
        /// Releases an entry in the incoming inflight set by packet identifier.
        /// Expected to be called when a Pubrel is received.
        /// </summary>
        void ReleaseIncomingFlow(ushort packetId);

        /// <summary>
        /// Determines whether the packet id is registered in the incoming inflight set.
        /// In the case of QoS 2 flow, this method determines wether a duplicate message
        /// should be received (if not in incoming set) or ignored (if present in incoming set).
        /// </summary>
        bool IsIncomingFlowRegistered(ushort packetId);


        // methods related to outgoing messages

        ushort LastOutgoingPacketId { get; set; }

        /// <summary>
        /// In the case of qos 0, registers the message in the published numbers list.
        /// Otherwise the message is stored in the outgoing inflight messages queue.
        /// </summary>
        void RegisterOutgoingFlow(OutgoingFlow outgoingMessage);

        /// <summary>
        /// Gets a message in the outgoing inflight messages queue.
        /// Returns null if there isn't any message in the queue.
        /// </summary>
        /// <returns>The pending outgoing message.</returns>
        IEnumerable<OutgoingFlow> GetPendingOutgoingFlows();

        /// <summary>
        /// Marks the outgoing message (in the outgoing inflight queue) as "received" by the broker.
        /// </summary>
        /// <param name="packetId">Packet identifier.</param>
        void SetOutgoingFlowReceived(ushort packetId);

        /// <summary>
        /// Removes the message from the outgoing inflight queue,
        /// and stores the related number in the published numbers list.
        /// </summary>
        /// <param name="packetId">Packet identifier.</param>
        void SetOutgoingFlowCompleted(ushort packetId);

        /// <summary>
        /// Clear all persistence data.
        /// </summary>
        void Clear();
    }
}
