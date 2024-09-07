namespace Chapter_8___Integrating_Minimal_APIs_with_Data_Sources
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Core.Configuration;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    public class MongoDbService : IDatabaseService
    {
        private readonly IMongoCollection<IEmployee> _employeesCollection;
        private readonly string _connectionString;
        
        public MongoDbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MongoDbConnection");
            var mongoClient = new MongoClient(_connectionString);
            var mongoDatabase = mongoClient.GetDatabase("MyCompany");
            _employeesCollection = mongoDatabase.GetCollection<IEmployee>("Employees");
        }
        
        public async Task<IEnumerable<IEmployee>> GetEmployeesAsync()
        {
            var result = await _employeesCollection.Find(new BsonDocument()).ToListAsync();
            return result;
        }
        
        public async Task AddEmployeeAsync(IEmployee employee)
        {
            var employeeToAdd = (Employee)employee;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(
                    "INSERT INTO Employees (Name, Salary, Address, City, Region, Country, Phone) VALUES (@Name, " +
                                                                "@Salary, @Address, @City, @Region, @Country, @Phone)", connection))
                {
                    command.Parameters.AddWithValue("@Name", employeeToAdd.Name);
                    command.Parameters.AddWithValue("@Salary", employeeToAdd.Salary);
                    command.Parameters.AddWithValue("@Address", employeeToAdd.Address);
                    command.Parameters.AddWithValue("@City", employeeToAdd.City);
                    command.Parameters.AddWithValue("@Region", employeeToAdd.Region);
                    command.Parameters.AddWithValue("@Country", employeeToAdd.Country);
                    command.Parameters.AddWithValue("@Phone", employeeToAdd.Phone);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
