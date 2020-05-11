using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ToDoApi.Models
{
  public static class DbPrepare
  {
    public static void PrepPopulation( IApplicationBuilder app )
    {
      using (var serviceScope =  app.ApplicationServices.CreateScope() )
      {
        SeedData( serviceScope.ServiceProvider.GetService<ToDoDbContext>() );
      }
    }

    public static void SeedData( ToDoDbContext context )
    {
      System.Console.WriteLine("Migrating database");
      context.Database.Migrate();

      if(!context.ToDoItems.Any())
        System.Console.WriteLine( "No entry" );
    }
  }
}
