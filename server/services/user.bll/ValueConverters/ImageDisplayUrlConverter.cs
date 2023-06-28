using AutoMapper;
using shared.dal.Configurations.Interfaces;
using shared.dal.Models;

namespace user.bll.ValueConverters
{
    public class ImageDisplayUrlConverter : IValueConverter<Image?, string?>
    {
        private readonly IFileConfigurationService _config;

        public ImageDisplayUrlConverter(IFileConfigurationService config)
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
