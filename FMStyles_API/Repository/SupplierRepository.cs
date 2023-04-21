using Dapper;
using FMStyles_API.DataConfig;
using FMStyles_API.DTOs;
using FMStyles_API.IRepository;
using FMStyles_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FMStyles_API.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DataContext _dataContext;
        private readonly IDbConnection _connection;

        public SupplierRepository(DataContext dataContext, IDbConnection connection)
        {
            _dataContext = dataContext;
            _connection = connection;
        }

        public bool CreateSupplier(Supplier supplier)
        {

            try
            {
                if (!SupplierExists(supplier.Email, supplier.Phone))
                {
                    supplier.CreatedAt = DateTime.Now;
                    supplier.UpdatedBy = null;
                    _dataContext.Add(supplier);
                    return Save();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while adding Supplier!" + ex.Message);
                return false;
            }
        }

        public bool DeleteSupplier(int supplierId)
        {
            var getSupplier = _dataContext.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (getSupplier != null)
            {
                getSupplier.DeletedAt = DateTime.Now;
                getSupplier.DeletedBy = 1;
                getSupplier.DeleteFlag = true;
                _dataContext.Update(getSupplier);
                return Save();
            }
            return false;
        }

        public Supplier GetSupplierById(int supplierId)
        {
            _connection.Open();
            var query = @"
                SELECT 
                    s.""Id""
                ,   s.""Code""
                ,   s.""Name""                
                ,   s.""Phone""
                ,   s.""Email""                
                ,   s.""DebtCode""
                ,   s.""ProvinceId""
                ,   s.""DistrictId""                
                ,   s.""CommuneId""
                ,   s.""Address""
                ,   s.""Status""
                ,   s.""CategoryId""
                ,   c.""Id""
                ,   c.""Name""
                FROM 
                    ""Suppliers"" s
                LEFT JOIN ""SuppliersCategories"" c
                ON (
                    s.""CategoryId"" = c.""Id""
                )
                WHERE 
                    s.""Id"" = @supplierId
            ";
            var supplier = _connection.Query<Supplier, SupplierCategory, Supplier>(
                query,
                (supplier, category) =>
                {
                    supplier.CategoryId = category.Id;
                    supplier.SupplierCategory = category;
                    return supplier;
                }, new { supplierId },
                splitOn: "CategoryId"
            ).SingleOrDefault();

            _connection.Close();
            return supplier;
        }


        public string CheckQueryFilter(FilterRequestDto filter, string queryFilter, string pagination)
        {
            if (filter.TextSearch != null)
            {
                 queryFilter += @"
                                         AND (unaccent(s.""Name"") ILIKE unaccent(@search) 
                                         OR unaccent(s.""Email"") ILIKE unaccent(@search))
                                        ";
                
            }
            if (filter.ProvinceId != null)
            {
                 queryFilter += @" AND 
                                         s.""ProvinceId"" = @provinceId
                                      " ;
            }
            if (filter.Status == 0 || filter.Status==1)
            {
                 queryFilter += @" AND 
                                        s.""Status"" = @status
                                    ";
            }
                
            return  queryFilter += pagination;
        }
        public PaginationResponseDto GetAllSuppliers(FilterRequestDto filter)
        { 
            _connection.Open();
            var queryFilter = @"
                          SELECT 
                                s.""Id""
                            ,   s.""Code""
                            ,   s.""Name""                
                            ,   s.""Phone""
                            ,   s.""Email""                
                            ,   s.""DebtCode""
                            ,   s.""ProvinceId""
                            ,   s.""DistrictId""                
                            ,   s.""CommuneId""
                            ,   s.""Address""
                            ,   s.""Status""
                            ,   s.""CategoryId""
                            ,   c.""Id""
                            ,   c.""Name""
                          FROM ""Suppliers"" s 
                          LEFT JOIN 
                            ""SuppliersCategories"" c 
                          ON 
                            s.""CategoryId"" = c.""Id""
                          WHERE s.""DeleteFlag"" = false";

            var queryCount = @"
                            SELECT 
                            COUNT (*) FROM ""Suppliers"" s 
                            WHERE s.""DeleteFlag"" = @deleteFlag  
                        ";
            var pagination = @" ORDER BY s.""Id""
                                        LIMIT @getLimit
                                        OFFSET @getOffset
                                        ";

            //count query filter
            string countFilterQuery = CheckQueryFilter(filter, queryCount, "");
            int countFilter = _connection.QuerySingleOrDefault<int>(countFilterQuery, new
            {
                deleteFlag = false,
                provinceId = filter.ProvinceId,
                status = filter.Status,
                search = "%" + filter.TextSearch + "%",
            });
            if (countFilter <= 0)
            {
                return null;
            }
            int pages = (int)(countFilter / filter.Limit);
            int totalPages = (countFilter % filter.Limit == 0) ? pages : (pages + 1);
            int offset = (int)((filter.Page - 1) * filter.Limit);

            //List query filter
            string myquery = CheckQueryFilter(filter, queryFilter, pagination);
            var listFiltersPagination = _connection.Query<Supplier, SupplierCategory, Supplier>(myquery,
                (supplier, category) =>
                {
                    supplier.SupplierCategory = category;
                    return supplier;
                },
                    new
                    {
                        getLimit = filter.Limit,
                        getOffset = offset,
                        provinceId = filter.ProvinceId,
                        status = filter.Status,
                        search = "%" + filter.TextSearch + "%",
                    }, splitOn: "CategoryId").ToList();

            //Count number item

            int from = (int)((filter.Page - 1) * filter.Limit +1);
            int to = (int)(from + filter.Limit > countFilter ? countFilter : from + filter.Limit - 1);
            //return listFiltersPagination;
            return new PaginationResponseDto()
            {
                MaxItem = countFilter,
                From = from,
                To = to,
                Suppliers = listFiltersPagination,
                TotalPages = totalPages
            };
        }

            public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0? true:false;
        }

        public ResponseDto UpdateSupplier(int supplierId, Supplier supplier)
        {
            var checkSup = _dataContext.Suppliers.FirstOrDefault(x => x.Id == supplierId);
            if (checkSup == null) {
                return new ResponseDto()
                {
                    Message = "Không tồn tại nhà cung cấp!",
                    isSuccess = false
                };
            }
            var checkEmail = _dataContext.Suppliers.Where(s => s.Email != checkSup.Email&&s.DeleteFlag!=true).FirstOrDefault(x => x.Email == supplier.Email);
            var checkPhone = _dataContext.Suppliers.Where(s => s.Phone != checkSup.Phone && s.DeleteFlag != true).FirstOrDefault(x=> x.Phone == supplier.Phone);
 
            if(checkEmail == null && checkPhone == null)
            {
                checkSup.Address = supplier.Address;
                checkSup.Code = supplier.Code;
                checkSup.Email = supplier.Email;
                checkSup.Phone = supplier.Phone;
                checkSup.Name = supplier.Name;
                checkSup.Phone = supplier.Phone;
                checkSup.Status = supplier.Status;
                checkSup.CategoryId = supplier.CategoryId;
                checkSup.ProvinceId = supplier.ProvinceId;
                checkSup.DistrictId = supplier.DistrictId;
                checkSup.CommuneId = supplier.CommuneId;
                checkSup.DebtCode = supplier.DebtCode;
                _dataContext.SaveChanges();
                return new ResponseDto()
                {
                    Message = "Chỉnh sửa nhà cung cấp thành công!"
                };
            }
            return new ResponseDto()
            {
                Message = "Số điện thoại hoặc email đã tồn tại!",
                isSuccess = false
            };

        }

        public bool SupplierExists(string email, string phone)
        {
            var supplier = _dataContext.Suppliers.Any(s=>(s.Email== email || s.Phone == phone)&&s.DeleteFlag==false);
            return supplier;
        }

        public int GetTotalPages(FilterRequestDto filter)
        {
            var query = @"
                            SELECT 
                            COUNT (*) FROM ""Suppliers"" s 
                            WHERE s.""DeleteFlag"" = @deleteFlag 
                            ";
            if (filter.TextSearch != null)
            {
                query += @"AND 
                            UPPER(s.""Name"") LIKE UPPER(@search) 
                           OR 
                            UPPER(s.""Email"") LIKE UPPER(@search)
                          ";
            }
            if (filter.Status ==0 || filter.Status==1)
            {
                query += @"
                           AND 
                            s.""Status""=@status                           
                          ";
            }
            if (filter.ProvinceId != null)
            {
                query += @"
                           AND 
                            s.""ProvinceId""=@provinceId
                          ";
            }
           

            int totalSuppliers = _connection.QuerySingleOrDefault<int>(query, 
                                  new { deleteFlag = false
                                       ,status = filter.Status
                                       ,search = "%" + filter.TextSearch + "%"
                                       ,provinceId = filter.ProvinceId
                                  });
            if (totalSuppliers <= 0)
            {
                return 0;
            }
            int pages = (int)(totalSuppliers / filter.Limit);
            int totalPages = (totalSuppliers % filter.Limit == 0) ? pages : (pages + 1);
            Console.WriteLine("totalPages" + totalPages);
            return totalPages;
        }
    }
}
