using List_Domain.Models;

namespace List_Domain.ModelDTO
{
    public class CustomListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }

        public CustomListDTO() { }

        public CustomListDTO(CustomList list)
        {
            Id = list.Id;
            Name = list.Name;
            IsDeleted = list.IsDeleted;
            UserId = list.UserId;
        }

    }
}
