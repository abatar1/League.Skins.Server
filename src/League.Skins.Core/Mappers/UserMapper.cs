using System.Linq;
using League.Skins.Data.Models;
using League.Skins.Web.Models;

namespace League.Skins.Core.Mappers
{
    public class UserMapper
    {
        public UserResponse ToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Login = user.Login,
                Editors = user.Editors.Select(x => x.Id).ToArray(),
                Editable = user.Editable.Select(x => x.Id).ToArray()
            };
        }
    }
}
