using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models.Calculator
{
    public class CalculateModel
    {
        public DayCalculation DayCalculation { get; set; }

        public List<DishParam> Dishes { get; set; }

        public Day Day { get; set; }
    }
}
