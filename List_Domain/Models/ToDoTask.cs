using List_Domain.CreateModel;
using List_Domain.ModelDTO;

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

        public ToDoTask()
        {

        }

        public ToDoTask(ToDoTaskDTO t)
        {
            Id = t.Id;
            Importance = t.Importance;
            Description = t.Description;
            Title = t.Title;
            DueToDate = t.DueToDate;
            CreationDate = t.CreationDate;
            IsCompleted = t.IsCompleted;
            IsFavorite = t.IsFavorite;
            IsDeleted = t.IsDeleted;
            CustomListId = t.CustomListId;
            UserId = t.UserId;
        }

        public ToDoTask(CreateToDoTask task, int uresId)
        {
            Title = task.Title;
            Description = task.Description;
            DueToDate = task.DueToDate;
            Importance = task.Importance;
            IsFavorite = task.IsFavorite;
            IsCompleted = false;
            IsDeleted = false;
            CreationDate = DateTime.Now;
            UserId = uresId;
            CustomListId = task.ListId;
        }

        public ToDoTask(CreateToDoTask task, int uresId, int taskId)
        {
            Id = taskId;
            Title = task.Title;
            Description = task.Description;
            DueToDate = task.DueToDate;
            Importance = task.Importance;
            IsFavorite = task.IsFavorite;
            IsCompleted = false;
            IsDeleted = false;
            CreationDate = DateTime.Now;
            UserId = uresId;
            CustomListId = task.ListId;
        }
    }
}

public enum Importance
{
    Low,
    Normal,
    High
}
