using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Entities;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LibraryAPI
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"app.Settings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Startup.Configuration["connectionStrings:LibraryDBConnectionString"];

            services.AddDbContext<LibraryContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<ILibraryRepository, LibraryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LibraryContext libraryContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            libraryContext.EnsureSeedDataForContext();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Author, Models.AuthorWithoutBooksDto>();
                cfg.CreateMap<Entities.Author, Models.AuthorDto>();
                cfg.CreateMap<Models.AuthorForCreationDto, Entities.Author>();
                cfg.CreateMap<Entities.LibraryContext, Models.BookDto>();
                cfg.CreateMap<Models.BookForCreationDto, Entities.Book>();
                cfg.CreateMap<Models.BookForUpdateDto, Entities.Book>();
                cfg.CreateMap<Entities.Book, Models.BookForUpdateDto>();
                cfg.CreateMap<Entities.Publisher, Models.PublisherDto>();
                cfg.CreateMap<Models.PublisherDto, Entities.Publisher>();
                cfg.CreateMap<Models.PublisherForCreationDto, Entities.Publisher>();
            });

            app.UseMvc();
        }
    }
}
