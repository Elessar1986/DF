using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class Dish
    {
        public int Id { get; set; }

        [Required]
        [Range(1,int.MaxValue)]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public bool IsInterval { get; set; }

        public int MinWeight { get; set; }

        public int MaxWeight { get; set; }

        public int ConstWeight { get; set; }

        public int MealId { get; set; }
        public Meal Meal { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdate { get; set; }

        public decimal MinProteins { get; set; }

        public decimal MaxProteins { get; set; }

        public decimal MinFats { get; set; }

        public decimal MaxFats { get; set; }

        public decimal MinCarbohydrates { get; set; }

        public decimal MaxCarbohydrates { get; set; }

        public decimal MinCalories { get; set; }

        public decimal MaxCalories { get; set; }

    }
}
