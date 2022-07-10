// test here
var q = new QueueLib.Queue<int>();

q.OnEnqueue += () =>
{
    Console.WriteLine("Enqueue event");
};
q.OnDequeue += () =>
{
    Console.WriteLine("Dequeue event");
};
q.OnClear += () =>
{
    Console.WriteLine("Clear event");
};

q.Enqueue(1);
q.Enqueue(2);
q.Enqueue(3);
q.Enqueue(4);

System.Console.WriteLine(q.Dequeue());
System.Console.WriteLine(q.Dequeue());
System.Console.WriteLine(q.Dequeue());
q.Clear();