using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models.Calculator
{
    public class ProductIngredientCalc
    {
        public decimal Proteins { get; set; }

        public decimal Fats { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Calories { get; set; }

        public int Weight { get; set; }
    }
}
