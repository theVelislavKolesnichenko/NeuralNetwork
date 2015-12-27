using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetConsoleClient
{
    public class Program
    {
        static void Main(string[] args)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("us-EN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("us-EN");

            int era = 0;

            int[] i = new int[] { 1, 2, 3 };
            int[] w = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            Network networkTest = new Network(new int[] { 3,3,3}, "test");

            int[] dims = { 3, 3, 3 };
            string file = "test.csv";
            Network network = new Network(dims, file);


            double error = int.MaxValue;
            while (error > 0.01)  
            {
                era++;
                error = network.Train();
                //foreach (Neuron neuron in network.Outputs)
                //{
                Console.Clear();
                Console.WriteLine("{0},error: {1}", era, error);
                //Thread.Sleep(50);
                //}

            }


            ConsoleKeyInfo c = Console.ReadKey();
            //if (c.Key == ConsoleKey.C) break;
            //else Console.WriteLine();
        }
    }
}
