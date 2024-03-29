﻿using List_Domain.Models.NotDbEntity;

namespace List_Domain.Models
{
    public class CustomList : UserEntity
    {
        public string Name { get; set; }

        public DateTime? ArchivalDate { get; set; }

        public virtual List<ToDoTask>? Tasks { get; set; } = new List<ToDoTask>();
    }
}
