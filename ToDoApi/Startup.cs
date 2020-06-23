using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;
using ToDoApi.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;

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
      services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );

      services.AddSwaggerGen( x => {
        x.SwaggerDoc( "v1", new OpenApiInfo { Title = "Application name", Version = "v1" } );

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine( AppContext.BaseDirectory, xmlFile );

        x.IncludeXmlComments( xmlPath, includeControllerXmlComments: true );

      } );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env )
    {
      if( env.IsDevelopment() )
      {
        app.UseDeveloperExceptionPage();
      }

      var swaggerOptions = new SwaggerOptions();
      Configuration.GetSection( nameof( SwaggerOptions )).Bind( swaggerOptions );

      app.UseSwagger();
      /*
      app.UseSwagger( option =>
      {
        option.RouteTemplate = swaggerOptions.JsonRoute;
      });
      */

      app.UseSwaggerUI( option => {
        option.SwaggerEndpoint( swaggerOptions.UiEndpoint, swaggerOptions.Description );
      });

      app.UseMvc();
      DbPrepare.PrepPopulation( app );
    }
  }
}
