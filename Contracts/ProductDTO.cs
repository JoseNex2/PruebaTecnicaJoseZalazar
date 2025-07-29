using entities.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ProductDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "la cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
        public int Quantity { get; set; }
        public int? CategoryId { get; set; }
        public DateTime FechaCreacion { get; set; }




        public ProductDTO(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
        public ProductDTO()
        {
            
        }
    }

    public class ProductDTOSalida
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public double Price { get; set; }

        public int Quantity { get; set; }

        public CategoryDTOSalida? Category { get; set; }

        public ProductDTOSalida(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public ProductDTOSalida()
        {
            
        }
    }
}
