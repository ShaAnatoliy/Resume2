using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            EventHandler h = new EventHandler(MyEventHandler);
            AsyncCaller ac = new AsyncCaller(h);
            bool completedOK = ac.Invoke(5000, null, EventArgs.Empty);

            Console.WriteLine("Task Completed: {0}", completedOK.ToString());
            Console.ReadKey();
        }

        static void MyEventHandler(object sender, EventArgs e)
        {
            Random rnd = new Random();
            long sum = 0;
            int n = 5000000;
            for (int ctr = 1; ctr <= n; ctr++)
            {
                int number = rnd.Next(0, 101);
                sum += number;
            }
            Console.WriteLine("Total:   {0:N0}", sum);
            Console.WriteLine("Mean:    {0:N2}", sum / n);
            Console.WriteLine("N:       {0:N0}", n);
        }
    }
}
