using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using webapi.Models;

namespace webapi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> users;
        private readonly IDataProtector protector;
        private readonly string key;

        public UserService(IConfiguration configuration, IDataProtectionProvider protectorProvider)
        {
            var client = new MongoClient(configuration.GetConnectionString("testDb"));
            var database = client.GetDatabase("testDb");
            users = database.GetCollection<User>("Users");
            key = configuration.GetSection("JwtKey").ToString() ?? "";
            protector = protectorProvider.CreateProtector(configuration.GetSection("ProtectorKey").ToString() ?? "");
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var encryptedUsers = await users.Find(_ => true).ToListAsync();
            return encryptedUsers.Select(DecryptUser).ToList();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            var encryptedUser = await users.Find(user => user.Id == id).FirstOrDefaultAsync();
            return encryptedUser == null ? null : DecryptUser(encryptedUser);
        }

        public async Task CreateUserAsync(User user)
        {
            user.FirstName = string.IsNullOrEmpty(user.FirstName) ? "" : protector.Protect(user.FirstName);
            user.LastName = string.IsNullOrEmpty(user.LastName) ? "" : protector.Protect(user.LastName);
            user.UserName = user.UserName;
            user.Password = PasswordHasher.HashPassword(user.Password);
            user.Products = [];

            await users.InsertOneAsync(user);
        }

        public User DecryptUser(User user)
        {
            user.Id = user.Id;
            user.FirstName = string.IsNullOrEmpty(user.FirstName) ? "" : protector.Unprotect(user.FirstName ?? "");
            user.LastName = string.IsNullOrEmpty(user.LastName) ? "" : protector.Unprotect(user.LastName ?? "");
            user.UserName = user.UserName;
            user.Password = "";
            return user;
        }


        public async Task DeleteUserAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var result = await users.DeleteOneAsync(filter);
        }

        public async Task<(string?, User?)> AuthenticateUserAsync(string username, string password)
        {
            var user = await users.Find(user => user.UserName == username).FirstOrDefaultAsync();
            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password)) return (null, null);
            var decryptedUser = DecryptUser(user);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new(ClaimTypes.NameIdentifier, user.Id ?? ""),
                    new(ClaimTypes.Name, username)
                ]),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), decryptedUser);
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