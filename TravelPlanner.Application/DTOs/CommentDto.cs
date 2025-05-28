using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanner.Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int NumLikes { get; set; }
        public int NumDislikes { get; set; }
        public bool LikedByUser { get; set; }
        public bool DislikedByUser { get; set; }
    }
}
