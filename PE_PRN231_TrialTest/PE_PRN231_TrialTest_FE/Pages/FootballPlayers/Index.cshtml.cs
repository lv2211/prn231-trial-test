using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PE.Infrastructure;
using PE.Infrastructure.Databases;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class IndexModel : PageModel
    {
        private readonly PE.Infrastructure.Databases.EnglishPremierLeague2024DbContext _context;

        public IndexModel(PE.Infrastructure.Databases.EnglishPremierLeague2024DbContext context)
        {
            _context = context;
        }

        public IList<FootballPlayer> FootballPlayer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FootballPlayer = await _context.FootballPlayers
                .Include(f => f.FootballClub).ToListAsync();
        }
    }
}
