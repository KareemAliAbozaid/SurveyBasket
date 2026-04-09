using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SurveyBasket.Authentication;
using SurveyBasket.Persistence;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SurveyBasket
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbcontext>(options =>
                 options.UseSqlServer(connectionString));

            services.AddScoped<IPollServices, PollService>();
            services.AddScoped<IAuthServices, AuthServices>();

            services
                .AddSwaggerConfig()
                .AddMapstarConfig()
                .AddFlountValidationConfig()
                .AddAuthConfig(configuration);

            return services;
        }
        private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddMapstarConfig(this IServiceCollection services)
        {
            //Add mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }
        private static IServiceCollection AddFlountValidationConfig(this IServiceCollection services)
        {
            //FluentValidation
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
        //Authentication
        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbcontext>();

            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.Configure<JwtOptions>(configuration.GetSection("jwt"));

            var jwtSettings = configuration.GetSection("jwt").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                };
            });

            var test = new
            {
                IssuerSigningKey = configuration["Jwt:Key"]!,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
            };

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                //options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

    }
}
