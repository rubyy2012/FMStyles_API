using FMStyles_API.DTOs;
using FMStyles_API.Models;
using System.Collections.Generic;

namespace FMStyles_API.IRepository
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetAllSuppliers();
        Supplier GetSupplierById(int supplierId);
        bool CreateSupplier(Supplier supplier);
        bool UpdateSupplier(int supplierId,Supplier supplier);
        bool SupplierExists(string supplierCode);
        bool DeleteSupplier(int supplierId);
        bool Save();
    }
}
