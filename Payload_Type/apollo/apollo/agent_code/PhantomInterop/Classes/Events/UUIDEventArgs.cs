using System;

namespace PhantomInterop.Classes
{
    public class SpectreManagerFFB2 : EventArgs
    {
        public readonly string UUID;
        public UUIDEventArgs(string uuid)
        {
            UUID = uuid;
        }
    }
}
