using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models.Calculator
{
    public class DayCalculation
    {
        public int[] DishWeight { get; set; }

        public int[] DishConstWeight { get; set; }

        public decimal Proteins { get; set; }

        public decimal Fats { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Calories { get; set; }

        public decimal BreakfastCoefficient { get; set; }

        public decimal LunchCoefficient { get; set; }

        public decimal DinnerCoefficient { get; set; }

        public decimal DayCoefficient { get; set; }

        public int[] PossibleMin { get; set; }

        public int[] PossibleMax { get; set; }

        public DayCalculation(int arrayCount)
        {
            DishWeight = new int[arrayCount];
            DishConstWeight = new int[arrayCount];
            PossibleMin = new int[arrayCount];
            PossibleMax = new int[arrayCount];

        }
    }
}
