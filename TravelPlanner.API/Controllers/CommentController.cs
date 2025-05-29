using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto commentDto)
        {
            try
            {
                if (commentDto == null)
                    return BadRequest(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var comment = _mapper.Map<Comment>(commentDto);

                if (!await _commentRepository.CreateComment(comment))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                if (!await _commentRepository.CommentExists(commentId))
                {
                    return NotFound();
                }

                var commentLikes = await _likeRepository.GetLikesByCommentId(commentId);
                var commentToDelete = await _commentRepository.GetCommentById(commentId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _likeRepository.DeleteCommentLikes(commentLikes))
                {
                    ModelState.AddModelError("", "Something went wrong deleting Comment Likes");
                }

                if (!await _commentRepository.DeleteComment(commentToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting Comment");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikeComment([FromBody] LikeCreateDto likeDto)
        {
            try
            {
                if (likeDto == null)
                    return BadRequest(ModelState);

                var like = await _likeRepository.GetCommentLikeByIds(likeDto.UserId, likeDto.CommentId);

                if (like != null)
                {
                    ModelState.AddModelError("", "Like already exists");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var likeMap = _mapper.Map<Like>(likeDto);
                likeMap.CommentId = likeDto.CommentId;

                if (!await _likeRepository.Like(likeMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
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
        public async Task<IActionResult> DeleteLikeComment([FromBody] LikeDto likeDto)
        {
            try
            {
                if (!await _likeRepository.CommentLikeExists(likeDto.UserId, likeDto.CommentId))
                {
                    return NotFound();
                }

                var likeToDelete = await _likeRepository.GetCommentLikeByIds(likeDto.UserId, likeDto.CommentId);

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
        public async Task<IActionResult> UpdateLikeComment([FromBody] LikeDto likeDto)
        {
            try
            {
                if (!await _likeRepository.CommentLikeExists(likeDto.UserId, likeDto.CommentId))
                {
                    return NotFound();
                }

                var likeToUpdate = await _likeRepository.GetCommentLikeByIds(likeDto.UserId, likeDto.CommentId);

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

