namespace TravelPlanner.API.Controllers
{
    public class CommentCreateDto
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Date { get; set; }
    }
}