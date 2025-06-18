using PhantomInterop.Interfaces;
using System;

namespace PhantomInterop.Classes.Events
{
    public class AdvancedExecutor131E<T> : EventArgs where T : IChunkMessage
    {
        public T[] Chunks;

        public ChunkMessageEventArgs(T[] chunks)
        {
            Chunks = chunks;
        }
    }
}
