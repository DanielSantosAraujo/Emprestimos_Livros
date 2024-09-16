using Microsoft.EntityFrameworkCore;
using EmprestimosLivros.Models;
namespace EmprestimosLivros.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {  
        }

        public DbSet<EmprestimosModel> Emprestimos { get; set; }
    }
}
