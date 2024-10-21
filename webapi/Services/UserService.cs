using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using webapi.Models;

namespace webapi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> users;
        private readonly string key;

        public UserService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("testDb"));
            var database = client.GetDatabase("testDb");
            users = database.GetCollection<User>("Users");
            key = configuration.GetSection("JwtKey").ToString() ?? "";
        }

        public async Task<List<User>> GetUsersAsync() => await users.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserAsync(string id) => await users.Find(user => user.Id == id).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user) => await users.InsertOneAsync(user);

        public async Task<(string?, User?)> AuthenticateUserAsync(string username, string password)
        {
            var user = await users.Find(user => user.UserName == username && user.Password == password).FirstOrDefaultAsync();
            if (user == null) return (null, null);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new(ClaimTypes.NameIdentifier, user.Id ?? ""),
                    new(ClaimTypes.Name, username)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), user);
        }

        public async Task<List<Product>> GetProducts(string userId)
        {
            var user = await GetUserAsync(userId);
            return user?.Products ?? [];
        }

        public async Task CreateProduct(string userId, Product product)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update
                .Push(user => user.Products, product);
            var result = await users.UpdateOneAsync(filter, update);
        }
    }
}