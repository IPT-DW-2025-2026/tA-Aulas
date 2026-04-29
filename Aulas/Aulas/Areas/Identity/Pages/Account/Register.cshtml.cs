// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Aulas.Data;
using Aulas.Data.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Aulas.Areas.Identity.Pages.Account {
   public class RegisterModel:PageModel {

      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly IUserStore<IdentityUser> _userStore;
      private readonly IUserEmailStore<IdentityUser> _emailStore;
      private readonly ILogger<RegisterModel> _logger;
      private readonly IEmailSender _emailSender;

      private readonly ApplicationDbContext _context;

      public RegisterModel(
          UserManager<IdentityUser> userManager,
          IUserStore<IdentityUser> userStore,
          SignInManager<IdentityUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender,
          ApplicationDbContext context) {
         _userManager = userManager;
         _userStore = userStore;
         _emailStore = GetEmailStore();
         _signInManager = signInManager;
         _logger = logger;
         _context = context;
         _emailSender = emailSender;
      }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      [BindProperty]
      public InputModel Input { get; set; }

      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      public string ReturnUrl { get; set; }




      /// <summary>
      ///     Classe utilizada para recolher os dados de um novo utilizador
      /// </summary>
      public class InputModel {
         /// <summary>
         ///     Email do novo utilizador 
         /// </summary>
         [Required(ErrorMessage = "o {0} é de preenchimento obrigatório")]
         [Display(Name = "Email")]
         public string Email { get; set; }

         /// <summary>
         ///     Password do novo utilizador
         /// </summary>
         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "Password")]
         public string Password { get; set; }

         /// <summary>
         ///  Confirmar a password
         /// </summary>
         [DataType(DataType.Password)]
         [Display(Name = "Confirmar password")]
         [Compare("Password", ErrorMessage = "A password e a sua confirmação não são iguais.")]
         public string ConfirmPassword { get; set; }

         // +++++++++++++++++++++++++++++++++++++++++++++
         // Objeto para recolher os dados pessoais do novo utilizador
         // neste caso, de um Aluno (Student)
         // +++++++++++++++++++++++++++++++++++++++++++++

         public Student Student { get; set; }

         // +++++++++++++++++++++++++++++++++++++++++++++

      } // Fim da classe InputModel


      public async Task OnGetAsync(string returnUrl = null) {
         ReturnUrl = returnUrl;

         // procurar os dados do Cursos
         ViewData["DegreeFK"] = new SelectList(_context.Degrees.OrderBy(d => d.Name), "Id", "Name");


      }





      public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
         returnUrl ??= Url.Content("~/");
         if(ModelState.IsValid) {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

            if(result.Succeeded) {
               _logger.LogInformation("User created a new account with password.");

               var userId = await _userManager.GetUserIdAsync(user);
               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
               var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

               await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

               if(_userManager.Options.SignIn.RequireConfirmedAccount) {
                  return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
               }
               else {
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return LocalRedirect(returnUrl);
               }
            }
            foreach(var error in result.Errors) {
               ModelState.AddModelError(string.Empty, error.Description);
            }
         }

         // If we got this far, something failed, redisplay form
         return Page();
      }

      private IdentityUser CreateUser() {
         try {
            return Activator.CreateInstance<IdentityUser>();
         }
         catch {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
         }
      }

      private IUserEmailStore<IdentityUser> GetEmailStore() {
         if(!_userManager.SupportsUserEmail) {
            throw new NotSupportedException("The default UI requires a user store with email support.");
         }
         return (IUserEmailStore<IdentityUser>)_userStore;
      }
   }
}
