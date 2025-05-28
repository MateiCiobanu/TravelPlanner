using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanner.Application.DTOs
{
    public class LikeCreateDto
    {
        public bool Type { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
}
