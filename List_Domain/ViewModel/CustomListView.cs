using List_Domain.Models;

namespace List_Domain.ViewModel
{
    public class CustomListView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
    }
}
