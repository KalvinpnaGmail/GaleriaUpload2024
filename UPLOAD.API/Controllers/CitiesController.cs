using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : GenericController<City>
    {
        public CitiesController(IGenericUnitOfWork<City> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
