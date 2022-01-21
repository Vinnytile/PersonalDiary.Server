using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedData.Models;

namespace PersonalDiary.Server.Extensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(configurationExpression =>
            {
                configurationExpression.CreateMap<UserRegisterDTO, User>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
