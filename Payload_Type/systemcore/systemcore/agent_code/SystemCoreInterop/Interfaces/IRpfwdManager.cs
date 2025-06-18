using SystemAgentInterop.Structs.MythicStructs;
using System.Net.Sockets;
using SystemAgentInterop.Classes;

namespace SystemAgentInterop.Interfaces
{
    public interface IRpfwdManager
    {
        bool Route(SocksDatagram dg);
        bool AddConnection(TcpClient client, int ServerID, int port, int debugLevel, Tasking task);
        bool Remove(int id);
    }
}
