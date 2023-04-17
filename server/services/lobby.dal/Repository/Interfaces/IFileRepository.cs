using lobby.dal.Domain;

namespace lobby.dal.Repository.Interfaces
{
    public interface IFileRepository
    {
        Image SaveFile(Guid userId, string tempFilePath, string extension);

        void DeleteFile(string filePath);

        string SanitizeFilename(string fileName);
    }
}
