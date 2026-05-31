using Microsoft.AspNetCore.Mvc;
using sharecare_backend.Models.User;
using System.IO;
using System.Reflection;
using System.Text.Json;
using sharecare_backend.Services;

namespace sharecare_backend.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbService _dbService;

        public UserController(ILogger<UserController> logger, DbService db)
        {
            _logger = logger;
            _dbService = db;
        }

        [HttpGet]
        public async Task<IActionResult> GETAllUsers()
        {
            try
            {
                IEnumerable<UserEntity> users = await _dbService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec);
                return Content("Error retrieving data: " + ec.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> POSTCreateUser([FromBody] UserEntity user)
        {
            try
            {
                await _dbService.CreateUserAsync(user);
                return Content("Great Success!");
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec);
                return Content("Error: " + ec.Message);
            }
        }

        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GETUserFromId(int id)
        {
            UserEntity user = await _dbService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"ProblemEntity with ID {id} not found." });
            }

            return Ok(user);
        }
        */
    }
}
