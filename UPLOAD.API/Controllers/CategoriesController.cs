using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.UnitsOfWork.Implementations;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class CategoriesController : GenericController<Category>
    {
        private readonly ICategoriesUnitOfWork _categoriesUnitOfWork;

        // GET: CategoryController
        public CategoriesController(IGenericUnitOfWork<Category > unitOfWork, ICategoriesUnitOfWork categoriesUnitOfWork) : base(unitOfWork)
        {
            _categoriesUnitOfWork = categoriesUnitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _categoriesUnitOfWork.GetComboAsync());
        }
        //public async Task<IEnumerable<Category>> GetComboAsync() => await _categoriesRepository.GetComboAsync();



    }
}
