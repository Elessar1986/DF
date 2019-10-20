using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using DietFood.Helpers.Abstract;
using DietFood.Helpers;
using DietFood.Models;

namespace DietFood
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            var options = optionsBuilder
                    //.UseMySQL("server=localhost;UserId=root;Password=;database=diet_food;")
                    .UseSqlServer(@"workstation id=dietfood.mssql.somee.com;packet size=4096;user id=qweasd123123_SQLLogin_1;pwd=uf153ljphn;data source=dietfood.mssql.somee.com;persist security info=False;initial catalog=dietfood")
                                      .Options;
            services.AddSingleton<IRepositoryHelper>(new RepositoryHelper(new MyContext(options)));
            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Main}/{action=Index}/{id?}");
            });
        }
    }
}
