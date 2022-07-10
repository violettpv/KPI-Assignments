using System.Collections;

namespace QueueLib
{
    // create queue based on linked list
    public class Queue<T> : IEnumerable<T>, ICollection
    {
        private QueueLib.LinkedList<T> list;
        
        public Queue()
        {
            this.list = new LinkedList<T>();

            // initialize events with empty delegates
            OnEnqueue = () => { };
            OnDequeue = () => { };
            OnClear = () => { };
        }

        // imlement IEnumerable
        // returns linked list enumerator
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // implement ICollection
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }
        public bool IsSynchronized
        {
            get
            {
                return this.list.IsSynchronized;
            }
        }
        public object SyncRoot
        {
            get
            {
                return this.list.SyncRoot;
            }
        }
        
        public void CopyTo(Array array, int index)
        {
            this.list.CopyTo(array, index);
        }

        // methods
        // NOTE: TrimExcess() can not be implemented as queue is based on an *dynamic* collection
        public void Enqueue(T item)
        {
            this.list.AddLast(item);
            OnEnqueue.Invoke();
        }
        public T Dequeue()
        {
            if (this.list.First == null)
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            else
            {
                // get the first item from the list and remove it
                T item = this.list.First;
                this.list.RemoveFirst();

                OnDequeue.Invoke();

                // return in
                return item;
            }
        }
        public T Peek()
        {
            if (this.list.First == null)
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            else
            {
                return this.list.First;
            }
        }
        public T[] ToArray()
        {
            return this.list.ToArray();
        }
        public bool Contains(T item)
        {
            return this.list.Contains(item);
        }
        public void Clear()
        {
            this.list.Clear();
            OnClear.Invoke();
        }
    
        // events
        public delegate void EventDelegate();

        public event EventDelegate OnEnqueue;
        public event EventDelegate OnDequeue;
        public event EventDelegate OnClear;
    }
}