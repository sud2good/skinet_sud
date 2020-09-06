using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypeAndBrandSpecification : BaseSpecification<Product>
    {
        // public ProductsWithTypeAndBrandSpecification(string sort,int? brandId, int? typeId ) 
        // : base(x =>
        //         (!brandId.HasValue || x.ProductBrandId == brandId) &&
        //         (!typeId.HasValue || x.ProductTypeId == typeId)
        //     )
        // {
        //     AddInclude(x => x.ProductType);
        //     AddInclude(x => x.ProductBrand);
        //     //AddOrderBy(x =>x.Name);

        //     if(!String.IsNullOrEmpty(sort))
        //     {
        //         switch(sort)
        //         {
        public ProductsWithTypeAndBrandSpecification(ProductSpecParams productParams) 
        : base(x =>
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) &&
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains
                (productParams.Search))
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x =>x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex-1), productParams.PageSize);

            if(!String.IsNullOrEmpty(productParams.Sort))
            {
                switch(productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p=>p.Price);
                        break;
                    default:
                        AddOrderBy(x=>x.Name);
                        break;
                }
            }
           
        }

        public ProductsWithTypeAndBrandSpecification(int id) : base(x => x.Id == id)
        {
                        AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}