using InfiniteTiers.DevicesStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetCategory(int? id);
        public void SaveCategory(Category category);
        public void UpdateCategory(Category category);
        public void DeleteCategory(int id);
        public bool CategoryExists(int id);
    }
}
