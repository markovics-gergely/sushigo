using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using user.dal.Configurations.Interfaces;

namespace user.dal.Configurations.Implementations
{
    public class UserConfigurationService : IUserConfigurationService
    {
        private readonly UserConfiguration _config;
        private readonly string _defaultContentPath;
        private const string _staticRequestPath = "files";
        private const string _imagesRelativePath = "images";

        public UserConfigurationService(IOptions<UserConfiguration> options, IHostingEnvironment environment)
        {
            _defaultContentPath = environment.WebRootPath;
            _config = options.Value;
        }

        public int GetMaxUploadSize()
        {
            return _config.MaxUploadSize;
        }

        public string GetStaticFilePhysicalPath()
        {
            var webroot = string.IsNullOrWhiteSpace(_config.StaticFilePath) ? _defaultContentPath : _config.StaticFilePath;
            return webroot;
        }

        public string GetImagesSubdirectory()
        {
            return _imagesRelativePath;
        }

        public string GetStaticFileRequestPath()
        {
            return _staticRequestPath;
        }
    }
}
