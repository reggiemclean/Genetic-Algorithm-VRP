using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1 {
	
	class population{
		public int customerNum {get; set;}
        public int demand {get; set;}
	}

	class program {

		static void Main(string[] args)
		{
			population[,] ga_population;
			ga_population = initPop(100);
			int changeForGit = 0;
			for (int gen = 1 ; gen < 100; gen++)
			{
				// apply genetic operators
				// do crossover
				// do mutation
				// evaluate fitness of population individuals
				// select new populations using some strategy

			}
		}

		static population[,] initPop(int pop_size)
		{
			population[,] init_population = new population[pop_size,101];
			// do something to initialize the population
			return init_population;
		}

		static population[] crossover()
		{
			population[] someChromosome = new population[101];
			// do some crossover

			// can use code from coding theory assignment to find random string
			return someChromosome;
		}

	}
}