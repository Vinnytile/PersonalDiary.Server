using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedData.Models;
using SharedData.Models.User;

namespace PersonalDiary.Server.ServiceExtensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(configurationExpression =>
            {
                configurationExpression.CreateMap<UserIdentityRegisterDTO, UserIdentity>();
                configurationExpression.CreateMap<NoteDTO, Note>();
                configurationExpression.CreateMap<UserProfileDTO, UserProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
