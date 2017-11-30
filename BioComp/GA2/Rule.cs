using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2
{
    class Rule
    {
        private List<Chromosone> data = new List<Chromosone>();
        private int classification = new int();

        #region Constructors

        public Rule()
        {
            for (int i = 0; i < Program.RuleSize; i++)
            {
                data.Add(new Chromosone());
            }

            classification = Program.Random.Next(0, 2);
            if (classification == 2)
            {
                throw new InvalidOperationException("Classification bit trying to be set to invalid number");
            }

        }

        public Rule(string dataAndClass)
        {
            List<string> chromesonesAsString = ChromesonesAsString(dataAndClass);

            for (var index = 0; index < chromesonesAsString.Count - 1; index++)
            {
                string chromesoneString = chromesonesAsString[index];
                data.Add(new Chromosone(chromesoneString));
            }

            int classificatioBit = int.Parse(chromesonesAsString[Program.RuleSize]);
            if (classificatioBit == 2)
            {
                throw new ArgumentException("Classifcation bit trying to be set to invalid number");
            }
            classification = classificatioBit;
        }

        public Rule(Rule oldRule)
        {
            foreach (Chromosone chromosone in oldRule.Data)
            {
                this.data.Add(new Chromosone(chromosone));
            }
            this.classification = oldRule.Classification;
        }

        public Rule(List<Chromosone> data, int classification)
        {
            this.data = data;
            this.classification = classification;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Chromosone chromosone in data)
            {
                stringBuilder.Append(chromosone);
            }

            return stringBuilder.ToString();
        }

        public void Mutate()
        {
            int index = Program.Random.Next(0, 8);
            if (index < Program.RuleSize && index >= 0)
            {
                if (Program.Random.Next()%2 == 0)
                {
                    data[index].MutateUpperBound();
                }
                else
                {
                    data[index].MutateLowerBound();
                }
            }
            else
            {
                classification = (classification + 1) % 2;
            }
        }

        #endregion

        #region Private Method

        private List<string> ChromesonesAsString(string input)
        {
            return input.Split('G').ToList();
        }

        #endregion

        #region Properties

        public List<Chromosone> Data
        {
            get { return data; }
        }

        public int Classification
        {
            get { return classification; }
        }

        #endregion
    }
}
