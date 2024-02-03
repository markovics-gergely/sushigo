using Microsoft.EntityFrameworkCore.ChangeTracking;
using shared.dal.Models;

namespace shared.dal.Comparers
{
    internal class CardTypePointValueComparer : ValueComparer<CardTypePoint>
    {
        public CardTypePointValueComparer() : base(
            (c1, c2) => 
            (c1 == null && c2 == null) ||
            (c1 != null && c2 != null && c1.CardType == c2.CardType && c1.Point == c2.Point)
            ,
            c => c.ToString().GetHashCode()
        )
        {
        }
    }
}
