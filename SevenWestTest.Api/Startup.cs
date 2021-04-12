using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SevenWestTest.Api.Infrastructure.Formatters;
using SevenWestTest.Api.Infrastructure.Options;
using SevenWestTest.Api.Providers;
using SevenWestTest.Api.Services;

namespace SevenWestTest.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOptions();
            services.AddMemoryCache();
            services.AddHttpClient();
            services.Configure<UserApiConfig>(Configuration.GetSection("UserApiConfig"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<UserApiConfig>>().Value);
            services.AddSingleton<IDataProvider,HttpDataProvider>();
            services.AddSingleton<IDataFormatter, JsonFormatter>();
            services.AddSingleton<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
