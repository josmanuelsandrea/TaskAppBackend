using AutoMapper;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Models.DTOS.Request;
using TaskApp.Models.DTOS.Response;

namespace TaskApp.Infrastructure.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Definir el mapeo entre objetos
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<UserRegisterDTO, User>();

            CreateMap<TaskM, TaskDTO>();
            CreateMap<TaskDTO, TaskM>();
            CreateMap<TaskRequest, TaskM>();
            CreateMap<TaskUpdate, TaskM>();
        }
    }
}
