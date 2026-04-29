using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Aulas.Data;
using Aulas.Data.Model;
using System.Globalization;

namespace Aulas.Pages.Students {
   public class CreateModel:PageModel {
      private readonly Aulas.Data.ApplicationDbContext _context;

      public CreateModel(Aulas.Data.ApplicationDbContext context) {
         _context = context;
      }

      public IActionResult OnGet() {
         ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name");
         return Page();
      }

      [BindProperty]
      public Student Student { get; set; } = default!;


      // For more information, see https://aka.ms/RazorPagesCRUD.
      public async Task<IActionResult> OnPostAsync() {

         if(!ModelState.IsValid) {
            ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name", Student.DegreeFK);
            return Page();
         }


         // atribuir o valor auxiliar da Propina ao atributo da Propina,
         // convertendo de string para decimal
         Student.TuitionFee = Convert.ToDecimal(Student.TuitionFeeAux.Replace('.', ','),
                                                new CultureInfo("pt-PT"));

         try {
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();
         }
         catch(Exception) {
            /* ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
             * Tratar a exceção de forma adequada, por exemplo, 
             * mostrando uma mensagem de erro para o utilizador, ou
             * redirecionando para uma página de erro, etc.
             * NUNCA se mostra a mensagem do 'throw' para o utilizador final, 
             * porque pode conter informações sensíveis sobre a aplicação, 
             * como por exemplo, o nome da base de dados, o nome do servidor, etc.
             * +++++++++++++++++++++++++++++++++++++++++++++++++++++++ */
            throw;
         }


         return RedirectToPage("./Index");
      }
   }
}
