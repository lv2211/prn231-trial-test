using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PE.Infrastructure;
using PE.Infrastructure.Databases;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class EditModel : PageModel
    {
        private readonly PE.Infrastructure.Databases.EnglishPremierLeague2024DbContext _context;

        public EditModel(PE.Infrastructure.Databases.EnglishPremierLeague2024DbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FootballPlayer FootballPlayer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballplayer =  await _context.FootballPlayers.FirstOrDefaultAsync(m => m.FootballPlayerId == id);
            if (footballplayer == null)
            {
                return NotFound();
            }
            FootballPlayer = footballplayer;
           ViewData["FootballClubId"] = new SelectList(_context.FootballClubs, "FootballClubId", "FootballClubId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FootballPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FootballPlayerExists(FootballPlayer.FootballPlayerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FootballPlayerExists(string id)
        {
            return _context.FootballPlayers.Any(e => e.FootballPlayerId == id);
        }
    }
}
