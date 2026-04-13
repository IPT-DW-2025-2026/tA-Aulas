using Aulas.Data.Model;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aulas.Data {
   public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):IdentityDbContext(options) {


      /* ********************************************
       * definir as 'tabelas' da base de dados
       * ******************************************** */
      public DbSet<MyUser> AppUsers { get; set; }
      public DbSet<Student> Students { get; set; }
      public DbSet<Professor> Professors { get; set; }
      public DbSet<Course> Courses { get; set; }
      public DbSet<Degree> Degrees { get; set; }
      public DbSet<Registration> Registrations { get; set; }
   }
}
