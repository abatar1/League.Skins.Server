using System;
using System.Linq;
using System.Threading.Tasks;
using League.Skins.Core.Mappers;
using League.Skins.Data.Models;
using League.Skins.Data.Repositories;
using League.Skins.Web.Models;
using Microsoft.Extensions.DependencyInjection;

namespace League.Skins.Core.Services
{
    public static class ChestDropServiceServiceExtensions
    {
        public static void RegisterChestDropServices(this IServiceCollection services)
        {
            services.AddSingleton<ChestDropRepository>();
            services.AddSingleton<ChestDropService>();
            services.AddSingleton<ChestDropMapper>();
        }
    }

    public class ChestDropService
    {
        private readonly ChestDropRepository _chestDropRepository;
        private readonly UserRepository _userRepository;
        private readonly ChestDropMapper _chestDropMapper;

        public ChestDropService(ChestDropRepository chestDropRepository, UserRepository userRepository, ChestDropMapper chestDropMapper)
        {
            _chestDropRepository = chestDropRepository;
            _userRepository = userRepository;
            _chestDropMapper = chestDropMapper;
        }

        public async Task<ServiceResponse> Add(string userId, ChestDropAddRequest request)
        {
            var creator = await _userRepository.GetById(userId);
            var owner = creator;
            if (creator.Id != request.OwnerId)
            {
                owner = await _userRepository.GetById(request.OwnerId);
                if (owner == null)
                    return ServiceResponse.Error(ErrorServiceCodes.EntityNotFound,
                        $"Owner with id {request.OwnerId} not found");
                if (owner.Editors.All(x => x.Id != userId))
                    return ServiceResponse.Error(ErrorServiceCodes.UserNotRelated,
                        $"User with id {userId} not related to user with id {owner.Id}");
            }

            var model = new ChestDrop
            {
                Creator = creator,
                CreationTime = DateTime.UtcNow,
                Comment = request.Comment,
                Owner = owner
            };
            EnumHelper.SetEnum(model, request.ChestType, x => x.ChestType, out var error);
            if (error != null) return error;
            EnumHelper.SetEnum(model, request.DropType, x => x.DropType, out error);
            if (error != null) return error;

            if (new[] {DropType.ChampionSkinPermanent, DropType.ChampionSkinShard}.Contains(model.DropType))
            {
                if (string.IsNullOrEmpty(request.SkinRarity))
                {
                    return ServiceResponse.Error(ErrorServiceCodes.ChestDropInvalidModel,
                        "Drop type is skin, but skin rarity is not given");
                }
                EnumHelper.SetEnum(model, request.SkinRarity, x => x.SkinRarity, out error);
                if (error != null) return error;
            }

            if (!string.IsNullOrEmpty(request.AdditionalDropType))
            {
                EnumHelper.SetEnum(model, request.AdditionalDropType, x => x.AdditionalDropType, out error);
                if (error != null) return error;
            }

            model = await _chestDropRepository.Create(model);
            return ServiceResponse<ChestDropResponse>.Ok(_chestDropMapper.ToResponse(model));
        }
    }
}
