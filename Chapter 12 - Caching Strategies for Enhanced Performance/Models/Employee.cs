using Microsoft.EntityFrameworkCore;

namespace Chapter_9___Entity_Framework_Core_and_Dapper.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
