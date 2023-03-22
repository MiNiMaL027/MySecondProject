using AutoMapper;
using List_Domain.CreateModel;
using List_Domain.Models;
using List_Domain.ViewModel;

namespace List_Service.Mapper
{
    public class AppMappingProfile : Profile
    {   
        public AppMappingProfile()
        {
            CreateProjection<CustomList, ViewCustomList>();

            CreateProjection<ToDoTask, ViewToDoTask>();

            CreateMap<CreateCustomList, CustomList>().ReverseMap();

            CreateMap<CustomList, ViewCustomList>().ReverseMap();

            CreateMap<ViewToDoTask, ToDoTask>().ReverseMap();

            CreateMap<CreateToDoTask, ToDoTask>().ReverseMap();

            CreateMap<ViewSettings, Settings>().ReverseMap();
        }
    }
}