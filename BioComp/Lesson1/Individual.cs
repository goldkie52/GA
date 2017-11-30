using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    class Individual
    {
        private List<Rule> genes;
        private int fitness;
        private const int MutationRate = 20;
        private string fitnessType = "Normal";

        private static Random random = new Random();

        public Individual(int numberOfGenes)
        {
            genes = new List<Rule>();

            for (int i = 0; i < numberOfGenes; i++)
            {
                List<int> ruleAndClass = new List<int>();
                for (int j = 0; j < 8; j++)
                {
                    if (j != 7)
                    {
                        ruleAndClass.Add(random.Next() % 3);
                    }
                    else
                    {
                        ruleAndClass.Add(random.Next() % 2);
                    }
                }
                genes.Add(new Rule(ruleAndClass));
            }

            CalculateFitness();
        }

        public Individual(List<Rule> genes)
        {
            this.genes = genes;
            CalculateFitness();
        }

        public Individual(string genes)
        {
            List <int> stringAsInt = new List<int>();
            for(int i = 0; i < genes.Length; i++)
            {
                stringAsInt.Add(Int32.Parse(genes[i].ToString()));
            }

            List<Rule> rules = new List<Rule>();
            for (int i = 0; i < 10; i++)
            {
                int startIndex = i * 7;
                rules.Add(new Rule(stringAsInt.GetRange(startIndex, 7)));
            }

            this.genes = rules;
        }

        private void CalculateFitness()
        {
            switch (fitnessType)
            {
                case "Normal":
                    CalculateFitnessNormal();
                    break;
                    //case "Squared":
                    //    CalculateFitnessSquared();
                    //    break;
            }
        }

        private void CalculateFitnessNormal()
        {
            int newFitness = 0;
            var dataTest = System.IO.File.ReadLines("C:\\Users\\Kieran\\Desktop\\data2.txt");

            foreach (string s in dataTest)
            {
                if (!s.StartsWith("6"))
                {
                    foreach (Rule gene in genes)
                    {
                        if (RuleMatchesData(s.Substring(0, 6), gene.ToString()))
                        {
                            if (s[7] == gene.Classification.ToString()[0])
                            {
                                newFitness++;
                            }

                            break;
                        }
                    }
                }

            }

            fitness = newFitness;
        }

        private bool RuleMatchesData(string dataInFile, string gene)
        {
            for (int i = 0; i < dataInFile.Length; i++)
            {
                if (dataInFile[i] != gene[i])
                {
                    if (gene[i] != '2')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //private void CalculateFitnessSquared()
        //{
        //    int genesAsBinary = 0;

        //    for (int i = 0; i < genes.Count; i++)
        //    {
        //        int power = (int)Math.Pow(2, i);
        //        genesAsBinary += genes[i]*power;
        //    }

        //    fitness = (int) Math.Pow(genesAsBinary, 2);
        //}

        public void ApplyMutation()
        {
            for (int i = 0; i < genes.Count*6; i++)
            {
                int randomNumber = random.Next() % MutationRate;
                if (randomNumber == 0)
                {
                    genes[i/6].ChangeBit(i%6);
                }
            }

            CalculateFitness();
        }

        public String OutputRules()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Rule gene in Genes)
            {
                foreach (int i in gene.Data)
                {
                    builder.Append(i);
                }

                builder.Append($" {gene.Classification}{Environment.NewLine}");
            }

            return builder.ToString();
        }

        public String GetRuleString()
        {
            string output = "";

            foreach (Rule rule in Genes)
            {
                foreach (int i in rule.Data)
                {
                    output = output + i;
                }

                output = output + rule.Classification;
            }

            return output;
        }

        #region Properies

        public int Fitness
        {
            get { return fitness; }
        }

        public List<Rule> Genes
        {
            get { return genes; }
        }

        #endregion
    }
}
