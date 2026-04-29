using System.ComponentModel.DataAnnotations;

namespace Aulas.Data.Model {

   /// <summary>
   /// Classe para representar os utilizadores da aplicação,
   /// ou seja, os dados que identificam cada utilizador.
   /// </summary>
   public class MyUser {

      [Key] // PK
      public int Id { get; set; }

      /// <summary>
      /// Nome do utilizador
      /// </summary>
      [StringLength(50)]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [Display(Name = "Nome Completo")]
      public string Name { get; set; } = "";

      /// <summary>
      /// Data de nascimento
      /// </summary>
      [Display(Name = "Data Nascimento")]
      [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
      [DataType(DataType.Date)]
      [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
      public DateOnly BirthDate { get; set; }

      /// <summary>
      /// Telemóvel do utilizador, 
      /// </summary>
      [Display(Name = "Telemóvel")]
      [StringLength(19)]
      [RegularExpression(@"\+?[0-9]{9,18}", 
         ErrorMessage = "O número de telemóvel deve conter apenas dígitos (entre 9 e 18) e pode começar com um sinal de mais.")]
      public string? CellPhone { get; set; }

      /// <summary>
      /// atributo para funcionar como FK entre a tabela dos MyUser
      /// e a tabela da Autenticação
      /// </summary>
      //        public string UserID { get; set; } = null!;

   }
}
