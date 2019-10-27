using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models.Calculator
{
    public class AllDishWeightCalc
    {
        public string MealTypeName { get; set; }
        public List<WeightCalc> WeightCalcs { get; set; }
    }

    public class WeightCalc
    {

        public string Name { get; set; }

        public List<int> Weight { get; set; }

    }
}
