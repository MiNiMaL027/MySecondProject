using List_Domain.Models;

namespace List_Domain.ModelDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } // чому нейм нуллабл а всі інші ні?
        public string Email { get; set; }
        public string EncodedJwt { get; set; }
    }  
}
