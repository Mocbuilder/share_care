/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sharecare_backend.Models.Problem;
using System.IO;
using System.Reflection;
using System.Text.Json;
using sharecare_backend.Services;

namespace sharecare_backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/[controller]/[action]")]
    public class DevController : ControllerBase
    {
        private readonly ILogger<DevController> _logger;
        private readonly DbService _dbService;

        public DevController(ILogger<DevController> logger, DbService db)
        {
            _logger = logger;
            _dbService = db;
        }

        [HttpPost]
        public async Task<IActionResult> POSTCreateProblem([FromBody] ProblemEntity problem)
        {
            try
            {
                ProblemDBEntity newProblem = problem.ToDBProblem();
                await _dbService.CreateProblemAsync(newProblem);

                return Content("Great Success!");
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec);
                return Content("Error: " + ec.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GETProblemFromId(int id)
        {
            ProblemDBEntity dbproblem = await _dbService.GetProbelemByIdAsync(id);
            if (dbproblem == null)
            {
                return NotFound(new { message = $"ProblemEntity with ID {id} not found." });
            }
            ProblemEntity problem = dbproblem.ToNormalProblem();

            return Ok(problem);
        }
    }
}
*/