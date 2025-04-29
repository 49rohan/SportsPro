using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.Models.Data;

using SportsPro.Services;

using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace SportsPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddDbContext<SportsProContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SportsPro")));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });


            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITechnicianService, TechnicianService>();
            services.AddScoped<ICountryService, CountryService>();

            services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<SportsProContext>()
              .AddDefaultTokenProviders();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            EnsureUserColumnsExist(app.ApplicationServices).Wait();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            // Create admin user and role
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                CreateAdminUser(services).Wait();
            }
        }

        private async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                // Create Admin role if it doesn't exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create Technician role if it doesn't exist
                if (!await roleManager.RoleExistsAsync("Technician"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Technician"));
                }

                // Create admin user if it doesn't exist
                var adminUser = await userManager.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@sportspro.com",
                        EmailConfirmed = true
                        // Temporarily remove Firstname and Lastname
                    };

                    var result = await userManager.CreateAsync(adminUser, "P@ssw0rd");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"Failed to create admin user: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAdminUser: {ex.Message}");
                // Don't throw the exception to prevent app startup failure
            }
        }



        private async Task EnsureUserColumnsExist(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SportsProContext>();
                try
                {
                    // Try to add the columns using raw SQL
                    await context.Database.ExecuteSqlRawAsync(@"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Firstname')
                BEGIN
                    ALTER TABLE AspNetUsers ADD Firstname nvarchar(max) NULL;
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Lastname')
                BEGIN
                    ALTER TABLE AspNetUsers ADD Lastname nvarchar(max) NULL;
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'RoleNames')
                BEGIN
                    ALTER TABLE AspNetUsers ADD RoleNames nvarchar(max) NULL;
                END
            ");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error ensuring user columns exist: {ex.Message}");
                }
            }
        }

    }
}