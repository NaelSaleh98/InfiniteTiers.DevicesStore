using InfiniteTiers.DevicesStore.Data.Models;
using System.Collections.Generic;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Get all categories and the related devices.
        /// </summary>
        /// <returns>
        /// List of all categories ordered by Name.
        /// </returns>
        public IEnumerable<Category> GetAll();

        /// <summary>
        /// Get specific Category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Category and related devices.</returns>
        public Category GetById(int? id);

        /// <summary>
        /// Save new category to database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>true if success, false if failed.</returns>
        public bool Save(Category category);

        /// <summary>
        /// Update exist category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>true if success, false if failed.</returns>
        public bool Update(Category category);

        /// <summary>
        /// Delete exist category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if success, flase if failed.</returns>
        public bool Delete(int? id);

        /// <summary>
        /// determine if category exist or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if exist, false if not.</returns>
        public bool IsExist(int id);
    }
}
