using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using League.Skins.Data.Models;
using MongoDB.Driver;

namespace League.Skins.Data.Repositories
{
    public class ChestDropRepository
    {
        private readonly IMongoCollection<ChestDrop> _chestDrops;

        private void CreateIndex(Expression<Func<ChestDrop, object>> indexLambda)
        {
            var key = Builders<ChestDrop>.IndexKeys.Ascending(indexLambda);
            _chestDrops.Indexes.CreateOne(new CreateIndexModel<ChestDrop>(key));
        }

        public ChestDropRepository(ISkinsStatisticsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _chestDrops = database.GetCollection<ChestDrop>(settings.ChestDropsCollectionName);
        }

        public async Task<List<ChestDrop>> GetAll() =>
            await _chestDrops.Find(book => true).ToListAsync();

        public async Task<ChestDrop> Get(string id) =>
            await _chestDrops.Find(book => book.Id == id).FirstOrDefaultAsync();

        public async Task<ChestDrop> Create(ChestDrop book)
        {
            await _chestDrops.InsertOneAsync(book);
            return book;
        }
    }
}
