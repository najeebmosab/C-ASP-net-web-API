using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Serialization;
using FinalProject.Datas;
using FinalProject.Controllers;
using FinalProject.Services;
using FinalProject.Mapping;

namespace FinalProject
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
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddCors();
            //services.AddControllers().AddNewtonsoftJson(options =>
            //      {
            //          options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //      });
            string connectionString = Configuration.GetConnectionString("MyTodolistDB");
            services.AddDbContext<ItemsContextForAllTables>(options =>options.UseSqlServer(connectionString));
            //options.UseLazyLoadingProxies().
            services.AddAutoMapper(typeof(FromProductsToDTO));
            services.AddAutoMapper(typeof(FromCategoryToDTO));
            services.AddAutoMapper(typeof(FromOrderDitelsToDTO));
            services.AddScoped<CategoryServices>();
            services.AddScoped<ImagesServices>();
            services.AddScoped<OrderDetailsServices>();
            services.AddScoped<OrderServices>();
            services.AddScoped<ProductsServices>();
            services.AddScoped<UserServices>();
            services.AddScoped<CartServices>();
            services.AddScoped<Jwt>();
            services.AddScoped<BrandServices>();
            services.AddScoped<SlidersServices>();
            services.AddScoped<CartProductService>();

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
             .AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = Jwt.TokenValidationParameters;
             });
           
            services.AddCors();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
          options.WithOrigins()
          .AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(orgen => true)
          );
            //app.UseMiddleware<AllowedCorsMiddlewares>();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
