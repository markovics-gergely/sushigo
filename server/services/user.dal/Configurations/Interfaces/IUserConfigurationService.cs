namespace user.dal.Configurations.Interfaces
{
    public interface IUserConfigurationService
    {
        int GetMaxUploadSize();
        string GetStaticFilePhysicalPath();
        string GetStaticFileRequestPath();
        string GetImagesSubdirectory();
    }
}
