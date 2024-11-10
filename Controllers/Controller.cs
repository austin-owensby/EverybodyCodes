using EverybodyCodes.PuzzleHelper;
using EverybodyCodes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EverybodyCodes.Controllers
{
    [ApiController]
    [Route("api")]
    public class Controller(SolutionService solutionService, PuzzleHelperService puzzleHelperService) : ControllerBase
    {
        private readonly SolutionService solutionService = solutionService;
        private readonly PuzzleHelperService puzzleHelperService = puzzleHelperService;

        /// <summary>
        /// Runs a specific quest's solution, and optionally posts the answer to Everybody Codes and returns the result.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <param name="send">Submit the result to Everybody Codes</param>
        /// <param name="example">Use an example file instead of the regular input, you must add the example at `Inputs/YYYY/DD/P_example.txt`</param>
        /// <response code="200">The result of running the solution. If submitting the solution, also returns the response from Everybody Codes.</response>
        [HttpGet("run-solution")]
        public async Task<ActionResult<string>> GetSolution([FromQuery, BindRequired] int year = Globals.START_YEAR, [FromQuery, BindRequired] int quest = 1, [FromQuery, BindRequired] int part = 1, bool send = false, bool example = false)
        {
            if (send && example)
            {
                return BadRequest("You're attempting to submit your answer to Everybody Codes while using an example input, this is likely a mistake.");
            }

            try
            {
                return await solutionService.GetSolution(year, quest, part, send, example);
            }
            catch (SolutionNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Imports the input from Everybody Codes for a specific quest's part.
        /// </summary>
        /// <remarks>
        /// The program is idempotent (You can run this multiple times as it will only add a file if it is needed.)
        /// </remarks>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <response code="200">A message on what was updated.</response>
        [HttpPost("import-input-file")]
        public async Task<string> ImportInputFile([FromQuery, BindRequired] int year = Globals.START_YEAR, [FromQuery, BindRequired] int quest = 1, [FromQuery, BindRequired] int part = 1)
        {
            return await puzzleHelperService.ImportInputFile(year, quest, part);
        }

        /// <summary>
        /// Creates missing daily solution service files.
        /// </summary>
        /// <remarks>
        /// Useful when a new year has started to preemptively generate the service files for the calendar year before the event starts.
        /// The program is idempotent (You can run this multiple times as it will only add files if they are needed.)
        /// 
        /// You'll likely only need to use this once per year and only if either your source code has gotten out of sync from the `main` branch or I haven't kept it up to date.
        /// </remarks> 
        /// <response code="200">A string describing the updated solution folders/files.</response>
        [HttpPost("generate-service-files")]
        public async Task<string> GenerateMissingSolutionServiceFiles()
        {
            return await puzzleHelperService.GenerateMissingSolutionServiceFiles();
        }
    }
}