namespace List_Domain.Models
{
    public class EmailConfirmationCode
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string ConfirmationCode { get; set; }
        public int EmailConfirmationLeftAttempts { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
