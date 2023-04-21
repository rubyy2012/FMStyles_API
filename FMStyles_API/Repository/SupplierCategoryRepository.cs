using Dapper;
using FMStyles_API.DataConfig;
using FMStyles_API.DTOs;
using FMStyles_API.IRepository;
using FMStyles_API.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FMStyles_API.Repository
{
    public class SupplierCategoryRepository : ISupplierCategoryRepository
    {
        private readonly IDbConnection _connection;
        private readonly DataContext _dataContext;

        public SupplierCategoryRepository(IDbConnection connection,DataContext dataContext)
        {
            _connection = connection;
            _dataContext = dataContext;
        }

        public bool CategoryExists(int id)
        {
            return GetListSupplierCategories().Any(c=>c.Id==id);
        }

        public IEnumerable<SupplierCategory> GetListSupplierCategories()
        {
            _connection.Open();
            var query = @"
                            SELECT 
                                 s.""Id""
                                ,s.""Name""
                            FROM ""SuppliersCategories"" s
                         ";
            var listCategories = _connection.Query<SupplierCategory>(query).ToList();
            return listCategories;
        }

        public SupplierCategory GetSupplierCategoryById(int categoryId)
        {
            var sql = @"    
                            SELECT 
                                 s.""Id""
                                ,s.""Name""
                            FROM ""SuppliersCategories""  s
                            WHERE s.""Id""=@categoryId
                      ";
            var supplierCategory = _connection.Query<SupplierCategory>(sql,new { categoryId}).SingleOrDefault();
            return supplierCategory;
        }
    }
}
