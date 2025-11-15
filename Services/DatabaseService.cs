using Microsoft.Data.Sqlite;
using Dapper;
using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CoffeeShop.db");
            _connectionString = $"Data Source={dbPath}";
        }

        public IEnumerable<CoffeeProduct> GetAllProducts()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection.Query<CoffeeProduct>("SELECT * FROM CoffeeProducts");
        }

        public CoffeeProduct? GetProductById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection.QueryFirstOrDefault<CoffeeProduct>(
                "SELECT * FROM CoffeeProducts WHERE Id = @Id", new { Id = id });
        }

        public IEnumerable<CoffeeProduct> GetProductsByCategory(string category)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection.Query<CoffeeProduct>(
                "SELECT * FROM CoffeeProducts WHERE Category = @Category", 
                new { Category = category });
        }

        public IEnumerable<string> GetCategories()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection.Query<string>("SELECT DISTINCT Category FROM CoffeeProducts");
        }

        public void AddProduct(CoffeeProduct product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            connection.Execute(@"
                INSERT INTO CoffeeProducts (Name, Description, Price, Category, ImageUrl, Origin, RoastLevel, Stock)
                VALUES (@Name, @Description, @Price, @Category, @ImageUrl, @Origin, @RoastLevel, @Stock)",
                product);
        }

        public bool UpdateProduct(CoffeeProduct product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var rowsAffected = connection.Execute(@"
                UPDATE CoffeeProducts 
                SET Name = @Name, Description = @Description, Price = @Price, Category = @Category, 
                    ImageUrl = @ImageUrl, Origin = @Origin, RoastLevel = @RoastLevel, Stock = @Stock
                WHERE Id = @Id",
                product);
            return rowsAffected > 0;
        }

        public bool DeleteProduct(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var rowsAffected = connection.Execute("DELETE FROM CoffeeProducts WHERE Id = @Id", new { Id = id });
            return rowsAffected > 0;
        }
    }
}