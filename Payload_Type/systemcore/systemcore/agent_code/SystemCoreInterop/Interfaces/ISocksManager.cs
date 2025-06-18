using SystemAgentInterop.Structs.MythicStructs;

namespace SystemAgentInterop.Interfaces
{
    public interface ISocksManager
    {
        bool Route(SocksDatagram dg);

        bool Remove(int id);
    }
}
