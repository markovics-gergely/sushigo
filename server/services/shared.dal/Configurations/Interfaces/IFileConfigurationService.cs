namespace shared.dal.Configurations.Interfaces
{
    public interface IFileConfigurationService
    {
        int GetMaxUploadSize();
        string GetStaticFilePhysicalPath();
        string GetStaticFileRequestPath();
        string GetImagesSubdirectory();
    }
}
