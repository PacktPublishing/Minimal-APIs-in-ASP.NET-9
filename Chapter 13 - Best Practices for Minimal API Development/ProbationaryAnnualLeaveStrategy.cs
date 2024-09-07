namespace Chapter_9___Entity_Framework_Core_and_Dapper
{
    public class ProbationaryAnnualLeaveStrategy : IAnnualLeaveStrategy
    {
        
        public int CalculateLeaveAllowance(Models.Employee employee)
        {
            var leaveTotal = 10;
            if(employee.Country == "United Kingdom")
            {
                leaveTotal += 3;
            }
            return leaveTotal;
        }
    }
}
