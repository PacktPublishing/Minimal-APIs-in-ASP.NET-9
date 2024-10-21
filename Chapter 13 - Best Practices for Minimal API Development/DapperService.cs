using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Net;
using System.Numerics;

namespace Chapter_9___Entity_Framework_Core_and_Dapper
{
    public class DapperService
    {
        public async Task AddEmployee(Employee employee)
        {
            using (var sqlConnection = new SqlConnection("YOURCONNECTIONSTRING"))
            {
                var sql = "INSERT INTO Employees (Name, Salary, Address, City, Region, Country, Phone) VALUES(@Name, @Salary, @Address, @City, @Region, @Country, @Phone, @PostalCode)";
                
                await sqlConnection.ExecuteAsync(sql, new
                {
                    employee.Name,
                    employee.Salary,
                    employee.Address,
                    employee.City,
                    employee.Region,
                    employee.Country,
                    employee.Phone,
                    employee.PostalCode
                });
            }
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            using (var sqlConnection = new SqlConnection("YOURCONNECTIONSTRING"))
            {
                var sql = "SELECT * FROM Employees WHERE Id = @employeeId";
                var result = await sqlConnection.QuerySingleAsync<Employee>(sql, new { employeeId = id });
                return result;
            }
        }

        public async Task UpdateEmployee(Employee employee)
        {
            using (var sqlConnection = new SqlConnection("YOURCONNECTIONSTRING"))
            {
                var sql = "UPDATE Employees SET Name = @Name, Salary = @Salary, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode WHERE Id = @id";
                var parameters = new
                {
                    employee.Id,
                    employee.Name,
                    employee.Salary,
                    employee.Address,
                    employee.City,
                    employee.Region,
                    employee.PostalCode
                };
                await sqlConnection.ExecuteAsync(sql, parameters);
            }
        }
        public async Task DeleteEmployeeById(int id)
        {
            using (var sqlConnection = new SqlConnection("YOURCONNECTIONSTRING"))
            {
                var sql = "DELETE FROM Employees WHERE Id = @employeeId";
                await sqlConnection.ExecuteAsync(sql, new { employeeId = id });
            }
        }
    }
}
