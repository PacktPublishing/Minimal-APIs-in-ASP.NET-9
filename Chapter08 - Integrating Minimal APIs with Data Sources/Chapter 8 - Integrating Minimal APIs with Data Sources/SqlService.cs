using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
namespace Chapter_8___Integrating_Minimal_APIs_with_Data_Sources
{
    public class SqlService : IDatabaseService
    {
        private readonly string _connectionString;
        
        public SqlService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<IEmployee>> GetEmployeesAsync()
        {
            var employees = new List<Employee>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT * FROM Employees", connection))
                {
                    using(var reader = await command.ExecuteReaderAsync())
{
                        while (await reader.ReadAsync())
                        {
                            var employee = new Employee
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Salary = reader.GetDecimal(2),
                                Address = reader.GetString(3),
                                City = reader.GetString(4),
                                Region = reader.GetString(5),
                                PostalCode = reader.GetString(6),
                                Country = reader.GetString(7),
                                Phone = reader.GetString(8)
                            };
                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees;
        }

        public async Task AddEmployeeAsync(IEmployee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(
                    "INSERT INTO Employees (Name, Salary, Address, City, Region, Country, Phone, PostalCode) VALUES (@Name, " +
                                                                "@Salary, @Address, @City, @Region, @Country, @Phone, @PostalCode)", connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@Address", employee.Address);
                    command.Parameters.AddWithValue("@City", employee.City);
                    command.Parameters.AddWithValue("@Region", employee.Region);
                    command.Parameters.AddWithValue("@Country", employee.Country);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("@PostalCode", employee.PostalCode);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
