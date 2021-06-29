using System.Text;
using F1Manager.Admin.Configuration;
using F1Manager.Api.Filters;
using F1Manager.SqlData;
using F1Manager.Teams.Configuration;
using F1Manager.Teams.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using F1Manager.Users.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace F1Manager.Api
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

            var issuer = Configuration["Users:Issuer"];
            var audience = Configuration["Users:Audience"];
            var secret = Configuration["Users:Secret"];

            var connectionString = Configuration["Teams:SqlConnectionString"];

            services.AddDbContext<F1ManagerDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = issuer,
                            ValidAudience = audience,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
                        };
                    });

            services.AddHealthChecks()
                .AddCheck<RedisCacheHealthCheck>("RedisCache");

            services.AddControllers(options =>
                options.Filters.Add(new F1ManagerExceptionFilter()));
            services.AddHttpContextAccessor();

            services.ConfigureAdministration();
            services.ConfigureUsers();
            services.ConfigureTeams();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "F1Manager.Api", Version = "v1"});
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "F1Manager.Api v1"));
            }
            app.UseCors(builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200",
                        "http://localhost",
                        "https://app.f1mgr.com",
                        "https://app-dev.f1mgr.com",
                        "https://app-tst.f1mgr.com",
                        "https://app-test.f1mgr.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
