using System.ComponentModel.DataAnnotations;

namespace List_Domain.Models.NotDbEntity
{
    public class LoginModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
