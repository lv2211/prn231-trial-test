using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PE.Core.Commons;
using PE.Core.Contracts;
using PE.Core.Dtos;
using PE.Infrastructure;

namespace PE_PRN231_TrialTest_BE.Controllers
{
    [Route("api/premier-leauge")]
    [ApiController]
    public class FootballPlayersController : ODataController
    {
        private readonly IFootballPlayerService _footballPlayerService;

        public FootballPlayersController(
            IFootballPlayerService footballPlayerService)
        {
            _footballPlayerService = footballPlayerService;
        }

        /// <summary>
        /// Get players
        /// </summary>
        /// <returns></returns>
        [HttpGet("players")]
        [EnableQuery]
        [Authorize(Policy = "ReadPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<ActionResult<ApiResponseModel<IEnumerable<FootballPlayerResponse>>>> GetFootballPlayers()
        {
            var players = await _footballPlayerService.GetPlayers();
            if (!players.Any()) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "No data of players!"
            });
            //return Ok(new ApiResponseModel<IEnumerable<FootballPlayerResponse>>
            //{
            //    StatusCode = System.Net.HttpStatusCode.OK,
            //    Message = "Fetch data successfully!",
            //    // OData can work with queryable collection.
            //    Response = players.AsQueryable()
            //});
            return Ok(players.AsQueryable());
        }

        /// <summary>
        /// Get player
        /// </summary>
        /// <param name="id">Player's id</param>
        /// <returns></returns>
        [HttpGet("players/{id}")]
        [Authorize(Policy = "ReadPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FootballPlayerResponse>> GetFootballPlayer(string id)
        {
            var footballPlayer = await _footballPlayerService.GetPlayer(id);

            if (footballPlayer == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Player not found!"
                });
            }
            return Ok(footballPlayer);
        }


        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="id">Player's id</param>
        /// <param name="request">Model for updating player</param>
        /// <returns></returns>
        [HttpPut("player/{id}")]
        [Authorize(Policy = "OtherPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateFootballPlayer(string id, UpdateFootballPlayerRequest request)
        {
            if (id != request.FootballPlayerId)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Id is not matched!"
                });
            }

            if (request.Birthday >= new DateTime(2007, 01, 01))
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Birthday must be before 2007!"
                });
            }
            if (!ModelState.IsValid) return BadRequest();

            var result = await _footballPlayerService.UpdatePlayer(request);
            if (!result) return BadRequest(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Update failed!"
            });

            return NoContent();
        }

        /// <summary>
        /// Create player
        /// </summary>
        /// <param name="request">Model for creating player</param>
        /// <returns></returns>
        [HttpPost("player")]
        [Authorize(Policy = "OtherPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponseModel<string>>> AddFootballPlayer(CreateFootballPlayerRequest request)
        {
            if (request.Birthday >= new DateTime(2007, 01, 01))
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Birthday must be before 2007!"
                });
            if (!ModelState.IsValid) return BadRequest();

            var result = await _footballPlayerService.AddPlayer(request);
            if (!result) return BadRequest(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Add failed!"
            });

            return Ok(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Add successfully!"
            });
        }

        /// <summary>
        /// Delete player
        /// </summary>
        /// <param name="id">Player's id</param>
        /// <returns></returns>
        [HttpDelete("player/{id}")]
        [Authorize(Policy = "OtherPolicy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFootballPlayer(string id)
        {
            var footballPlayer = await _footballPlayerService.GetPlayerById(id);
            if (footballPlayer == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Player not found!"
                });
            }

            var result = await _footballPlayerService.DeletePlayer(footballPlayer);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Delete failed!"
                });
            return NoContent();
        }


        [HttpGet("clubs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FootballClub>>> GetClubs()
        {
            return Ok(await _footballPlayerService.GetClubs());
        }
    }
}
