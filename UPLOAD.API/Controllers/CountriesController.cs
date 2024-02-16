using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.Data;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("/api/countries")]
    public class CountriesControlle:ControllerBase
    {
        private readonly DataContext _context;

        public CountriesControlle(DataContext context)
        {
            _context = context;
        }


      
    }
}
