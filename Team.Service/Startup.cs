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
                //API��n hangi client�tan istek al�p almayaca��n� belirlemesi CORS politikalar� ayarlama diye nitelendirilebilir. G�vrnlik ama�l� kullan�l�r.
                //add policy = politika ekle
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            #region DependencyInjections
            /*
             Transient (AddTransient)
             Uygulama i�erisinde ba��ml�l�k olarak olu�turdu�umuz ve kulland���m�z
             nesnenin her kullan�m ve �a�r�da tekrardan olu�turulmas�n� sa�lar.

             Singleton (AddSingleton)
             Uygulama i�erisinde ba��ml�l�k olu�turdu�umuz ve kulland���m�z nesnenin 
             tek bir sefer olu�turulmas�n� ve ayn� nesnenin uygulama i�inde kullan�lmas�n� sa�lar.

             Scoped (AddScoped)
             Uygulama i�erisindeki ba��ml�l�k olu�turdu�umu nesnenin request sonlanana kadar ayn� 
             nesneyi kullanmas�n� farkl� bir �a�r� i�in gelindi�inde yeni bir nesne yarat�lmas�n� sa�lar.
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
