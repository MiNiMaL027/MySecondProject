using List_Domain.CreateModel;
using List_Domain.ModelDTO;

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

    }
}
