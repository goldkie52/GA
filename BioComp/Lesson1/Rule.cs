using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    class Rule
    {
        private List<int> data;
        private int classification;

        public Rule(List<int> input)
        {
            data = input.GetRange(0, 6);
            classification = input[6];
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int i in data)
            {
                stringBuilder.Append(i);
            }
            
            return stringBuilder.ToString();
        }

        public void ChangeBit(int index)
        {
            if (index < 5 && index >= 0)
            {
                data[index] = (data[index] + 1) % 2;
            }
            else if (index == 5)
            {
                data[index] = (data[index] + 1) % 2;
            }
        }

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
