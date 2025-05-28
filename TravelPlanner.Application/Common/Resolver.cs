using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Application.Common
{
    public class NumCommentDislikesResolver : IValueResolver<Comment, CommentDto, int>
    {
        private readonly ILikeRepository _likeRepository;

        public NumCommentDislikesResolver(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public int Resolve(Comment source, CommentDto destination, int destMember, ResolutionContext context)
        {
            return _likeRepository.GetCommentDislikes(source.Id).GetAwaiter().GetResult();
        }
    }

    public class NumCommentLikesResolver : IValueResolver<Comment, CommentDto, int>
    {
        private readonly ILikeRepository _likeRepository;

        public NumCommentLikesResolver(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public int Resolve(Comment source, CommentDto destination, int destMember, ResolutionContext context)
        {
            return _likeRepository.GetCommentLikes(source.Id).GetAwaiter().GetResult();
        }
    }

    public class NumPostDislikesResolver : IValueResolver<Post, PostDto, int>
    {
        private readonly ILikeRepository _likeRepository;

        public NumPostDislikesResolver(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public int Resolve(Post source, PostDto destination, int destMember, ResolutionContext context)
        {
            return _likeRepository.GetPostDislikes(source.Id).GetAwaiter().GetResult();
        }
    }

    public class NumPostLikesResolver : IValueResolver<Post, PostDto, int>
    {
        private readonly ILikeRepository _likeRepository;

        public NumPostLikesResolver(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public int Resolve(Post source, PostDto destination, int destMember, ResolutionContext context)
        {
            return _likeRepository.GetPostLikes(source.Id).GetAwaiter().GetResult();
        }
    }

    public class CommentLikedByUser : IValueResolver<Comment, CommentDto, bool>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentLikedByUser(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _likeRepository = likeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Resolve(Comment source, CommentDto destination, bool destMember, ResolutionContext context)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    // No other changes are needed in the file as the error is caused by the missing namespace.
                    if (int.TryParse(userString, out int userId))
                        return _likeRepository.CommentLikedByUser(userId, source.Id).GetAwaiter().GetResult();
                }
            }
            return false;
        }
    }

    public class CommentDislikedByUser : IValueResolver<Comment, CommentDto, bool>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentDislikedByUser(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _likeRepository = likeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Resolve(Comment source, CommentDto destination, bool destMember, ResolutionContext context)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (int.TryParse(userString, out int userId))
                        return _likeRepository.CommentDislikedByUser(userId, source.Id).GetAwaiter().GetResult();
                }
            }
            return false;
        }
    }

    public class PostLikedByUser : IValueResolver<Post, PostDto, bool>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostLikedByUser(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _likeRepository = likeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Resolve(Post source, PostDto destination, bool destMember, ResolutionContext context)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (int.TryParse(userString, out int userId))
                        return _likeRepository.PostLikedByUser(userId, source.Id).GetAwaiter().GetResult();
                }
            }
            return false;
        }
    }

    public class PostDislikedByUser : IValueResolver<Post, PostDto, bool>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostDislikedByUser(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _likeRepository = likeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Resolve(Post source, PostDto destination, bool destMember, ResolutionContext context)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (int.TryParse(userString, out int userId))
                        return _likeRepository.PostDislikedByUser(userId, source.Id).GetAwaiter().GetResult();
                }
            }
            return false;
        }
    }
}
