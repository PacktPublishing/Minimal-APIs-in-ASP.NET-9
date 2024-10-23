namespace Chapter_2___Creating_your_first_Minimal_API
{
    public static class EmployeeManager
    {
        private static List<Employee> _employees = new List<Employee>();

        public static void Create(Employee employee)
        {
            _employees.Add(employee);
        }
        public static void Update(Employee employee)
        {
            _employees[_getEmployeeIndex(employee.Id)] = employee;
        }

        public static void ChangeName(int id, string name)
        {
            _employees[_getEmployeeIndex(id)].Name = name;
        }
        public static void Delete(int id)
        {
            _employees.RemoveAt(_getEmployeeIndex(id));
        }
        public static Employee Get(int id)
        {
            var employee = _employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                throw new ArgumentException("Id invalid");
            }
            return employee;
        }

        private static int _getEmployeeIndex(int id)
        {
            var employeeIndex = _employees.FindIndex(x => x.Id == id);
            if (employeeIndex == -1)
            {
                throw new ArgumentException($"Employee with Id {id} does not exist");
            }
            return employeeIndex;
        }
    }
}
