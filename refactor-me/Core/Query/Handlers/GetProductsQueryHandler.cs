using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query.Queries;
using System.Collections.Generic;

namespace RefactorMe.Core.Query
{
    public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<Product> Handle(GetProductsQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name))
            {
                return productRepository.GetByName(query.Name);
            }
            else
            {
                return productRepository.GetAll();
            }
        }
    }
}