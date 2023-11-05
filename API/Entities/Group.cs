namespace API.Entities
{
    public class Group
    {
        public Group()
        {
            
        }
        public Group(string? name)
        {
            Name = name;
        }

        [Key]
        public  string? Name { get; set; }
        public ICollection<connection>? Connections { get; set; }= new List<connection>();
    }
}
