﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class Product
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Proteins { get; set; }

        public decimal Fats { get; set; }

        public decimal Carbohydrates { get; set; }

        public  decimal Calories { get; set; }

        public List<Dish> Dishes { get; set; }
    }
}
