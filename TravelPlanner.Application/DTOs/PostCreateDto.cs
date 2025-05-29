using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TravelPlanner.Application.DTOs
{
    public class PostCreateDto
    {
        public string Title { get; set; }
        public string GooglePlaceId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public IFormFile Image { get; set; }
    }
}
