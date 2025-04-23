using System.Diagnostics.Metrics;
using System.Drawing;

namespace IcarusDroneServiceBackend {
        /// <summary>
        /// This class contains a method called `CalculateCost`, which calculates a cost based
        /// on an input value and a surcharge flag. It first calculates an increase amount, which
        /// is 15% of the input value. If the `applySurcharge` flag is true, it adds the increase
        /// amount to the value and returns the result. If the flag is false, it simply returns 
        /// the original value without any surcharge.
        /// </summary>
    public class CostCalculator {
        //  This method calculates the cost by adding a 15% surcharge if the
        //  `applySurcharge` flag is `true`, or returns the original value if
        //  the flag is `false`.
        public double CalculateCost (double value, bool applySurcharge){
            double increaseAmount = value * 0.15;

            if (applySurcharge){         
                return value + increaseAmount;
            } else {
                return value;
            }
        }
    }
}