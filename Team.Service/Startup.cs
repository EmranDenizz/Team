using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team.Business.DependencyInjections;
using Team.Business.Repository;
using Team.Business.Response;

namespace Team.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                //API’ýn hangi client’tan istek alýp almayacaðýný belirlemesi CORS politikalarý ayarlama diye nitelendirilebilir. Güvrnlik amaçlý kullanýlýr.
                //add policy = politika ekle
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            #region DependencyInjections
            /*
             Transient (AddTransient)
             Uygulama içerisinde baðýmlýlýk olarak oluþturduðumuz ve kullandýðýmýz
             nesnenin her kullaným ve çaðrýda tekrardan oluþturulmasýný saðlar.

             Singleton (AddSingleton)
             Uygulama içerisinde baðýmlýlýk oluþturduðumuz ve kullandýðýmýz nesnenin 
             tek bir sefer oluþturulmasýný ve ayný nesnenin uygulama içinde kullanýlmasýný saðlar.

             Scoped (AddScoped)
             Uygulama içerisindeki baðýmlýlýk oluþturduðumu nesnenin request sonlanana kadar ayný 
             nesneyi kullanmasýný farklý bir çaðrý için gelindiðinde yeni bir nesne yaratýlmasýný saðlar.
            */
            services.AddSingleton<IPlayerRepository, PlayerRepository>();

            #endregion

            services.AddControllers();

            //Scrutor paketini indirmeyi unutma.
            services.Scan(sc =>
                sc.FromCallingAssembly()

                    .FromAssemblies(
                        typeof(IScopedDependency).Assembly
                    )
                    .AddClasses()
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()

                    .FromAssemblies(
                        typeof(ISingletonDependency).Assembly
                    )
                    .AddClasses()
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()


                    .FromAssemblies(
                        typeof(ITransientDependency).Assembly
                    )
                    .AddClasses()
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Team API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Emran Deniz",
                        Email = string.Empty
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Team.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
