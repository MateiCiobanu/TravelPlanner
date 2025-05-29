using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string GooglePlaceId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public int CommentsNum { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public int NumLikes { get; set; }
        public int NumDislikes { get; set; }
        public bool LikedByUser { get; set; }
        public bool DislikedByUser { get; set; }
    }
}
