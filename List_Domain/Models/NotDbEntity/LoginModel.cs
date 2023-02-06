using System.ComponentModel.DataAnnotations;

namespace List_Domain.Models.NotDbEntity
{
    public class LoginModel
    {
        public string Email { get; set; }// не хотів всюди писати, пусті лянійки між пропертями, всюди додай бо зараз ріже око коли є атрибути
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
