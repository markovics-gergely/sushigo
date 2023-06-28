namespace shared.dal.Models
{
    /// <summary>
    /// Types of user roles
    /// </summary>
    public static class RoleTypes
    {
        /// <summary>
        /// Permission for the basic game
        /// </summary>
        public const string Classic = "Classic";

        /// <summary>
        /// Permission for every game mode
        /// </summary>
        public const string Party = "Party";

        /// <summary>
        /// Scope name of the role
        /// </summary>
        public const string RoleScope = "roles";

        /// <summary>
        /// Name of the claim for experience
        /// </summary>
        public const string ExpClaim = "experience";

        /// <summary>
        /// Name of the claim for game types
        /// </summary>
        public const string DeckClaim = "decks";

        /// <summary>
        /// Name of the claim for avatar url
        /// </summary>
        public const string AvatarClaim = "avatar";

        /// <summary>
        /// Name of the claim for active lobby
        /// </summary>
        public const string LobbyClaim = "lobby";

        /// <summary>
        /// Experience needed to claim party role
        /// </summary>
        public const long PartyExp = 100;

        /// <summary>
        /// Experience needed to claim a game type
        /// </summary>
        public const long DeckExp = 50;
    }
}
