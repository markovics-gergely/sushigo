using AutoMapper;
using user.dal.Configurations.Interfaces;
using user.dal.Domain;

namespace user.bll.ValueConverters
{
    public class ImageDisplayUrlConverter : IValueConverter<Image?, string?>
    {
        private readonly IUserConfigurationService _config;

        public ImageDisplayUrlConverter(IUserConfigurationService config)
        {
            _config = config;
        }

        public string? Convert(Image? sourceMember, ResolutionContext context)
        {
            if (sourceMember == null) { return null; }
            var baseUrl = $"/userfiles/{_config.GetStaticFileRequestPath()}/{_config.GetImagesSubdirectory()}";
            return baseUrl + sourceMember.DisplayPath;
        }
    }
}
