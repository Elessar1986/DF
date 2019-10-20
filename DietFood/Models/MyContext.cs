using Microsoft.EntityFrameworkCore;
using System;

namespace DietFood.Models
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base()
        {
            Database.EnsureCreated();
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Week> Weeks { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DietProgram> DietPrograms { get; set; }
        public DbSet<Calculation> Calculations { get; set; }
        public DbSet<DishCalculation> DishCalculations { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
              .Property(c => c.IsInterval)
               .HasConversion<int>();

            modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient[]
            {
                new Ingredient { Name="Свекла", Proteins = 1.5M, Fats = 0.1M, Carbohydrates = 8.8M, Calories= 40.0M, Id = 1 },
                new Ingredient { Name="Курица (грудка вареная)", Proteins = 29.80M, Fats = 1.8M, Carbohydrates = 0.5M, Calories= 137.0M, Id = 2 },
                new Ingredient { Name="Овсяная каша на молоке", Proteins = 3.2M, Fats = 4.1M, Carbohydrates = 14.20M, Calories= 102.0M, Id = 3 },
                new Ingredient { Name="Орехи", Proteins = 15M, Fats = 40M, Carbohydrates = 20M, Calories= 500.0M, Id = 4 },
                new Ingredient { Name="Смесь Ягод", Proteins = 3.2M, Fats = 4.1M, Carbohydrates = 14.20M, Calories= 102.0M, Id = 5 },
                new Ingredient { Name="Тирамису", Proteins = 9.8M, Fats = 10.8M, Carbohydrates = 46.7M, Calories= 292.0M, Id = 6 },
                new Ingredient { Name="Телятина в Сливочно-Мясном Соусе", Proteins = 22.6M, Fats = 12.8M, Carbohydrates = 9.4M, Calories= 243M, Id = 7 },
                new Ingredient { Name="Кус-Кус с Овощами", Proteins = 1.3M, Fats = 6.3M, Carbohydrates = 7.3M, Calories= 91M, Id = 8 },
                new Ingredient { Name="Морковный Крем-Суп", Proteins = 3M, Fats = 13.5M, Carbohydrates = 13.1M, Calories= 178M, Id = 9 },
                new Ingredient { Name="Салат с Тунцом", Proteins = 9M, Fats = 8M, Carbohydrates = 3M, Calories= 120M, Id = 10 },
                new Ingredient { Name="Яйцо Пашот на Подушке из Шпината", Proteins = 7.2M, Fats = 15.9M, Carbohydrates = 1.7M, Calories= 176M, Id = 11 }

            });
            modelBuilder.Entity<DietProgram>().HasData(
            new DietProgram[]
            {
                new DietProgram { Name="Похудение ( w / m)",  Id = 1, Calories = 20M, Proteins = 2M, Fats = 0.5M, Carbohydrates = 1.5M },
                new DietProgram { Name="Набор массы ( m )",  Id = 2, Calories = 35M, Proteins = 2M, Fats = 0.5M, Carbohydrates = 4M },
                new DietProgram { Name="Фитнесс ( w )",  Id = 3, Calories = 25M, Proteins = 1.5M, Fats = 0.5M, Carbohydrates = 3M },
                new DietProgram { Name="Баланс ( w / m )",  Id = 4, Calories = 25M, Proteins = 1.3M, Fats = 0.5M, Carbohydrates = 2M },
                new DietProgram { Name="Вегитарианец ( w / m )",  Id = 5, Calories = 25M, Proteins = 1M, Fats = 1.5M, Carbohydrates = 1M },
                new DietProgram { Name="Офис ( w / m )",  Id = 6, Calories = 20M, Proteins = 1.3M, Fats = 0.5M, Carbohydrates = 1.5M }
            });
            modelBuilder.Entity<Week>().HasData(
           new Week[]
           {
                new Week { Name="Неделя 1",  Id = 1, DietProgramId = 1, Created = DateTime.Now },
                new Week { Name="Неделя 2",  Id = 2, DietProgramId = 2, Created = DateTime.Now },
                new Week { Name="Неделя 3",  Id = 3, DietProgramId = 3, Created = DateTime.Now },
                new Week { Name="Неделя 4",  Id = 4, DietProgramId = 4, Created = DateTime.Now },
                new Week { Name="Неделя 5",  Id = 5, DietProgramId = 5, Created = DateTime.Now },
                new Week { Name="Неделя 6",  Id = 6, DietProgramId = 6, Created = DateTime.Now }
           });
            base.OnModelCreating(modelBuilder);
        }
    }
}
