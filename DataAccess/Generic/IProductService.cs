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

namespace DataAccess.Generic
{
    internal interface IProductService
    {
        Task<Result<IEnumerable<ProductDTOSalida>>> GetAllProductAsync();
        Task<Result<ProductDTOSalida?>> GetProductByIdAsync(int id);
        Task<Result<ProductDTOSalida>> CreateProductAsync(ProductDTO dto);
        Task<Result<ProductDTO>> UpdateProductAsync(int id, ProductDTO dto);
        Task<Result<bool>> DeleteAsync(int id);
    }

    public class ProductService: IProductService 
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<ProductDTOSalida>>> GetAllProductAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllAsync();

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

                    var productDtoS = new ProductDTOSalida()
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

                    return Result<ProductDTOSalida?>.Ok(productDtoS, "");
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
                    CategoryId = dto.CategoryId
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
        public async Task<Result<ProductDTO>> UpdateProductAsync(int id, ProductDTO dto)
        {
            try
            {
                var productEdit = await _unitOfWork.Products.GetByIdAsync(id);

                if (productEdit != null)
                {
                    productEdit.Name = dto.Name;
                    productEdit.Description = dto.Description;
                    productEdit.Price = dto.Price;
                    productEdit.Quantity = dto.Quantity;
                    productEdit.CategoryId = dto.CategoryId;

                    await _unitOfWork.Products.UpdateAsync(id, productEdit);
                    await _unitOfWork.CommitAsync();

                    return Result<ProductDTO>.Ok(dto, "Producto actualizado exitosamente");
                }
                return Result<ProductDTO>.Fail($"El producto con el id {id} no existe");
            }
            catch (Exception ex)
            {
                return Result<ProductDTO>.Fail($"Ha ocurrido un error: {ex.Message}"); 
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);

                if (existingProduct != null)
                {
                    await _unitOfWork.Products.DeleteByEntitiAsync(existingProduct);
                    await _unitOfWork.CommitAsync();
                    
                    return Result<Boolean>.Ok(true, "El producto se borro correctamente");
                }

                return Result<Boolean>.Fail($"El producto con el id {id} no existe");
            }
            catch (Exception ex)
            {
                return Result<Boolean>.Fail($"Ha ocurrido un error: {ex.Message}");
            }
        }
    }
}
