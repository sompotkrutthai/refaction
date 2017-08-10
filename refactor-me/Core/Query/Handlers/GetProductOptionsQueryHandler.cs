using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query.Queries;
using System.Collections.Generic;

namespace RefactorMe.Core.Query
{
    public class GetProductOptionsQueryHandler : IQueryHandler<GetProductOptionsQuery, IEnumerable<ProductOption>>
    {
        private readonly IProductRepository productRepository;

        public GetProductOptionsQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<ProductOption> Handle(GetProductOptionsQuery query)
        {
            Product product = productRepository.GetById(query.ProductId, includeOptions: true);
            return (product != null) ? product.Options : new List<ProductOption>();
        }
    }
}