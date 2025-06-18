using SystemAgentInterop.Structs.SystemCoreStructs;
using System;
using System.Net.Sockets;

namespace SystemAgentInterop.Interfaces
{
    public interface ITcpClientCallback
    {
        void OnAsyncConnect(TcpClient client, out Object state);
        void OnAsyncDisconnect(TcpClient client, Object state);
        void OnAsyncMessageReceived(TcpClient client, IPCData data, Object state);
    }
}
