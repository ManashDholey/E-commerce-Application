using System.Reflection;
using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure.Data;

namespace Infrastructure.Data
{
    public class StoreContextSheed
    {
        
        public static async Task SeedAsync(StoreContext context){
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if(!context.ProductBrands.Any()){
                var brandData=File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands= JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
               await context.ProductBrands.AddRangeAsync(brands); 
            }  
             if(!context.ProductTypes.Any()){
                var TypeData=File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var Types= JsonSerializer.Deserialize<List<ProductType>>(TypeData);
               await context.ProductTypes.AddRangeAsync(Types); 
            }  
            if(!context.Products.Any()){
                var productsData=File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products= JsonSerializer.Deserialize<List<Product>>(productsData);
               await context.Products.AddRangeAsync(products); 
            } 
            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                context.DeliveryMethods.AddRange(methods);
            }
            if(context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}