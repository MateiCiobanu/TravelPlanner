using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string GooglePlaceId { get; set; }
        public string? ImagePath { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
