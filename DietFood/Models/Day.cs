using DietFood.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class Day
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Meal> Meals { get; set; }

        public int WeekId { get; set; }

        public Week Week { get; set; }

        public DaysOfWeek DayName { get; set; }

        public int Done { get; set; }

        public List<Calculation> Calculations { get; set; }
    }
}
