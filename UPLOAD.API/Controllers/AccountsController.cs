﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UPLOAD.API.Helpers;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.DTOS;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly ImailHelpers _mailHelper;
        private readonly string _container;

        public AccountsController(IUsersUnitOfWork usersUnitOfWork, IConfiguration configuration, IFileStorage fileStorage, ImailHelpers mailHelper)
        {
            _usersUnitOfWork = usersUnitOfWork;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _mailHelper = mailHelper;
            _container = "users";
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
        {
            User user = model;
            //verificamos si el usuario tieene foto
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = Convert.FromBase64String(model.Photo);
                model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpp", _container);
            }

            var result = await _usersUnitOfWork.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _usersUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());

                var response = await SendConfirmationEmailAsync(user);
                if (response.WasSuccess)
                {
                    return NoContent();
                }

                return BadRequest(response.Message);
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            token = token.Replace(" ", "+");//token cuando se manda con espacios en blanco se reemplaza por +
            var user = await _usersUnitOfWork.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var result = await _usersUnitOfWork.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _usersUnitOfWork.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }
            if (result.IsLockedOut)
            {
                return BadRequest("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
            }

            return BadRequest("Email o contraseña incorrectos.");
        }

        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email!),
                new(ClaimTypes.Role, user.UserType.ToString()),
                new("Document", user.Document),
                new("FirstName", user.FirstName),
                new("LastName", user.LastName),
                new("Address", user.Address),
                new("Photo", user.Photo ?? string.Empty),
                new("CityId", user.CityId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _usersUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _usersUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAsync(User user)
        {
            try
            {
                var currentUser = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var photoUser = Convert.FromBase64String(user.Photo);
                    user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
                }

                currentUser.Document = user.Document;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Address = user.Address;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;
                currentUser.CityId = user.CityId;

                ////
                //var updatedUser = new User
                //{
                //    Id = currentUser.Id,
                //    UserName = currentUser.UserName,  // <-- Esto es clave
                //    Email = currentUser.Email,        // <-- También asegúrate de incluir el Email
                //    Document = user.Document,
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    Address = user.Address,
                //    PhoneNumber = user.PhoneNumber,
                //    Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo,
                //    CityId = user.CityId
                //};

                //

                var result = await _usersUnitOfWork.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user)
        {
            var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

            return _mailHelper.SendMail(user.FullName, user.Email!,
                $"Upload Acler - Confirmación de cuenta",
                $"<h1>Upload Acler - Confirmación de cuenta</h1>" +
                $"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
                $"<b><a href ={tokenLink}>Confirmar Email</a></b>");
        }
    }
}