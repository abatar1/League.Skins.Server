using League.Skins.Data.Models;
using League.Skins.Web.Models;

namespace League.Skins.Core.Mappers
{
    public class ChestDropMapper
    {
        public ChestDropResponse ToResponse(ChestDrop chestDrop)
        {
            return new ChestDropResponse
            {
                Id = chestDrop.Id,
                ChestType = chestDrop.ChestType.ToString(),
                DropType = chestDrop.DropType.ToString(),
                AdditionalDropType = chestDrop.AdditionalDropType?.ToString(),
                SkinRarity = chestDrop.SkinRarity?.ToString(),
                CreationTime = chestDrop.CreationTime,
                Comment = chestDrop.Comment
            };
        }
    }
}
