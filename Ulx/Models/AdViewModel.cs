namespace Ulx.Models
{
    public class AdViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public DateTime Created { get; set; }

        //public List<string> Files { get; set; }
    }
}
