using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query.Queries;
using System.Linq;

namespace RefactorMe.Core.Query
{
    public class GetProductOptionQueryHandler : IQueryHandler<GetProductOptionQuery, ProductOption>
    {
        private readonly IProductRepository productRepository;

        public GetProductOptionQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ProductOption Handle(GetProductOptionQuery query)
        {
            Product product = productRepository.GetById(query.ProductId, includeOptions: true);
            if (product == null)
            {
                return null;
            }

            return product.Options.SingleOrDefault(x => x.Id == query.Id);
        }
    }
}