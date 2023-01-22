using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StriderMqtt
{
    /// <summary>
    /// In memory persistence.
    /// This persistence support multiple incoming and outgoing messages,
    /// although ordering is only guaranteed when only one message is inflight per direction.
    /// </summary>
    public class InMemoryPersistence : IMqttPersistence, IDisposable
    {
        List<ushort> incomingPacketIds = new List<ushort>();
        List<OutgoingFlow> outgoingFlows = new List<OutgoingFlow>();

        public ushort LastOutgoingPacketId { get; set; }

        public InMemoryPersistence()
        {
        }

        public void RegisterIncomingFlow(ushort packetId)
        {
            incomingPacketIds.Add(packetId);
        }

        public void ReleaseIncomingFlow(ushort packetId)
        {
            if (incomingPacketIds.Contains(packetId))
            {
                incomingPacketIds.Remove(packetId);
            }
        }

        public bool IsIncomingFlowRegistered(ushort packetId)
        {
            return incomingPacketIds.Contains(packetId);
        }



        public void RegisterOutgoingFlow(OutgoingFlow outgoingMessage)
        {
            outgoingFlows.Add(outgoingMessage);
        }

        public IEnumerable<OutgoingFlow> GetPendingOutgoingFlows()
        {
            return new List<OutgoingFlow>(outgoingFlows);
        }

        public void SetOutgoingFlowReceived(ushort packetId)
        {
            OutgoingFlow msg = outgoingFlows.FirstOrDefault(m => m.PacketId == packetId);
            if (msg != null)
            {
                msg.Received = true;
            }
        }

        public void SetOutgoingFlowCompleted(ushort packetId)
        {
            outgoingFlows.RemoveAll(m => m.PacketId == packetId);
        }

        public void Clear()
        {
            incomingPacketIds.Clear();
            outgoingFlows.Clear();
            LastOutgoingPacketId = 0;
        }

        public void Dispose()
        {
            incomingPacketIds.Clear();
            outgoingFlows.Clear();
        }
    }
}
