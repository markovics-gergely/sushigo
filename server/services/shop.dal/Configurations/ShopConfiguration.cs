namespace shop.dal.Configurations
{
    public class ShopConfiguration
    {
        public int MaxUploadSize { get; set; } = 25;

        public string StaticFilePath { get; set; } = string.Empty;
    }
}
