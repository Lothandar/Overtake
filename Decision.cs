using Accord.MachineLearning.DecisionTrees; // for DecisionVariable
using Accord.MachineLearning.DecisionTrees.Learning; // for ID3Learning
using Accord.MachineLearning.DecisionTrees.Rules; // for DecisionSet
using Accord.Math; // for DataTable extensions such as ToArray;
using Accord.Math.Optimization.Losses; // for ZeroOneLoss
using Accord.Statistics.Filters; // for Codification
using System.Collections.Generic;

using System;
using System.Data; // for DataTable
using System.Linq;

namespace Overtake
{
    class Decision
    {
        static string[][] carinput;
        public static void Run() {
            // In this example, we will be using the famous Play Tennis example by Tom Mitchell(1998).
            // In Mitchell's example, one would like to infer if a person would play tennis or not
            // based solely on four input variables. Those variables are all categorical, meaning that
            // there is no order between the possible values for the variable (i.e. there is no order
            // relationship between Sunny and Rain, one is not bigger nor smaller than the other, but are 
            // just distinct).

            // Note: this example uses DataTables to represent the input data , but this is not required.
            var example = "Overtake";
            Console.WriteLine(example);

            DataTable data = new DataTable(example);

            data.Columns.Add("Separation", typeof(String));
            data.Columns.Add("Speed", typeof(String));
            data.Columns.Add("OncomingSpeed", typeof(String));
            data.Columns.Add("Result", typeof(String));
            var shuffledInputs = GetInputs(100);
            
            for(int index=0; index < shuffledInputs.Length; index++)
            {
                data.Rows.Add(shuffledInputs[index][0], shuffledInputs[index][1], shuffledInputs[index][2], shuffledInputs[index][3]);
            }
            

            
            

            // In order to try to learn a decision tree, we will first convert this problem to a more simpler
            // representation. Since all variables are categories, it does not matter if they are represented
            // as strings, or numbers, since both are just symbols for the event they represent. Since numbers
            // are more easily representable than text string, we will convert the problem to use a discrete 
            // alphabet through the use of a Accord.Statistics.Filters.Codification codebook.</para>

            // A codebook effectively transforms any distinct possible value for a variable into an integer 
            // symbol. For example, “Sunny” could as well be represented by the integer label 0, “Overcast” 
            // by “1”, Rain by “2”, and the same goes by for the other variables. So:</para>

            // Create a new codification codebook to convert strings into integer symbols
            var codebook = new Codification(data);

            // Translate our training data into integer symbols using our codebook:
            DataTable symbols = codebook.Apply(data);
            int[][] inputs = symbols.ToJagged<int>(new string[] { "Separation", "Speed", "OncomingSpeed", "Result" });
            int[] outputs = symbols.ToArray<int>("Overtake");
            string[] classNames = new string[] { "success", "fail" };

            // For this task, in which we have only categorical variables, the simplest choice 
            // to induce a decision tree is to use the ID3 algorithm by Quinlan. Let’s do it:

            // Create an ID3 algorithm
            var id3learning = new ID3Learning()
            {
                // Now that we already have our learning input/ouput pairs, we should specify our
                // decision tree. We will be trying to build a tree to predict the last column, entitled
                // “PlayTennis”. For this, we will be using the “Outlook”, “Temperature”, “Humidity” and
                // “Wind” as predictors (variables which will we will use for our decision). Since those
                // are categorical, we must specify, at the moment of creation of our tree, the
                // characteristics of each of those variables. So:

                new DecisionVariable("Separation",     150), // 3 possible values (Sunny, overcast, rain)
                new DecisionVariable("Speed", 150), // 3 possible values (Hot, mild, cool)  
                new DecisionVariable("OncomingSpeed",    150), // 2 possible values (High, normal)    
                new DecisionVariable("Result",        2)  // 2 possible values (Weak, strong) 
            };

            // Learn the training instances!
            DecisionTree tree = id3learning.Learn(inputs, outputs);

            // The tree can now be queried for new examples through 
            // its Decide method. For example, we can create a query

            int[] query = codebook.Transform(new[,]
            {
                { "Separation",     "150"  },
                { "Speed", "150"    },
                { "OncomingSpeed",    "150"   },
                { "Result",        "success" }
            });

            // And then predict the label using
            int predicted = tree.Decide(query);

            var answer = codebook.Revert("Overtake", predicted);
            Console.WriteLine("");

            Console.WriteLine(answer);

            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));
            Console.WriteLine($"{error * 100:F10}");

            DecisionSet rules = tree.ToRules();
            var encodedRules = rules.ToString();
            Console.WriteLine(encodedRules);



            Console.ReadKey(); // Keep the window open till a key is pressed
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

            return carinput;
        }


        public bool ApplyRules()
        {
            if (true)
            {
                return true;
            }

            if (false)
            {
                return false;
            }
        }

    }
}