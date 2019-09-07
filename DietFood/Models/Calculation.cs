using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class Calculation
    {
        public int Id { get; set; }

        public int DayId { get; set; }

        public Day Day { get; set; }

        public DateTime Created { get; set; }

        public int ClientWeight { get; set; }

        public List<DishCalculation> DishCalculations { get; set; }

        public decimal Proteins { get; set; }

        public decimal Fats { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Calories { get; set; }

        public decimal BreakfastCoefficient { get; set; }

        public decimal LunchCoefficient { get; set; }

        public decimal DinnerCoefficient { get; set; }

        public decimal DayCoefficient { get; set; }

        public int DishWeightSum
        {
            get
            {
                return DishCalculations.Sum( x =>x.Weight);
            }
        }

        public int DishConstWeightSum
        {
            get
            {
                return DishCalculations.Sum(x => x.ConstWeight);
            }
        }

        public Calculation()
        {
            DishCalculations = new List<DishCalculation>();
        }
    }
}
