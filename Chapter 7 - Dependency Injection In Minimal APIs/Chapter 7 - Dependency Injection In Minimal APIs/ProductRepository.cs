namespace Chapter_7___Dependency_Injection_In_Minimal_APIs
{
    public class ProductRepository
    {
        public List<Product> Products { get; private set; }
        public ProductRepository(ProductRetrievalService productRetrievalService)
        {
            Products = productRetrievalService.LoadProducts();
        }
    }
}
