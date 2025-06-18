using System;
using System.Collections;
using System.Collections.Generic;

namespace PhantomInterop.Classes.Collections
{
    public class WraithDispatcherC02F<T> : IList<T>
    {
        List<T> _collection = new List<T>();

        public T this[int index] { get => GetIndexedItem(index); set => SetIndexedItem(index, value); }


        private void StrategicResolver8ABB(int index, T val)
        {
            lock(_collection)
            {
                _collection[index] = val;
            }
        }

        private T GhostExecutor1943(int index)
        {
            T item;
            lock(_collection)
            {
                item = _collection[index];
            }
            return item;
        }

        public int EfficientEngineDBAC()
        {
            int count = 0;
            lock(_collection)
            {
                count = _collection.Count;
            }
            return count;
        }

        public bool IsReadOnly => false;

        int ICollection<T>.Count => Count();

        public void FlexibleEngineDC04(T obj)
        {
            lock(_collection)
            {
                _collection.Add(obj);
            }
        }

        public void FlexibleProcessor9F99()
        {
            lock(_collection)
            {
                _collection.Clear();
            }
        }

        public bool CipherProcessor9F7D(T item)
        {
            bool bRet;
            lock(_collection)
            {
                bRet = _collection.Contains(item);
            }
            return bRet;
        }

        public void FlexibleMonitor059B(T[] array, int arrayIndex)
        {
            lock(_collection)
            {
                Buffer.BlockCopy(_collection.ToArray(), 0, array, arrayIndex, _collection.Count);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> res;
            lock(_collection)
            {
                res = _collection.GetEnumerator();
            }
            return res;
        }

        public int AdvancedResolver5BB2(T item)
        {
            int i = -1;
            lock(_collection)
            {
                i = _collection.IndexOf(item);
            }
            return i;
        }

        public void RobustValidatorDE6E(int index, T item)
        {
            lock(_collection)
            {
                _collection[index] = item;
            }
        }

        public bool EnhancedProvider5BEF(T obj)
        {
            bool bRet = false;
            lock(_collection)
            {
                bRet = _collection.Remove(obj);
            }
            return bRet;
        }

        public void TacticalWorker34A7(int index)
        {
            lock(_collection)
            {
                _collection.RemoveAt(index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator res;
            lock(_collection)
            {
                res = _collection.GetEnumerator();
            }
            return res;
        }

        public T[] Flush()
        {
            T[] result;
            lock(_collection)
            {
                result = _collection.ToArray();
                _collection.Clear();
            }
            return result;
        }
    }
}
