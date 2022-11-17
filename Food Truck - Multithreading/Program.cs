using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

class Program
{   
    //Benefits of BlockingCollection??
    static BlockingCollection<Customer> customers = new BlockingCollection<Customer>();
    static int numberOfWorkers = 3;
    static int averageArrivalTime, averageServiceTime, simulationDuration;
    static int numberOfCustomers = 10;

    static Semaphore workerSemaphore = new Semaphore(numberOfWorkers, numberOfWorkers);

    static void Service(object? arg)
    {
        Thread.Sleep(averageArrivalTime);

        // workerSemaphore.WaitOne();
        // workerSemaphore.Release();

    }

    static void QueueCustomers(object? arg)
    {
        for(var i = 0; i < numberOfCustomers; i++)
        {
            //Check Tryadd, ParameterizedThreadStart
            customers.TryAdd(new Customer { CustomerThread = new Thread(new ParameterizedThreadStart(Service))});
        }
    }

    public static void Main(string[] args)
    {
        // Console.WriteLine("How many workers will we have? ");
        // numberOfWorkers = Convert.ToInt32(Console.ReadLine()) * 1000;

        Console.WriteLine("What is the average arrival time in seconds? ");
        averageArrivalTime = Convert.ToInt32(Console.ReadLine()) * 1000;

        Console.WriteLine("What is the average service time in seconds? ");
        averageServiceTime = Convert.ToInt32(Console.ReadLine()) * 1000;

        Console.WriteLine("How long will this simulation run in seconds? ");
        simulationDuration = Convert.ToInt32(Console.ReadLine()) * 1000;





        //What does this do exactly?
        var queueThread = new Thread(new ParameterizedThreadStart(QueueCustomers));
        queueThread.Start();

        //What are TryTake/out var??
        while(customers.TryTake(out var customer))
        {
            customer?.CustomerThread?.Start(null);
        }

        while(customers.TryTake(out var customer))
        {
            customer?.CustomerThread?.Join();
        }
    }
}
