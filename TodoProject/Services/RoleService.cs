using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TodoProject.models;
using TodoProject.Models;

namespace TodoProject.Services
{
    public class RoleService
    {
        private readonly IMongoCollection<Role> roleCollection;
        private readonly IOptions<TodoDatabaseSettings> dbSettings;
        private readonly CurrentUserAccessor currentUser;
        private readonly UserService userService;

        public RoleService(AppDbService appDb, IOptions<TodoDatabaseSettings> dbSettings, CurrentUserAccessor currentUser, UserService userService)
        {
            this.dbSettings = dbSettings;
            this.currentUser = currentUser;
            this.userService = userService;
            this.roleCollection = appDb.MongoDb.GetCollection<Role>(dbSettings.Value.RolesCollectionName);
        }

        public async Task<List<Role>> GetAll()
        {
            return await roleCollection.Find(x => true).ToListAsync();
        }

        public async Task<Boolean> AssignRole(string userId, string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId)) return false;

            var user = await userService.GetById(userId);
            if (user == null) return false;

            var role = await roleCollection.Find(x => x.Id == roleId).FirstOrDefaultAsync();
            if (role == null) return false;

            var objId = ObjectId.Parse(roleId);
            if (user.RoleIds.Contains(objId)) return true;

            user.RoleIds.Add(objId);
            return await userService.Update(user);
        }

        public async Task<Boolean> UnassignRole(string userId, string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId)) return false;

            var user = await userService.GetById(userId);
            if (user == null) return false;

            var role = await roleCollection.Find(x => x.Id == roleId).FirstOrDefaultAsync();
            if (role == null) return false;

            var objId = ObjectId.Parse(roleId);
            if (!user.RoleIds.Contains(objId)) return true;

            user.RoleIds.Remove(objId);
            return await userService.Update(user);
        }
    }
}
