using user.dal.Types;

namespace user.dal.Domain
{
    public class Friend
    {
        public Guid Id { get; set; }
        public required Guid SenderId { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
        public required Guid ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } = null!;
        public required bool Pending { get; set; } = true;
        public FriendTypes GetFriendTypes(Guid guid)
        {
            if (guid == SenderId && Pending) return FriendTypes.Sent;
            if (guid == ReceiverId && Pending) return FriendTypes.Received;
            if (guid == SenderId || guid == ReceiverId) return FriendTypes.Friend;
            return FriendTypes.None;
        }
        public FriendTypes GetFriendTypes(Guid guid1, Guid guid2)
        {
            if (GetFriendTypes(guid1) == FriendTypes.Friend && GetFriendTypes(guid2) == FriendTypes.Friend) return FriendTypes.Friend;
            return FriendTypes.None;
        }

        public bool OwnFriend(Guid guid1, Guid guid2)
        {
            return (SenderId == guid1 && ReceiverId == guid2) || (SenderId == guid2 && ReceiverId == guid1);
        }
    }
}
