﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class ProductIngredient
    { 
        public int Id { get; set; }

        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }

        public int Weight { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

    }
}
