namespace user.dal.Domain
{
    /// <summary>
    /// History of games played by users
    /// </summary>
    public class History
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Placement { get; set; }
        public int Point {  get; set; }
        public DateTime Created { get; set; }
    }
}
