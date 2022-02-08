using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoChat.Services;
using VideoChat.Settings;

namespace ProMobile.VideoAPI
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
            services.Configure<TwilioSettings>(settings =>
            {
                settings.AccountSid = "AC672c7b5023275442342d357cda7a0291";
                settings.ApiSecret = "FWvzJ8WEXY8eunvLMSPwdYRnO8KJ3Bwi";
                settings.ApiKey = "SK42f2c511c1de57927bb4a5df07ad27f8";
            })
                   .AddTransient<IVideoService, VideoService>();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy(name: Constants.MyAllowSpecificOrigins,
                                             builder =>
                                             {
                                                 builder.WithOrigins(Configuration["OriginsAllowed"].Split("||"))
                                                 .AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                             });
            });
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
#if DEBUG
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
           
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VideoAppAPI");
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
#endif
        }
    }
}
