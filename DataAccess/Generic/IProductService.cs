using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Utils;
using Contracts;
using entities.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataAccess.Generic
{
    internal interface IProductService
    {
        Task<Result<IEnumerable<ProductDTO>>> GetAllAsync();
        Task<Result<ProductDTO?>> GetByIdAsync(int id);
        Task<Result<ProductDTO>> CreateAsync(ProductDTO dto);
        Task<Result<ProductDTO>> UpdateAsync(int id, ProductDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
    }

    public class ProductService: IProductService 
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllAsync();

                var productDtos = new List<ProductDTO>();

                foreach (var product in products)
                {
                    var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

                    productDtos.Add(new ProductDTO
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        CategoryName = category?.Name?? "Sin categoria"
                    });
                }

                return Result<IEnumerable<ProductDTO>>.Ok(productDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDTO>>.Fail(ex.Message);
            }
        }

        public async Task<Result<ProductDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);

                if (product != null)
                {
                    var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);
                    var productDto = new ProductDTO()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        CategoryName = category?.Name?? "Sin categoria"
                    };

                    return Result<ProductDTO?>.Ok(productDto, "");
                }
                else
                {
                    return Result<ProductDTO?>.Fail("No se encontro ningun Producto");  
                }
            }
            catch (Exception ex)
            {
                return Result<ProductDTO?>.Fail($"Ocurrio un error interno: {ex.Message}");
            }
        }
        public Task<Result<ProductDTO>> CreateAsync(ProductDTO dto)
        {
            try
            {
                var newProduct = new Product()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Quantity = dto.Quantity,

                } 
                //var newProduct = _unitOfWork.Products.CreateAsync(dto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
    
}
