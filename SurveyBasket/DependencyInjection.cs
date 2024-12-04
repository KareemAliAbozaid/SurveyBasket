using MapsterMapper;
using SurveyBasket.Persistence;
using System.Reflection;

namespace SurveyBasket
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependecies(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllers();

            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
           services.AddDbContext<ApplicationDbcontext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IPollServices, PollService>();
            services
                .AddSwaggerConfig()
                .AddMapstarConfig()
                .AddFlountValidationConfig();

            return services;
        }
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        public static IServiceCollection AddMapstarConfig(this IServiceCollection services)
        {
            //Add mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }
        public static IServiceCollection AddFlountValidationConfig(this IServiceCollection services)
        {
            //FluentValidation
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
