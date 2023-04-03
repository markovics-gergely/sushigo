﻿using System.Net;
using shop.dal.Repository.Interfaces;
using shop.dal.Domain;
using shop.dal.Configurations.Interfaces;

namespace shop.dal.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly IShopConfigurationService _galleryConfiguration;

        public FileRepository(IShopConfigurationService galleryConfiguration)
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