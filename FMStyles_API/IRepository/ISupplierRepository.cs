using FMStyles_API.DTOs;
using FMStyles_API.Models;
using System.Collections.Generic;

namespace FMStyles_API.IRepository
{
    public interface ISupplierRepository
    {
        //IEnumerable<Supplier> GetAllSuppliers(FilterRequestDto filter);       
        PaginationResponseDto GetAllSuppliers(FilterRequestDto filter);
        Supplier GetSupplierById(int supplierId);
        //int CountTotalSupplier();
        string CheckQueryFilter(FilterRequestDto filter, string queryFilter, string pagination);
        bool CreateSupplier(Supplier supplier);
        ResponseDto UpdateSupplier(int supplierId,Supplier supplier);
        int GetTotalPages(FilterRequestDto filter);
        bool SupplierExists(string email, string phone);
        bool DeleteSupplier(int supplierId);
        bool Save();
    }
}
