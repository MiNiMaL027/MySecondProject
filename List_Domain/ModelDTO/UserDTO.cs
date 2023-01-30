using List_Domain.Models;

namespace List_Domain.ModelDTO
{
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
            Id = t.Id;
            Name = t.Name;
            Email = t.Email;
        }
    }  
}
