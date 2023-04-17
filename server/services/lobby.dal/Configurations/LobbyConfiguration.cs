namespace lobby.dal.Configurations
{
    public class LobbyConfiguration
    {
        public int MaxUploadSize { get; set; } = 25;

        public string StaticFilePath { get; set; } = string.Empty;
    }
}
