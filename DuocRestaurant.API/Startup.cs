using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Http.Features;
using DuocRestaurant.API.Hubs;

namespace DuocRestaurant.API
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
            services.AddMvcCore().AddNewtonsoftJson();

            services.Configure<RestaurantDatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<FlowSettings>(Configuration.GetSection("FlowSettings"));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IFlowService, FlowService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IMeasurementUnitService, MeasurementUnitService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISupplyRequestService, SupplyRequestService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IUserService, UserService>();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddSignalR();
            services.AddControllers();
            services.Configure<FormOptions>(option =>
            {
                option.ValueLengthLimit = int.MaxValue;
                option.MultipartBodyLengthLimit = int.MaxValue;
                option.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                            builder.WithOrigins("http://localhost:4200", "http://localhost:8100")
                                   .AllowAnyMethod()
                                   .AllowAnyHeader()
                                   .AllowCredentials());
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(name: "controllerMethod",
                   pattern: "api/{controller}/{action}/{id?}");
                endpoints.MapHub<OrdersHub>("/hubs/orders");
                endpoints.MapHub<NotificationsHub>("/hubs/notifications");
            });
        }
    }
}
