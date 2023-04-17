using AutoMapper;
using shop.dal.Configurations.Interfaces;

namespace shop.bll.ValueConverters
{
    public class ImageDisplayUrlConverter : IValueConverter<string, string>
    {
        private readonly IShopConfigurationService _config;

        public ImageDisplayUrlConverter(IShopConfigurationService config)
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
