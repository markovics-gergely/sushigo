using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using shop.dal.Configurations.Interfaces;

namespace shop.dal.Configurations.Implementations
{
    public class ShopConfigurationService : IShopConfigurationService
    {
        private readonly ShopConfiguration _config;
        private readonly string _defaultContentPath;
        private const string _staticRequestPath = "files";
        private const string _imagesRelativePath = "images";

        public ShopConfigurationService(IOptions<ShopConfiguration> options, IHostingEnvironment environment)
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
