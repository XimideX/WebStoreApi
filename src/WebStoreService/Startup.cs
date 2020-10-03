using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStoreDAO.CoreDAO;

namespace WebStoreService
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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
                options.AddPolicy(name: MyAllowSpecificOrigins, builder => 
                {
                    builder.AllowAnyMethod()
                        .WithOrigins("*")
                        .AllowAnyHeader();
                        // .AllowCredentials();
                });
            });
            services.AddDbContext<WebStoreContext>(opt =>
               opt.UseMySQL("server=localhost;database=webstore;user=root;password=Vamosleia#-3c",
                    builder => 
                    {
                        builder.MigrationsAssembly("WebStoreService");
                    }));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>();

            services.AddControllers();
            services.AddTransient<UnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            
            // app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
