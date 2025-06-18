using SystemAgentInterop.Classes.P2P;
using SystemAgentInterop.Structs.MythicStructs;
namespace SystemAgentInterop.Interfaces
{
    public interface IPeerManager
    {
        Peer AddPeer(PeerInformation info);
        bool Remove(string uuid);
        bool Remove(IPeer peer);
        bool Route(DelegateMessage msg);
    }
}
