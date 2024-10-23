namespace Chapter_8___Integrating_Minimal_APIs_with_Data_Sources
{
    public interface IEmployee
    {
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
