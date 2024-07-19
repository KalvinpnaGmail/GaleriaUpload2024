using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using UPLOAD.API.Data;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("/api/countries")]
    public class PaisesController : ControllerBase
    {
        private readonly DataContext _context;

        public PaisesController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //no me logues todas las operaciones SI SOLO ES LECTURA

            return Ok(await _context.Countries.AsNoTracking().ToListAsync());
        }


        /// <summary>
        /// 
        /// https://www.base64encoder.io/image-to-base64-converter/
        /// </summary>

        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(Country country)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            return Ok(country);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country is null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Country country)
        {
            _context.Update(country);
            await _context.SaveChangesAsync();
            return Ok(country);
            //NoContent es igual al ok pero no me interesa que devuelve
            //return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }

}

