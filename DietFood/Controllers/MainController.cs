using DietFood.Helpers;
using DietFood.Helpers.Abstract;
using DietFood.Models;
using DietFood.Models.Calculator;
using DietFood.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace DietFood.Controllers
{
    public class MainController : Controller
    {
        IRepositoryHelper _repository;
        FoodCalculate _foodCalculate;
        public MainController(IRepositoryHelper repository)
        {
            _repository = repository;
            _foodCalculate = new FoodCalculate(_repository);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.AddProduct(data);
                    return View("Index");
                }
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult AddWeek()
        {
            try
            {
                ViewBag.Programs = _repository.GetDietProgramSelectList();
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWeek(Week data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.AddWeek(data);
                    return RedirectToAction("Weeks");
                }
                ViewBag.Programs = _repository.GetDietProgramSelectList();
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult Weeks()
        {
            try
            {
                var model = _repository.GetWeeks();
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult EditWeek(int weekId)
        {
            try
            {
                var model = _repository.GetWeek(weekId);
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DeleteWeek(int weekId)
        {
            try
            {
                _repository.DeleteWeek(weekId);
                return RedirectToAction("Weeks");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult EditDay(int weekId, DaysOfWeek dayName)
        {
            try
            {
                var model = _repository.GetDay(weekId, dayName);
                if (model == null)
                {
                    model = _repository.AddDay(new Day { WeekId = weekId, DayName = dayName });
                }
                //model = new Day() { WeekId = weekId, DayName = (DaysOfWeek)dayId };

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DeleteMeal(int dayId, int mealId)
        {
            try
            {
                _repository.DeleteMeal(mealId);
                var model = _repository.GetDay(dayId);
                return View("EditDay", model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult EditMeal(int mealId)
        {
            try
            {
                var model = _repository.GetMeal(mealId);
                if (model == null)
                    throw new Exception("Нет такой еды)");
                ViewBag.Product = _repository.GetProductsSelectList();
                return View("EditMeal", model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }


        public IActionResult AddDish()
        {
            ViewBag.Products = _repository.GetProductsSelectList();
            return View();
        }

        public IActionResult AddMeal(int weekId, int dayId)
        {
            try
            {
                var meal = _repository.AddMeal(new Meal { WeekId = weekId, DayId = dayId });
                //ViewBag.Product = _repository.GetProductsSelectList();
                var day = _repository.GetDay(dayId);
                return RedirectToAction("EditDay", new { weekId = weekId, dayName = day.DayName });
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public IActionResult AddDish(Dish dish)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.AddDish(dish);
                    ViewBag.Product = _repository.GetProductsSelectList();
                    return Json(new { status = "ok", mealId = dish.MealId }); //PartialView("_AddDish", new Dish());
                }
                else
                {
                    ViewBag.Product = _repository.GetProductsSelectList();
                    return PartialView("_AddDish", dish);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }


        }

        public IActionResult DeleteDish(int Id, int mealId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.DeleteDish(Id);
                    return RedirectToAction("EditMeal", new { mealId = mealId });
                }
                else
                {
                    return View("Error", new Exception("Что то пошло не так)"));
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }


        }


        public IActionResult EditDish(int dishId)
        {
            try
            {
                var dish = _repository.GetDish(dishId);
                var model = _repository.GetMeal(dish.MealId);
                ViewBag.DishId = model.Dishes.IndexOf(dish);
                ViewBag.Product = _repository.GetProductsSelectList();

                return View("_AddMeal", model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult CalculateDay(int dayId)
        {
            try
            {
                var model = _foodCalculate.CalculateDayInWeight(dayId, 40);
                //_foodCalculate.CalculateDay(dayId);
                return RedirectToAction("DayCalculation", new { weight = 40, dayId = dayId });
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DayCalculation(int weight, int dayId)
        {
            try
            {
                var day = _repository.GetDay(dayId);

                ViewBag.Calories = day.Week.DietProgram.Calories * weight;
                ViewBag.Proteins = day.Week.DietProgram.Proteins * weight;
                ViewBag.Fats = day.Week.DietProgram.Fats * weight;
                ViewBag.Carbohydrates = day.Week.DietProgram.Carbohydrates * weight;

                var model = day.Calculations.FirstOrDefault(x => x.ClientWeight == weight);
                if (model == null) throw new Exception("Day not calculated!");
                return View(model);

            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DayCalculationByWeight(ClientWeightVM data)
        {
            try
            {
                _foodCalculate.CalculateDayInWeight(data.DayId, data.Weight);
                var day = _repository.GetDay(data.DayId);

                ViewBag.Calories = day.Week.DietProgram.Calories * data.Weight;
                ViewBag.Proteins = day.Week.DietProgram.Proteins * data.Weight;
                ViewBag.Fats = day.Week.DietProgram.Fats * data.Weight;
                ViewBag.Carbohydrates = day.Week.DietProgram.Carbohydrates * data.Weight;

                var model = day.Calculations.FirstOrDefault(x => x.ClientWeight == data.Weight);
                if (model == null) throw new Exception("Day not calculated!");
                return View("DayCalculation", model);

            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public IActionResult CustomDayCalculation(int weight, int dayId, int[] constWeight)
        {
            try
            {
                var day = _repository.GetDay(dayId);
                var model = day.Calculations.FirstOrDefault(x => x.ClientWeight == weight);
                if (model == null) throw new Exception("Day not calculated!");
                return View("DayCalculation", model);

            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }
    }
}