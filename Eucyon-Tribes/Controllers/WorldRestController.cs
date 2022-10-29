using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.WorldDTOs;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/worlds")]
    [ApiController]
    public class WorldRestController : ControllerBase
    {
        public IWorldService _worldService;

        public WorldRestController(IWorldService worldService)
        {
            _worldService = worldService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var result = _worldService.GetWorldsWithKingdoms();
            if (result == null || result.Count() == 0)
            {
                return StatusCode(500, result);
            }
            string json = JsonSerializer.Serialize<WorldResponseDTO[]>(result);
            return Ok(json);
        }

        [HttpGet("create")]
        public IActionResult Create(string? name)
        {
            var result = _worldService.CreateWorld(name);
            if (!result)
            {
                var error = new ErrorDTO("Given name is unsuitable.");
                string json = JsonSerializer.Serialize<ErrorDTO>(error);
                return BadRequest(json);
            }
            return StatusCode(201);
        }

        [HttpPost("")]
        public IActionResult Store(StoreWorldDTO? newWorld)
        {
            var result = _worldService.StoreWorld(newWorld);
            if (!result)
            {
                var error = new ErrorDTO("Given world cannot be stored.");
                string json = JsonSerializer.Serialize<ErrorDTO>(error);
                return BadRequest(json);
            }
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var result = _worldService.GetWorldDetails(id);
            string json;
            if (result == null)
            {
                var error = new ErrorDTO("Given ID was not found.");
                json = JsonSerializer.Serialize<ErrorDTO>(error);
                return BadRequest(json);
            }
            json = JsonSerializer.Serialize<WorldDetailDTO>(result);
            return Ok(json);
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(int id, string? name)
        {
            var result = _worldService.EditWorld(id, name);
            if (!result)
            {
                var error = new ErrorDTO("World with given ID could not be edited.");
                string json = JsonSerializer.Serialize<ErrorDTO>(error);
                return BadRequest(json);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateWorldDTO worldUpdate)
        {
            var result = _worldService.UpdateWorld(id, worldUpdate);
            if (!result)
            {
                var error = new ErrorDTO("World with given ID could not be updated.");
                string json = JsonSerializer.Serialize<ErrorDTO>(error);
                return BadRequest(json);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Destroy(int id)
        {
            var result = _worldService.DestroyWorld(id);
            if (!result)
            {
                var error = new ErrorDTO("World with given ID could not be destroyed.");
                string json = JsonSerializer.Serialize<ErrorDTO>(error);
                return BadRequest(json);
            }
            return Ok();
        }
    }
}
