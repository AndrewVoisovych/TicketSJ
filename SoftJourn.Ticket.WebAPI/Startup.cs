using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// 
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Open html instead of api
            DefaultFilesOptions DefaultFile = new DefaultFilesOptions();
            DefaultFile.DefaultFileNames.Clear();
            DefaultFile.DefaultFileNames.Add("Index.html");
            app.UseDefaultFiles(DefaultFile);
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
