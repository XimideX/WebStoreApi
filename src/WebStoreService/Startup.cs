using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            
            //Telss our database what are our enitties and what dat should they have
            services.AddDbContext<WebStoreContext>(opt =>
               opt.UseMySQL("server=localhost;database=webstore;user=root;password=Vamosleia#-3c",
                    builder => 
                    {
                        builder.MigrationsAssembly("WebStoreService");
                    }));

            // Add identity registers into database
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Ximid.Cookie";
                config.Cookie.HttpOnly = false;
                config.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
                config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = CookieSecurePolicy.None;
            });

            services.Configure<CookieOptions>(options =>
            {
                options.SameSite = SameSiteMode.None;
                options.Secure = true;
            });

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

            app.UseCookiePolicy();
            
            app.UseAuthentication();

            app.UseAuthorization();

            // app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
