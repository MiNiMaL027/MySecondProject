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
            CreateMap<CreateCustomList, CustomList>().ReverseMap(); // реверсом ніколи не скористаєшся 

            CreateMap<CustomList, CustomListView>().ReverseMap(); // реверсом ніколи не скористаєшся 

            CreateMap<ToDoTaskView, ToDoTask>().ReverseMap(); // реверсом ніколи не скористаєшся 

            CreateMap<CreateToDoTask, ToDoTask>().ReverseMap(); // реверсом ніколи не скористаєшся, тому він не потрібен

            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}