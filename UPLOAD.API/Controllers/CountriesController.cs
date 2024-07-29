using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Route("/api/countries")] antes se ponia asi es lo mismo siempre hablando del controlador  la llamada si o si /api/countries
    public class CountriesController : GenericController<Country>
    {
        private readonly ICountriesUnitofWork _countriesUnitofWork;

        public CountriesController(IGenericUnitOfWork<Country> unitOfWork, ICountriesUnitofWork countriesUnitofWork) : base(unitOfWork)
        {
          _countriesUnitofWork = countriesUnitofWork;
        }



        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _countriesUnitofWork.GetAsync();
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();


        }


        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _countriesUnitofWork.GetAsync(id);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();


        }



    }
}
