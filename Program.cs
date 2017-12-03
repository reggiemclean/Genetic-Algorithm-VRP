using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace Assignment1 {

    class NumWithCount
    {
        public int Element { get; set; }
        public int Count { get; set; }
    }

    class CustomerInfo
    {
        public int CustomerNumber { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Demand { get; set; }
        public int ReadyTime { get; set; }
        public int DueDate { get; set; }
        public int ServiceTime { get; set; }

        public override string ToString()
        {
            return CustomerNumber + " " + XCoordinate + " " + YCoordinate + " " + Demand + " " + ReadyTime + " " + DueDate + " " + ServiceTime;
        }
    }

    
    class Program {

        const int GA_POPSIZE = 100;
        const double GA_ELITISMRATE = 0.1;
        const double GA_MUTATIONRATE = 0.2;
        const double GA_CROSSOVERRATE = 0.9;
        const int num_customers = 101;
        const int MAX_CAPACITY = 100;
        public static List<CustomerInfo> customerList = new List<CustomerInfo>();
        public static double [] fitnessPopulation = new double[GA_POPSIZE];
        public static int[] NumberOfTrucks = new int[GA_POPSIZE];
        public static int[,] PotentialParents = new int[num_customers, GA_POPSIZE];
        public static int[,] NextPopulation = new int[num_customers, GA_POPSIZE];
        public static double[] fitnessNextPopulation = new double[GA_POPSIZE];
        public static double GlobalBestFitness = 999999999; // the global variable to keep track of the global best distance that has been found
        public static int BestSolutionIndex = 0; 



        static void Main(string[] args)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int[,] ga_population = new int[num_customers, GA_POPSIZE];
            ga_population = InitPop(GA_POPSIZE);
            ReadFile();
            fitnessPopulation = CalculateFitness(ga_population, fitnessPopulation);
            for (int gen = 1 ; gen <= 100; gen++)
			{
                TournamentSelection(2, ga_population);
                //OnePointCrossover(rand.Next(0,num_customers));
                UniformOrderCrossover(ga_population);
                //CycleCrossover();
                Mutation();
                Elitism(ga_population);
                fitnessNextPopulation = CalculateFitness(NextPopulation, fitnessNextPopulation);
                Array.Copy(NextPopulation, ga_population, NextPopulation.Length);
            }

            
            Console.WriteLine("Lowest Distance found: " + fitnessNextPopulation.Min());
            for (int x = 0; x < GA_POPSIZE; x++)
                if (fitnessNextPopulation[x] == fitnessNextPopulation.Min())
                {
                    Console.WriteLine("This is route #: " + x);
                    PrintRoute(x, ga_population);
                }

         Console.ReadKey();
		}

		static int[,] InitPop(int pop_size)
		{
            int[,] init_population = new int[num_customers, pop_size];
            List<int> RandomNumbers = new List<int>();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int customer = rnd.Next(1, num_customers + 1);
            for (int x = 0; x < pop_size ; x++)
            {
                for (int y= 0; y < num_customers; y++)
                {
                    while (RandomNumbers.Contains(customer))
                    {
                        customer = rnd.Next(0, num_customers);
                    }
                        init_population[y, x] = customer;
                        RandomNumbers.Add(customer);
                }
                RandomNumbers.Clear();
            }
			return init_population;
		}

        static void UniformOrderCrossover (int [,] pop)
        {
            Random rand = new Random();
            for (int x = 0; x < GA_POPSIZE-1; x+=2)
            {
                if (rand.NextDouble() < GA_CROSSOVERRATE)
                {
                    for (int y = 0; y < num_customers; y++)
                    {
                        if (rand.NextDouble() < 0.5)
                        {
                            //swap bits for next gen
                            NextPopulation[y, x] = PotentialParents[y, x + 1];
                            NextPopulation[y, x + 1] = PotentialParents[y, x];
                        }
                        else
                        {
                            // keep bit same in next gen
                            NextPopulation[y, x] = PotentialParents[y, x];
                            NextPopulation[y, x + 1] = PotentialParents[y, x + 1];
                        }
                    }
                }
            }
        }

        static void CycleCrossover()
        {
            Random rand = new Random();
            int numCycles = rand.Next(0, 80);
            int indexForCross = 0;
            List<int> cyclePoints = new List<int>();
            int count = 0;
            while (cyclePoints.Count < numCycles)
            {   
                while (cyclePoints.Contains(indexForCross))
                {
                    indexForCross = rand.Next(0, num_customers);
                }
                cyclePoints.Add(indexForCross);
            }
            cyclePoints.Sort();

            for (int x = 0; x < GA_POPSIZE-1; x+=2)
            {
                if (rand.NextDouble() < GA_CROSSOVERRATE)
                { 
                    for (int y = 0; y < num_customers; y++)
                    {
                        if (y == cyclePoints[count] && cyclePoints.Count > 0)
                        {
                            //don't cross
                            cyclePoints.RemoveAt(0);
                            NextPopulation[y, x] = PotentialParents[y, x];
                            NextPopulation[y, x + 1] = PotentialParents[y, x + 1];
                            count++;
                        }
                        else
                        {
                            NextPopulation[y, x] = PotentialParents[y, x + 1];
                            NextPopulation[y, x + 1] = PotentialParents[y, x];
                            //cross 
                        }
                    }
                }
            }
        }

        static void OnePointCrossover (int IndexForCross) // *************NEED TO CHECK FOR VALID ROUTES AFTER CROSS or should I just make fitness super big ??***********
		{
            int[] ChildA = new int[num_customers];
            int[] ChildB = new int[num_customers];
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            //*******************************************************************
            for (int x = 0; x <= GA_POPSIZE - 1; x+=2)
            {
                if (rand.NextDouble() < GA_CROSSOVERRATE)
                {
                    for (int y = 0; y < num_customers; y++)
                    {
                        if (y < IndexForCross)
                        {
                            ChildA[y] = PotentialParents[y, x];
                            ChildB[y] = PotentialParents[y, x + 1];
                        }
                        else
                        {
                            ChildA[y] = PotentialParents[y, x + 1];
                            ChildB[y] = PotentialParents[y, x];
                        }
                    }


                    for (int b = 0; b < num_customers; b++)
                    {
                        NextPopulation[b, x] = ChildA[b];
                        NextPopulation[b, x + 1] = ChildB[b];
                    }
                }
            }
        }

        static double[] CalculateFitness(int[,] pop, double[] fitnessPopulation)
        {
            //Single Objective - Evaluating the distance travelled
            double CalcFitness = 0;
            int capacity = 0;
            int numTrucks = 1;
            int FirstCity = 0;
            List<int> duplicates = new List<int>();
            for (int x = 0; x < GA_POPSIZE; x++)
            {
                capacity = customerList[pop[0, x]].Demand;
                for (int y = 0; y < num_customers - 1; y++)
                {
                    duplicates.Add(pop[y, x]);
                    if (capacity + customerList[pop[y + 1, x]].Demand <= MAX_CAPACITY)
                    {
                        CalcFitness += Math.Sqrt(Math.Pow(Math.Abs(customerList[pop[y, x]].XCoordinate - customerList[pop[y + 1, x]].XCoordinate), 2) + Math.Pow(Math.Abs(customerList[pop[y, x]].YCoordinate - customerList[pop[y + 1, x]].YCoordinate), 2));
                        capacity += customerList[pop[y + 1, x]].Demand;
                    }
                    else
                    {
                        CalcFitness += Math.Sqrt(Math.Pow(Math.Abs(customerList[pop[y, x]].XCoordinate - customerList[pop[FirstCity, x]].XCoordinate), 2) + Math.Pow(Math.Abs(customerList[pop[y, x]].YCoordinate - customerList[pop[FirstCity, x]].YCoordinate), 2));
                        // link last truck to go back to start
                        FirstCity = y + 1;
                        capacity = customerList[pop[y + 1, x]].Demand; // next customer cannot be added to this truck, add a new truck and reset capacity to new customer
                        numTrucks++;
                    }
                }

                fitnessPopulation[x] = CalcFitness;
                int check = duplicates.Distinct().Count();
                Console.WriteLine(check);
                //PrintRoute(x, pop);
                CheckRoute(x, pop);
                Console.ReadKey();
                if (CalcFitness < GlobalBestFitness)
                {
                    GlobalBestFitness = CalcFitness;
                    BestSolutionIndex = x;
                }

                NumberOfTrucks[x] = numTrucks;
                numTrucks = 1;
                CalcFitness = 0;

            }
            return fitnessPopulation;
        }

        static void TournamentSelection(int k, int [,] population) // maybe loop until k items have been picked, check item in list and store best number and fitness
        {
            int BestParentFound = 0;
            double BestLocalFitness = 9999999999;
            Random rand = new Random(Guid.NewGuid().GetHashCode()); // trying to generate "better" random numbers
            int PopCount = 0;
            int ParentCount = 0;
            List<int> ParentLocations = new List<int>();
            BestParentFound = rand.Next(0, GA_POPSIZE);
            while (PopCount < GA_POPSIZE)
            {
                while (ParentCount < k) //adds k parents to a list of potential parents
                {
                    ParentLocations.Add(rand.Next(0, GA_POPSIZE));
                    ParentCount++;
                }

                foreach (int parent in ParentLocations) // for each of the parents found and placed in list, checks if that parent is the best found parent
                {
                    if (fitnessPopulation[parent] < BestLocalFitness)
                    {
                        BestLocalFitness = fitnessPopulation[parent];
                        BestParentFound = parent;
                    }
                }
                //add best parent to PotentialParents
                for (int x = 0; x < num_customers; x++)
                    PotentialParents[x, PopCount] = population[x, BestParentFound];
                PopCount++;
                ParentCount = 0;
                BestLocalFitness = 999999999999;
                ParentLocations.Clear();
            }
        }

        static void Elitism(int [,] pop)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int NumberToKeep = rand.Next(0, 11);
            while (NumberToKeep > 0)
            {
                int ChromosomeNumber = rand.Next(0, GA_POPSIZE);
                for (int x = 0; x < num_customers; x++)
                {
                    NextPopulation[x, ChromosomeNumber] = pop[x, ChromosomeNumber];
                }
                NumberToKeep--;
            }
        }

        static void Mutation ()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            List<int> toBeSwapped = new List<int>();
            int pointA, pointB;
            int small, large;
            for (int x = 0; x < GA_POPSIZE; x++)
            {
                if (rand.NextDouble() > 0.9) // mutation should happen if value is between 0.9 and 1
                {
                    pointA = rand.Next(num_customers);
                    Thread.Sleep(10);
                    pointB = rand.Next(num_customers);
                    if (pointA > pointB)
                    {
                        small = pointB;
                        large = pointA;
                    }
                    else
                    {
                        small = pointA;
                        large = pointB;
                    }
                    for (int y = 0; y < num_customers; y++)
                    {
                        //inversion mutation = two points picked, chromosome is reversed within that range.
                        if (y > small && y < large)
                            toBeSwapped.Add(NextPopulation[y, x]);
                    }
                    toBeSwapped.Reverse();
                    for (int g = 0; g <num_customers; g++)
                    {
                        if (g > small && g < large)
                        {
                            NextPopulation[g, x] = toBeSwapped.First();
                            toBeSwapped.RemoveAt(0);
                        }
                    }
                }
            }
        }

        static void ReadFile()
        {
            var lines = System.IO.File.ReadAllLines("C:\\Users\\Reggie\\Desktop\\data\\C107_200.csv");
            foreach (string customer in lines)
            {
                var values = customer.Split(',');
                customerList.Add(new CustomerInfo()
                {
                    CustomerNumber = Convert.ToInt32(values[0]),
                    XCoordinate = Convert.ToInt32(values[1]),
                    YCoordinate = Convert.ToInt32(values[2]),
                    Demand = Convert.ToInt32(values[3]),
                    ReadyTime = Convert.ToInt32(values[4]),
                    DueDate = Convert.ToInt32(values[5]),
                    ServiceTime = Convert.ToInt32(values[6])
                });
            }
        }

        static void WriteFile (int [,] pop)
        {
            using (StreamWriter outfile = new StreamWriter(@"C:\\Users\\Reggie\\Desktop\\output.csv"))
            {
                for (int x = 0; x < GA_POPSIZE; x++)
                {
                    string content = "";
                    for (int y = 0; y < num_customers; y++)
                    {
                        content += pop[y, x].ToString("0.00") + ";";
                    }
                    outfile.WriteLine(content);
                }

            }
        }

        static void PrintRoute (int index, int [,] pop)
        {
            int trucks = 1;
            int currentCapacity = 0;
            int count = 0;
           
            while (count < (num_customers-1))
            {
                Console.WriteLine("Truck #: " + trucks + "'s Route is: ");
                while (count <= (num_customers - 1)  && currentCapacity + customerList[pop[count, BestSolutionIndex]].Demand <= MAX_CAPACITY)
                {
                    currentCapacity += customerList[ pop[count, BestSolutionIndex] ].Demand;
                    Console.Write(pop[count, BestSolutionIndex] + " ");
                    count++;
                }
                Console.WriteLine();
                currentCapacity = 0;
                trucks++;
            }
        }

        static int CheckRoute (int index, int [,] pop)
        {
            int[] buckets = new int[num_customers];
            for (int i = 0; i < num_customers; i++)
            {
                buckets[pop[i, index]]++;
            }
            List<int> check = new List<int>();
            

            return 2;
        }
    }
}