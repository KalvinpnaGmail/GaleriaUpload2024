using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class CategoriesController : GenericController<Category>
    {
        // GET: CategoryController
        public CategoriesController(IGenericUnitOfWork<Category > unitOfWork) : base(unitOfWork)
        {
        }
    }
}
