using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class Rule
    {
        private List<int> data = new List<int>();
        private int classification = new int();

        #region Constructors

        #region UnusedConstructor

        public Rule(List<int> dataAndClass)
        {
            if (dataAndClass.Count == Program.RuleSize + 1)
            {
                for (int i = 0; i < dataAndClass.Count - 1; i++)
                {
                    data.Add(dataAndClass[i]);
                }

                classification = Program.Random.Next(0, 2);
            }
            else
            {
                throw new ArgumentException($"The dateAndClass parameter was an incorrect length.{Environment.NewLine}Length = {dataAndClass.Count}, aiming for {Program.RuleSize + 1}");
            }
        }

        #endregion
        
        public Rule()
        {
            for (int i = 0; i < Program.RuleSize; i++)
            {
                data.Add(Program.Random.Next(0, 3));
            }

            classification = Program.Random.Next(0, 2);
            if (classification == 2)
            {
                throw new InvalidOperationException("Classification bit trying to be set to invalid number");
            }

        }

        public Rule(string dataAndClass)
        {
            for (int i = 0; i < Program.RuleSize; i++)
            {
                data.Add(int.Parse(dataAndClass[i].ToString()));
            }

            int classificatioBit = int.Parse(dataAndClass[Program.RuleSize].ToString());
            if (classificatioBit == 2)
            {
                throw new ArgumentException("Classifcation bit trying to be set to invalid number");
            }
            classification = classificatioBit;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int i in data)
            {
                stringBuilder.Append(i);
            }

            return stringBuilder.ToString();
        }

        public void MutateBit(int index)
        {
            if (index < Program.RuleSize && index >= 0)
            {
                int valueBefore = data[index];
                while (data[index] == valueBefore)
                {
                    data[index] = Program.Random.Next(0, 3);
                }
            }
            else if (index == Program.RuleSize)
            {
                classification = (classification + 1) % 2;
            }
        }

        #endregion

        #region Properties

        public List<int> Data
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
