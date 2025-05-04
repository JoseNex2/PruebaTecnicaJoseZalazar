using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entities.Domain;

namespace entities.DataContext.Dtos
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string name { get; set; }

    }
}
