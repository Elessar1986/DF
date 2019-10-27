using DietFood.Helpers;
using DietFood.Helpers.Abstract;
using DietFood.Models;
using DietFood.Models.Calculator;
using DietFood.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DietFood.Controllers
{
    public class MainController : Controller
    {
        IRepositoryHelper _repository;
        FoodCalculate _foodCalculate;
        PdfHelper _pdfHelper;
        public MainController(IRepositoryHelper repository)
        {
            _repository = repository;
            _foodCalculate = new FoodCalculate(_repository);
            _pdfHelper = new PdfHelper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddIngredient()
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
        public async Task<IActionResult> AddIngredient(Ingredient data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.AddIngredient(data);
                    return RedirectToAction("Ingredients");
                }
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }


        public IActionResult Ingredients()
        {
            try
            {
                var model = _repository.GetAllIngredients();
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult EditIngredient(int ingredientId)
        {
            try
            {
                var model = _repository.GetIngredient(ingredientId);
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditIngredient(Ingredient data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.UpdateIngredient(data);
                    return RedirectToAction("Ingredients");
                }
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DeleteIngredient(int ingredientId)
        {
            try
            {
                _repository.DeleteIngredient(ingredientId);
                return RedirectToAction("Ingredients");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
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
                    var id =_repository.AddProduct(data);
                    return RedirectToAction("AddIngredientToProduct", new { productId = id });
                }
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult AddIngredientToProduct(int productId)
        {
            try
            {
                var model = _repository.GetProduct(productId);
                ViewBag.Ingredients = _repository.GetAllIngredients().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public IActionResult AddIngredientToProduct(ProductIngredient data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.AddProductIngredient(data);
                    ViewBag.Ingredients = _repository.GetAllIngredients().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                    return Json(new { status = "ok", productId = data.ProductId }); //PartialView("_AddDish", new Dish());
                }
                else
                {
                    ViewBag.Ingredients = _repository.GetAllIngredients().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                    return PartialView("_AddIngredient", data);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DeleteProductIngredient(int Id, int productId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.DeleteProductIngredient(Id);
                    return RedirectToAction("AddIngredientToProduct", new { productId = productId });
                }
                else
                {
                    return View("Error", new Exception("Что то пошло не так"));
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddIngredientToProduct(int productId, )
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //await _repository.AddProduct(data);
        //            var prod = _repository.GetProduct(productId);


        //            return RedirectToAction("Products");
        //        }
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex);
        //    }
        //}

        public IActionResult SaveProduct(int productId)
        {
            try
            {
                ViewBag.ProductId = productId;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        //public IActionResult EditProduct(int productId)
        //{
        //    try
        //    {
        //        var model = _repository.GetProduct(productId);
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex);
        //    }
        //}

        public IActionResult Products()
        {
            try
            {
                var model = _repository.GetAllProducts();
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult EditProduct(Product data)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _repository.UpdateProduct(data);
        //            return RedirectToAction("Products");
        //        }
        //        return View(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex);
        //    }
        //}

        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                _repository.DeleteProduct(productId);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult AddProgram()
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

        public IActionResult EditProgram(int programId)
        {
            try
            {
                var model = _repository.GetProgram(programId);
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult Programs()
        {
            try
            {
                var model = _repository.GetAllPrograms();
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProgram(DietProgram data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.AddProgram(data);
                    return RedirectToAction("Programs");
                }
                return View(data); 
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProgram(DietProgram data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.UpdateProgram(data);
                    return RedirectToAction("Programs");
                }
                return View(data);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult DeleteProgram(int programId)
        {
            try
            {
                _repository.DeleteProgram(programId);
                return RedirectToAction("Programs");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult CalcWeeks()
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

        public IActionResult CalcWeekDays(int weekId)
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
                else
                {
                    _repository.DeleteCalculationsByDay(model);
                }

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

        public IActionResult CalculateDay(int dayId, int weight = 40)
        {
            try
            {
                var model = _foodCalculate.CalculateDayInWeight(dayId, weight);
                //_foodCalculate.CalculateDay(dayId);
                return RedirectToAction("DayCalculation", new { weight = weight, dayId = dayId });
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
        public IActionResult CustomDayCalculation(int weight, int dayId, Dictionary<int,int> constWeight)
        {
            try
            {
                _foodCalculate.CustomDayCalculate(weight, dayId, constWeight);

                return Json(new { status = "ok" } );

            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public FileResult DownloadDayCalcPDF(int dayId, int weight)
        {
            try
            {
                var day = _repository.GetDay(dayId);
                var model = day.Calculations.FirstOrDefault(x => x.ClientWeight == weight);
                var res = _pdfHelper.CreateTable(model, day.Week.Name, day.Week.DietProgram.Name, day.DayName, weight);
                return File(res, "application/pdf", $"{DateTime.Now}_calculation.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult DownloadAllWeightPDF(int dayId)
        {
            try
            {
                var day = _repository.GetDay(dayId);
                var model = _foodCalculate.GetAllDayCalcForPDF(dayId);
                var res = _pdfHelper.CreateTable(model, day.Week.Name, day.Week.DietProgram.Name, day.DayName);
                return File(res, "application/pdf", $"{DateTime.Now}_allDayCalculation.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}