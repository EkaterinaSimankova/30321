using Simankova.Domain.Entities;
using Simankova.Domain.Models;

namespace Simankova.UI.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
