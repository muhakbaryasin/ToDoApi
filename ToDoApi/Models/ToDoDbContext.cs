using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Models
{
  public class ToDoDbContext : DbContext
  {
    public ToDoDbContext( DbContextOptions<ToDoDbContext> options ) : base( options )
    {
    }

    public DbSet<ToDo> ToDoItems { get; set; }
  }
}
