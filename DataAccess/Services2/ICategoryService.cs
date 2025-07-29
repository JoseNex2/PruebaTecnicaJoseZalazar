using Contracts;
using DataAccess.Utils;
using entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services2
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDTOSalida>>> GetAllCategory();
        Task<Result<CategoryDTOSalida>> GetByName(int id);
        Task<Result<IEnumerable<CategoryDTOSalida>>> GetByNameCategory(string id);
        Task<Result<CategoryDTOSalida>> CreateCategory(Category category);
        Task<Result<CategoryDTOSalida>> UpdateCategory(int id, Category category);
        Task<Result<Boolean>> DeleteById(int id);
    }
}
