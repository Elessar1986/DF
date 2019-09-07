using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DietFood.Models
{
    public class Week
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdate { get; set; }

        [Required]
        public int DietProgramId{ get; set; }

        public DietProgram DietProgram { get; set; }

        public List<Day> Days { get; set; }

    }
}
