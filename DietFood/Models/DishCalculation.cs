using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class DishCalculation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MealType { get; set; }

        public int Weight { get; set; }

        public int ConstWeight { get; set; }

        public int CalculationId { get; set; }

        public Calculation Calculation { get; set; }

        public decimal Proteins { get; set; }

        public decimal Fats { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Calories { get; set; }

    }
}
