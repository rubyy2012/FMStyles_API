using AutoMapper;
using FMStyles_API.DTOs;
using FMStyles_API.IRepository;
using FMStyles_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("totalPages")]
        [ProducesResponseType(200)]
        public IActionResult GetTotalPages([FromQuery] FilterRequestDto filter)
        {
            int totalPages = _supplierRepository.GetTotalPages(filter);
            if (totalPages == 0)
            {
                return BadRequest("Không có nhà cung cấp nào!");
            }
            return Ok(totalPages);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SupplierResponseDto>))]
        public IActionResult GetAllSuppliers([FromQuery] FilterRequestDto filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var suppliersPagination = _mapper.Map<PaginationResponseDto>(_supplierRepository.GetAllSuppliers(filter));
            if (suppliersPagination == null)
            {
                return BadRequest("Không tìm thấy kết quả");
            }
            return Ok(suppliersPagination);
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
            var supplierAdd = _supplierRepository.CreateSupplier(supplierMap);
            if (!supplierAdd)
            {
                return BadRequest(new ResponseDto() 
                { Message = "Số điện thoại hoặc email đã tồn tại!",
                  Code = "0"
                });
            }
            else
            {
                return Ok(new ResponseDto()
                {
                    Message = "Tạo thành công nhà cung cấp!",
                    Code = "1"
                });
            }
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
        public ActionResult<ResponseDto> UpdateSupplier(int supplierId,SupplierRequestDto supplier)
        {
            Console.WriteLine("test" + supplier);
            if(supplier==null)
            {
                Console.WriteLine("null");
                ModelState.AddModelError("", "Bạn chưa nhập thông tin nhà cung cấp");
                return BadRequest(ModelState);
            }
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var supplierMap = _mapper.Map<Supplier>(supplier);
            var response = _supplierRepository.UpdateSupplier(supplierId, supplierMap);
            if(!response.isSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
