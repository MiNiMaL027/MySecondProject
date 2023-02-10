namespace List_Domain.Models.NotDbEntity
{
    public class UserEntity : BaseEntity
    {
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
