using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.SHARE.Entities;


namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("/api/countriesViejo")]
    public class CountryViejoController : ControllerBase
    {
        private readonly DataContext _context;

        public CountryViejoController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Countries.ToListAsync());
        }


        /// <summary>
        /// 
        /// https://www.base64encoder.io/image-to-base64-converter/
        /// </summary>

        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            return Ok(country);
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country is null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Country country)
        {
            _context.Update(country);
            await _context.SaveChangesAsync();
            return Ok(country);
        }



        // GET: CountryViejoController
    }
}

