using AutoMapper;
using List_Domain.CreateModel;
using List_Domain.ModelDTO;
using List_Domain.Models;
using List_Domain.ViewModel;
using Microsoft.AspNetCore.Http;

namespace List_Service.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CreateCustomList, CustomList>().ReverseMap();

            CreateMap<CustomList, CustomListView>().ReverseMap();

            CreateMap<ToDoTaskView, ToDoTask>().ReverseMap();

            CreateMap<CreateToDoTask, ToDoTask>().ReverseMap();

            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}