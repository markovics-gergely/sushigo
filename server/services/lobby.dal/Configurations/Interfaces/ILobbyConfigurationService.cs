namespace lobby.dal.Configurations.Interfaces
{
    public interface ILobbyConfigurationService
    {
        int GetMaxUploadSize();
        string GetStaticFilePhysicalPath();
        string GetStaticFileRequestPath();
        string GetImagesSubdirectory();
    }
}
