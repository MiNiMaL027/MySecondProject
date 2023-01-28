namespace List_Domain.Models
{
    public class CustomList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual List<ToDoTask>? Tasks { get; set; } = new();

        public CustomList() { }

        public CustomList(CustomListDTO list)
        {
            Id = list.Id;
            Name = list.Name;
            IsDeleted = list.IsDeleted;
            UserId= list.UserId;
        }

        public CustomList(CreateCustomList list,int userId)
        {
            Name= list.Name;
            IsDeleted = false;
            UserId= userId;
        }

        public CustomList(CreateCustomList list, int userId,int listId)
        {
            Id = listId;
            Name = list.Name;
            IsDeleted = false;
            UserId = userId;
        }
    }
    public class CustomListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }

        public CustomListDTO() { }

        public CustomListDTO(CustomList list)
        {
            Id= list.Id;
            Name = list.Name;
            IsDeleted = list.IsDeleted;
            UserId= list.UserId;
        }

    }

    public class CreateCustomList
    {
        public string Name { get; set; }
    }
}
