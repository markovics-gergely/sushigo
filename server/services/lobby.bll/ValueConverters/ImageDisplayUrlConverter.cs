using AutoMapper;
using lobby.dal.Configurations.Interfaces;

namespace lobby.bll.ValueConverters
{
    public class ImageDisplayUrlConverter : IValueConverter<string, string>
    {
        private readonly ILobbyConfigurationService _config;

        public ImageDisplayUrlConverter(ILobbyConfigurationService config)
        {
            _config = config;
        }

        public string Convert(string sourceMember, ResolutionContext context)
        {
            var baseUrl = $"/lobbyfiles/{_config.GetStaticFileRequestPath()}/{_config.GetImagesSubdirectory()}";
            return baseUrl + sourceMember;
        }
    }
}
