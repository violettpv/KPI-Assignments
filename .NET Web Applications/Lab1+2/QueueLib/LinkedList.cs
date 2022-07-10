using System.Collections;

namespace QueueLib
{
    // create node-based linked list
    public class LinkedList<T> : ICollection<T>, ICollection
    {
        // node class
        private class Node
        {
            public T data;
            public Node next;
            public Node prev;
             
            public Node(T data)
            {
                this.data = data;
                this.next = null;
                this.prev = null;
            }
        }
    
        private Node first;
        private Node last;
        private int count;
        
        public T First 
        {
            get
            {   
                // returns first node data
                return this.first.data;
            }
        }

        public LinkedList()
        {
            this.first = null;
            this.last = null;
            this.count = 0;
        }

        // implement ICollection<T>
        public int Count
        {
            // a readonly accessor
            get
            {
                return this.count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                // this collection is NOT readonly
                return false;
            }
        }

        public void Add(T item)
        {
            // this method is not required for queue, but is required by ICollection<T> to implement
            
            // create a node from passed data
            Node node = new Node(item);

            // insert in into reference "chain"
            if (this.first == null)
            {
                this.first = node;
                this.last = node;
            }
            else
            {
                // insert to the end
                this.last.next = node;
                node.prev = this.last;
                this.last = node;
            }
            this.count++;
        }
        public void Clear()
        {
            this.first = null;
            this.last = null;
            this.count = 0;
        }
        public bool Contains(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            } 

            // iterate through collection comparing data
            Node current = this.first;
            while (current != null)
            {
                if (current.data != null)
                {
                    if (current.data.Equals(item))
                    {
                        return true;
                    }
                }
                current = current.next;
            }
            return false;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            // iterate through collection & fill the array
            Node current = this.first;
            while (current != null)
            {
                array[arrayIndex] = current.data;
                current = current.next;
                arrayIndex++;
            }
        }
        public bool Remove(T item)
        {
            if (item == null) 
            {
                throw new ArgumentNullException();
            }
            if (this.count == 0) 
            {
                throw new InvalidOperationException();
            }

            // iterate through collection
            Node current = this.first;
            while (current != null)
            {
                if (current.data != null)
                {
                    // if item found
                    if (current.data.Equals(item))
                    {
                        if (current.prev == null) 
                        {
                            // if it was the first item
                            // replace with next one
                            this.first = current.next; 
                        }
                        else
                        {
                            // change previous node's "next" to current's "next"
                            current.prev.next = current.next;
                        }

                        if (current.next == null) 
                        {
                            // if it was the last item
                            // replace with previous one
                            this.last = current.prev; 
                        }
                        else
                        {
                            // change next node's "prev" to current's "prev"
                            current.next.prev = current.prev;
                        }
                        this.count--;
                        return true;
                    }
                }
                current = current.next;
            }
            return false;
        }

        // implement enumerator
        public IEnumerator<T> GetEnumerator()
        {
            Node current = this.first;
            while (current != null)
            {
                yield return current.data;
                current = current.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // implement ICollection
        public void CopyTo(Array array, int index)
        {
            Node current = this.first;
            while (current != null)
            {
                array.SetValue(current.data, index);
                current = current.next;
                index++;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                // this collection is not thread safe
                return false;
            }
        }
        public object SyncRoot
        {
            get
            {
                // this collection is not thread safe
                return this;
            }
        }
    
        // methods (that are necessairy for queue)
        public void AddLast(T item)
        {
            if (item == null) 
            {
                 throw new ArgumentNullException();
            }

            Node node = new Node(item);

            if (this.first == null)
            {
                // if collection is empty
                this.first = node;
                this.last = node;
            }
            else
            {
                // insert to the end
                // change last node's "next" to new node
                this.last.next = node;
                // change new node's "prev" to last node
                node.prev = this.last;
                // change last node to new node
                this.last = node;
            }
            this.count++;
        }
        public void RemoveFirst()
        {
            if (this.first == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                // replace first with first's "next"
                this.first = this.first.next;
                this.count--;
            }
        }
    }
}