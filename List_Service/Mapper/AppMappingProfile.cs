using AutoMapper;
using List_Domain.CreateModel;
using List_Domain.ModelDTO;
using List_Domain.Models;
using List_Domain.ViewModel;

namespace List_Service.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CreateCustomList, CustomList>();

            CreateMap<CustomList, ViewCustomList>().ReverseMap();

            CreateMap<ViewToDoTask, ToDoTask>().ReverseMap();

            CreateMap<CreateToDoTask, ToDoTask>();

            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}