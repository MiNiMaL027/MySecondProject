namespace List_Domain.Models
{
    public class ToDoTask
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
        public CustomList? CustomList { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

public enum Importance // енамок це теж стосується, окремий файл, ЗАВЖДИ І ВСЮДИ, і я б назвав це Пріоріті а не Імпортансе 
{
    Low,
    Normal,
    High
}
