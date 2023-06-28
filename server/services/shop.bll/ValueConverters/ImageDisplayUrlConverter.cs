using AutoMapper;
using shared.dal.Configurations.Interfaces;

namespace shop.bll.ValueConverters
{
    public class ImageDisplayUrlConverter : IValueConverter<string, string>
    {
        private readonly IFileConfigurationService _config;

        public ImageDisplayUrlConverter(IFileConfigurationService config)
        {
            _config = config;
        }

        public string Convert(string sourceMember, ResolutionContext context)
        {
            var baseUrl = $"/shopfiles/{_config.GetStaticFileRequestPath()}/{_config.GetImagesSubdirectory()}";
            return baseUrl + sourceMember;
        }
    }
}
