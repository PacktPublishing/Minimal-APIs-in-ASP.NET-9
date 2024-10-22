namespace Chapter_9___Entity_Framework_Core_and_Dapper
{
    public interface IAnnualLeaveStrategy
    {
        int CalculateLeaveAllowance(Models.Employee employee);
    }
}
