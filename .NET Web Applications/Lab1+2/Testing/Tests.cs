using NUnit.Framework;
using QueueLib;

namespace Testing
{
    public class Tests
    {
        [Test]
        public void QueueEnqueueTest()
        {
            var queue = new Queue<int>();

            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            Assert.AreEqual(3, queue.Count);

            var queueArray = queue.ToArray();
            for (int i = 1; i <= queueArray.Length; i++)
            {
                Assert.AreEqual(i, queueArray[i - 1]);
            }
        }

        [Test]
        public void QueueDequeueTest()
        {
            var queue = new Queue<string>();

            queue.Enqueue("1");
            queue.Enqueue("2");
            queue.Enqueue("3");

            Assert.AreEqual("1", queue.Dequeue());
            Assert.AreEqual("2", queue.Dequeue());
            Assert.AreEqual("3", queue.Dequeue());

            Assert.AreEqual(0, queue.Count);
        }

        [Test]
        public void QueuePeekTest()
        {
            var queue = new Queue<object>();
            
            queue.Enqueue(1);
            queue.Enqueue("two");
            queue.Enqueue(3.0);

            Assert.AreEqual(1, queue.Peek());
            Assert.AreEqual(3, queue.Count);
            Assert.AreNotEqual("two", queue.Peek());
        }

        [Test]
        public void QueueContainsTest()
        {
            var queue = new Queue<double>();

            queue.Enqueue(1.0);
            queue.Enqueue(2.0);
            queue.Enqueue(3.0);

            Assert.IsTrue(queue.Contains(1.0));
            Assert.IsTrue(queue.Contains(3.0));
            Assert.IsFalse(queue.Contains(4.0));
        }

        [Test]
        public void QueueClearTest()
        {
            var queue = new Queue<string>();

            queue.Enqueue("1");
            queue.Enqueue("2");

            queue.Clear();

            Assert.AreEqual(0, queue.Count);
            Assert.IsFalse(queue.Contains("1"));
        }
    }
}

