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
using RabbitMQ.Client.Core.DependencyInjection;
using Generator.Elements;
using Generator.Properties;
using MediatR;
using System.Reflection;
using Generator.Functions.Query;
using Generator.Hubs;

namespace Generator
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
            //CORS
            services.AddCors(o => o.AddPolicy("Access-Control-Allow-Origin", builder =>
            {
                builder.WithOrigins("https://localhost:5001")
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Generator", Version = "v1" });
            });

            //MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());


            //
            services.AddSingleton<IRepositorySensor, RepostiorySensor>();
            services.AddSingleton<IMongoSensorDBContext, MongoSensorDBContext>();

            //
            services.AddSignalR();

            //Rabbit config
            //services.AddRabbitMqClient();
            //var section = Configuration.GetSection("RabbitMq");
            //services.AddRabbitMqProducer(section);

            //Service send
            //services.AddTransient<IHandler<SendMessage>, SendMessageHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //CORS
            app.UseCors("Access-Control-Allow-Origin");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Generator v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<StockDataHub>("/stream");
            });
            
            
        }
    }
}
