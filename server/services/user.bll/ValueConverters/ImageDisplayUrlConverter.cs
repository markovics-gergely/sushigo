using AutoMapper;
using Microsoft.AspNetCore.Http;
using user.dal.Configurations.Interfaces;
using user.dal.Domain;

namespace user.bll.ValueConverters
{
    public class ImageDisplayUrlConverter : IValueConverter<Image?, string?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserConfigurationService _config;

        public ImageDisplayUrlConverter(IHttpContextAccessor httpContextAccessor, IUserConfigurationService config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public string? Convert(Image? sourceMember, ResolutionContext context)
        {
            if (sourceMember == null) { return null; }
            var baseUrl = $"/{_config.GetStaticFileRequestPath()}/{_config.GetImagesSubdirectory()}";
            return baseUrl + sourceMember.DisplayPath;
        }
    }
}
