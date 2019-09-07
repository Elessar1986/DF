using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models.Calculator
{
    public class DishParam
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MealType { get; set; }

        public int PossibleWeight { get; set; }

        public int MinWeight { get; set; }

        public int MaxWeight { get; set; }

        public int PossibleMinWeight { get; set; }

        public int PossibleMaxWeight { get; set; }

        public int ConstWeight { get; set; }

        public decimal ProteinsPer100 { get; set; }

        public decimal FatsPer100 { get; set; }

        public decimal CarbohydratesPer100 { get; set; }

        public decimal CaloriesPer100 { get; set; }

        public bool IsInterval { get; set; }

        public int GetInterval
        {
            get
            {
                return MaxWeight - MinWeight;
            }
        }

        public int GetPossibleInterval
        {
            get
            {
                return PossibleMaxWeight - PossibleMinWeight;
            }
        }

    }
}
