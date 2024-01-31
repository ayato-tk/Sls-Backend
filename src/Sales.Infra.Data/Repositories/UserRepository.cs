using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Sales.Domain.Entities;
using Sales.Infra.Data.Interfaces;

namespace Sales.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{

    private readonly IMongoCollection<User> _userCollection;

    private readonly IConfiguration _configuration;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        MongoClient mongoClient = new(_configuration["ConnectionStrings:MongoUrl"]);
        var database = mongoClient.GetDatabase("Sales");
        _userCollection = database.GetCollection<User>("Users");
    }

    public async Task<User> SignInUser(string email, string password)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var user = await _userCollection.Find(filter).FirstOrDefaultAsync() ?? throw new Exception("User not exists");
        if (password != user.Password) throw new Exception("Invalid user or password!");
        return user;
    }

}
