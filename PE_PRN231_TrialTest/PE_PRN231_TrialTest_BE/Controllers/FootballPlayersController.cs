using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PE.Infrastructure;
using PE.Infrastructure.Databases;

namespace PE_PRN231_TrialTest_BE.Controllers
{
    [Route("api/premeir-league-app")]
    [ApiController]
    public class FootballPlayersController : ControllerBase
    {
        private readonly EnglishPremierLeague2024DbContext _context;

        public FootballPlayersController(EnglishPremierLeague2024DbContext context)
        {
            _context = context;
        }

        // GET: api/FootballPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FootballPlayer>>> GetFootballPlayers()
        {
            return await _context.FootballPlayers.ToListAsync();
        }

        // GET: api/FootballPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FootballPlayer>> GetFootballPlayer(string id)
        {
            var footballPlayer = await _context.FootballPlayers.FindAsync(id);

            if (footballPlayer == null)
            {
                return NotFound();
            }

            return footballPlayer;
        }

        // PUT: api/FootballPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFootballPlayer(string id, FootballPlayer footballPlayer)
        {
            if (id != footballPlayer.FootballPlayerId)
            {
                return BadRequest();
            }

            _context.Entry(footballPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FootballPlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FootballPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FootballPlayer>> PostFootballPlayer(FootballPlayer footballPlayer)
        {
            _context.FootballPlayers.Add(footballPlayer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FootballPlayerExists(footballPlayer.FootballPlayerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFootballPlayer", new { id = footballPlayer.FootballPlayerId }, footballPlayer);
        }

        // DELETE: api/FootballPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFootballPlayer(string id)
        {
            var footballPlayer = await _context.FootballPlayers.FindAsync(id);
            if (footballPlayer == null)
            {
                return NotFound();
            }

            _context.FootballPlayers.Remove(footballPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FootballPlayerExists(string id)
        {
            return _context.FootballPlayers.Any(e => e.FootballPlayerId == id);
        }
    }
}
