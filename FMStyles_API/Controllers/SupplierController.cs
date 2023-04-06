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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SupplierResponseDto>))]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _mapper.Map<List<SupplierResponseDto>>(_supplierRepository.GetAllSuppliers());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(suppliers);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSupplier([FromBody] SupplierRequestDto supplier) 
        {
            if (supplier == null)
            {
                ModelState.AddModelError("", "Thông tin rỗng");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var supplierMap = _mapper.Map<Supplier>(supplier);
            if(!_supplierRepository.CreateSupplier(supplierMap))
            {
                return StatusCode(400, "Id danh mục nhà cung cấp không tồn tại");
            }
            return Ok("Create successfully!");
        }

        [HttpDelete("{supplierId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteSupplier(int supplierId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _supplierRepository.DeleteSupplier(supplierId);

            return Ok("Delete successfully!");
        }

        [HttpGet("{supplierId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<SupplierResponseDto> GetSupplierById(int supplierId) 
        { 
            var supplier = _supplierRepository.GetSupplierById(supplierId);
            if(supplier!=null)
            {
                var supplierMap = _mapper.Map<SupplierResponseDto>(supplier);
                   return supplierMap;
            }
            return StatusCode(404,"Không tìm thấy nhà cung cấp");
        }

        [HttpPut("{supplierId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult<Supplier> UpdateSupplier(int supplierId,SupplierRequestDto supplier)
        {
            if(supplier==null)
            {
                ModelState.AddModelError("", "Bạn chưa nhập thông tin nhà cung cấp");
                return BadRequest(ModelState);
            }
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var supplierMap = _mapper.Map<Supplier>(supplier);
            if(_supplierRepository.UpdateSupplier(supplierId, supplierMap))
            {
                return Ok("Update successfully!");
            }
            return NotFound();
        }


    }
}
