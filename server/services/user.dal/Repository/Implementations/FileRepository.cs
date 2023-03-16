using System.Net;
using user.dal.Configurations.Interfaces;
using user.dal.Domain;
using user.dal.Repository.Interfaces;

namespace user.dal.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly IUserConfigurationService _galleryConfiguration;

        public FileRepository(IUserConfigurationService galleryConfiguration)
        {
            _galleryConfiguration = galleryConfiguration;
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string SanitizeFilename(string fileName)
        {
            var file = Path.GetFileName(fileName);
            var htmlEncoded = WebUtility.HtmlEncode(file);
            return htmlEncoded;
        }

        public Image SaveFile(Guid userId, string tempFilePath, string extension)
        {
            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var separator = Path.DirectorySeparatorChar;
            var userDir = $"{_galleryConfiguration.GetStaticFilePhysicalPath()}{separator}{_galleryConfiguration.GetImagesSubdirectory()}{separator}{userId}";
            var savePath = $"{userDir}{separator}{fileName}{extension}";
            if (!Directory.Exists(userDir))
            {
                Directory.CreateDirectory(userDir);
            }
            File.Copy(tempFilePath, savePath);
            File.Delete(tempFilePath);
            var attributes = new FileInfo(savePath);
            return new Image
            {
                DisplayPath = $"/{userId}/{fileName}{extension}",
                PhysicalPath = savePath,
                Size = attributes.Length / Math.Pow(1024, 2),
                FileExtension = extension
            };
        }
    }
}
