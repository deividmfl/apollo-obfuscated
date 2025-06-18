using SystemAgentInterop.Enums.SystemCoreEnums;

namespace SystemAgentInterop.Types
{
    namespace Delegates
    {
        public delegate bool OnResponse<T>(T message);
        public delegate bool DispatchMessage(byte[] data, MessageType mt);
    }
}
