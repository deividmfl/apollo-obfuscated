using System;
using System.IO.Pipes;
using PhantomInterop.Structs.PhantomStructs;

namespace PhantomInterop.Interfaces
{
    public interface CipherWorkerF898
    {
        void OnAsyncConnect(PipeStream pipe, out Object state);
        void OnAsyncDisconnect(PipeStream pipe, Object state);
        void OnAsyncMessageReceived(PipeStream pipe, IPCData data, Object state);
    }
}
