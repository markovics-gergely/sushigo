namespace user.dal.Configurations.Interfaces
{
    public interface IShopConfigurationService
    {
        int GetMaxUploadSize();
        string GetStaticFilePhysicalPath();
        string GetStaticFileRequestPath();
        string GetImagesSubdirectory();
    }
}
