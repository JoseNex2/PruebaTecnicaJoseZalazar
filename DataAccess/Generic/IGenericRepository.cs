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
        Task<T> GetByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(int id, T entity);
        Task<bool> DeleteByIdAsync(int id);
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
        public async Task<T> GetByIdAsync(int id)
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
        public async Task<bool> CreateAsync(T entiti)
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
        public async Task<bool> UpdateAsync(int id, T entity)
        {
            try
            {
                T existingEntity = await GetByIdAsync(id);
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
        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                T entity = await GetByIdAsync(id);
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
