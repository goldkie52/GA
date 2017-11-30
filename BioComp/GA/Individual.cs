using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class Individual
    {
        private List<Rule> genes = new List<Rule>();
        private int fitness;

        #region Constructors

        public Individual()
        {
            for (int i = 0; i < Program.GeneSize; i++)
            {
                genes.Add(new Rule());
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
            int expectedLengthOfGenes = (Program.RuleSize + 1) * Program.GeneSize;
            if (genes.Length == expectedLengthOfGenes)
            {
                for (int i = 0; i < Program.GeneSize; i++)
                {
                    string geneSubStringForRule = genes.Substring(i * (Program.RuleSize + 1), Program.RuleSize + 1);
                    this.genes.Add(new Rule(geneSubStringForRule));
                }
            }
            else
            {
                throw new ArgumentException($"genes string legnth was incorrect. Expected {expectedLengthOfGenes} but recieved {genes.Length}");
            }

            CalculateFitness();
        }

        #endregion

        #region Public Methods

        public void ApplyMutation()
        {
            for (int i = 0; i < RuleString.Length; i++)
            {
                int randomNumber = Program.Random.Next() % Program.MutationRate;
                if (randomNumber == 0)
                {
                    int geneIndex = i / (Program.RuleSize + 1);
                    int chromosoneIndex = i % (Program.RuleSize+1);
                    genes[geneIndex].MutateBit(chromosoneIndex);
                }
            }

            CalculateFitness();
        }

        public String RuleString
        {
            get
            {
                string output = "";

                foreach (Rule rule in genes)
                {
                    foreach (int i in rule.Data)
                    {
                        output = output + i;
                    }

                    output = output + rule.Classification;
                }

                return output;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Rule gene in genes)
            {
                builder.AppendLine($"{gene} {gene.Classification}");
            }

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        private void CalculateFitness()
        {
            int newFitness = 0;

            foreach (string s in Program.FileAsList)
            {
                string dataFileCondition = s.Substring(0, Program.RuleSize);

                foreach (Rule rule in genes)
                {
                    if (RuleMatchesData(dataFileCondition, rule.ToString()))
                    {
                        string clasifcationInFile = s[Program.RuleSize + 1].ToString();
                        if (clasifcationInFile == rule.Classification.ToString())
                        {
                            newFitness++;
                        }

                        break;
                    }
                }
            }

            this.fitness = newFitness;
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

        #endregion

        #region Properties

        public List<Rule> Genes
        {
            get { return genes; }
        }

        public int Fitness
        {
            get { return fitness; }
        }

        #endregion
    }
}
