using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/kingdoms")]
    [ApiController]
    public class KingdomRestController : ControllerBase
    {
        private readonly IKingdomService _kingdomService;
        private readonly IBattleService _battleService;
        public KingdomRestController(IKingdomService kingdomService, IBattleService battleService)
        {
            _kingdomService = kingdomService;
            this._battleService = battleService;
        }

        [HttpGet("")]
        public IActionResult Index(int page, int itemCount)
        {
            KingdomsDTO[] kingdoms = _kingdomService.GetKingdoms(page, itemCount);
            return Ok(kingdoms);
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            KingdomDTO kingdomDTO = _kingdomService.GetKindom(id);
            if (kingdomDTO == null)
            {
                ErrorDTO error = new ErrorDTO(_kingdomService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error.Equals("Invalid kingdom Id"))
                    result.StatusCode = 400;
                else
                    result.StatusCode = 404;
                return result;
            }
            else
            {
                return Ok(kingdomDTO);
            }
        }

        [HttpPost("")]
        public IActionResult Store(CreateKingdomDTO createKingdomDTO)
        {
            Boolean boolean = _kingdomService.AddKingdom(createKingdomDTO);
            if (boolean)
            {
                return Ok();
            }
            else
            {
                ErrorDTO error = new ErrorDTO(_kingdomService.GetError());
                JsonResult result = new JsonResult(error);
                result.StatusCode = 400;
                return result;
            }
        }

        [HttpPost("with_location")]
        public IActionResult StoreWithLocation(KingdomCreateRequestDTO request)
        {
            KingdomCreateResponseDTO response = _kingdomService.AddKingdomWithLocation(request);
            if (response.Error)
            {
                return StatusCode(response.StatusCode, new ErrorDTO(response.Message));
            }
            else
            {
                return StatusCode(response.StatusCode, new StatusDTO(response.Message));
            }
        }

        [HttpGet("battles")]
        public IActionResult GetBattles(int page, int itemCount)
        {
            List<BattleResposeDto> battles = _kingdomService.GetBattles(page, itemCount);
            return Ok(battles);
        }

        [HttpPost("{id}/battles/cost")]
        public IActionResult Cost(BattleRequestDTO requestDTO, int id)
        {
            BattleCostDTO battleCostDTO = _battleService.BattleCost(requestDTO, id);
            if (battleCostDTO == null)
            {
                ErrorDTO error = new ErrorDTO(_battleService.GetError());
                JsonResult result = new JsonResult(error);
                
                if (error.Error.Equals("Kingdom not found"))
                {
                    result.StatusCode = 404;
                }
                else if (error.Error.Equals("Unauthorized"))
                {
                    result.StatusCode = 403;
                }
                else 
                {
                    result.StatusCode = 400;
                }
                return result;
            }
            return Ok(battleCostDTO);
        }

        [HttpPost("{id}/battles/attack")]
        public IActionResult Attack(BattleRequestDTO requestDTO, int id)
        {
            StatusDTO statusDTO = _battleService.CreateBattle(requestDTO, id);
            if (statusDTO == null)
            {
                ErrorDTO error = new ErrorDTO(_battleService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error.Equals("Kingdom not found"))
                {
                    result.StatusCode = 404;
                }
                else if (error.Error.Equals("Unauthorized"))
                {
                    result.StatusCode = 403;
                }
                else
                {
                    result.StatusCode = 400;
                }
                return result;
            }
            return Ok(statusDTO);
        }
    }
}
