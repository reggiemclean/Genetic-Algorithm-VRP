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

        public int GlobalBestFitness = 999999999; // the global variable to keep track of the global best distance that has been found
        static void Main(string[] args)
		{

            List<ReadInFile();
            List<int> ga_population = new List<int>();
            ga_population = InitPop(100);
			for (int gen = 1 ; gen <= 100; gen++)
			{
                // apply genetic operators
                // do crossover
                OnePointCrossover(ga_population, 4);
				// do mutation
				// evaluate fitness of population individuals
				// select new populations using some strategy

			}
		}

		static List<int> InitPop(int pop_size)
		{
			List<int> init_population = new List<int>();
			// do something to initialize the population
			return init_population;
		}

        //One point crossover
        static void OnePointCrossover(List<int> Parents, int IndexForCross)
		{

            //*******************************************************************
            List<int> NewChromosome = new List<int>(); // list that will contain the results of the crossover
            List<int> TempChromosome = new List<int>(); // temporary list to contain original values of one parent for swap






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
        static void ReadInFile()
        {


        }
	}
}