using PhantomInterop.Interfaces;
using System;

namespace PhantomInterop.Classes.Events
{
    public class SecureTransformer7B29 : EventArgs
    {
        public IMythicMessage Message;

        public MythicMessageEventArgs(IMythicMessage msg) => Message = msg;
    }
}
