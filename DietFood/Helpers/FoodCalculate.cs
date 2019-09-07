using DietFood.Helpers.Abstract;
using DietFood.Models;
using DietFood.Models.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DietFood.Helpers
{
    public class FoodCalculate
    {
        IRepositoryHelper _repository;

        private DayCalculation defaultDayOptions;
        private DayCalculation bestDayOptions;

        public FoodCalculate(IRepositoryHelper repository)
        {
            _repository = repository;
        }

        int iterCount;

        public CalculateModel CalculateDay(int dayId/*, int weight*/)
        {
            var day = _repository.GetDay(dayId);
            for (int i = 40; i <= 100; i += 5)
            {
                int weight = i;
                defaultDayOptions = new DayCalculation(day.Meals.Sum(x => x.Dishes.Count))
                {
                    Calories = day.Week.DietProgram.Calories * weight,
                    Proteins = day.Week.DietProgram.Proteins * weight,
                    Fats = day.Week.DietProgram.Fats * weight,
                    Carbohydrates = day.Week.DietProgram.Carbohydrates * weight,
                    BreakfastCoefficient = 40,
                    LunchCoefficient = 40,
                    DinnerCoefficient = 20
                };

                var dishList = GetDishParamList(day.Meals).OrderByDescending(x => x.GetInterval).ToList();

                iterCount = 0;
                CalculateBestDay(dishList);

                WriteCalculation(day, dishList, i);
                bestDayOptions = null;
            }

            return new CalculateModel();
        }

        public CalculateModel CalculateDayInWeight(int dayId, int weight)
        {
            var day = _repository.GetDay(dayId);

            defaultDayOptions = new DayCalculation(day.Meals.Sum(x => x.Dishes.Count))
            {
                Calories = day.Week.DietProgram.Calories * weight,
                Proteins = day.Week.DietProgram.Proteins * weight,
                Fats = day.Week.DietProgram.Fats * weight,
                Carbohydrates = day.Week.DietProgram.Carbohydrates * weight,
                BreakfastCoefficient = 40,
                LunchCoefficient = 40,
                DinnerCoefficient = 20
            };

            var dishList = GetDishParamList(day.Meals).OrderByDescending(x => x.GetInterval).ToList();

            CalculateBestDay(dishList);

            WriteCalculation(day, dishList, weight);

            return new CalculateModel();
        }

        public CalculateModel CustomDayCalculate(int weight, int dayId, int[] constWeight)
        {

            return new CalculateModel();
        }

        private decimal GetDayCoefficient(DayCalculation day)
        {
            decimal res = 0M;
            res += Math.Abs(defaultDayOptions.Calories - day.Calories) / defaultDayOptions.Calories;
            res += Math.Abs(defaultDayOptions.Proteins - day.Proteins) / defaultDayOptions.Proteins;
            res += Math.Abs(defaultDayOptions.Fats - day.Fats) / defaultDayOptions.Fats;
            res += Math.Abs(defaultDayOptions.Carbohydrates - day.Carbohydrates) / defaultDayOptions.Carbohydrates;
            res += Math.Abs(defaultDayOptions.BreakfastCoefficient - day.BreakfastCoefficient) / defaultDayOptions.BreakfastCoefficient;
            res += Math.Abs(defaultDayOptions.LunchCoefficient - day.LunchCoefficient) / defaultDayOptions.LunchCoefficient;
            res += Math.Abs(defaultDayOptions.DinnerCoefficient - day.DinnerCoefficient) / defaultDayOptions.DinnerCoefficient;
            res = Math.Round(res, 10);
            return res;
        }


        private void WriteCalculation(Day day, List<DishParam> dishes, int weight)
        {
            try
            {
                var calc = day.Calculations.FirstOrDefault(x => x.ClientWeight == weight);
                if (calc != null)
                    _repository.DeleteCalculation(calc.Id);

                AddCalculation(day.Id, dishes, weight);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddCalculation(int dayId, List<DishParam> dishes, int weight)
        {
            var calc = new Calculation
            {
                DayId = dayId,
                ClientWeight = weight,
                Calories = bestDayOptions.Calories,
                Proteins = bestDayOptions.Proteins,
                Fats = bestDayOptions.Fats,
                Carbohydrates = bestDayOptions.Carbohydrates,
                BreakfastCoefficient = bestDayOptions.BreakfastCoefficient,
                LunchCoefficient = bestDayOptions.LunchCoefficient,
                DinnerCoefficient = bestDayOptions.DinnerCoefficient,
                DayCoefficient = bestDayOptions.DayCoefficient,
                Created = DateTime.Now
            };
            _repository.AddCalculation(calc);
            for (int i = 0; i < bestDayOptions.DishWeight.Length; i++)
            {
                _repository.AddDishCalculation(new DishCalculation
                {
                    CalculationId = calc.Id,
                    MealType = dishes[i].MealType,
                    Weight = bestDayOptions.DishWeight[i],
                    ConstWeight = bestDayOptions.DishConstWeight[i],
                    Calories = bestDayOptions.DishWeight[i] * dishes[i].CaloriesPer100 / 100,
                    Proteins = bestDayOptions.DishWeight[i] * dishes[i].ProteinsPer100 / 100,
                    Fats = bestDayOptions.DishWeight[i] * dishes[i].FatsPer100 / 100,
                    Carbohydrates = bestDayOptions.DishWeight[i] * dishes[i].CarbohydratesPer100 / 100,
                    Name = dishes[i].Name
                });
            }

        }

        private void UpdateCalculation(Day day, List<DishParam> dishes, int weight, int Id)
        {
            //var calc = 
            var calc = new Calculation
            {

                DayId = day.Id,
                ClientWeight = weight,
                Calories = bestDayOptions.Calories,
                Proteins = bestDayOptions.Proteins,
                Fats = bestDayOptions.Fats,
                Carbohydrates = bestDayOptions.Carbohydrates,
                BreakfastCoefficient = bestDayOptions.BreakfastCoefficient,
                LunchCoefficient = bestDayOptions.LunchCoefficient,
                DinnerCoefficient = bestDayOptions.DinnerCoefficient,
                DayCoefficient = bestDayOptions.DayCoefficient,
                Created = DateTime.Now
            };
            _repository.AddCalculation(calc);
            for (int i = 0; i < bestDayOptions.DishWeight.Length; i++)
            {
                _repository.AddDishCalculation(new DishCalculation
                {
                    CalculationId = calc.Id,
                    MealType = dishes[i].MealType,
                    Weight = bestDayOptions.DishWeight[i],
                    ConstWeight = bestDayOptions.DishConstWeight[i],
                    Calories = bestDayOptions.DishWeight[i] * dishes[i].CaloriesPer100 / 100,
                    Proteins = bestDayOptions.DishWeight[i] * dishes[i].ProteinsPer100 / 100,
                    Fats = bestDayOptions.DishWeight[i] * dishes[i].FatsPer100 / 100,
                    Carbohydrates = bestDayOptions.DishWeight[i] * dishes[i].CarbohydratesPer100 / 100,
                    Name = dishes[i].Name
                });
            }

        }

        private List<DishParam> GetDishParamList(List<Meal> meals)
        {
            var res = new List<DishParam>();

            for (int i = 0; i < meals.Count; i++)
            {
                foreach (var dish in meals[i].Dishes)
                {
                    if (dish.IsInterval)
                    {
                        res.Add(new DishParam
                        {
                            ProteinsPer100 = dish.Product.Proteins,
                            CaloriesPer100 = dish.Product.Calories,
                            CarbohydratesPer100 = dish.Product.Carbohydrates,
                            FatsPer100 = dish.Product.Fats,
                            Name = dish.Product.Name,
                            MinWeight = dish.MinWeight,
                            MaxWeight = dish.MaxWeight,
                            PossibleWeight = dish.MinWeight + (dish.MaxWeight - dish.MinWeight) / 4, // dish.MinWeight,
                            PossibleMaxWeight = dish.MaxWeight,//(dish.MinWeight + dish.MaxWeight) / 2,
                            PossibleMinWeight = dish.MinWeight,
                            MealType = i,
                            IsInterval = dish.IsInterval
                        });
                    }
                    else
                    {
                        res.Add(new DishParam
                        {
                            ProteinsPer100 = dish.Product.Proteins,
                            CaloriesPer100 = dish.Product.Calories,
                            CarbohydratesPer100 = dish.Product.Carbohydrates,
                            FatsPer100 = dish.Product.Fats,
                            Name = dish.Product.Name,
                            ConstWeight = dish.ConstWeight,
                            PossibleWeight = dish.ConstWeight,
                            MealType = i,
                            IsInterval = dish.IsInterval
                        });
                    }
                }
            }

            return res;
        }



        private void CalculateBestDay(List<DishParam> dishes, int i = 0, int min = -1, int max = -1)
        {
            iterCount++;
            if (dishes[i].IsInterval && i < dishes.Count)
            {

                if (bestDayOptions != null && bestDayOptions.PossibleMin[i] > 0 && bestDayOptions.PossibleMax[i] > 0)
                {
                    dishes[i].PossibleMaxWeight = bestDayOptions.PossibleMax[i];
                    dishes[i].PossibleMinWeight = bestDayOptions.PossibleMin[i];
                }


                if (dishes[i].GetPossibleInterval <= 6) return; //exit condition

                var possWeight1 = dishes[i].PossibleMinWeight + (dishes[i].PossibleMaxWeight - dishes[i].PossibleMinWeight) / 4;
                dishes[i].PossibleWeight = possWeight1;

                var day1 = CalculateDay(dishes);
                if (bestDayOptions == null)
                {
                    bestDayOptions = day1;
                }
                if (bestDayOptions != null && bestDayOptions.DayCoefficient >= day1.DayCoefficient /*&& i + 1 == dishes.Count*/)
                {
                    bestDayOptions = day1;
                }
                if (i + 1 < dishes.Count) CalculateBestDay(dishes, i + 1);


                var possWeight3 = dishes[i].PossibleMinWeight + (dishes[i].PossibleMaxWeight - dishes[i].PossibleMinWeight) / 2;
                dishes[i].PossibleWeight = possWeight3;

                var day3 = CalculateDay(dishes);
                if (bestDayOptions == null)
                {
                    bestDayOptions = day3;
                }
                if (bestDayOptions != null && bestDayOptions.DayCoefficient >= day3.DayCoefficient /*&& i + 1 == dishes.Count*/)
                {
                    bestDayOptions = day3;
                }
                if (i + 1 < dishes.Count) CalculateBestDay(dishes, i + 1);


                var possWeight2 = dishes[i].PossibleMinWeight + (dishes[i].PossibleMaxWeight - dishes[i].PossibleMinWeight) / 4 * 3;
                dishes[i].PossibleWeight = possWeight2;

                var day2 = CalculateDay(dishes);
                if (bestDayOptions == null)
                {
                    bestDayOptions = day2;
                }
                if (bestDayOptions != null && bestDayOptions.DayCoefficient >= day2.DayCoefficient /*&& i + 1 == dishes.Count*/)
                {
                    bestDayOptions = day2;
                }
                if (i + 1 < dishes.Count) CalculateBestDay(dishes, i + 1);


                if (i == 0 && dishes[i].GetPossibleInterval >= 6)
                {
                    for (int j = 0; j < dishes.Count; j++)
                    {
                        if ((dishes[j].PossibleMaxWeight + dishes[j].PossibleMinWeight) / 2 == bestDayOptions.DishWeight[j])
                        {
                            bestDayOptions.PossibleMax[j] = dishes[j].PossibleMinWeight + (dishes[j].PossibleMaxWeight - dishes[j].PossibleMinWeight) / 4 * 3;
                            bestDayOptions.PossibleMin[j] = dishes[j].PossibleMinWeight + (dishes[j].PossibleMaxWeight - dishes[j].PossibleMinWeight) / 4;
                        }
                        else if ((dishes[j].PossibleMaxWeight + dishes[j].PossibleMinWeight) / 2 > bestDayOptions.DishWeight[j])
                        {
                            bestDayOptions.PossibleMax[j] = dishes[j].PossibleMinWeight + (dishes[j].PossibleMaxWeight - dishes[j].PossibleMinWeight) / 2;
                            bestDayOptions.PossibleMin[j] = dishes[j].PossibleMinWeight;
                        }
                        else
                        {
                            bestDayOptions.PossibleMax[j] = dishes[j].PossibleMaxWeight;
                            bestDayOptions.PossibleMin[j] = dishes[j].PossibleMinWeight + (dishes[j].PossibleMaxWeight - dishes[j].PossibleMinWeight) / 2;
                        }
                    }
                    CalculateBestDay(dishes);
                }// repeat condition



            }
        }

        private DayCalculation CalculateDay(List<DishParam> dishes)
        {
            var day = new DayCalculation(dishes.Count);

            for (int j = 0; j < dishes.Count; j++)
            {
                day.DishWeight[j] = dishes[j].PossibleWeight;


                if (!dishes[j].IsInterval) day.DishConstWeight[j] = dishes[j].PossibleWeight;
                day.Calories += dishes[j].CaloriesPer100 * dishes[j].PossibleWeight / 100;
                day.Proteins += dishes[j].ProteinsPer100 * dishes[j].PossibleWeight / 100;
                day.Fats += dishes[j].FatsPer100 * dishes[j].PossibleWeight / 100;
                day.Carbohydrates += dishes[j].CarbohydratesPer100 * dishes[j].PossibleWeight / 100;
            }
            decimal breakfast = dishes.Where(x => x.MealType == 0).Sum(x => x.PossibleWeight * x.CaloriesPer100);
            decimal lunch = dishes.Where(x => x.MealType == 1).Sum(x => x.PossibleWeight * x.CaloriesPer100);
            decimal dinner = dishes.Where(x => x.MealType == 2).Sum(x => x.PossibleWeight * x.CaloriesPer100);
            decimal sum = breakfast + lunch + dinner;
            day.BreakfastCoefficient = (breakfast / sum * 100);
            day.DinnerCoefficient = (dinner / sum * 100);
            day.LunchCoefficient = (lunch / sum * 100);

            day.DayCoefficient = GetDayCoefficient(day);

            return day;
        }

    }
}
