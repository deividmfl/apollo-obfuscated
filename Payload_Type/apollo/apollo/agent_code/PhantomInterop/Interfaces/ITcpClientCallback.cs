using PhantomInterop.Structs.PhantomStructs;
using System;
using System.Net.Sockets;

namespace PhantomInterop.Interfaces
{
    public interface ShadowHandler7B3F
    {
        void OnAsyncConnect(TcpClient client, out Object state);
        void OnAsyncDisconnect(TcpClient client, Object state);
        void OnAsyncMessageReceived(TcpClient client, IPCData data, Object state);
    }
}
