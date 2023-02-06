using List_Domain.Models; // лишнє

namespace List_Domain.ViewModel
{
    public class CustomListView // ВюМодел
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
    }
}
