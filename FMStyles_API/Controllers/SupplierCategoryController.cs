using AutoMapper;
using FMStyles_API.DTOs;
using FMStyles_API.IRepository;
using FMStyles_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FMStyles_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class SupplierCategoryController : ControllerBase
    {
        private readonly ISupplierCategoryRepository _supplierCategory;
        private readonly IMapper _mapper;

        public SupplierCategoryController(ISupplierCategoryRepository supplierCategory, IMapper mapper)
        {
            _supplierCategory = supplierCategory;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SupplierCategoryDto>))]
        public IActionResult GetListSupplierCategories()
        {
            var listSupplierCategories = _mapper.Map<IEnumerable<SupplierCategoryDto>> (_supplierCategory.GetListSupplierCategories()).ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(listSupplierCategories);
        }

        [HttpGet("{categoryId:int}")]
        [ProducesResponseType(200, Type = typeof(SupplierCategoryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetSupplierCategoryById(int categoryId)
        {
            if(!_supplierCategory.CategoryExists(categoryId))
            {
                ModelState.AddModelError("categoryId", "Danh mục không tồn tại!");
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<SupplierCategoryDto>(_supplierCategory.GetSupplierCategoryById(categoryId));
            return Ok(category);
        }

    }
}
