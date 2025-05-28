using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, ILikeRepository likeRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _postRepository.GetPosts();
                var postDtos = _mapper.Map<List<PostDto>>(posts);

                return Ok(postDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] PostCreateDto postDto)
        {
            try
            {
                if (postDto == null)
                    return BadRequest(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var postMap = _mapper.Map<Post>(postDto);

                if (!await _postRepository.CreatePost(postMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving post");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikePost([FromBody] LikeCreateDto likeDto)
        {
            try
            {
                if (likeDto == null)
                    return BadRequest(ModelState);

                var like = await _likeRepository.GetPostLikeByIds(likeDto.UserId, likeDto.CommentId);

                if (like != null)
                {
                    ModelState.AddModelError("", "User already exists");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var likeMap = _mapper.Map<Like>(likeDto);
                likeMap.PostId = likeDto.CommentId;

                if (!await _likeRepository.Like(likeMap))
                {
                    ModelState.AddModelError("", "Something went wrong while savin");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("unlike")]
        public async Task<IActionResult> DeleteLikePost([FromBody] LikeDto likeDto)
        {
            try
            {
                if (!await _likeRepository.PostLikeExists(likeDto.UserId, likeDto.CommentId))
                {
                    return NotFound();
                }

                var likeToDelete = await _likeRepository.GetPostLikeByIds(likeDto.UserId, likeDto.CommentId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _likeRepository.Unlike(likeToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting like");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("update_like")]
        public async Task<IActionResult> UpdateLikePost([FromBody] LikeDto likeDto)
        {
            try
            {
                if (!await _likeRepository.PostLikeExists(likeDto.UserId, likeDto.CommentId))
                {
                    return NotFound();
                }

                var likeToUpdate = await _likeRepository.GetPostLikeByIds(likeDto.UserId, likeDto.CommentId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                likeToUpdate.Type = !likeToUpdate.Type;

                if (!await _likeRepository.UpdateLike(likeToUpdate))
                {
                    ModelState.AddModelError("", "Something went wrong updating like");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
