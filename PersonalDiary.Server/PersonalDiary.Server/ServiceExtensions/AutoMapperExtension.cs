using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedData.Models;

namespace PersonalDiary.Server.ServiceExtensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(configurationExpression =>
            {
                configurationExpression.CreateMap<UserRegisterDTO, User>();
                configurationExpression.CreateMap<NoteDTO, Note>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
