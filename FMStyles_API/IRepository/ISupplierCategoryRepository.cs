using FMStyles_API.Models;
using System.Collections.Generic;

namespace FMStyles_API.IRepository
{
    public interface ISupplierCategoryRepository
    {
        IEnumerable<SupplierCategory> GetListSupplierCategories();
        SupplierCategory GetSupplierCategoryById(int id);
        bool CategoryExists(int id);
    }
}
