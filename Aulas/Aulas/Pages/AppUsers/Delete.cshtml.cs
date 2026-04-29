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
    public class DeleteModel : PageModel
    {
        private readonly Aulas.Data.ApplicationDbContext _context;

        public DeleteModel(Aulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myuser = await _context.AppUsers.FindAsync(id);
            if (myuser != null)
            {
                MyUser = myuser;
                _context.AppUsers.Remove(MyUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
