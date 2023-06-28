using System.Net;
using shared.dal.Configurations.Interfaces;
using shared.dal.Models;
using shared.dal.Repository.Interfaces;

namespace shared.dal.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly IFileConfigurationService _fileConfiguration;

        public FileRepository(IFileConfigurationService fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
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
            var userDir = $"{_fileConfiguration.GetStaticFilePhysicalPath()}{separator}{_fileConfiguration.GetImagesSubdirectory()}{separator}{userId}";
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
