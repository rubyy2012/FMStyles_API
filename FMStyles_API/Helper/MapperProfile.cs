using AutoMapper;
using FMStyles_API.DTOs;
using FMStyles_API.Models;

namespace FMStyles_API.Helper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Supplier, SupplierRequestDto>().ReverseMap();
            CreateMap<Supplier, SupplierResponseDto>().ReverseMap();
            CreateMap<SupplierCategory, SupplierCategoryDto>().ReverseMap();
        }
    }
}
