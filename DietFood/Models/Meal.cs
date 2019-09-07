using DietFood.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public decimal MinProteins { get; set; }

        public decimal MaxProteins { get; set; }

        public decimal MinFats { get; set; }

        public decimal MaxFats { get; set; }

        public decimal MinCarbohydrates { get; set; }

        public decimal MaxCarbohydrates { get; set; }

        public decimal MinCalories { get; set; }

        public decimal MaxCalories { get; set; }

        public int MinWeight { get; set; }

        public int MaxWeight { get; set; }

        public List<Dish> Dishes { get; set; }

        public int DayId { get; set; }
        public Day Day { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdate { get; set; }

        public int WeekId { get; set; }

        public string Name { get; set; }

        public int Done { get; set; }

    }
}
