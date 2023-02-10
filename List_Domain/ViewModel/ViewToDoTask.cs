using List_Domain.Enums;

namespace List_Domain.ViewModel
{
    public class ViewToDoTask 
    {
        public int Id { get; set; }

        public Importance? Importance { get; set; }

        public string? Description { get; set; }

        public string Title { get; set; }

        public DateTime? DueToDate { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsDeleted { get; set; }

        public int? CustomListId { get; set; }

        public int UserId { get; set; }
    }
}
