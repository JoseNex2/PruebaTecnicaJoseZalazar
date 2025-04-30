using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(T entity);
        Task<bool> UpdateProductAsync(int id, T entity);
        Task<bool> DeleteProductByIdAsync(int id);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.Context.Set<T>().ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<T> GetProductByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Context.Set<T>().FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> CreateProductAsync(T entiti)
        {
            try
            {
                await _unitOfWork.Context.Set<T>().AddAsync(entiti);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public async Task<bool> UpdateProductAsync(int id, T entity)
        {
            try
            {
                T existingEntity = await GetProductByIdAsync(id);
                if (existingEntity != null)
                {
                    PropertyInfo[] properties = entity.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        var newValue = property.GetValue(entity);
                        if (newValue != null && property.Name != "Id")
                        {
                            property.SetValue(existingEntity, newValue);
                        }
                    }
                    _unitOfWork.Context.Set<T>().Update(existingEntity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            try
            {
                T entity = await GetProductByIdAsync(id);
                if(entity != null)
                {
                    _unitOfWork.Context.Set<T>().Remove(entity);
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
