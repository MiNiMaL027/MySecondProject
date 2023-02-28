using List_Domain.Enums;

namespace List_Domain.CreateModel
{
    public class CreateToDoTask
    {
        public string Title { get; set; }

        public DateTime? DueToDate { get; set; }

        public string Description { get; set; }

        public bool IsFavorite { get; set; }

        public Importance? Importance { get; set; }

        public int CustomListId { get; set; }
    }
}

