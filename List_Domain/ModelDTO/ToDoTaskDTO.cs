﻿using List_Domain.Models;

namespace List_Domain.ModelDTO
{
    public class ToDoTaskDTO
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

        public ToDoTaskDTO()
        {

        }

        public ToDoTaskDTO(ToDoTask t)
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
    }
}
