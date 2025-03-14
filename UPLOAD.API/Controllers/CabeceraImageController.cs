﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPLOAD.API.UnitsOfWork.Implementations;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CabeceraImageController : GenericController<CabeceraImage>
    {
        private readonly ICabeceraImagenesUnitOfWork _cabeceraImagenesUnitOfWork;

        public CabeceraImageController(IGenericUnitOfWork<CabeceraImage> unitOfWork, ICabeceraImagenesUnitOfWork cabeceraImagenesUnitOfWork) : base(unitOfWork)
        {
            _cabeceraImagenesUnitOfWork = cabeceraImagenesUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _cabeceraImagenesUnitOfWork.GetAsync();
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _cabeceraImagenesUnitOfWork.GetAsync(id);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpPost("async")]
        public async Task<IActionResult> Post([FromBody] CabeceraImage cabeceraImage)
        {
            if (cabeceraImage == null)
            {
                return BadRequest(new { message = "Los datos de la cabecera son inválidos." });
            }

            var response = await _cabeceraImagenesUnitOfWork.AddAsync(cabeceraImage);

            if (!response.WasSuccess)
            {
                return BadRequest(new { message = response.Message });
            }

            return CreatedAtAction(nameof(GetAsync), new { id = response.Result.Id }, response.Result);
        }
    }
}