namespace todolist_api.Models
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ListDto> Lists { get; set; } = [];
    }
}