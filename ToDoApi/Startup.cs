using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;


namespace ToDoApi
{
  public class Startup
  {
    //please comment this later
    public IConfiguration Configuration { get; set; }

    //please comment this later
    public Startup( IConfiguration configuration )
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices( IServiceCollection services )
    {
      var connectionString = $"Data Source={Configuration["Database:Docker:Server"]},{Configuration["Database:Docker:Port"]};Initial Catalog={Configuration["Database:Docker:DBName"]};User ID={Configuration["Database:Docker:User"]};Password={Configuration["Database:Docker:Password"]};";

      services.AddDbContext<ToDoDbContext>
        ( opt => opt.UseSqlServer( connectionString ) );
      services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env )
    {
      if( env.IsDevelopment() )
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
      DbPrepare.PrepPopulation( app );
    }
  }
}
