using DietFood.Helpers.Abstract;
using DietFood.Models;
using DietFood.Models.Calculator;
using DietFood.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Helpers
{
    public class RepositoryHelper : IRepositoryHelper
    {
        MyContext _context;
        public RepositoryHelper(MyContext context)
        {
            _context = context;
        }

        public List<Product> Test()
        {
            return _context.Products.ToList();
        }

        public List<SelectListItem> GetProductsSelectList()
        {
            //throw new Exception("Test", new Exception("Test"));
            var res = _context.Products.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return res;
        }

        public List<SelectListItem> GetDietProgramSelectList()
        {
            var res = _context.DietPrograms.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return res;
        }

        public List<Week> GetWeeks()
        {
            var res = _context.Weeks.Include(x => x.DietProgram).Include(x => x.Days).ToList();
            return res;
        }

        public Week GetWeek(int weekId)
        {
            var res = _context.Weeks.Include(x => x.DietProgram).Include(x => x.Days).FirstOrDefault(x => x.Id == weekId);
            return res;
        }

        public void DeleteWeek(int weekId)
        {
            var week = _context.Weeks.FirstOrDefault(x => x.Id == weekId);
            _context.Weeks.Remove(week);
            _context.SaveChanges();
        }

        public void UpdateWeek(Week week)
        {
            _context.Weeks.Update(week);
            _context.SaveChanges();
        }

        public Day GetDay(int weekId, DaysOfWeek dayName)
        {
            var meal = _context.Days.Include(x => x.Week).Include(x => x.Meals).FirstOrDefault(x =>
                                    x.DayName == dayName &&
                                    x.WeekId == weekId);
            return meal;
        }

        public Day GetDay(int dayId)
        {
            var meal = _context.Days.Include(x => x.Week).Include(x => x.Calculations).ThenInclude(b => b.DishCalculations).Include(x => x.Meals).ThenInclude(c => c.Dishes).ThenInclude(v => v.Product).FirstOrDefault(x => x.Id == dayId);
            return meal;
        }

        public Meal GetMeal(int mealId)
        {
            var meal = _context.Meals.Include(x => x.Day).Include(x => x.Dishes).ThenInclude(c => c.Product).FirstOrDefault(x => x.Id == mealId);

            return meal;
        }


        public Dish GetDish(int dishId)
        {
            var dish = _context.Dishes.Include(x => x.Meal).Include(x => x.Product).FirstOrDefault(x => x.Id == dishId);
            return dish;
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }

        public int AddProduct(Product product)
        {
            var check = _context.Products.FirstOrDefault(x => x.Name == product.Name);
            if (check == null)
            {
                _context.Products.Add(product);
                _context.SaveChangesAsync();
            }
            return _context.Products.FirstOrDefault(x => x.Name == product.Name).Id;
        }

        public void AddProductIngredient(ProductIngredient data)
        {
            _context.ProductIngredients.Add(data);
            _context.SaveChanges();
            UpdateProduct(data.ProductId);

        }

        public void DeleteProductIngredient(int itemId)
        {
            //TODO: 
            var prodIngr = _context.ProductIngredients.FirstOrDefault(x => x.Id == itemId);
            _context.ProductIngredients.Remove(prodIngr);
            _context.SaveChanges();
            UpdateProduct(prodIngr.ProductId);
        }

        private void UpdateProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);
            var progIngedSum = new ProductIngredientCalc();
            if (product.ProductIngredients.Count > 0)
            {
                foreach (var item in product.ProductIngredients)
                {
                    progIngedSum.Calories += item.Ingredient.Calories / 100 * item.Weight;
                    progIngedSum.Proteins += item.Ingredient.Proteins / 100 * item.Weight;
                    progIngedSum.Fats += item.Ingredient.Fats / 100 * item.Weight;
                    progIngedSum.Carbohydrates += item.Ingredient.Carbohydrates / 100 * item.Weight;
                    progIngedSum.Weight += item.Weight;
                }
                product.Calories = Math.Round(progIngedSum.Calories / progIngedSum.Weight * 100, 2);
                product.Proteins = Math.Round(progIngedSum.Proteins / progIngedSum.Weight * 100, 2);
                product.Fats = Math.Round(progIngedSum.Fats / progIngedSum.Weight * 100, 2);
                product.Carbohydrates = Math.Round(progIngedSum.Carbohydrates / progIngedSum.Weight * 100, 2);
            }
            else
            {
                product.Calories = 0;
                product.Proteins = 0;
                product.Fats = 0;
                product.Carbohydrates = 0;
            }
            _context.SaveChanges();
        }

        public async Task AddWeek(Week week)
        {
            week.Created = DateTime.Now;

            await _context.Weeks.AddAsync(week);
            await _context.SaveChangesAsync();
        }

        public Day AddDay(Day day)
        {
            _context.Days.Add(day);
            for (int i = 0; i < 3; i++)
            {
                _context.Meals.Add(new Meal
                {
                    Created = DateTime.Now,
                    DayId = day.Id,
                    WeekId = day.WeekId,
                    Name = ((MealName)i).ToString()
                });
            }
            _context.SaveChanges();
            return day;
        }

        public Meal AddMeal(Meal meal)
        {
            var day = _context.Days.FirstOrDefault(x => x.Id == meal.DayId);
            meal.Name = ((MealName)day.Meals.Count).ToString();
            _context.Meals.Add(meal);
            _context.SaveChanges();
            return meal;
        }

        public Calculation AddCalculation(Calculation calculation)
        {
            _context.Calculations.Add(calculation);
            _context.SaveChanges();
            return calculation;
        }

        public void DeleteCalculation(int calcId)
        {
            var calc = _context.Calculations.FirstOrDefault(x => x.Id == calcId);
            if (calc != null)
            {
                _context.Calculations.Remove(calc);
                _context.SaveChanges();
            }
            else throw new Exception("No calculation find by Id!");
        }

        public DishCalculation AddDishCalculation(DishCalculation dishCalculation)
        {
            _context.DishCalculations.Add(dishCalculation);
            _context.SaveChanges();
            return dishCalculation;
        }

        public Meal DeleteMeal(int id)
        {
            var meal = _context.Meals.FirstOrDefault(x => x.Id == id);
            _context.Meals.Remove(meal);
            if (meal.Done > 0)
            {
                var day = _context.Days.FirstOrDefault(x => x.Id == meal.DayId);
                day.Done--;
                _context.Days.Update(day);
            }
            _context.SaveChanges();
            return meal;
        }

        public Dish AddDish(Dish dish)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == dish.ProductId);

            if (dish.IsInterval)
            {
                dish.MinCalories = product.Calories * dish.MinWeight / 100;
                dish.MaxCalories = product.Calories * dish.MaxWeight / 100;
                dish.MinProteins = product.Proteins * dish.MinWeight / 100;
                dish.MaxProteins = product.Proteins * dish.MaxWeight / 100;
                dish.MinFats = product.Fats * dish.MinWeight / 100;
                dish.MaxFats = product.Fats * dish.MaxWeight / 100;
                dish.MinCarbohydrates = product.Carbohydrates * dish.MinWeight / 100;
                dish.MaxCarbohydrates = product.Carbohydrates * dish.MaxWeight / 100;
            }
            else
            {
                dish.MinCalories = product.Calories * dish.ConstWeight / 100;
                dish.MaxCalories = product.Calories * dish.ConstWeight / 100;
                dish.MinProteins = product.Proteins * dish.ConstWeight / 100;
                dish.MaxProteins = product.Proteins * dish.ConstWeight / 100;
                dish.MinFats = product.Fats * dish.ConstWeight / 100;
                dish.MaxFats = product.Fats * dish.ConstWeight / 100;
                dish.MinCarbohydrates = product.Carbohydrates * dish.ConstWeight / 100;
                dish.MaxCarbohydrates = product.Carbohydrates * dish.ConstWeight / 100;
            }

            _context.Dishes.Add(dish);
            dish.Meal.Done++;
            if (dish.Meal.Done == 1)
            {
                var day = _context.Days.FirstOrDefault(x => x.Id == dish.Meal.DayId);
                day.Done++;
                _context.Days.Update(day);
            }
            if (dish.IsInterval)
            {
                dish.Meal.MaxWeight += dish.MaxWeight;
                dish.Meal.MinWeight += dish.MinWeight;
                dish.Meal.MinCalories += product.Calories * dish.MinWeight / 100;
                dish.Meal.MaxCalories += product.Calories * dish.MaxWeight / 100;
                dish.Meal.MinProteins += product.Proteins * dish.MinWeight / 100;
                dish.Meal.MaxProteins += product.Proteins * dish.MaxWeight / 100;
                dish.Meal.MinFats += product.Fats * dish.MinWeight / 100;
                dish.Meal.MaxFats += product.Fats * dish.MaxWeight / 100;
                dish.Meal.MinCarbohydrates += product.Carbohydrates * dish.MinWeight / 100;
                dish.Meal.MaxCarbohydrates += product.Carbohydrates * dish.MaxWeight / 100;
            }
            else
            {
                dish.Meal.MaxWeight += dish.ConstWeight;
                dish.Meal.MinWeight += dish.ConstWeight;
                dish.Meal.MinCalories += product.Calories * dish.ConstWeight / 100;
                dish.Meal.MaxCalories += product.Calories * dish.ConstWeight / 100;
                dish.Meal.MinProteins += product.Proteins * dish.ConstWeight / 100;
                dish.Meal.MaxProteins += product.Proteins * dish.ConstWeight / 100;
                dish.Meal.MinFats += product.Fats * dish.ConstWeight / 100;
                dish.Meal.MaxFats += product.Fats * dish.ConstWeight / 100;
                dish.Meal.MinCarbohydrates += product.Carbohydrates * dish.ConstWeight / 100;
                dish.Meal.MaxCarbohydrates += product.Carbohydrates * dish.ConstWeight / 100;
            }
            _context.Meals.Update(dish.Meal);
            _context.SaveChanges();
            return dish;
        }

        public void DeleteDish(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(x => x.Id == dishId);
            var product = _context.Products.FirstOrDefault(x => x.Id == dish.ProductId);

            dish.Meal.Done--;
            if (dish.Meal.Done == 0)
            {
                var day = _context.Days.FirstOrDefault(x => x.Id == dish.Meal.DayId);
                day.Done--;
                _context.Days.Update(day);
            }
            if (dish.IsInterval)
            {
                dish.Meal.MaxWeight -= dish.MaxWeight;
                dish.Meal.MinWeight -= dish.MinWeight;
                dish.Meal.MinCalories -= product.Calories * dish.MinWeight / 100;
                dish.Meal.MaxCalories -= product.Calories * dish.MaxWeight / 100;
                dish.Meal.MinProteins -= product.Proteins * dish.MinWeight / 100;
                dish.Meal.MaxProteins -= product.Proteins * dish.MaxWeight / 100;
                dish.Meal.MinFats -= product.Fats * dish.MinWeight / 100;
                dish.Meal.MaxFats -= product.Fats * dish.MaxWeight / 100;
                dish.Meal.MinCarbohydrates -= product.Carbohydrates * dish.MinWeight / 100;
                dish.Meal.MaxCarbohydrates -= product.Carbohydrates * dish.MaxWeight / 100;
            }
            else
            {
                dish.Meal.MaxWeight -= dish.ConstWeight;
                dish.Meal.MinWeight -= dish.ConstWeight;
                dish.Meal.MinCalories -= product.Calories * dish.ConstWeight / 100;
                dish.Meal.MaxCalories -= product.Calories * dish.ConstWeight / 100;
                dish.Meal.MinProteins -= product.Proteins * dish.ConstWeight / 100;
                dish.Meal.MaxProteins -= product.Proteins * dish.ConstWeight / 100;
                dish.Meal.MinFats -= product.Fats * dish.ConstWeight / 100;
                dish.Meal.MaxFats -= product.Fats * dish.ConstWeight / 100;
                dish.Meal.MinCarbohydrates -= product.Carbohydrates * dish.ConstWeight / 100;
                dish.Meal.MaxCarbohydrates -= product.Carbohydrates * dish.ConstWeight / 100;
            }
            _context.Dishes.Remove(dish);
            _context.Meals.Update(dish.Meal);
            _context.SaveChanges();
        }

        public async Task<int> AddProgram(DietProgram data)
        {
            await _context.DietPrograms.AddAsync(data);
            await _context.SaveChangesAsync();
            return data.Id;
        }

        public DietProgram GetProgram(int programId)
        {
            var res = _context.DietPrograms.FirstOrDefault(x =>x.Id == programId);
            return res;
        }

        public List<DietProgram> GetAllPrograms()
        {
            var res = _context.DietPrograms.ToList();
            return res;
        }

        public void DeleteProgram(int programId)
        {
            var res = _context.DietPrograms.FirstOrDefault(x => x.Id == programId);
            if (res != null) _context.DietPrograms.Remove(res);
            _context.SaveChanges();
        }

        public void UpdateProgram(DietProgram data)
        {
            var res = _context.DietPrograms.FirstOrDefault(x => x.Id == data.Id);
            if (res != null)
            {
                res.Calories = data.Calories;
                res.Carbohydrates = data.Carbohydrates;
                res.Fats = data.Fats;
                res.Name = data.Name;
                res.Proteins = data.Proteins;
                _context.DietPrograms.Update(res);
                _context.SaveChanges();
            }
            else throw new Exception($"No DietProgram whith id : '{data.Id}'!");
            
        }

        public Ingredient GetIngredient(int ingredientId)
        {
            var res = _context.Ingredients.FirstOrDefault(x => x.Id == ingredientId);
            return res;
        }

        public Product GetProduct(int productId)
        {
            var res = _context.Products.Include(x => x.ProductIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.Id == productId);
            return res;
        }

        public List<Ingredient> GetAllIngredients()
        {
            var res = _context.Ingredients.ToList();
            return res;
        }

        public List<Product> GetAllProducts()
        {
            var res = _context.Products.ToList();
            return res;
        }

        public void DeleteIngredient(int ingredientId)
        {
            var res = _context.Ingredients.FirstOrDefault(x => x.Id == ingredientId);
            if (res != null) _context.Ingredients.Remove(res);
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var res = _context.Products.FirstOrDefault(x => x.Id == productId);
            if (res != null) _context.Products.Remove(res);
            _context.SaveChanges();
        }

        //public void UpdateProduct(Product data)
        //{
        //    var res = _context.Products.FirstOrDefault(x => x.Id == data.Id);
        //    if (res != null)
        //    {
        //        res.Calories = data.Calories;
        //        res.Carbohydrates = data.Carbohydrates;
        //        res.Fats = data.Fats;
        //        res.Name = data.Name;
        //        res.Proteins = data.Proteins;
        //        _context.Products.Update(res);
        //        _context.SaveChanges();
        //    }
        //    else throw new Exception($"No Product whith id : '{data.Id}'!");

        //}

        public void UpdateIngredient(Ingredient data)
        {
            var res = _context.Ingredients.FirstOrDefault(x => x.Id == data.Id);
            if (res != null)
            {
                res.Calories = data.Calories;
                res.Carbohydrates = data.Carbohydrates;
                res.Fats = data.Fats;
                res.Name = data.Name;
                res.Proteins = data.Proteins;
                _context.Ingredients.Update(res);
                _context.SaveChanges();
            }
            else throw new Exception($"No Ingredient whith id : '{data.Id}'!");

        }
    }
}
