using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Aulas.Data;
using Aulas.Data.Model;
using Microsoft.AspNetCore.Http.Timeouts;
using System.ComponentModel.DataAnnotations;

namespace Aulas.Pages.Degrees {
   public class CreateModel:PageModel {

      /// <summary>
      /// base de dados do projeto, injetada via construtor
      /// </summary>
      private readonly ApplicationDbContext _context;

      /// <summary>
      /// Objecto que contem todas as definições do Servidor Web,
      /// injetadas via construtor.
      /// </summary>
      private readonly IWebHostEnvironment _webHostEnvironment;

      public CreateModel(ApplicationDbContext context,
         IWebHostEnvironment webHostEnvironment) {
         _context = context;
         _webHostEnvironment = webHostEnvironment;
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
      [Required(ErrorMessage = "O ficheiro de imagem é obrigatório.")]
      public IFormFile ImagemLogo { get; set; }




      // For more information, see https://aka.ms/RazorPagesCRUD .
      /// <summary>
      /// Processa o pedido HTTP POST, 
      /// quando o formulário do Create é submetido. 
      /// Se os dados forem válidos, o novo Curso é adicionado à base de dados 
      /// e o utilizador é redirecionado para a página de índice dos Cursos.
      /// </summary>
      /// <returns>página de listagem de todos os cursos</returns>
      public async Task<IActionResult> OnPostAsync() {
         /* A imagem como está a ser processada?
          * o ficheiro existe?
          *    se não, devolver controlo à página
          *    se sim, é uma imagem?
          *       se não, devolver controlo à página
          *       se sim, guardar a imagem num local do servidor 
          *               e guardar o caminho da imagem no objeto Curso
          * 
          */

         // se apesar da anotação quiser avaliar se a imagem existe ou não
         if(ImagemLogo == null) {
            ModelState.AddModelError("ImagemLogo", "O ficheiro de imagem é obrigatório.");
            return Page();
         }

         // há ficheiro, mas é uma imagem?
         if(ImagemLogo.ContentType != "image/jpeg"
            && ImagemLogo.ContentType != "image/png") {
            // Válido se PNP ou JPG
            // if (png==true || jpg==true) <=> if (png==false && jpg==false)
            ModelState.AddModelError("ImagemLogo", "O ficheiro de imagem deve ser JPEG ou PNG.");
            return Page();
         }

         // se chegamos aqui, o ficheiro existe e é uma imagem válida
         // o nome do ficheiro será um GUID + extensão do ficheiro original
         var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagemLogo.FileName).ToLowerInvariant();
         // atribuir o nome da imagem ao objeto Curso
         Degree.Logotype = fileName;
         // não vamos ainda guardar o ficheiro no disco rígido do servidor
         // só o faremos depois de guardar o objeto Curso na base de dados

         if(!ModelState.IsValid) {
            return Page();
         }

         try {
            _context.Degrees.Add(Degree);
            await _context.SaveChangesAsync();

            // guardar no disco rígido do servidor a imagem,
            // usando o nome do ficheiro que foi gerado
            string localizacaoImagem=_webHostEnvironment.WebRootPath;
            localizacaoImagem = Path.Combine(localizacaoImagem, "imagens");

            if(!Directory.Exists(localizacaoImagem)) {
               Directory.CreateDirectory(localizacaoImagem);
            }

            string caminhoCompleto=Path.Combine(localizacaoImagem, Degree.Logotype);
            using(var stream = new FileStream(caminhoCompleto, FileMode.Create)) {
               await ImagemLogo.CopyToAsync(stream);
            }

            return RedirectToPage("./Index");
         }
         catch(Exception) {
            // throw;
            /*
             * se ocorrer um erro ao guardar o Curso na base de dados,
             * ou na operação de guardar a imagem no disco rígido do servidor,
             * devemos tratar o problema
             *   - registar o erro num ficheiro de log, para que os programadores possam analisar o problema
             *   - o registo na BD e a imagem devem ser destruídas
             *   - mostrar uma mensagem de erro ao utilizador
             *   - devolver o controlo à página do Create, para que o utilizador possa tentar novamente
             */
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao guardar o curso. Tente novamente, por favor. Se persistir, contacte o Administrador do sistema.");
            return Page();
         }

      }
   }
}
