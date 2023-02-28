using List_Domain.Models.NotDbEntity;

namespace List_Domain.Models
{
    public class Settings : UserEntity
    {
        public bool AllowNotification { get; set; }

        public int? DefaultListId { get; set; }

        public CustomList? DefaultList { get; set; }
    }
}
