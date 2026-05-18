using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using api.Models;
using api.Dtos;
using api.interfaces;
using api.Dtos.Account;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(
    ITokenServices tokenServices,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ILogger<AccountController> logger
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDtos model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid model state"
                });

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var addToRoleResult =
                    await userManager.AddToRoleAsync(user, "User");

                if (!addToRoleResult.Succeeded)
                {
                    logger.LogError(
                        "Failed to add user {Email} to role 'User': {Errors}",
                        model.Email,
                        string.Join(", ",
                            addToRoleResult.Errors
                                .Select(e => e.Description))
                    );

                    return StatusCode(500, new
                    {
                        success = false,
                        message =
                            "User created but failed to assign role."
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
                        Token = tokenServices.CreateToken(user)
                    }
                });
            }

            return BadRequest(new
            {
                success = false,
                message = string.Join(", ",
                    result.Errors.Select(e => e.Description))
            });
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while registering user {Email}",
                model.Email
            );

            return StatusCode(500, new
            {
                success = false,
                message =
                    "An unexpected error occurred. Please try again later."
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDtos model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                success = false,
                message = "Invalid model state"
            });

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid username or password"
            });

        var result = await signInManager
            .CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid username or password"
            });

        return Ok(new
        {
            success = true,
            message = "Login successful",
            data = new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenServices.CreateToken(user)
            }
        });
    }
}