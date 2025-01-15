namespace UrlShortener.Function.MappingProfiles
{
    using AutoMapper;
    using UrlShortener.Core.Models;
    using UrlShortener.Function.DTOs;

    /// <summary>
    /// Defines the AutoMapper profile for mapping between <see cref="ShortenedUrl"/> and <see cref="ShortenedUrlResponse"/>.
    /// </summary>
    public sealed class ShortenedUrlProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortenedUrlProfile"/> class.
        /// </summary>
        public ShortenedUrlProfile()
        {
            CreateMap<ShortenedUrl, ShortenedUrlResponse>().ReverseMap();
        }
    }
}
