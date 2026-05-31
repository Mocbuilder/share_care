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
    public class ProblemController : ControllerBase
    {
        private readonly ILogger<ProblemController> _logger;
        private readonly DbService _dbService;

        public ProblemController(ILogger<ProblemController> logger, DbService db)
        {
            _logger = logger;
            _dbService = db;
        }

        [HttpGet]
        public async Task<IActionResult> GETAllProblems()
        {
            try
            {
                IEnumerable<ProblemDBEntity> dbProblems = await _dbService.GetAllProblemsAsync();

                List<ProblemEntity> problems = dbProblems
                    .Select(dbproblem => dbproblem.ToNormalProblem())
                    .ToList();

                return Ok(problems);
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec);
                return Content("Error retrieving data: " + ec.Message);
            }
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
        /*
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
        */
    }
}
