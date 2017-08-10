using RefactorMe.Core.DomainModels;
using RefactorMe.Core.External.Repositories;
using RefactorMe.Core.Query.Queries;

namespace RefactorMe.Core.Query
{
    public class GetProductQueryHandler : IQueryHandler<GetProductQuery, Product>
    {
        private readonly IProductRepository productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Product Handle(GetProductQuery query)
        {
            return productRepository.GetById(query.Id);
        }
    }
}