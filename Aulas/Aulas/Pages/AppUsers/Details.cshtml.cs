using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Aulas.Data;
using Aulas.Data.Model;

namespace Aulas.Pages.AppUsers
{
    public class DetailsModel : PageModel
    {
        private readonly Aulas.Data.ApplicationDbContext _context;

        public DetailsModel(Aulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public MyUser MyUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myuser = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == id);

            if (myuser is not null)
            {
                MyUser = myuser;

                return Page();
            }

            return NotFound();
        }
    }
}
