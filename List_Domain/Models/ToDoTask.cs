using List_Domain.Enums;
using List_Domain.Models.NotDbEntity;

namespace List_Domain.Models
{
    public class ToDoTask : UserEntity
    {
        public Importance? Importance { get; set; }

        public string? Description { get; set; }

        public string Title { get; set; }

        public DateTime? DueToDate { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsDeleted { get; set; }

        public int? CustomListId { get; set; }

        public CustomList? CustomList { get; set; }
    }
}
