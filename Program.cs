using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1 {

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
        const int GA_MAXITERATIONS = 1000;
        const double GA_ELITISMRATE = 0.1;
        const double GA_MUTATIONRATE = 0.2;
        const double GA_CROSSOVERRATE = 0.9;
        const int num_customers = 101;
        public static List<CustomerInfo> customerList = new List<CustomerInfo>();
        public int GlobalBestFitness = 999999999; // the global variable to keep track of the global best distance that has been found
        static void Main(string[] args)
        { 
            int[,] ga_population = new int[num_customers, GA_POPSIZE];
            ga_population = InitPop(GA_POPSIZE);
            /*for (int gen = 1 ; gen <= 100; gen++)
			{
                // apply genetic operators
                // do parent selection
                // do crossover
                OnePointCrossover(ga_population, 4);
				// do mutation
				// evaluate fitness of population individuals
                // elitism
				// new generation

			}*/
            ReadFile();
            foreach (CustomerInfo x in customerList)
                Console.WriteLine(x);
            Console.ReadKey();
		}

		static int[,] InitPop(int pop_size)
		{
            int[,] init_population = new int[num_customers, pop_size];
            List<int> RandomNumbers = new List<int>();
            Random rnd = new Random();
            int customer = rnd.Next(1, num_customers + 1);
			// do something to initialize the population
            for (int x = 0; x < pop_size ; x++)
            {
                for (int y= 0; y < num_customers; y++)
                {
                    while (RandomNumbers.Contains(customer))
                    {
                        customer = rnd.Next(1, num_customers + 1);
                    }
                        init_population[y, x] = customer;
                        RandomNumbers.Add(customer);
                }
                RandomNumbers.Clear();
            }
			return init_population;
		}

        //One point crossover
        static void OnePointCrossover (int IndexForCross, int [,] population)
		{

            //*******************************************************************
            for (int x = 0; x < num_customers; x++)
            {


            }






            //*******************************************************************

		}
        //End of one point crossover

        static int CalculateFitness(List<int> pop)
        {
            int fitness = 0;
            foreach (int x in pop)
            {
                fitness = fitness + x;
            }
            return fitness;
        }
        static void TournamentSelection(int k = 2)
        {

        }

        static void ReadFile()
        {
            var lines = System.IO.File.ReadAllLines("C:\\Users\\Reggie\\Desktop\\data\\C101_200.csv");
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
	}
}