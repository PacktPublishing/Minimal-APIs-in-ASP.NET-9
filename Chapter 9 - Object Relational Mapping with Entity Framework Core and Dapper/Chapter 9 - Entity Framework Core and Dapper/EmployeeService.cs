using Chapter_9___Entity_Framework_Core_and_Dapper.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Chapter_9___Entity_Framework_Core_and_Dapper
{
    public class EmployeeService
    {
        private MyCompanyContext _companyContext;
        public EmployeeService(MyCompanyContext myCompanyContext)
        {
            _companyContext = myCompanyContext;
        }
        public async Task AddEmployee(Models.Employee employee)
        {
            await _companyContext.Employees.AddAsync(employee);
            await _companyContext.SaveChangesAsync();
        }
        public async Task<Models.Employee> GetEmployeeById(int id)
        {
            var result = await _companyContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new EmployeeNotFoundException(id);
            }
            return result;
        }
        public async Task UpdateEmployee(Models.Employee employee)
        {
            var employeeToUpdate = await GetEmployeeById(employee.Id);
            _companyContext.Employees.Update(employeeToUpdate);
            await _companyContext.SaveChangesAsync();
        }
        public async Task DeleteEmployee(Employee employee)
        {
            _companyContext.Remove(employee);
            await _companyContext.SaveChangesAsync();
        }
    }
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(int id) : base($"Employee with id {id} could not be found") { }
    }
}
