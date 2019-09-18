using DietFood.Models;
using DietFood.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Helpers.Abstract
{
    public interface IRepositoryHelper
    {
        List<Product> Test();

        List<SelectListItem> GetProductsSelectList();

        List<SelectListItem> GetDietProgramSelectList();

        List<Week> GetWeeks();

        Week GetWeek(int weekId);

        void DeleteWeek(int weekId);

        void UpdateWeek(Week week);

        Meal GetMeal(int mealId);

        Day GetDay(int weekId, DaysOfWeek dayName);

        Day GetDay(int dayId);

        Dish GetDish(int dishId);

        Task AddProduct(Product product);

        Task<int> AddProgram(DietProgram program);

        DietProgram GetProgram(int programId);

        List<DietProgram> GetAllPrograms();

        void DeleteProgram(int programId);

        void UpdateProgram(DietProgram data);

        Task AddWeek(Week week);

        Day AddDay(Day day);

        Meal AddMeal(Meal meal);

        Meal DeleteMeal(int id);

        Dish AddDish(Dish dish);

        void DeleteDish(int dishId);

        Calculation AddCalculation(Calculation calculation);

        DishCalculation AddDishCalculation(DishCalculation dishCalculation);

        void DeleteCalculation(int calcId);

        Product GetProduct(int productId);

        List<Product> GetAllProducts();

        void DeleteProduct(int productId);

        void UpdateProduct(Product data);
    }
}
