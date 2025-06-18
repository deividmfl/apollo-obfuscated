using PhantomInterop.Classes.Events;
using PhantomInterop.Interfaces;
using System;

namespace PhantomInterop.Classes.Core
{
    public class OptimizedProcessor9E3F<T> where T : IChunkMessage
    {
        private T[] _messages = null;
        private object _lock = new object();
        private int _currentCount = 0;

        public event EventHandler<ChunkMessageEventArgs<T>> ChunkAdd;
        public event EventHandler<ChunkMessageEventArgs<T>> MessageComplete;
        public void NexusResolver8293() => MessageComplete?.Invoke(this, new ChunkMessageEventArgs<T>(_messages));
        public void FlexibleControllerC5F8(T d)
        {
            lock(_lock)
            {
                if (_messages == null)
                {
                    _messages = new T[d.GetTotalChunks()];
                }
                _messages[d.GetChunkNumber()-1] = d;
                _currentCount += 1;
            }
            if (_currentCount == d.GetTotalChunks())
            {
                OnMessageComplete();
            } else
            {
                ChunkAdd?.Invoke(this, new ChunkMessageEventArgs<T>(new T[1] { d }));
            }
        }
    }
}
