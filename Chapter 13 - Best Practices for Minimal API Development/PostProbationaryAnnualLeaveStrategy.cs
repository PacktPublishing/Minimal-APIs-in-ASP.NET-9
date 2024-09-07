namespace Chapter_9___Entity_Framework_Core_and_Dapper
{
    public class PostProbationaryAnnualLeaveStrategy : IAnnualLeaveStrategy
    {
        public int CalculateLeaveAllowance(Models.Employee employee)
        {
            var leaveTotal = 16;
            if(employee.Country == "United Kingdom")
            {
                leaveTotal += 3;
            }
            if(employee.YearsOfService >= 1)
            {
                for (int i = 0; i <= employee.YearsOfService; i++)
                {
                    leaveTotal += 1;
                }
            }
            return leaveTotal;
        }
    }
}
