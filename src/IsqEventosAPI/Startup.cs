
using AutoMapper;
using System;
using IsqEventos.Application;
using IsqEventos.Application.Contratos;
using IsqEventos.Persistencia;
using IsqEventos.Persistencia.Contatos;
using IsqEventos.Persistencia.contextos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace IsqEventosAPI

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
            services.AddDbContext<IsqEventosContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5, // N�mero m�ximo de tentativas
                            maxRetryDelay: TimeSpan.FromSeconds(30), // Atraso m�ximo entre tentativas
                            errorNumbersToAdd: null // Adicionar n�meros de erro personalizados, se necess�rio
                        );
                    });
            });

            services.AddControllers()
             .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEventosService, EventoService>();
            services.AddScoped<ILotesService, LoteService>();

            services.AddScoped<IGeralPersistencia, GeralPersiste>();
            services.AddScoped<IEventosPersistencia, EventoPersiste>();
            services.AddScoped<ILotePersistencia, LotePersiste>();
            services.AddScoped<IPalestrantesPersistencia, PalestrantePersiste>();

            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IsqEventos.API", Version = "v1" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IsqEventos.API v1"));


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowAnyOrigin());

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}