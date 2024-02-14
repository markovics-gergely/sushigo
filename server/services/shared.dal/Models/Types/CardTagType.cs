using System.ComponentModel;

namespace shared.dal.Models.Types
{
    public enum CardTagType
    {
        [Description("None")]
        NONE = 0,
        [Description("Converted")]
        CONVERTED = 1,
        [Description("Used")]
        USED = 2,
        [Description("Wasabi")]
        WASABI = 3,
    }
}
