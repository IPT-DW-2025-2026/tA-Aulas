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
    public class IndexModel : PageModel
    {
        private readonly Aulas.Data.ApplicationDbContext _context;

        public IndexModel(Aulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MyUser> MyUser { get;set; } = default!;

        public async Task OnGetAsync()
        {
            MyUser = await _context.AppUsers.ToListAsync();
        }
    }
}
