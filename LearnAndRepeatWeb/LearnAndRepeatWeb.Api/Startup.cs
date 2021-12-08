using FluentValidation.AspNetCore;
using LearnAndRepeatWeb.Business.ConfigModels;
using LearnAndRepeatWeb.Business.Constants;
using LearnAndRepeatWeb.Business.Mappers.User;
using LearnAndRepeatWeb.Business.Services.Implementations;
using LearnAndRepeatWeb.Business.Services.Interfaces;
using LearnAndRepeatWeb.Business.Validators.User;
using LearnAndRepeatWeb.Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LearnAndRepeatWeb.Api
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

            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<PostUserRequestValidator>();
                    s.DisableDataAnnotationsValidation = true;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnAndRepeatWeb.Api", Version = "v1" });
            });

            services.AddAutoMapper(typeof(UserMappingProfile));
            services.ConfigureConfigSectionModels(Configuration);
            services.ConfigureJwtAuthentication(Configuration);

            services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("LearnAndRepeatWebSqlServerConnectionString")));

            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnAndRepeatWeb.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

    public static class ConfigurationExtensionMethods
    {
        public static void ConfigureConfigSectionModels(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserConfigSectionModel>(options => configuration.GetSection(ConfigSectionNames.UserConfigSectionName).Bind(options));
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var userConfigSection = configuration.GetSection(ConfigSectionNames.UserConfigSectionName);
            UserConfigSectionModel userConfigSectionModel = userConfigSection.Get<UserConfigSectionModel>();
            var key = Encoding.ASCII.GetBytes(userConfigSectionModel.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }

}
