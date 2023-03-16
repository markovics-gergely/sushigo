namespace user.dal.Configurations
{
    public class UserConfiguration
    {
        public int MaxUploadSize { get; set; } = 25;

        public string StaticFilePath { get; set; } = string.Empty;
    }
}
