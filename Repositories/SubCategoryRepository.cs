using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SubCategoryRepository: ISubCategoriesRepository
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubCategory>> GetAllSubCategory()
        {
            return await _context.SubCategories.ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryById(string SubCategoryId)
        {
            return await _context.SubCategories.FirstOrDefaultAsync(sc => sc.Id.Equals(SubCategoryId)); 
        }

        public async Task<bool> AddSubCategory( SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Update(subCategory);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteSubCategory(string SubCategoryId)
        {
            SubCategory subCategory = await GetSubCategoryById(SubCategoryId);
            if (subCategory != null)
            {
               _context.SubCategories.Remove(subCategory);
                 await _context.SaveChangesAsync();
            }
        }
    }
}
