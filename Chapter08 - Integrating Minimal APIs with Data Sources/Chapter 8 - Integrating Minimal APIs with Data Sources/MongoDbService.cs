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
        private readonly IMongoCollection<EmployeeMongoDb> _employeesCollection;
        private readonly string _connectionString;
        
        public MongoDbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MongoDbConnection");
            var mongoClient = new MongoClient(_connectionString);
            var mongoDatabase = mongoClient.GetDatabase("MyCompany");
            _employeesCollection = mongoDatabase.GetCollection<EmployeeMongoDb>("Employees");
        }
        
        public async Task<IEnumerable<IEmployee>> GetEmployeesAsync()
        {
            var result = await _employeesCollection.Find(new BsonDocument()).ToListAsync();
            return result;
        }

        public async Task AddEmployeeAsync(IEmployee employee)
        {
            var employeeToAdd = new EmployeeMongoDb
            {
                Name = employee.Name,
                Salary = employee.Salary,
                Address = employee.Address,
                City = employee.City,
                Region = employee.Region,
                PostalCode = employee.PostalCode,
                Country = employee.Country,
                Phone = employee.Phone
            };
            await _employeesCollection.InsertOneAsync(employeeToAdd);
          
        }
    }
}
