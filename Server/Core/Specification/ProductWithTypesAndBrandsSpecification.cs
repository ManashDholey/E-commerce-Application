using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification
{
    public class ProductWithTypesAndBrandsSpecification:Specification<Product>
    {
        public ProductWithTypesAndBrandsSpecification(string sort)
        {
             AddInclude(e=> e.ProductBrand);
             AddInclude(e=> e.ProductType);
             AddOrderBy(x=> x.Name);
             if(!string.IsNullOrEmpty(sort)){
                switch(sort){
                    case "priceAsc":
                       AddOrderBy(p => p.Price);
                       break;
                    case "priceDesc":
                       AddOrderByDescending(p=> p.Price);
                       break;
                    default:
                      AddOrderBy(m=> m.Name);
                      break;       
                }
             }
        }

        public ProductWithTypesAndBrandsSpecification(int id) : base(x=> x.Id == id )
        {    AddInclude(e=> e.ProductBrand);
             AddInclude(e=> e.ProductType);
        }

        // public ProductWithTypesAndBrandsSpecification(ProductSpecParams productParams) : base(x =>
        //     (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
        //     (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
        //     (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        // {
        // }
    }
}