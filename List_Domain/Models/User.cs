﻿namespace List_Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } 
        public string? Password { get; set; }

        public User()
        {

        }
        public User(UserDTO t)
        {
            Id= t.Id;
            Name = t.Name;
            Email = t.Email;
        }
    }
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string EncodedJwt { get; set; }

        public UserDTO() 
        { 
           
        }
        public UserDTO(User t)
        {
            Id= t.Id;
            Name = t.Name;
            Email = t.Email;
        }
    }
}
