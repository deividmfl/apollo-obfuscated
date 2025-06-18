using PhantomInterop.Structs.MythicStructs;

namespace PhantomInterop.Interfaces
{
    public interface StealthWorker0247
    {
        bool Start();
        void Stop();
        string GetUUID();
        string GetMythicUUID();
        bool Connected();
        void ProcessMessage(DelegateMessage message);
        bool Finished();
    }
}
