using System.Text.Json;
namespace Chapter_7___Dependency_Injection_In_Minimal_APIs
{
    public class ProductRetrievalService
    {
        private const string _dataPath = @"C:/Products.json";
        public List<Product> LoadProducts()
        {
            var productJson = File.ReadAllText(_dataPath);
            return JsonSerializer.Deserialize<List<Product>>(productJson);
        }
    }
}
