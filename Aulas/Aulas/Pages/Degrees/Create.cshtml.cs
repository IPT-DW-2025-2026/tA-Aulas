using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Aulas.Data;
using Aulas.Data.Model;

namespace Aulas.Pages.Degrees {
   public class CreateModel:PageModel {

      /// <summary>
      /// base de dados do projeto, injetada via construtor
      /// </summary>
      private readonly ApplicationDbContext _context;

      public CreateModel(ApplicationDbContext context) {
         _context = context;
      }

      /// <summary>
      /// mostra a página do Create, quando o pedido é feito em HTTP GET
      /// </summary>
      /// <returns>a página do Create</returns>
      public IActionResult OnGet() {
         return Page();
      }


      /// <summary>
      /// atributo que define o objeto a ser processado na vista (page)
      /// </summary>
      [BindProperty]
      public Degree Degree { get; set; } = default!;

      /// <summary>
      /// atributo que define o ficheiro de imagem 
      /// a ser processado na vista (page)
      /// </summary>
      [BindProperty]
      public IFormFile? ImagemLogo { get; set; }




      // For more information, see https://aka.ms/RazorPagesCRUD .
      /// <summary>
      /// Processa o pedido HTTP POST, 
      /// quando o formulário do Create é submetido. 
      /// Se os dados forem válidos, o novo Curso é adicionado à base de dados 
      /// e o utilizador é redirecionado para a página de índice dos Cursos.
      /// </summary>
      /// <returns>página de listagem de todos os cursos</returns>
      public async Task<IActionResult> OnPostAsync() {

         if(!ModelState.IsValid) {
            return Page();
         }

         _context.Degrees.Add(Degree);
         await _context.SaveChangesAsync();

         return RedirectToPage("./Index");
      }
   }
}
