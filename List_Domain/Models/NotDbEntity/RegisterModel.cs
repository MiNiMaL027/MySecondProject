using System.ComponentModel.DataAnnotations;

namespace List_Domain.Models.NotDbEntity
{
    public class RegisterModel
    {
        public string Email { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
    }
}
