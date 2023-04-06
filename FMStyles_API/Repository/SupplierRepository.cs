using Dapper;
using FMStyles_API.DataConfig;
using FMStyles_API.DTOs;
using FMStyles_API.IRepository;
using FMStyles_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FMStyles_API.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DataContext _dataContext;
        private readonly IDbConnection _connection;

        public SupplierRepository(DataContext dataContext,IDbConnection connection)
        {
            _dataContext = dataContext;
            _connection = connection;
        }

        public bool CreateSupplier(Supplier supplier)
        {
            var category = _dataContext.SuppliersCategories.Where(sc => sc.Id == supplier.CategoryId).FirstOrDefault();
            if(category != null) 
            {
                if (!SupplierExists(supplier.Code))
                {
                    supplier.CreatedAt = DateTime.Now;
                    supplier.UpdatedBy = null;
                    _dataContext.Add(supplier);
                    return Save();
                }
                return false;
            }
            return false;
        }

        public bool DeleteSupplier(int supplierId)
        {
            var supplier = GetSupplierById(supplierId);
            if (supplier != null) 
            {
                _dataContext.Remove(supplier);
            }
            return Save();
        }

        public Supplier GetSupplierById(int supplierId)
        {
            _connection.Open();
            var query = "SELECT s.*, c FROM public.\"Suppliers\" s LEFT JOIN public.\"SuppliersCategories\" c ON s.\"CategoryId\" = c.\"Id\" Where s.\"Id\" = @supplierId";
            var listSuppliers = _connection.Query<Supplier, SupplierCategory, Supplier>(
                query,
                (supplier, category) =>
                {
                    supplier.SupplierCategory = category;
                    return supplier;
                }, new { supplierId },
                splitOn: "CategoryId"
            ).SingleOrDefault();

            _connection.Close();
            return listSuppliers;
        }


        public IEnumerable<Supplier> GetAllSuppliers()
        {
            _connection.Open();
            var query = "SELECT s.*, c.* FROM public.\"Suppliers\" s LEFT JOIN public.\"SuppliersCategories\" c on s.\"CategoryId\" = c.\"Id\"";
            var listSuppliers = _connection.Query<Supplier, SupplierCategory, Supplier>(
                query,
                (supplier, category) =>
                {
                    supplier.SupplierCategory = category;
                    return supplier;
                },
                splitOn: "CategoryId"
            ).ToList();
            return listSuppliers;
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SupplierExists(string supplierCode)
        {
            return GetAllSuppliers().Any(c => c.Code == supplierCode);
        }

        public bool UpdateSupplier(int supplierId, Supplier supplier)
        {
            var supplierUpdate = GetSupplierById(supplierId);
            if (supplierUpdate != null) 
            {
                _dataContext.Update(supplier);
                return Save();
            }
            return false;

        }

      
    }
}
