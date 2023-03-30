using shop.dal.Domain;

namespace shop.dal.Repository.Interfaces
{
    public interface IFileRepository
    {
        Image SaveFile(Guid userId, string tempFilePath, string extension);

        void DeleteFile(string filePath);

        string SanitizeFilename(string fileName);
    }
}
