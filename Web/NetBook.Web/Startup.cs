namespace NetBook.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using NetBook.Data;
    using NetBook.Data.Common;
    using NetBook.Data.Common.Repositories;
    using NetBook.Data.Models;
    using NetBook.Data.Repositories;
    using NetBook.Data.Seeding;
    using NetBook.Services.Data;
    using NetBook.Services.Data.Absence;
    using NetBook.Services.Data.Class;
    using NetBook.Services.Data.Grade;
    using NetBook.Services.Data.Remark;
    using NetBook.Services.Data.School;
    using NetBook.Services.Data.Student;
    using NetBook.Services.Data.Subject;
    using NetBook.Services.Data.User;
    using NetBook.Services.Mapping;
    using NetBook.Services.Messaging;
    using NetBook.Services.Models;
    using NetBook.Web.InputModels.Subject;
    using NetBook.Web.ViewModels.Subject;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Framework services
            // TODO: Add pooling when this bug is fixed: https://github.com/aspnet/EntityFrameworkCore/issues/9741
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<NetBookUser, NetBookRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<NetBookUser>, MyUserClaimsPrincipalFactory>();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = ".AspNetCore.ConsentCookie";
                });

            services.AddSingleton(this.configuration);

            // Identity stores
            services.AddTransient<IUserStore<NetBookUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<NetBookRole>, ApplicationRoleStore>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISmsSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISchoolService, SchoolService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISubjectService, SubjectService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IClassService, ClassService>();
            services.AddTransient<IAbsenceService, AbsenceService>();
            services.AddTransient<IRemarkService, RemarkService>();
            services.AddTransient<IGradeService, GradeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(SchoolServiceModel).GetTypeInfo().Assembly,
                typeof(SubjectAllViewModel).GetTypeInfo().Assembly,
                typeof(SubjectEditInputModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                //new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

                new DataSeeder(dbContext).SeedSubjects();
                new DataSeeder(dbContext).SeedSchool();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Identity/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
