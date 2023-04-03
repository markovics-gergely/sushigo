namespace shop.dal.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        public Card? Card { get; set; }

        public string PhysicalPath { get; set; } = string.Empty;

        public string DisplayPath { get; set; } = string.Empty;

        public double Size { get; set; }

        public string FileExtension { get; set; } = string.Empty;
    }
}
