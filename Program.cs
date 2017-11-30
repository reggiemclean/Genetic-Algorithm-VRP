using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1 {

  
    class Program {

        const int GA_POPSIZE = 100;
        const int GA_MAXITERATIONS = 1000;
        const double GA_ELITISMRATE = 0.1;
        const double GA_MUTATIONRATE = 0.2;
        const double GA_CROSSOVERRATE = 0.9;
        const int num_customers = 101;

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

            List<int> check = new List<int>();
            for (int y = 0; y < num_customers; y++)
                check.Add(ga_population[y, 0]);
            check.Sort();
            foreach (int x in check)
                Console.WriteLine(x);

            Console.WriteLine("Number of elements in Check: " + check.Count);

            
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
	}
}