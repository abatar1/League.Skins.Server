using System.Collections.Generic;
using System.Threading.Tasks;
using League.Skins.Data.Models;
using MongoDB.Driver;

namespace League.Skins.Data.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _chestDrops;

        public UserRepository(ISkinsStatisticsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _chestDrops = database.GetCollection<User>(settings.UserCollectionName);

            var key = Builders<User>.IndexKeys.Ascending(x => x.Login);
            _chestDrops.Indexes.CreateOne(new CreateIndexModel<User>(key));
        }

        public async Task<List<User>> GetAll() =>
            await _chestDrops.Find(user => true).ToListAsync();

        public async Task<User> GetById(string id) =>
            await _chestDrops.Find(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<User> GetByLogin(string login) =>
            await _chestDrops.Find(user => user.Login == login).FirstOrDefaultAsync();

        public async Task Create(User user)
        {
            await _chestDrops.InsertOneAsync(user);
        }

        public async Task Update(User updatedUser)
        {
            await _chestDrops.ReplaceOneAsync(user => user.Id == updatedUser.Id, updatedUser);
        }
    }
}
