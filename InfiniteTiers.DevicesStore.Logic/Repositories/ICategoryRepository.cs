using InfiniteTiers.DevicesStore.Data.Models;
using System.Collections.Generic;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> GetAll();
        public Category GetCategory(int? id);
        public void SaveCategory(Category category);
        public void UpdateCategory(Category category);
        public void DeleteCategory(int id);
        public bool CategoryExists(int id);
    }
}
