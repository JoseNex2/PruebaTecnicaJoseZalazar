using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Utils;
using Contracts;
using entities.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using DataAccess.Generic;

namespace DataAccess.Services2
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductDTOSalida>>> GetAllProductsAsync();
        Task<Result<ProductDTOSalida?>> GetProductByIdAsync(int id);
        Task<Result<IEnumerable<ProductDTOSalida>>> GetProductsByName(string name);
        Task<Result<ProductDTOSalida>> CreateProductAsync(ProductDTO dto);
        Task<Result<ProductDTOSalida>> UpdateProductAsync(int id, ProductDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
    }

    public class ProductService: IProductService 
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<ProductDTOSalida>>> GetAllProductsAsync()
        {
            try
            {
                var products = _unitOfWork.Products.GetAllAsync();

                var productDtos = new List<ProductDTOSalida>();

                foreach (var product in products)
                {
                    productDtos.Add(new ProductDTOSalida
                    {

                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        Category = new CategoryDTOSalida()
                        {
                            Name = product.Category?.Name?? "Sin catgoria"
                        }
                    });
                }

                return Result<IEnumerable<ProductDTOSalida>>.Ok(productDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDTOSalida>>.Fail(ex.Message);
            }
        }

        public async Task<Result<ProductDTOSalida?>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);

                if (product != null)
                {

                    var productDtos = new ProductDTOSalida()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        Category = new CategoryDTOSalida()
                        {
                            Name = product.Category?.Name?? "Sin categoria"
                        }
                    };

                    return Result<ProductDTOSalida?>.Ok(productDtos);
                }
                else
                {
                    return Result<ProductDTOSalida?>.Fail("No se encontro ningun Producto");  
                }
            }
            catch (Exception ex)
            {
                return Result<ProductDTOSalida?>.Fail($"Ocurrio un error interno: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ProductDTOSalida>>> GetProductsByName(string name)
        {
            try
            {
                var products = await _unitOfWork.Products.FindManyAsync(
                    p => p.Name.ToLower() == name.ToLower());

                var productsDtos = new List<ProductDTOSalida>();

                foreach (var product in products)
                {
                    productsDtos.Add(new ProductDTOSalida()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        Category = new CategoryDTOSalida()
                        {
                            Name = product.Category?.Name?? "Sin catgoria"
                        }
                    }); 
                }
                return Result<IEnumerable<ProductDTOSalida>>.Ok(productsDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDTOSalida>>.Fail($"Ha ocurrio un error interno: {ex.Message}");
            }
        }
        public async Task<Result<ProductDTOSalida>> CreateProductAsync(ProductDTO dto)
        {
            try
            {
                var newProduct = new Product()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    CategoryId = dto.CategoryId,
                    FechaCreacion = DateTime.Now
                };

                await _unitOfWork.Products.CreateAsync(newProduct);
                await _unitOfWork.CommitAsync();

                var resultDto = new ProductDTOSalida
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    Quantity = newProduct.Quantity,
                    Category = newProduct.Category != null ?
                    new CategoryDTOSalida { Name = newProduct.Category.Name }: null
                };

                return Result<ProductDTOSalida>.Ok(resultDto, "Producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<ProductDTOSalida>.Fail($"Ha ocurrido un error: {ex.Message}");
            }
        }
        public async Task<Result<ProductDTOSalida>> UpdateProductAsync(int id, ProductDTO dto)
        {
            try
            {
                var existProduct = await _unitOfWork.Products.GetByIdAsync(id);

                if (existProduct != null)
                {
                    existProduct.Name = dto.Name;
                    existProduct.Description = dto.Description;
                    existProduct.Price = dto.Price;
                    existProduct.Quantity = dto.Quantity;
                    existProduct.CategoryId = dto.CategoryId;

                    await _unitOfWork.Products.UpdateAsync(existProduct);
                    await _unitOfWork.CommitAsync();

                    var productdto = new ProductDTOSalida
                    {
                        Id= existProduct.Id,
                        Name = existProduct.Name,
                        Description = existProduct.Description,
                        Price = existProduct.Price,
                        Quantity = existProduct.Quantity,
                        Category = new CategoryDTOSalida()
                        {
                            Name = existProduct.Category?.Name?? "Sin categoria"
                        }
                    };

                    return Result<ProductDTOSalida>.Ok(productdto,"Producto actualizado exitosamente");
                }
                return Result<ProductDTOSalida>.Fail($"El producto con el id {id} no existe");
            }
            catch (Exception ex)
            {
                return Result<ProductDTOSalida>.Fail($"Ha ocurrido un error: {ex.Message}"); 
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);

                if (existingProduct != null)
                {
                    await _unitOfWork.Products.DeleteByEntityAsync(existingProduct);
                    await _unitOfWork.CommitAsync();
                    
                    return Result<bool>.Ok(true, "El producto se borro correctamente");
                }

                return Result<bool>.Fail($"El producto con el id {id} no existe");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Ha ocurrido un error: {ex.Message}");
            }
        }
    }
}
