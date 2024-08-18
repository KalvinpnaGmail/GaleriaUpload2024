using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.UnitsOfWork.Implementations;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Route("/api/provincias")] antes se ponia asi es lo mismo siempre hablando del controlador  la llamada si o si /api/provincias
    public class ProvinciasController : GenericController<Provincia>
    {
        private readonly IProvinciasUnitOfWork _provinciasUnitOfWork;

        public ProvinciasController(IGenericUnitOfWork<Provincia> unitOfWork, IProvinciasUnitOfWork  provinciasUnitOfWork) : base(unitOfWork)
        {
            _provinciasUnitOfWork = provinciasUnitOfWork;
        }


        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _provinciasUnitOfWork.GetAsync();
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();


        }


        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _provinciasUnitOfWork.GetAsync(id);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();


        }


       [HttpGet]
      public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _provinciasUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _provinciasUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }




    }
}
