using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GA2
{
    class Program
    {
        public static readonly Random Random = new Random();
        public const int PopulationSize = 150; //Must be even!
        public const int GeneSize = 5;
        public const int Generations = 1000;
        public const int RuleSize = 6;
        public const int MutationRate = 10; //Gives a one in MutationRate chance of mutating a bit
        public static readonly List<string> FileAsList = ReadFile();
        public static readonly List<string> TestFileAsList = ReadFile(true);
        public static DateTime TimeStarted = DateTime.Now;

        static void Main(string[] args)
        {
            Console.WriteLine($"GeneSize = {GeneSize}");
            for (int k = 0; k < 1; k++)
            {
                System.IO.File.WriteAllText($"C:\\Users\\Kieran\\Desktop\\Output{TimeStarted:HHmmss}.txt", "Generation number\tBest Fitness\tAverage Fitness\tBest Against Test Data");

                List<Individual> population = new List<Individual>();

                for (int i = 0; i < PopulationSize; i++)
                {
                    population.Add(new Individual());
                }

                OutputToFile(population, 0, "", k);

                for (int i = 0; i < Generations; i++)
                {
                    Individual bestIndividual = new Individual(FindBestIndividual(population));
                    string bestAgainstTestData = "";
                    if (i % 10 == 0)
                    {
                        bestAgainstTestData = TestAgainstRealData(bestIndividual);
                    }

                    //Run the selection process
                    population = Selection(population);

                    //Calculate the worst Individual
                    int worstIndividual = 0;
                    for (int j = 0; j < population.Count; j++)
                    {
                        if (population[j].Fitness <= population[worstIndividual].Fitness)
                        {
                            worstIndividual = j;
                        }
                    }

                    population[worstIndividual] = bestIndividual;

                    if (i == Generations - 1 || population.Any(individual => individual.Fitness == 1000))
                    {
                        Individual highestFitnessIndividual = FindBestIndividual(population);
                        bestAgainstTestData = TestAgainstRealData(bestIndividual);
                        Console.WriteLine($"{highestFitnessIndividual}Fitness = {highestFitnessIndividual.Fitness}");
                        OutputToFile(population, i + 1, bestAgainstTestData, k);
                        break;
                    }

                    OutputToFile(population, i + 1, bestAgainstTestData, k);
                }

                Console.WriteLine("Finished loop " + k);
            }



            Console.In.Read();
        }

        /// <summary>
        /// Outputs the population to the output file in the correct format
        /// </summary>
        private static void OutputToFile(List<Individual> population, int generationNumber, string bestAgainstTestData, int loopNumber)
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

            System.IO.File.AppendAllText($"C:\\Users\\Kieran\\Desktop\\Output{TimeStarted:HHmmss}.txt", Environment.NewLine + generationNumber + "\t" + bestFitness + "\t" + totalFitness / population.Count + "\t" + bestAgainstTestData);
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
                    winners.Add(new Individual(population[firstContestant]));
                }
                else
                {
                    winners.Add(new Individual(population[secondContestant]));
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
                string secoundChildGenes = winners[i + 1].RuleString;
                int crossOverPoint = Random.Next(0, firstChildGenes.Length);

                string firstChildTail = firstChildGenes.Substring(crossOverPoint);

                firstChildGenes = firstChildGenes.Substring(0, crossOverPoint) + secoundChildGenes.Substring(crossOverPoint);
                secoundChildGenes = secoundChildGenes.Substring(0, crossOverPoint) + firstChildTail;


                offspring.Add(new Individual(firstChildGenes));
                offspring.Add(new Individual(secoundChildGenes));
            }

            return offspring;
        }

        private static List<string> ReadFile(bool returnTestData = false)
        {
            List<string> outputList;
            if (returnTestData)
            {
                outputList = System.IO.File.ReadLines("C:\\Users\\Kieran\\Desktop\\Bio comp\\data3.txt").Skip(1001).ToList();
            }
            else
            {
                outputList = System.IO.File.ReadLines("C:\\Users\\Kieran\\Desktop\\Bio comp\\data3.txt").Take(1001).ToList();
                outputList.RemoveAt(0);
            }
            return outputList;
        }

        private static string TestAgainstRealData(Individual bestIndividual)
        {
            int fitness = 0;

            foreach (string s in TestFileAsList)
            {
                foreach (Rule rule in bestIndividual.Genes)
                {
                    if (RuleMatchesData(s, rule))
                    {
                        if (s[s.Length - 1].ToString() == rule.Classification.ToString())
                        {
                            fitness++;
                        }

                        break;
                    }
                }
            }

            return fitness.ToString();
        }

        private static bool RuleMatchesData(String dataInFile, Rule rule)
        {
            string[] fileData = dataInFile.Split(' ');

            for (int i = 0; i < fileData.Length - 1; i++)
            {
                float dataAtIAsFloat = float.Parse(fileData[i]);
                if (!rule.Data[i].VariableBetweenBounds(dataAtIAsFloat))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
