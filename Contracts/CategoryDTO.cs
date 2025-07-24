using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }


        public CategoryDTO(string name)
        {
            Name = name;
        }

        public CategoryDTO()
        {
            
        }
    }

    public class CategoryDTOSalida
    {
        public string Name { get; set; }

        public CategoryDTOSalida(string name)
        {
            Name = name;
        }

        public CategoryDTOSalida()
        {
            
        }
    }
}
