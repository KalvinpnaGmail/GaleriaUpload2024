using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Shared.Entities;
using UPLOAD.API.Data;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("/api/countries")]

    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Post(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            //el ok(county) me devuelve el pais como quedo sino lo necestio lo puedo mandar vacio ok()
            return Ok(country);
        }
    }


}
