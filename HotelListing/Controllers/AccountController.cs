using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using HotelListing.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IMapper mapper,
            IAuthManager authManager,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorReponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO userDTO)
        {
            try
            {
                foreach (var roleDTO in userDTO.Roles)
                {
                    if (!(await _roleManager.RoleExistsAsync(roleDTO)))
                    {
                        var role = new IdentityRole { Name = roleDTO };
                        var creatingRoleResult = await _roleManager.CreateAsync(role);
                        if (!creatingRoleResult.Succeeded)
                        {
                            return BadRequest("Can not create role");
                        }
                    }
                }

                var user = _mapper.Map<ApplicationUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            if (!await _authManager.ValidateUser(userDTO))
            {
                Error error = new Error
                {
                    StatusCode = 401,
                    Message = "Email/password is incorrect",
                    Key = "Email"
                };
                return Unauthorized(error);
            }
            return Accepted(new { Token = await _authManager.CreateToken() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAll();
                var userDTOs = _mapper.Map<List<UserDTO>>(users);
                return Ok(userDTOs);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
