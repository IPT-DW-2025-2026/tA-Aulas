using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aulas.Data.Model {

   /// <summary>
   /// classe que herda todas as características do MyUser, 
   /// ou seja, tem Id, Nome, Email, etc. 
   /// E, pode ter outras características específicas de um estudante, 
   /// como por exemplo, a matrícula, o curso que está fazendo, etc.
   /// </summary>
   public class Student:MyUser {


      /// <summary>
      /// Número atribuído a cada estudante, para o identificar de forma única
      /// </summary>
      public int StudentNumber { get; set; }

      /// <summary>
      /// atributo auxiliar para a propina, para ser usado na View,
      /// para recolher a propina como string, e depois converter para decimal,
      /// para ser guardada na base de dados
      /// </summary>
      [NotMapped] // esta anotação informa a EF para não criar um atributo na base de dados para este atributo
      [Required(ErrorMessage = "A {0} é obrigatória")] // esta anotação informa a EF para validar este atributo como obrigatório
      [Display(Name = "Propina")] // esta anotação informa a EF para usar o nome "Propina" na View, em vez de "TuitionFeeAux"
      [StringLength(10)]
      [RegularExpression("[0-9]{1,7}([,.][0-9]{1,2})?", 
         ErrorMessage = "A {0} deve ser um número com até 2 casas decimais")] // esta anotação informa a EF para validar este atributo como um número com até 2 casas decimais
      public string TuitionFeeAux { get; set; } = "";


      /// <summary>
      /// Propina paga pelo Student aquando da matrícula no Degree
      /// </summary>
      [Precision(9, 2)] // informa a EF para criar o atributo com 9 dígitos e 2, como parte decimal
      public decimal TuitionFee { get; set; }

      /// <summary>
      /// Data de matrícula do aluno
      /// </summary>
      [Display(Name = "Data Matrícula")]
      [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
      [DataType(DataType.Date)]
      public DateTime RegistrationDate { get; set; } = DateTime.Now;


      /* ****************************************
      * Construção dos Relacionamentos
      * *************************************** */

      // relacionamento 1-N


      /// <summary>
      /// FK para o Degree
      /// </summary>
      [ForeignKey(nameof(Degree))] // esta anotação informa a EF que o atributo 'DegreeFK' é uma FK em conjunto com o atributo 'Degree'
      [Display(Name = "Curso")]
      public int DegreeFK { get; set; } // FK para o Degree
      [ValidateNever] // informa a EF para não validar este atributo
      public Degree Degree { get; set; } = null!; // FK para o Degree



      // relacionamento N-M, com atributos no relacionamento
      /// <summary>
      /// Lista de UCs em que o aluno está inscrito
      /// </summary>
      public ICollection<Registration> RegistrationsList { get; set; } = [];
   }
}
