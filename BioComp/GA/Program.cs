using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class Program
    {
        public static readonly Random Random = new Random();
        public const int PopulationSize = 50; //Must be even!
        public const int GeneSize = 10;
        public const int Generations = 200;
        public const int RuleSize = 6;
        public const int MutationRate = 50; //Gives a one in MutationRate chance of mutating a bit
        public static readonly List<string> FileAsList = ReadFile();

        static void Main(string[] args)
        {
            System.IO.File.WriteAllText("C:\\Users\\Kieran\\Desktop\\Output.txt", "Generation number\tBest Fitness\tAverage Fitness");

            List<Individual> population = new List<Individual>();

            for (int i = 0; i < PopulationSize; i++)
            {
                population.Add(new Individual());
            }

            OutputToFile(population, 0);

            for (int i = 0; i < Generations; i++)
            {
                Individual bestIndividual = FindBestIndividual(population);
                Console.WriteLine($"{i+1}: {bestIndividual.Fitness}");

                //Run the selection process
                population = Selection(population);

                //Calculate the worst Individual
                int worstIndividual = 0;
                for (int j = 0; j < population.Count; j++)
                {
                    if (population[j].Fitness < population[worstIndividual].Fitness)
                    {
                        worstIndividual = j;
                    }
                }

                population[worstIndividual] = bestIndividual;

                OutputToFile(population, i + 1);
                if (i == Generations-1|| population.Any(individual => individual.Fitness == 64))
                {
                    Individual highestFitnessIndividual = FindBestIndividual(population);
                    Console.WriteLine($"{highestFitnessIndividual}Fitness = {highestFitnessIndividual.Fitness}");
                    break;
                }
            }

            Console.In.Read();
        }

        /// <summary>
        /// Outputs the population to the output file in the correct format
        /// </summary>
        /// <param name="population"></param>
        /// <param name="generationNumber"></param>
        private static void OutputToFile(List<Individual> population, int generationNumber)
        {
            int bestFitness = 0;
            int totalFitness = 0;

            foreach (Individual individual in population)
            {
                if (individual.Fitness > bestFitness)
                {
                    bestFitness = individual.Fitness;
                }

                totalFitness += individual.Fitness;
            }

            string textToOutput = System.IO.File.ReadAllText("C:\\Users\\Kieran\\Desktop\\Output.txt");
            System.IO.File.WriteAllText("C:\\Users\\Kieran\\Desktop\\Output.txt", textToOutput + Environment.NewLine + generationNumber + "\t" + bestFitness + "\t" + totalFitness / population.Count);
        }

        /// <summary>
        /// Calculate the best Individual
        /// </summary>
        private static Individual FindBestIndividual(List<Individual> population)
        {
            Individual bestIndividual = population[0];
            foreach (Individual individual in population)
            {
                if (individual.Fitness > bestIndividual.Fitness)
                {
                    bestIndividual = individual;
                }
            }

            return bestIndividual;
        }

        private static List<Individual> Selection(List<Individual> population)
        {
            //Survial of the fittest
            List<Individual> winners = Tornament(population);

            //Crossover
            winners = Crossover(winners);

            //Mutation
            foreach (Individual child in winners)
            {
                child.ApplyMutation();
            }

            return winners;
        }

        private static List<Individual> Tornament(List<Individual> population)
        {
            List<Individual> winners = new List<Individual>();
            for (int i = 0; i < population.Count; i++)
            {

                int firstContestant = Random.Next(0, population.Count);
                int secondContestant = Random.Next(0, population.Count);


                if (population[firstContestant].Fitness >= population[secondContestant].Fitness)
                {
                    winners.Add(population[firstContestant]);
                }
                else
                {
                    winners.Add(population[secondContestant]);
                }
            }

            return winners;
        }

        private static List<Individual> Crossover(List<Individual> winners)
        {
            List<Individual> offspring = new List<Individual>();
            for (int i = 0; i < winners.Count; i = i + 2)
            {
                string firstChildGenes = winners[i].RuleString;
                string secoundChildGenes = winners[i+1].RuleString;
                int crossOverPoint = Random.Next(0, firstChildGenes.Length);

                string firstChildTail = firstChildGenes.Substring(crossOverPoint);

                firstChildGenes = firstChildGenes.Substring(0, crossOverPoint) + secoundChildGenes.Substring(crossOverPoint);
                secoundChildGenes = secoundChildGenes.Substring(0, crossOverPoint) + firstChildTail;


                offspring.Add(new Individual(firstChildGenes));
                offspring.Add(new Individual(secoundChildGenes));
            }

            return offspring;
        }

        private static List<string> ReadFile()
        {
            List<string> outputList = System.IO.File.ReadLines("C:\\Users\\Kieran\\Desktop\\Bio comp\\data2.txt").ToList();
            string firstLine = outputList[0];
            outputList.RemoveAt(0);
            return outputList;
        }
    }
}
