using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Overtake
{
    class Program
    {
        public static Random Random;

        static void Main(string[] args)
        {
            // This example fetches and prints the first 10 data items
            Stopwatch stopwatch = new Stopwatch();
            double result;
            // all region as to be set as comment for quick run
            #region testingcode
            StringBuilder sb = new StringBuilder();
            var reader = new StreamReader(File.OpenRead("Test.csv"));
            List<List<double>> searchList = new List<List<double>>();
            while (!reader.EndOfStream)
            {
                string[] line= reader.ReadLine().Split(',');
                List<double> items = new List<double>();
                double item;
                foreach (string i in line)
                {
                    double.TryParse(i, out item);
                    items.Add(item);
                }
                    searchList.Add(items);
            }
            foreach (List<double> info in searchList)
            {
                

                stopwatch.Reset();

                // Begin timing.
                stopwatch.Start();


                result = DCOvertake.Run(info[0],info[1],info[2]);
                //Decision.Run();

                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

               
                sb.Append(info[0] + " " + info[1] + " " + info[2] +" "+ result+"% " +"in: "+ stopwatch.Elapsed +" , ");

                File.AppendAllText("log.txt", sb.ToString());
                sb.Clear();
            }
            #endregion

            /* code for normal running 
            
             stopwatch.Start();
            double epochs = 100; //epochs has to be an integer (stored into a double to make the creation of the matrix used easier (a class could have been used to create a List<class>))
            double hn  = 10 ;//hidden neurons has to be an integer too
            double lr = 0.1; //learning rates 

            result = DCOvertake.Run(epochs, ihn, lr);
            //Decision.Run();

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);*/

            Console.WriteLine("Press a key to end");
            Console.ReadLine();
        }





        public static void RandomSetAsRepeatable(bool repeatable)
        {
            if (repeatable)
                Random = new Random(0);
            else
                Random = new Random();
        }

        public static string[][] Shuffle(string[][] allInputs)
        {
            // The following statement is a shortcut to randomise an array and is equivalent to the longer:
            // return
            //   .Select(i => new { i, rand = random.NextDouble() }) // insert a temporary random key
            //   .OrderBy(x => x.rand) // sort on the random key
            //   .Select(x => x.i)     // remove the key
            //   .ToArray();
            return allInputs.OrderBy(x => Program.Random.NextDouble()).ToArray();
        }
        public static double[][] Shuffle(double[][] allInputs)
        {
            // The following statement is a shortcut to randomise an array and is equivalent to the longer:
            // return
            //   .Select(i => new { i, rand = random.NextDouble() }) // insert a temporary random key
            //   .OrderBy(x => x.rand) // sort on the random key
            //   .Select(x => x.i)     // remove the key
            //   .ToArray();
            return allInputs.OrderBy(x => Program.Random.NextDouble()).ToArray();
        }
    }
}
