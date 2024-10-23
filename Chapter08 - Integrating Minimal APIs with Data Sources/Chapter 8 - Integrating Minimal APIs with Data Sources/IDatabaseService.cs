namespace Chapter_8___Integrating_Minimal_APIs_with_Data_Sources
{
    public interface IDatabaseService
    {
        Task<IEnumerable<IEmployee>> GetEmployeesAsync();
        Task AddEmployeeAsync(IEmployee employee);
    }
}
