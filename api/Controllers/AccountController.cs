using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using api.Models;
using api.Dtos;
using api.interfaces;
using api.Dtos.Account;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenServices tokenServices, ILogger<AccountController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDtos model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid model state" });

                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!addToRoleResult.Succeeded)
                    {
                        _logger.LogError("Failed to add user {Email} to role 'User': {Errors}", model.Email, string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                        return StatusCode(500, new
                        {
                            success = false,
                            message = "User created but failed to assign role. Please contact support."
                        });
                    }
                    return Ok(new
                    {
                        success = true,
                         message = "User registered successfully",
                         data = new NewUserDto
                         {
                             UserName = user.UserName,
                             Email = user.Email,
                             Token = _tokenServices.CreateToken(user)
                         }
                         });
                }

                return BadRequest(new
                {
                    success = false,
                    message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user {Email}", model.Email);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDtos model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid model state" });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { success = false, message = "Invalid username or password" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { success = false, message = "Invalid username or password" });

            return Ok(new
            {
                success = true,
                message = "Login successful",
                data = new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenServices.CreateToken(user)
                }
            });
        }
    }
}