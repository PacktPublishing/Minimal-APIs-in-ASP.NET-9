namespace Chapter_7___Dependency_Injection_In_Minimal_APIs
{
    public class Order
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }
        public float DiscountAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
