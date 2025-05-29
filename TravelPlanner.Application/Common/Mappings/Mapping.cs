using AutoMapper;
using TravelPlanner.API.Controllers;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application.Common.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            
            // Trip mappings - EKLENEN KISIM
            CreateMap<TripCreationDto, Trip>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "planning"))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ItineraryItems, opt => opt.Ignore());
            
            CreateMap<Trip, TripDto>()
                .ForMember(dest => dest.ItineraryItems, opt => opt.Ignore()); // Will be set manually
            
            CreateMap<ItineraryItemDto, ItineraryItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Trip, opt => opt.Ignore());
            
            CreateMap<ItineraryItem, ItineraryItemDto>();
            
            // Existing mappings
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.CommentsNum, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.NumLikes, opt => opt.MapFrom<NumPostLikesResolver>())
                .ForMember(dest => dest.NumDislikes, opt => opt.MapFrom<NumPostDislikesResolver>())
                .ForMember(dest => dest.LikedByUser, opt => opt.MapFrom<PostLikedByUser>())
                .ForMember(dest => dest.DislikedByUser, opt => opt.MapFrom<PostDislikedByUser>());
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostCreateDto, Comment>();
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.NumLikes, opt => opt.MapFrom<NumCommentLikesResolver>())
                .ForMember(dest => dest.NumDislikes, opt => opt.MapFrom<NumCommentDislikesResolver>())
                .ForMember(dest => dest.LikedByUser, opt => opt.MapFrom<CommentLikedByUser>())
                .ForMember(dest => dest.DislikedByUser, opt => opt.MapFrom<CommentDislikedByUser>());
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<LikeCreateDto, Like>()
                .ForMember(dest => dest.CommentId, opt => opt.Ignore());
        }
    }
}