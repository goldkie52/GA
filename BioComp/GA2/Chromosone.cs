using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2
{
    class Chromosone
    {
        private double boundOne;
        private double boundTwo;
        private const double VariationLimit = 0.5;

        public Chromosone()
        {
            boundOne = Math.Truncate(Program.Random.NextDouble() * 1000000) / 1000000;
            boundTwo = Math.Truncate(Program.Random.NextDouble() * 1000000) / 1000000;
        }

        public Chromosone(string chromesoneString)
        {
            string[] bounds = chromesoneString.Split(',');
            boundOne = Math.Truncate(float.Parse(bounds[0].Substring(1)) * 1000000) / 1000000;
            boundTwo = Math.Truncate(float.Parse(bounds[1].Substring(0, bounds[1].Length - 1)) * 1000000) / 1000000;
        }

        public Chromosone(Chromosone oldChromosone)
        {
            this.boundOne = oldChromosone.LowestBound;
            this.boundTwo = oldChromosone.HighestBound;
        }

        public Chromosone(double boundOne, double boundTwo)
        {
            this.boundOne = boundOne;
            this.boundTwo =boundTwo;
        }

        #region Public Methods

        public bool VariableBetweenBounds(float input)
        {
            return LowestBound <= input && input <= HighestBound;
        }

        public void MutateLowerBound()
        {
            double currentLowestBound = LowestBound;
            double variation = Program.Random.NextDouble() % VariationLimit;
            if (Program.Random.Next()%2 == 0)
            {
                currentLowestBound += variation;
            }
            else
            {
                currentLowestBound -= variation;
            }

            LowestBound = Math.Truncate(currentLowestBound * 1000000) / 1000000;
        }

        public void MutateUpperBound()
        {
            double currentHighestBound = HighestBound;
            double variation = Program.Random.NextDouble() % VariationLimit;
            if (Program.Random.Next() % 2 == 0)
            {
                currentHighestBound += variation;
            }
            else
            {
                currentHighestBound -= variation;
            }

            HighestBound = Math.Truncate(currentHighestBound * 1000000) / 1000000;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append(LowestBound);

            while (builder.Length != 9)
            {
                builder.Append("0");
            }

            builder.Append(",");
            builder.Append(HighestBound);

            while (builder.Length != 18)
            {
                builder.Append("0");
            }

            builder.Append("}G");

            return builder.ToString();
        }

        #endregion

        #region Properties

        public double HighestBound
        {
            get
            {
                if (boundOne > boundTwo)
                {
                    return boundOne >= 1 ? 1 : boundOne;
                }
                return boundTwo >= 1 ? 1 : boundTwo;
            }
            set
            {
                if (boundOne > boundTwo)
                {
                    boundOne = value >= 1 ? 1 : value;
                }
                else
                {
                    boundTwo = value >= 1 ? 1 : value; ;
                }
            }
        }

        public double LowestBound
        {
            get
            {
                if (boundOne < boundTwo)
                {
                    return boundOne < 0 ? 0 : boundOne;
                }
                return boundTwo < 0 ? 0 : boundTwo;
            }
            set
            {
                if (boundOne < boundTwo)
                {
                    boundOne = value < 0 ? 0 : value;
                }
                else
                {
                    boundTwo = value < 0 ? 0 : value;
                }
            }
        }

        #endregion
        
    }
}
