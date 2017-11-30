using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    class Program
    {
        private static Random random = new Random();

        static void Main(string[] args)
        {
            System.IO.File.WriteAllText("C:\\Users\\Kieran\\Desktop\\Output.txt", "Generation number\tBest Fitness\tAverage Fitness");
            int populationSize = 50; // Must be even!
            int geneSize = 10;
            int generations = 300;

            List<Individual> population = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Individual(geneSize));
            }

            OutputPopulation(population, 0, false);

            for (int i = 0; i < generations; i++)
            {
                Individual bestIndividual = population[0];
                foreach (Individual individual in population)
                {
                    if (individual.Fitness > bestIndividual.Fitness)
                    {
                        bestIndividual = individual;
                    }
                }

                population = Selection(population, populationSize, geneSize);

                int worstIndividual = 0;
                for (int j = 0; j < population.Count; j++)
                {
                    if (population[j].Fitness < population[worstIndividual].Fitness)
                    {
                        worstIndividual = j;
                    }
                }

                population[worstIndividual] = bestIndividual;

                if (i != generations-1)
                {
                    OutputPopulation(population, i + 1, false);
                }
                else
                {
                    OutputPopulation(population, i + 1);
                }

                if (population.Max(ind => ind.Fitness) == 64)
                {
                    Console.WriteLine("Please check the file");
                    Console.WriteLine($"The best individual is:\n{bestIndividual.OutputRules()}");

                    break;
                }
            }

            Console.In.ReadLine();
        }

        private static void OutputPopulation(IList<Individual> population, int generationNumber, bool consoleOutput = true)
        {
            int individualNumber = 0;

            int bestFitness = 0;
            int totalFitness = 0;
            int childWithBestFitness = 0;

            if (consoleOutput)
            {
                Console.WriteLine("Current Population:");
            }

            foreach (Individual individual in population)
            {
                if (consoleOutput)
                {
                    Console.Write("{0}: ", individualNumber);
                    foreach (Rule gene in individual.Genes)
                    {
                        Console.Write(gene);
                    }
                    Console.WriteLine(" = {0}", individual.Fitness);
                }


                if (individual.Fitness > bestFitness)
                {
                    bestFitness = individual.Fitness;
                    childWithBestFitness = individualNumber;
                }

                totalFitness += individual.Fitness;
                individualNumber++;
            }

            string textToOutput = System.IO.File.ReadAllText("C:\\Users\\Kieran\\Desktop\\Output.txt");
            System.IO.File.WriteAllText("C:\\Users\\Kieran\\Desktop\\Output.txt", textToOutput + Environment.NewLine + generationNumber + "\t" + bestFitness + "\t" + totalFitness / population.Count);

            if (consoleOutput)
            {
                Console.WriteLine("The best fitness is {0} with {1}{2}", childWithBestFitness, bestFitness, Environment.NewLine);
            }
        }

        private static List<Individual> Selection(IList<Individual> population, int populationSize, int numberOfGenes)
        {
            //Tournament
            List<Individual> winners = Tournament(population);

            //Crossover
            List<Individual> offspring = CrossOverVersion2(winners, random, numberOfGenes);

            //Mutation
            foreach (Individual child in offspring)
            {
                child.ApplyMutation();
            }

            return offspring;
        }

        private static List<Individual> Tournament(IList<Individual> population)
        {
            List<Individual> winners = new List<Individual>();
            for (int i = 0; i < population.Count; i++)
            {

                int firstContestant = random.Next(0, population.Count);
                int secondContestant = random.Next(0, population.Count);


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

        private static List<Individual> Roulette(IList<Individual> population)
        {
            List<Individual> winners = new List<Individual>();
            int totalFitness = population.Sum(i => i.Fitness);

            foreach (Individual individual in population)
            {
                int selectionPoint = random.Next(totalFitness);
                int runningTotal = 0;
                int i;

                for (i = 0; runningTotal <= selectionPoint; i++)
                {
                    runningTotal += population[i].Fitness;
                }

                winners.Add(population[i - 1]);
            }

            return winners;
        }

        private static List<Individual> CrossOverVersion2(List<Individual> winners, Random random, int numberOfGenes)
        {
            List<Individual> offspring = new List<Individual>();
            for (int i = 0; i < winners.Count; i = i + 2)
            {
                string firstChildGenes = winners[random.Next(0,winners.Count)].GetRuleString();
                string secoundChildGenes = winners[random.Next(0, winners.Count)].GetRuleString();
                int crossOverPoint = random.Next(0, firstChildGenes.Length);

                string firstChildTail = firstChildGenes.Substring(crossOverPoint);

                firstChildGenes = firstChildGenes.Substring(0, crossOverPoint) +
                                  secoundChildGenes.Substring(crossOverPoint);
                secoundChildGenes = secoundChildGenes.Substring(0, crossOverPoint) + firstChildTail;

                offspring.Add(new Individual(firstChildGenes));
                offspring.Add(new Individual(secoundChildGenes));
            }

            return offspring;
        }

        private static List<Individual> CrossOver(List<Individual> winners, Random random, int numberOfGenes)
        {
            List<Individual> offspring = new List<Individual>();
            for (int i = 0; i < winners.Count; i = i + 2)
            {
                int crossOverPoint = random.Next(numberOfGenes);
                List<Rule> firstChildGenes = new List<Rule>();
                List<Rule> secoundChildGenes = new List<Rule>();

                for (int j = 0; j < crossOverPoint; j++)
                {
                    firstChildGenes.Add(winners[i].Genes[j]);
                    secoundChildGenes.Add(winners[i + 1].Genes[j]);
                }
                for (int j = crossOverPoint; j < numberOfGenes; j++)
                {
                    firstChildGenes.Add(winners[i + 1].Genes[j]);
                    secoundChildGenes.Add(winners[i].Genes[j]);
                }

                offspring.Add(new Individual(firstChildGenes));
                offspring.Add(new Individual(secoundChildGenes));
            }

            return offspring;
        }

        private static long LongRandom(long min, long max)
        {
            long result = random.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | random.Next((Int32)min, (Int32)max);
            return result;
        }
    }
}
