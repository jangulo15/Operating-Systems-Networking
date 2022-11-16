using System.Threading;
using System.Collections.Concurrent;

class Program
{   
    //Benefits of BlockingCollection??
    static BlockingCollection<Customer> customers = new BlockingCollection<Customer>();
    static int numberOfWorkers, averageArrivalTime, averageServiceTime, simulationDuration;
    static int numberOfCustomers = 10;

    static Semaphore semaphore = new Semaphore(numberOfWorkers, numberOfWorkers);

    static void Service(object arg)
    {
        Thread.Sleep(averageArrivalTime);

        semaphore.WaitOne();
        semaphore.Release();

    }

    static void QueueCustomer(object? arg)
    {
        for(var i = 0; i < numberOfCustomers; i++)
        {
            //Check Tryadd, ParameterizedThreadStart
            customers.TryAdd(new Customer { CustomerThread = new Thread(new ParameterizedThreadStart(Service))});
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("How many workers will we have? ");
        numberOfWorkers = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("What is the average arrival time? ");
        averageArrivalTime = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("What is the average service time? ");
        averageServiceTime = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("How long will this simulation run? ");
        simulationDuration = Convert.ToInt32(Console.ReadLine());





        //What does this do exactly?
        var queueThread = new Thread(new ParameterizedThreadStart(QueueCustomer));
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
