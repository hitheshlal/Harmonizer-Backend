using Harmonizer.DTO;
using Harmonizer.Services.Interface;
using Harmonizer.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Harmonizer.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginRequestDTO loginRequestDTO)
        {

            if(loginRequestDTO == null)
            {
                return BadRequest("Invalid request.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authRepository.Login(loginRequestDTO);

                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet("GetUserDetailsById")]
        public async Task<IActionResult> GetUserDetailsById(int id)
        {
            if(id <= 0)
            {
                return BadRequest("invalid input");
            }
            try
            {
                var result = await _authRepository.GetUserDetails(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

    }
}
