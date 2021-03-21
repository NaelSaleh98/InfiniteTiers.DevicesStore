using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AuthDbContext _context;

        public CategoryRepository (AuthDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll()
        {
            var categories = _context.Categories
                            .Include(c => c.Devices)
                            .OrderBy(c => c.Name);
            return categories.ToList();
        }

        public  Category GetCategory(int? id)
        {
            var category =  _context.Categories
                            .Include(c => c.Devices)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.CategoryId == id);

            return category;
        }

        public void SaveCategory(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category =  _context.Categories
                            .Include(c => c.Devices)
                            .Single(c => c.CategoryId == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
