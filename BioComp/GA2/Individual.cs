using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2
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

        public Individual(Individual oldIndividual)
        {
            foreach (Rule oldIndividualRule in oldIndividual.Genes)
            {
                this.genes.Add(new Rule(oldIndividualRule));
            }
            this.fitness = oldIndividual.Fitness;
        }

        public Individual(string genes)
        {
            List<string> genesAsString = genes.Split('I').ToList();
            genesAsString.RemoveAt(genesAsString.Count-1);
            foreach (string geneString in genesAsString)
            {
                this.genes.Add(new Rule(geneString));
            }

            CalculateFitness();
        }

        public Individual(List<Rule> genes)
        {
            this.genes = genes;

            CalculateFitness();
        }

        #endregion

        #region Public Methods

        public void ApplyMutation()
        {
            for (int i = 0; i < genes.Count; i++)
            {
                int randomNumber = Program.Random.Next(0, Program.MutationRate); //% Program.MutationRate;
                if (randomNumber == 0)
                {
                    genes[i].Mutate();
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
                    foreach (Chromosone chromosone in rule.Data)
                    {
                        output = output + chromosone;
                    }

                    output = output + rule.Classification + "I";
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
                foreach (Rule rule in genes)
                {
                    if (RuleMatchesData(s, rule))
                    {
                        if (s[s.Length-1].ToString() == rule.Classification.ToString())
                        {
                            newFitness++;
                        }

                        break;
                    }
                }
            }

            this.fitness = newFitness;
        }

        private bool RuleMatchesData(String dataInFile, Rule rule)
        {
            string[] fileData = dataInFile.Split(' ');

            for (int i = 0; i < fileData.Length-1; i++)
            {
                float dataAtIAsFloat = float.Parse(fileData[i]);
                if (!rule.Data[i].VariableBetweenBounds(dataAtIAsFloat))
                {
                    return false;
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
