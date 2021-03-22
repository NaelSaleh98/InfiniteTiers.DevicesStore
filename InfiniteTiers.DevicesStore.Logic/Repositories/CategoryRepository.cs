using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        #region private fields
        private readonly AuthDbContext _context;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public CategoryRepository(AuthDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories
                            .Include(c => c.Devices)
                            .OrderBy(c => c.Name);
            return categories;
        }

        public Category GetById(int? id)
        {
            var category = _context.Categories
                            .Include(c => c.Devices)
                            .Single(c => c.CategoryId == id);
            return category;
        }

        public bool Save(Category category)
        {
            try
            {
                _context.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public bool Update(Category category)
        {
            try
            {
                _context.Update(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public bool Delete(int? id)
        {
            try
            {
                var category = GetById(id);
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public bool IsExist(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        } 
        #endregion
    }
}
