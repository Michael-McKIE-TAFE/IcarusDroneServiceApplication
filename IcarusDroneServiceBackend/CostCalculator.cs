namespace IcarusDroneServiceBackend {
    public class CostCalculator {
        public double CalculateCost (double value, bool applySurcharge){
            double increaseAmount = 0;
            double returnAmount = 0;

            increaseAmount = value * 0.15f;

            if (applySurcharge){
                returnAmount = value + increaseAmount;          
                return returnAmount;
            } else {
                returnAmount = value;
                return returnAmount;
            }
        }
    }
}