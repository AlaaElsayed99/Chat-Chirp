namespace API.DTO
{
    public class PhotoDTO:BaseEntity
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        
    }
}