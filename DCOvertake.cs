using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overtake
{
    class DCOvertake
    {
		//check the decompiled dll or the csv file for true possible result name (case sensitive)
        private static readonly List<string> PossibleResults = new List<string> { "True" , "False" };
        static string[][] carinput;

        public static double Run(double e, double hn,double lr)
        {
            // Settings

            var generation = 1000; //number of data generated 
            var epochs = e; // Number of training iterations maybe change it to regenerate data each epoch see if it'll improve success ratio
            var inputNodes = 3;
            var hiddenNodes = Convert.ToInt32(hn);
            var outputNodes = 2;
            var learningRate = lr;
            var trainPerCent = 67;
            Program.RandomSetAsRepeatable(true);

            // Read the data and shuffle it
            var shuffledInputs = GetInputs(generation);
            var trainDataSetCount = Convert.ToInt32(shuffledInputs.Length * trainPerCent / 100);

            // Create a network with random weights
            var network = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes, learningRate);

            // Train on a random percentage of the 150 data samples
            var trainDataSet = shuffledInputs.Take(trainDataSetCount).ToArray();

            Console.WriteLine($"Training network with {trainDataSet.Length} samples using {epochs} epochs...");

            for (var epoch = 0; epoch < epochs; epoch++)
            {
                foreach (var input in trainDataSet)
                {
                    var inputList = input.Take(inputNodes).Select(double.Parse).ToArray();

                    var targets = new[] { 0.01, 0.01 };
                    targets[PossibleResults.IndexOf(input.Last())] = 0.99;

                    network.Train(NormaliseData(inputList), targets);
                }
            }
            var scoreCard = new List<bool>();


            // Test on the rest of the data samples
            var testDataset = shuffledInputs.Skip(trainDataSetCount).ToArray();

            foreach (var input in testDataset)
            {
                // The node with the largest value is the answer found
                var result = network.Query(NormaliseData(input.Take(3).Select(double.Parse).ToArray())).ToList();
                var predictedResult = PossibleResults[result.IndexOf(result.Max())];

                // The correct answer is in the final field of the input
                var correctResult = PossibleResults[PossibleResults.IndexOf(input.Last())];

                var miss = (predictedResult == correctResult) ? "" : "miss";
                Console.WriteLine($"{input[0],4}, {input[1],4}, {input[2],4}, {correctResult,-6}, {predictedResult,-6} {miss}");

                scoreCard.Add(correctResult == predictedResult);
            }
            double Performance = (scoreCard.Count(x => x) / Convert.ToDouble(scoreCard.Count)) * 100;
            Console.WriteLine($"Performance is {Performance} percent.");
            return Performance;
        }


        private static string[][] GetInputs(int generation)
        {
            //test which way work with matrices when access to internet and visual studio
            carinput = new string[generation][];
            
            for (int i = 0; i < generation; i++)
            {
                var overtake = Library.Overtake.GetNextOvertake();
                string[] j = new string[4];

                j[0] = Convert.ToString(overtake.InitialSeparationM);
                j[1] = Convert.ToString(overtake.OvertakingSpeedMPS);
                j[2] = Convert.ToString(overtake.OncomingSpeedMPS);
                j[3] = Convert.ToString(overtake.Success);

                carinput[i] = j;

                //carinput[i][0] = Convert.ToString(overtake.InitialSeparationM);
                //carinput[i][1] = Convert.ToString(overtake.OvertakingSpeedMPS);
                //carinput[i][2] = Convert.ToString(overtake.OncomingSpeedMPS);
                //carinput[i][3] = Convert.ToString(overtake.Success);
                //Console.WriteLine($"{overtake.ToString()}\n");
                //Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                //Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                //Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                //Console.WriteLine($"Success = {overtake.Success}\n");
            }

            return Program.Shuffle(carinput);
        }

        private static double[] NormaliseData(double[] input)
        {
			//to be checked with the decompiled file for max random generated numbers
            var maxInitialSeparationM = 300;
            var maxOvertakingSpeedMPS = 30;
            var maxOncomingSpeedMPS= 50;

            var normalised = new[]
            {
                (input[0]/maxInitialSeparationM),
                (input[1]/maxOvertakingSpeedMPS),
                (input[2]/maxOncomingSpeedMPS)
            };

            return normalised;
        }
    }
}
